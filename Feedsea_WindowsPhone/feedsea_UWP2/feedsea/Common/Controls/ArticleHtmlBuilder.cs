using feedsea.Common.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using feedsea.Common.Helpers;
using System.Globalization;
using feedsea.Settings;
using Windows.UI.Xaml.Media;
using feedsea.Common.Providers.Data;
using feedsea.Common.Providers.MobilizerProvider;

namespace feedsea.Common.Controls
{

   public class ArticleHtmlBuilder
      : IArticleHtmlBuilder
   {

      public async Task<string> BuildHtml(ArticleData article, YouTubeClients ytClient)
      {
         return await BuildArticleContent(article, ytClient);
      }

      private string GetColor(string resourceName)
      {
         var color = (App.Current.Resources[resourceName] as Windows.UI.Xaml.Media.SolidColorBrush).Color;
         return color.ToString().Substring(3, 6);
      }

      private async Task<string> BuildArticleContent(ArticleData article, YouTubeClients ytClient)
      {
         var articleHtml = Resources.Files.ArticleHtml;
         var dateFormat = CultureInfo.CurrentUICulture.DateTimeFormat;
         var backgroundColor = GetColor("DefaultPageBackgroundColor");
         var textColor = GetColor("DefaultTextColor");
         var linkColor = GetColor("ThemeMainColor");
         var authorColor = GetColor("SecondaryTextColor");
         return await Task.Run(async () =>
            {
               var content = article.Content;
               return articleHtml.Replace("{{fontColor}}", textColor).Replace("{{backgroundColor}}", backgroundColor).Replace("{{authorColor}}", authorColor).Replace("{{linkColor}}", linkColor).Replace("{{linkVisitedColor}}", linkColor).Replace("{{linkHoverColor}}", linkColor).Replace("{{title}}", article.Title).Replace("{{content}}", await (content ?? article.Summary).ProcessDocumentHyperlinks().TreatEmbededVideos(ytClient.ToString())).Replace("{{author}}", article.Author).Replace("{{pubdate}}", article.Date.ToString("g", dateFormat));
            });
      }

   }

}