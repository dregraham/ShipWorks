using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.Templates.Processing.TemplateXml;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;
using System.Diagnostics;
using System.Windows.Forms;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using System.Drawing.Imaging;
using ShipWorks.Data;
using Interapptive.Shared.Business;
using ShipWorks.Shipping.Settings.Origin;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    public class StampsShipmentType : PostalShipmentType
    {
        /// <summary>
        /// The ShipmentTypeCode enumeration value
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get { return ShipmentTypeCode.Stamps; }
        }

        /// <summary>
        /// Create the Form used to do the setup for the Stamps.com API
        /// </summary>
        public override Form CreateSetupWizard()
        {
            return new StampsSetupWizard();
        }

        /// <summary>
        /// Create the UserControl used to handle Stamps.com shipments
        /// </summary>
        public override ServiceControlBase CreateServiceControl()
        {
            return new StampsServiceControl();
        }

        /// <summary>
        /// Create the UserControl used to handle Stamps.com profiles
        /// </summary>
        public override ShippingProfileControlBase CreateProfileControl()
        {
            return new StampsProfileControl();
        }

        /// <summary>
        /// Create the settings control for stamps.com
        /// </summary>
        public override SettingsControlBase CreateSettingsControl()
        {
            return new StampsSettingsControl();
        }

        /// <summary>
        /// Stamps.com supports getting postal service rates
        /// </summary>
        public override bool SupportsGetRates
        {
            get { return true; }
        }

        /// <summary>
        /// Indicates if the shipment type supports accounts as the origin
        /// </summary>
        public override bool SupportsAccountAsOrigin
        {
            get { return true; }
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

            // The Stamps object may not yet be set if we are in the middle of creating a new shipment
            if (originID == (int) ShipmentOriginSource.Account && shipment.Postal.Stamps != null)
            {
                StampsAccountEntity account = StampsAccountManager.GetAccount(shipment.Postal.Stamps.StampsAccountID);
                if (account == null)
                {
                    account = StampsAccountManager.Accounts.FirstOrDefault();
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
        /// Get the rates for the given shipment.
        /// </summary>
        public override RateGroup GetRates(ShipmentEntity shipment)
        {
            ValidateShipment(shipment);

            try
            {
                List<RateResult> rates = new List<RateResult>();

                foreach (RateV11 stampsRate in StampsApiSession.GetRates(shipment))
                {
                    PostalServiceType serviceType = StampsUtility.GetPostalServiceType(stampsRate.ServiceType);

                    RateResult baseRate = null;
                    
                    // If its a rate that has sig\deliv, then you can's select the core rate itself
                    if (stampsRate.AddOns.Any(a => a.AddOnType == AddOnTypeV4.USADC))
                    {
                        baseRate = new RateResult(
                            PostalUtility.GetPostalServiceTypeDescription(serviceType),
                            stampsRate.DeliverDays.Replace("Days", ""));
                    }
                    else
                    {
                        baseRate = new RateResult(
                             PostalUtility.GetPostalServiceTypeDescription(serviceType),
                             stampsRate.DeliverDays.Replace("Days", ""),
                             stampsRate.Amount,
                             new PostalRateSelection(serviceType, PostalConfirmationType.None));
                    }

                    rates.Add(baseRate);

                    // Add a rate for each add-on
                    foreach (AddOnV4 addOn in stampsRate.AddOns)
                    {
                        string name = null;
                        PostalConfirmationType confirmationType = PostalConfirmationType.None;

                        switch (addOn.AddOnType)
                        {
                            case AddOnTypeV4.USADC:
                                name = string.Format("       Delivery Confirmation ({0:c})", addOn.Amount);
                                confirmationType = PostalConfirmationType.Delivery;
                                break;

                            case AddOnTypeV4.USASC:
                                name = string.Format("       Signature Confirmation ({0:c})", addOn.Amount);
                                confirmationType = PostalConfirmationType.Signature;
                                break;
                        }

                        if (name != null)
                        {
                            RateResult addOnRate = new RateResult(
                                name,
                                string.Empty,
                                stampsRate.Amount + addOn.Amount,
                                new PostalRateSelection(serviceType, confirmationType));

                            rates.Add(addOnRate);
                        }
                    }
                }

                return new RateGroup(rates);

            }
            catch (StampsException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Process the shipment
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            ValidateShipment(shipment);

            if (shipment.Postal.Service == (int) PostalServiceType.ExpressMail && shipment.Postal.Confirmation != (int) PostalConfirmationType.None)
            {
                throw new ShippingException("A confirmation option cannot be used with Express mail.");
            }

            try
            {
                StampsApiSession.ProcessShipment(shipment);
            }
            catch (StampsException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Validate the shipment before processing or rating
        /// </summary>
        private void ValidateShipment(ShipmentEntity shipment)
        {
            if (shipment.TotalWeight == 0)
            {
                throw new ShippingException("The shipment weight cannot be zero.");
            }
        }

        /// <summary>
        /// Void the shipment
        /// </summary>
        public override void VoidShipment(ShipmentEntity shipment)
        {
            try
            {
                StampsApiSession.VoidShipment(shipment);
            }
            catch (StampsException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get the USPS shipment details
        /// </summary>
        public override ShipmentCommonDetail GetShipmentCommonDetail(ShipmentEntity shipment)
        {
            StampsAccountEntity account = StampsAccountManager.GetAccount(shipment.Postal.Stamps.StampsAccountID);

            ShipmentCommonDetail commonDetail = base.GetShipmentCommonDetail(shipment);
            commonDetail.OriginAccount = (account == null) ? "" : account.Username;

            return commonDetail;
        }

        /// <summary>
        /// Ensures that the USPS specific data for the shipment is loaded.  If the data already exists, nothing is done.  It is not refreshed.
        /// </summary>
        public override void LoadShipmentData(ShipmentEntity shipment, bool refreshIfPresent)
        {
            base.LoadShipmentData(shipment, refreshIfPresent);

            ShipmentTypeDataService.LoadShipmentData(this, shipment, shipment.Postal, "Stamps", typeof(StampsShipmentEntity), refreshIfPresent);
        }
        
        /// <summary>
        /// Configure the properties of a newly created shipment
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
 	        base.ConfigureNewShipment(shipment);

            // We can be called during the creation of the base Postal shipment, before the Stamps one exists
            if (shipment.Postal.Stamps != null)
            {
                // Use the empty guids for now - they'll get set properly during processing
                shipment.Postal.Stamps.IntegratorTransactionID = Guid.Empty;
                shipment.Postal.Stamps.StampsTransactionID = Guid.Empty;
            }
        }

        /// <summary>
        /// Ensure the carrier specific profile data is created and loaded for the given profile
        /// </summary>
        public override void LoadProfileData(ShippingProfileEntity profile, bool refreshIfPresent)
        {
            base.LoadProfileData(profile, refreshIfPresent);

            ShipmentTypeDataService.LoadProfileData(profile.Postal, "Stamps", typeof(StampsProfileEntity), refreshIfPresent);
        }

        /// <summary>
        /// Get the default profile for the shipment type
        /// </summary>
        protected override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            base.ConfigurePrimaryProfile(profile);

            StampsProfileEntity stamps = profile.Postal.Stamps;

            stamps.StampsAccountID = StampsAccountManager.Accounts.Count > 0 ? StampsAccountManager.Accounts[0].StampsAccountID : 0;
            stamps.RequireFullAddressValidation = true;
            stamps.HidePostage = false;
            stamps.Memo = string.Empty;
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(ShipmentEntity shipment, ShippingProfileEntity profile)
        {
            base.ApplyProfile(shipment, profile);

            // We can be called during the creation of the base Postal shipment, before the Stamps one exists
            if (shipment.Postal.Stamps != null)
            {
                StampsShipmentEntity stampsShipment = shipment.Postal.Stamps;
                StampsProfileEntity stampsProfile = profile.Postal.Stamps;

                ShippingProfileUtility.ApplyProfileValue(stampsProfile.StampsAccountID, stampsShipment, StampsShipmentFields.StampsAccountID);
                ShippingProfileUtility.ApplyProfileValue(stampsProfile.RequireFullAddressValidation, stampsShipment, StampsShipmentFields.RequireFullAddressValidation);
                ShippingProfileUtility.ApplyProfileValue(stampsProfile.HidePostage, stampsShipment, StampsShipmentFields.HidePostage);
                ShippingProfileUtility.ApplyProfileValue(stampsProfile.Memo, stampsShipment, StampsShipmentFields.Memo);
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
        /// Determines if delivery\signature confirmation is available for the given service
        /// </summary>
        public override List<PostalConfirmationType> GetAvailableConfirmationTypes(string countryCode, PostalServiceType service, PostalPackagingType? packaging)
        {
            // If we don't know the packaging or country, it doesn't matter
            if (!string.IsNullOrWhiteSpace(countryCode) && packaging != null)
            {
                if (PostalUtility.IsFreeInternationalDeliveryConfirmation(countryCode, service, packaging.Value))
                {
                    return new List<PostalConfirmationType> { PostalConfirmationType.Delivery };
                }
            }

            if (service == PostalServiceType.PriorityMail && packaging == PostalPackagingType.Envelope)
            {
                return new List<PostalConfirmationType> { PostalConfirmationType.None };
            }

            return base.GetAvailableConfirmationTypes(countryCode, service, packaging);
        }

        /// <summary>
        /// Load all the label data for the given shipmentID
        /// </summary>
        private static List<TemplateLabelData> LoadLabelData(Func<ShipmentEntity> shipment)
        {
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
                foreach (DataResourceReference otherLabel in labelResources)
                {
                    labelData.Add(new TemplateLabelData(null, otherLabel.Label, TemplateLabelCategory.Supplemental, otherLabel));
                }
            }

            return labelData;
        }
    }
}
