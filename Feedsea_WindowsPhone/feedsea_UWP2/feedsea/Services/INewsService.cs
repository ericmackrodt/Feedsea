using feedsea.Common.Providers;
using feedsea.Common.Providers.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Services
{

   public interface INewsService
      : INotifyPropertyChanged
   {
      event EventHandler<bool> LoadingChanged;

      INewsSource SelectedSource { get; set; }

      ObservableCollection<ArticleData> Articles { get; set; }

      ObservableCollection<CategoryData> Sources { get; set; }

      Task<bool> Authenticate();

      Task Initialize(string sourceQuery, bool byId = true);

      Task RefreshNews(INewsSource source = null);

      Task LoadMoreNews();

      Task LoadSource(INewsSource source);

      Task ClearSelectedSource();

      Task MarkRead(INewsSource source = null);

      Task ToggleRead(ArticleData article);

      Task ToggleSaved(ArticleData article);

      Task RemoveSource(SubscriptionData source);

      Task RemoveCategory(CategoryData category);

      Task MarkMultipleArticlesRead(IEnumerable<ArticleData> article);

      Task LoadSavedArticles();

      Task<IEnumerable<ArticleData>> GetTileArticles(INewsSource source = null);

      ArticleData GetArticle(string id);

      SubscriptionData GetSubscription(string id);

      void ClearProviderData();

      void NotifyFail();

   }

}