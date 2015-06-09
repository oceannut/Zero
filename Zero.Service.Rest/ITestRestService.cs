using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Zero.Service.Rest
{

    [ServiceContract]
    public interface ITestRestService
    {

        [OperationContract]
        [WebGet(UriTemplate = "/test/path/",
            RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json)]
        string GetServicePath();

    }

}
