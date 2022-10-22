using Microsoft.EntityFrameworkCore;
using OcphAuthServer.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcphAuthServer
{
    public class OcphAuthContext :DbContext
    {
        private readonly DbContextOptions options;

        public OcphAuthContext(DbContextOptions options) : base(options) { }

        public DbSet<User>  Users { get; set; }
        public DbSet<Role>  Roles{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email).IsUnique();

            modelBuilder.Entity<User>()
                .Navigation(x => x.UserRoles)
                .AutoInclude();

            modelBuilder.Entity<UserRole>()
                .Navigation(x => x.Role)
                .AutoInclude();

            base.OnModelCreating(modelBuilder);
        }

    }

}
