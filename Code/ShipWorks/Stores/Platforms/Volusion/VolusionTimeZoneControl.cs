using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Controls;

namespace ShipWorks.Stores.Platforms.Volusion
{
    /// <summary>
    /// Custom TimeZone selection control for Volusion.  Exists just to ensure consistant help messaging.
    /// </summary>
    public partial class VolusionTimeZoneControl : TimeZoneSelection
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionTimeZoneControl()
        {
            InitializeComponent();
        }
    }
}
