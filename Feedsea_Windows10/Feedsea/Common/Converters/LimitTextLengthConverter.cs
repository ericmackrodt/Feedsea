using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Feedsea.Common.Converters
{
    public class LimitTextLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var text = WebUtility.HtmlDecode(Regex.Replace((string)value, @"<[^>]*>", string.Empty, RegexOptions.Singleline).Trim());
            var limit = parameter != null ? (int)parameter : 250;
            if (text.Length <= limit)
                return text;

            var reticence = "[...]";
            var txtLimited = text.Substring(0, limit - reticence.Length);
            var lastIndexOfSpace = txtLimited.LastIndexOf(' ');
            return string.Concat(txtLimited.Substring(0, lastIndexOfSpace + 1), reticence);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
