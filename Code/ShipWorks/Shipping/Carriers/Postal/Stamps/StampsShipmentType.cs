using Interapptive.Shared.Business;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Properties;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

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
            return new StampsSettingsControl(false);
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
                    account = StampsAccountManager.StampsAccounts.FirstOrDefault();
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

            List<RateResult> express1Rates = null;
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            // See if this shipment should really go through Express1
            if(shipment.ShipmentType == (int)ShipmentTypeCode.Stamps &&
               settings.StampsAutomaticExpress1 &&
               Express1Utilities.IsValidPackagingType((PostalServiceType?)null, (PostalPackagingType)shipment.Postal.PackagingType))
            {
                var express1Account = StampsAccountManager.GetAccount(settings.StampsAutomaticExpress1Account);

                if(express1Account == null)
                {
                    throw new ShippingException("The Express1 account to automatically use when processing with Stamps.com has not been selected.");
                }

                // We temporarily turn this into an Exprss1 shipment to get rated
                shipment.ShipmentType = (int)ShipmentTypeCode.Express1Stamps;
                shipment.Postal.Stamps.OriginalStampsAccountID = shipment.Postal.Stamps.StampsAccountID;
                shipment.Postal.Stamps.StampsAccountID = express1Account.StampsAccountID;

                try
                {
                    // Currently this actually recurses into this same method
                    express1Rates = ShipmentTypeManager.GetType(shipment).GetRates(shipment).Rates.ToList();
                }
                finally
                {
                    shipment.ShipmentType = (int)ShipmentTypeCode.Stamps;
                    shipment.Postal.Stamps.StampsAccountID = shipment.Postal.Stamps.OriginalStampsAccountID.Value;
                    shipment.Postal.Stamps.OriginalStampsAccountID = null;
                }
            }

            try
            {
                List<RateResult> stampsRates = StampsApiSession.GetRates(shipment);

                // For Stamps, we want to either promote Express1 or show the Express1 savings
                if(shipment.ShipmentType == (int)ShipmentTypeCode.Stamps)
                {
                    List<RateResult> finalRates = new List<RateResult>();

                    bool hasExpress1Savings = false;

                    // Go through each Stamps rate
                    foreach(RateResult stampsRate in stampsRates)
                    {
                        PostalRateSelection stampsRateDetail = (PostalRateSelection)stampsRate.Tag;

                        // If it's a rate they could (or have) saved on with Express1, we modify it
                        if(stampsRate.Selectable && stampsRateDetail != null && Express1Utilities.IsPostageSavingService(stampsRateDetail.ServiceType))
                        {
                            // See if Express1 returned a rate for this servie
                            RateResult express1Rate = null;
                            if(express1Rates != null)
                            {
                                express1Rate = express1Rates.Where(e1r => e1r.Selectable).FirstOrDefault(e1r =>
                                    ((PostalRateSelection)e1r.Tag).ServiceType == stampsRateDetail.ServiceType && ((PostalRateSelection)e1r.Tag).ConfirmationType == stampsRateDetail.ConfirmationType);
                            }

                            // If Express1 returned a rate, check to make sure it is a lower amount
                            if(express1Rate != null && express1Rate.Amount <= stampsRate.Amount)
                            {
                                finalRates.Add(express1Rate);
                                hasExpress1Savings = true;
                            }
                            else
                            {
                                finalRates.Add(stampsRate);

                                // Set the express rate to null so that it doesn't add the icon later
                                express1Rate = null;
                            }

                            RateResult rate = finalRates[finalRates.Count - 1];

                            // If user wanted Express 1 rates
                            if(settings.StampsAutomaticExpress1)
                            {
                                // If they actually got the rate, show the check
                                if(express1Rate != null)
                                {
                                    rate.AmountFootnote = Resources.check2;
                                }
                            }
                            else
                            {
                                // Stamps rates only.  If it's not a valid Express1 packaging type, don't promote a savings
                                if(Express1Utilities.IsValidPackagingType(((PostalRateSelection)rate.Tag).ServiceType, (PostalPackagingType)shipment.Postal.PackagingType))
                                {
                                    rate.AmountFootnote = Resources.star_green;
                                }
                            }
                        }
                        else
                        {
                            finalRates.Add(stampsRate);
                        }
                    }

                    RateGroup finalGroup = new RateGroup(finalRates);

                    if(settings.StampsAutomaticExpress1)
                    {
                        if(hasExpress1Savings)
                        {
                            finalGroup.FootnoteCreator = () => new Express1RateDiscountedFootnote(stampsRates, express1Rates);
                        }
                        else
                        {
                            finalGroup.FootnoteCreator = () => new Express1RateNotQualifiedFootnote();
                        }
                    }
                    else
                    {
                        if(Express1Utilities.IsValidPackagingType(null, (PostalPackagingType)shipment.Postal.PackagingType))
                        {
                            finalGroup.FootnoteCreator = () => new Express1RatePromotionFootnote(new Express1StampsSettingsFacade(settings));
                        }
                    }

                    return finalGroup;
                }
                else
                {
                    // Express1 rates - return as-is
                    return new RateGroup(stampsRates);
                }
            }
            catch(StampsException ex)
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

            stamps.StampsAccountID = StampsAccountManager.StampsAccounts.Count > 0 ? StampsAccountManager.StampsAccounts[0].StampsAccountID : 0;
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
