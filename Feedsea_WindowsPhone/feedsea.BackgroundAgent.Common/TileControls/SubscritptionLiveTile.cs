using feedsea.BackgroundAgent.Common.ContentBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Media;

namespace feedsea.BackgroundAgent.Common.TileControls
{
    public class SubscritptionLiveTile : TileContentBuilderBase
    {
        private void BuildContent(TileFace tile, TileMode mode)
        {
            BuildContentGrid(mode != TileMode.Transparent);

            //Title
            BuildArticleTitle(tile.Title);
            _articleTitle.Margin = new Thickness(20);
            SetOnGrid(_articleTitle, 0, 0, 3);
        }

        public override void BuildFront(TileFace tile, TileMode mode, string title, int? count = null)
        {
            BuildContent(tile, mode);

            //Favicon
            if (tile.FaviconBytes != null)
            {
                BuildFavicon(tile.FaviconBytes);
                _sourceFavicon.VerticalAlignment = VerticalAlignment.Top;
                _sourceFavicon.Margin = new Thickness(20, 20, 0, 22);
                SetOnGrid(_sourceFavicon, 0, 0);
            }

            BuildSourceTitle(tile.SourceName);
            _sourceTitle.VerticalAlignment = VerticalAlignment.Top;
            _sourceTitle.Margin = new Thickness(20, 15, 20, 20);
            SetOnGrid(_sourceTitle, 0, 1, 2);

            //Count
            if (count != null && count > 0)
            {
                BuildArticleCounter(count.Value);
                SetOnGrid(_articleCounterBorder, 0, 2);
            }

            if (_appLogo != null)
                _appLogo.Visibility = Visibility.Collapsed;

            BuildMainBorder(tile, mode);
        }

        public override void BuildBack(TileFace tile, TileMode mode)
        {
            BuildContent(tile, mode);
            BuildLogo();

            SetOnGrid(_appLogo, 0, 0, 2);

            if (_articleCounterBorder != null)
                _articleCounterBorder.Visibility = Visibility.Collapsed;

            BuildMainBorder(tile, mode);
        }
    }
}
