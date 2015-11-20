using System;
using ShipWorks.UI.Controls;
using Interapptive.Shared.Net;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// A specialized link for taking the user to associated help content
    /// </summary>
    public class HelpLink : LinkControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HelpLink()
        {
            Url = "http://www.interapptive.com/shipworks/help";
        }

        /// <summary>
        /// Gets or sets the URL that the help content is located at.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }

        /// <summary>
        /// Help Link click
        /// </summary>
        protected override void OnClick(EventArgs e)
        {
            WebHelper.OpenUrl(Url, this);
        }
    }
}
