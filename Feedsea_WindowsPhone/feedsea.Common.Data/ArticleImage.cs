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
    public partial class ArticleImage : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private int _ArticleImageID;

        private int _ArticleID;

        private string _ImageUrl;

        private string _ImageScope;

        private EntityRef<Article> _Article;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnArticleImageIDChanging(int value);
        partial void OnArticleImageIDChanged();
        partial void OnArticleIDChanging(int value);
        partial void OnArticleIDChanged();
        partial void OnImageUrlChanging(string value);
        partial void OnImageUrlChanged();
        partial void OnImageScopeChanging(string value);
        partial void OnImageScopeChanged();
        #endregion

        public ArticleImage()
        {
            this._Article = default(EntityRef<Article>);
            OnCreated();
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ArticleImageID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int ArticleImageID
        {
            get
            {
                return this._ArticleImageID;
            }
            set
            {
                if ((this._ArticleImageID != value))
                {
                    this.OnArticleImageIDChanging(value);
                    this.SendPropertyChanging();
                    this._ArticleImageID = value;
                    this.SendPropertyChanged("ArticleImageID");
                    this.OnArticleImageIDChanged();
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

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ImageUrl", DbType = "NVarChar(1000) NOT NULL")]
        public string ImageUrl
        {
            get
            {
                return this._ImageUrl;
            }
            set
            {
                if ((this._ImageUrl != value))
                {
                    this.OnImageUrlChanging(value);
                    this.SendPropertyChanging();
                    this._ImageUrl = value;
                    this.SendPropertyChanged("ImageUrl");
                    this.OnImageUrlChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ImageScope", DbType = "NVarChar(100) NOT NULL")]
        public string ImageScope
        {
            get
            {
                return this._ImageScope;
            }
            set
            {
                if ((this._ImageScope != value))
                {
                    this.OnImageScopeChanging(value);
                    this.SendPropertyChanging();
                    this._ImageScope = value;
                    this.SendPropertyChanged("ImageScope");
                    this.OnImageScopeChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "FK_ArticleImage_Article", Storage = "_Article", ThisKey = "ArticleID", OtherKey = "ArticleID", IsForeignKey = true)]
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
                        previousValue.ArticleImages.Remove(this);
                    }
                    this._Article.Entity = value;
                    if ((value != null))
                    {
                        value.ArticleImages.Add(this);
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
