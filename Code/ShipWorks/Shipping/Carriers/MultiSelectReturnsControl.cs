using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Returns control for display when there are multiple providers selected that all support returns
    /// </summary>
    public partial class MultiSelectReturnsControl : ReturnsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MultiSelectReturnsControl()
        {
            InitializeComponent();
        }
    }
}
