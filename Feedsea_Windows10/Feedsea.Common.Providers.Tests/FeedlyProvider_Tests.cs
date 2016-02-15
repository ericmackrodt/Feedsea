using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Feedsea.Common.Api.Feedly;
using Moq;
using Feedsea.Common.Api.Feedly.APIs;
using Feedsea.Common.Providers.Data;
using System.Threading.Tasks;
using Feedsea.Common.Providers.Feedly;
using System.Collections.Generic;
using System.Linq;

namespace Feedsea.Common.Providers.Tests
{
    [TestClass]
    public class FeedlyProvider_Tests
    {
        IFeedlySettings settings;

        private ArticleData MockArticle(string id, string name)
        {
            return Mock.Of<ArticleData>(o =>
                o.Author == "Test Author" &&
                o.Content == "Test Content" &&
                o.Date == new DateTime(2010, 5, 10) &&
                o.Summary == "Test Summary" &&
                o.Title == name &&
                o.UniqueID == id &&
                o.URL == "http://test.com");
        }

        private IFeedlySettings MockSettings()
        {
            var factory = new Mock<IFeedlySettings>();
            factory.Setup(o => o.ArticlesFromOldestToNewest).Returns(true);
            factory.Setup(o => o.ShowRead).Returns(true);
            factory.Setup(o => o.UserID).Returns("123");
            return factory.Object;
        }

        [TestInitialize]
        public void SetUp()
        {
            settings = MockSettings();
        }

        [TestMethod]
        public async Task DownloadArticles_ShouldOnlyLoadArticlesThatWerentDownloadedYet()
        {
            var getIdsResult = new FeedStreamIDs() { Ids = new string[] { "id1", "id2", "id3", "id4", "id5" } };
            var idsToGetContent = getIdsResult.Ids.Take(2).ToArray();
            var resultArray = idsToGetContent.Select(id => new Entry() { Id = id }).ToArray();
            var client = Mock.Of<IFeedlyClient>(o => 
                o.Streams.GetIDs("123", Ranked.Oldest, false) == Task.FromResult(getIdsResult) &&
                o.Entries.GetMultipleContent(idsToGetContent) == Task.FromResult(resultArray));

            var storage = new Mock<IProviderStorage>();
            storage.Setup(o => o.SaveArticles(It.IsAny<IEnumerable<ArticleData>>())).Returns(Task.FromResult(false));
            storage.Setup(o => o.LoadArticles(getIdsResult.Ids) == Task.FromResult(getIdsResult.Ids.Skip(2).Take(3).Select(id => new ArticleData() { UniqueID = id })));
            var sut = new FeedlyProvider(client, settings, storage.Object);
            var value = await sut.DownloadArticles(new SubscriptionData() { UrlID = "{userId}" });

            storage.Verify(o => o.SaveArticles(It.IsAny<IEnumerable<ArticleData>>()));
            CollectionAssert.AreEqual(resultArray.ToArticleCollection().ToList(), value.ToList(), "Result should be the same");
        }

        [TestMethod]
        public async Task DownloadArticles_ShouldOnlyDownloadNonExistingArticlesWhenPassingAList()
        {
            var getIdsResult = new FeedStreamIDs() { Ids = new string[] { "id1", "id2", "id3", "id4", "id5" } };
            var idsToGetContent = getIdsResult.Ids.Take(2).ToArray();
            var contentsFromIds = idsToGetContent.Select(id => new Entry() { Id = id }).ToArray();
            var articlesAlreadyDownloaded = getIdsResult.Ids.Skip(2).Take(3).Select(id => new ArticleData() { UniqueID = id });

            var client = Mock.Of<IFeedlyClient>(o =>
                o.Streams.GetIDs("123", Ranked.Oldest, false) == Task.FromResult(getIdsResult) &&
                o.Entries.GetMultipleContent(idsToGetContent) == Task.FromResult(contentsFromIds));
            var storage = new Mock<IProviderStorage>();
            storage.Setup(o => o.SaveArticles(It.IsAny<IEnumerable<ArticleData>>())).Returns(Task.FromResult(false));
            var sut = new FeedlyProvider(client, settings, storage.Object);
            var value = await sut.DownloadArticles(articlesAlreadyDownloaded, new SubscriptionData() { UrlID = "{userId}" });

            storage.Verify(o => o.SaveArticles(It.IsAny<IEnumerable<ArticleData>>()));
            CollectionAssert.AreEqual(getIdsResult.Ids, value.ArticlesOrder);
            CollectionAssert.AreEqual(contentsFromIds.ToArticleCollection().ToList(), value.DownloadedArticles.ToList(), "Result should be the same");
        }

        [TestMethod]
        public async Task DownloadArticles_ShouldDownloadAllArticlesIfNoneStored()
        {
            var getIdsResult = new FeedStreamIDs() { Ids = new string[] { "id1", "id2", "id3", "id4", "id5" } };
            var idsToGetContent = getIdsResult.Ids.ToArray();
            var contentsFromIds = idsToGetContent.Select(id => new Entry() { Id = id }).ToArray();

            var client = Mock.Of<IFeedlyClient>(o =>
                o.Streams.GetIDs("123", Ranked.Oldest, false) == Task.FromResult(getIdsResult) &&
                o.Entries.GetMultipleContent(idsToGetContent) == Task.FromResult(contentsFromIds));

            var storage = new Mock<IProviderStorage>();
            storage.Setup(o => o.SaveArticles(It.IsAny<IEnumerable<ArticleData>>()))
                .Returns(Task.FromResult(false));
            storage.Setup(o => o.LoadArticles(getIdsResult.Ids))
                .Returns(Task.FromResult((IEnumerable<ArticleData>)new List<ArticleData>()));

            var sut = new FeedlyProvider(client, settings, storage.Object);
            var value = await sut.DownloadArticles(new SubscriptionData() { UrlID = "{userId}" });

            storage.Verify(o => o.SaveArticles(It.IsAny<IEnumerable<ArticleData>>()));
            CollectionAssert.AreEqual(contentsFromIds.ToArticleCollection().ToList(), value.ToList(), "Result should be the same");
        }

        [TestMethod]
        public async Task DownloadArticles_ShouldDownloadAllArticlesIfNonePassed()
        {
            var getIdsResult = new FeedStreamIDs() { Ids = new string[] { "id1", "id2", "id3", "id4", "id5" } };
            var idsToGetContent = getIdsResult.Ids.ToArray();
            var contentsFromIds = idsToGetContent.Select(id => new Entry() { Id = id }).ToArray();
            var articlesAlreadyDownloaded = new List<ArticleData>();

            var client = Mock.Of<IFeedlyClient>(o =>
                o.Streams.GetIDs("123", Ranked.Oldest, false) == Task.FromResult(getIdsResult) &&
                o.Entries.GetMultipleContent(idsToGetContent) == Task.FromResult(contentsFromIds));

            var storage = new Mock<IProviderStorage>();
            storage.Setup(o => o.SaveArticles(It.IsAny<IEnumerable<ArticleData>>()))
                .Returns(Task.FromResult(false));

            var sut = new FeedlyProvider(client, settings, storage.Object);
            var value = await sut.DownloadArticles(articlesAlreadyDownloaded, new SubscriptionData() { UrlID = "{userId}" });

            storage.Verify(o => o.SaveArticles(It.IsAny<IEnumerable<ArticleData>>()));
            CollectionAssert.AreEqual(getIdsResult.Ids, value.ArticlesOrder);
            CollectionAssert.AreEqual(contentsFromIds.ToArticleCollection().ToList(), value.DownloadedArticles.ToList(), "Result should be the same");
        }
    }
}
