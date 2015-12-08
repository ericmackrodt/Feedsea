using feedsea.BackgroundAgent.Common.ContentBuilder;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace feedsea.BackgroundAgent.Common.Helpers
{

   public static class TileHelper
   {

      public static string[] GetMainTiles(this string[] fileNames)
      {
         return fileNames.Where(o => o.IsMainTile()).ToArray();
      }

      public static bool IsMainTile(this string fileName)
      {
         var parts = fileName.Split(new string[] { "__" }, StringSplitOptions.RemoveEmptyEntries);
         if ( parts.Length != 2 )
            return false;
         Guid result = Guid.Empty;
         return Guid.TryParse(parts[0], out result);
      }

      public static void SaveJpeg(this ITileContentBuilder tile, TileSize tileSize, TileType type, string shellContentFolder, string fileNameSufix, string fileName, IsolatedStorageFile storage)
      {
         var width = tileSize == TileSize.Wide ? 691 : 336;
         var control = tile.GetContent();
         try
         {
            control.UpdateLayout();
            control.Measure(new Windows.Foundation.Size(width, 336));
            control.UpdateLayout();
            control.Arrange(new Windows.Foundation.Rect(0, 0, width, 336));
            if ( !storage.DirectoryExists(shellContentFolder) )
            {
               storage.CreateDirectory(shellContentFolder);
            }
            // Delete images from earlier execution
            var pattern = Path.Combine(shellContentFolder, string.Concat("*", fileNameSufix));
            var filesTodelete = storage.GetFileNames(pattern);
            if ( type == TileType.Main )
               filesTodelete = GetMainTiles(filesTodelete);
            foreach ( var file in filesTodelete )
            {
               storage.DeleteFile(Path.Combine(shellContentFolder, file));
            }
            var wb = new WriteableBitmap(width, 336);
            wb.Render(control, new Windows.UI.Xaml.Media.TranslateTransform());
            wb.Invalidate();
            using ( System.IO.Stream myFileStream = storage.CreateFile(fileName) )
            {
               // Encode WriteableBitmap object to a JPEG stream.
               wb.WritePNG(myFileStream);
               myFileStream.Dispose();
            }
            wb = null;
         }
         catch ( Exception ex )
         {
            // Log it
            System.Diagnostics.Debug.WriteLine("Exception in SaveJpeg: " + ex.ToString());
         }
      }

      public async static Task<byte[]> DownloadImage(this string p)
      {
         if ( p == null )
            return null;
         using ( var cli = new HttpClient() )
         {
            return await cli.GetByteArrayAsync(p);
         }
      }

      public static async Task<byte[]> DownloadImage(this string p, HttpClient cli)
      {
         if ( p == null )
            return null;
         return await cli.GetByteArrayAsync(p);
      }

      public static string GetLiveTileBackUrl(this string imageUrl)
      {
         if ( imageUrl == null )
            return null;
         var imageUrlFormat = @"http://images.weserv.nl/?url={0}&h=119&w=230&q=30&t=square&output=png";
         return string.Format(imageUrlFormat, imageUrl.RemoveProtocol());
      }

      public static string GetFaviconUrl(this string p)
      {
         if ( p == null )
            return null;
         var imageUrlFormat = @"http://www.google.com/s2/favicons?domain={0}";
         return string.Format(imageUrlFormat, p);
      }

      public static string RemoveProtocol(this string url)
      {
         if ( string.IsNullOrWhiteSpace(url) )
            return null;
         var regex = @"^(?<protocol>(sm|ht|f)tp(s)?://)?(?<noProtocol>(?<domainFull>((?<www>[^/\.]{2,15})[\.])?(?<domain>(?<siteName>[^/\.]+)[\.](?<type>[aA-zZ]{2,3})([\.](?<country>[aA-zZ]{2,3}))?))[/]?(.+)?)";
         var noProtocol = System.Text.RegularExpressions.Regex.Match(url, regex);
         return noProtocol.Groups["noProtocol"].Value;
      }

      public static void SetSource(this Windows.UI.Xaml.Media.Imaging.BitmapImage image, byte[] buffer)
      {
         using ( var str = new MemoryStream() )
         {
            str.Write(buffer, 0, buffer.Length);
            image.SetSource(str.AsRandomAccessStream());
            str.Dispose();
         }
      }

   }

}