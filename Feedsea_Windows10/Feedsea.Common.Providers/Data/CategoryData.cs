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

        private string _urlID;
        public string UrlID
        {
            get { return _urlID; }
            set
            {
                _urlID = value;
                NotifyChange("UrlID");
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyChange("Name");
            }
        }

        private bool _own;
        public bool Own
        {
            get { return _own; }
            set
            {
                _own = value;
                NotifyChange("Own");
            }
        }

        private string _url;
        public string URL
        {
            get { return _url; }
            set
            {
                _url = value;
                NotifyChange("URL");
            }
        }
        
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                NotifyChange("IsSelected");
            }
        }

        private int _unreadNumber;
        public int UnreadNumber
        {
            get { return _unreadNumber; }
            set
            {
                _unreadNumber = value;
                NotifyChange("UnreadNumber");
            }
        }
        
        private List<SubscriptionData> _subscriptions;
        public List<SubscriptionData> Subscriptions
        {
            get { return _subscriptions; }
            set
            {
                _subscriptions = value;
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
