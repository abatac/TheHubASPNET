using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.Swagger.Annotations;
using TheHub.Data;
using TheHub.Models;

namespace TheHub.Controllers
{
    public class AuthenticationController : ApiController
    {
        /// <summary>
        ///     Generates token to be used in accessing web apis.
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <response code="200">User is successfully authorized</response>
        /// <response code="400">Request data is invalid</response>
        /// <response code="401">User is not authorized</response>
        [HttpPost]
        [Route("token")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TokenResponse))]
        public async Task<IHttpActionResult> Token([FromBody] LoginViewModel loginViewModel)
        {
            var userManager = new UserManager();

            if (!ModelState.IsValid) return BadRequest(ModelState);


            //Find user by Username
            var userToVerify = await userManager.FindByNameAsync(loginViewModel.Username);
            if (userToVerify == null) return Unauthorized();
            //If User is found, let's verify Username/Password Pair
            var isValid = await userManager.CheckPasswordAsync(userToVerify, loginViewModel.Password);

            if (isValid)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userToVerify.UserName)
                };

                var signingKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["JwtSigningKey"]));
                var expiryInMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["JwtExpiryInMinutes"]);

                var token = new JwtSecurityToken(
                    ConfigurationManager.AppSettings["JwtSite"],
                    ConfigurationManager.AppSettings["JwtSite"],
                    expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                    claims: claims
                );

                return Ok(new TokenResponse(new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo));
            }

            return Unauthorized();
        }

        /**
        [HttpPost]
        [Route("register")]
        public async Task<IHttpActionResult> Register([FromBody]RegisterViewModel model)
        {

            var userManager = new UserManager();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userIdentity = new ApplicationUser()
                {
                    UserName = model.Username
                };
                var result = await userManager.CreateAsync(userIdentity, model.Password);
                if (!result.Succeeded) return BadRequest("Error creating account");
                return Ok("Account created");
            }
            catch (Exception ex)
            {
                return BadRequest("Error creating account");
            }
        }
    **/
    }
}