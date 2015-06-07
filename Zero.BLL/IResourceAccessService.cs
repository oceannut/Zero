using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zero.Domain;

namespace Zero.BLL
{

    public interface IResourceAccessService
    {

        IEnumerable<ResourceAccessData> ListResourceAccessData(string name, string method);

        Task<IEnumerable<ResourceAccessData>> ListResourceAccessDataAsync(string name, string method);

    }

}
