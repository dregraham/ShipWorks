using System.Collections.Generic;
using System.Data;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Profiles;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.ShipEngine;
using Interapptive.Shared.Enums;
using ShipWorks.Shipping.Carriers.Amazon.SWA;

namespace ShipWorks.Shipping.UI.Carriers.Amazon.SWA
{
    /// <summary>
    /// AmazonSWA Profile Control
    /// </summary>
    [KeyedComponent(typeof(ShippingProfileControlBase), ShipmentTypeCode.AmazonSWA)]
    public partial class AmazonSWAProfileControl : ShippingProfileControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonSWAProfileControl" /> class.
        /// </summary>
        public AmazonSWAProfileControl()
        {
            InitializeComponent();

            ResizeGroupBoxes(tabPageSettings);

            // ShipEngine does not support EPL
            requestedLabelFormat.ExcludeFormats(ThermalLanguage.EPL);
        }

        /// <summary>
        /// Load the given profile
        /// </summary>
        /// <param name="profile"></param>
        public override void LoadProfile(ShippingProfileEntity profile)
        {
            base.LoadProfile(profile);

            dimensionsControl.Initialize();

            AmazonSWAProfileEntity AmazonSWAProfile = profile.AmazonSWA;
            PackageProfileEntity packageProfile = profile.Packages.Single();

            LoadAmazonSWAAccounts();

            EnumHelper.BindComboBox<AmazonSWAServiceType>(service);

            // From
            AddValueMapping(AmazonSWAProfile, AmazonSWAProfileFields.AmazonSWAAccountID, accountState, AmazonSWAAccount, labelAccount);

            // Service
            AddValueMapping(AmazonSWAProfile, AmazonSWAProfileFields.Service, serviceState, service, labelService);

            // Weight and dimensions
            AddValueMapping(packageProfile, PackageProfileFields.Weight, weightState, weight, labelWeight);
            AddValueMapping(packageProfile, PackageProfileFields.DimsProfileID, dimensionsState, dimensionsControl, labelDimensions);

            // Labels
            AddValueMapping(profile, ShippingProfileFields.RequestedLabelFormat, requestedLabelFormatState, requestedLabelFormat, labelThermalNote);
        }

        /// <summary>
        /// Loads the AmazonSWA accounts.
        /// </summary>
        private void LoadAmazonSWAAccounts()
        {
            AmazonSWAAccount.DisplayMember = "Key";
            AmazonSWAAccount.ValueMember = "Value";

            if (AmazonSWAAccountManager.Accounts.Count > 0)
            {
                AmazonSWAAccount.DataSource = AmazonSWAAccountManager.Accounts.Select(a => new KeyValuePair<string, long>(a.Description, a.AmazonSWAAccountID)).ToList();
                AmazonSWAAccount.Enabled = true;
            }
            else
            {
                AmazonSWAAccount.DataSource = new List<KeyValuePair<string, long>> { new KeyValuePair<string, long>("(No accounts)", 0) };
                AmazonSWAAccount.Enabled = false;
            }
        }

        /// <summary>
        /// Save all the package profile stuff to their entities
        /// </summary>
        public override void SaveToEntity()
        {
            base.SaveToEntity();

            if (dimensionsControl.Enabled)
            {
                dimensionsControl.SaveToEntities();
            }
        }
    }
}
