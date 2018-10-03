using Loadbalancer.Base;
using ProxyServer.Models;
using ProxyServer.Models.HTTP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ProxyServer.ViewModels
{
    public class ProxyVM : NotificationBase<Proxy>
    {
        public ProxyVM() : base(null) { }

        /// <summary>
        /// Get, Set proxy port
        /// </summary>
        public int ProxyPort
        {
            get { return This.Port; }
            set { SetProperty(This.Port, value, () => This.Port = value); }
        }

        /// <summary>
        /// Get, Set proxy cache timeout
        /// </summary>
        public int ProxyCacheTimeoutSec
        {
            get { return This.CacheTimeOutSec; }
            set { SetProperty(This.CacheTimeOutSec, value, () => This.CacheTimeOutSec = value); }
        }

        /// <summary>
        /// Enable, Diasable proxy changed content check
        /// </summary>
        public bool CheckChangeContent
        {
            get { return This.CheckChangeContent; }
            set { SetProperty(This.CheckChangeContent, value, () => This.CheckChangeContent = value); }
        }

        /// <summary>
        /// Enable, Diasable content filter
        /// </summary>
        public bool ContentFilter
        {
            get { return This.FilterContent; }
            set { SetProperty(This.FilterContent, value, () => This.FilterContent = value); }
        }

        /// <summary>
        /// Enable, Diasable identity header change
        /// </summary>
        public bool ChangeHeaders
        {
            get { return This.ChangeHeaders; }
            set {
                SetProperty(This.ChangeHeaders, value, () => This.ChangeHeaders = value);
                SetProperty(LogClient, false, () => LogClient = false);
                NotifyPropertyChanged(nameof(EnableUserShow));
            }
        }

        /// <summary>
        /// Enable, Diasable access authentication
        /// </summary>
        public bool AccessAuthentication
        {
            get { return This.AccessAuthentication; }
            set { SetProperty(This.AccessAuthentication, value, () => This.AccessAuthentication = value); }
        }

        /// <summary>
        /// Get, Set buffersize
        /// </summary>
        public int BufferSize
        {
            get { return This.BufferSize; }
            set { SetProperty(This.BufferSize, value, () => This.BufferSize = value); }
        }

        /// <summary>
        /// Enable, Diasable request headers logging
        /// </summary>
        public bool LogRequestHeaders
        {
            get { return This.LogRequestHeaders; }
            set { SetProperty(This.LogRequestHeaders, value, () => This.LogRequestHeaders = value); }
        }

        /// <summary>
        /// Enable, Diasable response headers logging
        /// </summary>
        public bool LogResponseHeaders
        {
            get { return This.LogResponseHeaders; }
            set { SetProperty(This.LogResponseHeaders, value, () => This.LogResponseHeaders = value); }
        }

        /// <summary>
        /// Enable, Diasable content in logging
        /// </summary>
        public bool LogContentIn
        {
            get { return This.LogContentIn; }
            set { SetProperty(This.LogContentIn, value, () => This.LogContentIn = value); }
        }

        /// <summary>
        /// Enable, Diasable content out logging
        /// </summary>
        public bool LogContentOut
        {
            get { return This.LogContentOut; }
            set { SetProperty(This.LogContentOut, value, () => This.LogContentOut = value); }
        }

        /// <summary>
        /// Enable, Diasable client identity logging
        /// </summary>
        public bool LogClient
        {
            get { return This.LogClient; }
            set { SetProperty(This.LogClient, value, () => This.LogClient = value); }
        }

        public bool NotActive
        {
            get { return !This.Active; }
        }

        public bool EnableUserShow { get => !This.ChangeHeaders; }

        public ObservableCollection<ListBoxItem> Log
        {
            get => This.Log;
        }

        /// <summary>
        /// Start or stop proxy, dependent on current state
        /// </summary>
        public void StartStopProxy()
        {
            if (This.Active)
            {
                This.Stop();
            }
            else
            {
                try
                {
                    This.Start();
                }
                catch (SocketException)
                {
                    MessageBox.Show("Kan server niet starten: poort al in gebruik.", "Poort error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            NotifyPropertyChanged(nameof(NotActive));
        }

        public HttpMessage GetMessage(int index)
        {
            if (0 <= index && index < This.HandledMessages.Count())
            {
                return This.HandledMessages[index];
            }
            return null;
        }

        public void ClearLog() => This.ClearLog();
    }
}
