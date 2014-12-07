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
    public delegate void LoginClickHandler(string username, string password);

    public partial class GenericLoginScreen : UserControl
    {
        public event LoginClickHandler OnLoginClick;
        public event EventHandler OnCanceled;

        public GenericLoginScreen(string title)
        {
            InitializeComponent();
            TxtTitle.Text = title;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (OnLoginClick != null)
                OnLoginClick(TxtUsername.Text, TxtPassword.Password);
        }

        public void Cancel()
        {
            if (OnCanceled != null)
                OnCanceled(this, new EventArgs());
        }
    }
}
