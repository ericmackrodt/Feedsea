using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Navigation;

namespace feedsea.Common.Controls
{
    public class NavigateOnTapTrigger : Behavior<FrameworkElement>
    {
        public int SourcesPageIndex { get; set; }

        public string NavigateTo { get; set; }
        public bool AssociateToDataContext { get; set; }

        public object NavigationObject
        {
            get { return (string)GetValue(NavigationObjectProperty); }
            set { SetValue(NavigationObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NavigationObjectProperty =
            DependencyProperty.Register("NavigationObject", typeof(string), typeof(NavigateOnTapTrigger), new PropertyMetadata(null, new PropertyChangedCallback(NavigationObjectPropertyChanged)));

        private static void NavigationObjectPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        public string NavigationParameter
        {
            get { return (string)GetValue(NavigationParameterProperty); }
            set { SetValue(NavigationParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NavigationParameterProperty =
            DependencyProperty.Register("NavigationParameter", typeof(string), typeof(NavigateOnTapTrigger), new PropertyMetadata(null));

        private static void NavigationParameterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        
        public string NavigationSecondParameter
        {
            get { return (string)GetValue(NavigationSecondParameterProperty); }
            set { SetValue(NavigationSecondParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NavigationSecondParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NavigationSecondParameterProperty =
            DependencyProperty.Register("NavigationSecondParameter", typeof(string), typeof(NavigateOnTapTrigger), new PropertyMetadata(null));
        
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Tap += AssociatedObject_Tap;
        }

        void AssociatedObject_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (AssociateToDataContext)
                PhoneApplicationService.Current.State["NavigationObject"] = AssociatedObject.DataContext;

            if (NavigationObject != null)
                PhoneApplicationService.Current.State["NavigationObject"] = NavigationObject;

            App.RootFrame.Navigate(new Uri(string.Format(NavigateTo, NavigationParameter, NavigationSecondParameter), UriKind.Relative));
        }
    }
}
