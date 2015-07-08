using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Caliburn.Micro;

using Nega.Common;
using Nega.WpfCommon;
using Nega.WcfCommon;

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
            if (string.IsNullOrWhiteSpace(this.Username))
            {
                MessageBoxHelper.ShowWarning("用户名不能为空");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.Password))
            {
                MessageBoxHelper.ShowWarning("密码不能为空");
                return;
            }
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
                        WebException webException = ex.InnerExceptions[0] as WebException;
                        if (webException != null)
                        {
                            KeyValuePair<HttpStatusCode, string> status = WebHelper.GetStatusCodeAndMessage(webException);
                            if (status.Key == HttpStatusCode.BadRequest)
                            {
                                MessageBoxHelper.ShowWarning(status.Value);
                            }
                            else
                            {
                                this.logger.Log(ex);
                                MessageBoxHelper.ShowError();
                            }
                        }
                    });
        }

        public void Cancel()
        {
            this.TryClose(false);
        }

    }

}
