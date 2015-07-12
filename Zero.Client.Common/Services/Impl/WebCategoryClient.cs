using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Nega.Common;
using Nega.WcfCommon;

using Zero.Domain;
using System.Collections.Specialized;

namespace Zero.Client.Common
{

    public class WebCategoryClient : ICategoryClient
    {

        private const string service = "CategoryRestService.svc/category";
        private readonly string url;

        public WebCategoryClient(string url)
        {
            this.url = url;
        }

        public Category SaveCategory(int scope, string name, string desc, string parentId)
        {
            using (WebClient client = WebClientBuilder.Build())
            {
                client.SupportJson();
                client.SupportAuthorization(ClientContext.Current.UserToken);

                string serviceUrl = string.Format("{0}/{1}/{2}/", url, service, scope);
                JsonStringBuilder data = new JsonStringBuilder();
                data.AppendLeftBrace();
                data.Append("name", name).AppendComma();
                data.Append("desc", desc).AppendComma();
                data.Append("parentId", parentId);
                data.AppendRightBrace();
                string result = client.UploadString(new Uri(serviceUrl), "POST", data.ToString());

                return JsonHelper.Deserialize<Category>(result);
            }
        }

        public Task<Category> SaveCategoryAsync(int scope, string name, string desc, string parentId)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    return SaveCategory(scope, name, desc, parentId);
                });
        }

        public Category UpdateCategory(int scope, string id, string name, string desc)
        {
            using (WebClient client = WebClientBuilder.Build())
            {
                client.SupportJson();
                client.SupportAuthorization(ClientContext.Current.UserToken);

                string serviceUrl = string.Format("{0}/{1}/{2}/{3}/", url, service, scope, id);
                JsonStringBuilder data = new JsonStringBuilder();
                data.AppendLeftBrace();
                data.Append("name", name).AppendComma();
                data.Append("desc", desc);
                data.AppendRightBrace();
                string result = client.UploadString(new Uri(serviceUrl), "PUT", data.ToString());

                return JsonHelper.Deserialize<Category>(result);
            }
        }

        public Task<Category> UpdateCategoryAsync(int scope, string id, string name, string desc)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    return UpdateCategory(scope, id, name, desc);
                });
        }

        public void DeleteCategory(string scope, string id)
        {
            using (WebClient client = WebClientBuilder.Build())
            {
                client.SupportAuthorization(ClientContext.Current.UserToken);

                string serviceUrl = string.Format("{0}/{1}/{2}/{3}/", url, service, scope, id);
                client.UploadString(new Uri(serviceUrl), "DELETE", "");
            }
        }

        public Task DeleteCategoryAsync(string scope, string id)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    DeleteCategory(scope, id);
                });
        }

        public IEnumerable<Category> ListCategory(int scope)
        {
            using (WebClient client = WebClientBuilder.Build())
            {
                client.SupportAuthorization(ClientContext.Current.UserToken);

                Category[] categories = null;

                string serviceUrl = string.Format("{0}/{1}/{2}/", url, service, scope);

                string s = client.DownloadString(new Uri(serviceUrl));
                categories = JsonHelper.Deserialize<Category[]>(s);

                return categories;
            }
        }

        public Task<IEnumerable<Category>> ListCategoryAsync(int scope)
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
