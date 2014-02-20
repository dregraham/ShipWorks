﻿using System;
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
        /// Initializes a new instance of the <see cref="NoneServiceControl"/> class.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        public NoneServiceControl(RateControl rateControl) : 
            base (ShipmentTypeCode.None, rateControl)
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
