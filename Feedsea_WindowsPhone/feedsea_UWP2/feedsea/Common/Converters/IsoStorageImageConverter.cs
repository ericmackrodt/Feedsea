using feedsea.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace feedsea.Common.Converters
{

   public class IsoStorageImageConverter
      : Windows.UI.Xaml.Data.IValueConverter
   {

      public object Convert(object value, Type targetType, object parameter, System.String culture)
      {
         if ( parameter == null )
            throw new Exception("You have to set a default image as parameter!");
         Windows.UI.Xaml.Media.Imaging.BitmapImage bi = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
         using ( IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication() )
         {
            var fileName = string.Concat(InternalSettings.FolderFavicons, "/", string.Format(InternalSettings.SourceFaviconName, value));
            if ( !myIsolatedStorage.FileExists(fileName) )
               bi.UriSource = new Uri(new Uri("ms-appx://"), (string)parameter);
            else
            {
               using ( System.IO.Stream fileStream = myIsolatedStorage.OpenFile(fileName, FileMode.Open, FileAccess.Read) )
               {
                  bi.SetSource(fileStream.AsRandomAccessStream());
               }
            }
         }
         return bi;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.String culture)
      {
         throw new NotImplementedException();
      }

   }

}