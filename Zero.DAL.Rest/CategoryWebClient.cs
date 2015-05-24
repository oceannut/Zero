using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Nega.Common;
using Nega.Data;

using Zero.Domain;
using Zero.DAL;
using System.Runtime.Serialization.Json;
using System.IO;

namespace Zero.DAL.Rest
{

    public class CategoryWebClient : GenericDao<Category, string>, ICategoryDao
    {

        private string url;

        public CategoryWebClient(string url)
        {
            this.url = url;
        }

        public override int Save(Category entity)
        {
            int count = 0;

            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            client.UploadStringCompleted +=
                (o, e) =>
                {
                    if (e.Error == null)
                    {
                        count = 1;
                    }
                    else
                    {

                    }
                };
            string serviceUrl = string.Format("{0}/CategoryRestService.svc/category/", url);
            entity.Creation = DateTime.Now;
            entity.Modification = DateTime.Now;

            string s = null;
            DataContractJsonSerializer json = new DataContractJsonSerializer(entity.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                json.WriteObject(stream, entity);
                s = Encoding.UTF8.GetString(stream.ToArray());
            }

            client.UploadStringAsync(new Uri(serviceUrl), s);

            return count;
        }


        public Category Get(int scope, string id)
        {
            throw new NotImplementedException();
        }

        public Category GetByCode(int scope, string code, bool? includeParent = null)
        {
            throw new NotImplementedException();
        }

        public bool IsCodeExisted(int scope, string code)
        {
            throw new NotImplementedException();
        }

        public int Count(int? scope = null, string parentId = null, bool? isDisused = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category> List(int? scope = null, string parentId = null, bool? isDisused = null)
        {
            throw new NotImplementedException();
        }

        public TreeNodeCollection<Category> Tree(int scope)
        {
            throw new NotImplementedException();
        }
    }

}
