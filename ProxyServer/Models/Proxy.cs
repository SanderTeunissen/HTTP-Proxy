using ProxyServer.Models.HTTP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ProxyServer.Models
{
    public class Proxy
    {
        #region Proxy Settings
        public int Port { get; set; }
        public int CacheTimeOutSec { get; set; }
        public bool CheckChangeContent { get; set; }
        public bool FilterContent { get; set; }
        public bool ChangeHeaders { get; set; }
        public bool AccessAuthentication { get; set; }
        public int BufferSize { get; set; }
        #endregion

        #region Log Settings
        public bool LogRequestHeaders { get; set; }
        public bool LogResponseHeaders { get; set; }
        public bool LogContentIn { get; set; }
        public bool LogContentOut { get; set; }
        public bool LogClient { get; set; }
        #endregion

        private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

        public ObservableCollection<HttpMessage> HandledMessages { get; set; }

        public ObservableCollection<ListBoxItem> Log { get; set; }

        public bool Active { get; set; }
        public bool Stopping { get; set; }

        private TcpListener TcpListener;

        private byte[] PlaceholderImage;

        private List<CacheItem> ProxyCache;

        public int FrequentUseCount { get; set; }

        public Proxy()
        {
            Port = 8080;
            CacheTimeOutSec = 300;
            CheckChangeContent = false;
            FilterContent = false;
            ChangeHeaders = false;
            AccessAuthentication = false;
            BufferSize = 1024;
            LogRequestHeaders = true;
            LogResponseHeaders = true;
            LogContentIn = true;
            LogContentOut = true;
            LogClient = true;
            HandledMessages = new ObservableCollection<HttpMessage>();
            PlaceholderImage = ReadPlaceholderImage();
            Log = new ObservableCollection<ListBoxItem>();
            ProxyCache = new List<CacheItem>();
            FrequentUseCount = 5;
        }

        public void ClearLog()
        {
            HandledMessages.Clear();
            Log.Clear();
        }

        /// <summary>
        /// Start the proxy server
        /// </summary>
        public void Start()
        {
            if (!Stopping)
            {
                TcpListener = new TcpListener(IPAddress.Any, Port);
                TcpListener.Start();
                Active = true;
                Task.Run(() => ListenForRequests());
            }
        }

        /// <summary>
        /// Stop the proxy server
        /// </summary>
        public void Stop()
        {
            Active = false;
            Stopping = true;
            Task.Run(() =>
            {
                while (true)
                {
                    if (TcpListener.Pending()) continue;
                    TcpListener.Stop();
                    break;
                }
                Stopping = false;
            });
        }

        private byte[] ReadPlaceholderImage()
        {
            using (FileStream file = new FileStream(@"C:\Users\Sander\Desktop\ProxyServer\ProxyServer\Content\placeholder.png", FileMode.Open))
            {
                byte[] bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                file.Close();
                return bytes;
            }
        }

        /// <summary>
        /// Start listing for incoming requests
        /// </summary>
        private void ListenForRequests()
        {
            while (Active)
            {
                try
                {
                    TcpClient client = TcpListener.AcceptTcpClient();
                    Task.Run(() => HandleRequest(client));
                }
                catch
                {
                    if (!Stopping && Active)
                    {
                        Console.WriteLine("Could not connect to client");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        private async void HandleRequest(TcpClient client)
        {
            try
            {
                using (client)
                using (NetworkStream clientNs = client.GetStream())
                using (MemoryStream clienMs = new MemoryStream())
                {
                    var buffer = new byte[BufferSize];
                    int bytesRead;
                    Thread.Sleep(5);
                    if (clientNs.CanRead)
                    {
                        if (clientNs.DataAvailable)
                        {
                            do
                            {
                                bytesRead = clientNs.Read(buffer, 0, buffer.Length);
                                clienMs.Write(buffer, 0, bytesRead);
                            } while (clientNs.DataAvailable);
                        }
                    }
                    try
                    {
                        HttpRequest request = HttpRequest.Parse(clienMs.GetBuffer());
                        dispatcher.Invoke(() => AddToLog(request));
                        HttpResponse response = null;
                        if (!AccessAuthentication || Authenticated(request))
                        {
                            FilterUser(request);
                            if (FilterContent && request.AsksForImage())
                            {
                                response = HttpResponse.GetPlaceholderImageResponse(PlaceholderImage);
                                clientNs.Write(response.ToBytes, 0, response.ToBytes.Length);
                            }
                            if (ProxyCache.Where(x => x.RequestLine == request.RequestLine && x.Valid(CacheTimeOutSec)).Count() > 0 && (CheckChangeContent || HandledMessages.Where(x => x.FirstLine == request.FirstLine).Count() > FrequentUseCount))
                            {
                                response = ProxyCache.FirstOrDefault(x => x.RequestLine == request.RequestLine && x.Valid(CacheTimeOutSec)).ToResponse();
                                clientNs.Write(response.ToBytes, 0, response.ToBytes.Length);
                            }
                            if (response == null)
                            {
                                try
                                {
                                    using (TcpClient serverClient = new TcpClient(request.GetRequestedHost(), 80))
                                    using (NetworkStream serverStream = serverClient.GetStream())
                                    {
                                        serverClient.SendTimeout = 500;
                                        serverClient.ReceiveTimeout = 1000;
                                        var serverBuffer = new byte[BufferSize];
                                        using (MemoryStream ms = new MemoryStream())
                                        {
                                            await serverStream.WriteAsync(request.ToBytes, 0, request.ToBytes.Length);
                                            int numBytesRead;
                                            try
                                            {
                                                while ((numBytesRead = serverStream.Read(serverBuffer, 0, serverBuffer.Length)) > 0)
                                                {
                                                    clientNs.Write(serverBuffer, 0, numBytesRead);
                                                    ms.Write(serverBuffer, 0, numBytesRead);
                                                }
                                            }
                                            catch { }
                                            try
                                            {
                                                response = HttpResponse.Parse(ms.GetBuffer());
                                                if (IsCacheable(request) && (CheckChangeContent || HandledMessages.Where(x => x.FirstLine == request.FirstLine).Count() > FrequentUseCount))
                                                {
                                                    ProxyCache.Add(new CacheItem(request, response));
                                                }
                                            }
                                            catch
                                            {
                                                Debug.WriteLine("Response not parsable");
                                            }
                                        }
                                    }
                                }
                                catch (SocketException)
                                {
                                    clientNs.Write(HttpResponse.Get404Response().ToBytes, 0, HttpResponse.Get400Response().ToBytes.Length);
                                }
                            }
                        }
                        else
                        {
                            response = HttpResponse.Get407Response();
                            clientNs.Write(response.ToBytes, 0, response.ToBytes.Length);
                        }
                        dispatcher.Invoke(() => AddToLog(response));
                    }
                    catch (FormatException)
                    {
                        clientNs.Write(HttpResponse.Get400Response().ToBytes, 0, HttpResponse.Get400Response().ToBytes.Length);
                    }
                    catch
                    {
                        clientNs.Write(HttpResponse.Get500Response().ToBytes, 0, HttpResponse.Get400Response().ToBytes.Length);
                    }
                }
            }
            catch { Debug.WriteLine("Error while handling incomming request"); }
        }

        /// <summary>
        /// Add new message to log
        /// </summary>
        /// <param name="message"></param>
        public void AddToLog(HttpMessage message)
        {
            HandledMessages.Add(message);
            Log.Add(new ListBoxItem { Content = message.FirstLine });
        }

        /// <summary>
        /// Remove user from request if option is active
        /// </summary>
        /// <param name="request">Incoming request</param>
        public void FilterUser(HttpRequest request)
        {
            if (request.HasHeader("user-agent") && ChangeHeaders)
            {
                request.DeleteHeader("user-agent");
            }
        }

        /// <summary>
        /// Check if item is cachable
        /// </summary>
        /// <param name="request">Incoming request</param>
        /// <returns>True when cacheable</returns>
        public bool IsCacheable(HttpRequest request)
        {
            if (FilterContent && request.AsksForImage())
                return false;
            if (!request.RequestLine.Split(' ')[0].ToLower().Contains("get"))
                return false;
            if (request.HasHeader("Authorization"))
                return false;
            if (request.HasHeader("Cache-Control") && request.GetHeader("Cache-Control").Value == "private")
                return false;
            return true;
        }

        /// <summary>
        /// Check if request is authenticated
        /// </summary>
        /// <param name="request">incoming request</param>
        /// <returns>True if authenticated, otherwise false</returns>
        public bool Authenticated(HttpRequest request)
        {
            if (request.HasHeader("Proxy-Authorization"))
            {
                string authstring = request.GetHeader("Proxy-Authorization").Value;
                string username = "admin";
                string password = "admin";
                string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                if (authstring.StartsWith("Basic "))
                    if (authstring.Equals("Basic " + encoded)) return true;
            }
            return false;
        }
    }
}
