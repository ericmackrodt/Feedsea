using feedsea.Common;
using feedsea.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace feedsea.AppThemes
{

   public class ThemeResourceDictionary
      : ResourceDictionary
   {
      private ResourceDictionary defaultResources;

      public ResourceDictionary DefaultResources
      {
         get
         {
            return defaultResources;
         }
         set
         {
            defaultResources = value;
            if ( value != null && GetCurrentTheme() == AppTheme.Default )
            {
               MergedDictionaries.Clear();
               MergedDictionaries.Add(value);
            }
         }
      }

      private ResourceDictionary darkResources;

      public ResourceDictionary DarkResources
      {
         get
         {
            return darkResources;
         }
         set
         {
            darkResources = value;
            if ( value != null && GetCurrentTheme() == AppTheme.Dark )
            {
               MergedDictionaries.Clear();
               MergedDictionaries.Add(value);
            }
         }
      }

      public bool IsDesignMode
      {
         get
         {
            return Windows.ApplicationModel.DesignMode.DesignModeEnabled || Windows.ApplicationModel.DesignMode.DesignModeEnabled;
         }
      }

      private AppTheme GetCurrentTheme()
      {
         if ( IsDesignMode )
            return AppTheme.Dark;
         else
         {
            var settings = new AppearanceSettings();
            return settings.CurrentThemeSetting;
         }
      }

   }

}