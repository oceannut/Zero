using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;

using Zero.Domain;

namespace Zero.DAL.EF
{

    public class UserDataContext : DbContext
    {

        public UserDataContext(string connectionString)
            : base(connectionString)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Roles)
                .WithMany(e => e.Users)
                .Map(m =>
                {
                    m.ToTable("userroles");
                    m.MapLeftKey("userId");
                    m.MapRightKey("roleId");
                });
        }

    }

}
