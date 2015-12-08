using Cimbalino.Phone.Toolkit.Services;
using feedsea.Common;
using feedsea.Common.Controls;
using feedsea.Common.MVVM;
using feedsea.Common.MVVM.Commands;
using feedsea.Common.Providers;
using feedsea.Common.Providers.Instapaper;
using feedsea.Common.Providers.OneNote;
using feedsea.Common.Providers.Pocket;
using feedsea.Resources;
using feedsea.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace feedsea.ViewModels
{

   public class SettingsThirdPartyViewModel
      : AViewModel<SettingsThirdPartyViewModel>
   {
      private IThirdPartySettings _settings;
      private IOneNoteSettings _oneNoteSettings;
      private IOneNoteProvider _oneNote;
      private IPocketProvider _pocket;
      private IPocketSettings _pocketSettings;
      private IInstapaperProvider _instapaper;
      private IInstapaperSettings _instapaperSettings;
      private IOAuthLogin _oauth;
      private IXAuthLogin _xauth;
      private IMessageBoxService _messageBox;

#region Properties
      public IOAuthLogin OAuthLogin
      {
         get
         {
            return _oauth;
         }
      }

      public IXAuthLogin XAuthLogin
      {
         get
         {
            return _xauth;
         }
      }

      [IgnoreDataMember]
      public int SelectedYoutubeClient
      {
         get
         {
            return (int)_settings.YoutubeClientSetting;
         }
         set
         {
            _settings.YoutubeClientSetting = (YouTubeClients)value;
            NotifyChanged(o => o.SelectedYoutubeClient);
         }
      }

      [IgnoreDataMember]
      public int SelectedLinkBrowser
      {
         get
         {
            return (int)_settings.LinkNavigationSetting;
         }
         set
         {
            _settings.LinkNavigationSetting = (LinkNavigationBrowsers)value;
            NotifyChanged(o => o.SelectedLinkBrowser);
         }
      }
#endregion Properties

#region Pocket_Settings_Properties
      [IgnoreDataMember]
      public bool IsPocketEnabled
      {
         get
         {
            return _settings.IsPocketEnabledSetting;
         }
         set
         {
            _settings.IsPocketEnabledSetting = value;
            NotifyChanged(o => o.IsPocketEnabled);
         }
      }

      [IgnoreDataMember]
      public bool IsThirdPartyPocketEnabled
      {
         get
         {
            return _settings.PocketShareEnabledSetting;
         }
         set
         {
            _settings.PocketShareEnabledSetting = value;
            if ( value )
               _pocketSettings.IsEnabledSetting = false;
            NotifyChanged(o => o.IsThirdPartyPocketEnabled);
         }
      }

      [IgnoreDataMember]
      public bool IsInternalPocketEnabled
      {
         get
         {
            return _pocketSettings.IsEnabledSetting;
         }
         set
         {
            _pocketSettings.IsEnabledSetting = value;
            if ( value )
               _settings.PocketShareEnabledSetting = false;
            NotifyChanged(o => o.IsInternalPocketEnabled);
         }
      }
#endregion Pocket_Settings_Properties

#region Instapaper_Settings_Properties
      [IgnoreDataMember]
      public bool IsInstapaperEnabled
      {
         get
         {
            return _instapaperSettings.IsEnabledSetting;
         }
         set
         {
            _instapaperSettings.IsEnabledSetting = value;
            NotifyChanged(o => o.IsInstapaperEnabled);
         }
      }
#endregion Instapaper_Settings_Properties

#region OneNote_Settings_Properties
      [IgnoreDataMember]
      public bool IsOneNoteActive
      {
         get
         {
            return _oneNoteSettings.IsLoggedInSetting;
         }
         set
         {
            _oneNoteSettings.IsLoggedInSetting = value;
            NotifyChanged(o => o.IsOneNoteActive);
         }
      }
#endregion OneNote_Settings_Properties

#region Commands
      private ICommand _toggleOneNoteCommand;

      public ICommand ToggleOneNoteCommand
      {
         get
         {
            return _toggleOneNoteCommand;
         }
         set
         {
            _toggleOneNoteCommand = value;
         }
      }

      private ICommand _togglePocketCommand;

      public ICommand TogglePocketCommand
      {
         get
         {
            return _togglePocketCommand;
         }
      }

      private ICommand _toggleInstapaperCommand;

      public ICommand ToggleInstapaperCommand
      {
         get
         {
            return _toggleInstapaperCommand;
         }
      }
#endregion Commands


      public SettingsThirdPartyViewModel(IThirdPartySettings settings, IOneNoteProvider oneNoteProvider, IOneNoteSettings oneNoteSettings, IOAuthLogin oauthLogin, IXAuthLogin xauthLogin, IPocketProvider pocketProvider, IPocketSettings pocketSettings, IInstapaperSettings instapaperSettings, IInstapaperProvider instapaperProvider, IMessageBoxService messageBoxService, IConnectionVerify connectionVerify)
      {
         _settings = settings;
         _oneNote = oneNoteProvider;
         _oneNoteSettings = oneNoteSettings;
         _oauth = oauthLogin;
         _xauth = xauthLogin;
         _pocket = pocketProvider;
         _pocketSettings = pocketSettings;
         _messageBox = messageBoxService;
         _instapaperSettings = instapaperSettings;
         _instapaper = instapaperProvider;
         this.connection = connectionVerify;
         this._toggleOneNoteCommand = new AsyncDelegateCommand(ToggleOneNote);
         this._togglePocketCommand = new AsyncDelegateCommand(TogglePocket);
         this._toggleInstapaperCommand = new AsyncDelegateCommand(ToggleInstapaper);
      }

#region OneNote_Settings_Methods
      private async Task ToggleOneNote(object arg)
      {
         IsOneNoteActive = await ConnectionVerifyCall(async () =>
               {
                  if ( IsOneNoteActive )
                  {
                     var code = await _oauth.Login(_oneNote.LoginData.LoginUrl, _oneNote.LoginData.RedirectUrl);
                     if ( !string.IsNullOrWhiteSpace(code) )
                     {
                        var result = await _oneNote.Login(code);
                        return result == LoginStatus.Ok;
                     }
                  }
                  return false;
               });
      }
#endregion OneNote_Settings_Methods

#region Pocket_Settings_Methods
      private async Task TogglePocket(object arg)
      {
         int choice = -1;
         if ( IsPocketEnabled )
            choice = await _messageBox.ShowAsync(AppResources.Msg_Enable_Pocket, AppResources.Lbl_Enable_Pocket, new string[] { AppResources.Share_Pocket_Internal, AppResources.Share_Pocket_ThirdParty });
         if ( choice == -1 )
         {
            IsPocketEnabled = false;
            return ;
         }
         if ( choice == 1 )
         {
            IsThirdPartyPocketEnabled = true;
            IsPocketEnabled = true;
            return ;
         }
         if ( choice == 0 )
         {
            IsPocketEnabled = IsInternalPocketEnabled = await ConnectionVerifyCall(async () =>
                     {
                        await _pocket.PreLogin();
                        var preLoginResult = _pocket.LoginData;
                        if ( preLoginResult == null )
                           return false;
                        var result = await _oauth.Login(_pocket.LoginData.LoginUrl, _pocket.LoginData.RedirectUrl);
                        if ( result == null )
                           return false;
                        var loginStatus = await _pocket.Login();
                        return loginStatus == LoginStatus.Ok;
                     });
         }
      }
#endregion Pocket_Settings_Methods

#region Instapaper_Settings_Methods
      private async Task ToggleInstapaper(object arg)
      {
         IsInstapaperEnabled = await ConnectionVerifyCall(async () =>
               {
                  if ( IsInstapaperEnabled )
                     return await _xauth.Login(_instapaper);
                  return false;
               });
      }
#endregion Instapaper_Settings_Methods

   }

}