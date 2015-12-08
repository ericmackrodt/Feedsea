using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Providers.Data
{
    public interface INewsSource : INotifyPropertyChanged
    {
        string UrlID { get; set; }
        string Name { get; set; }
    }
}
