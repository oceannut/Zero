using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nega.Common;
using Nega.Data;

namespace Zero.DAL.EF
{
    public class AuditEntryDao : IAuditWriter, IAuditEntryDao
    {

        private string connectionString;

        public AuditEntryDao(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Write(AuditEntry entry)
        {
            entry.Id = Guid.NewGuid().ToString();
            using (AuditEntryDataContext context = new AuditEntryDataContext(connectionString))
            {
                context.AuditEntries.Add(entry);
                context.SaveChanges();
            }
        }

        public void Write(IEnumerable<AuditEntry> entries)
        {
            foreach (var item in entries)
            {
                item.Id = Guid.NewGuid().ToString();
            }
            using (AuditEntryDataContext context = new AuditEntryDataContext(connectionString))
            {
                context.AuditEntries.AddRange(entries);
                context.SaveChanges();
            }
        }

        public Paging<AuditEntry> List(PagingRequest request, string resourceName, int? resourceMethod, string keywords)
        {
            throw new NotImplementedException();
        }

    }
}
