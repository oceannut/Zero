using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;

using Zero.Domain;

namespace Zero.DAL.EF
{
    public class KeyValDataContext : DbContext
    {

        public KeyValDataContext(string connectionString)
            : base(connectionString)
        {

        }

        public DbSet<KeyVal> KeyVals { get; set; }

    }
}
