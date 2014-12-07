using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace feedsea.Common.MVVM.Converters
{
    public class BoolToDefaultStringConverter : FrameworkElement, IValueConverter
    {
        public string DefaultString
        {
            get { return (string)GetValue(DefaultStringProperty); }
            set { SetValue(DefaultStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DefaultString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultStringProperty =
            DependencyProperty.Register("DefaultString", typeof(string), typeof(BoolToDefaultStringConverter), new PropertyMetadata(""));
        
        public bool UseDefaultValue
        {
            get { return (bool)GetValue(UseDefaultValueProperty); }
            set { SetValue(UseDefaultValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UseDefaultValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UseDefaultValueProperty =
            DependencyProperty.Register("UseDefaultValue", typeof(bool), typeof(BoolToDefaultStringConverter), new PropertyMetadata(false));



        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string) || UseDefaultValue ? DefaultString : (string)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
