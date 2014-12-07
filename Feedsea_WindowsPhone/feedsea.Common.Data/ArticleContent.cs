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
    public partial class ArticleContent : INotifyPropertyChanging, INotifyPropertyChanged
    {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private int _ArticleContentID;

        private int _ArticleID;

        private string _Content;

        private System.Nullable<int> _ContentOrder;

        private EntityRef<Article> _Article;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnArticleContentIDChanging(int value);
        partial void OnArticleContentIDChanged();
        partial void OnArticleIDChanging(int value);
        partial void OnArticleIDChanged();
        partial void OnContentChanging(string value);
        partial void OnContentChanged();
        partial void OnContentOrderChanging(System.Nullable<int> value);
        partial void OnContentOrderChanged();
        #endregion

        public ArticleContent()
        {
            this._Article = default(EntityRef<Article>);
            OnCreated();
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ArticleContentID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int ArticleContentID
        {
            get
            {
                return this._ArticleContentID;
            }
            set
            {
                if ((this._ArticleContentID != value))
                {
                    this.OnArticleContentIDChanging(value);
                    this.SendPropertyChanging();
                    this._ArticleContentID = value;
                    this.SendPropertyChanged("ArticleContentID");
                    this.OnArticleContentIDChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ArticleID", DbType = "Int NOT NULL")]
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
                    if (this._Article.HasLoadedOrAssignedValue)
                    {
                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this.OnArticleIDChanging(value);
                    this.SendPropertyChanging();
                    this._ArticleID = value;
                    this.SendPropertyChanged("ArticleID");
                    this.OnArticleIDChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Content", DbType = "NText", UpdateCheck = UpdateCheck.Never)]
        public string Content
        {
            get
            {
                return this._Content;
            }
            set
            {
                if ((this._Content != value))
                {
                    this.OnContentChanging(value);
                    this.SendPropertyChanging();
                    this._Content = value;
                    this.SendPropertyChanged("Content");
                    this.OnContentChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ContentOrder", DbType = "Int")]
        public System.Nullable<int> ContentOrder
        {
            get
            {
                return this._ContentOrder;
            }
            set
            {
                if ((this._ContentOrder != value))
                {
                    this.OnContentOrderChanging(value);
                    this.SendPropertyChanging();
                    this._ContentOrder = value;
                    this.SendPropertyChanged("ContentOrder");
                    this.OnContentOrderChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "FK_ArticleContent_Article", Storage = "_Article", ThisKey = "ArticleID", OtherKey = "ArticleID", IsForeignKey = true)]
        public Article Article
        {
            get
            {
                return this._Article.Entity;
            }
            set
            {
                Article previousValue = this._Article.Entity;
                if (((previousValue != value)
                            || (this._Article.HasLoadedOrAssignedValue == false)))
                {
                    this.SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        this._Article.Entity = null;
                        previousValue.ArticleContents.Remove(this);
                    }
                    this._Article.Entity = value;
                    if ((value != null))
                    {
                        value.ArticleContents.Add(this);
                        this._ArticleID = value.ArticleID;
                    }
                    else
                    {
                        this._ArticleID = default(int);
                    }
                    this.SendPropertyChanged("Article");
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
    }
}
