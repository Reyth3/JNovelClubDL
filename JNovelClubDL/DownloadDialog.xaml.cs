using HtmlAgilityPack;
using PdfSharp.Pdf;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JNovelClubDL
{
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// Interaction logic for DownloadDialog.xaml
    /// </summary>
    public partial class DownloadDialog : Window
    {
        public DownloadDialog()
        {
            InitializeComponent();
        }

        private HtmlDocument _doc;

        public string Filename { get; set; }
        public double Progress { get; set; }

        private void StartDownload(object sender, RoutedEventArgs e)
        {
            if (_doc == null || Filename == null)
                return;
            Progress = 0;
            var b = (sender as Button);
            b.IsEnabled = false;
            var selectedFormat = formatsPanel.Children.OfType<RadioButton>().FirstOrDefault(o => o.IsChecked == true);
            if(selectedFormat == null)
            {
                MessageBox.Show("You have to select the output format first.", "Output Format", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var destination = saveTo.Children.OfType<RadioButton>().FirstOrDefault(o => o.IsChecked == true);
            if(destination == null)
            {
                MessageBox.Show("You need to select the destination first.", "File Destination", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Progress = 0.2;
            var html = _doc.DocumentNode.Descendants("div").Where(o => o.GetAttributeValue("class", "").Contains("PartReader__content")).FirstOrDefault();
            if(html == null)
            {
                MessageBox.Show("There was a problem with the website.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var path = System.IO.Path.Combine(destination.Content.ToString() == "Subfolder" ? Directory.GetCurrentDirectory() : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "J-Novel Club", Filename);
            switch (selectedFormat.Content.ToString())
            {
                case "HTML":
                    path += ".html";
                    File.WriteAllText(path, html.InnerHtml);
                    break;
                case "TXT":
                    path += ".txt";
                    File.WriteAllText(path, WebUtility.HtmlDecode(html.InnerText));
                    break;
                case "EPUB":
                    // TODO
                    MessageBox.Show("Not supported yet.");
                    break;
                case "PDF":
                    // TODO
                    MessageBox.Show("Not supported yet.");
                    break;
            }
            Progress = 1;
            b.IsEnabled = true;
        }

        public void SetSource(HtmlDocument document, string title)
        {
            _doc = document;
            Filename = title;
        }
    }
}
