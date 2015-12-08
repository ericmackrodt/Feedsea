using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace feedsea.BackgroundAgent.Common.ContentBuilder
{

   public interface ITileContentBuilder
   {

      void BuildFront(TileFace tile, TileMode mode, TileType type, string title, int? count = null);

      void BuildBack(TileFace tile, TileMode mode, TileType type);

      void SaveTile(TileSize tileSize, TileType type, string shellContentFolder, string fileNameSufix, string fileName, IsolatedStorageFile storage);

      Windows.UI.Xaml.Controls.Border GetContent();

   }

}