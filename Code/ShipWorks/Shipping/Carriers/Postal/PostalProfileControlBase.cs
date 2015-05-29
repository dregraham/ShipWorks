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
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Insurance;
using ShipWorks.UI.Utility;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// UserControl base for editing postal profiles
    /// </summary>
    public partial class PostalProfileControlBase : ShippingProfileControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PostalProfileControlBase()
        {
            InitializeComponent();

            ResizeGroupBoxes(tabControl);
        }

        /// <summary>
        /// Load the UI for the given profile entity
        /// </summary>
        public override void LoadProfile(ShippingProfileEntity profile)
        {
            base.LoadProfile(profile);

            PostalProfileEntity postal = profile.Postal;

            List<KeyValuePair<string, long>> origins = ShipmentTypeManager.GetType((ShipmentTypeCode) profile.ShipmentType).GetOrigins();

            originCombo.DisplayMember = "Key";
            originCombo.ValueMember = "Value";
            originCombo.DataSource = origins;

            LoadServiceTypes();

            // Only Express 1 Endicia should see the cubic packaging type
            EnumHelper.BindComboBox<PostalPackagingType>(packagingType, p => (p != PostalPackagingType.Cubic || (ShipmentTypeCode)profile.ShipmentType == ShipmentTypeCode.Express1Endicia));
            EnumHelper.BindComboBox<PostalCustomsContentType>(contentType);

            LoadConfirmationTypes(profile);

            dimensionsControl.Initialize();

            AddValueMapping(profile, ShippingProfileFields.OriginID, senderState, originCombo, labelSender);
            AddValueMapping(postal, PostalProfileFields.Service, serviceState, service, labelService);

            AddValueMapping(postal, PostalProfileFields.Confirmation, confirmationState, confirmation, labelConfirmation);
            AddValueMapping(postal, PostalProfileFields.Weight, weightState, weight, labelWeight);

            AddValueMapping(postal, PostalProfileFields.PackagingType, packagingState, packagingType, labelPackaging);
            AddValueMapping(postal, PostalProfileFields.NonMachinable, machinableState, nonMachinable);
            AddValueMapping(postal, PostalProfileFields.NonRectangular, machinableState, nonRectangular);

            AddValueMapping(postal, PostalProfileFields.CustomsContentType, customsContentState, contentType);
            AddValueMapping(postal, PostalProfileFields.CustomsContentDescription, customsContentState, contentDescription);

            AddValueMapping(postal, PostalProfileFields.DimsProfileID, dimensionsState, dimensionsControl, labelDimensions);

            // Insurance
            AddValueMapping(profile, ShippingProfileFields.Insurance, insuranceState, insuranceControl);

            // Express Mail
            AddValueMapping(postal, PostalProfileFields.ExpressSignatureWaiver, expressSignatureRequirementState, expressSignatureRequirement);

            // Returns
            AddValueMapping(profile, ShippingProfileFields.ReturnShipment, returnState, returnShipment);

            groupReturns.Visible = ShipmentTypeManager.GetType((ShipmentTypeCode)profile.ShipmentType).SupportsReturns;
        }

        /// <summary>
        /// Load all the service types
        /// </summary>
        private void LoadServiceTypes()
        {
            service.DisplayMember = "Key";
            service.ValueMember = "Value";

            ShipmentTypeCode shipmentType = (ShipmentTypeCode) Profile.ShipmentType;

            service.DataSource = ActiveEnumerationBindingSource.Create<PostalServiceType>(PostalUtility.GetDomesticServices(shipmentType).Concat(PostalUtility.GetInternationalServices(shipmentType))
                .Select(s => new KeyValuePair<string, PostalServiceType>(PostalUtility.GetPostalServiceTypeDescription(s), s)).ToList());
        }

        /// <summary>
        /// Loads the confirmation types available to the shipment type.
        /// </summary>
        private void LoadConfirmationTypes(ShippingProfileEntity profile)
        {
            confirmation.DisplayMember = "Key";
            confirmation.ValueMember = "Value";

            PostalShipmentType postalType = ShipmentTypeManager.GetType((ShipmentTypeCode) profile.ShipmentType) as PostalShipmentType;

            confirmation.DataSource = postalType.GetAllConfirmationTypes()
                .Select(confirmationType => new KeyValuePair<string, PostalConfirmationType>(EnumHelper.GetDescription(confirmationType), confirmationType)).ToList();
        }

        /// <summary>
        /// The selected content type has changed
        /// </summary>
        private void OnContentTypeChanged(object sender, EventArgs e)
        {
            contentDescription.Visible = contentType.SelectedValue != null && (PostalCustomsContentType) contentType.SelectedValue == PostalCustomsContentType.Other;
        }
    }
}
