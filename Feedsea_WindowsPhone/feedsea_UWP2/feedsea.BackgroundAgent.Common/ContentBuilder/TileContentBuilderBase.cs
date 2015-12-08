using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using feedsea.BackgroundAgent.Common.Helpers;
using System.IO.IsolatedStorage;

namespace feedsea.BackgroundAgent.Common.ContentBuilder
{

   public abstract class TileContentBuilderBase
      : ITileContentBuilder
   {
      protected Windows.UI.Xaml.Controls.Grid _contentGrid;
      protected Windows.UI.Xaml.Controls.Border _mainBorder;
      protected Windows.UI.Xaml.Controls.TextBlock _titleTextBlock;
      protected Windows.UI.Xaml.Controls.Border _articleCounterBorder;
      protected Windows.UI.Xaml.Controls.TextBlock _articleCounterText;
      protected Windows.UI.Xaml.Controls.Image _appLogo;
      protected Windows.UI.Xaml.Controls.TextBlock _articleTitle;
      protected Windows.UI.Xaml.Controls.Image _sourceFavicon;
      protected Windows.UI.Xaml.Controls.TextBlock _sourceTitle;
      protected Windows.UI.Xaml.Controls.TextBlock _categoryTitle;
      protected Windows.UI.Xaml.Media.Imaging.BitmapImage _faviconBitmap;
      protected Windows.UI.Xaml.Media.Imaging.BitmapImage _articleBitmap;
      protected Windows.UI.Xaml.Media.Brush _backgroundBrush;

      protected virtual Windows.UI.Xaml.Controls.TextBlock BaseTextBlock(string font, double size)
      {
         var tb = new Windows.UI.Xaml.Controls.TextBlock();
         tb.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
         tb.FontFamily = new Windows.UI.Xaml.Media.FontFamily(font);
         tb.FontWeight = Windows.UI.Text.FontWeights.SemiBold;
         tb.FontSize = size;
         return tb;
      }

      protected virtual void BuildArticleCounter(int? count = null)
      {
         if ( _articleCounterText == null && count > 0 )
         {
            _articleCounterText = BaseTextBlock("Segoe UI", 32);
         }
         if ( _articleCounterBorder == null && count > 0 )
         {
            _articleCounterBorder = new Windows.UI.Xaml.Controls.Border();
            _articleCounterBorder.Padding = new Windows.UI.Xaml.Thickness(15, 2, 15, 2);
            _articleCounterBorder.CornerRadius = new CornerRadius(20.0);
            _articleCounterBorder.Margin = new Windows.UI.Xaml.Thickness(5, 20, 20, 20);
            _articleCounterBorder.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Color.FromArgb(0x69, 0, 0, 0)); //#69000000

            _articleCounterBorder.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
            _articleCounterBorder.Child = _articleCounterText;
         }
         if ( _articleCounterText != null )
         {
            if ( count == 0 || count == null )
               _articleCounterBorder.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            else
            {
               _articleCounterText.Text = (count > 999 ? 999 : count).ToString();
               _articleCounterBorder.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
         }
      }

      protected virtual void BuildLogo()
      {
         if ( _appLogo == null )
         {
            _appLogo = new Windows.UI.Xaml.Controls.Image();
            _appLogo.Height = 33;
            _appLogo.Margin = new Windows.UI.Xaml.Thickness(20);
            _appLogo.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
            _appLogo.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            var logo = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(new Uri("ms-appx://"), "/feedsea.BackgroundAgent.Common;component/Resources/logo.png"));
            _appLogo.Source = logo;
         }
         _appLogo.Visibility = Windows.UI.Xaml.Visibility.Visible;
      }

      protected virtual void BuildArticleTitle(string text)
      {
         if ( _articleTitle == null )
         {
            _articleTitle = BaseTextBlock("Cambria", 42);
            _articleTitle.TextWrapping = Windows.UI.Xaml.TextWrapping.Wrap;
            _articleTitle.MaxHeight = 194;
            _articleTitle.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Bottom;
         }
         _articleTitle.Text = text;
         _articleTitle.Margin = new Windows.UI.Xaml.Thickness(20, 0, 20, 70);
      }

      protected virtual void BuildFavicon(byte[] faviconBytes)
      {
         if ( _sourceFavicon == null )
         {
            _sourceFavicon = new Windows.UI.Xaml.Controls.Image();
            _sourceFavicon.Height = 32;
            _sourceFavicon.Width = 32;
         }
         _sourceFavicon.Margin = new Windows.UI.Xaml.Thickness(20, 10, 0, 22);
         _sourceFavicon.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Bottom;
         _sourceFavicon.Source = GetBitmap(faviconBytes, _faviconBitmap);
         _sourceFavicon.Visibility = Windows.UI.Xaml.Visibility.Visible;
      }

