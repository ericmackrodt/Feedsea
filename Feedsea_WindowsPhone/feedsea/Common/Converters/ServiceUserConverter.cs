using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace feedsea.Common.Converters
{
    public class ServiceUserConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var param = (string)parameter;

            if (param.ToLower() == "twitter")
                return string.Concat("{0} - @{1}", value, parameter);

            return string.Concat("{0} - {1}", value, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
