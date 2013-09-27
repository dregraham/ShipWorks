using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore;
using ShipWorks.UI.Controls.Html;
using System.Windows.Forms;
using System.Threading;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// Utility class for working with template output
    /// </summary>
    public static class TemplateOutputUtility
    {        
        // Path to the native dll that contains the print template
        static string printTemplatePath;

        /// <summary>
        /// Static constructor
        /// </summary>
        static TemplateOutputUtility()
        {
            string resourceFile = DataPath.NativeDll;

            Uri resourceUri = new Uri(resourceFile);
            string resourcePath = "res://" + resourceUri.AbsolutePath.Replace("/", "\\") + "/";

            printTemplatePath = resourcePath + "PrintTemplate.htm";
        }

        /// <summary>
        /// The full path to the IE PrintTemplate we use
        /// </summary>
        public static string PrintTemplatePath
        {
            get { return printTemplatePath; }
        }

        /// <summary>
        /// Create a new HTML control that is bound to the UI thread.  This allows us to uses a spin-wait mechanism on a background
        /// thread to wait for its load state to be complete.  If parent is null, a new Control instance will be instantiated
        /// and used as the parent.  Note that the HtmlControl must be disposed on the UI thread
        /// </summary>
        public static HtmlControl CreateUIBoundHtmlControl(Control parent)
        {
            if (Program.ExecutionMode.IsUIDisplayed && Program.MainForm.InvokeRequired)
            {
                return (HtmlControl) Program.MainForm.Invoke(new Func<Control, HtmlControl>(CreateUIBoundHtmlControl), parent);
            }

            HtmlControl htmlControl = new HtmlControl();
            htmlControl.Left = -1000;
            htmlControl.Top = -1000;
            htmlControl.AllowActivation = false;

            if (parent == null)
            {
                parent = new Control();
            }

            // Set the parent
            htmlControl.Parent = parent;

            return htmlControl;
        }
    }
}
