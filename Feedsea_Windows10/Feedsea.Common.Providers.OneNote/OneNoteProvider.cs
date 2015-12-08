using Feedsea.Common.Api.OneNote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Providers.OneNote
{
    public class OneNoteProvider : IOneNoteProvider
    {
        private IOneNoteSettings _settings;

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
                    LoginUrl = new OneNoteApiClient(_settings).GetLoginUrl(),
                    RedirectUrl = _settings.OAuthRedirectUrl
                };
            }
        }

        public OneNoteProvider(IOneNoteSettings oneNoteSettings)
        {
            _settings = oneNoteSettings;
        }

        public async Task<LoginStatus> Login()
        {
            if (string.IsNullOrWhiteSpace(_settings.OAuthRefreshToken) && string.IsNullOrWhiteSpace(_settings.OAuthToken))
                return LoginStatus.Pending;

            if (DateTime.Now < _settings.OAuthTokenExpiration)
                return LoginStatus.Ok;
            try
            {
                if (string.IsNullOrWhiteSpace(_settings.OAuthRefreshToken))
                    return LoginStatus.Pending;

                var cli = new OneNoteApiClient(_setting);
                var refreshTokenRequest = new RefreshTokenRequest()
                {
                    ClientID = _settings.OAuthClientID,
                    ClientSecret = _settings.OAuthClientSecret,
                    RefreshToken = _settings.OAuthRefreshTokenSetting,
                    RedirectUrl = _settings.OAuthRedirectUrl
                };
                var response = await cli.RefreshAccessToken(refreshTokenRequest);

                _settings.OAuthTokenSetting = response.AccessToken;
                _settings.OAuthTokenLimitSetting = DateTime.Now.AddSeconds(response.ExpiresIn);
                _settings.OAuthRefreshTokenSetting = response.RefreshToken;

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
                var cli = new OneNoteApiClient(_settings);
                await cli.RequestAccessToken((string)loginData);

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
                var cli = new OneNoteApiClient(_settings.OAuthTokenSetting);
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
