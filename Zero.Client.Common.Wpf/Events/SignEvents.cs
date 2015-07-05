using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Caliburn.Micro;

namespace Zero.Client.Common.Wpf
{

    public class SignEvent
    {

        private readonly string username;

        public string Username
        {
            get { return username; }
        }

        public SignEvent(string username)
        {
            this.username = username;
        }

    }

    public class SigninEvent : SignEvent
    {

        public SigninEvent(string username)
            : base(username)
        {

        }

    }

}
