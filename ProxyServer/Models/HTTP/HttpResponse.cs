using System;
using System.Collections.Generic;
using System.Text;

namespace ProxyServer.Models.HTTP
{
    public class HttpResponse : HttpMessage
    {
        public string StatusLine
        {
            get => base.FirstLine;
            set
            {
                base.FirstLine = value;
            }
        }

        public HttpResponse(string requestLine, List<HttpHeader> headers, byte[] body) : base(requestLine, headers, body)
        {
            if (HasHeader("Content-Type"))
            {
                string contentType = GetHeader("Content-Type").Value.ToLower();
                if (contentType.Contains("text"))
                {
                    Type = HttpContentType.text;
                }
                else if (contentType.Contains("multipart"))
                {
                    Type = HttpContentType.multipart;
                }
                else if (contentType.Contains("message"))
                {
                    Type = HttpContentType.message;
                }
                else if (contentType.Contains("image"))
                {
                    Type = HttpContentType.image;
                }
                else if (contentType.Contains("audio"))
                {
                    Type = HttpContentType.audio;
                }
                else if (contentType.Contains("video"))
                {
                    Type = HttpContentType.video;
                }
                else if (contentType.Contains("application"))
                {
                    Type = HttpContentType.application;
                }
                else
                {
                    Type = HttpContentType.unknown;
                }
            }
            else
            {
                Type = HttpContentType.unknown;
            }
        }

        public HttpContentType Type { get; set; }

        /// <summary>
        /// Parse response object from byte[]
        /// </summary>
        /// <param name="request">Response as byte[]</param>
        /// <returns>Response as object</returns>
        public static HttpResponse Parse(byte[] request)
        {
            try
            {
                var lines = ReadLines(request);
                return new HttpResponse(lines[0], ReadHeaders(lines), ReadBody(request));
            }
            catch
            {
                throw new FormatException();
            }
        }

        /// <summary>
        /// Get current timestamp for response
        /// </summary>
        /// <returns>HTTP Timestamp</returns>
        private static string GetDateToday()
        {
            var sb = new StringBuilder();
            sb.Append(DateTime.UtcNow.ToString("dddd"));
            sb.Append(", ");
            sb.Append(DateTime.UtcNow.ToShortDateString());
            sb.Append(' ');
            sb.Append(DateTime.UtcNow.ToShortTimeString());
            sb.Append(" UTC");
            return sb.ToString();
        }

        /// <summary>
        /// Genereate response for cache object
        /// </summary>
        /// <returns>200 http response with cache object</returns>
        public static HttpResponse GetCacheResponse(List<HttpHeader> headers, byte[] body)
        {
            var r = new HttpResponse("HTTP/1.1 200 Ok (from proxy cache)", headers, body);
            if (r.HasHeader("Date"))
            {
                r.UpdateHeader("Date", GetDateToday());
            }
            else
            {
                r.AddHeader("Date", GetDateToday());
            }
            return r;
        }

        /// <summary>
        /// Genereate response for placeholder image
        /// </summary>
        /// <returns>200 http response with placeholder image</returns>
        public static HttpResponse GetPlaceholderImageResponse(byte[] image)
        {
            var headers = new List<HttpHeader>
            {
                new HttpHeader("Content-Type","image/png"),
                new HttpHeader("Content-Length", image.Length.ToString()),
                new HttpHeader("Date", GetDateToday())
            };
            return new HttpResponse("HTTP/1.1 200 Ok (placeholder image)", headers, image);
        }

        /// <summary>
        /// Genereate 400 response
        /// </summary>
        /// <returns>400 http response</returns>
        public static HttpResponse Get400Response()
        {
            return new HttpResponse("HTTP/1.1 400 Bad Request", new List<HttpHeader> { new HttpHeader("Server", "NotS Proxy"), new HttpHeader("Connection", "Closed") }, new byte[] { });
        }

        /// <summary>
        /// Genereate 404 response
        /// </summary>
        /// <returns>404 http response</returns>
        public static HttpResponse Get404Response()
        {
            return new HttpResponse("HTTP/1.1 404 Not Found", new List<HttpHeader> { new HttpHeader("Server", "NotS Proxy"), new HttpHeader("Connection", "Closed") }, new byte[] { });
        }

        /// <summary>
        /// Genereate 407 response
        /// </summary>
        /// <returns>407 http response</returns>
        public static HttpResponse Get407Response()
        {
            return HttpResponse.Parse(Encoding.GetEncoding(1252).GetBytes("HTTP/1.1 407 Proxy Authentication Required\r\nProxy-Authenticate: Basic realm=\"Proxy\"\r\nContent-Length: 0\r\n\r\n"));
        }

        /// <summary>
        /// Genereate 500 response
        /// </summary>
        /// <returns>500 http response</returns>
        public static HttpResponse Get500Response()
        {
            return new HttpResponse("HTTP/1.1 500 Internal Server Error", new List<HttpHeader> { new HttpHeader("Server", "NotS Proxy"), new HttpHeader("Connection", "Closed") }, new byte[] { });
        }

        /// <summary>
        /// Genereate 503 response
        /// </summary>
        /// <returns>503 http response</returns>
        public static HttpResponse Get503Response()
        {
            return new HttpResponse("HTTP/1.1 503 Service Unavailable", new List<HttpHeader> { new HttpHeader("Server", "NotS Proxy"), new HttpHeader("Connection", "Closed") }, new byte[] { });
        }

        /// <summary>
        /// Genereate 504 response
        /// </summary>
        /// <returns>504 http response</returns>
        public static HttpResponse Get504Response()
        {
            return new HttpResponse("HTTP/1.1 504 Gateway Timeout", new List<HttpHeader> { new HttpHeader("Server", "NotS Proxy"), new HttpHeader("Connection", "Closed") }, new byte[] { });
        }
    }
}
