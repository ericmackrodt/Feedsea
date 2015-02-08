using Feedsea.Common.Api.OneNote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Api.OneNote
{
    public interface IOneNoteApiClient
    {
        string GetLoginUrl(string clientId, string redirectUri, string state);
        Task<AccessTokenResponse> RequestAccessToken(AuthTokenRequest request);
        Task<AccessTokenResponse> RefreshAccessToken(RefreshTokenRequest request);
        Task<PagesResponse> AddPage(string content);
    }
}
