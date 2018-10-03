using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyServer.Models.HTTP
{
    public class HttpRequest : HttpMessage
    {
        /// <summary>
        /// HTTP Request first line
        /// </summary>
        public string RequestLine
        {
            get => base.FirstLine;
            set
            {
                base.FirstLine = value;
            }
        }

        /// <summary>
        /// Constructor for HTTP Request
        /// </summary>
        /// <param name="requestLine">First line of the request</param>
        /// <param name="headers">Request headers</param>
        /// <param name="body">Request body</param>
        public HttpRequest(string requestLine, List<HttpHeader> headers, byte[] body) : base(requestLine, headers, body) { }

        /// <summary>
        /// Parse request object from byte[]
        /// </summary>
        /// <param name="request">Request as byte[]</param>
        /// <returns>Request as object</returns>
        public static HttpRequest Parse(byte[] request)
        {
            try
            {
                if (request.Count() == 0)
                    throw new Exception();
                var lines = ReadLines(request);
                return new HttpRequest(lines[0], ReadHeaders(lines), ReadBody(request));
            }
            catch
            {
                throw new FormatException();
            }
        }

        /// <summary>
        /// Checks if request asks server to send back an image
        /// </summary>
        /// <returns>True is requests image, otherwise false</returns>
        public bool AsksForImage()
        {
            var ImageFiles = new string[] { ".jpg",".png", ".gif", ".webp", ".svg", ".ai", ".eps" };
            return ImageFiles.Any(type => RequestLine.Split(' ')[1].ToLower().Contains(type));
        }

        /// <summary>
        /// Get the host from the request
        /// </summary>
        /// <returns>Host address as string</returns>
        public string GetRequestedHost()
        {
            if (HasHeader("Host"))
            {
                return GetHeader("Host").Value;
            }
            return FirstLine.Split(' ')[1];
        }
    }
}
