﻿using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.OnTrac.BestRate;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Net.Track;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

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
        /// Gets the AvailablePackageTypes for this shipment type and shipment along with their descriptions.
        /// </summary>
        public override Dictionary<int, string> BuildPackageTypeDictionary(List<ShipmentEntity> shipments, IExcludedPackageTypeRepository excludedServiceTypeRepository)
        {
            return GetAvailablePackageTypes(excludedServiceTypeRepository)
                .Cast<OnTracPackagingType>()
                .Union(shipments.Select(x => x.OnTrac)
                    .Where(x => x != null)
                    .Select(x => (OnTracPackagingType) x.PackagingType))
                .ToDictionary(packagingType => (int) packagingType, packagingType => EnumHelper.GetDescription(packagingType));
        }

        /// <summary>
        /// Create OnTrac specific information
        /// </summary>
        protected override void LoadShipmentDataInternal(ShipmentEntity shipment, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadShipmentData(
                this, shipment, shipment, "OnTrac", typeof(OnTracShipmentEntity), refreshIfPresent);
        }

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment) =>
            GetServiceDescriptionInternal((OnTracServiceType) shipment.OnTrac.Service);

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        public override string GetServiceDescription(string serviceCode) =>
            Functional.ParseInt(serviceCode)
                .Match(x => GetServiceDescriptionInternal((OnTracServiceType) x), _ => "Unknown");

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        private string GetServiceDescriptionInternal(OnTracServiceType service) =>
            EnumHelper.GetDescription(service);

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
                new InsuranceChoice(shipment, shipment.OnTrac, shipment.OnTrac, shipment.OnTrac),
                new DimensionsAdapter(shipment.OnTrac))
            {
                TotalWeight = shipment.TotalWeight
            };
        }

        /// <summary>
        /// Create and Initialize a new shipment
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            if (shipment.OnTrac == null)
            {
                shipment.OnTrac = new OnTracShipmentEntity(shipment.ShipmentID);
            }

            OnTracShipmentEntity onTracShipment = shipment.OnTrac;

            onTracShipment.DeclaredValue = 0;

            onTracShipment.IsCod = false;
            onTracShipment.CodType = 0;
            onTracShipment.CodAmount = 0;

            onTracShipment.PackagingType = (int) OnTracPackagingType.Package;

            onTracShipment.InsurancePennyOne = false;
            onTracShipment.InsuranceValue = 0;
            onTracShipment.Insurance = false;

            shipment.OnTrac.RequestedLabelFormat = (int) ThermalLanguage.None;

            base.ConfigureNewShipment(shipment);
        }

        /// <summary>
        /// Get the default profile for the shipment type
        /// </summary>
        public override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
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

            onTrac.PackagingType = (int) OnTracPackagingType.Package;
            onTrac.ResidentialDetermination = (int) ResidentialDeterminationType.FromAddressValidation;

            onTrac.Reference1 = string.Empty;
            onTrac.Reference2 = string.Empty;
            onTrac.Instructions = string.Empty;
        }

        /// Update the dynamic shipment data that could have changed "outside" the known editor
        /// </summary>
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            shipment.InsuranceProvider = settings.OnTracInsuranceProvider;
            shipment.OnTrac.InsurancePennyOne = settings.OnTracInsurancePennyOne;
            shipment.Insurance = shipment.OnTrac.Insurance;

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
        protected override SettingsControlBase CreateSettingsControl()
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
        private static List<TemplateLabelData> LoadLabelData(Func<ShipmentEntity> shipment)
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
        /// Get postal Tracking link
        /// </summary>
        public override string GetCarrierTrackingUrl(ShipmentEntity shipment)
        {
            if (!shipment.Processed || string.IsNullOrEmpty(shipment.TrackingNumber))
            {
                return string.Empty;
            }

            return $"https://www.ontrac.com/trackingresults.asp?tracking_number={shipment.TrackingNumber}";
        }

        /// <summary>
        /// Get the OnTrac account to be used for the given shipment
        /// </summary>
        public static OnTracAccountEntity GetAccountForShipment(ShipmentEntity shipment)
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
        public override bool IsResidentialStatusRequired(IShipmentEntity shipment) => true;

        /// <summary>
        /// Get the dims weight from a shipment, if any
        /// </summary>
        protected override double GetDimsWeight(IShipmentEntity shipment) =>
            shipment.OnTrac?.DimsAddWeight == true ? shipment.OnTrac.DimsWeight : 0;

        /// <summary>
        /// Gets an instance to the best rate shipping broker for the OnTrac shipment type based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of an OnTracBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment, IBestRateExcludedAccountRepository bestRateExcludedAccountRepository)
        {
            IEnumerable<long> excludedAccounts = bestRateExcludedAccountRepository.GetAll();
            IEnumerable<IOnTracAccountEntity> nonExcludedAccounts = OnTracAccountManager.AccountsReadOnly.Where(a => !excludedAccounts.Contains(a.AccountId));

            if (nonExcludedAccounts.Any())
            {
                return new OnTracBestRateBroker();
            }

            return new NullShippingBroker();
        }

        /// <summary>
        /// Customs is never required for OnTrac.
        /// </summary>
        protected override bool IsCustomsRequiredByShipment(IShipmentEntity shipment)
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
                shipment.OnTrac.RequestedLabelFormat = (int) requestedLabelFormat;
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
