using Feedsea.Common.Api.Feedly.APIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly
{
    public interface IFeedlyClient
    {
        IAuthenticationAPI Authentication { get; }
        ICategoriesAPI Categories { get; }
        IEntriesAPI Entries { get; }
        IFeedsAPI Feeds { get; }
        IMarkersAPI Markers { get; }
        IMixesAPI Mixes { get; }
        IPreferencesAPI Preferences { get; }
        IProfileAPI Profile { get; }
        ISearchAPI Search { get; }
        IShortUrlAPI ShortUrl { get; }
        IStreamsAPI Streams { get; }
        ISubscriptionsAPI Subscriptions { get; }
        ITagsAPI Tags { get; }
    }
}
