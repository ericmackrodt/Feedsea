using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly.APIs
{
    public interface IAuthenticationAPI
    {
        string GetLoginUrl();
        Task RequestAccessToken(AuthTokenRequest request);
        Task RefreshToken();
    }
}
