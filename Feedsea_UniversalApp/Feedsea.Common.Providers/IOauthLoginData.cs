using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Providers
{
    public interface IOauthLoginData
    {
        string LoginUrl { get; set; }
        string RedirectUrl { get; set; }
    }
}
