using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.Postal.Endicia;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Express1-specific Service Control.
    /// </summary>
    public partial class Express1ServiceControl : EndiciaServiceControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1ServiceControl()
            : base (ShipmentTypeCode.PostalExpress1, EndiciaReseller.Express1)
        {
            InitializeComponent();
        }
    }
}
