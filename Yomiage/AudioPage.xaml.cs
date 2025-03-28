using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Yomiage.Models;
using System.Diagnostics;
using Yomiage.Services;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Yomiage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AudioPage : Page
    {
        public ObservableCollection<Article> Articles => App.Articles;
        public AudioPage()
        {
            this.InitializeComponent();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button || button.Tag is not string id)
            {
                return; // TODO エラーハンドリング
            }
            var article = App.Articles.FirstOrDefault(obj => obj.Id == id);
            if (article == null)
            {
                return;
            }
            App.Articles.Remove(article);
        }

        private async void PlayAllButton_Click(object sender, RoutedEventArgs e) {
            if (App.Articles.Count == 0)
            {
                return; // TODO エラーハンドリング
            }
            var texts = App.Articles.SelectMany(article => new List<string> { article.Title }.Concat(YahooParser.GetTexts(article.Html))).ToList();
            var voicevox = new VoiceVoxService();
            await voicevox.PlayTexts(texts, SubtitleDisplay);  
        }

        private void DeleteAllButton_Click(object sender, RoutedEventArgs e) {
            App.Articles.Clear();
        }
    }
}
