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

namespace Feedsea.Common.Api.Feedly
{
    public class FeedlyWebClient : IFeedlyClient, IDisposable
    {
        private readonly string _baseUrl;
        private readonly IFeedlyApiSettings _settings;
        private readonly HttpClient _client;

        public FeedlyWebClient(IFeedlyApiSettings settings)
        {
            _settings = settings;
            _baseUrl = ApiConstants.BaseServiceUrl;

            var handler = new HttpClientHandler();

            if (handler.SupportsAutomaticDecompression)
                handler.AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate;

            _client = new HttpClient(handler);
            _client.BaseAddress = new Uri(_baseUrl);
            _client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            UpdateClientAccessToken();
        }

        public string GetLoginUrl()
        {
            return QueryHelper.BuildRequestUrl(_baseUrl, "auth/auth", new 
            {
                scope = "http://cloud.feedly.com/subscriptions",
                response_type = "code",
                client_id = _settings.ClientID,
                redirect_uri = ApiConstants.LoginDefaultRedirectUrl,
                state = ""
            }.BuildQueryString());
        }

        public async Task RequestAccessToken(AuthTokenRequest request)
        {
            request.GrantType = "authorization_code";
            var response = await Post<AuthTokenResponse>("auth/token", request);
            _settings.OAuthToken = response.AccessToken;
            _settings.OAuthRefreshToken = response.RefreshToken;
            _settings.OAuthTokenExpiration = DateTime.Now.AddSeconds(response.ExpiresIn - 1);
            _settings.UserID = response.Id;
        }

        public async Task RefreshToken()
        {
            var request = new RefreshTokenRequest()
            {
                GrantType = "refresh_token",
                ClientId = _settings.ClientID,
                ClientSecret = _settings.ClientSecret,
                RefreshToken = _settings.OAuthRefreshToken
            };

            var response = await Post<AuthTokenResponse>("auth/token", request);
            _settings.OAuthToken = response.AccessToken;
            _settings.OAuthTokenExpiration = DateTime.Now.AddSeconds(response.ExpiresIn - 1);
        }

        public async Task<Profile> GetProfile()
        {
            return await Get<Profile>("profile");
        }

        public async Task<FeedCategory[]> GetCategories()
        {
            return await Get<FeedCategory[]>("categories");
        }

        public async Task Subscribe(string url, string title, FeedCategory[] categories = null)
        {
            var content = new Subscription() 
            {
                Id = url,
                Title = title,
                Categories = categories
            };
            await Post("subscriptions", content);
        }

        public async Task<Subscription[]> GetSubscriptions()
        {
            return await Get<Subscription[]>("subscriptions");
        }

        public async Task<Topic[]> GetTopics()
        {
            return await Get<Topic[]>("topics");
        }

        public async Task AddTopic(Topic topic)
        {
            await Post("topics", topic);
        }

        public async Task<SearchResponse> SearchFeeds(string query)
        {
            return await Get<SearchResponse>(QueryHelper.BuildRequestUrl("search/feeds", new { q  = Uri.EscapeDataString(query) }.BuildQueryString()));
        }

        public async Task<SearchResponse> SearchFeeds(string query, int numberOfResults)
        {
            return await Get<SearchResponse>(QueryHelper.BuildRequestUrl("search/feeds", new { q = Uri.EscapeDataString(query), n = numberOfResults }.BuildQueryString()));
        }

        public async Task<FeedStream> GetStream(string id, string continuation = null, int? count = null, long? newerThan = null, Ranked? ranked = null, bool? unreadOnly = null)
        {
            return await Get<FeedStream>(QueryHelper.BuildRequestUrl("streams/contents", new
            {
                streamId = id,
                continuation = continuation,
                count = count,
                newerThan = newerThan,
                ranked = ranked != null ? ranked.ToString().ToLower() : null,
                unreadOnly = unreadOnly
            }.BuildQueryString()));
        }

