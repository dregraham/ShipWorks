using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.Custom.EntityClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Defines most of the logic for the carrier specific best rate brokers
    /// </summary>
    /// <typeparam name="TAccount">Type of account</typeparam>
    public abstract class BestRateBroker<TAccount> : IBestRateShippingBroker where TAccount : ICarrierAccount
    {
        private readonly string carrierDescription;

        /// <summary>
        /// Constructor
        /// </summary>
        protected BestRateBroker(ShipmentType shipmentType, ICarrierAccountRepository<TAccount> accountRepository, string carrierDescription)
        {
            this.AccountRepository = accountRepository;
            this.carrierDescription = carrierDescription;
            shipmentType.ShouldApplyShipSense = false;
            ShipmentType = shipmentType;
            GetRatesAction = ShippingManager.GetRates;
        }

        /// <summary>
        /// Gets or sets the account repository.
        /// </summary>
        protected ICarrierAccountRepository<TAccount> AccountRepository
        {
            get;
            private set;
        }

        /// <summary>
        /// Shipment type for the broker
        /// </summary>
        public virtual ShipmentType ShipmentType { get; private set; }

        /// <summary>
        /// The action to GetRates.
        /// </summary>
        public Func<ShipmentEntity, ShipmentType, RateGroup>GetRatesAction { get; set; }

        public abstract InsuranceProvider GetInsuranceProvider(ShippingSettingsEntity settings);

        /// <summary>
        /// Gets a value indicating whether there any accounts available to a broker.
        /// </summary>
        /// <value>
        /// <c>true</c> if the broker [has accounts]; otherwise, <c>false</c>.
        /// </value>
        public bool HasAccounts
        {
            get { return AccountRepository.Accounts.Any(); }
        }

        /// <summary>
        /// Gets a value indicating whether the broker is a counter broker.
        /// </summary>
        /// <value>
        ///   False by default
        /// </value>
        public virtual bool IsCounterRate
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether customs forms are required for the given shipment for any accounts of a broker.
        /// A value of false will only be returned if all of the carrier accounts do not require customs forms.
        /// </summary>
        public virtual bool IsCustomsRequired(ShipmentEntity shipment)
        {
            List<TAccount> accounts = new List<TAccount>();

            try
            {
                accounts = AccountsForRates(shipment);
            }
            catch (ShippingException)
            {
                // We don't need to worry about customs if there are no accounts
            }

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
        /// <param name="brokerExceptions"></param>
        /// <returns>A list of RateResults composed of the single best rate for each account.</returns>
        public virtual RateGroup GetBestRates(ShipmentEntity shipment, List<BrokerException> brokerExceptions)
        {
            List<TAccount> accounts = GetAccountsForRating(shipment, brokerExceptions);

            // Get rates for each account asynchronously
            IDictionary<TAccount, Task<RateGroup>> accountRateTasks = accounts.ToDictionary(a => a,
                                                                                            a => CreateGetBestRateTask(shipment, brokerExceptions, a));

            Task.WaitAll(accountRateTasks.Values.ToArray<Task>());
            IDictionary<TAccount, RateGroup> accountRateGroups = accountRateTasks.Where(x => x.Value.Result != null)
                                                                                 .ToDictionary(x => x.Key, x => x.Value.Result);

            // Filter the returned rates
            List<RateResult> filteredRates = FilterAccountRates(accountRateGroups);

            IDictionary<RateResult, TAccount> accountLookup = CreateRateAccountDictionary(accountRateGroups);

            foreach (RateResult rate in filteredRates)
            {
                UpdateRateDetails(accountLookup, rate);
            }

            RateGroup bestRateGroup = new RateGroup(filteredRates.ToList());
            if (!filteredRates.Any())
            {
                // With no rates for the group, the carrier will default to UPS, so set it correctly if there are no rates.
                bestRateGroup.Carrier = ShipmentType.ShipmentTypeCode;
            }

            AddFootnoteCreators(accountRateGroups, bestRateGroup);

            return bestRateGroup;
        }

        /// <summary>
        /// Update the rate details
        /// </summary>
        private void UpdateRateDetails(IDictionary<RateResult, TAccount> accountLookup, RateResult rate)
        {
            // Account for the rate being a previously cached rate where the tag is already a best rate tag
            object originalTag = rate.OriginalTag;

            // Replace the service type with a function that will select the correct shipment type
            rate.Tag = new BestRateResultTag
            {
                OriginalTag = originalTag,
                ResultKey = GetResultKey(rate),
                RateSelectionDelegate = CreateRateSelectionFunction(accountLookup[rate], originalTag),
                AccountDescription = AccountDescription(accountLookup[rate])
            };

            rate.Description = rate.Description.Contains(carrierDescription) ? rate.Description : carrierDescription + " " + rate.Description;
            rate.CarrierDescription = carrierDescription;

            // Child rates (like USPS Priority with signature or delivery confirmation) won't have a provider logo set
            rate.ProviderLogo = rate.ProviderLogo ?? EnumHelper.GetImage(ShipmentType.ShipmentTypeCode);
        }

        /// <summary>
        /// Get list of accounts for rating
        /// </summary>
        private List<TAccount> GetAccountsForRating(ShipmentEntity shipment, ICollection<BrokerException> brokerExceptions)
        {
            try
            {
                return AccountsForRates(shipment);
            }
            catch (ShippingException ex)
            {
                brokerExceptions.Add(new BrokerException(ex, BrokerExceptionSeverityLevel.Error, ShipmentType));
            }

            return new List<TAccount>();
        }

        /// <summary>
        /// Filter the account rate groups
        /// </summary>
        private List<RateResult> FilterAccountRates(IDictionary<TAccount, RateGroup> accountRateGroups)
        {
            return accountRateGroups.SelectMany(x => x.Value.Rates)
                .Where(IsValidRate)
                .Where(r => !IsExcludedServiceType(r.OriginalTag))
                .ToList();
        }

        /// <summary>
        /// Create a dictionary of rates with their associated accounts for lookup later
        /// </summary>
        private static Dictionary<RateResult, TAccount> CreateRateAccountDictionary(IDictionary<TAccount, RateGroup> accountRateGroups)
        {
            return accountRateGroups
                .Select(ar => ar.Value.Rates.Select(r => new KeyValuePair<RateResult, TAccount>(r, ar.Key)))
                .SelectMany(x => x)
                .Where(x => x.Key != null)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Create a task to get best rates
        /// </summary>
        private Task<RateGroup> CreateGetBestRateTask(ShipmentEntity shipment, List<BrokerException> brokerExceptions, TAccount a)
        {
            return Task<RateGroup>.Factory.StartNew(() =>
            {
                using (new BestRateScope())
                {
                    return GetRatesForAccount(shipment, a, brokerExceptions);
                }
            });
        }

        /// <summary>
        /// Returns a list of accounts from the account repository where the account country code
        /// matches the shipment origin country code.
        /// </summary>
        private List<TAccount> AccountsForRates(ShipmentEntity shipment)
        {
            // Only add the account to the account list if it's not null
            TAccount accountForRating = AccountRepository.Accounts.Count() == 1 ? AccountRepository.Accounts.First() : AccountRepository.DefaultProfileAccount;
            IEnumerable<TAccount> accounts = Object.Equals(accountForRating, default(TAccount)) ? new List<TAccount>() : new List<TAccount> { accountForRating };

            // Filter the list to be the default profile account, and that it's other properties are valid
            accounts = accounts.Where(account =>
            {
                if (account is NullEntity ||
                    (shipment.OriginOriginID == (int)ShipmentOriginSource.Account && Equals(account, accountForRating)))
                {
                    return true;
                }

                PersonAdapter personAdapter = new PersonAdapter(account, "");
                return personAdapter.AdjustedCountryCode((ShipmentTypeCode)shipment.ShipmentType) == shipment.AdjustedOriginCountryCode();
            });

            return accounts.ToList();
        }

        /// <summary>
        /// Gets a description from the specified account
        /// </summary>
        protected virtual string AccountDescription(TAccount account)
        {
            return string.Empty;
        }

        /// <summary>
        /// Configures the broker using the given settings.
        /// </summary>
        /// <param name="brokerSettings">The broker settings.</param>
        public virtual void Configure(IBestRateBrokerSettings brokerSettings)
        {
            // Do nothing
        }

        /// <summary>
        /// Gets the result key for a given rate
        /// </summary>
        /// <param name="rate">Rate result for which to create a result key</param>
        /// <returns>Concatenation of the carrier description and the original rate tag</returns>
        protected virtual string GetResultKey(RateResult rate)
        {
            return carrierDescription + rate.OriginalTag;
        }

        /// <summary>
        /// Adds footnote creation functions from the accountRates collection into the bestRateGroup
        /// </summary>
        /// <param name="accountRates">RateGroups from which to get existing footnote functions</param>
        /// <param name="bestRateGroup">RateGroup into which the footnote creation functions will be added</param>
        private static void AddFootnoteCreators(IEnumerable<KeyValuePair<TAccount, RateGroup>> accountRates, RateGroup bestRateGroup)
        {
            // Get distinct types of footnotes
            IEnumerable<IRateFootnoteFactory> distinctFootnoteFactories = accountRates.SelectMany(x => x.Value.FootnoteFactories)
                                                                                      .Where(f => f.AllowedForBestRate)
                                                                                      .GroupBy(f => f.GetType())
                                                                                      .Select(f => f.FirstOrDefault());

            // Add each distinct footnote to the rate group that we're going to return
            foreach (IRateFootnoteFactory factory in distinctFootnoteFactories)
            {
                bestRateGroup.AddFootnoteFactory(factory);
            }
        }

        /// <summary>
        /// Gets rates for the specified account
        /// </summary>
        /// <param name="shipment">Shipment that will be used as the basis for getting rates</param>
        /// <param name="account">Account for which rates will be retrieved</param>
        /// <param name="exceptionHandler">Exceptions will be given to this action for handling</param>
        /// <returns></returns>
        private RateGroup GetRatesForAccount(ShipmentEntity shipment, TAccount account, List<BrokerException> exceptionHandler)
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

                // Denote whether the rate is a counter or not based on the broker
                // that was retrieving the rates
                RateGroup rateGroup = GetRates(testRateShipment);

                // Verify that the rates returned are actually valid and add the exception
                // if they aren't
                InvalidRateGroup invalidRateGroup = rateGroup as InvalidRateGroup;
                if (invalidRateGroup != null)
                {
                    exceptionHandler.Add(WrapShippingException(invalidRateGroup.ShippingException));
                    return null;
                }

                rateGroup.Rates.ForEach(r => r.IsCounterRate = IsCounterRate);

                return rateGroup;
            }
            catch (ShippingException ex)
            {
                // Offload exception handling to the passed in exception handler
                exceptionHandler.Add(WrapShippingException(ex));
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
            return new BrokerException(ex, BrokerExceptionSeverityLevel.Error, ShipmentType);
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
                    ChangeShipmentType(selectedShipment);

                    // Since the list of providers used in best rate settings is independent than the global
                    // list we need to make sure the shipment type is removed from the excluded list if needed
                    ShippingSettingsEntity settings = ShippingSettings.Fetch();
                    if (settings.ExcludedTypes.Contains(selectedShipment.ShipmentType))
                    {
                        settings.ExcludedTypes = settings.ExcludedTypes.Where(t => t != selectedShipment.ShipmentType).ToArray();
                        ShippingSettings.Save(settings);
                    }

                    LoadShipment(selectedShipment);

                    SelectRate(selectedShipment);
                    UpdateChildShipmentSettings(selectedShipment, originalShipment, account);

                    SetServiceTypeFromTag(selectedShipment, originalTag);
                };
        }

        /// <summary>
        /// Loads the shipment.
        /// </summary>
        private static void LoadShipment(ShipmentEntity selectedShipment)
        {
            try
            {
                ShippingManager.EnsureShipmentLoaded(selectedShipment);
            }
            catch (NotFoundException)
            {
                ShipmentType shipmentType = ShipmentTypeManager.GetType(selectedShipment);
                shipmentType.ConfigureNewShipment(selectedShipment);
            }
        }

        /// <summary>
        /// Changes the shipment type on the specified shipment
        /// </summary>
        protected virtual void ChangeShipmentType(ShipmentEntity selectedShipment)
        {
            selectedShipment.ShipmentType = (int) ShipmentType.ShipmentTypeCode;
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
            currentShipment.ShipSenseStatus = originalShipment.ShipSenseStatus;
            currentShipment.ShipSenseChangeSets = originalShipment.ShipSenseChangeSets;

            UpdateShipmentOriginAddress(currentShipment, originalShipment, account);
        }

        /// <summary>
        /// Updates the origin address of the shipment.
        /// </summary>
        /// <param name="currentShipment">The shipment that will be updated.</param>
        /// <param name="originalShipment">The original shipment from which data can be copied.</param>
        /// <param name="account">The AccountEntity for this shipment.</param>
        protected virtual void UpdateShipmentOriginAddress(ShipmentEntity currentShipment, ShipmentEntity originalShipment, TAccount account)
        {
            // Set the address of the shipment to either the account, or the address of the original shipment
            if (!IsCounterRate && currentShipment.OriginOriginID == (int)ShipmentOriginSource.Account)
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
            return GetRatesAction(shipment, ShipmentType);
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
