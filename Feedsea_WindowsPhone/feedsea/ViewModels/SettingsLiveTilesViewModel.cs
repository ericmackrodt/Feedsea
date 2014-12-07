using feedsea.BackgroundAgent.Common;
using feedsea.Common;
using feedsea.Common.MVVM;
using feedsea.Common.MVVM.Commands;
using feedsea.Resources;
using feedsea.Settings;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace feedsea.ViewModels
{
    public class SettingsLiveTilesViewModel : AViewModel<SettingsLiveTilesViewModel>
    {
        private ILiveTileSettings _settings;
        private ITilePin _tiles;
        private IFullLoadingService _loadingService;
        private bool _tileTypeChanged;

        [IgnoreDataMember]
        public bool LiveTilesEnabled
        {
            get { return _settings.LiveTileEnabledSetting; }
            set
            {
                _settings.LiveTileEnabledSetting = value;
                NotifyChanged(o => o.LiveTilesEnabled);
            }
        }

        [IgnoreDataMember]
        public int SelectedTileMode
        {
            get { return (int)_settings.TileModeSetting; }
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
            get { return _toggleLiveTilesCommand; }
        }

        public SettingsLiveTilesViewModel(
            ILiveTileSettings liveTileSettings,
            ITilePin tilePin,
            IFullLoadingService loadingService,
            IConnectionVerify connectionVerify)
        {
            _settings = liveTileSettings;
            _tiles = tilePin;
            _loadingService = loadingService;
            this.connection = connectionVerify;

            _toggleLiveTilesCommand = new RelayCommand(ToggleLiveTiles);
        }

        private void ToggleLiveTiles()
        {
            LiveTilesEnabled = !LiveTilesEnabled;
            SetLiveTileValue(LiveTilesEnabled);
        }

        private static void SetLiveTileValue(bool value)
        {
            var bgAgnt = new BackgroundAgentController();
            bgAgnt.StartPeriodicAgent(value);

            if (!value)
            {
                var tile = new FlipTileData()
                {
                    SmallBackgroundImage = new Uri("Assets\\Tiles\\MainTileSmall.png", UriKind.Relative),
                    BackBackgroundImage = new Uri("None", UriKind.Relative),
                    WideBackBackgroundImage = new Uri("None", UriKind.Relative),
                    BackgroundImage = new Uri("Assets\\Tiles\\MainTileMedium.png", UriKind.Relative),
                    WideBackgroundImage = new Uri("Assets\\Tiles\\MainTileLarge.png", UriKind.Relative)
                };

                ShellTile.ActiveTiles.First().Update(tile);
            }
        }

        public async Task ExitSettingsActions()
        {
            if (!_tileTypeChanged)
                return;

            _loadingService.StartLoading(AppResources.Msg_UpdatingLiveTiles);

            await ConnectionVerifyCall(async () =>
            {
                await _tiles.UpdateLiveTiles();
            });

            _loadingService.EndLoading();
        }
    }
}
