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

   public class PaginationTrigger
      : Behavior<Windows.UI.Xaml.Controls.GridView>
   {

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
      public static readonly Windows.UI.Xaml.DependencyProperty CommandProperty = Windows.UI.Xaml.DependencyProperty.Register("Command", typeof(ICommand), typeof(PaginationTrigger), new PropertyMetadata(null, new PropertyChangedCallback(CommandPropertyChanged)));

      private static void CommandPropertyChanged(Windows.UI.Xaml.DependencyObject d, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e)
      {
      }

      protected override void OnAttached()
      {
         base.OnAttached();
         AssociatedObject.ItemRealized += AssociatedObject_ItemRealized;
      }

      private void AssociatedObject_ItemRealized(object sender, ItemRealizationEventArgs e)
      {
         var item = e.Container.Content;
         var items = AssociatedObject.ItemsSource;
         var index = items.IndexOf(item);
         if ( items.Count - index <= 1 )
         {
            if ( Command == null || !Command.CanExecute(null) )
               return ;
            Command.Execute(index);
         }
      }

      protected override void OnDetaching()
      {
         base.OnDetaching();
         AssociatedObject.ItemRealized -= AssociatedObject_ItemRealized;
      }

   }

}