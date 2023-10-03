using System.Reflection;
using System.Windows;
using System.Windows.Interactivity;
using Microsoft.Web.WebView2.Wpf;

namespace ShipWorks.UI.Behaviors
{
    [Obfuscation(Exclude = true)]
    public class WebView2RunStartupScriptBehavior : Behavior<WebView2>
    {
        public static readonly DependencyProperty ScriptProperty = DependencyProperty.Register("Script",
            typeof(string), typeof(WebView2RunStartupScriptBehavior),
            new FrameworkPropertyMetadata("document.documentElement.style.overflow ='hidden'"));

        public static readonly DependencyProperty ScriptNameProperty = DependencyProperty.Register("ScriptName",
            typeof(string), typeof(WebView2RunStartupScriptBehavior), new FrameworkPropertyMetadata("InitScript"));

        public static readonly DependencyProperty ScriptTypeProperty = DependencyProperty.Register("ScriptType",
            typeof(string), typeof(WebView2RunStartupScriptBehavior), new FrameworkPropertyMetadata("JavaScript"));

        /// <summary>
        /// Gets or sets the script.
        /// </summary>
        public string Script { get; set; }

        /// <summary>
        /// Gets or sets the name of the script.
        /// </summary>
        public string ScriptName { get; set; }

        /// <summary>
        /// Gets or sets the type of the script.
        /// </summary>
        public string ScriptType { get; set; }

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += OnBrowserLoadCompleted;
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnBrowserLoadCompleted;
        }

        /// <summary>
        /// Executes Script when BrowserLoadCompleted
        /// </summary>
        private void OnBrowserLoadCompleted(object sender, RoutedEventArgs routedEventArgs)
        {
			WebView2 browser = (WebView2) sender;

            string scriptName = (string) GetValue(ScriptNameProperty);
            string script = (string) GetValue(ScriptProperty);
            string scriptType = (string) GetValue(ScriptTypeProperty);

            browser.ExecuteScriptAsync(scriptName);
        }
    }
}
