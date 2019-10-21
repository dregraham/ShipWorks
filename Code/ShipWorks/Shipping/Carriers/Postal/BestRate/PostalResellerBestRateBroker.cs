﻿using System.Collections.Generic;
using Autofac;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.BestRate
{
    /// <summary>
    /// Base class for postal reseller brokers, like Usps and Endicia
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PostalResellerBestRateBroker<T, TInterface> : BestRateBroker<T, TInterface>
        where T : TInterface
        where TInterface : ICarrierAccount
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected PostalResellerBestRateBroker(ShipmentType shipmentType, ICarrierAccountRepository<T, TInterface> accountRepository, string carrierDescription, IBestRateExcludedAccountRepository bestRateExcludedAccountRepository) :
            base(shipmentType, accountRepository, carrierDescription, bestRateExcludedAccountRepository)
        {

        }

        /// <summary>
        /// Get best rates for the specified shipment
        /// </summary>
        public override RateGroup GetBestRates(ShipmentEntity shipment, List<BrokerException> brokerExceptions)
        {
            brokerExceptions.Add(new BrokerException(new ShippingException("Flat rate and regional boxes were not checked for best rates."), BrokerExceptionSeverityLevel.Information, ShipmentType));

            // Postal services do not ship weights over 70 lbs.  Return no rates if this is the case.
            if (shipment.TotalWeight > 70)
            {
                return new RateGroup(new List<RateResult>());
            }

            return base.GetBestRates(shipment, brokerExceptions);
        }

        /// <summary>
        /// Gets a list of Postal rates
        /// </summary>
        /// <param name="shipment">Shipment for which rates should be retrieved</param>
        /// <returns>List of RateResults</returns>
        protected override RateGroup GetRates(ShipmentEntity shipment)
        {
            RateGroup rates = base.GetRates(shipment);

            // If a postal counter provider, show USPS logo, otherwise show appropriate logo such as endicia:
            rates.Rates.ForEach(UseProperUspsLogo);

            return rates;
        }

        /// <summary>
        /// Masks the usps logo.
        /// </summary>
        /// <param name="rate">The rate.</param>
        /// <returns></returns>
        protected void UseProperUspsLogo(RateResult rate)
        {
            if (ShipmentTypeManager.IsPostal(rate.ShipmentType))
            {
                rate.ProviderLogo = EnumHelper.GetImage(rate.ShipmentType);
            }
        }

        /// <summary>
        /// Returns whether the specified service type should be excluded from consideration
        /// </summary>
        /// <param name="tag">Service type to check</param>
        /// <returns></returns>
        protected override bool IsExcludedServiceType(object tag)
        {
            PostalServiceType serviceType = ((PostalRateSelection) tag).ServiceType;

            return serviceType == PostalServiceType.MediaMail ||
                   serviceType == PostalServiceType.LibraryMail ||
                   serviceType == PostalServiceType.BoundPrintedMatter ||
                   serviceType == PostalServiceType.DhlBpmExpedited ||
                   serviceType == PostalServiceType.DhlBpmGround;
        }

        /// <summary>
        /// Updates data on the postal child shipment that is required for checking best rate
        /// </summary>
        /// <param name="currentShipment">Shipment that we'll be working with</param>
        /// <param name="originalShipment">The original shipment from which data can be copied.</param>
        /// <param name="account">The Account Entity for this shipment.</param>
        protected override void UpdateChildShipmentSettings(ShipmentEntity currentShipment, ShipmentEntity originalShipment, T account)
        {
            base.UpdateChildShipmentSettings(currentShipment, originalShipment, account);

            currentShipment.Postal.DimsHeight = originalShipment.BestRate.DimsHeight;
            currentShipment.Postal.DimsWidth = originalShipment.BestRate.DimsWidth;
            currentShipment.Postal.DimsLength = originalShipment.BestRate.DimsLength;
            currentShipment.Postal.DimsProfileID = originalShipment.BestRate.DimsProfileID;

            // ConfigureNewShipment sets these fields, but we need to make sure they're what we expect
            currentShipment.Postal.DimsWeight = originalShipment.BestRate.DimsWeight;
            currentShipment.Postal.DimsAddWeight = originalShipment.BestRate.DimsAddWeight;
            currentShipment.Postal.PackagingType = (int) PostalPackagingType.Package;
            currentShipment.Postal.InsuranceValue = originalShipment.BestRate.InsuranceValue;
            currentShipment.Postal.Insurance = originalShipment.BestRate.Insurance;

            // Update total weight
            ShipmentType.UpdateTotalWeight(currentShipment);

            UpdateChildAccountId(currentShipment.Postal, account);
        }

        /// <summary>
        /// Configures a postal reseller shipment for use in the get rates method
        /// </summary>
        /// <param name="shipment">Test shipment that will be used to get rates</param>
        protected override void CreateShipmentChild(ShipmentEntity shipment)
        {
            shipment.Postal = new PostalShipmentEntity();
        }

        /// <summary>
        /// Gets the service type from the rate tag
        /// </summary>
        /// <param name="tag">Service type specified in the rate tag</param>
        protected override int GetServiceTypeFromTag(object tag)
        {
            return (int) ((PostalRateSelection) tag).ServiceType;
        }

        /// <summary>
        /// Sets the service type on the Postal shipment from the value in the rate tag
        /// </summary>
        /// <param name="shipment">Shipment that will be updated</param>
        /// <param name="tag">Rate tag that represents the service type</param>
        protected override void SetServiceTypeFromTag(ShipmentEntity shipment, object tag)
        {
            shipment.Postal.Service = GetServiceTypeFromTag(tag);
        }

        /// <summary>
        /// Gets the result key for a given rate
        /// </summary>
        /// <param name="rate">Rate result for which to create a result key</param>
        /// <returns>Concatenation of the carrier description and the original rate tag</returns>
        protected override string GetResultKey(RateResult rate)
        {
            // Account for the rate being a previously cached rate where the tag is already a best rate tag;
            // we need to pass the original tag that is a postal service type
            object originalTag = rate.OriginalTag;
            return "Postal" + EnumHelper.GetDescription((PostalServiceType) GetServiceTypeFromTag(originalTag));
        }

        /// <summary>
        /// Updates the account id on the postal reseller shipment
        /// </summary>
        /// <param name="postalShipmentEntity">Postal shipment on which the account id should be set</param>
        /// <param name="account">Account that should be used for this shipment</param>
        protected abstract void UpdateChildAccountId(PostalShipmentEntity postalShipmentEntity, T account);

        /// <summary>
        /// Gets rates from the RatingService without Express1 rates
        /// </summary>
        protected RateGroup GetRatesFunction(ShipmentEntity shipment)
        {
            RateGroup rates;

            // Get rates from ISupportExpress1Rates if it is registered for the shipmenttypecode
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                UpdateShipmentForCurrentBrokerType(shipment);

                ISupportExpress1Rates ratingService = lifetimeScope.ResolveKeyed<ISupportExpress1Rates>(ShipmentType.ShipmentTypeCode);

                // Get rates without express1 rates
                rates = ratingService.GetRates(shipment, false);
            }

            // If a postal counter provider, show USPS logo, otherwise show appropriate logo such as endicia:
            rates.Rates.ForEach(UseProperUspsLogo);

            rates.Carrier = shipment.ShipmentTypeCode;

            return rates;
        }
    }
}
