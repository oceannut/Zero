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
    public interface ICategoryService
    {

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "/category/{scope}/0/",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Category SaveCategory(string scope, string parentId, string name, string code, string order);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "/category/{scope}/{id}/name/{name}/",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Category UpdateCategoryName(string scope, string id, string name);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "/category/{scope}/{id}/order/{order}/",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Category UpdateCategoryOrder(string scope, string id, string order);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "/category/{scope}/{id}/parent/{parentId}/",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Category UpdateCategoryParent(string scope, string id, string parentId);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "/category/{scope}/{id}/disused/{disused}/",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Category UpdateCategoryDisused(string scope, string id, string disused);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/category/{scope}/{id}/",
            RequestFormat = WebMessageFormat.Json)]
        void DeleteCategory(string scope, string id);

        [OperationContract]
        [WebGet(UriTemplate = "/category/{scope}/{id}/",
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Category GetCategory(string scope, string id);

        [OperationContract]
        [WebGet(UriTemplate = "/category/{scope}/code/{code}/",
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Category GetCategoryByCode(string scope, string code);

    }

}
