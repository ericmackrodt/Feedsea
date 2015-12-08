using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Providers
{
    public interface ISettings
    {
        string OAuthRedirectUrl { get; }
        string OAuthClientID { get; }
        string OAuthClientSecret { get; }
        string OAuthTokenSetting { get; set; }
        string OAuthRefreshTokenSetting { get; set; }
        DateTime OAuthTokenLimitSetting { get; set; }
    }
}
