using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Feedsea.Settings
{
    public abstract class SettingsBase : INotifyPropertyChanged
    {
        ApplicationData _appData;
        ApplicationDataContainer _settings;

        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsBase()
        {
            _appData = ApplicationData.Current;
            _settings = _appData.LocalSettings;
        }
        
        public T GetValueOrDefault<T>(T defaultValue, [CallerMemberName]string propertyName = "")
        {
            T value;

            var settingKey = GetSettingKey(propertyName);
            if (_settings.Values.Keys.Contains(settingKey))
            {
                value = (T)_settings.Values[settingKey];
            }
            else
            {
                value = defaultValue;
            }

            return value;
        }

        public void AddOrUpdateValue(object value, [CallerMemberName]string propertyName = "")
        {
            var settingKey = GetSettingKey(propertyName);

            if (_settings.Values[settingKey] != value)
            {
                // Store the new value
                _settings.Values[settingKey] = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string GetSettingKey(string key)
        {
            return string.Format("{0}_{1}", this.GetType().Name, key);
        }

        public void ClearSettings()
        {
            _settings.Values.Clear();
        }
    }
}
