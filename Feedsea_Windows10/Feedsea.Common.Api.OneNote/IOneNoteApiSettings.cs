using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Api.OneNote
{
    public interface IOneNoteApiSettings
    {
        string OAuthToken { get; set; }
        string OAuthRefreshToken { get; set; }
        DateTime OAuthTokenExpiration { get; set; }
        string OAuthClientID { get; }
        string OAuthClientSecret { get; }
        string OAuthRedirectUrl { get; }
    }
}
