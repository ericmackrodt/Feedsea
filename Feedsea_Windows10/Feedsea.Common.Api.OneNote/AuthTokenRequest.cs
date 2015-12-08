using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feedsea.Common.Api.OneNote
{
    public class AuthTokenRequest
    {
        public string ClientID { get; set; }
        public string RedirectUrl { get; set; }
        public string ClientSecret { get; set; }
        public string AuthorizationCode { get; set; }
    }
}
