using Feedsea.Common.Services;
using Feedsea.Common.Providers;
using Feedsea.Common.Providers.Feedly;
using MVVMBasic;
using MVVMBasic.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Feedsea.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        public event EventHandler OnAuthenticated;
        public event EventHandler LoginCanceled;

        private IFeedlySettings _settings;
        private ICommand _loginCommand;
        private IOAuthLogin _oauth;
        private IAuthenticationProvider _provider;

        public ICommand LoginCommand
        {
            get { return _loginCommand; }
        }

        public WelcomeViewModel(IFeedlySettings settings, IOAuthLogin oauth, IAuthenticationProvider provider)
        {
            _settings = settings;
            _oauth = oauth;
            _provider = provider;

            _loginCommand = new RelayCommandAsync(Login);
        }

        private async Task Login(object arg)
        {
            var loginCommand = (LoginCommand as RelayCommandAsync);
            loginCommand.IsEnabled = false;
            var loginData = _provider.LoginData;

            var data = await _oauth.Login(loginData.LoginUrl, loginData.RedirectUrl);

            if (data.Status == BrokerStatus.UserCancel || data.Status == BrokerStatus.ErrorHttp)
            {
                loginCommand.IsEnabled = true;
                if (LoginCanceled != null)
                    LoginCanceled(this, new EventArgs());
                return;
            }

            var result = LoginStatus.Pending;

            if (!string.IsNullOrWhiteSpace(data.Code))
                result = await _provider.Login(data.Code);

            if (result == LoginStatus.Ok && OnAuthenticated != null)
                OnAuthenticated(this, new EventArgs());
            else if (LoginCanceled != null)
            {
                loginCommand.IsEnabled = true;
                LoginCanceled(this, new EventArgs());
            }
        }
    }
}
