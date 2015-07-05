using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Caliburn.Micro;

using Nega.Common;
using Nega.WpfCommon;

namespace Zero.Client.Common.Wpf
{

    public class SigninViewModel : Screen
    {

        private readonly ISignClient signClient;
        private readonly ILogger logger;
        private readonly IEventAggregator eventAggregator;

        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                if (username != value)
                {
                    username = value;
                    NotifyOfPropertyChange(() => this.Username);
                }
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    NotifyOfPropertyChange(() => this.Password);
                }
            }
        }

        public SigninViewModel(ISignClient signClient, IEventAggregator eventAggregator)
        {
            this.signClient = signClient;
            this.eventAggregator = eventAggregator;
            this.logger = Nega.Common.LogManager.GetLogger();

            this.Username = "zsp";
            this.Password = "psz";
        }

        public void Signin()
        {
            this.signClient.SigninAsync(this.Username, this.Password)
                .ExcuteOnUIThread<string>(
                    (result) =>
                    {
                        ClientContext.Current.Signin(this.Username, result);
                        this.eventAggregator.PublishOnUIThread(new SigninEvent(this.Username));

                        this.TryClose(true);
                    },
                    (ex) =>
                    {
                        this.logger.Log(ex);
                        MessageBoxHelper.ShowError();
                    });
        }

        public void Cancel()
        {
            this.TryClose(false);
        }

    }

}
