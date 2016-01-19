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
        public ArticleListViewModel ViewModel { get { return (ArticleListViewModel)DataContext; } }

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

        private void SetViewVisualState(double width)
        {
            if (width > 600 && ViewModel.ArticleViewTemplate == Common.ArticleViewTemplateEnum.Cards)
                VisualStateManager.GoToState(this, "NormalState", false);
            else
                VisualStateManager.GoToState(this, "MobileState", false);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetViewVisualState(e.NewSize.Width);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.ArticleLayoutChanged += ViewModel_ArticleLayoutChanged;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.ArticleLayoutChanged -= ViewModel_ArticleLayoutChanged;
        }

        private void ViewModel_ArticleLayoutChanged(object sender, EventArgs e)
        {
            SetViewVisualState(ActualWidth);
        }

        private void ItemsWrapGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetItemsWidth(sender as ItemsWrapGrid, e.NewSize.Width);
        }

        private static void SetItemsWidth(ItemsWrapGrid itemsPanel, double newWidth)
        {
            var maxWidth = 450;
            var minWidth = 270;

            var byOne = newWidth;
            var byTwo = newWidth / 2;
            var byThree = newWidth / 3;
            var byFour = newWidth / 4;

            if (byOne <= 540 && byOne >= minWidth)
            {
                itemsPanel.ItemWidth = byOne;
                return;
            }

            if (byTwo <= maxWidth && byTwo >= minWidth)
            {
                itemsPanel.ItemWidth = byTwo;
                return;
            }

            if (byThree <= maxWidth && byThree >= minWidth)
            {
                itemsPanel.ItemWidth = byThree;
                return;
            }

            if (byFour <= maxWidth && byFour >= minWidth)
            {
                itemsPanel.ItemWidth = byFour;
            }
        }

        private void BtnGoToTop_Click(object sender, RoutedEventArgs e)
        {
            LstArticles.ScrollIntoView(ViewModel.Articles.FirstOrDefault());
        }
    }
}
