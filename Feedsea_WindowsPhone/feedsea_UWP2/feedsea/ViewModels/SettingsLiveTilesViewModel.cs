using feedsea.BackgroundAgent.Common;
using feedsea.Common;
using feedsea.Common.MVVM;
using feedsea.Common.MVVM.Commands;
using feedsea.Resources;
using feedsea.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace feedsea.ViewModels
{

   public class SettingsLiveTilesViewModel
      : AViewModel<SettingsLiveTilesViewModel>
   {
      private ILiveTileSettings _settings;
      private ITilePin _tiles;
      private IFullLoadingService _loadingService;
      private bool _tileTypeChanged;

      [IgnoreDataMember]
      public bool LiveTilesEnabled
      {
         get
         {
            return _settings.LiveTileEnabledSetting;
         }
         set
         {
            _settings.LiveTileEnabledSetting = value;
            NotifyChanged(o => o.LiveTilesEnabled);
         }
      }

      [IgnoreDataMember]
      public int SelectedTileMode
      {
         get
         {
            return (int)_settings.TileModeSetting;
         }
         set
         {
            _tileTypeChanged = _settings.TileModeSetting != (TileMode)value;
            _settings.TileModeSetting = (TileMode)value;
            NotifyChanged(o => o.SelectedTileMode);
         }
      }

      private ICommand _toggleLiveTilesCommand;

      public ICommand ToggleLiveTilesCommand
      {
         get
         {
            return _toggleLiveTilesCommand;
         }
      }


      public SettingsLiveTilesViewModel(ILiveTileSettings liveTileSettings, ITilePin tilePin, IFullLoadingService loadingService, IConnectionVerify connectionVerify)
      {
         _settings = liveTileSettings;
         _tiles = tilePin;
         _loadingService = loadingService;
         this.connection = connectionVerify;
         _toggleLiveTilesCommand = new RelayCommand(ToggleLiveTiles);
      }

      private async void ToggleLiveTiles()
      {
         LiveTilesEnabled = !LiveTilesEnabled;
         await SetLiveTileValue(LiveTilesEnabled);
      }

      private static async System.Threading.Tasks.Task SetLiveTileValue(bool value)
      {
         var bgAgnt = new BackgroundAgentController();
         bgAgnt.StartPeriodicAgent(value);
         if ( !value )
         {
            var tile = new FlipTileData()
               {
                  SmallBackgroundImage = new Uri(new Uri("ms-appx://"), "Assets\\Tiles\\MainTileSmall.png"), BackBackgroundImage = new Uri(new Uri("ms-appx://"), "None"), WideBackBackgroundImage = new Uri(new Uri("ms-appx://"), "None"), BackgroundImage = new Uri(new Uri("ms-appx://"), "Assets\\Tiles\\MainTileMedium.png"), WideBackgroundImage = new Uri(new Uri("ms-appx://"), "Assets\\Tiles\\MainTileLarge.png")
               };
            await tile.UpdateAsync();
         }
      }

      public async Task ExitSettingsActions()
      {
         if ( !_tileTypeChanged )
            return ;
         _loadingService.StartLoading(AppResources.Msg_UpdatingLiveTiles);
         await ConnectionVerifyCall(async () =>
            {
               await _tiles.UpdateLiveTiles();
            });
         _loadingService.EndLoading();
      }

   }

}