using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
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
        public static void Show(this UIElement element)
        {
            Extensions.Show(element, 0, null);
        }

        /// <summary>
        /// Fades an element in over the speified time
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="duration">The total milliseconds for the fade</param>
        public static void Show(this UIElement element, int duration)
        {
            Extensions.Show(element, duration, null);
        }

        /// <summary>
        /// Fades an element in over the speified time
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="duration">The total milliseconds for the fade</param>
        /// <param name="callback">A callback function for when the animation is complete</param>
        public static void Show(this UIElement element, int duration, Action<UIElement> callback)
        {
            element.Visibility = Visibility.Visible;
            Extensions.SetOpacity(element, duration, 1.0d, (e) =>
            {
                if (callback != null)
                {
                    callback(e);
                }
            });
        }

        /// <summary>
        /// Fades an element out over the speified time
        /// </summary>
        /// <param name="element">The element</param>
        public static void Hide(this UIElement element)
        {
            Extensions.Hide(element, 0, null);
        }

        /// <summary>
        /// Fades an element out over the speified time
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="duration">The total milliseconds for the fade</param>
        public static void Hide(this UIElement element, int duration)
        {
            Extensions.Hide(element, duration, null);
        }

        /// <summary>
        /// Fades an element out over the speified time
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="duration">The total milliseconds for the fade</param>
        /// <param name="callback">A callback function for when the animation is complete</param>
        public static void Hide(this UIElement element, int duration, Action<UIElement> callback)
        {
            Extensions.SetOpacity(element, duration, 0.0d, (e) =>
            {
                e.Visibility = Visibility.Collapsed;
                if (callback != null)
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
        private static void SetOpacity(this UIElement element, int duration, double opacity, Action<UIElement> callback)
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
        public static void Animate(this UIElement element, string property, int duration, double value)
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
        public static void Animate(this UIElement element, string property, int duration, double value, Action<UIElement> callback)
        {
            Storyboard storyBoard = new Storyboard();

            DoubleAnimation animation = new DoubleAnimation();
            animation.To = value;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(duration));

            if (callback != null)
            {
                animation.Completed += delegate { callback(element); };
            }

            Storyboard.SetTarget(animation, element);
            Storyboard.SetTargetProperty(animation, new PropertyPath(property));

            storyBoard.Children.Add(animation);
            storyBoard.Begin();
        }

        #endregion
    }
}
