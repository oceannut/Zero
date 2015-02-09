using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;

using Zero.Domain;

namespace Zero.DAL.EF
{
    public class CategoryDataContext : DbContext
    {

        public CategoryDataContext(string connectionString)
            : base(connectionString)
        {

        }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
                .HasOptional(e => e.Parent)
                .WithMany()
                .HasForeignKey(e => e.ParentId)
                .WillCascadeOnDelete(false);

        }

    }
}
