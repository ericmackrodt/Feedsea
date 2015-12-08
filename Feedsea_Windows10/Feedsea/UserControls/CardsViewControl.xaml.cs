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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Feedsea.UserControls
{
    public sealed partial class CardsViewControl : UserControl, IArticleViewControl
    {
        public CardsViewControl()
        {
            this.InitializeComponent();
        }

        public void ScrollToTop(ArticleData article)
        {
            LstArticles.ScrollIntoView(article);
        }

        private void ItemsWrapGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var itemsPanel = (ItemsWrapGrid)LstArticles.ItemsPanelRoot;
            double margin = 40.0;
            var maxWidth = 400;
            var minWidth = 300;

            var byOne = e.NewSize.Width;
            var byTwo = e.NewSize.Width / 2;
            var byThree = e.NewSize.Width / 3;
            var byFour = e.NewSize.Width / 4;

            if (byOne <= maxWidth && byOne >= minWidth)
                itemsPanel.ItemWidth = byOne;

            if (byTwo <= maxWidth && byTwo >= minWidth)
                itemsPanel.ItemWidth = byTwo - margin;

            if (byThree <= maxWidth && byThree >= minWidth)
                itemsPanel.ItemWidth = byThree - margin;

            if (byFour <= maxWidth && byFour >= minWidth)
                itemsPanel.ItemWidth = byFour - margin;


            //MyItemsPanel.ItemWidth = (e.NewSize.Width - margin) / (double)itemsNumber;
        }
    }
}
