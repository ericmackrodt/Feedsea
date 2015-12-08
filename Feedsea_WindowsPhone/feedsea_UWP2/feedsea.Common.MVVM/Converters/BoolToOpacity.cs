using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace feedsea.Common.MVVM.Converters
{

   public class BoolToOpacity
      : Windows.UI.Xaml.Data.IValueConverter
   {

      public object Convert(object value, Type targetType, object parameter, System.String culture)
      {
         return (double)((bool)value ? double.Parse((string)parameter, System.Globalization.CultureInfo.InvariantCulture) : (double)1.0);
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.String culture)
      {
         throw new NotImplementedException();
      }

   }

}