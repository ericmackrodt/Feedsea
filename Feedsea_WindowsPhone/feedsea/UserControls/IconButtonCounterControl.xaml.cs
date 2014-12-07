using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;

namespace feedsea.UserControls
{
    public partial class IconButtonCounterControl : UserControl
    {
        public event RoutedEventHandler Click;

        public string NavigateTo { get; set; }

        public ImageSource IconSource
        {
            get
            {
                return imgIcon.Source;
            }
            set
            {
                imgIcon.Source = value;
            }
        }
        
        public int Counter
        {
            get { return (int)GetValue(CounterProperty); }
            set { SetValue(CounterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Counter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CounterProperty =
            DependencyProperty.Register("Counter", typeof(int), typeof(IconButtonCounterControl), new PropertyMetadata(0, CounterPropertyChanged));

        private static void CounterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;

            var control = (d as IconButtonCounterControl);

            var value = (int)e.NewValue;

            if (value > 0)
            {
                control.txtCounter.Text = value.ToString();
                control.brdCounter.Visibility = Visibility.Visible;
            }
            else
                control.brdCounter.Visibility = Visibility.Collapsed;
        }
        
        public IconButtonCounterControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Click != null)
                Click(sender, e);
        }
    }
}
