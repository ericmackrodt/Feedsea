using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly
{
    public interface IFeedlyApiSettings
    {
        string OAuthToken { get; set; }
        string OAuthRefreshToken { get; set; }
        DateTime OAuthTokenExpiration { get; set; }
        string UserID { get; set; }
        string OAuthClientID { get; }
        string OAuthClientSecret { get; }
        string OAuthRedirectUrl { get; }
    }
}
