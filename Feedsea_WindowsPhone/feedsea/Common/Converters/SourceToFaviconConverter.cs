using feedsea.Common.Providers;
using feedsea.Common.Providers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace feedsea.Common.Converters
{
    public class SourceToFaviconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var source = value as INewsSource;

            if (source == null) return null;

            var isCategory = source is CategoryData;
            var isOwn = isCategory && (source as CategoryData).Own;

            var id = isCategory ? null : (source as SubscriptionData).Link;
            var alternative = isOwn ? "../Assets/Icons/" + source.Name + ".png" : "../Assets/Icons/source-icon.png";

            return id == null ? alternative : string.Concat("http://www.google.com/s2/favicons?domain=", id);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
