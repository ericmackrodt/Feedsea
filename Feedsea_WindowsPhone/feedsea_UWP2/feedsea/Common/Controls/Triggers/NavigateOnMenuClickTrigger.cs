using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml.Navigation;

namespace feedsea.Common.Controls
{

   public class NavigateOnMenuClickTrigger
      : Behavior<MenuItem>
   {

      public int SourcesPageIndex { get; set; }

      public string NavigateTo { get; set; }

      public bool AssociateToDataContext { get; set; }

      public object NavigationObject
      {
         get
         {
            return (string)GetValue(NavigationObjectProperty);
         }
         set
         {
            SetValue(NavigationObjectProperty, value);
         }
      }

      // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
      public static readonly Windows.UI.Xaml.DependencyProperty NavigationObjectProperty = Windows.UI.Xaml.DependencyProperty.Register("NavigationObject", typeof(string), typeof(NavigateOnMenuClickTrigger), new PropertyMetadata(null, new PropertyChangedCallback(NavigationObjectPropertyChanged)));

      private static void NavigationObjectPropertyChanged(Windows.UI.Xaml.DependencyObject d, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e)
      {
      }

      public string NavigationParameter
      {
         get
         {
            return (string)GetValue(NavigationParameterProperty);
         }
         set
         {
            SetValue(NavigationParameterProperty, value);
         }
      }

      // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
      public static readonly Windows.UI.Xaml.DependencyProperty NavigationParameterProperty = Windows.UI.Xaml.DependencyProperty.Register("NavigationParameter", typeof(string), typeof(NavigateOnMenuClickTrigger), new PropertyMetadata(null));

      private static void NavigationParameterPropertyChanged(Windows.UI.Xaml.DependencyObject d, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e)
      {
      }

      public string NavigationSecondParameter
      {
         get
         {
            return (string)GetValue(NavigationSecondParameterProperty);
         }
         set
         {
            SetValue(NavigationSecondParameterProperty, value);
         }
      }

      // Using a DependencyProperty as the backing store for NavigationSecondParameter.  This enables animation, styling, binding, etc...
      public static readonly Windows.UI.Xaml.DependencyProperty NavigationSecondParameterProperty = Windows.UI.Xaml.DependencyProperty.Register("NavigationSecondParameter", typeof(string), typeof(NavigateOnMenuClickTrigger), new PropertyMetadata(null));

      protected override void OnAttached()
      {
         base.OnAttached();
         AssociatedObject.Click += AssociatedObject_Click;
      }

      void AssociatedObject_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
      {
         if ( AssociateToDataContext )
            Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().State["NavigationObject"] = AssociatedObject.DataContext;
         if ( NavigationObject != null )
            Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().State["NavigationObject"] = NavigationObject;
         App.RootFrame.Navigate(new Uri(new Uri("ms-appx://"), string.Format(NavigateTo, NavigationParameter, NavigationSecondParameter)));
      }

   }

}