using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zero.Domain;
using Zero.DAL;

namespace Zero.DAL.EF
{
    public class KeyValDao : IKeyValDao
    {

        private string connectionString;

        public KeyValDao(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public KeyVal Get(string key)
        {
            using (KeyValDataContext context = new KeyValDataContext(connectionString))
            {
                return context.KeyVals.Find(key);
            }
        }

        public IEnumerable<KeyVal> List()
        {
            using (KeyValDataContext context = new KeyValDataContext(connectionString))
            {
                return (from keyVal in context.KeyVals
                        select keyVal)
                        .ToArray();
            }
        }
    }
}
