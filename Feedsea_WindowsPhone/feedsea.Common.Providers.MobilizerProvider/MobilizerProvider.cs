using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Providers.MobilizerProvider
{
    public class MobilizerProvider : IMobilizerProvider
    {
        public async Task<string> GetMobilized(Mobilizer mobilizer, string articleUrl)
        {
            switch (mobilizer)
            {
                case Mobilizer.Instapaper:
                    return await GetInstapaper(articleUrl);
                case Mobilizer.Readability:
                default:
                    return await GetReadability(articleUrl);
            }
        }

        private async Task<string> GetReadability(string articleUrl)
        {
            var url = string.Format("http://www.readability.com/m?url={0}", articleUrl);
            var data = await DownloadArticle(url);
            return GetContent(data, "section", "rdb-article-content");
        }

        private async Task<string> GetInstapaper(string articleUrl)
        {
            var url = string.Format("http://mobilizer.instapaper.com/m?u={0}", articleUrl);
            var data = await DownloadArticle(url);
            return GetContent(data, "div", "story");
        }

        private string GetContent(string data, string nodeType, string selector)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(data);
            var content = doc.DocumentNode.Descendants(nodeType).FirstOrDefault(o => o.Attributes.Any(x => x.Name == "id" && x.Value == selector));
            if (content != null)
                return content
                    .InnerHtml;

            return null;
        }

        private async Task<string> DownloadArticle(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync(url);
                    result.EnsureSuccessStatusCode();
                    return await result.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException is WebException && (ex.InnerException as WebException).Status == WebExceptionStatus.RequestCanceled)
                {
                    return null;
                }
                else
                    throw new ProviderException(ExceptionReason.NoInternetConnection);
            }
        }
    }
}
