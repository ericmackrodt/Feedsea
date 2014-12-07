using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace feedsea.Common.MVVM.Commands
{
    public class ConnectionVerifyAsyncCommand : ConnectionVerifyAsyncCommand<object>
    {
        public ConnectionVerifyAsyncCommand(Func<object, Task> execute, IConnectionVerify connectionVerify)
            : this(execute, null, connectionVerify)
        {
        }

        public ConnectionVerifyAsyncCommand(Func<object, Task> asyncExecute,
                       Predicate<object> canExecute, IConnectionVerify connectionVerify)
            :base(asyncExecute, canExecute, connectionVerify)
        {
        }
    }

    public class ConnectionVerifyAsyncCommand<T> : ICommand
    {
        protected readonly Predicate<object> _canExecute;
        protected Func<T, Task> _asyncExecute;
        protected IConnectionVerify _connectionVerify;

        public event EventHandler CanExecuteChanged;

        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    if (CanExecuteChanged != null)
                    {
                        CanExecuteChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        public ConnectionVerifyAsyncCommand(Func<T, Task> execute, IConnectionVerify connectionVerify)
            : this(execute, null, connectionVerify)
        {
        }

        public ConnectionVerifyAsyncCommand(Func<T, Task> asyncExecute,
                       Predicate<object> canExecute, IConnectionVerify connectionVerify)
        {
            _asyncExecute = asyncExecute;
            _canExecute = canExecute;
            _connectionVerify = connectionVerify;
            IsEnabled = true;
        }

        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        public async void Execute(object parameter)
        {
            await ExecuteAsync((T)parameter);
        }

        protected virtual async Task ExecuteAsync(T parameter)
        {
            if (!_connectionVerify.HasInternetConnection())
            {
                _connectionVerify.ShowNoConnectionMessage();
                return;
            }

            try
            {
                await _asyncExecute(parameter);
            }
            catch (Exception ex)
            {
                _connectionVerify.VerifyConnectionException(ex);
            }
        }
    }
}