        public async Task<FeedStream> GetMixes(string id, string continuation, int? count = null, int? hours = null, long? newerThan = null, bool? backFill = null, bool? unreadOnly = null)
        {
            return await Get<FeedStream>(QueryHelper.BuildRequestUrl("mixes/contents", new
            {
                streamId = id,
                continuation = continuation,
                count = count,
                newerThan = newerThan,
                hours = hours,
                backFill = backFill,
                unreadOnly = unreadOnly
            }.BuildQueryString()));
        }

        public async Task<CountsResponse> GetCounts(bool? autoRefresh = null, long? newerThan = null)
        {
            return await Get<CountsResponse>(QueryHelper.BuildRequestUrl("markers/counts", new
            {
                autoRefresh = autoRefresh,
                newerThan = newerThan
            }.BuildQueryString()));
        }

        public async Task MarkRead(IMarkerInput input)
        {
            input.Action = "markAsRead";
            await Post("markers", input);
        }

        public async Task KeepUnread(MarkerInputEntries input)
        {
            input.Action = "keepUnread";
            await Post("markers", input);
        }

        public async Task<ReadOperations> GetLastestReadOperations()
        {
            return await Get<ReadOperations>("markers/reads");
        }

        public async Task DeleteCategory(string id)
        {
            await Delete(string.Concat("categories/", id));
        }

        public async Task DeleteSubscription(string id)
        {
            await Delete(string.Concat("subscriptions/", id));
        }

        public async Task<Entry> GetEntryContent(string entryId)
        {
            return await Get<Entry>(string.Concat("entries/", entryId));
        }

        public async Task SaveEntry(TagEntryInput entry)
        {
            await Put(string.Concat("tags/", WebUtility.UrlEncode(ApiConstants.GlobalTag_Saved.Replace(ApiConstants.FormatKey_UserId, entry.UserId))), entry);
        }

        public async Task RemoveFromSaved(TagEntryInput entry)
        {
            await Delete(string.Format("tags/{0}/{1}", WebUtility.UrlEncode(ApiConstants.GlobalTag_Saved.Replace(ApiConstants.FormatKey_UserId, entry.UserId)), WebUtility.UrlEncode(entry.EntryId)));
        }

        private async Task<T> Get<T>(string path)
        {
            await CheckTokenState();

            var result = await _client.GetAsync(path, HttpCompletionOption.ResponseContentRead);
            result.EnsureSuccessStatusCode();
            var data = await result.Content.ReadAsStringAsync();
            //Add Error Handling
            return JsonConvert.DeserializeObject<T>(data.EscapeJson());
        }

        private async Task Post(string path, object content)
        {
            await CheckTokenState();

            var requestContent = new StringContent(JsonConvert.SerializeObject(content), new UTF8Encoding(), "application/json");
            var result = await _client.PostAsync(path, requestContent);
            result.EnsureSuccessStatusCode();
            //Add Error Handling
        }

        private async Task<T> Post<T>(string path, object content)
        {
            await CheckTokenState();

            var requestContent = new StringContent(JsonConvert.SerializeObject(content), new UTF8Encoding(), "application/json");
            var result = await _client.PostAsync(path, requestContent);
            result.EnsureSuccessStatusCode();
            var data = await result.Content.ReadAsStringAsync();
            //Add Error Handling
            return JsonConvert.DeserializeObject<T>(data.EscapeJson());
        }

        private async Task Put(string path, object content)
        {
            await CheckTokenState();

            var requestContent = new StringContent(JsonConvert.SerializeObject(content), new UTF8Encoding(), "application/json");
            var result = await _client.PutAsync(path, requestContent);
            result.EnsureSuccessStatusCode();
            //Add Error Handling
        }

        private async Task Delete(string path)
        {
            await CheckTokenState();

            var result = await _client.DeleteAsync(path);
            result.EnsureSuccessStatusCode();
            //Add Error Handling
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        private void UpdateClientAccessToken() {
            if (!string.IsNullOrWhiteSpace(_settings.OAuthToken))
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", _settings.OAuthToken);
        }

        private async Task CheckTokenState()
        {
            if (!string.IsNullOrWhiteSpace(_settings.OAuthRefreshToken) &&
                !string.IsNullOrWhiteSpace(_settings.OAuthToken) &&
                DateTime.Now < _settings.OAuthTokenExpiration)
                await RefreshToken();
        }
    }
}
