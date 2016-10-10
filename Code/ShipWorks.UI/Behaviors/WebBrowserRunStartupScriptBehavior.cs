using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Navigation;

namespace ShipWorks.UI.Behaviors
{
    [Obfuscation(Exclude = true)]
    public class WebBrowserRunStartupScriptBehavior : Behavior<WebBrowser>
    {
        public static readonly DependencyProperty ScriptProperty = DependencyProperty.Register("Script",
            typeof(string), typeof(WebBrowserRunStartupScriptBehavior),
            new FrameworkPropertyMetadata("document.documentElement.style.overflow ='hidden'"));

        public static readonly DependencyProperty ScriptNameProperty = DependencyProperty.Register("ScriptName",
            typeof(string), typeof(WebBrowserRunStartupScriptBehavior), new FrameworkPropertyMetadata("InitScript"));

        public static readonly DependencyProperty ScriptTypeProperty = DependencyProperty.Register("ScriptType",
            typeof(string), typeof(WebBrowserRunStartupScriptBehavior), new FrameworkPropertyMetadata("JavaScript"));

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
            AssociatedObject.LoadCompleted += OnBrowserLoadCompleted;
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.LoadCompleted -= OnBrowserLoadCompleted;
        }

        /// <summary>
        /// Executes Script when BrowserLoadCompleted
        /// </summary>
        private void OnBrowserLoadCompleted(object sender, NavigationEventArgs e)
        {
            WebBrowser browser = (WebBrowser) sender;

            string scriptName = (string) GetValue(ScriptNameProperty);
            string script = (string) GetValue(ScriptProperty);
            string scriptType = (string) GetValue(ScriptTypeProperty);

            browser.InvokeScript(scriptName, script, scriptType);
        }
    }
}
