using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using feedsea.BackgroundAgent.Common.Helpers;
using System.IO.IsolatedStorage;

namespace feedsea.BackgroundAgent.Common.ContentBuilder
{
    public abstract class TileContentBuilderBase : ITileContentBuilder
    {
        protected Grid _contentGrid;
        protected Border _mainBorder;
        protected TextBlock _titleTextBlock;
        protected Border _articleCounterBorder;
        protected TextBlock _articleCounterText;
        protected Image _appLogo;
        protected TextBlock _articleTitle;
        protected Image _sourceFavicon;
        protected TextBlock _sourceTitle;
        protected TextBlock _categoryTitle;
        protected BitmapImage _faviconBitmap;
        protected BitmapImage _articleBitmap;
        protected Brush _backgroundBrush;

        protected virtual TextBlock BaseTextBlock(string font, double size)
        {
            var tb = new TextBlock();
            tb.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
            tb.FontFamily = new FontFamily(font);
            tb.FontWeight = FontWeights.SemiBold;
            tb.FontSize = size;

            return tb;
        }

        protected virtual void BuildArticleCounter(int? count = null)
        {
            if (_articleCounterText == null && count > 0)
            {
                _articleCounterText = BaseTextBlock("Segoe UI", 32);
            }

            if (_articleCounterBorder == null && count > 0)
            {
                _articleCounterBorder = new Border();
                _articleCounterBorder.Padding = new Thickness(15, 2, 15, 2);
                _articleCounterBorder.CornerRadius = new CornerRadius(20.0);
                _articleCounterBorder.Margin = new Thickness(5, 20, 20, 20);
                _articleCounterBorder.Background = new SolidColorBrush(Color.FromArgb(0x69, 0, 0, 0)); //#69000000
                _articleCounterBorder.VerticalAlignment = VerticalAlignment.Top;
                _articleCounterBorder.Child = _articleCounterText;
            }

            if (_articleCounterText != null)
            {
                if (count == 0 || count == null)
                    _articleCounterBorder.Visibility = Visibility.Collapsed;
                else
                {
                    _articleCounterText.Text = (count > 999 ? 999 : count).ToString();
                    _articleCounterBorder.Visibility = Visibility.Visible;
                }
            }
        }

        protected virtual void BuildLogo()
        {
            if (_appLogo == null)
            {
                _appLogo = new Image();
                _appLogo.Height = 33;
                _appLogo.Margin = new Thickness(20);
                _appLogo.VerticalAlignment = VerticalAlignment.Top;
                _appLogo.HorizontalAlignment = HorizontalAlignment.Left;
                var logo = new BitmapImage(new Uri("/feedsea.BackgroundAgent.Common;component/Resources/logo.png", UriKind.Relative));
                _appLogo.Source = logo;
            }

            _appLogo.Visibility = Visibility.Visible;
        }

        protected virtual void BuildArticleTitle(string text)
        {
            if (_articleTitle == null)
            {
                _articleTitle = BaseTextBlock("Cambria", 42);
                _articleTitle.TextWrapping = TextWrapping.Wrap;
                _articleTitle.MaxHeight = 194;
                _articleTitle.VerticalAlignment = VerticalAlignment.Bottom;
            }

            _articleTitle.Text = text;
            _articleTitle.Margin = new Thickness(20, 0, 20, 70);
        }

        protected virtual void BuildFavicon(byte[] faviconBytes)
        {
            if (_sourceFavicon == null)
            {
                _sourceFavicon = new Image();
                _sourceFavicon.Height = 32;
                _sourceFavicon.Width = 32;
            }

            _sourceFavicon.Margin = new Thickness(20, 10, 0, 22);
            _sourceFavicon.VerticalAlignment = VerticalAlignment.Bottom;
            _sourceFavicon.Source = GetBitmap(faviconBytes, _faviconBitmap);
            _sourceFavicon.Visibility = Visibility.Visible;
        }

        protected virtual void BuildSourceTitle(string name)
        {
            if (_sourceTitle == null)
            {
                _sourceTitle = BaseTextBlock("Segoe UI", 32);
            }

            _sourceTitle.Margin = new Thickness(20, 10, 20, 20);
            _sourceTitle.VerticalAlignment = VerticalAlignment.Bottom;
            _sourceTitle.Text = name;
            _sourceTitle.Visibility = Visibility.Visible;
        }

        protected virtual void BuildCategoryTitle(string name)
        {
            if (_categoryTitle == null)
            {
                _categoryTitle = BaseTextBlock("Segoe WP", 30);
                _categoryTitle.VerticalAlignment = VerticalAlignment.Top;
                _categoryTitle.HorizontalAlignment = HorizontalAlignment.Left;
                _categoryTitle.FontWeight = FontWeights.Bold;
                _categoryTitle.Margin = new Thickness(20, 15, 20, 20);
            }

            _categoryTitle.Text = name;
            _categoryTitle.Visibility = Visibility.Visible;
        }

        protected virtual void BuildContentGrid(bool withBackground = true)
        {
            if (_contentGrid == null)
            {
                _contentGrid = new Grid();
                _contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
                _contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                _contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
            }

            if (withBackground)
                _contentGrid.Background = new SolidColorBrush(Color.FromArgb(0x64, 0, 0, 0));
            else
                _contentGrid.Background = new SolidColorBrush(Colors.Transparent);
        }

        protected virtual void BuildMainBorder(Brush background = null)
        {
            if (_mainBorder == null)
            {
                _mainBorder = new Border();
            }

            _mainBorder.Background = background;
        }

        protected virtual void BuildMainBorder(TileFace tile, TileMode mode)
        {
            if (mode != TileMode.Transparent && tile.ImageBytes != null)
            {
                _backgroundBrush = new ImageBrush();
                (_backgroundBrush as ImageBrush).ImageSource = GetBitmap(tile.ImageBytes, _articleBitmap);
                (_backgroundBrush as ImageBrush).Stretch = Stretch.UniformToFill;

                if (mode == TileMode.SemiTransparent)
                    _backgroundBrush.Opacity = 0.5;
            }
            else
            {
                _backgroundBrush = GetColor(0, 0, 0, 0);
            }

            BuildMainBorder(_backgroundBrush);
            _mainBorder.Child = _contentGrid;
        }

        protected virtual void SetOnGrid(FrameworkElement control, int row, int column, int colSpan = 1, int rowSpan = 1)
        {
            if (!_contentGrid.Children.Contains(control))
                _contentGrid.Children.Add(control);

            Grid.SetRow(control, row);
            Grid.SetColumn(control, column);
            Grid.SetColumnSpan(control, colSpan);
            Grid.SetRowSpan(control, rowSpan);
        }

        protected virtual BitmapImage GetBitmap(byte[] data, BitmapImage bmp)
        {
            if (bmp == null)
                bmp = new BitmapImage();

            bmp.SetSource(data);
            return bmp;
        }

        protected virtual SolidColorBrush GetColor(byte a, byte r, byte g, byte b)
        {
            return new SolidColorBrush(Color.FromArgb(a, r, g, b));
        }

        public abstract void BuildFront(TileFace tile, TileMode mode, TileType type, string title, int? count = null);

        public abstract void BuildBack(TileFace tile, TileMode mode, TileType type);

        public virtual void SaveTile(TileSize tileSize, TileType type, string shellContentFolder, string fileNameSufix, string fileName, IsolatedStorageFile storage)
        {
            this.SaveJpeg(tileSize, type, shellContentFolder, fileNameSufix, fileName, storage);
        }

        public virtual Border GetContent()
        {
            return _mainBorder;
        }
    }
}
