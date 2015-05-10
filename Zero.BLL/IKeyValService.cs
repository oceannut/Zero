using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zero.Domain;

namespace Zero.BLL
{

    public interface IKeyValService
    {

        KeyVal GetKeyVal(string key);

        IEnumerable<KeyVal> ListKeyVal();

    }

}
