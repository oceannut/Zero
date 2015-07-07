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
        public IEnumerable<ResourceAccess> ListResourceAccesses(string name, int method)
        {
            return null;
        }
    }
}
