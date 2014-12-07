using Microsoft.Phone.Data.Linq.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace feedsea.Common.Data
{
    [global::System.Data.Linq.Mapping.TableAttribute()]
    public partial class Category : INotifyPropertyChanging, INotifyPropertyChanged, ISource
    {
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private string _UrlID;

        private string _Name;

        private bool _Own;

        private EntitySet<NewsSourceCategory> _NewsSourceCategories;

        //private EntitySet<NewsSource> _NewsSources;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnUrlIDChanging(string value);
        partial void OnUrlIDChanged();
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        partial void OnOwnChanging(bool value);
        partial void OnOwnChanged();
        #endregion

        public Category()
        {
            this._NewsSourceCategories = new EntitySet<NewsSourceCategory>(new Action<NewsSourceCategory>(this.attach_NewsSourceCategories), new Action<NewsSourceCategory>(this.detach_NewsSourceCategories));
            //this._NewsSources = new EntitySet<NewsSource>(new Action<NewsSource>(this.attach_NewsSources), new Action<NewsSource>(this.detach_NewsSources));
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

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Own", DbType ="Bit NOT NULL default(0)", CanBeNull = false)]
        public bool Own
        {
            get
            {
                return this._Own;
            }
            set
            {
                if ((this._Own != value))
                {
                    this.OnOwnChanging(value);
                    this.SendPropertyChanging();
                    this._Own = value;
                    this.SendPropertyChanged("Own");
                    this.OnOwnChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "FK_NewsSourceCategory_Category", Storage = "_NewsSourceCategories", ThisKey = "UrlID", OtherKey = "CategoryID", DeleteRule = "NO ACTION")]
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

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

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

        private void attach_NewsSourceCategories(NewsSourceCategory entity)
        {
            this.SendPropertyChanging();
            entity.Category = this;
        }

        private void detach_NewsSourceCategories(NewsSourceCategory entity)
        {
            this.SendPropertyChanging();
            entity.Category = null;
        }
    }
}
