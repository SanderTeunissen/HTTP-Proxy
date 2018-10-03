using ProxyServer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace ProxyServer.Models.HTTP
{
    public class HttpMessage
    {
        public string FirstLine;
        public List<HttpHeader> Headers;
        public byte[] Body;

        /// <summary>
        /// HTTP message constructor
        /// </summary>
        /// <param name="firstLine">First line of the HTTP message</param>
        /// <param name="headers">List of headers of the HTTP message</param>
        /// <param name="body">Body of the HTTP message</param>
        protected HttpMessage(string firstLine, List<HttpHeader> headers, byte[] body)
        {
            FirstLine = firstLine;
            Headers = headers;
            Body = body;
        }

        /// <summary>
        /// Check if HTTP message has particular header
        /// </summary>
        /// <param name="key">Headername</param>
        /// <returns>True if header is in object, otherwise false</returns>
        public bool HasHeader(string key)
        {
            if (Headers.Where(x => x.Key.ToLower() == key.ToLower()).Count() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Get list of headers with particular name
        /// </summary>
        /// <param name="key">Headername</param>
        /// <returns>All instances Http header object of header with given name</returns>
        public List<HttpHeader> GetHeaders(string key)
        {
            return Headers.Where(x => x.Key.ToLower() == key.ToLower()).ToList();
        }

        /// <summary>
        /// Get specific header
        /// </summary>
        /// <param name="key">Headername</param>
        /// <param name="occurence">Position of the header from all headers with that name</param>
        /// <returns>Http header object</returns>
        public HttpHeader GetHeader(string key, int occurence = 1)
        {
            var filter = Headers.Where(x => x.Key.ToLower() == key.ToLower());
            int count = filter.Count();
            if (count <= 0 || count > occurence)
            {
                throw new KeyNotFoundException();
            }
            return filter.ToArray()[occurence - 1];
        }

        /// <summary>
        /// Add new header to HTTP message
        /// </summary>
        /// <param name="key">Name of the header</param>
        /// <param name="value">Value of the header</param>
        public void AddHeader(string key, string value)
        {
            Headers.Add(new HttpHeader(key, value));
        }

        /// <summary>
        /// Update the value from a header
        /// </summary>
        /// <param name="key">Name of the header</param>
        /// <param name="newValue">Updated value</param>
        /// <param name="occurence">Position of the header from all headers with that name</param>
        public void UpdateHeader(string key, string newValue, int occurence = 1)
        {
            var filter = Headers.Where(x => x.Key.ToLower() == key.ToLower());
            int count = filter.Count();
            if (count <= 0 || count > occurence)
            {
                throw new KeyNotFoundException();
            }
            filter.ToArray()[occurence - 1].Value = newValue;
        }

        /// <summary>
        /// Remove a header from the HTTP message
        /// </summary>
        /// <param name="key">Name of the header</param>
        /// <param name="occurence">Position of the header from all headers with that name</param>
        public void DeleteHeader(string key, int occurence = 1)
        {
            var filter = Headers.Where(x => x.Key.ToLower() == key.ToLower());
            int count = filter.Count();
            if (count <= 0 || count > occurence)
            {
                throw new KeyNotFoundException();
            }
            Headers.Remove(filter.ToArray()[occurence - 1]);
        }

        /// <summary>
        /// Support function for parsing message, used for reading all lines
        /// </summary>
        /// <param name="ByteArray">HTTP message object</param>
        /// <returns>List of all lines read in byte[]</returns>
        protected static List<string> ReadLines(byte[] ByteArray)
        {
            List<string> lines = new List<string>();
            StringBuilder sb = new StringBuilder();
            foreach (byte x in ByteArray)
            {
                if (x == '\n')
                {
                    string line = sb.ToString();
                    lines.Add(line);
                    sb.Clear();
                    continue;
                }
                if (x == '\r')
                    continue;
                sb.Append(Convert.ToChar(x));
            }
            lines.Add(sb.ToString());
            return lines;
        }

        /// <summary>
        /// Read all headers from lines
        /// </summary>
        /// <param name="lines">List of lines from the HTTP message</param>
        /// <param name="firstLineIncluded">If the first line is included or not</param>
        /// <returns>List of all HTTP message headers</returns>
        protected static List<HttpHeader> ReadHeaders(List<string> lines, bool firstLineIncluded = true)
        {
            bool skipedFirst = false;
            List<HttpHeader> list = new List<HttpHeader>();
            foreach (string line in lines)
            {
                if (!skipedFirst && firstLineIncluded)
                {
                    skipedFirst = true;
                }
                else
                {
                    if (line.Equals(""))
                    {
                        break;
                    }
                    int Seperator = line.IndexOf(':');
                    if (Seperator == -1)
                    {
                        throw new Exception(string.Format("Invalid http header line: {0}", line));
                    }
                    string Key = line.Substring(0, Seperator);
                    int Pos = Seperator + 1;
                    while ((Pos < line.Length) && (line[Pos] == ' '))
                    {
                        Pos++;
                    }
                    string Value = line.Substring(Pos, line.Length - Pos);
                    list.Add(new HttpHeader(Key, Value));
                }
            }
            return list;
        }

        /// <summary>
        /// Read the body of a HTTP message as byte[]
        /// </summary>
        /// <param name="ByteArray">HTTP message</param>
        /// <returns>Byte[] of the body</returns>
        protected static byte[] ReadBody(byte[] ByteArray)
        {
            StringBuilder sb = new StringBuilder();
            int index = 0;
            for (int i = 0; i < ByteArray.Length; i++)
            {
                if (ByteArray[i] == '\n')
                {
                    string line = sb.ToString();
                    if (line == "")
                    {
                        index = i + 1;
                        break;
                    }
                    sb.Clear();
                    continue;
                }
                if (ByteArray[i] == '\r')
                    continue;
                sb.Append(Convert.ToChar(ByteArray[i]));
            }
            if (index > 0)
            {
                return ByteArray.Skip(index).ToArray();
            }
            return new byte[] { };
        }

        /// <summary>
        /// Parse byte[] body to string
        /// </summary>
        /// <returns>HTTP Body as string</returns>
        public string BodyAsString()
        {
            Debug.WriteLine("bd:" + Body.Length);
            if (HasHeader("Content-Encoding") && GetHeader("Content-Encoding").Value == "gzip")
            {
                return GzipParser.ParseToString(Body);
            }
            return Encoding.GetEncoding("ISO-8859-1").GetString(Body);
        }

        /// <summary>
        /// HTTP message as string
        /// </summary>
        public new string ToString
        {
            get {
                if (!HasHeader("content-length"))
                {
                    String.Format("{0}{2}{1}", FirstLine, HeadersAsString(), Environment.NewLine);
                }
                return String.Format("{0}{3}{1}{3}{3}{2}", FirstLine,HeadersAsString(), BodyAsString(), Environment.NewLine);
            }
        }

        /// <summary>
        /// HTTP message as byte[]
        /// </summary>
        public byte[] ToBytes
        {
            get
            {
                byte[] newLine = Encoding.ASCII.GetBytes(Environment.NewLine);
                List<byte> result = new List<byte>();
                result.AddRange(Encoding.ASCII.GetBytes(FirstLine));
                result.AddRange(newLine);
                result.AddRange(Encoding.ASCII.GetBytes(HeadersAsString()));
                result.AddRange(newLine);
                result.AddRange(newLine);
                result.AddRange(Body);
                return result.ToArray();
            }
        }

        /// <summary>
        /// Get all headers as string
        /// </summary>
        public string HeadersAsString()
        {
            return String.Join(Environment.NewLine, Headers.Select(x => x.ToString));
        }
    }
}
