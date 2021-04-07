using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheHub.Models
{
    public class TokenResponse
    {

        public TokenResponse(string token, DateTime expiration)
        {
            Token = token;
            Expiration = expiration;
        }

        [JsonProperty("token")] public string Token { get; set; }
        [JsonProperty("expiration")] public DateTime Expiration { get; set; }
    }
}