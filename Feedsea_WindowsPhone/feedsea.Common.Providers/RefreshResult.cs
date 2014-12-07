using feedsea.Common.Providers.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Providers
{
    public class RefreshResult
    {
        public ObservableCollection<ArticleData> Articles { get; set; }
        public ObservableCollection<CategoryData> Sources { get; set; }
    }
}
