using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using feedsea.BackgroundAgent.Common.Helpers;

namespace feedsea.Common.Converters
{

   public class ImageByResolutionConverter
      : Windows.UI.Xaml.Data.IValueConverter
   {

      public object Convert(object value, Type targetType, object parameter, System.String culture)
      {
         var originalHeight = 250;
         var originalWidth = 430;
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Application.Host was not upgraded
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Application.Host was not upgraded
         var width = App.Current.Host.Content.ActualWidth;
         var ratio = width / originalWidth;
         var height = (int)(originalHeight * ratio);
         return value == null ? parameter : string.Format("http://images.weserv.nl/?url={0}?itok=hhErCWAa&h={1}&w={2}&q=30&t=square", (value as string).RemoveProtocol(), height, width);
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.String culture)
      {
         throw new NotImplementedException();
      }

   }

}