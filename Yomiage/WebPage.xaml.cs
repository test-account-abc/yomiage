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
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Animation;
using Yomiage.Services;
using Yomiage.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Yomiage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WebPage : Page
    {
        private readonly RotateTransform expandButtonRotateTransform;
        private double currentExpandButtonAngle = 0;
        private bool isExpandedButtons = true;

        public WebPage()
        {
            this.InitializeComponent();
            expandButtonRotateTransform = new RotateTransform() { Angle = 0 };
            ExpandButton.RenderTransform = expandButtonRotateTransform;
            ExpandButton.RenderTransformOrigin = new Windows.Foundation.Point(0.5, 0.5);
            AddArticleButton.IsEnabled = false;
        }

        private void ExpandButton_Click(object sender, RoutedEventArgs e)
        {
            StartExpandButtonAnimation();
            HandleButtonsPanelExpansion();
        }

        private void StartExpandButtonAnimation()
        {
            currentExpandButtonAngle += 180;
            var animation = new DoubleAnimation()
            {
                To = currentExpandButtonAngle,
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut }
            };
            var storyboard = new Storyboard();
            storyboard.Children.Add(animation);

            Storyboard.SetTarget(animation, expandButtonRotateTransform);
            Storyboard.SetTargetProperty(animation, "Angle");

            storyboard.Begin();
        }

        private void HandleButtonsPanelExpansion()
        {
            isExpandedButtons = !isExpandedButtons;
            AddArticleButton.Visibility = isExpandedButtons ? Visibility.Visible : Visibility.Collapsed;

            DoubleAnimation expandAnimation = new DoubleAnimation()
            {
                To = isExpandedButtons ? 230 : 50,
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                EasingFunction = new QuadraticEase()
            };

            Storyboard storyboard = new Storyboard();
            Storyboard.SetTarget(expandAnimation, ButtonsPanel);
            Storyboard.SetTargetProperty(expandAnimation, "Width");
            storyboard.Children.Add(expandAnimation);
            storyboard.Begin();
        }

        private bool IsAllowedUrl()
        {
            string? url = WebView?.Source?.ToString();
            if (url == null )
            {
                return false;
            }
            return UrlParser.IsYahooPickupUrl(url) || UrlParser.IsYahooArticleUrl(url);
        }

        private async void AddArticleButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsAllowedUrl())
            {
                return;
            }
            AddArticleButton.IsEnabled = false;
            string url = WebView.Source.ToString();
            var html = await GetHtmlFromWebView();
            if (UrlParser.IsYahooPickupUrl(url))
            {
                html = await HttpRequest.FetchHtmlAsync(YahooParser.GetArticleUrlFromPickup(html)!);
            }
            var article = new Article(url, html, WebView.CoreWebView2.DocumentTitle);
            App.Articles.Add(article);
            AddArticleButton.IsEnabled = true;
            ShowAddArticleCompletionInfoBar();
        }

        private void WebView_NavigationCompleted(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            AddArticleButton.IsEnabled = IsAllowedUrl();
        }

        private async Task<string> GetHtmlFromWebView()
        {
            if (WebView.CoreWebView2 == null)
            {
                return string.Empty;
            }
            try
            {
                var html = await WebView.CoreWebView2.ExecuteScriptAsync("document.documentElement.outerHTML");
                return System.Text.Json.JsonSerializer.Deserialize<string>(html)!;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        private void AddArticleCompletionInfoBar_CloseButtonClick(InfoBar sender, object args)
        {
            AddArticleCompletionInfoBar.IsOpen = false;
        }

        private async void ShowAddArticleCompletionInfoBar()
        {
            AddArticleCompletionInfoBar.IsOpen = true;
            await Task.Delay(3000);
            AddArticleCompletionInfoBar.IsOpen = false;
        }
    }
}
