using feedsea.Common.Providers;
using feedsea.Common.Providers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Models
{
    public class AdItemModel : IArticle
    {
        public string Author { get; set; }

        public string Content { get; set; }

        public DateTime Date { get; set; }

        public EnclosureData[] Enclosure { get; set; }

        public bool IsFavorite { get; set; }

        public bool IsRead { get; set; }

        public string MainImageUrl { get; set; }

        public SubscriptionData Source { get; set; }

        public string Summary { get; set; }

        public string Title { get; set; }

        public string URL { get; set; }

        public string UniqueID { get; set; }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
