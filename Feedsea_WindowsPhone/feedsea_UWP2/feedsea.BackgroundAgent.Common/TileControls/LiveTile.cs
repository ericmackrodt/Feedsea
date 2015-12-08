using feedsea.BackgroundAgent.Common.ContentBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace feedsea.BackgroundAgent.Common.TileControls
{

   public class LiveTile
      : TileContentBuilderBase
   {

      private void BuildContent(TileFace tile, TileMode mode)
      {
         BuildContentGrid(mode != TileMode.Transparent);
         //Title
         BuildArticleTitle(tile.Title);
         SetOnGrid(_articleTitle, 0, 0, 3);
         //Favicon
         if ( tile.FaviconBytes != null )
         {
            BuildFavicon(tile.FaviconBytes);
            SetOnGrid(_sourceFavicon, 0, 0);
         }
         BuildSourceTitle(tile.SourceName);
         SetOnGrid(_sourceTitle, 0, 1, 2);
      }

      private void BuildMainTileFront(TileFace tile, TileMode mode, string title, int? count = null)
      {
         BuildContent(tile, mode);
         BuildLogo();
         SetOnGrid(_appLogo, 0, 0, 2);
         //Count
         BuildArticleCounter(count);
         if ( _articleCounterBorder != null )
            SetOnGrid(_articleCounterBorder, 0, 2);
         BuildMainBorder(tile, mode);
         //Disappear with doesn't belong to this tile
         if ( _categoryTitle != null )
            _categoryTitle.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
      }

      private void BuildCategoryTileFront(TileFace tile, TileMode mode, string title, int? count = null)
      {
         BuildContent(tile, mode);
         BuildCategoryTitle(title);
         SetOnGrid(_categoryTitle, 0, 0, 2);
         //Count
         BuildArticleCounter(count);
         if ( _articleCounterBorder != null )
            SetOnGrid(_articleCounterBorder, 0, 2);
         if ( _appLogo != null )
            _appLogo.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
         BuildMainBorder(tile, mode);
      }

      private void BuildSubscriptionTileFront(TileFace tile, TileMode mode, string title, int? count = null)
      {
         BuildContent(tile, mode);
         _articleTitle.Margin = new Windows.UI.Xaml.Thickness(20);
         //Favicon
         if ( tile.FaviconBytes != null )
         {
            BuildFavicon(tile.FaviconBytes);
            _sourceFavicon.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
            _sourceFavicon.Margin = new Windows.UI.Xaml.Thickness(20, 20, 0, 22);
            SetOnGrid(_sourceFavicon, 0, 0);
         }
         BuildSourceTitle(tile.SourceName);
         _sourceTitle.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
         _sourceTitle.Margin = new Windows.UI.Xaml.Thickness(20, 15, 20, 20);
         SetOnGrid(_sourceTitle, 0, 1);
         //Count
         BuildArticleCounter(count);
         if ( _articleCounterBorder != null )
            SetOnGrid(_articleCounterBorder, 0, 2);
         if ( _appLogo != null )
            _appLogo.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
         BuildMainBorder(tile, mode);
         //Disappear with doesn't belong to this tile
         if ( _categoryTitle != null )
            _categoryTitle.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
      }

      private void BuildMainTileBack(TileFace tile, TileMode mode)
      {
         BuildContent(tile, mode);
         BuildLogo();
         SetOnGrid(_appLogo, 0, 0, 2);
         if ( _articleCounterBorder != null )
            _articleCounterBorder.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
         BuildMainBorder(tile, mode);
         //Disappear with doesn't belong to this tile
         if ( _categoryTitle != null )
            _categoryTitle.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
      }

      private void BuildCategoryTileBack(TileFace tile, TileMode mode)
      {
         BuildContent(tile, mode);
         BuildLogo();
         SetOnGrid(_appLogo, 0, 0, 2);
         if ( _articleCounterBorder != null )
            _articleCounterBorder.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
         if ( _categoryTitle != null )
            _categoryTitle.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
         BuildMainBorder(tile, mode);
      }

      private void BuildSubscriptionTileBack(TileFace tile, TileMode mode)
      {
         BuildContent(tile, mode);
         _articleTitle.Margin = new Windows.UI.Xaml.Thickness(20);
         BuildLogo();
         SetOnGrid(_appLogo, 0, 0, 2);
         if ( _articleCounterBorder != null )
            _articleCounterBorder.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
         if ( _sourceFavicon != null )
            _sourceFavicon.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
         if ( _sourceTitle != null )
            _sourceTitle.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
         BuildMainBorder(tile, mode);
         //Disappear with doesn't belong to this tile
         if ( _categoryTitle != null )
            _categoryTitle.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
      }

      public override void BuildFront(TileFace tile, TileMode mode, TileType type, string title, int? count = null)
      {
         switch ( type )
         {
            case TileType.Category:
               BuildCategoryTileFront(tile, mode, title, count);
               break;
            case TileType.Subscription:
               BuildSubscriptionTileFront(tile, mode, title, count);
               break;
            case TileType.Main:
            default:
               BuildMainTileFront(tile, mode, title, count);
               break;
         }
      }

      public override void BuildBack(TileFace tile, TileMode mode, TileType type)
      {
         switch ( type )
         {
            case TileType.Category:
               BuildCategoryTileBack(tile, mode);
               break;
            case TileType.Subscription:
               BuildSubscriptionTileBack(tile, mode);
               break;
            case TileType.Main:
            default:
               BuildMainTileBack(tile, mode);
               break;
         }
      }

   }

}