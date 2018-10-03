using ProxyServer.Models.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyServer.Models
{
    class CacheItem
    {
        public string RequestLine { get; set; }
        public DateTime CacheMoment { get; set; }
        public byte[] Content { get; set; }
        public int MaxAge;

        private List<HttpHeader> ResponseHeaders;

        /// <summary>
        /// Constructor for 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="httpResponse"></param>
        public CacheItem(HttpRequest request, HttpResponse httpResponse)
        {
            RequestLine = request.RequestLine;
            Content = httpResponse.Body;
            CacheMoment = DateTime.UtcNow;
            ResponseHeaders = httpResponse.Headers;
            MaxAge = 0;
            if (request.HasHeader("Cache-Control") && request.GetHeader("Cache-Control").Value.Contains("max-age="))
            {
                int.TryParse(request.GetHeader("Cache-Control").Value.Split('=')[1], out MaxAge);
            }
        }

        /// <summary>
        /// Check if cached item still is valid
        /// </summary>
        /// <param name="maxTimeToBeValid">Maximum time passed since cache</param>
        /// <returns></returns>
        public bool Valid(int maxTimeToBeValid)
        {
            if (MaxAge > 0)
            {
                return ((DateTime.UtcNow - CacheMoment).TotalSeconds <= maxTimeToBeValid) && ((DateTime.UtcNow - CacheMoment).TotalSeconds <= MaxAge);
            }
            return (DateTime.UtcNow - CacheMoment).TotalSeconds <= maxTimeToBeValid;
        }

        /// <summary>
        /// Create new response for cached object
        /// </summary>
        /// <returns>HTTP response object</returns>
        public HttpResponse ToResponse()
        {
            return HttpResponse.GetCacheResponse(ResponseHeaders, Content);
        }
    }
}
