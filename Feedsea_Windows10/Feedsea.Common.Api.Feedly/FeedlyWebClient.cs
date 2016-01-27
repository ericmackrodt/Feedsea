using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using HttpClientHelpers;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Feedsea.Common.Api.Feedly.APIs;

namespace Feedsea.Common.Api.Feedly
{
    public class FeedlyWebClient : IFeedlyClient, IDisposable
    {
        private readonly string baseUrl;
        private readonly IFeedlyApiSettings settings;
        private readonly HttpClient client;

        private IAuthenticationAPI authentication;
        private ICategoriesAPI categories;
        private IEntriesAPI entries;
        private IFeedsAPI feeds;
        private IMarkersAPI markers;
        private IMixesAPI mixes;
        private IPreferencesAPI preferences;
        private IProfileAPI profile;
        private ISearchAPI search;
        private IShortUrlAPI shortURL;
        private IStreamsAPI streams;
        private ISubscriptionsAPI subscriptions;
        private ITagsAPI tags;

        private bool shouldCheckTokenState;
        internal bool ShouldCheckTokenState
        {
            get { return shouldCheckTokenState; }
            set { shouldCheckTokenState = value; }
        }

        public IAuthenticationAPI Authentication
        {
            get
            {
                return authentication ?? (authentication = new AuthenticationAPI(baseUrl, this, settings));
            }
        }

        public ICategoriesAPI Categories
        {
            get
            {
                return categories ?? (categories = new CategoriesAPI(this));
            }
        }

        public IEntriesAPI Entries
        {
            get
            {
                return entries ?? (entries = new EntriesAPI(this));
            }
        }

        public IFeedsAPI Feeds
        {
            get
            {
                return feeds ?? (feeds = new FeedsAPI(this));
            }
        }

        public IMarkersAPI Markers
        {
            get
            {
                return markers ?? (markers = new MarkersAPI(this));
            }
        }

        public IMixesAPI Mixes
        {
            get
            {
                return mixes ?? (mixes = new MixesAPI(this));
            }
        }

        public IPreferencesAPI Preferences
        {
            get
            {
                return preferences ?? (preferences = new PreferencesAPI(this));
            }
        }

        public IProfileAPI Profile
        {
            get
            {
                return profile ?? (profile = new ProfileAPI(this));
            }
        }

        public ISearchAPI Search
        {
            get
            {
                return search ?? (search = new SearchAPI(this));
            }
        }

        public IShortUrlAPI ShortUrl
        {
            get
            {
                return shortURL ?? (shortURL = new ShortUrlAPI(this));
            }
        }

        public IStreamsAPI Streams
        {
            get
            {
                return streams ?? (streams = new StreamsAPI(this));
            }
        }

        public ISubscriptionsAPI Subscriptions
        {
            get
            {
                return subscriptions ?? (subscriptions = new SubscriptionsAPI(this));
            }
        }

        public ITagsAPI Tags
        {
            get
            {
                return tags ?? (tags = new TagsAPI(this));
            }
        }

        public FeedlyWebClient(IFeedlyApiSettings settings)
        {
            shouldCheckTokenState = true;

            this.settings = settings;
            baseUrl = ApiConstants.BaseServiceUrl;

            var handler = new HttpClientHandler();

            if (handler.SupportsAutomaticDecompression)
                handler.AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate;

            this.client = new HttpClient(handler);
            this.client.BaseAddress = new Uri(baseUrl);
            this.client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            UpdateClientAccessToken();
        }

        internal async Task<T> Get<T>(string path)
        {
            await (Authentication as AuthenticationAPI).CheckTokenState();

            var result = await client.GetAsync(path, HttpCompletionOption.ResponseContentRead);
            result.EnsureSuccessStatusCode();
            var data = await result.Content.ReadAsStringAsync();
            //Add Error Handling
            return JsonConvert.DeserializeObject<T>(data.EscapeJson());
        }

        internal async Task Post(string path, object content)
        {
            if (ShouldCheckTokenState)
                await (Authentication as AuthenticationAPI).CheckTokenState();

            var requestContent = new StringContent(JsonConvert.SerializeObject(content), new UTF8Encoding(), "application/json");
            var result = await client.PostAsync(path, requestContent);
            result.EnsureSuccessStatusCode();
            //Add Error Handling
        }

        internal async Task<T> Post<T>(string path, object content)
        {
            if (ShouldCheckTokenState)
                await (Authentication as AuthenticationAPI).CheckTokenState();

            var requestContent = new StringContent(JsonConvert.SerializeObject(content), new UTF8Encoding(), "application/json");
            var result = await client.PostAsync(path, requestContent);
            result.EnsureSuccessStatusCode();
            var data = await result.Content.ReadAsStringAsync();
            //Add Error Handling
            return JsonConvert.DeserializeObject<T>(data.EscapeJson());
        }

        internal async Task Put(string path, object content)
        {
            await (Authentication as AuthenticationAPI).CheckTokenState();

            var requestContent = new StringContent(JsonConvert.SerializeObject(content), new UTF8Encoding(), "application/json");
            var result = await client.PutAsync(path, requestContent);
            result.EnsureSuccessStatusCode();
            //Add Error Handling
        }

        internal async Task Delete(string path)
        {
            await (Authentication as AuthenticationAPI).CheckTokenState();

            var result = await client.DeleteAsync(path);
            result.EnsureSuccessStatusCode();
            //Add Error Handling
        }

        public void Dispose()
        {
            client.Dispose();
        }

        internal void UpdateClientAccessToken() {
            if (!string.IsNullOrWhiteSpace(settings.OAuthToken))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", settings.OAuthToken);
        }
    }
}