using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nega.Common;
using Nega.Modularity;

using Zero.Domain;
using Zero.DAL;

namespace Zero.DAL.Caching
{

    public class KeyValCache : IKeyValDao, IModule
    {

        private IKeyValDao dao;
        private ICache cache;

        public string Name
        {
            get { return "KeyVal Cache"; }
        }

        public IModuleLicence Licence
        {
            get { return Unlimited.Solo; }
        }

        public KeyValCache(IKeyValDao dao, CacheManager cacheManager)
        {
            if (dao == null || cacheManager == null)
            {
                throw new ArgumentNullException();
            }

            this.dao = dao;
            this.cache = cacheManager.GetCache(Name) as ICache;
        }

        public KeyVal Get(string key)
        {
            return this.cache.Get(key) as KeyVal;
        }

        public IEnumerable<KeyVal> List()
        {
            List<KeyVal> list = new List<KeyVal>();
            var items = this.cache.Items;
            foreach (var item in items)
            {
                list.Add(item.Value as KeyVal);
            }

            return list;
        }

        public void Initialize()
        {
            this.cache.Clear();
            IEnumerable<KeyVal> keyVals = this.dao.List();
            if (keyVals != null && keyVals.Count() > 0)
            {
                foreach (var keyVal in keyVals)
                {
                    this.cache.Add(keyVal.Key, keyVal);
                }
            }
        }

        public void Destroy()
        {
            this.cache.Dispose();
        }

        public void Dispose()
        {
            Destroy();
        }

    }

}
