using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Providers
{
    public interface IProviderSettings : ISettings, INotifyPropertyChanged
    {
        bool ShowReadSetting { get; set; }
        string UserIdSetting { get; set; }
        bool ArticlesFromOldestToNewestSetting { get; set; }
        bool ShowReadIfNoUnreadSetting { get; set; }
        string ProfilePictureSetting { get; set; }
        string LoginEmailSetting { get; set; }
        string UserNameSetting { get; set; }
        string LoggedInServiceSetting { get; set; }
    }
}
