using feedsea.Common.Controls;
using feedsea.Common.MVVM;
using feedsea.Common.MVVM.Commands;
using feedsea.Common.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace feedsea.ViewModels
{
    public class LoginViewModel : AViewModel<LoginViewModel>
    {
        public event EventHandler OnAuthenticated;
        public event EventHandler LoginCanceled;

        private INewsProvider provider;
        private IOAuthLogin oauth;

        private ICommand loginCommand;
        public ICommand LoginCommand
        {
            get { return loginCommand; }
        }

        public LoginViewModel(INewsProvider serviceProvider, IOAuthLogin oauthLogin, IConnectionVerify connectionVerify)
        {
            provider = serviceProvider;
            oauth = oauthLogin;
            connection = connectionVerify;

            loginCommand = new AsyncDelegateCommand(Login);
        }

        public bool CancelLogin()
        {
            if (oauth.IsOpen)
            {
                oauth.Cancel();
                return true;
            }

            return false;
        }

        private async Task Login(object arg)
        {
            await ConnectionVerifyCall(async () =>
            {
                (LoginCommand as AsyncDelegateCommand).IsEnabled = false;
                var loginData = provider.LoginData;
                var code = await oauth.Login(loginData.LoginUrl, loginData.RedirectUrl);
                var result = LoginStatus.Pending;

                if (!string.IsNullOrEmpty(code))
                    result = await provider.Login(code);

                if (result == LoginStatus.Ok && OnAuthenticated != null)
                    OnAuthenticated(this, new EventArgs());
                else if (LoginCanceled != null)
                {
                    (LoginCommand as AsyncDelegateCommand).IsEnabled = true;
                    LoginCanceled(this, new EventArgs());
                }
            });
        }
    }
}
