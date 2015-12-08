using Feedsea.Common.Providers.Data;
using Feedsea.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Feedsea.Common.Controls
{
    public class SourceListTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CategoryTemplate { get; set; }
        public DataTemplate SubscriptionTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is CategoryData || item is ExpandableCategory)
                return CategoryTemplate;
            else if (item is SubscriptionData)
                return SubscriptionTemplate;

            return null;
        }
    }
}
