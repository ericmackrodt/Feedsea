using System;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using System.Windows.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace feedsea.Common.MVVM.Commands
{

   public class ParameterRelayCommand<T>
      : ICommand
   {
      private Action<T> _handler;

      public ParameterRelayCommand(Action<T> handler)
      {
         _handler = handler;
      }
      private bool _isEnabled;

      public bool IsEnabled
      {
         get
         {
            return _isEnabled;
         }
         set
         {
            if ( value != _isEnabled )
            {
               _isEnabled = value;
               if ( CanExecuteChanged != null )
               {
                  CanExecuteChanged(this, EventArgs.Empty);
               }
            }
         }
      }

      public bool CanExecute(object parameter)
      {
         return IsEnabled;
      }

      public event EventHandler CanExecuteChanged;

      public void Execute(object parameter)
      {
         _handler((T)parameter);
      }

   }

}