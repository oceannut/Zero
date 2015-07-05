using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Zero.Client.Common
{

    public static class WebClientBuilder
    {

        private static Encoding defautWebClientEncoding = Encoding.UTF8;

        public static WebClient Build(Encoding encoding = null)
        {
            WebClient client = new WebClient();
            if (encoding == null)
            {
                client.Encoding = defautWebClientEncoding;
            }
            else
            {
                client.Encoding = encoding;
            }

            return client;
        }

        public static void SupportJson(this WebClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException();
            }

            client.Headers[HttpRequestHeader.ContentType] = "application/json";
        }

        public static void SupportAuthorization(this WebClient client, string credentials)
        {
            if (client == null || string.IsNullOrWhiteSpace(credentials))
            {
                throw new ArgumentNullException();
            }

            client.Headers[HttpRequestHeader.Authorization] = credentials;
        }

    }

}
