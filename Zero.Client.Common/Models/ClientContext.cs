using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.Client.Common
{

    public class ClientContext
    {

        private static ClientContext current = new ClientContext();
        public static ClientContext Current
        {
            get { return current; }
        }

        private ClientContext() { }

        public string Username { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public string UserToken { get; private set; }

        public void Signin(string username, string userToken = null)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException();
            }

            this.Username = username;
            if (string.IsNullOrWhiteSpace(userToken))
            {
                this.UserToken = username;
            }
            else
            {
                this.UserToken = userToken;
            }
            this.IsAuthenticated = true;
        }

        public void Signout(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException();
            }
            if (this.Username != username)
            {
                throw new System.Security.SecurityException();
            }

            this.Username = null;
            this.UserToken = null;
            this.IsAuthenticated = false;
        }

    }

}
