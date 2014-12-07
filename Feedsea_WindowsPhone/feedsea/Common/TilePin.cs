using feedsea.BackgroundAgent.Common;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common
{
    public class TilePin : ITilePin
    {
        public async Task<bool> PinTile(LiveTileData data)
        {
            ShellTile tiletopin = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(data.NavigationUri.ToString()));
            if (tiletopin == null)
            {
                var manager = new BackgroundAgentManager();

                await manager.GetImages(data);
                using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    var tile = manager.BuildTiles(data, storage);
                    ShellTile.Create(data.NavigationUri, tile, true);
                }

                return true;
            }
            else
                return false;
        }

        public async Task UpdateLiveTiles()
        {
            var manager = new BackgroundAgentManager();
            await manager.DownloadBaseData();
            var data = await manager.DownloadTileData();

            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                foreach (var t in data)
                {
                    t.Key.Update(manager.BuildTiles(t.Value, storage));
                }
            }
        }
    }
}
