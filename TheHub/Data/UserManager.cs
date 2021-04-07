using AspNet.Identity.SQLite;
using Microsoft.AspNet.Identity;
using TheHub.Models;

namespace TheHub.Data
{
    public class UserManager : UserManager<ApplicationUser>
    {
        public UserManager() : base(new UserStore<ApplicationUser, IdentityRole>(new ApplicationDbContext()))
        {
        }
    }
}