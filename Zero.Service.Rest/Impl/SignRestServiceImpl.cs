using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Net;

using Zero.Domain;
using Zero.BLL;

namespace Zero.Service.Rest
{
    
    public class SignRestServiceImpl : MarshalByRefObject, ISignRestService
    {

        private readonly IUserService userService;

        public SignRestServiceImpl()
        {
            
        }

        public SignRestServiceImpl(IUserService userService)
        {
            this.userService = userService;
        }

        public User Signup(string username, string pwd, string name, string email)
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
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.InternalServerError);
            }

            return user;
        }

        public bool Signin(string username, string pwd)
        {
            throw new NotImplementedException();
        }

        public bool Signout(string username, string pwd)
        {
            throw new NotImplementedException();
        }

        public bool IsUsernameExist(string username)
        {
            return false;
        }

    }
}
