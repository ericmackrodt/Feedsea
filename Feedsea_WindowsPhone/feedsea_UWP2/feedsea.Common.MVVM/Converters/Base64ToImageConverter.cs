using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace feedsea.Common.MVVM.Converters
{

   public class Base64ToImageConverter
      : Windows.UI.Xaml.Data.IValueConverter
   {

      public object Convert(object value, Type targetType, object parameter, System.String culture)
      {
         if ( string.IsNullOrWhiteSpace(value as string) )
            return "../Assets/Icons/source-icon.png";
         var imageBytes = System.Convert.FromBase64String(value as string);
         Windows.UI.Xaml.Media.Imaging.BitmapImage bitmapImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
         using ( MemoryStream ms = new MemoryStream(imageBytes) )
         {
            bitmapImage.SetSource(ms.AsRandomAccessStream());
            return bitmapImage;
         }
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.String culture)
      {
         throw new NotImplementedException();
      }

   }

}