using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// UserControl for editing USPS profiles
    /// </summary>
    public partial class UspsProfileControl : StampsProfileControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UspsProfileControl(ShipmentTypeCode shipmentTypeCode) : base(shipmentTypeCode)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the given profile into the control
        /// </summary>
        public override void LoadProfile(ShippingProfileEntity profile)
        {
            base.LoadProfile(profile);

            StampsProfileEntity stampsProfile = profile.Postal.Stamps;

            AddValueMapping(stampsProfile, StampsProfileFields.HidePostage, stateRateShop, rateShop, labelRateShop);
        }
    }
}
