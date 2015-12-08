using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Helpers
{

   public static class ApplicationSettingHelper
   {

      /// <summary>
      /// Attempt to get a value from the application settings
      /// if not successful (no present, wrong type), returns provided default value
      /// </summary>
      /// <typeparam name="TValue">Expected type of the setting</typeparam>
      /// <param name="key">Name of the key</param>
      /// <param name="defaultValue">Default value to be returned if fails</param>
      /// <returns></returns>
      public static TValue TryGetValueWithDefault<TValue>(string key, TValue defaultValue)
      {
         TValue retval = defaultValue;
         if ( Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey(key) )
         {
            object value = Windows.Storage.ApplicationData.Current.LocalSettings.Values[key];
            if ( value is TValue )
            {
               retval = (TValue)value;
            }
         }
         return retval;
      }

      /// <summary>
      /// Add key/value or Update existing key with new value to the application settings
      /// </summary>
      /// <param name="key">Name of the key</param>
      /// <param name="value">New or updated value</param>
      /// <returns></returns>
      public static bool AddOrUpdateValue(string key, Object value)
      {
         bool valueChanged = false;
         if ( Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey(key) )
         {
            if ( Windows.Storage.ApplicationData.Current.LocalSettings.Values[key] != value )
            { // set to the value if it is different

               Windows.Storage.ApplicationData.Current.LocalSettings.Values[key] = value;
               valueChanged = true;
            }
         }
         else
         { // key is not in dictionary, create it

            Windows.Storage.ApplicationData.Current.LocalSettings.Values.Add(key, value);
            valueChanged = true;
         }
         return valueChanged;
      }

      /// <summary>
      /// Explicit Save. Not needed as Save will automatically be called at appInstance exit
      /// </summary>
      public static void Save()
      {
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.IO.IsolatedStorage.IsolatedStorageSettings.Save was not upgraded
         Windows.Storage.ApplicationData.Current.LocalSettings.Save();
      }

   }

}