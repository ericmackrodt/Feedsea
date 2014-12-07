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
    public partial class NewsSourceCategory
    {
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private string _CategoryID;

        private string _NewsSourceID;

        private EntityRef<Category> _Category;

        private EntityRef<NewsSource> _NewsSource;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnCategoryIDChanging(string value);
        partial void OnCategoryIDChanged();
        partial void OnNewsSourceIDChanging(string value);
        partial void OnNewsSourceIDChanged();
        #endregion

        public NewsSourceCategory()
        {
            this._NewsSource = default(EntityRef<NewsSource>);
            this._Category = default(EntityRef<Category>);
            OnCreated();
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CategoryID", DbType = "NVarChar(800)", IsPrimaryKey = true)]
        public string CategoryID
        {
            get
            {
                return this._CategoryID;
            }
            set
            {
                if ((this._CategoryID != value))
                {
                    if (this._Category.HasLoadedOrAssignedValue)
                    {
                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this.OnCategoryIDChanging(value);
                    this.SendPropertyChanging();
                    this._CategoryID = value;
                    this.SendPropertyChanged("CategoryID");
                    this.OnCategoryIDChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "FK_NewsSourceCategory_Category", Storage = "_Category", ThisKey = "CategoryID", OtherKey = "UrlID", IsForeignKey = true)]
        public Category Category
        {
            get
            {
                return this._Category.Entity;
            }
            set
            {
                Category previousValue = this._Category.Entity;
                if (((previousValue != value)
                            || (this._Category.HasLoadedOrAssignedValue == false)))
                {
                    this.SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        this._Category.Entity = null;
                        previousValue.NewsSourceCategories.Remove(this);
                    }
                    this._Category.Entity = value;
                    if ((value != null))
                    {
                        value.NewsSourceCategories.Add(this);
                        this._CategoryID = value.UrlID;
                    }
                    else
                    {
                        this._CategoryID = default(string);
                    }
                    this.SendPropertyChanged("Category");
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_NewsSourceID", DbType = "NVarChar(800)", IsPrimaryKey = true)]
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

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "FK_NewsSourceCategory_NewsSource", Storage = "_NewsSource", ThisKey = "NewsSourceID", OtherKey = "UrlID", IsForeignKey = true)]
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
                        previousValue.NewsSourceCategories.Remove(this);
                    }
                    this._NewsSource.Entity = value;
                    if ((value != null))
                    {
                        value.NewsSourceCategories.Add(this);
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
