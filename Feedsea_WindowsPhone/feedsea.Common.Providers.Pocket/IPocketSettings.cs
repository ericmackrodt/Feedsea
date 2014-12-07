using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Providers.Pocket
{
    public interface IPocketSettings : ISettings
    {
        string LoginRequestTokenSetting { get; set; }
        bool IsEnabledSetting { get; set; }
    }
}
