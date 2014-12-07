using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace feedsea.UserControls
{
    public partial class FullSizeLoadingPanel : UserControl
    {
        public bool IsOpen { get; set; }

        public FullSizeLoadingPanel()
        {
            InitializeComponent();
        }

        public void SetMessage(string message)
        {
            txtLoading.Text = message;
        }

        public void Open()
        {
            IsOpen = true;
            Visibility = System.Windows.Visibility.Visible;
            OpeningAnimation.Begin();
            SpinningAnimation.Begin();
        }

        public void Close()
        {
            IsOpen = false;
            ClosingAnimation.Begin();
            ClosingAnimation.Completed += (s, ev) =>
            {
                SpinningAnimation.Stop();
                Visibility = System.Windows.Visibility.Collapsed;
            };
        }
    }
}
