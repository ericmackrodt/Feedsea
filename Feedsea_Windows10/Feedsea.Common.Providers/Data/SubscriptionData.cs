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
        private string _image;
        private string _link;
        private int _unreadNumber;
        private string _urlID;
        private string _name;

        public string Image
        {
            get { return _image; }
            set
            {
                _image = value;
                NotifyChange("Image");
            }
        }
        public string Link
        {
            get { return _link; }
            set
            {
                _link = value;
                NotifyChange("Link");
            }
        }
        public int UnreadNumber
        {
            get { return _unreadNumber; }
            set
            {
                _unreadNumber = value;
                NotifyChange("UnreadNumber");
            }
        }
        public string UrlID
        {
            get { return _urlID; }
            set
            {
                _urlID = value;
                NotifyChange("UrlID");
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyChange("Name");
            }
        }

        private CategoryData[] categories;
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
