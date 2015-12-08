using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;

namespace Feedsea.Common.Converters
{
    public class TimeAgoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return BuildShowDate((DateTime)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public string BuildShowDate(DateTime date)
        {
            var resourceLoader = new ResourceLoader();

            var span = (DateTime.Now - date);

            if (span.Days > 0)
                return string.Concat(span.Days, " ", span.Days > 1 ? resourceLoader.GetString("ArticleTemplate_AgoDays/Text") : resourceLoader.GetString("ArticleTemplate_AgoDay/Text"));
            if (span.Hours > 0)
                return string.Concat(span.Hours, " ", span.Hours > 1 ? resourceLoader.GetString("ArticleTemplate_AgoHours/Text") : resourceLoader.GetString("ArticleTemplate_AgoHour/Text"));
            if (span.Minutes > 0)
                return string.Concat(span.Minutes, " ", span.Minutes > 1 ? resourceLoader.GetString("ArticleTemplate_AgoMinutes/Text") : resourceLoader.GetString("ArticleTemplate_AgoMinute/Text"));
            if (span.Seconds >= 0)
                return string.Concat(span.Seconds, " ", span.Seconds > 1 ? resourceLoader.GetString("ArticleTemplate_AgoSeconds/Text") : resourceLoader.GetString("ArticleTemplate_AgoSecond/Text"));

            return "";
        }
    }
}
