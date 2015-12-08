using Feedsea.Common.Api.Feedly;
using Feedsea.Common.Providers.Data;
using Feedsea.Common.Providers.Feedly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Feedsea.Common.Providers.Feedly
{
    public static class FeedlyHelper
    {
        public static CategoryData ToCategory(this FeedCategory cat, UnreadCount count)
        {
            var category = new CategoryData();
            category.Name = cat.Label;
            category.UrlID = cat.Id;
            category.Own = false;
            if (count != null)
                category.UnreadNumber = count.Count;
            return category;
        }

        public static SubscriptionData ToSubscription(this SearchResultData searchResult)
        {
            var sub = new SubscriptionData();
            sub.Link = searchResult.Url;
            sub.Name = searchResult.Title;
            sub.UrlID = searchResult.Id;
            return sub;
        }

        public static SubscriptionData ToSubscription(this Subscription sub, UnreadCount count, bool loadCategories)
        {
            var subscription = new SubscriptionData();

            subscription.Link = sub.Website;
            subscription.Name = sub.Title;
            subscription.UrlID = sub.Id;

            if (count != null)
                subscription.UnreadNumber = count.Count;

            if (loadCategories)
            {
                if (!sub.Categories.Any())
                {
                    subscription.Categories = new CategoryData[] { new CategoryData()
                    {
                        UrlID = ApiConstants.GlobalCategory_Uncategorized,
                        Name = "Uncategorized",
                        Own = true
                    }};
                }
                else
                {
                    subscription.Categories = sub.Categories.Select(o => new CategoryData()
                    {
                        Name = o.Label,
                        UrlID = o.Id,
                        Own = false
                    }).ToArray();
                }
            }

            return subscription;
        }

        public static SubscriptionData ToSubscription(this EntryOrigin origin)
        {
            if (origin == null)
                return null;

            return new SubscriptionData()
            {
                UrlID = origin.StreamId,
                Name = origin.Title,
                Link = origin.HtmlUrl
            };
        }

        public static ArticleData ToArticle(this Entry entry)
        {
            var article = new ArticleData();

            var hasSummary = entry.Summary != null;
            var hasContent = entry.Content != null;

            string summary = "";

            if (hasSummary)
                summary = entry.Summary.Content;
            else if (hasContent)
                summary = entry.Content.Content;

            var isSaved = entry.Tags != null ? entry.Tags.Any(o => Regex.IsMatch(o.Id, ApiConstants.GlobalTag_Regex)) : false;

            article.Author = entry.Author;
            article.Date = entry.Published.FromUnixTime();
            article.Summary = summary;
            article.Content = hasContent ? entry.Content.Content : null;
            article.Title = entry.Title;
            article.UniqueID = entry.Id;

            if (entry.Canonical != null && entry.Canonical.Any())
            {
                article.URL = entry.Canonical.First().Href;
            }
            else if (entry.Alternate != null && entry.Alternate.Any())
            {
                article.URL = entry.Alternate.First().Href;
            }

            article.IsRead = !entry.Unread;
            article.IsFavorite = isSaved;

            if (entry.Origin != null)
                article.Source = new SubscriptionData()
                {
                    Name = entry.Origin.Title,
                    UrlID = entry.Origin.StreamId,
                    Link = entry.Origin.HtmlUrl
                };

            if (entry.Visual != null)
                article.MainImageUrl = entry.Visual.Url.ToLower() == "none" ? null : entry.Visual.Url;

            if (entry.Enclosure != null)
                article.Enclosure = entry.Enclosure.Select(o => new EnclosureData() { Href = o.Href, Type = o.Type }).ToArray();

            article.Source = entry.Origin.ToSubscription();

            return article;
        }

        public static IEnumerable<ArticleData> ToArticleCollection(this IEnumerable<Entry> entries)
        {
            var col = new List<ArticleData>();
            foreach (var item in entries)
            {
                var article = item.ToArticle();
                col.Add(article);
            }
            return col;
        }

        public static SearchResultData ToSearchResult(this SearchResult result)
        {
            var searchRes = new SearchResultData();

            searchRes.Id = result.FeedId;
            searchRes.Title = result.Title;
            searchRes.Url = result.Website;
            searchRes.Subscribers = (result.Subscribers > 999 ? Math.Round(result.Subscribers / 100m, 0).ToString() + "k" : result.Subscribers.ToString()) + " ";
            searchRes.Description = result.Description;
            searchRes.Tags = result.Tags != null ? string.Join(", ", result.Tags.Select(o => string.Concat("#", o))) : "";

            return searchRes;
        }

        public static DateTime FromUnixTime(this long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTime).ToLocalTime();
        }

        public static long ToUnixTime(this DateTime dateTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (dateTime - epoch).Milliseconds;
        }

        public static string CleanTextAndDecodeHtml(this string text)
        {
            return Uri.UnescapeDataString(Regex.Replace(text, @"<[^>]*>", string.Empty, RegexOptions.Singleline).Trim());
        }

        public static string FormatCategoryLabel(this string categoryName)
        {
            var s = "";

            foreach (var c in categoryName)
            {
                s += new char[] { '<', '>', '?', '&', '/', '\\' }.Contains(c) ? '_' : c;
            }

            return s;
        }
    }
}
