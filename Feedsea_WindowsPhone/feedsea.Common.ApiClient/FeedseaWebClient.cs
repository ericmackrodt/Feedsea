using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.ApiClient
{
    public class QueryStringParam
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public QueryStringParam(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }

    public class FeedseaWebClient : HttpClient
    {
        private string baseUrl;

        public FeedseaWebClient()
            : base()
        {
            this.baseUrl = "http://www.feedsea.net/";
        }

        public FeedseaWebClient(string baseUrl)
            : base()
        {
            this.baseUrl = baseUrl;
        }

        public async Task<Response<Source>> GetSourceInfoTaskAsync(string url)
        {
            return await GetRequest<Source>(BuildMethodUrl("api/feed/info", new QueryStringParam("url", url)));  //string.Concat("http://feedsea.azurewebsites.net/api/feed/info?url=", HttpUtility.UrlEncode(url)));            
        }

        public async Task<Item[]> GetSourceItemsTaskAsync(string url)
        {
            return null;
        }

        public async Task<Response<Item[]>> GetMultipleSourceItemsTaskAsync(string[] sources)
        {
            return await PostRequest<Item[]>(BuildMethodUrl("api/feed/MultipleSourceItems", null), sources.ProtoBufSerialize()); // "http://feedsea.azurewebsites.net/api/feed/MultipleSourceItems", sources.ProtoBufSerialize());
        }

        public async Task<Response<string>> GetFaviconTaskAsync(string url)
        {
            return await GetRequest<string>(BuildMethodUrl("api/favicon", new QueryStringParam("url", url)));// string.Concat("http://feedsea.azurewebsites.net/api/favicon?url=", HttpUtility.UrlEncode(url)));
        }

        public async Task<Response<LiveTileData[]>> GetLiveTileData(LiveTileSourceData[] sources)
        {
            return await PostRequest<LiveTileData[]>(BuildMethodUrl("api/LiveTile", null), sources.ProtoBufSerialize()); // "http://feedsea.azurewebsites.net/api/LiveTile", sources);
        }

        public async Task<string> GetArticle(string url)
        {
            return await GetStringRequest(BuildMethodUrl("api/Mobilize", new QueryStringParam("url", url)));// string.Concat("http://www.readability.com/m?url=", HttpUtility.UrlEncode(url)));
        }

        private async Task<string> GetStringRequest(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var result = await SendAsync(request);
            return await result.Content.ReadAsStringAsync();
        }

        private async Task<Response<T>> GetRequest<T>(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));
            var result = await SendAsync(request);
            var data = ProtoBuf.Serializer.Deserialize<Response<T>>(await result.Content.ReadAsStreamAsync());
            return data;

            //var serializer = MessagePackSerializer.Create<Response<T>>();
            //return serializer.Unpack(await result.Content.ReadAsStreamAsync());
        }

        private string BuildArrayQueryString(IEnumerable<string> items, string parameter)
        {
            var parameters = new List<string>();
            var num = 0;
            foreach (var item in items)
                parameters.Add(string.Format("{0}[{1}]={2}", parameter, num++, WebUtility.UrlEncode(item)));

            return string.Join("&", parameters);
        }

        private async Task<Response<TResponse>> PostRequest<TResponse>(string url, Stream content)
        {
            return await PostRequest<TResponse>(url, new StreamContent(content));
        }

        private async Task<Response<TResponse>> PostRequest<TResponse>(string url, byte[] content)
        {
            return await PostRequest<TResponse>(url, new ByteArrayContent(content));
        }

        private async Task<Response<TResponse>> PostRequest<TResponse>(string url, HttpContent content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));
            //request.Headers.Add("Content-Type", "application/x-protobuf");
            request.Content = content;
            request.Content.Headers.Add("Content-Type", "application/x-protobuf");
            var result = await SendAsync(request);
            var response = Serializer.Deserialize<Response<TResponse>>(await result.Content.ReadAsStreamAsync());
            return response;
        }

        //private async Task<Response<TResponse>> PostRequest<TRequest, TResponse>(string url, obj content)
        //{
        //    return await PostRequest<TResponse>(url, content.ProtoBufSerialize());

        //    //var request = new HttpRequestMessage(HttpMethod.Post, url);
        //    //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));
        //    ////request.Headers.Add("Content-Type", "application/x-protobuf");
        //    //request.Content = new ByteArrayContent(content.ProtoBufSerialize());
        //    //request.Content.Headers.Add("Content-Type", "application/x-protobuf");
        //    //var result = await SendAsync(request);
        //    //var response = Serializer.Deserialize<Response<TResponse>>(await result.Content.ReadAsStreamAsync());
        //    //return response;
            
        //    //var reqSerializer = MessagePackSerializer.Create<TRequest>();
        //    //var packed = reqSerializer.PackSingleObject(content);
        //    //request.Content = new ByteArrayContent(packed);
        //    //var result = await SendAsync(request);
        //    //var respSerializer = MessagePackSerializer.Create<Response<TResponse>>();
        //    //return respSerializer.Unpack(await result.Content.ReadAsStreamAsync());
        //}

        private string BuildMethodUrl(string path, params QueryStringParam[] queryStringData)
        {
            var queryString = "";

            if (queryStringData != null && queryStringData.Count() > 0)
            {
                var q = queryStringData.Select(o => string.Concat(o.Key, "=", WebUtility.UrlDecode(o.Value)));
                queryString = string.Concat("?", string.Join("&", q));
            }

            return string.Concat(baseUrl, path, queryString);
        }
    }
}
