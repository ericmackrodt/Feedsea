using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace feedsea.Common.Api.OneNote
{
    public class RefreshTokenRequest
    {
        public string ClientID { get; set; }
        public string RedirectUrl { get; set; }
        public string ClientSecret { get; set; }
        public string RefreshToken { get; set; }
    }
}
