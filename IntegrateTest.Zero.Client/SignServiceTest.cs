using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace IntegrateTest.Zero.Client
{
    public class SignServiceTest
    {

        public void TestIsUsernameExist()
        {
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");

            //client.DownloadDataTaskAsync("http://localhost:49938/SignService.svc/sign/zsp/")
            //    .ContinueWith(
            //        (task) =>
            //        {
            //            if (task.Exception == null)
            //            {
            //                string result = Encoding.GetEncoding("UTF-8").GetString(task.Result);
            //                Console.WriteLine("result is:");
            //                Console.WriteLine(result);
            //            }
            //            else
            //            {
            //                Console.WriteLine(task.Exception);
            //            }
            //        });

            client.DownloadStringTaskAsync("http://localhost:49938/SignService.svc/sign/zsp/")
                .ContinueWith(
                    (task) =>
                    {
                        if (task.Exception == null)
                        {
                            Console.WriteLine("result is:");
                            Console.WriteLine(task.Result);
                        }
                        else
                        {
                            Console.WriteLine(task.Exception);
                        }
                    });
        }

        public void TestSignup()
        {
            StringBuilder json = new StringBuilder();
            json.Append("{");
            json.Append("\"pwd\":\"testPwd\",");
            json.Append("\"name\":\"testName\",");
            json.Append("\"email\":\"test@test.com\"");
            json.Append("}");
            byte[] requestData = Encoding.GetEncoding("UTF-8").GetBytes(json.ToString());

            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");
            client.Headers.Add("ContentLength", requestData.Length.ToString());

            client.UploadDataTaskAsync("http://localhost:49938/SignService.svc/sign/zsp123456,,,1/", "POST", requestData)
                .ContinueWith(
                    (task) =>
                    {
                        if (task.Exception == null)
                        {
                            string result = Encoding.GetEncoding("UTF-8").GetString(task.Result);
                            Console.WriteLine("result is:");
                            Console.WriteLine(result);
                        }
                        else
                        {
                            Console.WriteLine((task.Exception.InnerException as WebException).Message);
                            Console.WriteLine("-------------------------");
                            Console.WriteLine(task.Exception);
                        }
                    });
        }

    }
}
