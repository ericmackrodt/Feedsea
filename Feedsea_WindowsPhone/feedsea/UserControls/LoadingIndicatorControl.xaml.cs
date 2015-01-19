using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;

namespace feedsea.UserControls
{
    public partial class LoadingIndicatorControl : UserControl
    {
        private bool _loadingState = false;

        public LoadingIndicatorControl()
        {
            InitializeComponent();
        }

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register(
                "IsBusy",
                typeof(bool),
                typeof(LoadingIndicatorControl),
                new PropertyMetadata(false, OnIsBusyChanged));

        private static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;

            var control = (d as LoadingIndicatorControl);
            if ((bool)e.NewValue)
            {
                control.StartLoading();
            }
            else
            {
                control.EndLoading(null);
            }
        }

        public void StartLoading()
        {
            if (_loadingState) return;

            _loadingState = true;

            Visibility = System.Windows.Visibility.Visible;
            SpinningAnimation.Begin();
            OpeningAnimation.Begin();
        }

        public void StartLoading(string message)
        {
            txtLoading.Text = message;
            StartLoading();
        }

        public void EndLoading(Action done)
        {
            if (!_loadingState) return;

            ClosingAnimation.Begin();
            ClosingAnimation.Completed += (s, ev) =>
            {
                SpinningAnimation.Stop();
                Visibility = Visibility.Collapsed;
                _loadingState = false;
                if (done != null)
                    done();
            };
        }

        public void EndLoading(string message, Action done)
        {
            DoneAnimation.Begin();
            txtLoading.Text = message;
            ClosingAnimation.Completed += (s, ev) =>
            {
                SpinningAnimation.Stop();
                Visibility = Visibility.Collapsed;
                if (done != null)
                    done();
            };
        }
    }
}
