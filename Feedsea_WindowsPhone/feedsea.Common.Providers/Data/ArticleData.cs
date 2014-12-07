using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using feedsea.Common.Providers.Helpers;
using System.Runtime.Serialization;

namespace feedsea.Common.Providers.Data
{
    public class ArticleData : IArticle
    {
        private string uniqueID;
        public string UniqueID
        {
            get { return uniqueID; }
            set
            {
                uniqueID = value;
                NotifyChange("UniqueID");
            }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                NotifyChange("Title");
            }
        }

        private string summary;
        public string Summary
        {
            get { return summary; }
            set
            {
                summary = value;
                NotifyChange("Summary");
            }
        }
        
        private string content;
        public string Content
        {
            get { return content; }
            set
            {
                content = value;
                NotifyChange("Content");
            }
        }

        private string author;
        public string Author
        {
            get { return author; }
            set
            {
                author = value;
                NotifyChange("Author");
            }
        }

        private bool isRead;
        public bool IsRead
        {
            get { return isRead; }
            set
            {
                isRead = value;
                NotifyChange("IsRead");
            }
        }

        private bool isFavorite;
        public bool IsFavorite
        {
            get { return isFavorite; }
            set
            {
                isFavorite = value;
                NotifyChange("IsFavorite");
            }
        }

        private string mainImageUrl;
        public string MainImageUrl
        {
            get { return mainImageUrl; }
            set
            {
                mainImageUrl = value;
                NotifyChange("MainImageUrl");
            }
        }

        private string url;
        public string URL
        {
            get { return url; }
            set
            {
                url = value;
                NotifyChange("URL");
            }
        }

        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                NotifyChange("Date");
            }
        }

        private SubscriptionData source;
        public SubscriptionData Source
        {
            get { return source; }
            set
            {
                source = value;
                NotifyChange("Source");
            }
        }

        private EnclosureData[] enclosure;
        public EnclosureData[] Enclosure
        {
            get { return enclosure; }
            set
            {
                enclosure = value;
                NotifyChange("Enclosure");
            }
        }

        private bool isDataLoaded;

        public bool IsDataLoaded
        {
            get { return isDataLoaded; }
            set
            {
                isDataLoaded = value;
                NotifyChange("IsDataLoaded");
            }
        }

        private string mobilizedUrl;
        public string MobilizedUrl
        {
            get { return mobilizedUrl; }
            set
            {
                mobilizedUrl = value;
                NotifyChange("MobilizedUrl");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public ArticleData()
        {

        }

        public void NotifyChange(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
