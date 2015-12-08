using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace feedsea.Common.MVVM.Converters
{

   public class HasStringToVisibility
      : Windows.UI.Xaml.Data.IValueConverter
   {

      public object Convert(object value, Type targetType, object parameter, System.String culture)
      {
         return string.IsNullOrWhiteSpace((string)value) ? Windows.UI.Xaml.Visibility.Collapsed : Windows.UI.Xaml.Visibility.Visible;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.String culture)
      {
         throw new NotImplementedException();
      }

   }

}