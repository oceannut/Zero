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
    public interface ICategoryRestService
    {

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/category/{scope}/",
            BodyStyle= WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Category SaveCategory(string scope, string name, string description);


        //[OperationContract]
        //[WebInvoke(Method = "POST",
        //    UriTemplate = "/category/",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json)]
        //Category SaveCategory(Category category);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            UriTemplate = "/category/",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Category UpdateCategory(Category category);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/category/{id}/",
            RequestFormat = WebMessageFormat.Json)]
        void DeleteCategory(string id);

        //[OperationContract]
        //[WebGet(UriTemplate = "/category/{id}/",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json)]
        //Category GetCategory(string id);

        [OperationContract]
        [WebGet(UriTemplate = "/category/{scope}/code/{code}/",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Category GetCategoryByCode(string scope, string code);

        [OperationContract]
        [WebGet(UriTemplate = "/category/{scope}/",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Category[] ListCategories(string scope);

    }

}
