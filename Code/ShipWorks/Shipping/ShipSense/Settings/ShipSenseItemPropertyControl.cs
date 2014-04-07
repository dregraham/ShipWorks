using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Shipping.ShipSense.Settings
{
    public partial class ShipSenseItemPropertyControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShipSenseItemPropertyControl"/> class.
        /// </summary>
        public ShipSenseItemPropertyControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the selected properties.
        /// </summary>
        /// <param name="selectedProperties">The selected properties.</param>
        public void LoadSelectedProperties(IEnumerable<string> selectedProperties)
        { }

        /// <summary>
        /// Gets the selected item properties.
        /// </summary>
        public List<string> SelectedItemProperties
        {
            get
            {
                return new List<string>();
            }
        }
    }
}
