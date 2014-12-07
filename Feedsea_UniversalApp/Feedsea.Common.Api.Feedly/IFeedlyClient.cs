using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly
{
    public interface IFeedlyClient
    {
        string GetLoginUrl(string clientId, string redirectUri, string state);
        Task<AuthTokenResponse> RequestAccessToken(AuthTokenRequest request);
        Task<AuthTokenResponse> RefreshToken(RefreshTokenRequest request);
        Task<Profile> GetProfile();
        Task<FeedCategory[]> GetCategories();
        Task Subscribe(string url, string title, FeedCategory[] categories = null);
        Task<Subscription[]> GetSubscriptions();
        Task<Topic[]> GetTopics();
        Task AddTopic(Topic topic);
        Task<SearchResponse> SearchFeeds(string query);
        Task<SearchResponse> SearchFeeds(string query, int numberOfResults);
        Task<FeedStream> GetStream(string id, string continuation = null, int? count = null, long? newerThan = null, Ranked? ranked = null, bool? unreadOnly = null);
        Task<FeedStream> GetMixes(string id, string continuation, int? count = null, int? hours = null, long? newerThan = null, bool? backFill = null, bool? unreadOnly = null);
        Task<CountsResponse> GetCounts(bool? autoRefresh = null, long? newerThan = null);
        Task MarkRead(IMarkerInput input);
        Task KeepUnread(MarkerInputEntries input);
        Task<ReadOperations> GetLastestReadOperations();
        Task DeleteCategory(string id);
        Task DeleteSubscription(string id);
        Task<Entry> GetEntryContent(string entryId);
        Task SaveEntry(TagEntryInput entry);
        Task RemoveFromSaved(TagEntryInput entry);
    }
}
