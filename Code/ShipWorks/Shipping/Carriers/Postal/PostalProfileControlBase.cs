using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Profiles;
using ShipWorks.UI.Utility;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// UserControl base for editing postal profiles
    /// </summary>
    [KeyedComponent(typeof(ShippingProfileControlBase), ShipmentTypeCode.PostalWebTools)]
    public partial class PostalProfileControlBase : ShippingProfileControlBase
    {
        private BindingList<KeyValuePair<long, string>> returnProfileList = new BindingList<KeyValuePair<long, string>>();
        private BindingSource bindingSource = new BindingSource();

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
            PackageProfileEntity packageProfile = profile.Packages.Single();

            List<KeyValuePair<string, long>> origins = ShipmentTypeManager.GetType((ShipmentTypeCode) profile.ShipmentType).GetOrigins();

            originCombo.DisplayMember = "Key";
            originCombo.ValueMember = "Value";
            originCombo.DataSource = origins;

            LoadServiceTypes();

            // Only Express 1 Endicia should see the cubic packaging type
            EnumHelper.BindComboBox<PostalPackagingType>(packagingType, p => (p != PostalPackagingType.Cubic || (ShipmentTypeCode) profile.ShipmentType == ShipmentTypeCode.Express1Endicia));
            EnumHelper.BindComboBox<PostalCustomsContentType>(contentType);

            LoadConfirmationTypes(profile);

            dimensionsControl.Initialize();

            AddValueMapping(profile, ShippingProfileFields.OriginID, senderState, originCombo, labelSender);
            AddValueMapping(postal, PostalProfileFields.Service, serviceState, service, labelService);

            AddValueMapping(postal, PostalProfileFields.Confirmation, confirmationState, confirmation, labelConfirmation);
            AddValueMapping(packageProfile, PackageProfileFields.Weight, weightState, weight, labelWeight);

            AddValueMapping(postal, PostalProfileFields.PackagingType, packagingState, packagingType, labelPackaging);
            AddValueMapping(postal, PostalProfileFields.NonMachinable, machinableState, nonMachinable);
            AddValueMapping(postal, PostalProfileFields.NonRectangular, machinableState, nonRectangular);

            AddValueMapping(postal, PostalProfileFields.CustomsContentType, customsContentState, contentType);
            AddValueMapping(postal, PostalProfileFields.CustomsContentDescription, customsContentState, contentDescription);

            AddValueMapping(packageProfile, PackageProfileFields.DimsProfileID, dimensionsState, dimensionsControl, labelDimensions);

            // Insurance
            AddValueMapping(profile, ShippingProfileFields.Insurance, insuranceState, insuranceControl);

            // Express Mail
            AddValueMapping(postal, PostalProfileFields.ExpressSignatureWaiver, expressSignatureRequirementState, expressSignatureRequirement);

            // Returns
            RefreshProfileMenu();
            returnProfileID.DisplayMember = "Value";
            returnProfileID.ValueMember = "Key";

            AddValueMapping(profile, ShippingProfileFields.ReturnShipment, returnState, returnShipment);
            AddValueMapping(profile, ShippingProfileFields.IncludeReturn, includeReturnState, includeReturn);
            AddValueMapping(profile, ShippingProfileFields.ApplyReturnProfile, applyReturnProfileState, applyReturnProfile);
            AddValueMapping(profile, ShippingProfileFields.ReturnProfileID, applyReturnProfileState, returnProfileID);

            // Remove state checkbox event handler since we'll be enabling/disabling manually
            returnState.CheckedChanged -= new EventHandler(OnStateCheckChanged);
            includeReturnState.CheckedChanged -= new EventHandler(OnStateCheckChanged);

            // Remove this event handler twice since it's added twice by AddValueMapping above
            applyReturnProfileState.CheckedChanged -= new EventHandler(OnStateCheckChanged);
            applyReturnProfileState.CheckedChanged -= new EventHandler(OnStateCheckChanged);

            // Manually enable/disable for mutually exclusive return controls
            includeReturn.Enabled = includeReturnState.Checked && !returnShipment.Checked;
            returnShipment.Enabled = returnState.Checked && !includeReturn.Checked;
            applyReturnProfile.Enabled = applyReturnProfileState.Checked && includeReturn.Checked;
            returnProfileID.Enabled = applyReturnProfileState.Checked && applyReturnProfile.Checked && includeReturn.Checked;
            returnProfileIDLabel.Enabled = applyReturnProfileState.Checked && applyReturnProfile.Checked && includeReturn.Checked;

            groupReturns.Visible = ShipmentTypeManager.GetType((ShipmentTypeCode) profile.ShipmentType).SupportsReturns;
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

        /// <summary>
        /// Click of the Include Return checkbox
        /// </summary>
        private void OnIncludeReturnChanged(object sender, EventArgs e)
        {
            if (includeReturn.Checked)
            {
                returnShipment.Enabled = false;
                applyReturnProfile.Enabled = applyReturnProfileState.Checked;
                returnProfileID.Enabled = applyReturnProfileState.Checked && applyReturnProfile.Checked;
                returnProfileIDLabel.Enabled = applyReturnProfileState.Checked && applyReturnProfile.Checked;
            }
            else
            {
                returnShipment.Enabled = returnState.Checked;
                applyReturnProfile.Enabled = false;
                returnProfileID.Enabled = false;
                returnProfileIDLabel.Enabled = false;
            }
        }

        /// <summary>
        /// Click of the Apply Return Profile checkbox
        /// </summary>
        private void OnApplyReturnChanged(object sender, EventArgs e)
        {
            if (applyReturnProfile.Checked)
            {
                returnProfileID.Enabled = applyReturnProfileState.Checked;
                returnProfileIDLabel.Enabled = applyReturnProfileState.Checked;
            }
            else
            {
                returnProfileID.Enabled = false;
                returnProfileIDLabel.Enabled = false;
            }
        }

        /// <summary>
        /// Opening the return profiles menu
        /// </summary>
        private void OnReturnProfileIDOpened(object sender, EventArgs e)
        {
            // Populate the list of profiles
            RefreshProfileMenu();
        }

        /// <summary>
        /// Click of the Return Shipment checkbox
        /// </summary>
        protected virtual void OnReturnShipmentChanged(object sender, EventArgs e)
        {
            if (returnShipment.Checked)
            {
                includeReturn.Enabled = false;
            }
            else
            {
                includeReturn.Enabled = includeReturnState.Checked;
            }
        }

        /// <summary>
        /// Click of the Include Return State Checkbox
        /// </summary>
        protected virtual void OnIncludeReturnStateChanged(object sender, EventArgs e)
        {
            if (includeReturnState.Checked)
            {
                includeReturn.Enabled = !returnShipment.Checked;
                applyReturnProfile.Enabled = includeReturn.Checked && applyReturnProfileState.Checked;
                returnProfileID.Enabled = applyReturnProfile.Checked && applyReturnProfileState.Checked;
                returnProfileIDLabel.Enabled = applyReturnProfile.Checked && applyReturnProfileState.Checked;
            }
            else
            {
                includeReturn.Enabled = includeReturn.Checked = false;
                applyReturnProfile.Enabled = applyReturnProfile.Checked = false;
                returnProfileID.Enabled = returnProfileIDLabel.Enabled = false;
            }
        }

        /// <summary>
        /// Click of the Apply Return Profile State Checkbox
        /// </summary>
        protected virtual void OnApplyReturnProfileStateChanged(object sender, EventArgs e)
        {
            if (applyReturnProfileState.Checked)
            {
                applyReturnProfile.Enabled = includeReturn.Checked;
                returnProfileID.Enabled = applyReturnProfile.Checked;
                returnProfileIDLabel.Enabled = applyReturnProfile.Checked;
            }
            else
            {
                applyReturnProfile.Enabled = applyReturnProfile.Checked = false;
                returnProfileID.Enabled = returnProfileIDLabel.Enabled = false;
            }
        }

        /// <summary>
        /// Click of the Return Shipment State Checkbox
        /// </summary>
        protected virtual void OnReturnStateChanged(object sender, EventArgs e)
        {
            if (returnState.Checked)
            {
                returnShipment.Enabled = !includeReturn.Checked;
            }
            else
            {
                returnShipment.Enabled = returnShipment.Checked = false;
            }

        }

        /// <summary>
        /// Add applicable profiles for the given shipment type to the context menu
        /// </summary>
        private void RefreshProfileMenu()
        {
            BindingList<KeyValuePair<long, string>> newReturnProfileList = new BindingList<KeyValuePair<long, string>>();

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IShippingProfileService shippingProfileService = lifetimeScope.Resolve<IShippingProfileService>();

                IEnumerable<IShippingProfileEntity> returnProfiles = shippingProfileService
                .GetConfiguredShipmentTypeProfiles()
                .Where(p => p.ShippingProfileEntity.ShipmentType.HasValue)
                .Where(p => p.IsApplicable(ShipmentTypeCode.Endicia))
                .Where(p => p.ShippingProfileEntity.ReturnShipment == true)
                .Select(s => s.ShippingProfileEntity).Cast<IShippingProfileEntity>()
                .OrderBy(g => g.Name);

                foreach (IShippingProfileEntity profile in returnProfiles)
                {
                    newReturnProfileList.Add(new KeyValuePair<long, string>(profile.ShippingProfileID, profile.Name));
                }
                if (newReturnProfileList.Count == 0)
                {
                    newReturnProfileList.Add(new KeyValuePair<long, string>(-1, "(No Profile)"));
                }
            }
            returnProfileList = newReturnProfileList;

            // Reset data sources because calling resetbindings() doesn't work
            bindingSource.DataSource = returnProfileList;
            returnProfileID.DataSource = bindingSource;
        }
    }
}
