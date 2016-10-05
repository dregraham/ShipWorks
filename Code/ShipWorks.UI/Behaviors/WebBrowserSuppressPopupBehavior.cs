using System.Reflection;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Navigation;

namespace ShipWorks.UI.Behaviors
{
    [Obfuscation(Exclude = true)]
    public class WebBrowserSuppressPopupBehavior : Behavior<WebBrowser>
    {
        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.Navigated += OnBrowserNavigated;
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.Navigated -= OnBrowserNavigated;
        }

        /// <summary>
        /// Suppresses errors and other popups
        /// </summary>
        private void OnBrowserNavigated(object sender, NavigationEventArgs e)
        {
            WebBrowser browser = (WebBrowser)sender;

            FieldInfo webBrowserInfo = browser.GetType()
                .GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);

            object comWebBrowser = webBrowserInfo?.GetValue(browser);

            comWebBrowser?.GetType()
                .InvokeMember("Silent", BindingFlags.SetProperty, null, comWebBrowser, new object[] { true });
        }
    }
}
