using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nega.Data;

using Zero.Domain;

namespace Zero.DAL
{
    public interface IRoleDao : IDao<Role>
    {

        IList<Role> List(string userId);

    }
}
