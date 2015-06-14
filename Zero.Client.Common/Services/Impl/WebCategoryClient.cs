﻿using System;
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

        public Category SaveCategory(int scope, string name, string desc, string parentId)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.ContentType] = "application/json";

                string serviceUrl = string.Format("{0}/CategoryRestService.svc/category/{1}/", url, scope);
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

        public IEnumerable<Category> ListCategory(int scope)
        {
            using (WebClient client = new WebClient())
            {

                client.Encoding = Encoding.UTF8;
                client.Headers.Add(HttpRequestHeader.Authorization, "123");

                Category[] categories = null;

                string serviceUrl = string.Format("{0}/CategoryRestService.svc/category/{1}/", url, scope);

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
