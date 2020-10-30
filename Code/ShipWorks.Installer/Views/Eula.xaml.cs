using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ShipWorks.Installer.Views
{
    /// <summary>
    /// Interaction logic for Eula.xaml
    /// </summary>
    public partial class Eula : Page
    {
        public Eula()
        {
            InitializeComponent();

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ShipWorks.Installer.License.rtf";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                eulaTextBox.Selection.Load(stream, DataFormats.Rtf);
                eulaTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

                eulaTextBox.AddHandler(Hyperlink.RequestNavigateEvent, new RoutedEventHandler(HandleHyperlinkClick));

            }
        }

        /// <summary>
        /// Handle clicking a URL 
        /// </summary>
        private void HandleHyperlinkClick(object inSender, RoutedEventArgs inArgs)
        {
                Hyperlink link = inArgs.Source as Hyperlink;
                if (link != null)
                {
                    OpenUrl(link.NavigateUri.ToString());
                    inArgs.Handled = true;
                }
        }

        /// <summary>
        /// Open the URL in default browser
        /// </summary>
        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
