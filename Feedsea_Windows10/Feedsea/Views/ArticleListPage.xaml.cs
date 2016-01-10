using Feedsea.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Autofac;
using Feedsea.Common;
using Feedsea.Common.Providers.Data;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Feedsea.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ArticleListPage : Page
    {
        public ArticleListViewModel ViewModel { get { return DataContext as ArticleListViewModel; } }

        public string ThisProp { get; set; }

        public ArticleListPage()
        {
            this.InitializeComponent();
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var currentId = (e.Parameter as INewsSource).UrlID;

            if (e.NavigationMode == NavigationMode.Back)
            {
                var state = SuspensionManager.SessionStateForFrame(this.Frame);
                if (state != null && state.ContainsKey(currentId))
                {
                    object value = null;

                    if (state.TryGetValue(currentId, out value))
                    {
                        DataContext = value;
                    }
                }
            }

            if (e.NavigationMode != NavigationMode.Back)
            {
                await ViewModel.LoadData(e.Parameter);
                await ViewModel.Refresh(e.Parameter);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            var state = SuspensionManager.SessionStateForFrame(this.Frame);
            state[ViewModel.SelectedSource.UrlID] = DataContext;
        }
    }
}
