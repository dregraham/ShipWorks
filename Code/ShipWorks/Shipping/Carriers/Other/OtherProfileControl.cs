using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Users.Security;
using ShipWorks.Users;
using ShipWorks.Shipping.Settings.Origin;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Carriers.Other
{
    /// <summary>
    /// Profile editor for the "Other" shipment type
    /// </summary>
    public partial class OtherProfileControl : ShippingProfileControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OtherProfileControl()
        {
            InitializeComponent();

            ResizeGroupBoxes(tabPage);
        }

        /// <summary>
        /// Load the UI for the given profile entity
        /// </summary>
        public override void LoadProfile(ShippingProfileEntity profile)
        {
            base.LoadProfile(profile);

            OtherProfileEntity other = profile.Other;

            List<KeyValuePair<string, long>> origins = ShipmentTypeManager.GetType(ShipmentTypeCode.Other).GetOrigins();

            originCombo.DisplayMember = "Key";
            originCombo.ValueMember = "Value";
            originCombo.DataSource = origins;

            // Add the mappings that control the value flow
            AddValueMapping(profile, ShippingProfileFields.OriginID, senderState, originCombo, labelSender);
            AddValueMapping(other, OtherProfileFields.Carrier, carrierState, carrier, labelCarrier);
            AddValueMapping(other, OtherProfileFields.Service, serviceState, service, labelService);

            // Insurance
            AddValueMapping(profile, ShippingProfileFields.Insurance, insuranceState, insuranceControl);

            // Returns
            AddValueMapping(profile, ShippingProfileFields.ReturnShipment, returnState, returnShipment);
        }
    }
}
