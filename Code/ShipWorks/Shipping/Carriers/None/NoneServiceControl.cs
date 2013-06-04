using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.None
{
    /// <summary>
    /// UserControl for editing the settings of the "None" service type
    /// </summary>
    public partial class NoneServiceControl : ServiceControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NoneServiceControl() : 
            base (ShipmentTypeCode.None)
        {
            InitializeComponent();
        }

        /// <summary>
        /// This control has no weight to refresh.
        /// </summary>
        public override void RefreshContentWeight()
        {

        }
    }
}
