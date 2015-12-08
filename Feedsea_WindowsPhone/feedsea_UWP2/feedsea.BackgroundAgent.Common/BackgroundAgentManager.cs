using feedsea.BackgroundAgent.Common.TileControls;
using feedsea.Common.Api.Feedly;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using feedsea.BackgroundAgent.Common.Helpers;
using System.IO;
using Windows.UI.Xaml;
using System.Net;
using feedsea.BackgroundAgent.Common.Resources;
using Windows.UI.Xaml.Controls;
using System.Net.Http;
using feedsea.BackgroundAgent.Common.ContentBuilder;

namespace feedsea.BackgroundAgent.Common
{

   public delegate void TileCreactionEnded(bool error);

   public class BackgroundAgentManager
   {
      public event TileCreactionEnded CreationEnded;
      private readonly string _feedlyUserId;
      private readonly string _feedlyToken;
      private FeedlyWebClient _client;
      private CountsResponse _counts;
      private Subscription[] _subscriptions;
      private FeedCategory[] _categories;
      private TileMode _tileMode;
      private ITileContentBuilder _builder = null;

      public Func<long> ApplicationMemoryUsage { get; set; }

      public Func<long> ApplicationMemoryLimit { get; set; }

      public DateTime ExecutionStarted { get; set; }

      public ITileBuilder TileBuilder { get; set; }


      public BackgroundAgentManager()
      {
         _feedlyUserId = (string)Windows.Storage.ApplicationData.Current.LocalSettings.Values["GeneralSettings_UserIdSetting"];
         _feedlyToken = (string)Windows.Storage.ApplicationData.Current.LocalSettings.Values["GeneralSettings_OAuthTokenSetting"];
         if ( Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey("LiveTileSettings_TileModeSetting") )
            _tileMode = (TileMode)Windows.Storage.ApplicationData.Current.LocalSettings.Values["LiveTileSettings_TileModeSetting"];
         else
            _tileMode = TileMode.Normal;
         _client = new FeedlyWebClient(_feedlyToken);
      }

      public async Task Start()
      {
         await DownloadBaseData();
         await DownloadAndUpdateTiles();
         //var data = await DownloadTileData();
         if ( ShouldStopWorking() )
            return ;
      //BuildTiles(data);
      }

      public async Task GetImages(LiveTileData data)
      {
         using ( var cli = new HttpClient() )
         {
            if ( !string.IsNullOrWhiteSpace(data.Front.Favicon) )
               data.Front.FaviconBytes = await data.Front.Favicon.GetFaviconUrl().DownloadImage(cli);
            if ( _tileMode != TileMode.Transparent && !string.IsNullOrWhiteSpace(data.Front.Image) && data.Front.Image != "(none)" )
               data.Front.ImageBytes = await data.Front.Image.GetLiveTileBackUrl().DownloadImage(cli);
            if ( !string.IsNullOrWhiteSpace(data.Back.Favicon) )
               data.Back.FaviconBytes = await data.Back.Favicon.GetFaviconUrl().DownloadImage(cli);
            if ( _tileMode != TileMode.Transparent && !string.IsNullOrWhiteSpace(data.Back.Image) && data.Front.Image != "(none)" )
               data.Back.ImageBytes = await data.Back.Image.GetLiveTileBackUrl().DownloadImage(cli);
         }
      }

