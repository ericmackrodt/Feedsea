using feedsea.BackgroundAgent.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Settings
{

   public interface ILiveTileSettings
   {

      bool LiveTileEnabledSetting { get; set; }

      TileMode TileModeSetting { get; set; }

   }

}