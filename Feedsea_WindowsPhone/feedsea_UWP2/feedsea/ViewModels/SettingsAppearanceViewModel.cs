using feedsea.Common;
using feedsea.Common.MVVM;
using feedsea.Settings;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.ViewModels
{

   public class SettingsAppearanceViewModel
      : AViewModel<SettingsAppearanceViewModel>
   {
      public event EventHandler ThemeChanged;
      private IAppearanceSettings _settings;
      private ApplicationFrameViewModel _frameViewModel;

      [IgnoreDataMember]
      public bool HideAppBarMainPage
      {
         get
         {
            return _settings.HideAppBarOnMainPageSetting;
         }
         set
         {
            _settings.HideAppBarOnMainPageSetting = value;
            _frameViewModel.HideAppBarOnMainPage = value;
            NotifyChanged(o => o.HideAppBarMainPage);
         }
      }

      [IgnoreDataMember]
      public bool HideAppBarArticlePage
      {
         get
         {
            return _settings.HideAppBarOnArticlePageSetting;
         }
         set
         {
            _settings.HideAppBarOnArticlePageSetting = value;
            _frameViewModel.HideAppBarOnMainPage = value;
            NotifyChanged(o => o.HideAppBarArticlePage);
         }
      }

      [IgnoreDataMember]
      public bool ShowTrayBar
      {
         get
         {
            return _settings.ShowTrayOnTopSetting;
         }
         set
         {
            _settings.ShowTrayOnTopSetting = value;
            _frameViewModel.ShowAppTray = value;
            NotifyChanged(o => o.ShowTrayBar);
         }
      }

      [IgnoreDataMember]
      public int CurrentAppTheme
      {
         get
         {
            return (int)_settings.CurrentThemeSetting;
         }
         set
         {
            var val = (AppTheme)value;
            if ( _settings.CurrentThemeSetting != val && ThemeChanged != null )
            {
               _settings.CurrentThemeSetting = val;
               ThemeChanged(this, new EventArgs());
               NotifyChanged(o => o.CurrentAppTheme);
            }
         }
      }

      [IgnoreDataMember]
      public int ArticleListTemplate
      {
         get
         {
            return (int)_settings.ArticleItemsTemplateTypeSetting;
         }
         set
         {
            _settings.ArticleItemsTemplateTypeSetting = (ArticleTemplateType)value;
            SendMessage("ArticleTemplateType");
            NotifyChanged(o => o.ArticleListTemplate);
         }
      }


      public SettingsAppearanceViewModel(IAppearanceSettings settings, ApplicationFrameViewModel applicationFrameViewModel)
      {
         _settings = settings;
         _frameViewModel = applicationFrameViewModel;
      }

      private static void SendMessage(string property)
      {
         Messenger.Default.Send<GenericMessage<MessageContent>>(new GenericMessage<MessageContent>(new MessageContent(MessageContentType.SettingsUpdated, property)));
      }

   }

}