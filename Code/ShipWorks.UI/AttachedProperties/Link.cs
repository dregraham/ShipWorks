using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace ShipWorks.UI.AttachedProperties
{
    public class Link
    {
        public static readonly DependencyProperty OpenInBrowserProperty =
            DependencyProperty.RegisterAttached("OpenInBrowser", typeof(bool), typeof(Link),
                new PropertyMetadata(false, OnOpenInBrowserChanged));

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

        private static void HyperlinkOnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
