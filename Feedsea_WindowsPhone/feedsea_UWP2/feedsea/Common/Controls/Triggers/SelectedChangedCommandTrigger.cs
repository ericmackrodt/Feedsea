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

   public class SelectedChangedCommandTrigger
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
      public static readonly Windows.UI.Xaml.DependencyProperty CommandProperty = Windows.UI.Xaml.DependencyProperty.Register("Command", typeof(ICommand), typeof(SelectedChangedCommandTrigger), new PropertyMetadata(null));

      protected override void OnAttached()
      {
         base.OnAttached();
         AssociatedObject.SelectionChanged += OnSelectedChanged;
      }

      private void OnSelectedChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
      {
         if ( Command == null || AssociatedObject.SelectedItem == null || !Command.CanExecute(AssociatedObject.SelectedItem) )
            return ;
         Command.Execute(AssociatedObject.SelectedItem);
         AssociatedObject.SelectedItem = null;
      }

   }

}