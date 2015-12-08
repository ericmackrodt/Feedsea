using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Settings
{

   public abstract class SettingsBase
      : ISettings
   {
      // Our settings
      protected Windows.Storage.ApplicationDataContainer settings;

      public SettingsBase()
      {
         settings = Windows.Storage.ApplicationData.Current.LocalSettings;
      }

      /// <summary>
      /// Update a setting value for our application. If the setting does not
      /// exist, then add the setting.
      /// </summary>
      /// <param name="key"></param>
      /// <param name="value"></param>
      /// <returns></returns>
      public bool AddOrUpdateValue(string key, Object value)
      {
         bool valueChanged = false;
         var settingKey = GetSettingKey(key);
         // If the key exists
         if ( settings.Values.ContainsKey(settingKey) )
         {
            // If the value has changed
            if ( settings.Values[settingKey] != value )
            {
               // Store the new value
               settings.Values[settingKey] = value;
               valueChanged = true;
            }
         }
         // Otherwise create the key.
         else
         {
            settings.Values.Add(settingKey, value);
            valueChanged = true;
         }
         return valueChanged;
      }

      /// <summary>
      /// Get the current value of the setting, or if it is not found, set the 
      /// setting to the default setting.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="key"></param>
      /// <param name="defaultValue"></param>
      /// <returns></returns>
      public T GetValueOrDefault<T>(string key, T defaultValue)
      {
         T value;
         var settingKey = GetSettingKey(key);
         // If the key exists, retrieve the value.
         if ( settings.Values.ContainsKey(settingKey) )
         {
            value = (T)settings.Values[settingKey];
         }
         // Otherwise, use the default value.
         else
         {
            value = defaultValue;
         }
         return value;
      }

      /// <summary>
      /// Save the settings.
      /// </summary>
      public void Save()
      {
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.IO.IsolatedStorage.IsolatedStorageSettings.Save was not upgraded
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.IO.IsolatedStorage.IsolatedStorageSettings.Save was not upgraded
         settings.Save();
      }

      private string GetSettingKey(string key)
      {
         return string.Format("{0}_{1}", this.GetType().Name, key);
      }

      public void ClearSettings()
      {
         settings.Clear();
      }

      public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
   }

}