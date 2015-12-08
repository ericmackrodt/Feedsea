using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Api.Feedly
{
    [BaseUrl("http://cloud.feedly.com/v3/")]
    public class FeedlyWebClient : WebClientBase
    {
        public FeedlyWebClient()
        {

        }

        public FeedlyWebClient(string accessToken)
            : base("OAuth", accessToken)
        {
        }

        public override string GetLoginUrl(string clientId, string redirectUri, string state)
        {
            return BaseUrlAttribute.GetAttributeValue(typeof(FeedlyWebClient))
                .BuildUrl("auth/auth",
                new QueryStringParam("scope", "https://cloud.feedly.com/subscriptions"),
                new QueryStringParam("response_type", "code"),
                new QueryStringParam("client_id", clientId),
                new QueryStringParam("redirect_uri", redirectUri),
                new QueryStringParam("state", state));
        }

        public async Task<AuthTokenResponse> RequestAccessToken(AuthTokenRequest request)
        {
            request.GrantType = "authorization_code";
            return await PostRequest<AuthTokenResponse>(BuildMethodUrl("auth/token"), request);
        }

        public async Task<AuthTokenResponse> RefreshToken(RefreshTokenRequest request)
        {
            request.GrantType = "refresh_token";
            return await PostRequest<AuthTokenResponse>(BuildMethodUrl("auth/token"), request);
        }

        public async Task<Profile> GetProfile()
        {
            return await GetRequest<Profile>(BuildMethodUrl("profile"));
        }

        public async Task<FeedCategory[]> GetCategories()
        {
            return await GetRequest<FeedCategory[]>(BuildMethodUrl("categories"));
        }

        public async Task Subscribe(string url, string title, FeedCategory[] categories = null)
        {
            var content = new Subscription() 
            {
                Id = url,
                Title = title,
                Categories = categories
            };
            await PostRequest(BuildMethodUrl("subscriptions"), content);
        }

        public async Task<Subscription[]> GetSubscriptions()
        {
            return await GetRequest<Subscription[]>(BuildMethodUrl("subscriptions"));
        }

        public async Task<Topic[]> GetTopics()
        {
            return await GetRequest<Topic[]>(BuildMethodUrl("topics"));
        }

        public async Task AddTopic(Topic topic)
        {
            await PostRequest(BuildMethodUrl("topics"), topic);
        }

        public async Task<SearchResponse> SearchFeeds(string query)
        {
            return await GetRequest<SearchResponse>(BuildMethodUrl("search/feeds", new QueryStringParam("q", Uri.EscapeDataString(query))));
        }

        public async Task<SearchResponse> SearchFeeds(string query, int numberOfResults)
        {
            return await GetRequest<SearchResponse>(BuildMethodUrl("search/feeds", new QueryStringParam("q", Uri.EscapeDataString(query)), new QueryStringParam("n", numberOfResults)));
        }

        public async Task<FeedStream> GetStream(string id, string continuation = null, int? count = null, long? newerThan = null, Ranked? ranked = null, bool? unreadOnly = null)
        {
            var data = new List<QueryStringParam>();
            data.Add(new QueryStringParam("streamId", id));

            if(!string.IsNullOrWhiteSpace(continuation))
                data.Add(new QueryStringParam("continuation", continuation));

            if (count != null)            
                data.Add(new QueryStringParam("count", count));
            
            if (newerThan != null)
                data.Add(new QueryStringParam("newerThan", newerThan));
            
            if (ranked != null)
                data.Add(new QueryStringParam("ranked", ranked.ToString().ToLower()));
                
            if (unreadOnly != null)
                data.Add(new QueryStringParam("unreadOnly", unreadOnly));
            
            return await GetRequest<FeedStream>(BuildMethodUrl("streams/contents", data.ToArray()));
        }

        public async Task<FeedStream> GetMixes(string id, string continuation, int? count = null, int? hours = null, long? newerThan = null, bool? backFill = null, bool? unreadOnly = null)
        {
            var data = new List<QueryStringParam>();
            data.Add(new QueryStringParam("streamId", id));

            if (!string.IsNullOrWhiteSpace(continuation))
                data.Add(new QueryStringParam("continuation", continuation));

            if (count != null)
                data.Add(new QueryStringParam("count", count));

            if (newerThan != null)
                data.Add(new QueryStringParam("newerThan", newerThan));

            if (hours != null)
                data.Add(new QueryStringParam("hours", hours));

            if (backFill != null)
                data.Add(new QueryStringParam("backfill", backFill));

            if (unreadOnly != null)
                data.Add(new QueryStringParam("unreadOnly", unreadOnly));

            return await GetRequest<FeedStream>(BuildMethodUrl("mixes/contents", data.ToArray()));
        }

        public async Task<CountsResponse> GetCounts(bool? autoRefresh = null, long? newerThan = null)
        {
            var data = new List<QueryStringParam>();

            if (autoRefresh != null)
                data.Add(new QueryStringParam("autoRefresh", autoRefresh));

            if (newerThan != null)
                data.Add(new QueryStringParam("newerThan", newerThan));

            return await GetRequest<CountsResponse>(BuildMethodUrl("markers/counts", data.ToArray()));
        }

        public async Task MarkRead(IMarkerInput input)
        {
            input.Action = "markAsRead";
            await PostRequest(BuildMethodUrl("markers"), input);
        }

        public async Task KeepUnread(MarkerInputEntries input)
        {
            input.Action = "keepUnread";
            await PostRequest(BuildMethodUrl("markers"), input);
        }

        public async Task<ReadOperations> GetLastestReadOperations()
        {
            return await GetRequest<ReadOperations>(BuildMethodUrl("markers/reads"));
        }

        public async Task DeleteCategory(string id)
        {
            await DeleteRequest(BuildMethodUrl(string.Concat("categories/", id)));
        }

        public async Task DeleteSubscription(string id)
        {
            await DeleteRequest(BuildMethodUrl(string.Concat("subscriptions/", id)));
        }

        public async Task<Entry> GetEntryContent(string entryId)
        {
            return await GetRequest<Entry>(BuildMethodUrl(string.Concat("entries/", entryId)));
        }

        public async Task SaveEntry(TagEntryInput entry)
        {
            await PutRequest(BuildMethodUrl(string.Concat("tags/", WebUtility.UrlEncode(ApiConstants.GlobalTag_Saved.Replace(ApiConstants.FormatKey_UserId, entry.UserId)))), entry);
        }

        public async Task RemoveFromSaved(TagEntryInput entry)
        {
            await DeleteRequest(BuildMethodUrl(string.Format("tags/{0}/{1}", WebUtility.UrlEncode(ApiConstants.GlobalTag_Saved.Replace(ApiConstants.FormatKey_UserId, entry.UserId)), WebUtility.UrlEncode(entry.EntryId))));
        }
    }
}
