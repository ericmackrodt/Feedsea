using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using HtmlAgilityPack;
using System.Windows;

namespace feedsea.Common.Helpers
{
    public static class GeneralHelpers
    {
        public static string TreatEmbededVideos(this string article, string youtubeClient)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(article);

            var content = doc.DocumentNode;

            switch (youtubeClient.ToLower())
            {
                case "default":
                    content = doc.ChangeYoutubeToHtml5();
                    break;
                case "metrotube":
                    content = doc.ChangeYoutubeToMetroTube();
                    break;
                case "mytube":
                    content = doc.ChangeYoutubeToMyTube();
                    break;
                case "toib":
                    content = doc.ChangeYoutubeToToib();
                    break;
                case "other":
                    content = doc.ChangeYoutubeToOtherClient();
                    break;
                default:
                    content = doc.ChangeYoutubeToHtml5();
                    break;
            }

            return content.InnerHtml;
        }

        private static HtmlNode ChangeYoutubeToHtml5(this HtmlDocument doc)
        {
            var iframes = doc.DocumentNode.Descendants("iframe").Where(o => o.Attributes.Any(x => x.Name == "src" && x.Value.Contains("youtube") && !x.Value.Contains("html5=1")));

            if (iframes != null && iframes.Count() > 0)
            {
                foreach (var iframe in iframes.ToList())
                {
                    var src = new Uri(iframe.GetAttributeValue("src", ""));
                    var url = string.Concat(src.ToString(), src.Query.Any() ? "&" : "?", "html5=1").AddHttp(false);
                    iframe.SetAttributeValue("src", url);
                }
            }

            return doc.DocumentNode;
        }

        private static HtmlNode ChangeYoutubeToMetroTube(this HtmlDocument doc)
        {
            return doc.ChangeYoutubeToClient("metrotube:VideoPage?VideoID={0}", "Assets\\MetroTubePlay.png", false);
        }

        private static HtmlNode ChangeYoutubeToClient(this HtmlDocument doc, string clientUri, string clientImage, bool useFullUrl)
        {
            var iframes = doc.DocumentNode.Descendants("iframe").Where(o => o.Attributes.Any(x => x.Name == "src" && x.Value.Contains("youtube")));

            if (iframes != null && iframes.Count() > 0)
            {
                string mask = null;
                foreach (var iframe in iframes.ToList())
                {
                    var url = iframe.GetAttributeValue("src", "");

                    var videoId = GetYoutubeId(url);

                    HtmlNode div = doc.CreateElement("div");
                    div.Attributes.Add("class", "metrotube-video");
                    //div.Attributes.Add("data-videoid", videoId);
                    div.Attributes.Add("data-videourl", string.Format(clientUri, useFullUrl ? url.Replace("/embed/", "/watch?v=") : videoId));
                    div.Attributes.Add("style", string.Format(@"background: url(http://img.youtube.com/vi/{0}/0.jpg) no-repeat center center", videoId));

                    var metrotubeMask = doc.CreateElement("img");
                    metrotubeMask.Attributes.Add("class", "mask");
                    if (mask == null)
                    {
                        var res = Application.GetResourceStream(new Uri(clientImage, UriKind.Relative));
                        var bytes = res.Stream.ReadToEnd();
                        mask = Convert.ToBase64String(bytes);
                    }

                    metrotubeMask.Attributes.Add("src", string.Format("data:image/png;base64,{0}", mask));
                    div.AppendChild(metrotubeMask);

                    iframe.ParentNode.ReplaceChild(div, iframe);
                }
            }

            return doc.DocumentNode;
        }

        private static string GetOldYoutubeId(string url)
        {
            var videoId = url.Split(new string[] { "v=", "embed/" }, StringSplitOptions.RemoveEmptyEntries)[1];
            var ampersandPosition = videoId.IndexOf('&');
            var questionMarkPosition = videoId.IndexOf('?');
            var position = ampersandPosition < questionMarkPosition ? ampersandPosition : questionMarkPosition;
            if (position != -1)
                videoId = videoId.Substring(0, position);
            return videoId;
        }

        private static string GetYoutubeId(string url)
        {
            var regex1 = @"http(s)?://(www\.)?youtube.com/embed/(?<id>[a-zA-Z0-9_-]{11})(\?.+)?";
            var regex2 = @"http(s)?://(www\.)?youtube.com/(.+)?v=(?<id>[a-zA-Z0-9_-]{11})(\&.+)?";

            var match = Regex.Match(url, regex1);

            if (!match.Success)
                match = Regex.Match(url, regex2);

            if (!match.Success)
                return GetOldYoutubeId(url);

            var id = match.Groups["id"];

            if (id == null) return GetOldYoutubeId(url);

            return id.Value;
        }

        private static HtmlNode ChangeYoutubeToMyTube(this HtmlDocument doc)
        {
            return doc.ChangeYoutubeToClient("mytube:link={0}", "Assets\\MyTubePlay.png", true);
        }

        private static HtmlNode ChangeYoutubeToToib(this HtmlDocument doc)
        {
            return doc.ChangeYoutubeToClient("toib:PlayVideo?VideoID={0}", "Assets\\ToibPlay.png", false);
        }

        private static HtmlNode ChangeYoutubeToOtherClient(this HtmlDocument doc)
        {
            return doc.ChangeYoutubeToClient("vnd.youtube:{0}?vndapp=youtube", "Assets\\YouTubePlay.png", false);
        }

        public static string AddHttp(this string url, bool addEndSlash)
        {
            var end = url;
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                end = string.Concat("http:", end.StartsWith("//") ? "" : "//", end);
            if (!url.EndsWith("/") && addEndSlash)
                end = string.Concat(end, "/");
            return end;
        }

        public static byte[] ReadToEnd(this System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        public static string ProcessDocumentHyperlinks(this string article)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(article);

            var hyperLinks = doc.DocumentNode.Descendants("a").Where(o => o.Attributes.Any(x => x.Name == "href"));

            foreach (var link in hyperLinks.ToList())
            {
                var url = link.GetAttributeValue("href", "");
                link.Attributes.Remove("href");
                link.Attributes.Add("onclick", "linkClick(this)");
                link.Attributes.Add("data-link", url);
            }

            return doc.DocumentNode.InnerHtml;
        }
    }
}
