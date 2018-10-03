using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyServer.Models
{
    class GzipParser
    {
        /// <summary>
        /// Unzip gzip byte[]
        /// </summary>
        /// <param name="gzipData">gzip byte[]</param>
        /// <returns>unzipped byte[]</returns>
        public static byte[] ParseToSByteArray(byte[] gzipData)
        {
            return Parse(new MemoryStream(gzipData));
        }

        /// <summary>
        /// Unzip gzip stream to byte[]
        /// </summary>
        /// <param name="gzipData">gzip stream</param>
        /// <returns>unzipped byte[]</returns>
        public static byte[] ParseToSByteArray(Stream gzipData)
        {
            return Parse(gzipData);
        }

        /// <summary>
        /// Unzip gzip byte[] to string
        /// </summary>
        /// <param name="gzipData">gzip byte[]</param>
        /// <returns>unzipped string</returns>
        public static string ParseToString(byte[] gzipData)
        {
            return Encoding.UTF8.GetString(Parse(new MemoryStream(gzipData)));
        }

        /// <summary>
        /// Unzip gzip stream to string
        /// </summary>
        /// <param name="gzipData">gzip stream</param>
        /// <returns>unzipped string</returns>
        public static string ParseToString(Stream gzipData)
        {
            return Encoding.UTF8.GetString(Parse(gzipData));
        }

        /// <summary>
        /// Parse gzipped byte[]
        /// </summary>
        /// <param name="gzipData">gzipped byte[]</param>
        /// <returns>unzipped byte[]</returns>
        private static byte[] Parse(Stream gzipData)
        {
            try
            {
                using (GZipStream decompressionStream = new GZipStream(gzipData, CompressionMode.Decompress))
                using (var decompressedMemory = new MemoryStream())
                {
                    decompressionStream.CopyTo(decompressedMemory);
                    decompressedMemory.Position = 0;
                    return decompressedMemory.ToArray();
                }
            }
            catch
            {
                return Encoding.UTF8.GetBytes("No valid Gzip");
            }
        }
    }
}
