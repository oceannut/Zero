using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using Nega.Common;
using Nega.WcfCommon;

using Zero.Domain;
using Zero.BLL;

namespace Zero.Service.Rest
{
    
    public class SignRestServiceImpl : ISignRestService
    {

        private readonly IAuthenticationProvider authenticationProvider;
        private readonly ICredentialsProvider credentialsProvider;
        private readonly IClientManager clientManager;
        private readonly IUserService userService;

        public SignRestServiceImpl(IAuthenticationProvider authenticationProvider, 
            ICredentialsProvider credentialsProvider, 
            IClientManager clientManager,
            IUserService userService)
        {
            this.authenticationProvider = authenticationProvider;
            this.credentialsProvider = credentialsProvider;
            this.clientManager = clientManager;
            this.userService = userService;
        }

        public string Signup(string username, string pwd, string name, string email,
            bool autoSignin)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new WebFaultException<string>("用户名不能为空", HttpStatusCode.BadRequest);
            }
            if (string.IsNullOrWhiteSpace(pwd))
            {
                throw new WebFaultException<string>("密码不能为空", HttpStatusCode.BadRequest);
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new WebFaultException<string>("姓名不能为空", HttpStatusCode.BadRequest);
            }

            User user = new User();
            user.Username = username;
            user.Pwd = pwd;
            user.Name = name;
            user.Email = email;
            user.Creation = DateTime.Now;
            user.Modification = DateTime.Now;
            user.Id = Guid.NewGuid().ToString();

            try
            {
                this.userService.SaveUser(user);

                string userToken = credentialsProvider.GenerateUserToken(username);
                if (autoSignin)
                {
                    clientManager.AddClient(username, userToken);
                }

                return userToken;
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public string Signin(string username, string pwd)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new WebFaultException<string>("用户名不能为空", HttpStatusCode.BadRequest);
            }
            if (string.IsNullOrWhiteSpace(pwd))
            {
                throw new WebFaultException<string>("密码不能为空", HttpStatusCode.BadRequest);
            }

            AuthenticationResult result;
            string[] roles;
            try
            {
                result = this.authenticationProvider.Authenticate(username, pwd, out roles);
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.InternalServerError);
            }

            if (AuthenticationResult.Pass == result)
            {
                string userToken = credentialsProvider.GenerateUserToken(username);
                clientManager.AddClient(username, userToken, roles);

                return userToken;
            }
            else if (AuthenticationResult.NotExisted == result)
            {
                throw new WebFaultException<string>("用户名不存在", HttpStatusCode.BadRequest);
            }
            else if (AuthenticationResult.Mismatch == result)
            {
                throw new WebFaultException<string>("密码错误", HttpStatusCode.BadRequest);
            }
            else
            {
                throw new WebFaultException<string>("用户名或密码不合法", HttpStatusCode.BadRequest);
            }
        }

        public bool Signout(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new WebFaultException<string>("用户名不能为空", HttpStatusCode.BadRequest);
            }
            try
            {
                clientManager.RemoveClient(username);

                return true;
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public bool IsUsernameExist(string username)
        {
            return false;
        }

    }
}
