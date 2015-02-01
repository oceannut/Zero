using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Zero.Domain;
using Zero.BLL;

namespace Zero.Service.Rest
{
    
    public class SignService : ISignService
    {

        private readonly IUserService userService;

        //public SignService(IUserService userService)
        //{
        //    this.userService = userService;
        //}

        public SignService()
        {
            
        }

        public User Signup(string username, string pwd, string name, string email)
        {
            throw new NotImplementedException();
        }

        public bool Signin(string username, string pwd)
        {
            throw new NotImplementedException();
        }

        public bool Signout(string username)
        {
            throw new NotImplementedException();
        }

        public bool IsUsernameExist(string username)
        {
            return false;
        }

    }
}
