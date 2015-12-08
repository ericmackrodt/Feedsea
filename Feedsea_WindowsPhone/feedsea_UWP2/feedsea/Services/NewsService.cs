using feedsea.Common.MVVM.Tombstone;
using feedsea.Common.Providers;
using feedsea.Common.Providers.Data;
using feedsea.Common.Providers.Feedly;
using feedsea.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Services
{

   [DataContract]
   public class NewsService
      : INewsService
   {
      public event EventHandler<bool> LoadingChanged;
      private INewsProvider provider;
//private IAppSettings settings;
#region Properties
      private INewsSource selectedSource;

      [DataMember]
      public INewsSource SelectedSource
      {
         get
         {
            return selectedSource;
         }
         set
         {
            selectedSource = value;
            NotifyChanged();
         }
      }

      private ObservableCollection<ArticleData> articles;

      [DataMember]
      public ObservableCollection<ArticleData> Articles
      {
         get
         {
            return articles;
         }
         set
         {
            articles = value;
            NotifyChanged();
         }
      }

      private ObservableCollection<CategoryData> sources;

      [DataMember]
      public ObservableCollection<CategoryData> Sources
      {
         get
         {
            return sources;
         }
         set
         {
            sources = value;
            NotifyChanged();
         }
      }
#endregion Properties


#region Constructors
      public NewsService(INewsProvider newsProvider)
      {
         provider = newsProvider;
         //settings = appSettings;
         Articles = new ObservableCollection<ArticleData>();
         Sources = new ObservableCollection<CategoryData>();
      }
#endregion Constructors

#region Public_Methods
      public async Task<bool> Authenticate()
      {
         return await provider.Login() == LoginStatus.Pending;
      }

      public async Task Initialize(string sourceQuery, bool byId = true)
      {
         LoadingChange(true);
         if ( Articles != null && Articles.Any() )
            Articles.Clear();
         Sources = await provider.LoadCategoriesWithSources();
         SelectedSource = null;
         INewsSource source = null;
         if ( byId )
            source = GetSourceById(sourceQuery);
         else
            source = GetSourceByName(sourceQuery);
         var result = await provider.Refresh(source);
         if ( result != null )
         {
            Sources = result.Sources;
            Articles = result.Articles;
            SelectedSource = source;
         }
         LoadingChange(false);
      }

      public async Task RefreshNews(INewsSource source = null)
      {
         LoadingChange(true);
         if ( Articles != null && Articles.Any() )
            Articles.Clear();
         var result = await provider.Refresh(source ?? SelectedSource);
         if ( result != null )
         {
            Sources = result.Sources;
            Articles = result.Articles;
         }
         LoadingChange(false);
      }

      public async Task LoadMoreNews()
      {
         var arts = await provider.LoadMoreArticles(SelectedSource);
         if ( arts != null )
            foreach ( var art in arts )
               Articles.Add(art);
      }

      public async Task LoadSource(INewsSource source)
      {
         LoadingChange(true);
         if ( Articles != null && Articles.Any() )
            Articles.Clear();
         Articles = await provider.LoadArticles(source);
         SelectedSource = source;
         LoadingChange(false);
      }

      public async Task MarkRead(INewsSource source = null)
      {
         LoadingChange(true);
         await provider.MarkAllArticlesRead(source ?? SelectedSource);
         var articlesTask = provider.LoadArticles(source ?? SelectedSource);
         var sourcesTask = provider.LoadCategoriesWithSources();
         Articles = await articlesTask;
         Sources = await sourcesTask;
         LoadingChange(false);
      }

      public async Task ToggleRead(ArticleData article)
      {
         if ( article.IsRead )
            await provider.UnmarkArticleRead(article);
         else
            await provider.MarkArticleRead(article);
         if ( article.Source != null )
            ChangeUnreadNumber(article.IsRead, article.Source.UrlID);
      }

      public async Task ToggleSaved(ArticleData article)
      {
         if ( article.IsFavorite )
            await provider.RemoveFromSaved(article);
         else
            await provider.SaveArticleForLater(article);
      }

      public async Task ClearSelectedSource()
      {
         LoadingChange(true);
         if ( Articles != null && Articles.Any() )
            Articles.Clear();
         Articles = await provider.LoadArticles();
         SelectedSource = null;
         LoadingChange(false);
      }

      public async Task RemoveSource(SubscriptionData source)
      {
         LoadingChange(true);
         SelectedSource = null;
         var data = await provider.RemoveSource(source);
         if ( data != null )
         {
            Articles = data.Articles;
            Sources = data.Sources;
         }
         LoadingChange(false);
      }

      public async Task RemoveCategory(CategoryData category)
      {
         LoadingChange(true);
         var data = await provider.RemoveCategory(category);
         if ( data != null )
         {
            Sources = data.Sources;
            Articles = data.Articles;
         }
         LoadingChange(false);
      }

      public async Task MarkMultipleArticlesRead(IEnumerable<ArticleData> article)
      {
         var result = await provider.MarkMultipleArticlesRead(article);
         if ( result )
         {
            foreach ( var obj in article )
               if ( obj.Source != null )
                  ChangeUnreadNumber(obj.IsRead, obj.Source.UrlID);
         }
      }

      public async Task LoadSavedArticles()
      {
         LoadingChange(true);
         var selectedSource = provider.GetSavedArticlesCategoryItem();
         if ( Articles != null && Articles.Any() )
            Articles.Clear();
         Articles = await provider.LoadArticles(selectedSource);
         SelectedSource = selectedSource;
         LoadingChange(false);
      }

      public void ClearProviderData()
      {
         provider.ClearProviderData();
      }

      public async Task<IEnumerable<ArticleData>> GetTileArticles(INewsSource source = null)
      {
         return await provider.LoadTileArticles(source);
      }

      public void NotifyFail()
      {
         LoadingChange(false);
      }

      public ArticleData GetArticle(string id)
      {
         return Articles.FirstOrDefault(o => o.UniqueID == id);
      }

      public SubscriptionData GetSubscription(string id)
      {
         return Sources.SelectMany(o => o.Subscriptions).FirstOrDefault(o => o.UrlID == id);
      }
#endregion Public_Methods

#region Private_Methods
      private INewsSource GetSourceByName(string query)
      {
         if ( string.IsNullOrWhiteSpace(query) || Sources == null )
            return null;
         INewsSource source = Sources.FirstOrDefault(o => o.Name.ToLower().Contains(query.ToLower()));
         if ( source == null )
            source = Sources.SelectMany(o => o.Subscriptions).FirstOrDefault(o => o.Name.ToLower().Contains(query.ToLower()));
         return source;
      }

      private INewsSource GetSourceById(string id)
      {
         if ( string.IsNullOrWhiteSpace(id) || Sources == null )
            return null;
         INewsSource source = Sources.FirstOrDefault(o => o.UrlID == System.Net.WebUtility.UrlDecode(id));
         if ( source == null )
            source = Sources.SelectMany(o => o.Subscriptions).FirstOrDefault(o => o.UrlID == System.Net.WebUtility.UrlDecode(id));
         return source;
      }

      private void ChangeUnreadNumber(bool isRead, string id)
      {
         var src = Sources.Where(o => o != null && o.Subscriptions != null).SelectMany(o => o.Subscriptions).FirstOrDefault(o => o.UrlID == id);
         if ( src == null )
            return ;
         if ( isRead )
            src.UnreadNumber--;
         else
            src.UnreadNumber++;
      }

      private void LoadingChange(bool isLoading)
      {
         if ( LoadingChanged != null )
            LoadingChanged(this, isLoading);
      }
#endregion Private_Methods

#region INotifyPropertyChanged_Members
      public virtual void NotifyChanged([CallerMemberName]string propertyName = null)
      {
         if ( PropertyChanged != null )
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }

      public event PropertyChangedEventHandler PropertyChanged;
#endregion INotifyPropertyChanged_Members
   }

}