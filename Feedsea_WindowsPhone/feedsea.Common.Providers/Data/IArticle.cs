using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Providers.Data
{
    public interface IArticle : INotifyPropertyChanged
    {
        string UniqueID { get; set; }
        string Title { get; set; }
        string Summary { get; set; }
        string Content { get; set; }
        string Author { get; set; }
        bool IsRead { get; set; }
        bool IsFavorite { get; set; }
        string MainImageUrl { get; set; }
        string URL { get; set; }
        DateTime Date { get; set; }
        SubscriptionData Source { get; set; }
        EnclosureData[] Enclosure { get; set; }
    }
}
