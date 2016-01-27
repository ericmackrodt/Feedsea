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
        public async Task TestMethod1()
        {
            var getIdsResult = new FeedStreamIDs() { Ids = new string[] { "id1", "id2", "id3", "id4", "id5" } };
            var contentIds = getIdsResult.Ids.Take(2).ToArray();
            var client = Mock.Of<IFeedlyClient>(o => 
                o.Streams.GetIDs("123", Ranked.Oldest, false) == Task.FromResult(getIdsResult) &&
                o.Entries.GetMultipleContent(contentIds) == Task.FromResult(contentIds.Select(id => new Entry() { Id = id }).ToArray()));
            var storage = Mock.Of<IProviderStorage>(o => o.LoadArticles(getIdsResult.Ids) == Task.FromResult(getIdsResult.Ids.Skip(2).Take(3).Select(id => new ArticleData() { UniqueID = id })));
            var sut = new FeedlyProvider(client, settings, storage);
            var value = await sut.DownloadArticles(new ArticleData(), new SubscriptionData() { UrlID = "{userId}" });

        }
    }
}
