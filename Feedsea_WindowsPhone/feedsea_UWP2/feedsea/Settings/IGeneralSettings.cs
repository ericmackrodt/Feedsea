using feedsea.BackgroundAgent.Common;
using feedsea.Common;
using feedsea.Common.Providers;
using feedsea.Common.Providers.MobilizerProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Settings
{

   public interface IGeneralSettings
      : IProviderSettings
   {

      bool FirstLoadSetting { get; set; }

      bool MarkArticlesReadWhenOpenedSetting { get; set; }

      string CategoryToLoadSetting { get; set; }

      bool MarkReadOnFeedScrollSetting { get; set; }

      bool DownloadArticleIfNoContentSetting { get; set; }

      bool FirstRefreshSetting { get; set; }

      bool IsAdsDisabledSetting { get; set; }

      bool AskConfirmationMarkReadSetting { get; set; }

   }

}