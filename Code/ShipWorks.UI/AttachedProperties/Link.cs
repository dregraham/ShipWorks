using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace ShipWorks.UI.AttachedProperties
{
    /// <summary>
    /// Attached property for opening a link in a browser
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class Link
    {
        /// <summary>
        /// The DependencyProperty
        /// </summary>
        public static readonly DependencyProperty OpenInBrowserProperty =
            DependencyProperty.RegisterAttached("OpenInBrowser", typeof(bool), typeof(Link),
                new PropertyMetadata(false, OnOpenInBrowserChanged));
        
        /// <summary>
        /// Wires up the event handler for when the url is clicked
        /// </summary>
        private static void OnOpenInBrowserChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Hyperlink hyperlink = d as Hyperlink;

            if(hyperlink == null)
            {
                return;
            }

            hyperlink.RequestNavigate -= HyperlinkOnRequestNavigate;

            if((bool)e.NewValue)
            {
                hyperlink.RequestNavigate += HyperlinkOnRequestNavigate;
            }
        }

        /// <summary>
        /// Opens the URL in the browser
        /// </summary>
        private static void HyperlinkOnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        /// <summary>
        /// Get the current value of the property
        /// </summary>
        public static bool GetOpenInBrowser(DependencyObject d) => (bool)d.GetValue(OpenInBrowserProperty);

        /// <summary>
        /// Set the current value of the property
        /// </summary>
        public static void SetOpenInBrowser(DependencyObject d, bool value) => d.SetValue(OpenInBrowserProperty, value);
    }
}
