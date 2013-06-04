using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using System.Text.RegularExpressions;

namespace ShipWorks.ApplicationCore.Dashboard
{
    /// <summary>
    /// A dashboard action that results in the opening of a URL.
    /// </summary>
    public class DashboardActionUrl : DashboardAction
    {
        Uri targetUri;

        /// <summary>
        /// Takes markup that must contain an [link][/link] tag with an href.
        /// </summary>
        public DashboardActionUrl(string markup, string url)
            : base(markup)
        {
            if (!string.IsNullOrEmpty(url))
            {
                targetUri = new Uri(url);
            }
        }

        /// <summary>
        /// Takes markup that must contain an [link][/link] tag with an href.
        /// </summary>
        public DashboardActionUrl(string markup, Uri uri)
            : base(markup)
        {
            targetUri = uri;
        }

        /// <summary>
        /// Open the target URI
        /// </summary>
        protected override void PerformAction(Control owner)
        {
            if (targetUri != null)
            {
                WebHelper.OpenUrl(targetUri, owner);
            }
        }
    }
}
