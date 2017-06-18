using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JNovelClubDL
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HtmlDocument doc = new HtmlDocument();

        public string Status { get; set; }
        public bool DownloadEnabled { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Status = "Loading Chromium engine...";
            browser.LoadingStateChanged += Browser_LoadingStateChanged;
        }

        private async void Browser_LoadingStateChanged(object sender, CefSharp.LoadingStateChangedEventArgs e)
        {
            if(!e.IsLoading)
            {
                await Task.Delay(750);
                var html = await browser.GetBrowser().MainFrame.GetSourceAsync();
                doc.LoadHtml(html);
                if (doc.DocumentNode.Descendants("div").Where(o => o.GetAttributeValue("class", "").Contains("PartReader__content")).FirstOrDefault() != null)
                {
                    Status = "Click Download button to save the currently open chapter.";
                    DownloadEnabled = true;
                }
                else
                {
                    Status = "Open novel reader first.";
                    DownloadEnabled = false;
                }
            }
        }

        private void TryDownloadNovel(object sender, RoutedEventArgs e)
        {
            // TODO
        }
    }
}
