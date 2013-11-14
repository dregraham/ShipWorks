using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Interapptive.Shared.Business;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Settings.Origin;

namespace ShipWorks.Shipping.Carriers.Postal.BestRate
{
    public abstract class PostalResellerBestRateBroker<T> : IBestRateShippingBroker where T : EntityBase2
    {
        private readonly ICarrierAccountRepository<T> accountRepository;
        private readonly string carrierDescription;

        protected PostalResellerBestRateBroker(ICarrierAccountRepository<T> accountRepository, string carrierDescription)
        {
            this.accountRepository = accountRepository;
            this.carrierDescription = carrierDescription;
        }

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
        public List<RateResult> GetBestRates(ShipmentEntity shipment, Action<ShippingException> exceptionHandler)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            List<RateResult> allRates = new List<RateResult>();

            List<T> accounts = accountRepository.Accounts.ToList();

            Dictionary<RateResult, PostalShipmentEntity> rateShipments = new Dictionary<RateResult, PostalShipmentEntity>();

            // Create a clone so we don't have to worry about modifying the original shipment
            ShipmentEntity testRateShipment = EntityUtility.CloneEntity(shipment);
            testRateShipment.ShipmentType = (int)ShipmentCode;

            foreach (T account in accounts)
            {
                testRateShipment.Postal = new PostalShipmentEntity();

                ConfigureNewShipment(testRateShipment);
                UpdateChildShipmentSettings(testRateShipment, shipment, account);

                try
                {
                    var rates = GetRates(testRateShipment);
                    MergeDescriptionsWithNonSelectableRates(rates);

                    IEnumerable<RateResult> results = rates.Where(r => r.Tag != null)
                                                            .Where(r => r.Selectable)
                                                            .Where(r => r.Amount > 0)
                                                            .Where(r => !IsExcludedServiceType(((PostalRateSelection)r.Tag).ServiceType));

                    // Save a mapping between the rate and the shipment used to get the rate
                    foreach (RateResult result in results)
                    {
                        rateShipments.Add(result, testRateShipment.Postal);
                        allRates.Add(result);
                    }
                }
                catch (ShippingException ex)
                {
                    // Offload exception handling to the passed in exception handler
                    exceptionHandler(ex);
                }
            }

            // Return all the rates, then group by PostalServiceType and ServiceLevel
            List<RateResult> filteredRates = allRates
                .GroupBy(r => ((PostalRateSelection)r.Tag).ServiceType)
                .SelectMany(RateResultsByServiceLevel)
                .ToList();

            foreach (RateResult rate in filteredRates)
            {
                // Replace the service type with a function that will select the correct shipment type
                rate.Tag = CreateRateSelectionFunction(rateShipments[rate], (PostalRateSelection)rate.Tag);
                rate.Description = rate.Description.Contains(carrierDescription) ? rate.Description : carrierDescription + " " + rate.Description;
            }

            return filteredRates.ToList();
        }

        /// <summary>
        /// Merge rate descriptions meant as headers with actual rate descriptions
        /// </summary>
        /// <param name="rates">Collection of rates to update</param>
        /// <remarks>It is important that these rates are in the same order that they are returned from
        /// the shipment type's GetRates method or the merging could be incorrect</remarks>
        private static void MergeDescriptionsWithNonSelectableRates(IEnumerable<RateResult> rates)
        {
            Regex beginsWithSpaces = new Regex("^[ ]+");
            
            RateResult lastNonSelectable = null;

            foreach (var rate in rates)
            {
                if (rate.Selectable)
                {
                    if (beginsWithSpaces.IsMatch(rate.Description) && lastNonSelectable != null)
                    {
                        rate.Description = lastNonSelectable.Description + beginsWithSpaces.Replace(rate.Description, " ");
                        rate.Days = lastNonSelectable.Days;
                    }
                }
                else
                {
                    lastNonSelectable = rate;
                }
            }
        }

        /// <summary>
        /// Returns whether the specified service type should be excluded from consideration
        /// </summary>
        /// <param name="serviceType">Service type to check</param>
        /// <returns></returns>
        private static bool IsExcludedServiceType(PostalServiceType serviceType)
        {
            return serviceType == PostalServiceType.MediaMail ||
                   serviceType == PostalServiceType.LibraryMail ||
                   serviceType == PostalServiceType.BoundPrintedMatter ||
                   serviceType == PostalServiceType.DhlBpmExpedited ||
                   serviceType == PostalServiceType.DhlBpmStandard;
        }

