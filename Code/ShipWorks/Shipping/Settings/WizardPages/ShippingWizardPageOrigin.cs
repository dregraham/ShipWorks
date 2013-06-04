using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;

namespace ShipWorks.Shipping.Settings.WizardPages
{
    /// <summary>
    /// Wizard page for managing the origins list
    /// </summary>
    public partial class ShippingWizardPageOrigin : WizardPage
    {
        bool initialized = false;
        ShipmentType shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingWizardPageOrigin(ShipmentType shipmentType)
        {
            InitializeComponent();

            this.shipmentType = shipmentType;
        }

        /// <summary>
        /// Stepping into the control
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            if (!initialized)
            {
                originManagerControl.Initialize();
                initialized = true;
            }
        }

        /// <summary>
        /// A new origin has been added
        /// </summary>
        private void OnOriginAdded(object sender, EventArgs e)
        {
            long originID = ShippingOriginManager.Origins.Max(s => s.ShippingOriginID);

            ShippingProfileEntity defaultProfile = shipmentType.GetPrimaryProfile();

            if (defaultProfile.OriginID == (int) ShipmentOriginSource.Store ||
                defaultProfile.OriginID == (int) ShipmentOriginSource.Other)
            {
                defaultProfile.OriginID = originID;

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.SaveAndRefetch(defaultProfile);
                }
            }
        }
    }
}
