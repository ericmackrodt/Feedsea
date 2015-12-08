using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace feedsea.UserControls
{

   public delegate void LoginClickHandler(string username, string password);

   public partial class GenericLoginScreen
      : Windows.UI.Xaml.Controls.UserControl
   {
      public event LoginClickHandler OnLoginClick;
      public event EventHandler OnCanceled;

      public GenericLoginScreen(string title)
      {
         InitializeComponent();
         TxtTitle.Text = title;
      }

      private void BtnLogin_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
      {
         if ( OnLoginClick != null )
            OnLoginClick(TxtUsername.Text, TxtPassword.Password);
      }

      public void Cancel()
      {
         if ( OnCanceled != null )
            OnCanceled(this, new EventArgs());
      }

   }

}