using feedsea.Common.Providers;
using feedsea.Common.Providers.Data;
using feedsea.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace feedsea.Common.Converters
{
    public class SourceToTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var source = value as INewsSource;

            if (source == null) return null;

            var isCategory = source is CategoryData;
            var isOwn = isCategory && (source as CategoryData).Own;

            if (isOwn)
                return AppResources.ResourceManager.GetString(source.Name);

            return source.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
