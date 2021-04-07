using AspNet.Identity.SQLite;

namespace TheHub.Data
{
    public class ApplicationDbContext : SQLiteDatabase
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
        }
    }
}