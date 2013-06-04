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
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.EquaShip.Enums;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Carriers.EquaShip
{
    /// <summary>
    /// Control for configuring EquaShip profiles
    /// </summary>
    public partial class EquaShipProfileControl : ShippingProfileControlBase
    {
        public EquaShipProfileControl()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Load the given profile into the control
        /// </summary>
        public override void LoadProfile(ShippingProfileEntity profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }

            base.LoadProfile(profile);

            LoadEquashipAccounts();
            LoadOrigins();
            LoadConfirmationTypes();
            LoadPackagingTypes();
            LoadServiceTypes();

            EquaShipProfileEntity equaship = profile.EquaShip;

            // origin
            List<KeyValuePair<string, long>> origins = ShipmentTypeManager.GetType((ShipmentTypeCode) profile.ShipmentType).GetOrigins();
            originCombo.DisplayMember = "Key";
            originCombo.ValueMember = "Value";
            originCombo.DataSource = origins;

            EnumHelper.BindComboBox<EquaShipServiceType>(service);
            EnumHelper.BindComboBox<EquaShipPackageType>(packagingType);

            dimensionsControl.Initialize();

            AddValueMapping(equaship, EquaShipProfileFields.EquaShipAccountID, accountState, accountCombo, labelAccount);
            AddValueMapping(profile, ShippingProfileFields.OriginID, senderState, originCombo, labelSender);
            AddValueMapping(equaship, EquaShipProfileFields.Service, serviceState, service, labelService);
            AddValueMapping(equaship, EquaShipProfileFields.Confirmation, confirmationState, confirmation, labelConfirmation);
            AddValueMapping(equaship, EquaShipProfileFields.Weight, weightState, weight, labelWeight);
            AddValueMapping(equaship, EquaShipProfileFields.PackageType, packagingState, packagingType, labelPackaging);
            AddValueMapping(equaship, EquaShipProfileFields.DimsProfileID, dimensionsState, dimensionsControl, labelDimensions);
            AddValueMapping(equaship, EquaShipProfileFields.ReferenceNumber, stateReference, customerReference, labelReference);
            AddValueMapping(equaship, EquaShipProfileFields.Description, stateDescription, customerDescription, labelDescription);
            AddValueMapping(equaship, EquaShipProfileFields.ShippingNotes, stateShippingNotes, customerShippingNotes, labelShippingNotes);

            // Insurance
            AddValueMapping(profile, ShippingProfileFields.Insurance, insuranceState, insuranceControl);
        }

        /// <summary>
        /// Loads the list of EquaShip accounts
        /// </summary>
        private void LoadEquashipAccounts()
        {
            accountCombo.DisplayMember = "Key";
            accountCombo.ValueMember = "Value";

            if (EquaShipAccountManager.Accounts.Count > 0)
            {
                accountCombo.DataSource = EquaShipAccountManager.Accounts.Select(a => new KeyValuePair<string, long>(a.Description, a.EquaShipAccountID)).ToList();
                accountCombo.Enabled = true;
            }
            else
            {
                accountCombo.DataSource = new List<KeyValuePair<string, long>> { new KeyValuePair<string, long>("(No accounts)", 0) };
                accountCombo.Enabled = false;
            }
        }

        /// <summary>
        /// Load origin combobox
        /// </summary>
        private void LoadOrigins()
        {
            List<KeyValuePair<string, long>> origins = ShipmentTypeManager.GetType(ShipmentTypeCode.EquaShip).GetOrigins();

            originCombo.DisplayMember = "Key";
            originCombo.ValueMember = "Value";
            originCombo.DataSource = origins;
        }

        /// <summary>
        /// Loads the ServiceType combobox
        /// </summary>
        private void LoadServiceTypes()
        {
            EnumHelper.BindComboBox<EquaShipServiceType>(service);
        }

        /// <summary>
        /// Loads the PackagingType combobox
        /// </summary>
        private void LoadPackagingTypes()
        {
            EnumHelper.BindComboBox<EquaShipPackageType>(packagingType);
        }

        /// <summary>
        /// Loads the confirmation types
        /// </summary>
        private void LoadConfirmationTypes()
        {
            EnumHelper.BindComboBox<EquaShipConfirmationType>(confirmation);
        }
    }
}