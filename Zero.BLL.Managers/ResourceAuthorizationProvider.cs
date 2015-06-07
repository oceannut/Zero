using Nega.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.BLL
{
    public class ResourceAuthorizationProvider : IResourceAuthorizationProvider
    {
        public IEnumerable<ResourceAccess> ListResourceAccess(string name, string method)
        {
            return null;
        }
    }
}
