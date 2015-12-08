using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using feedsea.ViewModels;
using System.Windows.Input;
using Windows.UI.Xaml.Data;
using feedsea.Common.MVVM.Tombstone;

namespace feedsea.Views
{

   public partial class AddSource
      : Windows.UI.Xaml.Controls.Page
   {

      public AddSourceViewModel ViewModel
      {
         get
         {
            return (DataContext as AddSourceViewModel);
         }
      }


      public AddSource()
      {
         InitializeComponent();
         ViewModel.SaveComplete += AddSource_SaveComplete;
      }

      void AddSource_SaveComplete(object sender, EventArgs e)
      {
         if ( NavigationService.CanGoBack )
            ((Windows.UI.Xaml.Controls.Frame)Windows.UI.Xaml.Window.Current.Content).GoBack();
      }

      protected async override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
      {
         base.OnNavigatedTo(e);
         TombstoneHelper.page_OnNavigatedTo(this, e);
         string value = null;
         if ( NavigationContext.QueryString.TryGetValue("sourceId", out value) )
            ViewModel.LoadData(value);
         if ( NavigationContext.QueryString.TryGetValue("searchTerm", out value) )
            await ViewModel.LoadDataAndSearch(value);
      }

      protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
      {
         base.OnNavigatedFrom(e);
         TombstoneHelper.page_OnNavigatedFrom(this, e);
      }

      private void txtURL_KeyUp(object sender, Windows.UI.Core.KeyEventArgs e)
      {
         if ( e.Key == Windows.System.VirtualKey.Enter )
         {
            this.Focus(FocusState.Programmatic);
            ViewModel.SearchCommand.Execute(txtURL.Text);
         }
      }

      private void txtURL_ActionIconTapped(object sender, EventArgs e)
      {
         //LoadData();
         this.Focus(FocusState.Programmatic);
         ViewModel.SearchCommand.Execute(txtURL.Text);
      }

      private void LoadData()
      {
         this.Focus(FocusState.Programmatic);
         txtURL.GetBindingExpression(PhoneTextBox.TextProperty).UpdateSource();
      //ViewModel.LoadDataCommand.Execute(null);
      }

   }

}