using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace feedsea.Common.MVVM.Converters
{
    public class DefaultStringConverter : FrameworkElement, IValueConverter
    {
        public string DefaultString
        {
            get { return (string)GetValue(DefaultStringProperty); }
            set { SetValue(DefaultStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DefaultString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultStringProperty =
            DependencyProperty.Register("DefaultString", typeof(string), typeof(DefaultStringConverter), new PropertyMetadata(""));

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string) ? DefaultString : (string)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
