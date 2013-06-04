using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ShipWorks.ApplicationCore.Dashboard
{
    /// <summary>
    /// Represents an action that can be taken on a dashboard item
    /// </summary>
    public abstract class DashboardAction
    {
        string text;
        LinkArea linkArea;

        static Regex regex = new Regex(@"(?<pre>.*)\[link\](?<link>.*)\[/link\](?<post>.*)",
            RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        /// <summary>
        /// Standard constructor
        /// </summary>
        protected DashboardAction(string markup)
        {
            Match match = regex.Match(markup);
            if (match.Success)
            {
                string pre = match.Groups["pre"].Value;
                string link = match.Groups["link"].Value;
                string post = match.Groups["post"].Value;

                text = pre + link + post;
                linkArea = new LinkArea(pre.Length, link.Length);
            }
            else
            {
                text = markup;
            }
        }

        /// <summary>
        /// The text to display for the link.
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// The area within the text that is shown as linked.
        /// </summary>
        public LinkArea LinkArea
        {
            get { return linkArea; }
            set { linkArea = value; }
        }

        /// <summary>
        /// The link has been clicked and the action should execute.
        /// </summary>
        public void Execute(Control owner)
        {
            PerformAction(owner);
        }

        /// <summary>
        /// Overridden by derived classes to provide their specialized action processing
        /// </summary>
        abstract protected void PerformAction(Control owner);
    }
}
