﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Insurance;
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
            GetRatesAction = ShippingManager.GetRates;
        }

        /// <summary>
        /// Shipment type for the broker
        /// </summary>
        protected ShipmentType ShipmentType { get; private set; }

        /// <summary>
        /// The action to GetRates.
        /// </summary>
        public Func<ShipmentEntity, RateGroup>GetRatesAction { get; set; }

        public abstract InsuranceProvider GetInsuranceProvider(ShippingSettingsEntity settings);

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
        /// Determines whether customs forms are required for the given shipment for any accounts of a broker.
        /// A value of false will only be returned if all of the carrier accounts do not require customs forms.
        /// </summary>
        public virtual bool IsCustomsRequired(ShipmentEntity shipment)
        {
            List<TAccount> accounts = accountRepository.Accounts.ToList();
            foreach (TAccount account in accounts)
            {
                // Create a clone so we don't have to worry about modifying the original shipment
                ShipmentEntity clonedShipmentEntity = EntityUtility.CloneEntity(shipment);
                clonedShipmentEntity.ShipmentType = (int)ShipmentType.ShipmentTypeCode;

                CreateShipmentChild(clonedShipmentEntity);
                ShipmentType.ConfigureNewShipment(clonedShipmentEntity);
                UpdateChildShipmentSettings(clonedShipmentEntity, shipment, account);

                ShipmentType shipmentType = ShipmentTypeManager.GetType(clonedShipmentEntity);
                if (shipmentType.IsCustomsRequired(clonedShipmentEntity))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the single best rate for each account based 
        /// on the configuration of the best rate shipment data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="exceptionHandler"></param>
        /// <returns>A list of RateResults composed of the single best rate for each account.</returns>
        public virtual RateGroup GetBestRates(ShipmentEntity shipment, Action<BrokerException> exceptionHandler)
        {
            List<TAccount> accounts = accountRepository.Accounts.ToList();

            // Get rates for each account asynchronously
            IDictionary<TAccount, Task<RateGroup>> accountRateTasks = accounts.ToDictionary(a => a,
                a => Task<RateGroup>.Factory.StartNew(() => GetRatesForAccount(shipment, a, exceptionHandler)));

            Task.WaitAll(accountRateTasks.Values.ToArray<Task>());
            IDictionary<TAccount, RateGroup> accountRateGroups = accountRateTasks.Where(x => x.Value.Result != null)
                                                                            .ToDictionary(x => x.Key, x => x.Value.Result);

            // Filter the returned rates
            List<RateResult> filteredRates = accountRateGroups.SelectMany(x => x.Value.Rates)
                                                         .Where(IsValidRate)
                                                         .Where(r => !IsExcludedServiceType(r.Tag))
                                                         .ToList();

            // Create a dictionary of rates with their associated accounts for lookup later
            IDictionary<RateResult, TAccount> accountLookup = accountRateGroups
                .Select(ar => ar.Value.Rates.Select(r => new KeyValuePair<RateResult, TAccount>(r, ar.Key)))
                .SelectMany(x => x)
                .Where(x => x.Key != null)
                .ToDictionary(x => x.Key, x => x.Value);

            foreach (RateResult rate in filteredRates)
            {
                // Replace the service type with a function that will select the correct shipment type
                rate.Tag = new BestRateResultTag
                {
                    OriginalTag = rate.Tag,
                    ResultKey = GetResultKey(rate),
                    RateSelectionDelegate = CreateRateSelectionFunction(accountLookup[rate], rate.Tag)
                };
                
                rate.Description = rate.Description.Contains(carrierDescription) ? rate.Description : carrierDescription + " " + rate.Description;
                rate.CarrierDescription = carrierDescription;
            }

            RateGroup bestRateGroup = new RateGroup(filteredRates.ToList());
            AddFootnoteCreators(accountRateGroups, bestRateGroup);

            return bestRateGroup;
        }

        /// <summary>
        /// Gets the result key for a given rate
        /// </summary>
        /// <param name="rate">Rate result for which to create a result key</param>
        /// <returns>Concatenation of the carrier description and the original rate tag</returns>
        protected virtual string GetResultKey(RateResult rate)
        {
            return carrierDescription + rate.Tag;
        }

        /// <summary>
        /// Adds footnote creation functions from the accountRates collection into the bestRateGroup
        /// </summary>
        /// <param name="accountRates">RateGroups from which to get existing footnote functions</param>
        /// <param name="bestRateGroup">RateGroup into which the footnote creation functions will be added</param>
        private void AddFootnoteCreators(IEnumerable<KeyValuePair<TAccount, RateGroup>> accountRates, RateGroup bestRateGroup)
        {
            // Get distinct types of footnotes
            IEnumerable<Func<RateFootnoteControl>> footnoteCreators = accountRates.SelectMany(x => x.Value.FootnoteCreators)
                                                                                  .GroupBy(x => x.Method.ReturnType)
                                                                                  .Select(x => x.FirstOrDefault());

            // Add each distinct footnote to the rate group that we're going to return
            foreach (Func<RateFootnoteControl> footnoteCreator in footnoteCreators)
            {
                Func<RateFootnoteControl> creator = footnoteCreator;

                bestRateGroup.AddFootnoteCreator(() =>
                    {
                        RateFootnoteControl control = creator();
                        control.SetCarrierName(carrierDescription);
                        return control;
                    });
            }
        }

        /// <summary>
        /// Gets rates for the specified account
        /// </summary>
        /// <param name="shipment">Shipment that will be used as the basis for getting rates</param>
        /// <param name="account">Account for which rates will be retrieved</param>
        /// <param name="exceptionHandler">Exceptions will be given to this action for handling</param>
        /// <returns></returns>
        private RateGroup GetRatesForAccount(ShipmentEntity shipment, TAccount account, Action<BrokerException> exceptionHandler)
        {
            try
            {
                // Create a clone so we don't have to worry about modifying the original shipment
                ShipmentEntity testRateShipment = EntityUtility.CloneEntity(shipment);
                testRateShipment.ShipmentType = (int)ShipmentType.ShipmentTypeCode;

                //Set declared value to 0 (for insurance) on the copied shipment prior to getting rates
                testRateShipment.BestRate.InsuranceValue = 0;

                CreateShipmentChild(testRateShipment);
                ShipmentType.ConfigureNewShipment(testRateShipment);
                UpdateChildShipmentSettings(testRateShipment, shipment, account);

                return GetRates(testRateShipment);
            }
            catch (ShippingException ex)
            {
                // Offload exception handling to the passed in exception handler
                exceptionHandler(WrapShippingException(ex));
                return null;
            }
        }

        /// <summary>
        /// Wrap a BrokerException around a ShippingException so it can be handled by BestRateShipmentType
        /// </summary>
        /// <param name="ex">ShippingException to wrap</param>
        /// <returns></returns>
        protected virtual BrokerException WrapShippingException(ShippingException ex)
        {
            return new BrokerException(ex, BrokerExceptionSeverityLevel.Error);
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

            currentShipment.Insurance = originalShipment.Insurance;

            currentShipment.BestRateEvents = originalShipment.BestRateEvents;

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
        protected virtual RateGroup GetRates(ShipmentEntity shipment)
        {
            return GetRatesAction(shipment);
        }

        /// <summary>
        /// Should the rate be included in the results
        /// </summary>
        private static bool IsValidRate(RateResult rate)
        {
            return rate != null && rate.Tag != null && rate.Selectable && rate.Amount > 0;
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
