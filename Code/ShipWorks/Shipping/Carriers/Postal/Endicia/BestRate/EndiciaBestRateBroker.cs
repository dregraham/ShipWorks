using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Interapptive.Shared.Business;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Settings.Origin;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.BestRate
{
    public class EndiciaBestRateBroker : IBestRateShippingBroker
    {
        private readonly EndiciaShipmentType shipmentType;
        private readonly ICarrierAccountRepository<EndiciaAccountEntity> accountRepository;

        public EndiciaBestRateBroker() : this(new EndiciaShipmentType(), new EndiciaAccountRepository())
        {
            
        }

        public EndiciaBestRateBroker(EndiciaShipmentType shipmentType, ICarrierAccountRepository<EndiciaAccountEntity> accountRepository)
        {
            this.shipmentType = shipmentType;
            this.accountRepository = accountRepository;
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
        /// Gets the single best rate for each UPS account based 
        /// on the configuration of the best rate shipment data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="exceptionHandler"></param>
        /// <returns>A list of RateResults composed of the single best rate for each UPS account.</returns>
        public List<RateResult> GetBestRates(ShipmentEntity shipment, Action<ShippingException> exceptionHandler)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            List<RateResult> allRates = new List<RateResult>();

            List<EndiciaAccountEntity> upsAccounts = accountRepository.Accounts.ToList();

            Dictionary<RateResult, PostalShipmentEntity> rateShipments = new Dictionary<RateResult, PostalShipmentEntity>();

            // Create a clone so we don't have to worry about modifying the original shipment
            ShipmentEntity testRateShipment = EntityUtility.CloneEntity(shipment);
            testRateShipment.ShipmentType = (int)ShipmentTypeCode.Endicia;

            foreach (EndiciaAccountEntity account in upsAccounts)
            {
                testRateShipment.Postal = new PostalShipmentEntity { Endicia = new EndiciaShipmentEntity() };

                shipmentType.ConfigureNewShipment(testRateShipment);
                UpdateEndiciaShipmentSettings(testRateShipment, shipment, account);

                try
                {
                    var rates = shipmentType.GetRates(testRateShipment).Rates;
                    MergeDescriptionsWithNonSelectableRates(rates);

                    IEnumerable<RateResult> results = rates.Where(r => r.Tag != null)
                                                           .Where(r => r.Selectable)
                                                           .Where(r => r.Amount > 0)
                                                           .Where(r => !IsExcludedServiceType(((PostalRateSelection)r.Tag).ServiceType));

                    // Save a mapping between the rate and the UPS shipment used to get the rate
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
                rate.Description = rate.Description.Contains("Endicia") ? rate.Description : "Endicia " + rate.Description;
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
        /// <param name="rateShipment">UpsShipment that was used to get the rate</param>
        /// <param name="originalTag">PostalServiceType associated with the specific rate</param>
        /// <returns>A function that, when executed, will convert the passed in shipment to a UPS shipment
        /// used to create the rate.</returns>
        private static Action<ShipmentEntity> CreateRateSelectionFunction(PostalShipmentEntity rateShipment, PostalRateSelection originalTag)
        {
            return selectedShipment =>
                {
                    rateShipment.Service = (int) originalTag.ServiceType;
                    rateShipment.Confirmation = (int) originalTag.ConfirmationType;
                
                    selectedShipment.ShipmentType = (int)ShipmentTypeCode.Endicia;
                    ShippingManager.EnsureShipmentLoaded(selectedShipment);

                    // Save a reference to the Endicia shipment entity because if we set the shipment id while it's 
                    // attached to the Postal entity, the Endicia entity will be set to null
                    EndiciaShipmentEntity newEndiciaShipment = rateShipment.Endicia;
                    newEndiciaShipment.ShipmentID = selectedShipment.ShipmentID;

                    selectedShipment.Postal = rateShipment;
                    selectedShipment.Postal.Endicia = newEndiciaShipment;
                    selectedShipment.Postal.IsNew = false;
                    selectedShipment.Postal.Endicia.IsNew = false;
                };
        }

        /// <summary>
        /// Gets a list of rates by PostalServiceType
        /// </summary>
        /// <param name="upsTypeGroup">Group </param>
        /// <returns></returns>
        private static IEnumerable<RateResult> RateResultsByServiceLevel(IGrouping<PostalServiceType, RateResult> upsTypeGroup)
        {
            return upsTypeGroup.GroupBy(r => r.ServiceLevel).Select(CheapestRateInGroup);
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
        /// Updates data on the Ups shipment that is required for checking best rate
        /// </summary>
        /// <param name="testRateShipment">Shipment that we'll be working with</param>
        /// <param name="originalShipment">The original shipment from which data can be copied.</param>
        /// <param name="endiciaAccount">The UPS Account Entity for this shipment.</param>
        private static void UpdateEndiciaShipmentSettings(ShipmentEntity testRateShipment, ShipmentEntity originalShipment, EndiciaAccountEntity endiciaAccount)
        {
            testRateShipment.OriginOriginID = originalShipment.OriginOriginID;

            // Set the address of the shipment to either the UPS account, or the address of the original shipment
            if (testRateShipment.OriginOriginID == (int)ShipmentOriginSource.Account)
            {
                PersonAdapter.Copy(endiciaAccount, "", testRateShipment, "Origin");
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
            testRateShipment.Postal.Endicia.EndiciaAccountID = endiciaAccount.EndiciaAccountID;
        }
    }
}