      protected virtual void BuildSourceTitle(string name)
      {
         if ( _sourceTitle == null )
         {
            _sourceTitle = BaseTextBlock("Segoe UI", 32);
         }
         _sourceTitle.Margin = new Windows.UI.Xaml.Thickness(20, 10, 20, 20);
         _sourceTitle.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Bottom;
         _sourceTitle.Text = name;
         _sourceTitle.Visibility = Windows.UI.Xaml.Visibility.Visible;
      }

      protected virtual void BuildCategoryTitle(string name)
      {
         if ( _categoryTitle == null )
         {
            _categoryTitle = BaseTextBlock("Segoe WP", 30);
            _categoryTitle.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
            _categoryTitle.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            _categoryTitle.FontWeight = Windows.UI.Text.FontWeights.Bold;
            _categoryTitle.Margin = new Windows.UI.Xaml.Thickness(20, 15, 20, 20);
         }
         _categoryTitle.Text = name;
         _categoryTitle.Visibility = Windows.UI.Xaml.Visibility.Visible;
      }

      protected virtual void BuildContentGrid(bool withBackground = true)
      {
         if ( _contentGrid == null )
         {
            _contentGrid = new Grid();
            _contentGrid.ColumnDefinitions.Add(new ColumnDefinition()
               {
                  Width = new Windows.UI.Xaml.GridLength(1, Windows.UI.Xaml.GridUnitType.Auto)
               });
            _contentGrid.ColumnDefinitions.Add(new ColumnDefinition()
               {
                  Width = new Windows.UI.Xaml.GridLength(1, Windows.UI.Xaml.GridUnitType.Star)
               });
            _contentGrid.ColumnDefinitions.Add(new ColumnDefinition()
               {
                  Width = new Windows.UI.Xaml.GridLength(1, Windows.UI.Xaml.GridUnitType.Auto)
               });
         }
         if ( withBackground )
            _contentGrid.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Color.FromArgb(0x64, 0, 0, 0));
         else
            _contentGrid.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
      }

      protected virtual void BuildMainBorder(Windows.UI.Xaml.Media.Brush background = null)
      {
         if ( _mainBorder == null )
         {
            _mainBorder = new Windows.UI.Xaml.Controls.Border();
         }
         _mainBorder.Background = background;
      }

      protected virtual void BuildMainBorder(TileFace tile, TileMode mode)
      {
         if ( mode != TileMode.Transparent && tile.ImageBytes != null )
         {
            _backgroundBrush = new Windows.UI.Xaml.Media.ImageBrush();
            (_backgroundBrush as Windows.UI.Xaml.Media.ImageBrush).ImageSource = GetBitmap(tile.ImageBytes, _articleBitmap);
            (_backgroundBrush as Windows.UI.Xaml.Media.ImageBrush).Stretch = Windows.UI.Xaml.Media.Stretch.UniformToFill;
            if ( mode == TileMode.SemiTransparent )
               _backgroundBrush.Opacity = 0.5;
         }
         else
         {
            _backgroundBrush = GetColor(0, 0, 0, 0);
         }
         BuildMainBorder(_backgroundBrush);
         _mainBorder.Child = _contentGrid;
      }

      protected virtual void SetOnGrid(Windows.UI.Xaml.FrameworkElement control, int row, int column, int colSpan = 1, int rowSpan = 1)
      {
         if ( !_contentGrid.Children.Contains(control) )
            _contentGrid.Children.Add(control);
         Windows.UI.Xaml.Controls.Grid.SetRow(control, row);
         Windows.UI.Xaml.Controls.Grid.SetColumn(control, column);
         Windows.UI.Xaml.Controls.Grid.SetColumnSpan(control, colSpan);
         Windows.UI.Xaml.Controls.Grid.SetRowSpan(control, rowSpan);
      }

      protected virtual Windows.UI.Xaml.Media.Imaging.BitmapImage GetBitmap(byte[] data, Windows.UI.Xaml.Media.Imaging.BitmapImage bmp)
      {
         if ( bmp == null )
            bmp = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
         bmp.SetSource(data.AsRandomAccessStream());
         return bmp;
      }

      protected virtual Windows.UI.Xaml.Media.SolidColorBrush GetColor(byte a, byte r, byte g, byte b)
      {
         return new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
      }

      public abstract void BuildFront(TileFace tile, TileMode mode, TileType type, string title, int? count = null);

      public abstract void BuildBack(TileFace tile, TileMode mode, TileType type);

      public virtual void SaveTile(TileSize tileSize, TileType type, string shellContentFolder, string fileNameSufix, string fileName, IsolatedStorageFile storage)
      {
         this.SaveJpeg(tileSize, type, shellContentFolder, fileNameSufix, fileName, storage);
      }

      public virtual Windows.UI.Xaml.Controls.Border GetContent()
      {
         return _mainBorder;
      }

   }

}