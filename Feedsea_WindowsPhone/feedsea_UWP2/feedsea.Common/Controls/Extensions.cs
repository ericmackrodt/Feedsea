using System;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using System.Windows.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using System.Collections.Generic;

namespace feedsea.Common.Controls
{

   public static class Extensions
   {

#region Animation
      /// <summary>
      /// Fades an element in over the speified time
      /// </summary>
      /// <param name="element">The element</param>
      public static void Show(this Windows.UI.Xaml.UIElement element)
      {
         Extensions.Show(element, 0, null);
      }

      /// <summary>
      /// Fades an element in over the speified time
      /// </summary>
      /// <param name="element">The element</param>
      /// <param name="duration">The total milliseconds for the fade</param>
      public static void Show(this Windows.UI.Xaml.UIElement element, int duration)
      {
         Extensions.Show(element, duration, null);
      }

      /// <summary>
      /// Fades an element in over the speified time
      /// </summary>
      /// <param name="element">The element</param>
      /// <param name="duration">The total milliseconds for the fade</param>
      /// <param name="callback">A callback function for when the animation is complete</param>
      public static void Show(this Windows.UI.Xaml.UIElement element, int duration, Action<Windows.UI.Xaml.UIElement> callback)
      {
         element.Visibility = Windows.UI.Xaml.Visibility.Visible;
         Extensions.SetOpacity(element, duration, 1.0d, (e) =>
            {
               if ( callback != null )
               {
                  callback(e);
               }
            });
      }

      /// <summary>
      /// Fades an element out over the speified time
      /// </summary>
      /// <param name="element">The element</param>
      public static void Hide(this Windows.UI.Xaml.UIElement element)
      {
         Extensions.Hide(element, 0, null);
      }

      /// <summary>
      /// Fades an element out over the speified time
      /// </summary>
      /// <param name="element">The element</param>
      /// <param name="duration">The total milliseconds for the fade</param>
      public static void Hide(this Windows.UI.Xaml.UIElement element, int duration)
      {
         Extensions.Hide(element, duration, null);
      }

      /// <summary>
      /// Fades an element out over the speified time
      /// </summary>
      /// <param name="element">The element</param>
      /// <param name="duration">The total milliseconds for the fade</param>
      /// <param name="callback">A callback function for when the animation is complete</param>
      public static void Hide(this Windows.UI.Xaml.UIElement element, int duration, Action<Windows.UI.Xaml.UIElement> callback)
      {
         Extensions.SetOpacity(element, duration, 0.0d, (e) =>
            {
               e.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
               if ( callback != null )
               {
                  callback(e);
               }
            });
      }

      /// <summary>
      /// Fades an element to the specified opacity over the speified time
      /// </summary>
      /// <param name="element">The element</param>
      /// <param name="duration">The total milliseconds for the fade</param>
      /// <param name="opacity">The desired opacity</param>
      /// <param name="callback">A callback function for when the animation is complete</param>
      private static void SetOpacity(this Windows.UI.Xaml.UIElement element, int duration, double opacity, Action<Windows.UI.Xaml.UIElement> callback)
      {
         Extensions.Animate(element, "Opacity", duration, opacity, callback);
      }

      /// <summary>
      /// Animates a UI element
      /// </summary>
      /// <param name="element">The element</param>
      /// <param name="property">The property to animate</param>
      /// <param name="duration">The duration of the animation in milliseconds</param>
      /// <param name="value">The value to animate to</param>
      public static void Animate(this Windows.UI.Xaml.UIElement element, string property, int duration, double value)
      {
         Extensions.Animate(element, property, duration, value, null);
      }

      /// <summary>
      /// Animates a UI element
      /// </summary>
      /// <param name="element">The element</param>
      /// <param name="property">The property to animate</param>
      /// <param name="duration">The duration of the animation in milliseconds</param>
      /// <param name="value">The value to animate to</param>
      /// <param name="callback">A callback function for when the animation is complete</param>
      public static void Animate(this Windows.UI.Xaml.UIElement element, string property, int duration, double value, Action<Windows.UI.Xaml.UIElement> callback)
      {
         Windows.UI.Xaml.Media.Animation.Storyboard storyBoard = new Windows.UI.Xaml.Media.Animation.Storyboard();
         Windows.UI.Xaml.Media.Animation.DoubleAnimation animation = new Windows.UI.Xaml.Media.Animation.DoubleAnimation()
            {
               EnableDependentAnimation = true
            };
         animation.To = value;
         animation.Duration = new Windows.UI.Xaml.Duration(TimeSpan.FromMilliseconds(duration));
         if ( callback != null )
         {
            animation.Completed += delegate
               {
                  callback(element);
               };
         }
         Windows.UI.Xaml.Media.Animation.Storyboard.SetTarget(animation, element);
         Windows.UI.Xaml.Media.Animation.Storyboard.SetTargetProperty(animation, new PropertyPath(property).Path);
         storyBoard.Children.Add(animation);
         storyBoard.Begin();
      }
#endregion

   }

}