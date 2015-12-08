using feedsea.BackgroundAgent.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Settings
{

   public class LiveTileSettings
      : SettingsBase, ILiveTileSettings
   {
      const string LiveTileEnabledSettingKeyName = "LiveTileEnabledSetting";
      const bool LiveTileEnabledSettingDefault = true;

      public bool LiveTileEnabledSetting
      {
         get
         {
            return GetValueOrDefault<bool>(LiveTileEnabledSettingKeyName, LiveTileEnabledSettingDefault);
         }
         set
         {
            if ( AddOrUpdateValue(LiveTileEnabledSettingKeyName, value) )
            {
               Save();
            }
         }
      }

      const string TileModeSettingKeyName = "TileModeSetting";
      const TileMode TileModeSettingDefault = TileMode.Normal;

      public TileMode TileModeSetting
      {
         get
         {
            return GetValueOrDefault<TileMode>(TileModeSettingKeyName, TileModeSettingDefault);
         }
         set
         {
            if ( AddOrUpdateValue(TileModeSettingKeyName, value) )
            {
               Save();
            }
         }
      }

   }

}