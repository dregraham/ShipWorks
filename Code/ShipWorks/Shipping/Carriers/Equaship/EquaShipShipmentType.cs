using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Filters.Content.Conditions.Shipments;
using ShipWorks.Shipping.Editing;
using ShipWorks.Data.Model.EntityClasses;
using System.Windows.Forms;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.Carriers.EquaShip.Enums;
using ShipWorks.Data.Model.HelperClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Settings;
using ShipWorks.Data;
using ShipWorks.Shipping.ShipSense.Packaging;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using System.Drawing.Imaging;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Tracking;
using Interapptive.Shared.Business;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Carriers.EquaShip
{
    /// <summary>
    /// "EquaShip" ShipmentType implementation
    /// </summary>
    public class EquaShipShipmentType : ShipmentType
    {
        /// <summary>
        /// Shipment Type Code
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get { return ShipmentTypeCode.EquaShip; }
        }

        /// <summary>
        /// Accounts can be the origin
        /// </summary>
        public override bool SupportsAccountAsOrigin
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// EquaShip has rate support
        /// </summary>
        public override bool SupportsGetRates
        {
            get { return true; }
        }

        /// <summary>
        /// Creates the wizard for configuring EquaShip 
        /// </summary>
        public override ShipmentTypeSetupWizardForm CreateSetupWizard()
        {
            return new EquashipSetupWizard();
        }

        /// <summary>
        /// Settings control
        /// </summary>
        public override SettingsControlBase CreateSettingsControl()
        {
            return new EquaShipSettingsControl();
        }

        /// <summary>
        /// Service Control
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected override ServiceControlBase InternalCreateServiceControl(RateControl rateControl)
        {
            return new EquaShipServiceControl(rateControl);
        }

        /// <summary>
        /// Create the control used to edit profile settings for EquaShip
        /// </summary>
        public override ShippingProfileControlBase CreateProfileControl()
        {
            return new EquaShipProfileControl();
        }

        /// <summary>
        /// Gets the package adapter for the shipment.
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(ShipmentEntity shipment)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ensure the carrier specific profile data is created and loaded for the given profile
        /// </summary>
        public override void LoadProfileData(ShippingProfileEntity profile, bool refreshIfPresent)
        {
            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }
            // Load the profile data
            ShipmentTypeDataService.LoadProfileData(profile, "EquaShip", typeof(EquaShipProfileEntity), refreshIfPresent);
        }

        /// <summary>
        /// Load the EquaShip specific shipment data
        /// </summary>
        public override void LoadShipmentData(ShipmentEntity shipment, bool refreshIfPresent)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            ShipmentTypeDataService.LoadShipmentData(this, shipment, shipment, "EquaShip", typeof(EquaShipShipmentEntity), refreshIfPresent);
        }

        /// <summary>
        /// Gets the description of an EquaShip service
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            return EnumHelper.GetDescription((EquaShipServiceType)shipment.EquaShip.Service);
        }

    
        /// <summary>
        /// Configures non-profiled properties of new shipments
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            base.ConfigureNewShipment(shipment);

            shipment.EquaShip.InsuranceValue = 0;
            shipment.EquaShip.RequestedLabelFormat = (int)ThermalLanguage.None;
        }

        /// <summary>
        /// Get the details of the equaship parcel
        /// </summary>
        public override ShipmentParcel GetParcelDetail(ShipmentEntity shipment, int parcelIndex)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            return new ShipmentParcel(shipment, null,
                new InsuranceChoice(shipment, shipment, shipment.EquaShip, null),
                new DimensionsAdapter(shipment.EquaShip));
        }

        /// <summary>
        /// Configures defaults
        /// </summary>
        protected override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }

            base.ConfigurePrimaryProfile(profile);

            long originID = ShippingOriginManager.Origins.Count > 0 ? ShippingOriginManager.Origins[0].ShippingOriginID : (long)ShipmentOriginSource.Store;
            profile.OriginID = originID;

            EquaShipProfileEntity equaship = profile.EquaShip;

            long shipperID = EquaShipAccountManager.Accounts.Count > 0 ? EquaShipAccountManager.Accounts[0].EquaShipAccountID : (long)0;
            profile.EquaShip.EquaShipAccountID = shipperID;

            equaship.Service = (int)EquaShipServiceType.Ground;
            equaship.PackageType = (int)EquaShipPackageType.Box;
            equaship.ReferenceNumber = "";
            equaship.Description = "";
            equaship.ShippingNotes = "";
            equaship.DimsProfileID = 0;
            equaship.Weight = 0;
            equaship.DimsLength = 0;
            equaship.DimsWidth = 0;
            equaship.DimsHeight = 0;
            equaship.DimsWeight = 0;
            equaship.DimsAddWeight = true;
            equaship.DeclaredValue = 0;
            equaship.EmailNotification = false;
            equaship.SaturdayDelivery = false;
            equaship.Confirmation = (int)EquaShipConfirmationType.None;
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(ShipmentEntity shipment, ShippingProfileEntity profile)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }

            base.ApplyProfile(shipment, profile);

            EquaShipShipmentEntity eqShipment = shipment.EquaShip;
            EquaShipProfileEntity eqProfile = profile.EquaShip;

            // Special case - only apply if the weight is not zero.  This prevents the weight entry from the default profile from overwriting the prefilled weight from products.
            if (eqProfile.Weight != null && eqProfile.Weight.Value != 0)
            {
                ShippingProfileUtility.ApplyProfileValue(eqProfile.Weight, shipment, ShipmentFields.ContentWeight);
            }

            long? accountID = (eqProfile.EquaShipAccountID == 0 && EquaShipAccountManager.Accounts.Count > 0) ? 
                (long?) EquaShipAccountManager.Accounts[0].EquaShipAccountID :
                eqProfile.EquaShipAccountID;

            ShippingProfileUtility.ApplyProfileValue(accountID, eqShipment, EquaShipShipmentFields.EquaShipAccountID);
            ShippingProfileUtility.ApplyProfileValue(eqProfile.Service, eqShipment, EquaShipShipmentFields.Service);
            ShippingProfileUtility.ApplyProfileValue(eqProfile.PackageType, eqShipment, EquaShipShipmentFields.PackageType);
            ShippingProfileUtility.ApplyProfileValue(eqProfile.ReferenceNumber, eqShipment, EquaShipShipmentFields.ReferenceNumber);
            ShippingProfileUtility.ApplyProfileValue(eqProfile.Description, eqShipment, EquaShipShipmentFields.Description);
            ShippingProfileUtility.ApplyProfileValue(eqProfile.ShippingNotes, eqShipment, EquaShipShipmentFields.ShippingNotes);

            ShippingProfileUtility.ApplyProfileValue(eqProfile.DimsProfileID, eqShipment, EquaShipShipmentFields.DimsProfileID);
            if (eqProfile.DimsProfileID != null)
            {
                ShippingProfileUtility.ApplyProfileValue(eqProfile.DimsLength, eqShipment, EquaShipShipmentFields.DimsLength);
                ShippingProfileUtility.ApplyProfileValue(eqProfile.DimsWidth, eqShipment, EquaShipShipmentFields.DimsWidth);
                ShippingProfileUtility.ApplyProfileValue(eqProfile.DimsHeight, eqShipment, EquaShipShipmentFields.DimsHeight);
                ShippingProfileUtility.ApplyProfileValue(eqProfile.DimsWeight, eqShipment, EquaShipShipmentFields.DimsWeight);
                ShippingProfileUtility.ApplyProfileValue(eqProfile.DimsAddWeight, eqShipment, EquaShipShipmentFields.DimsAddWeight);
            }

            ShippingProfileUtility.ApplyProfileValue(eqProfile.DeclaredValue, eqShipment, EquaShipShipmentFields.DeclaredValue);
            ShippingProfileUtility.ApplyProfileValue(eqProfile.EmailNotification, eqShipment, EquaShipShipmentFields.EmailNotification);
            ShippingProfileUtility.ApplyProfileValue(eqProfile.SaturdayDelivery, eqShipment, EquaShipShipmentFields.SaturdayDelivery);
            ShippingProfileUtility.ApplyProfileValue(eqProfile.Confirmation, eqShipment, EquaShipShipmentFields.Confirmation);

            UpdateDynamicShipmentData(shipment);

            UpdateTotalWeight(shipment);
        }

        /// <summary>
        /// Update the origin address based on the given originID value.  If the shipment has already been processed, nothing is done.  If
        /// the originID is no longer valid and the address could not be updated, false is returned.
        /// </summary>
        public override bool UpdatePersonAddress(ShipmentEntity shipment, PersonAdapter person, long originID)
        {
            if (shipment.Processed)
            {
                return true;
            }

            if (originID == (int) ShipmentOriginSource.Account)
            {
                EquaShipAccountEntity account = EquaShipAccountManager.GetAccount(shipment.EquaShip.EquaShipAccountID);
                if (account == null)
                {
                    account = EquaShipAccountManager.Accounts.FirstOrDefault();
                }

                if (account != null)
                {
                    PersonAdapter.Copy(account, "", person);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return base.UpdatePersonAddress(shipment, person, originID);
        }

        /// <summary>
        /// Update the dynamic data of the shipoment
        /// </summary>
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            base.UpdateDynamicShipmentData(shipment);

            // ensure international/domestic services

            // update the dimensions information
            DimensionsManager.UpdateDimensions(new DimensionsAdapter(shipment.EquaShip));

            shipment.RequestedLabelFormat = shipment.EquaShip.RequestedLabelFormat;
        }

        /// <summary>
        /// Processes a shipment void
        /// </summary>
        public override void VoidShipment(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            try
            {
                EquaShipClient.VoidShipment(shipment);
            }
            catch (EquaShipException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Update the total weight of the shipment
        /// </summary>
        public override void UpdateTotalWeight(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            shipment.TotalWeight = shipment.ContentWeight;

            if (shipment.EquaShip.DimsAddWeight)
            {
                shipment.TotalWeight += shipment.EquaShip.DimsWeight;
            }
        }

        /// <summary>
        /// Gets the processing synchronizer to be used during the PreProcessing of a shipment.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Equaship is no longer available.</exception>
        public override IShipmentProcessingSynchronizer GetProcessingSynchronizer()
        {
            throw new InvalidOperationException("Equaship is no longer an available shipment type.");
        }

        /// <summary>
        /// Process a shipment
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            try
            {
                EquaShipClient.ProcessShipment(shipment);
            }
            catch (EquaShipException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Track a shipment
        /// </summary>
        public override TrackingResult TrackShipment(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            try
            {
                return EquaShipClient.TrackShipment(shipment.TrackingNumber);
            }
            catch (EquaShipException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Retrieves the EquaShip shipping rates for a shipment
        /// </summary>
        public override RateGroup GetRates(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            List<RateResult> rates = new List<RateResult>();

            try
            {
                List<EquaShipServiceRate> equashipRates = EquaShipClient.GetRates(shipment);

                rates.AddRange(equashipRates.Select(e => new RateResult
                    (EnumHelper.GetDescription(e.Service),
                    ((int)(e.EstimatedDelivery.Date - DateTime.Now.Date).TotalDays).ToString(),
                    Convert.ToDecimal(e.Rate),
                    e.Service)));

                return new RateGroup(rates);
            }
            catch (EquaShipException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Generate the carrier specific template xml
        /// </summary>
        public override void GenerateTemplateElements(ElementOutline container, Func<ShipmentEntity> shipment, Func<ShipmentEntity> loaded)
        {
            var labels = new Lazy<List<TemplateLabelData>>(() => LoadLabelData(shipment));

            // Add the labels content
            container.AddElement("Labels",
                new LabelsOutline(container.Context, shipment, labels, ImageFormat.Png),
                ElementOutline.If(() => shipment().Processed));
        }

        /// <summary>
        /// Load all the label data for the given shipmentID
        /// </summary>
        private static List<TemplateLabelData> LoadLabelData(Func<ShipmentEntity> shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            List<TemplateLabelData> labelData = new List<TemplateLabelData>();

            // Get the resource list for our shipment
            List<DataResourceReference> resources = DataResourceManager.GetConsumerResourceReferences(shipment().ShipmentID);

            // Could be none for upgraded 2x shipments
            if (resources.Count > 0)
            {
                // Add our standard label output
                DataResourceReference labelResource = resources.Single(i => i.Label == "LabelPrimary");
                labelData.Add(new TemplateLabelData(null, "Label", TemplateLabelCategory.Primary, labelResource));

                // Add all label parts
                var labelResources = resources.Where(r => r.Label.StartsWith("LabelPart"));
                foreach (DataResourceReference documentResource in labelResources)
                {
                    labelData.Add(new TemplateLabelData(null, documentResource.Label, TemplateLabelCategory.Supplemental, documentResource));
                }

                // Supporting documents
                var customsResources = resources.Where(r => r.Label.StartsWith("Customs"));
                foreach (DataResourceReference customsResource in customsResources)
                {
                    labelData.Add(new TemplateLabelData(null, customsResource.Label, TemplateLabelCategory.Supplemental, customsResource));
                }
            }

            return labelData;
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for EquaShip based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of a NullShippingBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            return new NullShippingBroker();
        }

        /// <summary>
        /// Saves the requested label format to the child shipment
        /// </summary>
        public override void SaveRequestedLabelFormat(ThermalLanguage requestedLabelFormat, ShipmentEntity shipment)
        {
            if (shipment.EquaShip != null)
            {
                shipment.EquaShip.RequestedLabelFormat = (int)requestedLabelFormat;
            }
        }
    }
}