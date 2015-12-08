using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Settings
{

   public interface ISettings
   {

      bool AddOrUpdateValue(string Key, Object value);

      T GetValueOrDefault<T>(string Key, T defaultValue);

      void Save();

      event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
   }

}