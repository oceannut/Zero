using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zero.Domain;
using Zero.BLL;
using Zero.DAL;

namespace Zero.BLL.Impl
{

    public class KeyValServiceImpl : IKeyValService
    {

        private IKeyValDao keyValDao;

        public KeyValServiceImpl(IKeyValDao keyValDao)
        {
            this.keyValDao = keyValDao;
        }

        public KeyVal GetKeyVal(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(key);
            }
            return keyValDao.Get(key);
        }

        public IEnumerable<KeyVal> ListKeyVal()
        {
            return keyValDao.List();
        }

    }

}
