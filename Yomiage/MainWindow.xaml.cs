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
using System.Reflection.Metadata;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Yomiage
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            NavigateToPage("web");
        }

        private void navView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            string tag = args.InvokedItemContainer.Tag.ToString()!;
            NavigateToPage(tag);
        }

        private void NavigateToPage(string tag)
        {
            Type? pageType = null;
            switch (tag)
            {
                case "web":
                    pageType = typeof(WebPage);
                    break;
                case "audio":
                    pageType = typeof(AudioPage);
                    break;
            }
            if (pageType == null || contentFrame.CurrentSourcePageType == pageType)
            {
                return;
            }
            contentFrame.Navigate(pageType);
        }

        private async void FooterItem_Tapped(object sender, TappedRoutedEventArgs e) {
            Uri uri = new Uri("https://www.google.com");
            await Launcher.LaunchUriAsync(uri);
        }
    }
}
