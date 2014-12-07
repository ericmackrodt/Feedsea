using feedsea.Common.MVVM;
using feedsea.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace feedsea.Common
{
    public class FeedListItemTypeSelector : ADataTemplateSelector
    {
        public DataTemplate AdTemplate { get; set; }
        public DataTemplate ArticleTemplate { get; set; }

        public object Selector
        {
            get { return (object)GetValue(SelectorProperty); }
            set { SetValue(SelectorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Selector.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectorProperty =
            DependencyProperty.Register("Selector", typeof(object), typeof(FeedListItemTypeSelector), new PropertyMetadata(null));

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (Selector is AdItemModel)
                return AdTemplate;

            return ArticleTemplate;
        }
    }
}
