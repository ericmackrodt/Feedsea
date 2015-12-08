using feedsea.Common.Api.OneNote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Providers.OneNote
{
    public class OneNoteProvider : IOneNoteProvider
    {
        private IOneNoteSettings settings;

        public string ServiceName
        {
            get { return "OneNote"; }
        }

        public IOauthLoginData LoginData
        {
            get
            {
                return new LoginData()
                {
                    LoginUrl = new OneNoteApiClient().GetLoginUrl(settings.OAuthClientID, settings.OAuthRedirectUrl, ""),
                    RedirectUrl = settings.OAuthRedirectUrl
                };
            }
        }

        public OneNoteProvider(IOneNoteSettings oneNoteSettings)
        {
            settings = oneNoteSettings;
        }

        public async Task<LoginStatus> Login()
        {
            if (string.IsNullOrWhiteSpace(settings.OAuthRefreshTokenSetting) && string.IsNullOrWhiteSpace(settings.OAuthTokenSetting))
                return LoginStatus.Pending;

            if (DateTime.Now < settings.OAuthTokenLimitSetting)
                return LoginStatus.Ok;
            try
            {
                if (string.IsNullOrWhiteSpace(settings.OAuthRefreshTokenSetting))
                    return LoginStatus.Pending;

                var cli = new OneNoteApiClient(settings.OAuthTokenSetting);
                var refreshTokenRequest = new RefreshTokenRequest()
                {
                    ClientID = settings.OAuthClientID,
                    ClientSecret = settings.OAuthClientSecret,
                    RefreshToken = settings.OAuthRefreshTokenSetting,
                    RedirectUrl = settings.OAuthRedirectUrl
                };
                var response = await cli.RefreshAccessToken(refreshTokenRequest);

                settings.OAuthTokenSetting = response.AccessToken;
                settings.OAuthTokenLimitSetting = DateTime.Now.AddSeconds(response.ExpiresIn);
                settings.OAuthRefreshTokenSetting = response.RefreshToken;

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
                var cli = new OneNoteApiClient(settings.OAuthTokenSetting);
                var tokenRequest = new AuthTokenRequest()
                {
                    ClientID = settings.OAuthClientID,
                    ClientSecret = settings.OAuthClientSecret,
                    AuthorizationCode = (string)loginData,
                    RedirectUrl = settings.OAuthRedirectUrl
                };
                var response = await cli.RequestAccessToken(tokenRequest);
                settings.OAuthTokenSetting = response.AccessToken;
                settings.OAuthTokenLimitSetting = DateTime.Now.AddSeconds(response.ExpiresIn);
                settings.OAuthRefreshTokenSetting = response.RefreshToken;

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

        public async Task AddPage(string content)
        {
            try
            {
                await Login();
                var cli = new OneNoteApiClient(settings.OAuthTokenSetting);
                await cli.AddPage(content);
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException is WebException && (ex.InnerException as WebException).Status == WebExceptionStatus.RequestCanceled)
                {
                    return;
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
