using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using Zero.Domain;

namespace Zero.Service.Rest
{

    [ServiceContract]
    public interface ISignRestService
    {

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "/sign/{username}/",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        User Signup(string username, string pwd, string name, string email);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "/sign/{username}/",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        bool Signin(string username, string pwd);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/sign/{username}/",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        bool Signout(string username, string pwd);

        [OperationContract]
        [WebGet(UriTemplate = "/sign/{username}/",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        bool IsUsernameExist(string username);

    }

}
