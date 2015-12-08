using feedsea.Common.Api.OneNote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Providers.OneNote
{
    public interface IOneNoteSettings : IOneNoteApiSettings
    {
        bool IsLoggedInSetting { get; set; }
    }
}
