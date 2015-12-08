using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace feedsea.Common
{

   public class Clip
   {

      public static bool GetToBounds(Windows.UI.Xaml.DependencyObject depObj)
      {
         return (bool)depObj.GetValue(ToBoundsProperty);
      }

      public static void SetToBounds(Windows.UI.Xaml.DependencyObject depObj, bool clipToBounds)
      {
         depObj.SetValue(ToBoundsProperty, clipToBounds);
      }

      /// <summary>
      /// Identifies the ToBounds Dependency Property.
      /// <summary>
      public static readonly Windows.UI.Xaml.DependencyProperty ToBoundsProperty = Windows.UI.Xaml.DependencyProperty.RegisterAttached("ToBounds", typeof(bool), typeof(Clip), new PropertyMetadata(false, OnToBoundsPropertyChanged));

      private static void OnToBoundsPropertyChanged(Windows.UI.Xaml.DependencyObject d, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e)
      {
         Windows.UI.Xaml.FrameworkElement fe = d as Windows.UI.Xaml.FrameworkElement;
         if ( fe != null )
         {
            ClipToBounds(fe);
            // whenever the element which this property is attached to is loaded
            // or re-sizes, we need to update its clipping geometry
            fe.Loaded += new Windows.UI.Xaml.RoutedEventHandler(fe_Loaded);
            fe.SizeChanged += new Windows.UI.Xaml.SizeChangedEventHandler(fe_SizeChanged);
         }
      }

      /// <summary>
      /// Creates a rectangular clipping geometry which matches the geometry of the
      /// passed element
      /// </summary>
      private static void ClipToBounds(Windows.UI.Xaml.FrameworkElement fe)
      {
         if ( GetToBounds(fe) )
         {
            //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.FrameworkElement.Clip was not upgraded
            //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.UIElement.Clip was not upgraded
            fe.Clip = new Windows.UI.Xaml.Media.RectangleGeometry()
               {
                  Rect = new Windows.Foundation.Rect(0, 0, fe.ActualWidth, fe.ActualHeight)
               };
         }
         else
         {
            //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.FrameworkElement.Clip was not upgraded
            //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.UIElement.Clip was not upgraded
            fe.Clip = null;
         }
      }

      static void fe_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
      {
         ClipToBounds(sender as Windows.UI.Xaml.FrameworkElement);
      }

      static void fe_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
      {
         ClipToBounds(sender as Windows.UI.Xaml.FrameworkElement);
      }

   }

}