      public async System.Threading.Tasks.Task DownloadAndUpdateTiles()
      {
         var currentTiles = ((System.Collections.Generic.IEnumerable<Windows.UI.StartScreen.SecondaryTile>)await Windows.UI.StartScreen.SecondaryTile.FindAllForPackageAsync());
         Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
               var error = false;
               try
               {
                  using ( var storage = IsolatedStorageFile.GetUserStoreForApplication() )
                  {
                     foreach ( var t in currentTiles )
                     {
                        var data = await DownloadData(t.NavigationUri);
                        if ( data.Front != null && data.Back != null )
                        {
                           await GetImages(data);
                           if ( ShouldStopWorking() )
                              break;
                           await BuildTiles(data, storage).UpdateAsync();
                        }
                        if ( ShouldStopWorking() )
                           break;
                     }
                  }
               }
               catch
               {
                  error = true;
               }
               if ( CreationEnded != null )
                  CreationEnded(error);
            });
      }

      public async Task<Dictionary<ShellTile, LiveTileData>> DownloadTileData()
      {
         try
         {
            var currentTiles = ((System.Collections.Generic.IEnumerable<Windows.UI.StartScreen.SecondaryTile>)await Windows.UI.StartScreen.SecondaryTile.FindAllForPackageAsync());
            var dictionary = new Dictionary<ShellTile, LiveTileData>();
            foreach ( var t in currentTiles )
            {
               var data = await DownloadData(t.NavigationUri);
               if ( data.Front != null && data.Back != null )
               {
                  await GetImages(data);
                  dictionary.Add(t, data);
               }
            }
            return dictionary;
         }
         catch ( HttpRequestException )
         {
            throw new TileCreationException(ExceptionType.NoNetworkAccess);
         }
      }

      public async Task DownloadBaseData()
      {
         try
         {
            var countsTask = _client.GetCounts();
            var subscriptionsTask = _client.GetSubscriptions();
            var categoriesTask = _client.GetCategories();
            _counts = await countsTask;
            _subscriptions = await subscriptionsTask;
            _categories = await categoriesTask;
         }
         catch ( HttpRequestException ex )
         {
            throw new TileCreationException(ExceptionType.NoNetworkAccess);
         }
      }

      public ShellTileData BuildTiles(LiveTileData data, IsolatedStorageFile storage)
      {
         if ( _builder == null )
            _builder = new LiveTile();
         //Wide Front
         _builder.BuildFront(data.Front, _tileMode, data.Type, data.Title, data.Count);
         var frontWideSuffix = GetTileFileName(data.Title, LiveTileSettings.MainTileWideFrontFileName);
         var frontWideFilepath = Path.Combine(LiveTileSettings.FolderShellContent, string.Concat(Guid.NewGuid(), frontWideSuffix));
         _builder.SaveJpeg(TileSize.Wide, data.Type, LiveTileSettings.FolderShellContent, frontWideSuffix, frontWideFilepath, storage);
         //Medium Front
         var frontMediumSuffix = GetTileFileName(data.Title, LiveTileSettings.MainTileMediumFrontFileName);
         var frontMediumFilepath = Path.Combine(LiveTileSettings.FolderShellContent, string.Concat(Guid.NewGuid(), frontMediumSuffix));
         _builder.SaveTile(TileSize.Medium, data.Type, LiveTileSettings.FolderShellContent, frontMediumSuffix, frontMediumFilepath, storage);
         //Wide Back
         _builder.BuildBack(data.Back, _tileMode, data.Type);
         var backWideSuffix = GetTileFileName(data.Title, LiveTileSettings.MainTileWideBackFileName);
         var backWideFilepath = Path.Combine(LiveTileSettings.FolderShellContent, string.Concat(Guid.NewGuid(), backWideSuffix));
         _builder.SaveTile(TileSize.Wide, data.Type, LiveTileSettings.FolderShellContent, backWideSuffix, backWideFilepath, storage);
         //Medium Back
         var backMediumSuffix = GetTileFileName(data.Title, LiveTileSettings.MainTileMediumBackFileName);
         var backMediumFilepath = Path.Combine(LiveTileSettings.FolderShellContent, string.Concat(Guid.NewGuid(), backMediumSuffix));
         _builder.SaveTile(TileSize.Medium, data.Type, LiveTileSettings.FolderShellContent, backMediumSuffix, backMediumFilepath, storage);
         data.Front.ImageBytes = null;
         data.Front.FaviconBytes = null;
         data.Back.ImageBytes = null;
         data.Back.FaviconBytes = null;
         return new FlipTileData()
         {
            WideBackgroundImage = new Uri(new Uri("ms-appx://"), "isostore:" + frontWideFilepath), WideBackBackgroundImage = new Uri(new Uri("ms-appx://"), "isostore:" + backWideFilepath), BackgroundImage = new Uri(new Uri("ms-appx://"), "isostore:" + frontMediumFilepath), BackBackgroundImage = new Uri(new Uri("ms-appx://"), "isostore:" + backMediumFilepath)
         };
      }

      private async Task<LiveTileData> DownloadData(Uri source)
      {
         var urlId = "";
         var type = TileType.Main;
         var title = "";
         if ( source.ToString() == "/" )
            urlId = ApiConstants.GlobalCategory_All;
         else
         {
            var src = source.ToString();
            var query = src.Contains("open=") ? System.Net.WebUtility.UrlDecode(src.ToString().Split(new string[] { "open=" }, StringSplitOptions.RemoveEmptyEntries)[1]) : "";
            urlId = query;
            var isCategory = !urlId.StartsWith("feed");
            type = isCategory ? TileType.Category : TileType.Subscription;
            if ( isCategory )
            {
               var cat = _categories.FirstOrDefault(o => o.Id == query);
               if ( cat == null )
               {
                  Strings.Culture = System.Globalization.CultureInfo.CurrentCulture;
                  title = Strings.Uncategorized;
               }
               else
                  title = cat.Label;
            }
            else
            {
               var sub = _subscriptions.FirstOrDefault(o => o.Id == query);
               title = sub.Title;
            }
         }
         urlId = urlId.Replace(ApiConstants.FormatKey_UserId, _feedlyUserId);
         var stream = await _client.GetStream(urlId, count: 2);
         var count = _counts.UnreadCounts.FirstOrDefault(o => o.Id == urlId);
         var data = new LiveTileData()
            {
               Count = count == null ? 0 : count.Count, NavigationUri = source, Type = type, Title = title
            };
         if ( stream.Items.Any() )
         {
            var front = stream.Items.First();
            var tileSub = _subscriptions.First(o => o.Id == front.Origin.StreamId);
            data.Front = new TileFace()
               {
                  Favicon = tileSub.Website, SourceID = tileSub.Id, Image = front.Visual == null || front.Visual.Url == "(none)" || front.Visual.Url == "none" ? null : front.Visual.Url, SourceName = tileSub.Title, Title = front.Title
               };
            var back = stream.Items.Last();
            tileSub = _subscriptions.First(o => o.Id == back.Origin.StreamId);
            data.Back = new TileFace()
               {
                  Favicon = tileSub.Website, SourceID = tileSub.Id, Image = back.Visual == null || back.Visual.Url == "(none)" || back.Visual.Url == "none" ? null : back.Visual.Url, SourceName = tileSub.Title, Title = back.Title
               };
         }
         return data;
      }

      private void FreeMemory()
      {
         GC.Collect();
         GC.WaitForPendingFinalizers();
         GC.Collect();
      }

      private bool ShouldStopWorking()
      {
         if ( !System.Diagnostics.Debugger.IsAttached )
         {
            if ( (DateTime.Now - ExecutionStarted).Seconds > 23 )
               return true;
         }
         if ( ShouldFreeMemory() )
            FreeMemory();
         return ShouldFreeMemory();
      }

      private bool ShouldFreeMemory()
      {
         double highPercentage = 0.95;
         double highMemory = ApplicationMemoryLimit() * highPercentage;
         return ApplicationMemoryUsage() > highMemory;
      }

      private string GetTileFileName(string title, string suffix)
      {
         var ttl = string.Join("", title.ToLower().Select(o => char.IsLetterOrDigit(o) ? o.ToString() : ""));
         return string.Format("{0}_{1}", ttl, suffix);
      }

   }

}