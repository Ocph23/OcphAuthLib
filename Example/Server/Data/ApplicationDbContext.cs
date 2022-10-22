using Microsoft.EntityFrameworkCore;

namespace Example.Server.Data
{
    public class ApplicationDbContext : OcphAuthServer.OcphAuthContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }


    }
}
