using Cimbalino.Phone.Toolkit.Services;
using feedsea.BackgroundAgent.Common;
using feedsea.Common;
using feedsea.Common.Controls;
using feedsea.Common.MVVM;
using feedsea.Common.MVVM.Commands;
using feedsea.Common.Providers;
using feedsea.Common.Providers.Data;
using feedsea.Common.Providers.Feedly;
using feedsea.Common.Providers.Instapaper;
using feedsea.Common.Providers.MobilizerProvider;
using feedsea.Common.Providers.OneNote;
using feedsea.Common.Providers.Pocket;
using feedsea.Models;
using feedsea.Resources;
using feedsea.Services;
using feedsea.Settings;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using System.Windows.Input;

namespace feedsea.ViewModels
{

   public delegate void RedirectToPocketHandler(string url);

   [DataContract]
   public class SettingsViewModel
      : AViewModel<SettingsViewModel>
   {
      public event EventHandler LoggedOut;
      private IPaidFeatures _paidFeatures;
      private IGeneralSettings _generalSettings;
      private INewsService _service;
      private IMessageBoxService _messageBox;

      [IgnoreDataMember]
      public bool IsAdsEnabled
      {
         get
         {
            return !_generalSettings.IsAdsDisabledSetting;
         }
         set
         {
            _generalSettings.IsAdsDisabledSetting = !value;
            NotifyChanged(o => o.IsAdsEnabled);
         }
      }

      private ObservableCollection<SettingItemModel> _settingsItems;

      public ObservableCollection<SettingItemModel> SettingsItems
      {
         get
         {
            return _settingsItems;
         }
         set
         {
            _settingsItems = value;
            NotifyChanged(o => o.SettingsItems);
         }
      }

      private ICommand disableAdsCommand;

      public ICommand DisableAdsCommand
      {
         get
         {
            return disableAdsCommand;
         }
      }

      private ICommand logoutCommand;

      public ICommand LogoutCommand
      {
         get
         {
            return logoutCommand;
         }
      }


      public SettingsViewModel(IPaidFeatures paidFeatures, IGeneralSettings generalSettings, INewsService service, IMessageBoxService messageBox)
      {
         _paidFeatures = paidFeatures;
         _generalSettings = generalSettings;
         _service = service;
         _messageBox = messageBox;
         this.disableAdsCommand = new AsyncDelegateCommand(DisableAds);
         this.logoutCommand = new RelayCommand(Logout);
      }

      public override void LoadData(object argument)
      {
         SettingsItems = new ObservableCollection<SettingItemModel>(argument as IEnumerable<SettingItemModel>);
      }

      private void Logout()
      {
         if ( IsBusy )
            return ;
         var result = _messageBox.Show(AppResources.Logout_Message, AppResources.Logout, MessageBoxButton.OKCancel);
         if ( result != 10 )
            return ;
         _service.ClearProviderData();
         (_generalSettings as SettingsBase).ClearSettings();
         IsDataLoaded = false;
         Messenger.Default.Send<GenericMessage<MessageContent>>(new GenericMessage<MessageContent>(new MessageContent(MessageContentType.Logoff, "")));
         if ( LoggedOut != null )
            LoggedOut(this, new EventArgs());
      }

      private async Task DisableAds(object arg)
      {
         var result = await _paidFeatures.BuyFeature(_paidFeatures.DisableAdsProduct);
         if ( result == LicenseStatus.AlreadyActivated )
         {
            _messageBox.Show(AppResources.Purchase_AlreadyPurchased);
            IsAdsEnabled = false;
         }
         else if ( result == LicenseStatus.Purchased )
         {
            _messageBox.Show(AppResources.Purchase_Success);
            IsAdsEnabled = false;
         }
         else if ( result == LicenseStatus.NotPurchased )
         {
            _messageBox.Show(AppResources.Purchase_Canceled);
            IsAdsEnabled = true;
         }
      }

   }

}