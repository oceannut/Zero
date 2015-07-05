using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nega.Common;

namespace Zero.DAL.EF
{

    public class AuditEntryDataContext : DbContext
    {

        public AuditEntryDataContext(string connectionString)
            : base(connectionString)
        {

        }

        public DbSet<AuditEntry> AuditEntries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var config = modelBuilder.Entity<AuditEntry>();
            config.Property(t => t.Resource.Name).HasColumnName("ResourceName");
            config.Property(t => t.Resource.Method).HasColumnName("ResourceMethod");

        }

    }

}
