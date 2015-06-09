using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Nega.Common;
using Nega.WcfCommon;

using Zero.Domain;

namespace Zero.Client.Common
{

    public class WebCategoryClient : ICategoryClient
    {

        private readonly string url;

        public WebCategoryClient(string url)
        {
            this.url = url;
        }

        public Category SaveCategory(int scope, string name, string desc)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";

                    string serviceUrl = string.Format("{0}/CategoryRestService.svc/category/{1}/", url, scope);
                    string data = JsonHelper.Serialize<Category>(
                        new Category
                        {
                            Name = name,
                            Desc = desc
                        });
                    string result = client.UploadString(new Uri(serviceUrl), "POST", data);

                    return JsonHelper.Deserialize<Category>(result);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<Category> SaveCategoryAsync(int scope, string name, string desc)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category> ListCategory(int? scope)
        {
            using (WebClient client = new WebClient())
            {

                client.Encoding = Encoding.UTF8;
                client.Headers.Add(HttpRequestHeader.Authorization, "123");

                Category[] categories = null;

                string serviceUrl = string.Format("{0}/CategoryRestService.svc/category/{1}/", url, scope.HasValue ? scope.Value : 0);

                string s = client.DownloadString(new Uri(serviceUrl));
                categories = JsonHelper.Deserialize<Category[]>(s);

                return categories;
            }
        }

        public Task<IEnumerable<Category>> ListCategoryAsync(int? scope)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    return ListCategory(scope);
                });
        }


        public TreeNodeCollection<Category> Tree(int scope)
        {
            var categories = ListCategory(scope);
            return Category.BuildTree(categories);
        }

    }

}
