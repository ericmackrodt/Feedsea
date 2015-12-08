using System;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using System.Windows.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace feedsea.Common.Controls
{

   /// <summary>
   /// Container for swipable controls
   /// </summary>
   public class SwipeContentControl
      : Windows.UI.Xaml.Controls.ContentControl
   {
#region Events
      /// <summary>
      /// Event raised when the control is swiped to the left
      /// </summary>
      public event EventHandler SwipeLeft;

      /// <summary>
      /// Raises the SwipeLeft event
      /// </summary>
      protected virtual void OnSwipeLeft()
      {
         if ( this.SwipeLeft != null )
         {
            this.SwipeLeft(this, new EventArgs());
         }
         if ( this.SwipeLeftCommand != null && this.SwipeLeftCommand.CanExecute(null) )
         {
            this.SwipeLeftCommand.Execute(null);
         }
      }

      /// <summary>
      /// Event raised when the control is swiped to the right
      /// </summary>
      public event EventHandler SwipeRight;

      /// <summary>
      /// Raises the SwipeRight event
      /// </summary>
      protected virtual void OnSwipeRight()
      {
         if ( this.SwipeRight != null )
         {
            this.SwipeRight(this, new EventArgs());
         }
         if ( this.SwipeRightCommand != null && this.SwipeRightCommand.CanExecute(null) )
         {
            this.SwipeRightCommand.Execute(null);
         }
      }
#endregion

#region Properties
      /// <summary>
      /// Dependency property for whether a left swipe is allowed
      /// </summary>
      public static readonly Windows.UI.Xaml.DependencyProperty CanSwipeLeftProperty = Windows.UI.Xaml.DependencyProperty.Register("CanSwipeLeft", typeof(bool), typeof(SwipeContentControl), new PropertyMetadata(false));

      /// <summary>
      /// Gets or sets whether a left swipe is allowed
      /// </summary>
      public bool CanSwipeLeft
      {
         get
         {
            return (bool)GetValue(CanSwipeLeftProperty);
         }
         set
         {
            SetValue(CanSwipeLeftProperty, value);
         }
      }

      /// <summary>
      /// Dependency property for whether a right swipe is allowed
      /// </summary>
      public static readonly Windows.UI.Xaml.DependencyProperty CanSwipeRightProperty = Windows.UI.Xaml.DependencyProperty.Register("CanSwipeRight", typeof(bool), typeof(SwipeContentControl), new PropertyMetadata(false));

      /// <summary>
      /// Gets or sets whether a right swipe is allowed
      /// </summary>
      public bool CanSwipeRight
      {
         get
         {
            return (bool)GetValue(CanSwipeRightProperty);
         }
         set
         {
            SetValue(CanSwipeRightProperty, value);
         }
      }

      public ICommand SwipeLeftCommand
      {
         get
         {
            return (ICommand)GetValue(SwipeLeftCommandProperty);
         }
         set
         {
            SetValue(SwipeLeftCommandProperty, value);
         }
      }

      // Using a DependencyProperty as the backing store for SwipeLeftCommand.  This enables animation, styling, binding, etc...
      public static readonly Windows.UI.Xaml.DependencyProperty SwipeLeftCommandProperty = Windows.UI.Xaml.DependencyProperty.Register("SwipeLeftCommand", typeof(ICommand), typeof(SwipeContentControl), new PropertyMetadata(null));

      public ICommand SwipeRightCommand
      {
         get
         {
            return (ICommand)GetValue(SwipeRightCommandProperty);
         }
         set
         {
            SetValue(SwipeRightCommandProperty, value);
         }
      }

      // Using a DependencyProperty as the backing store for SwipeRightCommand.  This enables animation, styling, binding, etc...
      public static readonly Windows.UI.Xaml.DependencyProperty SwipeRightCommandProperty = Windows.UI.Xaml.DependencyProperty.Register("SwipeRightCommand", typeof(ICommand), typeof(SwipeContentControl), new PropertyMetadata(null));
#endregion

      /// <summary>
      /// Creates a new instance
      /// </summary>
      public SwipeContentControl()
      {
         this.RenderTransform = new Windows.UI.Xaml.Media.TranslateTransform();
         this.DefaultStyleKey = typeof(Windows.UI.Xaml.Controls.ContentControl);
         //GestureListener listener = GestureService.GetGestureListener(this);
         //listener.DragDelta += DragDelta;
         //listener.DragCompleted += DragCompleted;
         this.ManipulationDelta += SwipeContentControl_ManipulationDelta;
         this.ManipulationCompleted += SwipeContentControl_ManipulationCompleted;
      }

      void SwipeContentControl_ManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
      {
         Windows.UI.Xaml.Media.TranslateTransform transform = (Windows.UI.Xaml.Media.TranslateTransform)this.RenderTransform;
         if ( (transform.X > 120.0d) && this.CanSwipeRight )
         {
            /* Calculate the offset */
            double offset = this.ActualWidth + 20.0d;
            /* Fade out and shift to the right */
            this.Animate("Opacity", 100, 0.0d);
            this.Animate("(RenderTransform).(TranslateTransform.X)", 100, offset, (element) =>
               {
                  /* Raise the event so the containing page can set the new content */
                  this.OnSwipeRight();
                  /* Delay 100ms with an 'empty' animation for a smooth animation */
                  this.Animate("(RenderTransform).(TranslateTransform.X)", 100, offset, (element2) =>
                     {
                        /* Move the control to the left hand side - we need to retrieve the 
                        * transform again here as it no longer points to the same object. I
                        * haven't figured out if there is a reason for this or if it is a bug. */
                        transform = (Windows.UI.Xaml.Media.TranslateTransform)this.RenderTransform;
                        transform.X = -offset;
                        /* Fade in and slide in from the left */
                        this.Animate("Opacity", 100, 1.0d);
                        this.Animate("(RenderTransform).(TranslateTransform.X)", 150, 0.0d);
                     });
               });
         }
         else if ( (transform.X < -120.0d) && this.CanSwipeLeft )
         {
            /* Same as above but in reverse */
            double offset = this.ActualWidth + 20.0d;
            this.Animate("Opacity", 100, 0.0d);
            this.Animate("(RenderTransform).(TranslateTransform.X)", 100, -offset, (element) =>
               {
                  this.OnSwipeLeft();
                  /* Delay 100ms for a smooth animation */
                  this.Animate("(RenderTransform).(TranslateTransform.X)", 100, -offset, (element2) =>
                     {
                        transform = (Windows.UI.Xaml.Media.TranslateTransform)this.RenderTransform;
                        transform.X = offset;
                        this.Animate("Opacity", 100, 1.0d);
                        this.Animate("(RenderTransform).(TranslateTransform.X)", 150, 0.0d);
                     });
               });
         }
         else if ( transform.X != 0.0d )
         {
            this.Animate("(RenderTransform).(TranslateTransform.X)", 200, 0.0d);
         }
      }

      void SwipeContentControl_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
      {
         if ( ((e.Delta.Translation.X < 0) && this.CanSwipeLeft) || ((e.Delta.Translation.X > 0) && this.CanSwipeRight) )
         {
            Windows.UI.Xaml.Media.TranslateTransform transform = (Windows.UI.Xaml.Media.TranslateTransform)this.RenderTransform;
            if ( transform.X == 0 )
            {
               if ( (e.Delta.Translation.X > -15) && (e.Delta.Translation.X < 15) )
               {
                  return ;
               }
            }
            transform.X += e.Delta.Translation.X;
         }
      }
#region Swipe
#endregion

   }

}