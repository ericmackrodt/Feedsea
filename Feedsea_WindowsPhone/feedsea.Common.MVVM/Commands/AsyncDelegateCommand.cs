﻿using System;
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
    public class AsyncDelegateCommand : AsyncDelegateCommand<object>
    {
        public AsyncDelegateCommand(Func<object, Task> execute)
            : this(execute, null)
        {
        }

        public AsyncDelegateCommand(Func<object, Task> asyncExecute,
                       Predicate<object> canExecute)
            :base(asyncExecute, canExecute)
        {
        }
    }

    public class AsyncDelegateCommand<T> : ICommand
    {
        protected readonly Predicate<object> _canExecute;
        protected Func<T, Task> _asyncExecute;

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

        public AsyncDelegateCommand(Func<T, Task> execute)
            : this(execute, null)
        {
        }

        public AsyncDelegateCommand(Func<T, Task> asyncExecute,
                       Predicate<object> canExecute)
        {
            _asyncExecute = asyncExecute;
            _canExecute = canExecute;
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
            await _asyncExecute(parameter);
        }
    }
}
