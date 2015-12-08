using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using System.Windows.Input;
using Microsoft.Xaml.Interactivity;

namespace feedsea.Common.Controls
{

   public class PivotSourceLoadTrigger
      : Behavior<Pivot>
   {

      public int SourcesPageIndex { get; set; }

      public ICommand Command
      {
         get
         {
            return (ICommand)GetValue(CommandProperty);
         }
         set
         {
            SetValue(CommandProperty, value);
         }
      }

      // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
      public static readonly Windows.UI.Xaml.DependencyProperty CommandProperty = Windows.UI.Xaml.DependencyProperty.Register("Command", typeof(ICommand), typeof(PivotSourceLoadTrigger), new PropertyMetadata(null, new PropertyChangedCallback(CommandPropertyChanged)));

      private static void CommandPropertyChanged(Windows.UI.Xaml.DependencyObject d, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e)
      {
      }

      protected override void OnAttached()
      {
         base.OnAttached();
         AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
      }

      private void AssociatedObject_SelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
      {
         if ( AssociatedObject.SelectedIndex == SourcesPageIndex && Command.CanExecute(null) )
            Command.Execute(null);
      }

      protected override void OnDetaching()
      {
         base.OnDetaching();
         AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
      }

   }

}