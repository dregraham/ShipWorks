using System;
using System.Linq;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.UI.Wizard;

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

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IShippingProfileManager shippingProfileManager = lifetimeScope.Resolve<IShippingProfileManager>();
                ShippingProfileEntity defaultProfile = shippingProfileManager.GetOrCreatePrimaryProfile(shipmentType);

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
}
