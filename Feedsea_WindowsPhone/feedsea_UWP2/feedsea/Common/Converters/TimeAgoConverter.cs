using feedsea.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace feedsea.Common.Converters
{

   public class TimeAgoConverter
      : Windows.UI.Xaml.Data.IValueConverter
   {

      public object Convert(object value, Type targetType, object parameter, System.String culture)
      {
         return BuildShowDate((DateTime)value);
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.String culture)
      {
         throw new NotImplementedException();
      }

      public string BuildShowDate(DateTime date)
      {
         var span = (DateTime.Now - date);
         if ( span.Days > 0 )
            return string.Concat(span.Days, " ", span.Days > 1 ? AppResources.AgoDays : AppResources.AgoDay);
         if ( span.Hours > 0 )
            return string.Concat(span.Hours, " ", span.Hours > 1 ? AppResources.AgoHours : AppResources.AgoHour);
         if ( span.Minutes > 0 )
            return string.Concat(span.Minutes, " ", span.Minutes > 1 ? AppResources.AgoMinutes : AppResources.AgoMinute);
         if ( span.Seconds >= 0 )
            return string.Concat(span.Seconds, " ", span.Seconds > 1 ? AppResources.AgoSeconds : AppResources.AgoSecond);
         return "";
      }

   }

}