using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nega.Common;
using Nega.Data;

using Zero.Domain;
using Zero.BLL;
using Zero.DAL;

namespace Zero.BLL.Impl
{

    public class ResourceAccessServiceImpl : IResourceAccessService
    {

        public ResourceAccessServiceImpl()
        {

        }

        public IEnumerable<ResourceAccessData> ListResourceAccessData(string name, string method)
        {
            return new List<ResourceAccessData>();
        }

        public Task<IEnumerable<ResourceAccessData>> ListResourceAccessDataAsync(string name, string method)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    return ListResourceAccessData(name, method);
                });
        }

        public IEnumerable<ResourceAccess> ListResourceAccess(string name, string method)
        {
            IEnumerable<ResourceAccessData> datas = ListResourceAccessData(name, method);
            if (datas != null && datas.Count() > 0)
            {
                return (from item in datas select item.Access);
            }
            else
            {
                return null;
            }
        }

    }

}
