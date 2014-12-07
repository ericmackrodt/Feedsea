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
    public partial class Article : INotifyPropertyChanging, INotifyPropertyChanged
    {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private int _ArticleID;

        private string _NewsSourceID;

        private System.Nullable<System.DateTime> _Date;

        private string _Title;

        private string _Summary;

        private string _Author;

        private string _URL;

        private bool _IsRead;

        private bool _IsFavorite;

        private string _UniqueID;

        private EntityRef<NewsSource> _NewsSource;

        private EntitySet<ArticleContent> _ArticleContents;

        private EntitySet<ArticleImage> _ArticleImages;

        private EntityRef<ReadLater> _ReadLater;

        private EntityRef<ReadQueue> _ReadQueue;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnArticleIDChanging(int value);
        partial void OnArticleIDChanged();
        partial void OnNewsSourceIDChanging(string value);
        partial void OnNewsSourceIDChanged();
        partial void OnDateChanging(System.Nullable<System.DateTime> value);
        partial void OnDateChanged();
        partial void OnTitleChanging(string value);
        partial void OnTitleChanged();
        partial void OnSummaryChanging(string value);
        partial void OnSummaryChanged();
        partial void OnAuthorChanging(string value);
        partial void OnAuthorChanged();
        partial void OnURLChanging(string value);
        partial void OnURLChanged();
        partial void OnIsReadChanging(bool value);
        partial void OnIsReadChanged();
        partial void OnIsFavoriteChanging(bool value);
        partial void OnIsFavoriteChanged();
        partial void OnUniqueIDChanging(string value);
        partial void OnUniqueIDChanged();
        #endregion

        public Article()
        {
            this._NewsSource = default(EntityRef<NewsSource>);
            this._ArticleContents = new EntitySet<ArticleContent>(new Action<ArticleContent>(this.attach_ArticleContents), new Action<ArticleContent>(this.detach_ArticleContents));
            this._ArticleImages = new EntitySet<ArticleImage>(new Action<ArticleImage>(this.attach_ArticleImages), new Action<ArticleImage>(this.detach_ArticleImages));
            this._ReadLater = default(EntityRef<ReadLater>);
            this._ReadQueue = default(EntityRef<ReadQueue>);
            OnCreated();
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ArticleID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int ArticleID
        {
            get
            {
                return this._ArticleID;
            }
            set
            {
                if ((this._ArticleID != value))
                {
                    this.OnArticleIDChanging(value);
                    this.SendPropertyChanging();
                    this._ArticleID = value;
                    this.SendPropertyChanged("ArticleID");
                    this.OnArticleIDChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_NewsSourceID", DbType = "NVarChar(800)")]
        public string NewsSourceID
        {
            get
            {
                return this._NewsSourceID;
            }
            set
            {
                if ((this._NewsSourceID != value))
                {
                    if (this._NewsSource.HasLoadedOrAssignedValue)
                    {
                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this.OnNewsSourceIDChanging(value);
                    this.SendPropertyChanging();
                    this._NewsSourceID = value;
                    this.SendPropertyChanged("NewsSourceID");
                    this.OnNewsSourceIDChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Date", DbType = "DateTime")]
        public System.Nullable<System.DateTime> Date
        {
            get
            {
                return this._Date;
            }
            set
            {
                if ((this._Date != value))
                {
                    this.OnDateChanging(value);
                    this.SendPropertyChanging();
                    this._Date = value;
                    this.SendPropertyChanged("Date");
                    this.OnDateChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Title", DbType = "NVarChar(4000)")]
        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                if ((this._Title != value))
                {
                    this.OnTitleChanging(value);
                    this.SendPropertyChanging();
                    this._Title = value;
                    this.SendPropertyChanged("Title");
                    this.OnTitleChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Summary", DbType = "NVarChar(4000)")]
        public string Summary
        {
            get
            {
                return this._Summary;
            }
            set
            {
                if ((this._Summary != value))
                {
                    this.OnSummaryChanging(value);
                    this.SendPropertyChanging();
                    this._Summary = value;
                    this.SendPropertyChanged("Summary");
                    this.OnSummaryChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Author", DbType = "NVarChar(256)")]
        public string Author
        {
            get
            {
                return this._Author;
            }
            set
            {
                if ((this._Author != value))
                {
                    this.OnAuthorChanging(value);
                    this.SendPropertyChanging();
                    this._Author = value;
                    this.SendPropertyChanged("Author");
                    this.OnAuthorChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_URL", DbType = "NVarChar(800)")]
        public string URL
        {
            get
            {
                return this._URL;
            }
            set
            {
                if ((this._URL != value))
                {
                    this.OnURLChanging(value);
                    this.SendPropertyChanging();
                    this._URL = value;
                    this.SendPropertyChanged("URL");
                    this.OnURLChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IsRead", DbType = "Bit NOT NULL")]
        public bool IsRead
        {
            get
            {
                return this._IsRead;
            }
            set
            {
                if ((this._IsRead != value))
                {
                    this.OnIsReadChanging(value);
                    this.SendPropertyChanging();
                    this._IsRead = value;
                    this.SendPropertyChanged("IsRead");
                    this.OnIsReadChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IsFavorite", DbType = "Bit NOT NULL")]
        public bool IsFavorite
        {
            get
            {
                return this._IsFavorite;
            }
            set
            {
                if ((this._IsFavorite != value))
                {
                    this.OnIsFavoriteChanging(value);
                    this.SendPropertyChanging();
                    this._IsFavorite = value;
                    this.SendPropertyChanged("IsFavorite");
                    this.OnIsFavoriteChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UniqueID", DbType = "NVarChar(800)")]
        public string UniqueID
        {
            get
            {
                return this._UniqueID;
            }
            set
            {
                if ((this._UniqueID != value))
                {
                    this.OnUniqueIDChanging(value);
                    this.SendPropertyChanging();
                    this._UniqueID = value;
                    this.SendPropertyChanged("UniqueID");
                    this.OnUniqueIDChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "FK_Article_NewsSource", Storage = "_NewsSource", ThisKey = "NewsSourceID", OtherKey = "UrlID", IsForeignKey = true)]
        public NewsSource NewsSource
        {
            get
            {
                return this._NewsSource.Entity;
            }
            set
            {
                NewsSource previousValue = this._NewsSource.Entity;
                if (((previousValue != value)
                            || (this._NewsSource.HasLoadedOrAssignedValue == false)))
                {
                    this.SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        this._NewsSource.Entity = null;
                        previousValue.Articles.Remove(this);
                    }
                    this._NewsSource.Entity = value;
                    if ((value != null))
                    {
                        value.Articles.Add(this);
                        this._NewsSourceID = value.UrlID;
                    }
                    else
                    {
                        this._NewsSourceID = default(string);
                    }
                    this.SendPropertyChanged("NewsSource");
                }
            }
        }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "FK_ArticleContent_Article", Storage = "_ArticleContents", ThisKey = "ArticleID", OtherKey = "ArticleID", DeleteRule = "NO ACTION")]
        public EntitySet<ArticleContent> ArticleContents
        {
            get
            {
                return this._ArticleContents;
            }
            set
            {
                this._ArticleContents.Assign(value);
            }
        }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "FK_ArticleImage_Article", Storage = "_ArticleImages", ThisKey = "ArticleID", OtherKey = "ArticleID", DeleteRule = "NO ACTION")]
        public EntitySet<ArticleImage> ArticleImages
        {
            get
            {
                return this._ArticleImages;
            }
            set
            {
                this._ArticleImages.Assign(value);
            }
        }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "FK_ReadLater_Article", Storage = "_ReadLater", ThisKey = "ArticleID", OtherKey = "ArticleID", IsUnique = true, IsForeignKey = false, DeleteRule = "NO ACTION")]
        public ReadLater ReadLater
        {
            get
            {
                return this._ReadLater.Entity;
            }
            set
            {
                ReadLater previousValue = this._ReadLater.Entity;
                if (((previousValue != value)
                            || (this._ReadLater.HasLoadedOrAssignedValue == false)))
                {
                    this.SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        this._ReadLater.Entity = null;
                        previousValue.Article = null;
                    }
                    this._ReadLater.Entity = value;
                    if ((value != null))
                    {
                        value.Article = this;
                    }
                    this.SendPropertyChanged("ReadLater");
                }
            }
        }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "FK_ReadQueue_Article", Storage = "_ReadQueue", ThisKey = "ArticleID", OtherKey = "ArticleID", IsUnique = true, IsForeignKey = false, DeleteRule = "NO ACTION")]
        public ReadQueue ReadQueue
        {
            get
            {
                return this._ReadQueue.Entity;
            }
            set
            {
                ReadQueue previousValue = this._ReadQueue.Entity;
                if (((previousValue != value)
                            || (this._ReadQueue.HasLoadedOrAssignedValue == false)))
                {
                    this.SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        this._ReadQueue.Entity = null;
                        previousValue.Article = null;
                    }
                    this._ReadQueue.Entity = value;
                    if ((value != null))
                    {
                        value.Article = this;
                    }
                    this.SendPropertyChanged("ReadQueue");
                }
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

        private void attach_ArticleContents(ArticleContent entity)
        {
            this.SendPropertyChanging();
            entity.Article = this;
        }

        private void detach_ArticleContents(ArticleContent entity)
        {
            this.SendPropertyChanging();
            entity.Article = null;
        }

        private void attach_ArticleImages(ArticleImage entity)
        {
            this.SendPropertyChanging();
            entity.Article = this;
        }

        private void detach_ArticleImages(ArticleImage entity)
        {
            this.SendPropertyChanging();
            entity.Article = null;
        }
    }
}
