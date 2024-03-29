using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using feedsea.Resources;
using Windows.UI.Xaml.Controls.Primitives;
using feedsea.ViewModels;
using feedsea.Common.Providers;
using feedsea.Common.Providers.Data;

namespace feedsea.UserControls
{

   public partial class SharingControl
      : Windows.UI.Xaml.Controls.UserControl
   {
      public event EventHandler OnClose;

      public ShareArticleViewModel ViewModel
      {
         get
         {
            return (ShareArticleViewModel)DataContext;
         }
      }


      public SharingControl()
      {
         InitializeComponent();
      }

      public void Load(ArticleData article)
      {
         ViewModel.LoadData(article);
         ViewModel.Done += ViewModel_Done;
      }

      void ViewModel_Done(object sender, EventArgs e)
      {
         Close();
      }

      public void Close()
      {
         var parent = (this.Parent as Windows.UI.Xaml.Controls.Primitives.Popup);
         if ( OnClose != null )
            OnClose(this, new EventArgs());
         if ( parent == null )
         {
            return ;
         }
         ClosingAnimation.Completed += (s, e) =>
            {
               parent.IsOpen = false;
            };
         ClosingAnimation.Begin();
      }

      public void PopupOpenEvent()
      {
         var parent = (this.Parent as Windows.UI.Xaml.Controls.Primitives.Popup);
         if ( parent == null )
         {
            LayoutRoot.Opacity = 1;
            return ;
         }
         parent.Opened += (s, e) =>
            {
               OpeningAnimation.Begin();
            };
      }

   }

}