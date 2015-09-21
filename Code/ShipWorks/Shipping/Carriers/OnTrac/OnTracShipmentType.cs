using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Conditions.Shipments;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.OnTrac.BestRate;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Net.Rates;
using ShipWorks.Shipping.Carriers.OnTrac.Net.Shipment;
using ShipWorks.Shipping.Carriers.OnTrac.Net.Track;
using ShipWorks.Shipping.Carriers.OnTrac.Schemas.Shipment;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.ShipSense.Packaging;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Stores.Platforms.Amazon.WebServices.Associates;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// OnTrac Shipment Type
    /// </summary>
    public class OnTracShipmentType : ShipmentType
    {
        /// <summary>
        /// Returns OnTrac ShipmentTypeCode
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get
            {
                return ShipmentTypeCode.OnTrac;
            }
        }

        /// <summary>
        /// OnTrac accounts have an address that can be used as the shipment origin
        /// </summary>
        public override bool SupportsAccountAsOrigin
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// OnTrac supports rates
        /// </summary>
        public override bool SupportsGetRates
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this shipment type has accounts
        /// </summary>
        public override bool HasAccounts
        {
            get
            {
                return OnTracAccountManager.Accounts.Any();
            }
        }

        /// <summary>
        /// Returns new OnTrac Service Control
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected override ServiceControlBase InternalCreateServiceControl(RateControl rateControl)
        {
            return new OnTracServiceControl(rateControl);
        }

        /// <summary>
        /// Create the UserControl used to edit OnTrac profiles.
        /// </summary>
        public override ShippingProfileControlBase CreateProfileControl()
        {
            return new OnTracProfileControl();
        }

        /// <summary>
        /// Gets the package adapter for the shipment.
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(ShipmentEntity shipment)
        {
            if (shipment.OnTrac == null)
            {
                ShippingManager.EnsureShipmentLoaded(shipment);
            }

            return new List<IPackageAdapter>()
            {
                new OnTracPackageAdapter(shipment)
            };
        }

        /// <summary>
        /// Gets the service types that are available for this shipment type (i.e have not
        /// been excluded). The integer values are intended to correspond to the appropriate
        /// enumeration values of the specific shipment type (i.e. the integer values would
        /// correspond to PostalServiceType values for a UspsShipmentType).
        /// </summary>
        /// <param name="repository">The repository from which the service types are fetched.</param>
        public override IEnumerable<int> GetAvailableServiceTypes(IExcludedServiceTypeRepository repository)
        {
            IEnumerable<int> allServices = EnumHelper.GetEnumList<OnTracServiceType>().Select(x => x.Value).Cast<int>();
            return allServices.Except(GetExcludedServiceTypes(repository));
        }

        /// <summary>
        /// Gets the Package types that are available for this shipment type
        /// </summary>
        /// <param name="repository">The repository from which the Package types are fetched.</param>
        public override IEnumerable<int> GetAvailablePackageTypes(IExcludedPackageTypeRepository repository)
        {
            return EnumHelper.GetEnumList<OnTracPackagingType>()
                .Select(x => x.Value)
                .Cast<int>()
                .Except(GetExcludedPackageTypes(repository));
        }

        /// <summary>
        /// Create OnTrac specific information
        /// </summary>
        public override void LoadShipmentData(ShipmentEntity shipment, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadShipmentData(
                this, shipment, shipment, "OnTrac", typeof(OnTracShipmentEntity), refreshIfPresent);
        }

        /// <summary>
        /// Ensure the carrier specific profile data is created and loaded for the given profile
        /// </summary>
        public override void LoadProfileData(ShippingProfileEntity profile, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadProfileData(profile, "OnTrac", typeof(OnTracProfileEntity), refreshIfPresent);
        }

        /// <summary>
        /// Get shipment description
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment)
        {
            return EnumHelper.GetDescription((OnTracServiceType)shipment.OnTrac.Service);
        }

        /// <summary>
        /// Get all service types
        /// </summary>
        public static IEnumerable<OnTracServiceType> ServiceTypes
        {
            get
            {
                return EnumHelper.GetEnumList<OnTracServiceType>().Select(x => x.Value);
            }
        } 

        /// <summary>
        /// Get the OnTrac shipment details
        /// </summary>
        public override ShipmentCommonDetail GetShipmentCommonDetail(ShipmentEntity shipment)
        {
            ShipmentCommonDetail commonDetail = new ShipmentCommonDetail();

            OnTracShipmentEntity onTrac = shipment.OnTrac;
            OnTracAccountEntity account = OnTracAccountManager.GetAccount(onTrac.OnTracAccountID);

            commonDetail.OriginAccount = (account == null) ? "" : account.AccountNumber.ToString();
            commonDetail.ServiceType = onTrac.Service;

            commonDetail.PackagingType = onTrac.PackagingType;
            commonDetail.PackageLength = onTrac.DimsLength;
            commonDetail.PackageWidth = onTrac.DimsWidth;
            commonDetail.PackageHeight = onTrac.DimsHeight;

            return commonDetail;
        }

        /// <summary>
        /// Get the parcel data for the shipment
        /// </summary>
        public override ShipmentParcel GetParcelDetail(ShipmentEntity shipment, int parcelIndex)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            return new ShipmentParcel(shipment, null,
                new InsuranceChoice(shipment, shipment, shipment.OnTrac, shipment.OnTrac),
                new DimensionsAdapter(shipment.OnTrac));
        }

        /// <summary>
        /// Gets the processing synchronizer to be used during the PreProcessing of a shipment.
        /// </summary>
        protected override IShipmentProcessingSynchronizer GetProcessingSynchronizer()
        {
            return new OnTracShipmentProcessingSynchronizer();
        }

        /// <summary>
        /// Process the shipment
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            try
            {
                if (shipment == null)
                {
                    throw new ArgumentNullException("shipment");
                }

                OnTracAccountEntity account = GetAccountForShipment(shipment);

                OnTracShipmentRequest onTracShipmentRequest = new OnTracShipmentRequest(account);
                DatabaseOnTracShipmentRepository onTracShipmentRepository = new DatabaseOnTracShipmentRepository();

                // None is only an option if an invalid country is selected.
                if (((OnTracServiceType)shipment.OnTrac.Service) == OnTracServiceType.None)
                {
                    throw new OnTracException("OnTrac does not provide service outside of the United States.", true);
                }

                if (shipment.RequestedLabelFormat != (int) ThermalLanguage.None)
                {
                    shipment.ActualLabelFormat = shipment.RequestedLabelFormat;
                }
                else
                {
                    shipment.ActualLabelFormat = null;
                }

                // Transform shipment to OnTrac DTO
                ShipmentRequestList shipmentRequestList = OnTracDtoAdapter.CreateShipmentRequestList(
                    shipment,
                    account.AccountNumber);

                // Get new shipment from OnTrac and save the shipment info
                ShipmentResponse shipmentResponse = onTracShipmentRequest.ProcessShipment(shipmentRequestList);

                onTracShipmentRepository.SaveShipmentFromOnTrac(shipmentResponse, shipment);
            }
            catch (OnTracException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get new OnTracSetupWizard
        /// </summary>
        public override ShipmentTypeSetupWizardForm CreateSetupWizard()
        {
            return new OnTracSetupWizard();
        }

        /// <summary>
        /// Create and Initialize a new shipment
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            OnTracShipmentEntity onTracShipment = shipment.OnTrac;

            onTracShipment.DeclaredValue = 0;

            onTracShipment.IsCod = false;
            onTracShipment.CodType = 0;
            onTracShipment.CodAmount = 0;

            onTracShipment.PackagingType = (int)OnTracPackagingType.Package;

            onTracShipment.InsurancePennyOne = false;
            onTracShipment.InsuranceValue = 0;

            shipment.OnTrac.RequestedLabelFormat = (int)ThermalLanguage.None;

            base.ConfigureNewShipment(shipment);
        }

        /// <summary>
        /// Get the default profile for the shipment type
        /// </summary>
        protected override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            base.ConfigurePrimaryProfile(profile);
            profile.OriginID = (int) ShipmentOriginSource.Account;

            OnTracProfileEntity onTrac = profile.OnTrac;

            onTrac.OnTracAccountID = OnTracAccountManager.Accounts.Count > 0
                ? OnTracAccountManager.Accounts[0].OnTracAccountID
                : 0;

            onTrac.Service = (int) OnTracServiceType.Ground;
            onTrac.SaturdayDelivery = false;
            onTrac.SignatureRequired = false;

            onTrac.Weight = 0;
            onTrac.DimsProfileID = 0;
            onTrac.DimsLength = 0;
            onTrac.DimsWidth = 0;
            onTrac.DimsHeight = 0;
            onTrac.DimsWeight = 0;
            onTrac.DimsAddWeight = true;

            onTrac.PackagingType = (int)OnTracPackagingType.Package;
            onTrac.ResidentialDetermination = (int)ResidentialDeterminationType.FromAddressValidation;

            onTrac.Reference1 = string.Empty;
            onTrac.Reference2 = string.Empty;
            onTrac.Instructions = string.Empty;
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(ShipmentEntity shipment, ShippingProfileEntity profile)
        {
            base.ApplyProfile(shipment, profile);

            OnTracShipmentEntity onTracShipment = shipment.OnTrac;
            OnTracProfileEntity onTracProfile = profile.OnTrac;

            long? accountID = (onTracProfile.OnTracAccountID == 0 && OnTracAccountManager.Accounts.Count > 0)
                ? OnTracAccountManager.Accounts[0].OnTracAccountID
                : onTracProfile.OnTracAccountID;

            ShippingProfileUtility.ApplyProfileValue(accountID, onTracShipment, OnTracShipmentFields.OnTracAccountID);
            ShippingProfileUtility.ApplyProfileValue(onTracProfile.Service, onTracShipment, OnTracShipmentFields.Service);
            ShippingProfileUtility.ApplyProfileValue(onTracProfile.PackagingType, onTracShipment, OnTracShipmentFields.PackagingType);

            ShippingProfileUtility.ApplyProfileValue(onTracProfile.SaturdayDelivery, onTracShipment, OnTracShipmentFields.SaturdayDelivery);
            ShippingProfileUtility.ApplyProfileValue(onTracProfile.SignatureRequired, onTracShipment, OnTracShipmentFields.SignatureRequired);

            if (onTracProfile.Weight.HasValue && onTracProfile.Weight.Value != 0)
            {
                ShippingProfileUtility.ApplyProfileValue(onTracProfile.Weight, shipment, ShipmentFields.ContentWeight);
            }

            ShippingProfileUtility.ApplyProfileValue(onTracProfile.DimsProfileID, onTracShipment, OnTracShipmentFields.DimsProfileID);
            ShippingProfileUtility.ApplyProfileValue(onTracProfile.DimsWeight, onTracShipment, OnTracShipmentFields.DimsWeight);
            ShippingProfileUtility.ApplyProfileValue(onTracProfile.DimsLength, onTracShipment, OnTracShipmentFields.DimsLength);
            ShippingProfileUtility.ApplyProfileValue(onTracProfile.DimsHeight, onTracShipment, OnTracShipmentFields.DimsHeight);
            ShippingProfileUtility.ApplyProfileValue(onTracProfile.DimsWidth, onTracShipment, OnTracShipmentFields.DimsWidth);
            ShippingProfileUtility.ApplyProfileValue(onTracProfile.DimsAddWeight, onTracShipment, OnTracShipmentFields.DimsAddWeight);

            ShippingProfileUtility.ApplyProfileValue(onTracProfile.Reference1, onTracShipment, OnTracShipmentFields.Reference1);
            ShippingProfileUtility.ApplyProfileValue(onTracProfile.Reference2, onTracShipment, OnTracShipmentFields.Reference2);
            ShippingProfileUtility.ApplyProfileValue(onTracProfile.Instructions, onTracShipment, OnTracShipmentFields.Instructions);

            ShippingProfileUtility.ApplyProfileValue(onTracProfile.ResidentialDetermination, shipment, ShipmentFields.ResidentialDetermination);

            UpdateTotalWeight(shipment);

            UpdateDynamicShipmentData(shipment);
        }

        /// <summary>
        /// Gets rates
        /// </summary>
        public override RateGroup GetRates(ShipmentEntity shipment)
        {
            return GetCachedRates<OnTracException>(shipment, GetRatesFromApi);  
        }

        /// <summary>
        /// Gets rates from the OnTrac API
        /// </summary>
        private RateGroup GetRatesFromApi(ShipmentEntity shipment)
        {
            OnTracAccountEntity account = null;

            try
            {
                account = GetAccountForShipment(shipment);
            }
            catch (OnTracException ex)
            {
                if (ex.Message == "No OnTrac account is selected for the shipment.")
                {
                    // Provide a message with additional context
                    throw new OnTracException("An OnTrac account is required to view rates.", ex);
                }
                else
                {
                    throw;
                }
            }

            OnTracRates rateRequest = new OnTracRates(account);
            return rateRequest.GetRates(shipment, GetAvailableServiceTypes().Cast<OnTracServiceType>().Union(new List<OnTracServiceType> { (OnTracServiceType)shipment.OnTrac.Service }));
        }

        /// <summary>
        /// Update the dynamic shipment data that could have changed "outside" the known editor
        /// </summary>
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            shipment.InsuranceProvider = settings.OnTracInsuranceProvider;
            shipment.OnTrac.InsurancePennyOne = settings.OnTracInsurancePennyOne;

            // If they are using carrier insurance, send the actual value of the shipment as declared value
            if (shipment.Insurance && shipment.InsuranceProvider == (int) InsuranceProvider.Carrier)
            {
                shipment.OnTrac.DeclaredValue = shipment.OnTrac.InsuranceValue;
            }
            else
            {
                // otherwise send the 100 or the actual value, whichever is less.
                // We do this because the first 100 is free.
                shipment.OnTrac.DeclaredValue = Math.Min(100, shipment.OnTrac.InsuranceValue);
            }

            shipment.RequestedLabelFormat = shipment.OnTrac.RequestedLabelFormat;
        }

        /// <summary>
        /// Create the settings control for OnTrac
        /// </summary>
        public override SettingsControlBase CreateSettingsControl()
        {
            return new OnTracSettingsControl();
        }

        /// <summary>
        /// Generate the carrier specific template XML
        /// </summary>
        public override void GenerateTemplateElements(
            ElementOutline container, Func<ShipmentEntity> shipment, Func<ShipmentEntity> loaded)
        {
            Lazy<List<TemplateLabelData>> labels = new Lazy<List<TemplateLabelData>>(() => LoadLabelData(shipment));

            // Add the labels content
            container.AddElement(
                "Labels",
                new LabelsOutline(container.Context, shipment, labels, ImageFormat.Png),
                ElementOutline.If(() => shipment().Processed));
        }

        /// <summary>
        /// Load all the label data for the given shipmentID
        /// </summary>
        static List<TemplateLabelData> LoadLabelData(Func<ShipmentEntity> shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            List<TemplateLabelData> labelData = new List<TemplateLabelData>();

            // Get the resource list for our shipment
            List<DataResourceReference> resources =
                DataResourceManager.GetConsumerResourceReferences(shipment().ShipmentID);

            if (resources.Count > 0)
            {
                // Add our standard label output
                DataResourceReference labelResource = resources.Single(i => i.Label == "LabelPrimary");
                labelData.Add(new TemplateLabelData(null, "Label", TemplateLabelCategory.Primary, labelResource));
            }

            return labelData;
        }

        /// <summary>
        /// Track Shipment
        /// </summary>
        public override TrackingResult TrackShipment(ShipmentEntity shipment)
        {
            try
            {
                OnTracTrackedShipment shipmentRequest = new OnTracTrackedShipment(GetAccountForShipment(shipment), new HttpVariableRequestSubmitter());
                TrackingResult trackingResults = shipmentRequest.GetTrackingResults(shipment.TrackingNumber);

                return trackingResults;
            }
            catch (OnTracException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get the OnTrac account to be used for the given shipment
        /// </summary>
        private static OnTracAccountEntity GetAccountForShipment(ShipmentEntity shipment)
        {
            OnTracAccountEntity account = OnTracAccountManager.GetAccount(shipment.OnTrac.OnTracAccountID);
            if (account == null)
            {
                throw new OnTracException("No OnTrac account is selected for the shipment.");
            }

            return account;
        }

        /// <summary>
        /// OnTrac always uses the residential indicator
        /// </summary>
        public override bool IsResidentialStatusRequired(ShipmentEntity shipment)
        {
            return true;
        }

        /// <summary>
        /// Update the origin address based on the given originID value.  If the shipment has already been processed, nothing is done.  If
        /// the originID is no longer valid and the address could not be updated, false is returned.
        /// </summary>
        /// <returns>True if successful</returns>
        public override bool UpdatePersonAddress(ShipmentEntity shipment, PersonAdapter person, long originID)
        {
            bool isSuccessfull = false;

            if (shipment.Processed)
            {
                isSuccessfull = true;
            }
            else
            {
                // shipment not processed
                if (originID == (int)ShipmentOriginSource.Account && shipment.OnTrac != null)
                {
                    OnTracAccountEntity account = OnTracAccountManager.GetAccount(shipment.OnTrac.OnTracAccountID)
                        ?? OnTracAccountManager.Accounts.FirstOrDefault();

                    if (account != null)
                    {
                        PersonAdapter.Copy(account, "", person);
                        isSuccessfull = true;
                    }
                }
                else
                {
                    // origin not specified as an account. Use base behavior.
                    isSuccessfull = base.UpdatePersonAddress(shipment, person, originID);
                }
            }
            return isSuccessfull;
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

            if (shipment.OnTrac.DimsAddWeight)
            {
                shipment.TotalWeight += shipment.OnTrac.DimsWeight;
            }
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for the OnTrac shipment type based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of an OnTracBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            return new OnTracBestRateBroker();
        }

        /// <summary>
        /// Gets the fields used for rating a shipment.
        /// </summary>
        public override RatingFields RatingFields
        {
            get
            {
                if (ratingField != null)
                {
                    return ratingField;
                }

                ratingField = base.RatingFields;
                ratingField.ShipmentFields.Add(OnTracShipmentFields.OnTracAccountID);
                ratingField.ShipmentFields.Add(OnTracShipmentFields.CodAmount);
                ratingField.ShipmentFields.Add(OnTracShipmentFields.CodType);
                ratingField.ShipmentFields.Add(OnTracShipmentFields.SaturdayDelivery);
                ratingField.ShipmentFields.Add(OnTracShipmentFields.DeclaredValue);
                ratingField.ShipmentFields.Add(OnTracShipmentFields.PackagingType);
                ratingField.ShipmentFields.Add(OnTracShipmentFields.DimsAddWeight);
                ratingField.ShipmentFields.Add(OnTracShipmentFields.DimsHeight);
                ratingField.ShipmentFields.Add(OnTracShipmentFields.DimsLength);
                ratingField.ShipmentFields.Add(OnTracShipmentFields.DimsWidth);
                ratingField.ShipmentFields.Add(OnTracShipmentFields.DimsWeight);

                return ratingField;
            }
        }

        /// <summary>
        /// Customs is never required for OnTrac.
        /// </summary>
        protected override bool IsCustomsRequiredByShipment(ShipmentEntity shipment)
        {
            return false;
        }
        /// <summary>
        /// Saves the requested label format to the child shipment
        /// </summary>
        public override void SaveRequestedLabelFormat(ThermalLanguage requestedLabelFormat, ShipmentEntity shipment)
        {
            if (shipment.OnTrac != null)
            {
                shipment.OnTrac.RequestedLabelFormat = (int)requestedLabelFormat;
            }
        }

        /// <summary>
        /// Update the label format of carrier specific unprocessed shipments
        /// </summary>
        public override void UpdateLabelFormatOfUnprocessedShipments(SqlAdapter adapter, int newLabelFormat, RelationPredicateBucket bucket)
        {
            bucket.Relations.Add(ShipmentEntity.Relations.OnTracShipmentEntityUsingShipmentID);

            adapter.UpdateEntitiesDirectly(new OnTracShipmentEntity { RequestedLabelFormat = newLabelFormat }, bucket);
        }
    }
}
