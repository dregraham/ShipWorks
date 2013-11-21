using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Settings.Origin;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Defines most of the logic for the carrier specific best rate brokers
    /// </summary>
    /// <typeparam name="TAccount">Type of account</typeparam>
    public abstract class BestRateBroker<TAccount> : IBestRateShippingBroker where TAccount : EntityBase2
    {
        private readonly ICarrierAccountRepository<TAccount> accountRepository;
        private readonly string carrierDescription;

        /// <summary>
        /// Constructor
        /// </summary>
        protected BestRateBroker(ShipmentType shipmentType, ICarrierAccountRepository<TAccount> accountRepository, string carrierDescription)
        {
            this.accountRepository = accountRepository;
            this.carrierDescription = carrierDescription;
            ShipmentType = shipmentType;
            GetRatesAction = shipment => ShippingManager.GetRates(shipment).Rates;
        }

        /// <summary>
        /// Shipment type for the broker
        /// </summary>
        protected ShipmentType ShipmentType { get; private set; }

        /// <summary>
        /// The action to GetRates.
        /// </summary>
        public Func<ShipmentEntity, IEnumerable<RateResult>> GetRatesAction { get; set; }

        /// <summary>
        /// Gets a value indicating whether there any accounts available to a broker.
        /// </summary>
        /// <value>
        /// <c>true</c> if the broker [has accounts]; otherwise, <c>false</c>.
        /// </value>
        public bool HasAccounts
        {
            get { return accountRepository.Accounts.Any(); }
        }

        /// <summary>
        /// Gets the single best rate for each account based 
        /// on the configuration of the best rate shipment data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="exceptionHandler"></param>
        /// <returns>A list of RateResults composed of the single best rate for each account.</returns>
        public virtual List<RateResult> GetBestRates(ShipmentEntity shipment, Action<BrokerException> exceptionHandler)
        {
            List<RateResult> allRates = new List<RateResult>();

            List<TAccount> accounts = accountRepository.Accounts.ToList();

            Dictionary<RateResult, TAccount> rateShipments = new Dictionary<RateResult, TAccount>();

            // Create a clone so we don't have to worry about modifying the original shipment
            ShipmentEntity testRateShipment = EntityUtility.CloneEntity(shipment);
            testRateShipment.ShipmentType = (int)ShipmentType.ShipmentTypeCode;

            foreach (TAccount account in accounts)
            {
                try
                {
                    CreateShipmentChild(testRateShipment);
                    ShipmentType.ConfigureNewShipment(testRateShipment);
                    UpdateChildShipmentSettings(testRateShipment, shipment, account);

                    var rates = GetRates(testRateShipment);

                    IEnumerable<RateResult> results = rates.Where(r => r.Tag != null)
                                                           .Where(r => r.Selectable)
                                                           .Where(r => r.Amount > 0)
                                                           .Where(r => !IsExcludedServiceType(r.Tag));

                    // Save a mapping between the rate and the shipment used to get the rate
                    foreach (RateResult result in results)
                    {
                        rateShipments.Add(result, account);
                        allRates.Add(result);
                    }
                }
                catch (ShippingException ex)
                {
                    // Offload exception handling to the passed in exception handler
                    exceptionHandler(WrapShippingException(ex));
                }
            }

            // Return all the rates, then group by PostalServiceType and ServiceLevel
            List<RateResult> filteredRates = allRates
                .GroupBy(r => GetServiceTypeFromTag(r.Tag))
                .SelectMany(RateResultsByServiceLevel)
                .ToList();

            foreach (RateResult rate in filteredRates)
            {
                // Replace the service type with a function that will select the correct shipment type
                rate.Tag = CreateRateSelectionFunction(rateShipments[rate], rate.Tag);
                rate.Description = rate.Description.Contains(carrierDescription) ? rate.Description : carrierDescription + " " + rate.Description;
            }

            return filteredRates.ToList();
        }

        /// <summary>
        /// Wrap a BrokerException around a ShippingException so it can be handled by BestRateShipmentType
        /// </summary>
        /// <param name="ex">ShippingException to wrap</param>
        /// <returns></returns>
        protected virtual BrokerException WrapShippingException(ShippingException ex)
        {
            return new BrokerException(ex, BrokerExceptionSeverityLevel.High, carrierDescription);
        }

        /// <summary>
        /// Creates a function that can be used to select a specific rate
        /// </summary>
        /// <param name="account">ChildShipment that was used to get the rate</param>
        /// <param name="originalTag">PostalServiceType associated with the specific rate</param>
        /// <returns>A function that, when executed, will convert the passed in shipment to a Postal reseller shipment
        /// used to create the rate.</returns>
        private Action<ShipmentEntity> CreateRateSelectionFunction(TAccount account, object originalTag)
        {
            return selectedShipment =>
                {
                    // Create a clone so we don't have to worry about modifying the original shipment
                    ShipmentEntity originalShipment = EntityUtility.CloneEntity(selectedShipment);
                    selectedShipment.ShipmentType = (int) ShipmentType.ShipmentTypeCode;

                    ShippingManager.EnsureShipmentLoaded(selectedShipment);

                    SelectRate(selectedShipment);
                    UpdateChildShipmentSettings(selectedShipment, originalShipment, account);

                    SetServiceTypeFromTag(selectedShipment, originalTag);
                };
        }

        /// <summary>
        /// Updates data on the postal child shipment that is required for checking best rate
        /// </summary>
        /// <param name="currentShipment">Shipment that we'll be working with</param>
        /// <param name="originalShipment">The original shipment from which data can be copied.</param>
        /// <param name="account">The Account Entity for this shipment.</param>
        protected virtual void UpdateChildShipmentSettings(ShipmentEntity currentShipment, ShipmentEntity originalShipment, TAccount account)
        {
            currentShipment.OriginOriginID = originalShipment.OriginOriginID;

            // Set the address of the shipment to either the account, or the address of the original shipment
            if (currentShipment.OriginOriginID == (int)ShipmentOriginSource.Account)
            {
                PersonAdapter.Copy(account, "", currentShipment, "Origin");
            }
            else
            {
                PersonAdapter.Copy(originalShipment, currentShipment, "Origin");
            }
        }

        /// <summary>
        /// Gets the service type from the rate tag
        /// </summary>
        /// <param name="tag">Service type specified in the rate tag</param>
        protected virtual int GetServiceTypeFromTag(object tag)
        {
            return (int)tag;
        }

        /// <summary>
        /// Gets a list of rates for the specified shipment
        /// </summary>
        protected virtual IEnumerable<RateResult> GetRates(ShipmentEntity shipment)
        {
            return GetRatesAction(shipment);
        }

        /// <summary>
        /// Gets a list of rates by PostalServiceType
        /// </summary>
        /// <param name="typeGroup">Group </param>
        /// <returns></returns>
        private static IEnumerable<RateResult> RateResultsByServiceLevel(IGrouping<int, RateResult> typeGroup)
        {
            return typeGroup.GroupBy(r => r.ServiceLevel).Select(CheapestRateInGroup);
        }

        /// <summary>
        /// Gets the cheapest rate in group of rates.
        /// </summary>
        /// <param name="serviceLevelGroup">Group of rates from which to return the cheapest</param>
        /// <returns></returns>
        private static RateResult CheapestRateInGroup(IGrouping<ServiceLevelType, RateResult> serviceLevelGroup)
        {
            return serviceLevelGroup.OrderBy(r => r.Amount).FirstOrDefault();
        }

        /// <summary>
        /// Perform carrier specific actions when a rate is selected
        /// </summary>
        protected virtual void SelectRate(ShipmentEntity shipment)
        {

        }

        /// <summary>
        /// Returns whether the specified service type should be excluded from consideration
        /// </summary>
        /// <param name="tag">Service type to check</param>
        /// <returns></returns>
        protected abstract bool IsExcludedServiceType(object tag);

        /// <summary>
        /// Configures a postal reseller shipment for use in the get rates method
        /// </summary>
        /// <param name="shipment">Test shipment that will be used to get rates</param>
        protected abstract void CreateShipmentChild(ShipmentEntity shipment);

        /// <summary>
        /// Sets the service type on the shipment from the value in the rate tag
        /// </summary>
        /// <param name="shipment">Shipment that will be updated</param>
        /// <param name="tag">Rate tag that represents the service type</param>
        protected abstract void SetServiceTypeFromTag(ShipmentEntity shipment, object tag);
    }
}
