using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ShipWorks.UI.Controls;
using Interapptive.Shared.Net;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// A specialized link for taking the user to associated help content
    /// </summary>
    public partial class HelpLink : LinkControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HelpLink()
        {

        }

        /// <summary>
        /// Help Link click
        /// </summary>
        protected override void OnClick(EventArgs e)
        {
            WebHelper.OpenUrl("http://www.interapptive.com/shipworks/help", this);
        }
    }
}
