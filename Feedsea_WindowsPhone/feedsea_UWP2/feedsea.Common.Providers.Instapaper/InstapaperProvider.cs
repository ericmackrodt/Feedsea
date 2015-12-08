using feedsea.Common.Api.Instapaper;
using feedsea.Common.Providers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Providers.Instapaper
{
    public class InstapaperProvider : IInstapaperProvider
    {
        private IInstapaperSettings settings;

        public string ServiceName
        {
            get { return "Instapaper"; }
        }

        public IOauthLoginData LoginData
        {
            get { throw new NotImplementedException(); }
        }

        public InstapaperProvider(IInstapaperSettings instapaperSettings)
        {
            settings = instapaperSettings;
        }

        public Task<LoginStatus> Login()
        {
            throw new NotImplementedException();
        }

        public Task<LoginStatus> Login(object loginData)
        {
            throw new NotImplementedException();
        }

        public async Task<LoginStatus> Login(string username, string password)
        {
            try
            {
                var client = new InstapaperApiClient();
                var result = await client.Authenticate(username, password);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    settings.OAuthTokenSetting = result;
                    return LoginStatus.Ok;
                }

                return LoginStatus.Pending;
            }
            catch (HttpRequestException)
            {
                throw new ProviderException(ExceptionReason.NoInternetConnection);
            }
        }

        public async Task Add(ArticleData article)
        {
            try
            {
                var client = new InstapaperApiClient();
                await client.Add(settings.OAuthTokenSetting, new AddRequestData()
                {
                    Url = article.URL,
                    Title = article.Title
                });
            }
            catch (HttpRequestException)
            {
                throw new ProviderException(ExceptionReason.NoInternetConnection);
            }
        }
    }
}
