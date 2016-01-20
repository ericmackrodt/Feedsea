using HttpClientHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly.APIs
{
    public class AuthenticationAPI : IAuthenticationAPI
    {
        private IFeedlyApiSettings settings;
        private FeedlyWebClient client;
        private string baseUrl;
        
        internal AuthenticationAPI(string baseUrl, FeedlyWebClient client, IFeedlyApiSettings settings)
        {
            this.client = client;
            this.baseUrl = baseUrl;
            this.settings = settings;
        }

        public string GetLoginUrl()
        {
            return QueryHelper.BuildRequestUrl(baseUrl, "auth/auth", new
            {
                scope = "https://cloud.feedly.com/subscriptions",
                response_type = "code",
                client_id = settings.OAuthClientID,
                redirect_uri = settings.OAuthRedirectUrl ?? ApiConstants.LoginDefaultRedirectUrl,
                state = ""
            }.BuildQueryString());
        }

        public async Task RefreshToken()
        {
            var request = new RefreshTokenRequest()
            {
                GrantType = "refresh_token",
                ClientId = settings.OAuthClientID,
                ClientSecret = settings.OAuthClientSecret,
                RefreshToken = settings.OAuthRefreshToken
            };

            client.ShouldCheckTokenState = false;
            var response = await client.Post<AuthTokenResponse>("auth/token", request);
            client.ShouldCheckTokenState = true;
            settings.OAuthToken = response.AccessToken;
            settings.OAuthTokenExpiration = DateTime.Now.AddSeconds(response.ExpiresIn - 1);
            client.UpdateClientAccessToken();
        }

        public async Task RequestAccessToken(AuthTokenRequest request)
        {
            request.GrantType = "authorization_code";
            client.ShouldCheckTokenState = false;
            var response = await client.Post<AuthTokenResponse>("auth/token", request);
            client.ShouldCheckTokenState = true;
            settings.OAuthToken = response.AccessToken;
            settings.OAuthRefreshToken = response.RefreshToken;
            settings.OAuthTokenExpiration = DateTime.Now.AddSeconds(response.ExpiresIn - 1);
            settings.UserID = response.Id;
            client.UpdateClientAccessToken();
        }

        internal async Task CheckTokenState()
        {
            if (!string.IsNullOrWhiteSpace(settings.OAuthRefreshToken) &&
                !string.IsNullOrWhiteSpace(settings.OAuthToken) &&
                DateTime.Now >= settings.OAuthTokenExpiration)
                await RefreshToken();
        }
    }
}
