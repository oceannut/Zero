using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zero.Domain;

namespace Zero.DAL
{

    public interface IKeyValDao
    {

        KeyVal Get(string key);

        IEnumerable<KeyVal> List();

    }

}
