using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.Client.Common
{

    public interface ISignClient
    {

        string Signin(string username, string pwd);

        Task<string> SigninAsync(string username, string pwd);

    }

}
