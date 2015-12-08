using Feedsea.Common;
using Feedsea.Common.Services;
using Feedsea.Models;
using MVVMBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.ViewModels
{
    public class MenusViewModel : BaseViewModel
    {
        private ObservableCollection<MenuItem> _mainItems;
        public ObservableCollection<MenuItem> MainItems
        {
            get { return _mainItems; }
            set
            {
                if (_mainItems != value)
                {
                    _mainItems = value;
                    NotifyChanged();
                }
            }
        }

        private ObservableCollection<MenuItem> _secondaryItems;
        public ObservableCollection<MenuItem> SecondaryItems
        {
            get { return _secondaryItems; }
            set
            {
                if (_secondaryItems != value)
                {
                    _secondaryItems = value;
                    NotifyChanged();
                }
            }
        }

        public MenusViewModel(IMenuService menuService)
        {
            MainItems = new ObservableCollection<MenuItem>(menuService.MainMenuItems());
        }
    }
}
