using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Feedsea.Common.Providers.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Feedsea.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Feedsea.UserControls
{
    public sealed partial class CardsViewControl : UserControl, IArticleViewControl
    {
        public ArticleListViewModel ViewModel { get { return (ArticleListViewModel)DataContext; } }

        public CardsViewControl()
        {
            this.InitializeComponent();
            Loaded += CardsViewControl_Loaded;
        }

        private void CardsViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        [Obsolete("Not a permanent solution!")]
        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "ArticleViewTemplate") return;

            if (Width > 600 && ViewModel.ArticleViewTemplate == Common.ArticleViewTemplateEnum.Cards)
                VisualStateManager.GoToState(this, "NormalState", false);
            else
                VisualStateManager.GoToState(this, "MobileState", false);
        }

        public void ScrollToTop(ArticleData article)
        {
            LstArticles.ScrollIntoView(article);
        }

        private void ItemsWrapGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var itemsPanel = (ItemsWrapGrid)sender;//LstArticles.ItemsPanelRoot;
            var maxWidth = 500;
            var minWidth = 300;

            var byOne = e.NewSize.Width;
            var byTwo = e.NewSize.Width / 2;
            var byThree = e.NewSize.Width / 3;
            var byFour = e.NewSize.Width / 4;

            if (byOne <= 600 && byOne >= minWidth)
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

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width > 600 && ViewModel.ArticleViewTemplate == Common.ArticleViewTemplateEnum.Cards)
                VisualStateManager.GoToState(this, "NormalState", false);
            else
                VisualStateManager.GoToState(this, "MobileState", false);
        }
    }
}