        /// <summary>
        /// Creates a function that can be used to select a specific rate
        /// </summary>
        /// <param name="rateShipment">ChildShipment that was used to get the rate</param>
        /// <param name="originalTag">PostalServiceType associated with the specific rate</param>
        /// <returns>A function that, when executed, will convert the passed in shipment to a Postal reseller shipment
        /// used to create the rate.</returns>
        private Action<ShipmentEntity> CreateRateSelectionFunction(PostalShipmentEntity rateShipment, PostalRateSelection originalTag)
        {
            return selectedShipment =>
                {
                    rateShipment.Service = (int) originalTag.ServiceType;
                    rateShipment.Confirmation = (int) originalTag.ConfirmationType;

                    selectedShipment.ShipmentType = (int)ShipmentCode;
                    ShippingManager.EnsureShipmentLoaded(selectedShipment);

                    selectedShipment.Postal = rateShipment;
                    selectedShipment.Postal.IsNew = false;

                    SelectChildShipment(rateShipment, selectedShipment);
                };
        }

        /// <summary>
        /// Gets a list of rates by PostalServiceType
        /// </summary>
        /// <param name="typeGroup">Group </param>
        /// <returns></returns>
        private static IEnumerable<RateResult> RateResultsByServiceLevel(IGrouping<PostalServiceType, RateResult> typeGroup)
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
        /// Updates data on the postal child shipment that is required for checking best rate
        /// </summary>
        /// <param name="testRateShipment">Shipment that we'll be working with</param>
        /// <param name="originalShipment">The original shipment from which data can be copied.</param>
        /// <param name="account">The Account Entity for this shipment.</param>
        private void UpdateChildShipmentSettings(ShipmentEntity testRateShipment, ShipmentEntity originalShipment, T account)
        {
            testRateShipment.OriginOriginID = originalShipment.OriginOriginID;

            // Set the address of the shipment to either the account, or the address of the original shipment
            if (testRateShipment.OriginOriginID == (int)ShipmentOriginSource.Account)
            {
                PersonAdapter.Copy(account, "", testRateShipment, "Origin");
            }
            else
            {
                PersonAdapter.Copy(originalShipment, testRateShipment, "Origin");
            }

            testRateShipment.Postal.DimsHeight = testRateShipment.BestRate.DimsHeight;
            testRateShipment.Postal.DimsWidth = testRateShipment.BestRate.DimsWidth;
            testRateShipment.Postal.DimsLength = testRateShipment.BestRate.DimsLength;

            // ConfigureNewShipment sets these fields, but we need to make sure they're what we expect
            testRateShipment.Postal.DimsWeight = originalShipment.ContentWeight;
            testRateShipment.Postal.DimsAddWeight = false;
            testRateShipment.Postal.PackagingType = (int)PostalPackagingType.Package;
            testRateShipment.Postal.Service = (int)PostalServiceType.PriorityMail;

            UpdateChildAccountId(testRateShipment.Postal, account);
        }

        /// <summary>
        /// Gets the shipment type code for the postal reseller shipment type
        /// </summary>
        protected abstract ShipmentTypeCode ShipmentCode { get; }

        /// <summary>
        /// Convert the best rate shipment into the specified postal reseller shipment
        /// </summary>
        /// <param name="rateShipment">Postal shipment on which to set reseller shipment data</param>
        /// <param name="selectedShipment">Best rate shipment that is being converted</param>
        protected abstract void SelectChildShipment(PostalShipmentEntity rateShipment, ShipmentEntity selectedShipment);

        /// <summary>
        /// Configures a postal reseller shipment for use in the get rates method
        /// </summary>
        /// <param name="shipment">Test shipment that will be used to get rates</param>
        protected abstract void ConfigureNewShipment(ShipmentEntity shipment);

        /// <summary>
        /// Gets rates for the specified shipment
        /// </summary>
        /// <param name="shipment">Shipment for which to get rates</param>
        /// <returns>List of rates</returns>
        protected abstract IEnumerable<RateResult> GetRates(ShipmentEntity shipment);

        /// <summary>
        /// Updates the account id on the postal reseller shipment
        /// </summary>
        /// <param name="postalShipmentEntity">Postal shipment on which the account id should be set</param>
        /// <param name="account">Account that should be used for this shipment</param>
        protected abstract void UpdateChildAccountId(PostalShipmentEntity postalShipmentEntity, T account);
    }
}
