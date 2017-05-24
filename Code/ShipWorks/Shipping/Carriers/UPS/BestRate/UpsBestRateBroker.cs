using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;
using ShipWorks.Shipping.Carriers.UPS.LocalRating.RateFootnotes;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Carriers.UPS.BestRate
{
    /// <summary>
    /// Rate broker that finds the best rates for UPS accounts
    /// </summary>
    public class UpsBestRateBroker : PackageBasedBestRateBroker<UpsAccountEntity, IUpsAccountEntity, UpsPackageEntity>
    {
        private bool isMailInnovationsAvailable;
        private bool canUseSurePost;

        /// <summary>
        /// Creates a broker with the default shipment type and account repository
        /// </summary>
        /// <remarks>This is designed to be used within ShipWorks</remarks>
        public UpsBestRateBroker(ShipmentType shipmentType)
            : this(shipmentType, new UpsAccountRepository(), new UpsSettingsRepository())
        {
        }

        /// <summary>
        /// Creates a broker with the specified shipment type and account repository
        /// </summary>
        /// <param name="shipmentType">Instance of a UPS shipment type that will be used to get rates</param>
        /// <param name="accountRepository">Instance of an account repository that will get UPS accounts</param>
        /// <param name="upsSettingsRepository">The ups settings repository.</param>
        /// <remarks>This is designed to be used by tests</remarks>
        public UpsBestRateBroker(ShipmentType shipmentType, ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository,
            ICarrierSettingsRepository upsSettingsRepository) :
            base(shipmentType, accountRepository, "UPS")
        {
            SettingsRepository = upsSettingsRepository;
            GetRatesAction = (shipment, type) => GetUpsRates(shipment);
        }

        /// <summary>
        /// Gets or sets the settings repository.
        /// </summary>
        protected ICarrierSettingsRepository SettingsRepository { get; }

        /// <summary>
        /// Gets a list of UPS rates
        /// </summary>
        /// <param name="shipment">Shipment for which rates should be retrieved</param>
        /// <param name="brokerExceptions">Action that performs exception handling</param>
        /// <returns>List of NoncompetitiveRateResults</returns>
        /// <remarks>This is overridden because Ups has a requirement that we have to hide their branding if
        /// other carriers rates are present</remarks>
        public override RateGroup GetBestRates(ShipmentEntity shipment, List<BrokerException> brokerExceptions)
        {
            RateGroup bestRates = base.GetBestRates(shipment, brokerExceptions);

            if (isMailInnovationsAvailable)
            {
                brokerExceptions.Add(new BrokerException(new ShippingException("UPS doesn't provide rates for Mail Innovations."), BrokerExceptionSeverityLevel.Information, ShipmentType));
            }

            if (canUseSurePost && !bestRates.Rates.Any(r => UpsUtility.IsUpsSurePostService((UpsServiceType) (r.OriginalTag))))
            {
                // The account is configured to use SurePost, but there weren't any SurePost rates returned, so
                // we want to flag this in the from of sending a BrokerException to the exception handler
                brokerExceptions.Add(new BrokerException(new ShippingException("UPS did not provide SurePost rates."), BrokerExceptionSeverityLevel.Warning, ShipmentType));
            }

            return bestRates;
        }

        /// <summary>
        /// Configures the specified broker settings.
        /// </summary>
        /// <param name="brokerSettings">The broker settings.</param>
        public override void Configure(IBestRateBrokerSettings brokerSettings)
        {
            base.Configure(brokerSettings);

            isMailInnovationsAvailable = brokerSettings.IsMailInnovationsAvailable(ShipmentType);
            canUseSurePost = brokerSettings.CanUseSurePost();
        }

        /// <summary>
        /// Gets the insurance provider.
        /// </summary>
        public override InsuranceProvider GetInsuranceProvider(IShippingSettingsEntity settings)
        {
            return (InsuranceProvider) settings.UpsInsuranceProvider;
        }

        /// <summary>
        /// Applies Ups specific data to the specified shipment
        /// </summary>
        /// <param name="currentShipment">Shipment that will be modified</param>
        /// <param name="originalShipment">Shipment that contains original data for copying</param>
        /// <param name="account">Account that will be attached to the shipment</param>
        protected override void UpdateChildShipmentSettings(ShipmentEntity currentShipment, ShipmentEntity originalShipment, UpsAccountEntity account)
        {
            base.UpdateChildShipmentSettings(currentShipment, originalShipment, account);

            currentShipment.Ups.Packages[0].DimsHeight = originalShipment.BestRate.DimsHeight;
            currentShipment.Ups.Packages[0].DimsWidth = originalShipment.BestRate.DimsWidth;
            currentShipment.Ups.Packages[0].DimsLength = originalShipment.BestRate.DimsLength;
            currentShipment.Ups.Packages[0].DimsProfileID = originalShipment.BestRate.DimsProfileID;

            // ConfigureNewShipment sets these fields, but we need to make sure they're what we expect
            currentShipment.Ups.Packages[0].Weight = originalShipment.ContentWeight;
            currentShipment.Ups.Packages[0].DimsAddWeight = originalShipment.BestRate.DimsAddWeight;
            currentShipment.Ups.Packages[0].DimsWeight = originalShipment.BestRate.DimsWeight;

            // Update total weight
            ShipmentType.UpdateTotalWeight(currentShipment);

            currentShipment.Ups.Packages[0].PackagingType = (int) UpsPackagingType.Custom;
            currentShipment.Ups.Service = (int) UpsServiceType.UpsGround;
            SetAccount(currentShipment, account);

            currentShipment.Ups.Packages[0].Insurance = originalShipment.Insurance;
            currentShipment.Ups.Packages[0].InsuranceValue = originalShipment.BestRate.InsuranceValue;
        }

        /// <summary>
        /// Sets the UPS  account ID on the given shipment.
        /// </summary>
        public virtual void SetAccount(ShipmentEntity currentShipment, UpsAccountEntity account)
        {
            currentShipment.Ups.UpsAccountID = account.UpsAccountID;
        }

        /// <summary>
        /// Gets the account identifier.
        /// </summary>
        protected virtual long GetAccountID(UpsAccountEntity account)
        {
            return account.UpsAccountID;
        }

        /// <summary>
        /// Checks whether the service type specified in the rate should be excluded from best rate consideration
        /// </summary>
        /// <param name="tag">Ups service type from the rate tag</param>
        /// <returns></returns>
        protected override bool IsExcludedServiceType(object tag)
        {
            UpsServiceType serviceType = (UpsServiceType) tag;
            return serviceType == UpsServiceType.UpsSurePostBoundPrintedMatter || serviceType == UpsServiceType.UpsSurePostMedia;
        }

        /// <summary>
        /// Creates and attaches a new instance of a UpsShipment to the specified shipment
        /// </summary>
        protected override void CreateShipmentChild(ShipmentEntity shipment)
        {
            shipment.Ups = new UpsShipmentEntity();
        }

        /// <summary>
        /// Sets the service type on the Ups shipment from the value in the rate tag
        /// </summary>
        /// <param name="shipment">Shipment that will be updated</param>
        /// <param name="tag">Rate tag that represents the service type</param>
        protected override void SetServiceTypeFromTag(ShipmentEntity shipment, object tag)
        {
            shipment.Ups.Service = (int) tag;
        }

        /// <summary>
        /// Gets the current entity state for the specified shipment's child
        /// </summary>
        protected override EntityState ChildShipmentEntityState(ShipmentEntity shipment)
        {
            return shipment.Ups.Fields.State;
        }

        /// <summary>
        /// Gets a collection of packages from the specified shipment
        /// </summary>
        protected override IList<UpsPackageEntity> Packages(ShipmentEntity shipment)
        {
            return shipment.Ups.Packages;
        }

        /// <summary>
        /// Gets the id for the specified package
        /// </summary>
        protected override long PackageId(UpsPackageEntity package)
        {
            return package.UpsPackageID;
        }

        /// <summary>
        /// Sets the id on the specified package
        /// </summary>
        protected override void SetPackageId(UpsPackageEntity package, long packageId)
        {
            package.UpsPackageID = packageId;
        }

        /// <summary>
        /// Gets a description from the specified account
        /// </summary>
        protected override string AccountDescription(UpsAccountEntity account)
        {
            return account.Description;
        }
        
        /// <summary>
        /// Gets the rates function.
        /// </summary>
        private RateGroup GetUpsRates(ShipmentEntity shipment)
        {
            RateGroup rates = new RateGroup(new List<RateResult>());

            // Get rates from ISupportExpress1Rates if it is registered for the shipmenttypecode
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                UpdateShipmentForCurrentBrokerType(shipment);

                IUpsBestRateRatingService ratingService = lifetimeScope.ResolveKeyed<IUpsBestRateRatingService>(ShipmentType.ShipmentTypeCode);

                try
                {
                    rates = ratingService.GetRates(shipment);
                }
                catch (UpsBestRateRatingException)
                {
                    rates.AddFootnoteFactory(new UpsLocalRatingDisabledFootnoteFactory(AccountRepository.GetAccount(shipment)));
                }
                catch (UpsLocalRatingException ex)
                {
                    rates.AddFootnoteFactory(new UpsLocalRatingExceptionFootnoteFactory(ex.Message, shipment.ShipmentTypeCode));
                }
            }

            rates.Carrier = shipment.ShipmentTypeCode;

            return rates;
        }
    }
}
