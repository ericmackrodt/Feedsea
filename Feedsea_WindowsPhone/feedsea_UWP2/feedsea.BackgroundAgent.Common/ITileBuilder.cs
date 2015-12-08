using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.BackgroundAgent.Common
{

   public enum TileSize
   {Wide,
   Medium}

   public interface ITileBuilder
   {

      void SetTileData(TileFace data, TileMode mode, string title, bool isFront, int? count);

      void SaveTile(TileSize tileSize, TileType type, string shellContentFolder, string fileNameSufix, string fileName, IsolatedStorageFile storage);

   }

}