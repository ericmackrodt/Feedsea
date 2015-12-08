using Feedsea.Common.Providers.Data;
using MVVMBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Feedsea.Models
{
    public class MenuItem : ObservableModel, INewsSource
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyChanged();
                }
            }
        }

        private Symbol _symbol;
        public Symbol Symbol
        {
            get { return _symbol; }
            set
            {
                if (_symbol != value)
                {
                    _symbol = value;
                    NotifyChanged();
                }
            }
        }

        public char SymbolAsChar
        {
            get
            {
                return (char)this.Symbol;
            }
        }

        private string _urlID;
        public string UrlID
        {
            get { return _urlID; }
            set
            {
                if (_urlID != value)
                {
                    _urlID = value;
                    NotifyChanged();
                }
            }
        }

        private bool _isMostEngaging;
        public bool IsMostEngaging
        {
            get { return _isMostEngaging; }
            set { _isMostEngaging = value; }
        }

        public int UnreadNumber { get; set; }
    }
}
