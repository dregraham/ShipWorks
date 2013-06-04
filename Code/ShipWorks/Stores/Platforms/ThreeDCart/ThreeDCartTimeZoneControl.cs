using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Controls;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// Custom TimeZone selection control for ThreeDCart.  Exists just to ensure consistant help messaging.
    /// </summary>
    public partial class ThreeDCartTimeZoneControl : TimeZoneSelection
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartTimeZoneControl()
        {
            InitializeComponent();
        }
    }
}
