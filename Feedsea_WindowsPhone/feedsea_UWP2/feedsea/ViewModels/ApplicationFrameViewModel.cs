using feedsea.Common;
using feedsea.Common.MVVM;
using feedsea.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.ViewModels
{

   [DataContract]
   public class ApplicationFrameViewModel
      : AViewModel<ApplicationFrameViewModel>
   {
      private IAppearanceSettings _appearanceSettings;
      private bool showAppTray;

      [DataMember]
      public bool ShowAppTray
      {
         get
         {
            return _appearanceSettings.ShowTrayOnTopSetting;
         }
         set
         {
            showAppTray = value;
            NotifyChanged(o => o.ShowAppTray);
         }
      }

      private bool hideAppBarOnMainPage;

      [DataMember]
      public bool HideAppBarOnMainPage
      {
         get
         {
            return _appearanceSettings.HideAppBarOnMainPageSetting;
         }
         set
         {
            hideAppBarOnMainPage = value;
            NotifyChanged(o => o.HideAppBarOnMainPage);
         }
      }

      private bool hideAppBarOnArticlePage;

      [DataMember]
      public bool HideAppBarOnArticlePage
      {
         get
         {
            return _appearanceSettings.HideAppBarOnArticlePageSetting;
         }
         set
         {
            hideAppBarOnArticlePage = value;
            NotifyChanged(o => o.HideAppBarOnArticlePage);
         }
      }


      public ApplicationFrameViewModel()
      {
      }

      public ApplicationFrameViewModel(IAppearanceSettings appearanceSettings)
      {
         _appearanceSettings = appearanceSettings;
      }
   }

}