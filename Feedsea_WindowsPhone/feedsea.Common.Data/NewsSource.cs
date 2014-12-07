using Microsoft.Phone.Data.Linq.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Data
{
    [global::System.Data.Linq.Mapping.TableAttribute()]
    public partial class NewsSource : INotifyPropertyChanging, INotifyPropertyChanged, ISource
    {
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private string _UrlID;

        private string _Name;

        //private string _Favicon;

        private string _Image;

        private string _Link;

        private int _UnreadNumber;

        private System.Nullable<System.DateTime> _LastFetch;

        private EntitySet<Article> _Articles;

        private EntitySet<NewsSourceCategory> _NewsSourceCategories;

        //private EntitySet<Category> _Categories;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnUrlIDChanging(string value);
        partial void OnUrlIDChanged();
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        partial void OnImageChanging(string value);
        partial void OnImageChanged();
        partial void OnLinkChanging(string value);
        partial void OnLinkChanged();
        partial void OnUnreadNumberChanging(int value);
        partial void OnUnreadNumberChanged();
        partial void OnLastFetchChanging(System.Nullable<System.DateTime> value);
        partial void OnLastFetchChanged();
        #endregion

        public NewsSource()
        {
            this._Articles = new EntitySet<Article>(new Action<Article>(this.attach_Articles), new Action<Article>(this.detach_Articles));
            this._NewsSourceCategories = new EntitySet<NewsSourceCategory>(new Action<NewsSourceCategory>(this.attach_NewsSourceCategories), new Action<NewsSourceCategory>(this.detach_NewsSourceCategories));
            OnCreated();
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UrlID", AutoSync = AutoSync.OnInsert, DbType = "NVarChar(800) NOT NULL", IsPrimaryKey = true, IsDbGenerated = false)]
        public string UrlID
        {
            get
            {
                return this._UrlID;
            }
            set
            {
                if ((this._UrlID != value))
                {
                    this.OnUrlIDChanging(value);
                    this.SendPropertyChanging();
                    this._UrlID = value;
                    this.SendPropertyChanged("UrlID");
                    this.OnUrlIDChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Name", DbType = "NVarChar(1000) NOT NULL", CanBeNull = false)]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                if ((this._Name != value))
                {
                    this.OnNameChanging(value);
                    this.SendPropertyChanging();
                    this._Name = value;
                    this.SendPropertyChanged("Name");
                    this.OnNameChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Image", DbType = "NVarChar(800)", CanBeNull = true)]
        public string Image
        {
            get
            {
                return this._Image;
            }
            set
            {
                if ((this._Image != value))
                {
                    this.OnImageChanging(value);
                    this.SendPropertyChanging();
                    this._Image = value;
                    this.SendPropertyChanged("Image");
                    this.OnImageChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Link", DbType = "NVarChar(800) NOT NULL", CanBeNull = false)]
        public string Link
        {
            get
            {
                return this._Link;
            }
            set
            {
                if ((this._Link != value))
                {
                    this.OnLinkChanging(value);
                    this.SendPropertyChanging();
                    this._Link = value;
                    this.SendPropertyChanged("Link");
                    this.OnLinkChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UnreadNumber", DbType = "Int NOT NULL default(0)", CanBeNull = false)]
        public int UnreadNumber
        {
            get
            {
                return this._UnreadNumber;
            }
            set
            {
                if ((this._UnreadNumber != value))
                {
                    this.OnUnreadNumberChanging(value);
                    this.SendPropertyChanging();
                    this._UnreadNumber = value;
                    this.SendPropertyChanged("UnreadNumber");
                    this.OnUnreadNumberChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LastFetch", DbType = "DateTime")]
        public System.Nullable<System.DateTime> LastFetch
        {
            get
            {
                return this._LastFetch;
            }
            set
            {
                if ((this._LastFetch != value))
                {
                    this.OnLastFetchChanging(value);
                    this.SendPropertyChanging();
                    this._LastFetch = value;
                    this.SendPropertyChanged("LastFetch");
                    this.OnLastFetchChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "FK_NewsSourceCategory_NewsSource", Storage = "_NewsSourceCategories", ThisKey = "UrlID", OtherKey = "NewsSourceID", DeleteRule = "NO ACTION")]
        public EntitySet<NewsSourceCategory> NewsSourceCategories
        {
            get
            {
                return this._NewsSourceCategories;
            }
            set
            {
                this._NewsSourceCategories.Assign(value);
            }
        }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "FK_Article_Source", Storage = "_Articles", ThisKey = "UrlID", OtherKey = "NewsSourceID", DeleteRule = "NO ACTION")]
        public EntitySet<Article> Articles
        {
            get
            {
                return this._Articles;
            }
            set
            {
                this._Articles.Assign(value);
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void attach_Articles(Article entity)
        {
            this.SendPropertyChanging();
            entity.NewsSource = this;
        }

        private void detach_Articles(Article entity)
        {
            this.SendPropertyChanging();
            entity.NewsSource = null;
        }

        private void attach_NewsSourceCategories(NewsSourceCategory entity)
        {
            this.SendPropertyChanging();
            entity.NewsSource = this;
        }

        private void detach_NewsSourceCategories(NewsSourceCategory entity)
        {
            this.SendPropertyChanging();
            entity.NewsSource = null;
        }
    }
}
