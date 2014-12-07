using feedsea.Common.Data;
using feedsea.Common.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using feedsea.Common.Converters;

namespace feedsea.Common.Helpers
{
    public static class FeedHelper
    {
        public static IEnumerable<Article> GetNews(this Source feed)
        {
            //TODO: review
            return feed.Items
                .Select(o => BuildArticleData(o));
        }

        public static Article BuildArticleData(this Item item)
        {
            //var hasContent = !string.IsNullOrWhiteSpace(item.Content);
            var hasSummary = !string.IsNullOrWhiteSpace(item.Summary);

            var summary = hasSummary ? item.Summary : "";

            var article = new Article()
            {
                Author = item.Author,
                Date = new DateConverter(item.PubDate, System.Threading.Thread.CurrentThread.CurrentUICulture).Convert(),
                Summary = summary,
                Title = item.Title,
                UniqueID = item.UniqueID,
                URL = item.Link
            };

            if (!string.IsNullOrWhiteSpace(item.Image))
                article.ArticleImages.Add(new ArticleImage()
                {
                    ImageUrl = item.Image,
                    ImageScope = "Thumbnail"//img.Scope.ToString()
                });

            return article;
        }

        public static Article BuildArticleData(this Item item, NewsSource source)
        {
            var art = BuildArticleData(item);
            art.NewsSourceID = source.UrlID;
            return art;
        }

        public static NewsSource GetNewsSource(this Source info, string sourceUrl)
        {
            return new feedsea.Common.Data.NewsSource()
            {
                Name = info.Title,
                Image = info.ImageLogo ?? "",
                Link = info.Link,
                LastFetch = DateTime.Now,
                UrlID = info.URL,
            };
        }

        public static Source GetSource(this NewsSource src)
        {
            return new Source()
            {
                Title = src.Name,
                Link = src.UrlID,
                Id = src.UrlID
            };
        }

        public static IEnumerable<Source> GetSources(this IEnumerable<NewsSource> srcs)
        {
            return srcs.Select(o => o.GetSource());
        }
    }
}
