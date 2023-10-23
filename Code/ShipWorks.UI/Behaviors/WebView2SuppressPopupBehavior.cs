using System.Reflection;
using System.Windows.Interactivity;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

namespace ShipWorks.UI.Behaviors
{
    [Obfuscation(Exclude = true)]
    public class WebView2SuppressPopupBehavior : Behavior<WebView2>
    {
        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.NavigationCompleted += OnBrowserNavigated;
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.NavigationCompleted -= OnBrowserNavigated;
        }

        /// <summary>
        /// Suppresses errors and other popups
        /// </summary>
        private void OnBrowserNavigated(object sender, CoreWebView2NavigationCompletedEventArgs coreWebView2NavigationCompletedEventArgs)
        {
            WebView2 browser = (WebView2)sender;

            FieldInfo webBrowserInfo = browser.GetType()
                .GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);

            object comWebBrowser = webBrowserInfo?.GetValue(browser);

            comWebBrowser?.GetType()
                .InvokeMember("Silent", BindingFlags.SetProperty, null, comWebBrowser, new object[] { true });
        }
    }
}
