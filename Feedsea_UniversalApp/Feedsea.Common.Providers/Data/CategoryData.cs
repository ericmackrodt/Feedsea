using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Feedsea.Common.Providers.Data
{
    public class CategoryData : INewsSource
    {
        public CategoryData()
        {
        }

        private string urlID;
        public string UrlID
        {
            get { return urlID; }
            set
            {
                urlID = value;
                NotifyChange("UrlID");
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyChange("Name");
            }
        }

        private bool own;
        public bool Own
        {
            get { return own; }
            set
            {
                own = value;
                NotifyChange("Own");
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
        
        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                NotifyChange("IsSelected");
            }
        }

        private List<SubscriptionData> subscriptions;
        [DataMember]
        public List<SubscriptionData> Subscriptions
        {
            get { return subscriptions; }
            set
            {
                subscriptions = value;
                NotifyChange("Subscriptions");
            }
        }
                
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyChange(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
