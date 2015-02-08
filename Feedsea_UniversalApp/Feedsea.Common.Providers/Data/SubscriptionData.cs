using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Providers.Data
{
    public class SubscriptionData : INewsSource
    {
        private string image;
        private string link;
        private int unreadNumber;
        private string urlID;
        private string name;

        public string Image
        {
            get { return image; }
            set
            {
                image = value;
                NotifyChange("Image");
            }
        }
        public string Link
        {
            get { return link; }
            set
            {
                link = value;
                NotifyChange("Link");
            }
        }
        public int UnreadNumber
        {
            get { return unreadNumber; }
            set
            {
                unreadNumber = value;
                NotifyChange("UnreadNumber");
            }
        }
        public string UrlID
        {
            get { return urlID; }
            set
            {
                urlID = value;
                NotifyChange("UrlID");
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyChange("Name");
            }
        }

        private CategoryData[] categories;
        [DataMember]
        public CategoryData[] Categories
        {
            get { return categories; }
            set
            {
                categories = value;
                NotifyChange("Categories");
            }
        }
        
        public SubscriptionData()
        {
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyChange(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
