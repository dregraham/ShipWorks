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
using ShipWorks.Shipping.Carriers.Asendia;

namespace ShipWorks.Shipping.UI.Carriers.Asendia
{
    /// <summary>
    /// Asendia Profile Control
    /// </summary>
    [KeyedComponent(typeof(ShippingProfileControlBase), ShipmentTypeCode.Asendia)]
    public partial class AsendiaProfileControl : ShippingProfileControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsendiaProfileControl" /> class.
        /// </summary>
        public AsendiaProfileControl()
        {
            InitializeComponent();

            ResizeGroupBoxes(tabPageSettings);

            // ShipEngine does not support ZPL
            requestedLabelFormat.ExcludeFormats(ThermalLanguage.EPL);
        }

        /// <summary>
        /// Load the given profile
        /// </summary>
        /// <param name="profile"></param>
        public override void LoadProfile(ShippingProfileEntity profile)
        {
            base.LoadProfile(profile);

            AsendiaProfileEntity AsendiaProfile = profile.Asendia;
            
            LoadAsendiaAccounts();

            EnumHelper.BindComboBox<AsendiaServiceType>(service);
            EnumHelper.BindComboBox<ShipEngineContentsType>(contents);
            EnumHelper.BindComboBox<ShipEngineNonDeliveryType>(nonDelivery);

            //From
            AddValueMapping(AsendiaProfile, AsendiaProfileFields.AsendiaAccountID, accountState, asendiaAccount, labelAccount);

            //Service
            AddValueMapping(AsendiaProfile, AsendiaProfileFields.Service, serviceState, service, labelService);

            // Labels
            AddValueMapping(profile, ShippingProfileFields.RequestedLabelFormat, requestedLabelFormatState, requestedLabelFormat, labelThermalNote);

            // Insurance
            AddValueMapping(profile, ShippingProfileFields.Insurance, insuranceState, insuranceControl);
            
            //Options
            AddValueMapping(AsendiaProfile, AsendiaProfileFields.NonMachinable, nonMachinableState, nonMachinable, labelNonMachinable);

            //Customs
            AddValueMapping(AsendiaProfile, AsendiaProfileFields.Contents, contentsState, contents, labelContents);
            AddValueMapping(AsendiaProfile, AsendiaProfileFields.NonDelivery, nonDeliveryState, nonDelivery, labelNonDelivery);
        }

        /// <summary>
        /// Loads the Asendia accounts.
        /// </summary>
        private void LoadAsendiaAccounts()
        {
            asendiaAccount.DisplayMember = "Key";
            asendiaAccount.ValueMember = "Value";

            if (AsendiaAccountManager.Accounts.Count > 0)
            {
                asendiaAccount.DataSource = AsendiaAccountManager.Accounts.Select(a => new KeyValuePair<string, long>(a.Description, a.AsendiaAccountID)).ToList();
                asendiaAccount.Enabled = true;
            }
            else
            {
                asendiaAccount.DataSource = new List<KeyValuePair<string, long>> { new KeyValuePair<string, long>("(No accounts)", 0) };
                asendiaAccount.Enabled = false;
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
