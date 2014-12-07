using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Providers.Instapaper
{
    public interface IInstapaperSettings : ISettings
    {
        bool IsEnabledSetting { get; set; }
    }
}
