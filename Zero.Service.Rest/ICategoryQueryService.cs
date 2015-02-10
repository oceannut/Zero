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
    public interface ICategoryQueryService
    {

        [OperationContract]
        [WebGet(UriTemplate = "/category/{scope}/",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Category[] GetCategories(string scope);

    }

}
