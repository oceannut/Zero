using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nega.Common;
using Nega.Data;

namespace Zero.DAL
{

    public interface IAuditEntryDao
    {

        Paging<AuditEntry> List(PagingRequest request, 
            string resourceName, int? resourceMethod, string keywords);

    }

}
