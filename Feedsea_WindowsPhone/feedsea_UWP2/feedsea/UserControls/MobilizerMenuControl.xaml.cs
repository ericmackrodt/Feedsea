using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using feedsea.Common;
using Windows.UI.Xaml.Controls.Primitives;

namespace feedsea.UserControls
{

   public partial class MobilizerMenuControl
      : Windows.UI.Xaml.Controls.UserControl
   {
      public event EventHandler<Mobilizers> MobilizerSelected;

      public Mobilizers MobilizerSelectedProperty
      {
         get
         {
            return (Mobilizers)GetValue(MyPropertyProperty);
         }
         set
         {
            SetValue(MyPropertyProperty, value);
         }
      }

      // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
      public static readonly Windows.UI.Xaml.DependencyProperty MyPropertyProperty = Windows.UI.Xaml.DependencyProperty.Register("MobilizerSelected", typeof(Mobilizers), typeof(MobilizerMenuControl), new PropertyMetadata(Mobilizers.Page));

      public MobilizerMenuControl()
      {
         InitializeComponent();
      }

      private void ListBox_SelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
      {
         MobilizerSelectedProperty = (Mobilizers)MobilizersList.SelectedIndex;
         if ( MobilizerSelected != null )
            MobilizerSelected(this, MobilizerSelectedProperty);
         MobilizersList.SelectedIndex = -1;
      }

      public void Close()
      {
         var parent = (this.Parent as Windows.UI.Xaml.Controls.Primitives.Popup);
         if ( parent == null )
         {
            return ;
         }
         ClosingAnimation.Completed += (s, e) =>
            {
               parent.IsOpen = false;
            };
         ClosingAnimation.Begin();
      }

      public void PopupOpenEvent()
      {
         var parent = (this.Parent as Windows.UI.Xaml.Controls.Primitives.Popup);
         if ( parent == null )
         {
            LayoutRoot.Opacity = 1;
            return ;
         }
         parent.Opened += (s, e) =>
            {
               OpeningAnimation.Begin();
            };
      }

   }

}