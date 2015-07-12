using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Nega.Common;
using Nega.WcfCommon;

namespace Zero.Client.Common
{

    public class WebSignClient : ISignClient
    {

        private const string service = "SignRestService.svc/sign";
        private readonly string url;

        public WebSignClient(string url)
        {
            this.url = url;
        }

        public string Signin(string username, string pwd)
        {
            using (WebClient client = WebClientBuilder.Build())
            {
                client.SupportJson();
            
                string serviceUrl = string.Format("{0}/{1}/{2}/", url, service, username);
                JsonStringBuilder data = new JsonStringBuilder();
                data.AppendLeftBrace();
                data.Append("pwd", pwd);
                data.AppendRightBrace();
                string result = client.UploadString(new Uri(serviceUrl), "PUT", data.ToString());

                return JsonHelper.Deserialize<string>(result);
            }
        }

        public Task<string> SigninAsync(string username, string pwd)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    return Signin(username, pwd);
                });
        }

    }

}
