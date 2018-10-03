using ProxyServer.Models.HTTP;
using ProxyServer.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ProxyServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProxyVM VM;
        public MainWindow()
        {
            InitializeComponent();
            VM = new ProxyVM();
            DataContext = VM;
        }

        private void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            VM.StartStopProxy();
        }

        private void ClearLogButton_Click(object sender, RoutedEventArgs e)
        {
            VM.ClearLog();
            HttpMessageDetails.Items.Clear();
        }

        private void LogList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HttpMessageDetails.Items.Clear();
            var m = VM.GetMessage(LogList.SelectedIndex);
            if (m != null)
            {
                if (VM.LogClient && m.HasHeader("user-agent"))
                {
                    HttpMessageDetails.Items.Add(new ListBoxItem { Content = m.GetHeader("user-agent").Value });
                }
                if ((VM.LogRequestHeaders && m.GetType() == typeof(HttpRequest)) || (VM.LogResponseHeaders && m.GetType() == typeof(HttpResponse)))
                {
                    HttpMessageDetails.Items.Add(new ListBoxItem { Content = m.HeadersAsString() });
                }
                if ((VM.LogContentIn && m.GetType() == typeof(HttpRequest)))
                {
                    HttpMessageDetails.Items.Add(new ListBoxItem { Content = m.BodyAsString() });
                }
                if ((VM.LogContentOut && m.GetType() == typeof(HttpResponse)))
                {
                    HttpResponse r = (HttpResponse)m;
                    if (r.Type == HttpContentType.image)
                    {
                        HttpMessageDetails.Items.Add(new ListBoxItem { Content = "<Image>" });
                    }
                    else
                    {
                        HttpMessageDetails.Items.Add(new ListBoxItem { Content = m.BodyAsString() });
                    }
                }
            }
        }
    }
}
