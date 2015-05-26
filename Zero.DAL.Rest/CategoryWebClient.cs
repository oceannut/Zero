using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Nega.Common;
using Nega.Data;
using Nega.WcfCommon;

using Zero.Domain;
using Zero.DAL;

namespace Zero.DAL.Rest
{

    public class CategoryWebClient : GenericDao<Category, string>, ICategoryDao
    {

        private readonly string url;

        public CategoryWebClient(string url)
        {
            this.url = url;
        }

        public override int Save(Category entity)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";

                    string serviceUrl = string.Format("{0}/CategoryRestService.svc/category/", url);
                    string s = JsonHelper.Serialize<Category>(entity);
                    client.UploadString(new Uri(serviceUrl), "POST", s);

                    return 1;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public override int Update(Category entity)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                string serviceUrl = string.Format("{0}/CategoryRestService.svc/category/", url);
                string s = JsonHelper.Serialize<Category>(entity);

                client.UploadString(new Uri(serviceUrl), "PUT", s);

                return 1;
            }
        }

        public override int Update(IEnumerable<Category> col)
        {
            return base.Update(col);
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
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                Category[] categories = null;

                string serviceUrl = string.Format("{0}/CategoryRestService.svc/category/{1}/", url, scope.HasValue ? scope.Value : 0);

                string s = client.DownloadString(new Uri(serviceUrl));
                categories = JsonHelper.Deserialize<Category[]>(s);

                return categories;
            }
        }

        public TreeNodeCollection<Category> Tree(int scope)
        {
            var categories = List(scope);
            return Category.BuildTree(categories);
        }
    }

}
