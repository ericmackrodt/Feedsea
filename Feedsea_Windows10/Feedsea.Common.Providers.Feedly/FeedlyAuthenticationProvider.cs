using Feedsea.Common.Api.Feedly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Providers.Feedly
{
    public class FeedlyAuthenticationProvider : IAuthenticationProvider
    {
        private readonly IFeedlySettings settings;
        private readonly IProviderStorage storage;
        private readonly IFeedlyClient client;

        [Obsolete("I hate this method")]
        private async Task GetUserData()
        {
            var user = await client.Profile.Get();
            settings.ProfilePicture = user.Picture;

            if (user.TwitterConnected)
            {
                settings.LoginEmail = user.Twitter;
                settings.LoggedInService = "Twitter";
            }
            else if (user.FacebookConnected)
            {
                settings.LoginEmail = user.Email;
                settings.LoggedInService = "Facebook";
            }
            else if (user.WindowsLiveConnected)
            {
                settings.LoginEmail = user.Email;
                settings.LoggedInService = "Microsoft";
            }
            else if (user.EvernoteConnected)
            {
                settings.LoginEmail = user.Email;
                settings.LoggedInService = "Evernote";
            }
            else
            {
                settings.LoginEmail = user.Email;
                settings.LoggedInService = "Google";
            }

            settings.UserName = user.FullName;
        }

        public string ServiceName
        {
            get { return "Feedly"; }
        }

        public IOauthLoginData LoginData
        {
            get
            {
                return new LoginData()
                {
                    LoginUrl = client.Authentication.GetLoginUrl(),
                    RedirectUrl = ApiConstants.LoginDefaultRedirectUrl
                };
            }
        }

        public FeedlyAuthenticationProvider(IFeedlyClient feedlyClient, IFeedlySettings providerSettings, IProviderStorage providerStorage)
        {
            client = feedlyClient;
            settings = providerSettings;
            storage = providerStorage;
        }

        public async Task Initialization()
        {
            await storage.Initialize();
        }

        public async Task<LoginStatus> Login()
        {
            if (string.IsNullOrWhiteSpace(settings.OAuthRefreshToken) && string.IsNullOrWhiteSpace(settings.OAuthToken))
                return LoginStatus.Pending;

            if (DateTime.Now < settings.OAuthTokenExpiration)
                return LoginStatus.Ok;
            try
            {
                if (string.IsNullOrWhiteSpace(settings.OAuthRefreshToken))
                    return LoginStatus.Pending;

                await client.Authentication.RefreshToken();

                await GetUserData();

                return LoginStatus.Ok;
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException is WebException && (ex.InnerException as WebException).Status == WebExceptionStatus.RequestCanceled)
                {
                    return LoginStatus.Pending;
                }
                else
                    throw new ProviderException(ExceptionReason.NoInternetConnection);
            }
        }

        public async Task<LoginStatus> Login(object loginData)
        {
            try
            {
                var code = ((string)loginData);
                var tokenRequest = new AuthTokenRequest()
                {
                    ClientId = settings.OAuthClientID,
                    ClientSecret = settings.OAuthClientSecret,
                    Code = code,
                    RedirectUri = ApiConstants.LoginDefaultRedirectUrl
                };
                await client.Authentication.RequestAccessToken(tokenRequest);

                await GetUserData();

                return LoginStatus.Ok;
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException is WebException && (ex.InnerException as WebException).Status == WebExceptionStatus.RequestCanceled)
                {
                    return LoginStatus.Pending;
                }
                else
                    throw new ProviderException(ExceptionReason.NoInternetConnection);
            }
        }

        public Task<LoginStatus> Login(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
