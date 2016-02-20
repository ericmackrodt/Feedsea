using Feedsea.Common.Providers.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.Foundation;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Feedsea.Common.Controls
{
    public class PaginatedArticlesCollection : ObservableCollection<ArticleData>, ISupportIncrementalLoading
    {
        private Func<Task<IEnumerable<ArticleData>>> loadMore;

        public bool HasMoreItems { get; protected set; }

        public PaginatedArticlesCollection(IEnumerable<ArticleData> articles, Func<Task<IEnumerable<ArticleData>>> loadMore)
            : base(articles)
        {
            HasMoreItems = true;
            this.loadMore = loadMore;
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return AsyncInfo.Run(async c =>
            {
                var data = await loadMore();

                AddMultiple(data);

                HasMoreItems = data.Any();

                return new LoadMoreItemsResult()
                {
                    Count = (uint)data.Count()
                };
            });
        }

        public void AddMultiple(IEnumerable<ArticleData> articles)
        {
            foreach (var item in articles)
            {
                Add(item);
            }
        }
    }
}
