using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using feedsea.Common.Api.OneNote;
using System.Net.Http;
using System.Net.Http.Headers;
using HttpClientHelpers;

namespace Feedsea.Common.Api.OneNote
{
    public class OneNoteApiClient : IOneNoteApiClient
    {
        private IOneNoteApiSettings _settings;

        private readonly string _baseUrl = "https://www.onenote.com/api/v1.0/";
        private readonly string _loginUrl = "https://login.live.com/";
        private readonly HttpClient _client;

        public OneNoteApiClient(IOneNoteApiSettings settings)//"Bearer"
        {
            _settings = settings;

            var handler = new HttpClientHandler();

            if (handler.SupportsAutomaticDecompression)
                handler.AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate;

            _client = new HttpClient(handler);
            _client.BaseAddress = new Uri(_baseUrl);
            _client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        }

        public string GetLoginUrl()
        {
            return QueryHelper.BuildRequestUrl(_loginUrl, "oauth20_authorize.srf", new
            {
                client_id = _settings.OAuthClientID,
                scope = "wl.offline_access%20office.onenote_create",
                response_type = "code",
                redirect_uri = _settings.OAuthRedirectUrl,
                display = "touch"
            }.BuildQueryString());
        }

        private async Task<TOut> PostRequest<TOut>(string url, string contentType, object content)
        {
            return await PostRequest<TOut>(url, contentType, content);
        }

        public async Task RequestAccessToken(string authorizationCode)
        {
            var response = await PostRequest<AccessTokenResponse>(
                QueryHelper.BuildRequestUrl(_baseUrl, "oauth20_token.srf", ""), 
                "application/x-www-form-urlencoded", 
                new 
                {
                    client_id = _settings.OAuthClientID,
                    redirect_url = _settings.OAuthRedirectUrl,
                    client_secret = _settings.OAuthClientSecret,
                    code = authorizationCode,
                    grant_type = "authorization_code"
                });

            _settings.OAuthToken = response.AccessToken;
            _settings.OAuthTokenExpiration = DateTime.Now.AddSeconds(response.ExpiresIn);
            _settings.OAuthRefreshToken = response.RefreshToken;
        }

        public async Task<AccessTokenResponse> RefreshAccessToken(RefreshTokenRequest request)
        {
            return await PostRequest<AccessTokenResponse>(
                QueryHelper.BuildRequestUrl(_baseUrl, "oauth20_token.srf", ""),
                "application/x-www-form-urlencoded", 
                new 
                {
                    client_id = request.ClientID,
                    redirect_url = request.RedirectUrl,
                    client_secret = request.ClientSecret,
                    refresh_token = request.RefreshToken,
                    grant_type = "refresh_token"
                });
        }

        public async Task<PagesResponse> AddPage(string content)
        {
            return await PostRequest<PagesResponse>(QueryHelper.BuildRequestUrl(_baseUrl, "pages", ""), "text/html", content);
        }
    }
}
