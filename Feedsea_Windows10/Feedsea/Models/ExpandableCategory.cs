using Feedsea.Common.Providers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using MVVMBasic;
using System.Windows.Input;
using MVVMBasic.Commands;
using System.Collections.ObjectModel;

namespace Feedsea.Models
{
    public class ExpandableCategory : ObservableModel, INewsSource
    {
        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                NotifyChanged();
            }
        }
        
        private CategoryData _category;
        public CategoryData Category
        {
            get { return _category; }
            set
            {
                _category = value;
                NotifyChanged();
            }
        }

        public string Name
        {
            get { return _category.Name; }
            set
            {
                if (_category.Name != value)
                {
                    _category.Name = value;
                    NotifyChanged();
                }
            }
        }
        
        public string UrlID
        {
            get { return _category.UrlID; }
            set
            {
                if (_category.UrlID != value)
                {
                    _category.UrlID = value;
                    NotifyChanged();
                }
            }
        }

        private ICommand _toggleExpandCommand;
        public ICommand ToggleExpandCommand
        {
            get { return _toggleExpandCommand; }
        }

        public int UnreadNumber { get; set; }

        //public static implicit operator ExpandableCategory(CategoryData category)
        //{
        //    if (category == null) throw new ArgumentNullException();
        //    return new ExpandableCategory(category);
        //}
        
        //public static implicit operator CategoryData(ExpandableCategory expCategory)
        //{
        //    if (expCategory == null) throw new ArgumentNullException();
        //    return expCategory.Category;
        //}

        public static explicit operator ExpandableCategory(CategoryData category)
        {
            if (category == null) throw new ArgumentNullException();
            return new ExpandableCategory(category);
        }

        public static explicit operator CategoryData(ExpandableCategory expCategory)
        {
            if (expCategory == null) throw new ArgumentNullException();
            return expCategory.Category;
        }

        public ExpandableCategory(INewsSource category)
        {
            _category = (CategoryData)category;

            //_toggleExpandCommand = new RelayCommand(ToggleExpand);
        }

        //private void ToggleExpand(object obj)
        //{
        //    _isExpanded = !_isExpanded;

        //    if (_isExpanded)
        //    {
        //        var groupHeaderIndex = this.ParentNewsSources.IndexOf(this);
        //        var insertIndex = groupHeaderIndex + 1;

        //        for (int i = 0; i < _category.Subscriptions.Count; i++)
        //        {
        //            this.ParentNewsSources.Insert(insertIndex, _category.Subscriptions[i]);
        //            insertIndex++;
        //        }
        //    }
        //    else
        //    {
        //        foreach (var subscription in _category.Subscriptions)
        //        {
        //            this.ParentNewsSources.Remove(subscription);
        //        }
        //    }
        //}
    }
}
