using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Providers.Instapaper
{
    public interface IInstapaperSettings : ISettings
    {
        bool IsEnabledSetting { get; set; }
    }
}
