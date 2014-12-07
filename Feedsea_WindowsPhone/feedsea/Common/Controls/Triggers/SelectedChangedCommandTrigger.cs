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
    public class SelectedChangedCommandTrigger : Behavior<LongListSelector>
    {
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(SelectedChangedCommandTrigger), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += OnSelectedChanged;
        }

        private void OnSelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Command == null || AssociatedObject.SelectedItem == null || !Command.CanExecute(AssociatedObject.SelectedItem)) return;

            Command.Execute(AssociatedObject.SelectedItem);
            AssociatedObject.SelectedItem = null;
        }
    }
}
