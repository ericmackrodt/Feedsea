using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Api.OneNote
{
    [BaseUrl("https://www.onenote.com/api/v1.0/")]
    [LoginUrl("https://login.live.com/")]
    public class OneNoteApiClient : WebClientBase
    {
        public OneNoteApiClient() { }

        public OneNoteApiClient(string accessToken)
            : base("Bearer", accessToken)
        {

        }

        public override string GetLoginUrl(string clientId, string redirectUri, string state)
        {
            return LoginUrlAttribute.GetAttributeValue(this.GetType())
                .BuildUrl("oauth20_authorize.srf",
                new QueryStringParam("client_id", clientId),
                new QueryStringParam("scope", "wl.offline_access%20office.onenote_create"),
                new QueryStringParam("response_type", "code"),
                new QueryStringParam("redirect_uri", redirectUri),
                new QueryStringParam("display", "touch"));
        }

        public async Task<AccessTokenResponse> RequestAccessToken(AuthTokenRequest request)
        {
            return await PostRequest<AccessTokenResponse>(BuildAuthUrl("oauth20_token.srf"), ClientHelper.BuildQueryString(
                new QueryStringParam("client_id", request.ClientID),
                new QueryStringParam("redirect_uri", request.RedirectUrl),
                new QueryStringParam("client_secret", request.ClientSecret),
                new QueryStringParam("code", request.AuthorizationCode),
                new QueryStringParam("grant_type", "authorization_code")), null, "application/x-www-form-urlencoded");
        }

        public async Task<AccessTokenResponse> RefreshAccessToken(RefreshTokenRequest request)
        {
            return await PostRequest<AccessTokenResponse>(BuildAuthUrl("oauth20_token.srf"), ClientHelper.BuildQueryString(
                new QueryStringParam("client_id", request.ClientID),
                new QueryStringParam("redirect_uri", request.RedirectUrl),
                new QueryStringParam("client_secret", request.ClientSecret),
                new QueryStringParam("refresh_token", request.RefreshToken),
                new QueryStringParam("grant_type", "refresh_token")), null, "application/x-www-form-urlencoded");
        }

        public async Task<PagesResponse> AddPage(string content)
        {
            return await PostRequest<PagesResponse>(BuildMethodUrl("pages"), content, Encoding.UTF8, "text/html");
        }
    }
}
