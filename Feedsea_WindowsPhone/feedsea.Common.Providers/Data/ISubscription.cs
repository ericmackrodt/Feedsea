using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Providers.Data
{
    public interface ISubscription : INewsSource
    {
        string Image { get; set; }
        string Link { get; set; }
        int UnreadNumber { get; set; }
        CategoryData[] Categories { get; set; }

        event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
