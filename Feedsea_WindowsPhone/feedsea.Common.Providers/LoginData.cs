using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Providers
{
    public class LoginData : IOauthLoginData
    {
        public string LoginUrl { get; set; }
        public string RedirectUrl { get; set; }
    }
}
