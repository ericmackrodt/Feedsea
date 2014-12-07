using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Settings
{
    public abstract class SettingsBase : ISettings
    {
        // Our settings
        protected IsolatedStorageSettings settings;
        public SettingsBase()
        {
            settings = IsolatedStorageSettings.ApplicationSettings;
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
            if (settings.Contains(settingKey))
            {
                // If the value has changed
                if (settings[settingKey] != value)
                {
                    // Store the new value
                    settings[settingKey] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                settings.Add(settingKey, value);
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
            if (settings.Contains(settingKey))
            {
                value = (T)settings[settingKey];
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
