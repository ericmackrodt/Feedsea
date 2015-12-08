using feedsea.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace feedsea.Common
{

   public class ArticleItemTemplateSelector
      : ADataTemplateSelector
   {

      public DataTemplate NormalTemplate { get; set; }

      public DataTemplate NormalTemplateNoImage { get; set; }

      public DataTemplate SmallTemplate { get; set; }

      public DataTemplate SmallTemplateNoImage { get; set; }

      public object Selector
      {
         get
         {
            return (ArticleTemplateType)GetValue(SelectorProperty);
         }
         set
         {
            SetValue(SelectorProperty, value);
         }
      }

      // Using a DependencyProperty as the backing store for Selector.  This enables animation, styling, binding, etc...
      public static readonly Windows.UI.Xaml.DependencyProperty SelectorProperty = Windows.UI.Xaml.DependencyProperty.Register("Selector", typeof(object), typeof(ArticleItemTemplateSelector), new PropertyMetadata(null, SelectoryPropertyChanged));

      private static void SelectoryPropertyChanged(Windows.UI.Xaml.DependencyObject d, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e)
      {
         (d as ArticleItemTemplateSelector).ContentTemplate = (d as ArticleItemTemplateSelector).SetTemplate();
      }

      public override DataTemplate SelectTemplate(object item, Windows.UI.Xaml.DependencyObject container)
      {
         return SetTemplate();
      }

      private DataTemplate SetTemplate()
      {
         switch ( (ArticleTemplateType)Selector )
         {
            case ArticleTemplateType.NormalTemplate:
               return this.NormalTemplate;
            case ArticleTemplateType.NormalTemplateNoImage:
               return this.NormalTemplateNoImage;
            case ArticleTemplateType.SmallTemplate:
               return this.SmallTemplate;
            case ArticleTemplateType.SmallTemplateNoImage:
               return this.SmallTemplateNoImage;
            default:
               return this.NormalTemplate;
         }
      }

   }

}