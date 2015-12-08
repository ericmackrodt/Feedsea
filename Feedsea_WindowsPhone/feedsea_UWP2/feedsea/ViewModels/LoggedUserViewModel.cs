using feedsea.Common.MVVM;
using feedsea.Models;
using feedsea.Resources;
using feedsea.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.ViewModels
{

   public class LoggedUserViewModel
      : AViewModel<LoggedUserViewModel>
   {
      private IGeneralSettings _settings;
      private LoggedUserModel _loggedUser;

      public LoggedUserModel LoggedUser
      {
         get
         {
            return _loggedUser;
         }
         set
         {
            _loggedUser = value;
            NotifyChanged(o => o.LoggedUser);
         }
      }

      private bool _showEmail;

      public bool ShowEmail
      {
         get
         {
            return _showEmail;
         }
         set
         {
            _showEmail = value;
            NotifyChanged(o => o.ShowEmail);
         }
      }

      private bool _showPicture;

      public bool ShowPicture
      {
         get
         {
            return _showPicture;
         }
         set
         {
            _showPicture = value;
            NotifyChanged(o => o.ShowPicture);
         }
      }


      public LoggedUserViewModel(IGeneralSettings settings)
      {
         _settings = settings;
         LoggedUser = new LoggedUserModel()
            {
               Name = _settings.UserNameSetting ?? AppResources.Sources, Picture = _settings.ProfilePictureSetting, Service = _settings.LoggedInServiceSetting, ServiceUser = _settings.LoginEmailSetting
            };
         ShowPicture = !string.IsNullOrWhiteSpace(_settings.ProfilePictureSetting);
         ShowEmail = !string.IsNullOrWhiteSpace(_settings.LoginEmailSetting);
      }
   }

}