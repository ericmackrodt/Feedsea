using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Providers.OneNote
{
    public interface IOneNoteSettings : ISettings
    {
        bool IsLoggedInSetting { get; set; }
    }
}
