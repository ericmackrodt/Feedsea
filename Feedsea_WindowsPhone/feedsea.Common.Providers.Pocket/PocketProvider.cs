using feedsea.Common.Api.Pocket;
using feedsea.Common.Providers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Providers.Pocket
{
    public class PocketProvider : IPocketProvider
    {
        private IPocketSettings settings;

        public string ServiceName
        {
            get { return "Feedly"; }
        }

        public PocketProvider(IPocketSettings pocketSettings)
        {
            settings = pocketSettings;
        }

        public IOauthLoginData LoginData
        {
            get
            {
                return new LoginData()
                {
                    LoginUrl = new PocketWebClient().GetLoginUrl(settings.LoginRequestTokenSetting, settings.OAuthRedirectUrl),
                    RedirectUrl = settings.OAuthRedirectUrl
                };
            }
        }

        public async Task PreLogin()
        {
            try
            {
                var client = new PocketWebClient();
                var result = await client.GetRequestToken(new RequestTokenData()
                {
                    ConsumerKey = settings.OAuthClientID,
                    RedirectUri = settings.OAuthRedirectUrl
                });

                settings.LoginRequestTokenSetting = result.Code;
            }
            catch (HttpRequestException)
            {
                throw new ProviderException(ExceptionReason.NoInternetConnection);
            }
        }

        public async Task<LoginStatus> Login()
        {
            try
            {
                var client = new PocketWebClient();
                var result = await client.GetAccessToken(new AccessTokenRequest()
                {
                    ConsumerKey = settings.OAuthClientID,
                    Code = settings.LoginRequestTokenSetting
                });

                settings.OAuthTokenSetting = result.AccessToken;

                return LoginStatus.Ok;
            }
            catch (HttpRequestException ex)
            {
                if (ex.Message.Contains("403"))
                    return LoginStatus.Pending;

                throw new ProviderException(ExceptionReason.NoInternetConnection);
            }
        }

        public Task<LoginStatus> Login(object loginData)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Add(ArticleData article)
        {
            try
            {
                var client = new PocketWebClient();
                var result = await client.Add(new AddRequest()
                {
                    AccessToken = settings.OAuthTokenSetting,
                    ConsumerKey = settings.OAuthClientID,
                    Title = article.Title,
                    Url = article.URL,
                });

                return result.Status == 1;
            }
            catch (HttpRequestException)
            {
                throw new ProviderException(ExceptionReason.NoInternetConnection);
            }
        }


        public Task<LoginStatus> Login(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
