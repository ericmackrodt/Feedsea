using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using feedsea.ViewModels;
using System.Windows.Input;
using System.Windows.Data;
using feedsea.Common.MVVM.Tombstone;

namespace feedsea.Views
{
    public partial class AddSource : PhoneApplicationPage
    {
        public AddSourceViewModel ViewModel { get { return (DataContext as AddSourceViewModel); } }

        public AddSource()
        {
            InitializeComponent();
            ViewModel.SaveComplete += AddSource_SaveComplete;
        }

        void AddSource_SaveComplete(object sender, EventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            TombstoneHelper.page_OnNavigatedTo(this, e);

            string value = null;

            if (NavigationContext.QueryString.TryGetValue("sourceId", out value))
                ViewModel.LoadData(value);

            if (NavigationContext.QueryString.TryGetValue("searchTerm", out value))
                await ViewModel.LoadDataAndSearch(value);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            TombstoneHelper.page_OnNavigatedFrom(this, e);
        }

        private void txtURL_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Focus();
                ViewModel.SearchCommand.Execute(txtURL.Text);
            }
        }

        private void txtURL_ActionIconTapped(object sender, EventArgs e)
        {
            //LoadData();
            this.Focus();
            ViewModel.SearchCommand.Execute(txtURL.Text);
        }

        private void LoadData()
        {
            this.Focus();
            txtURL.GetBindingExpression(PhoneTextBox.TextProperty).UpdateSource();
            //ViewModel.LoadDataCommand.Execute(null);
        }
    }
}