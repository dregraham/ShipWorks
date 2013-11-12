using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;

namespace ShipWorks.Shipping.Carriers.FedEx.BestRate
{
    /// <summary>
    /// An implementation of the IBestRateShippingBroker that Rate broker that 
    /// finds the best rates for FedEx accounts.
    /// </summary>
    public class FedExBestRateBroker : IBestRateShippingBroker
    {

        private readonly FedExShipmentType shipmentType;
        private readonly ICarrierAccountRepository<FedExAccountEntity> accountRepository;

        public FedExBestRateBroker() : this(new FedExShipmentType(), new FedExAccountRepository())
        {}

        private FedExBestRateBroker(FedExShipmentType shipmentType, ICarrierAccountRepository<FedExAccountEntity> accountRepository)
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
        /// <exception cref="System.NotImplementedException"></exception>
        public bool HasAccounts
        {
            get { return accountRepository.Accounts.Any(); }
        }

        /// <summary>
        /// Gets the rates for each of the accounts of a specific shipping provider based
        /// on the configuration of the best rate shipment data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="exceptionHandler"></param>
        /// <returns>
        /// A list of RateResults for each account of a specific shipping provider (i.e. if two accounts 
        /// are registered for a single provider, the list of rates would have two entries if both 
        /// accounts returned rates).
        /// </returns>
        public List<RateResult> GetBestRates(ShipmentEntity shipment, Action<ShippingException> exceptionHandler)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            List<RateResult> allRates = new List<RateResult>();

            List<FedExAccountEntity> accounts = accountRepository.Accounts.ToList();

            Dictionary<RateResult, FedExShipmentEntity> rateShipments = new Dictionary<RateResult, FedExShipmentEntity>();

            // Create a clone so we don't have to worry about modifying the original shipment
            ShipmentEntity testRateShipment = EntityUtility.CloneEntity(shipment);
            testRateShipment.ShipmentType = (int)ShipmentTypeCode.FedEx;

            foreach (FedExAccountEntity account in accounts)
            {
                testRateShipment.FedEx = new FedExShipmentEntity();

                shipmentType.ConfigureNewShipment(testRateShipment);
                UpdateShipmentSettings(testRateShipment, shipment.ContentWeight, account.FedExAccountID);

                try
                {
                    IEnumerable<RateResult> results = shipmentType.GetRates(testRateShipment).Rates
                                                                  .Where(r => r.Tag != null)
                                                                  .Where(r => r.Amount > 0);

                    // Save a mapping between the rate and the shipment used to get the rate
                    foreach (RateResult result in results)
                    {
                        rateShipments.Add(result, testRateShipment.FedEx);
                    }

                    allRates.AddRange(results);
                }
                catch (ShippingException ex)
                {
                    // Offload exception handling to the passed in exception handler
                    exceptionHandler(ex);
                }
            }

            // Return all the rates, then group by FedExServiceType and ServiceLevel
            List<RateResult> filteredRates = allRates
                .GroupBy(r => ((FedExRateSelection)r.Tag).ServiceType)
                .SelectMany(RateResultsByServiceLevel)
                .ToList();

            foreach (RateResult rate in filteredRates)
            {
                // Replace the service type with a function that will select the correct shipment type
                rate.Tag = CreateRateSelectionFunction(rateShipments[rate], rate.Tag);
                rate.Description = rate.Description.Contains("FedEx") ? rate.Description : "FedEx " + rate.Description;
            }

            return filteredRates.ToList();
        }


        /// <summary>
        /// Creates a function that can be used to select a specific rate
        /// </summary>
        /// <param name="rateShipment">FedExShipment that was used to get the rate</param>
        /// <param name="originalTag">FedExServiceType associated with the specific rate</param>
        /// <returns>A function that, when executed, will convert the passed in shipment to a FedEx shipment
        /// used to create the rate.</returns>
        private static Action<ShipmentEntity> CreateRateSelectionFunction(FedExShipmentEntity rateShipment, object originalTag)
        {
            return selectedShipment =>
            {
                rateShipment.Service = (int)originalTag;
                selectedShipment.ShipmentType = (int)ShipmentTypeCode.FedEx;
                ShippingManager.EnsureShipmentLoaded(selectedShipment);

                if (selectedShipment.FedEx == null)
                {
                    selectedShipment.FedEx = rateShipment;
                }
                else
                {
                    // Grab the original FedEx package so we can get it's FedExPackageID, as we'll need to set it on the
                    // cloned package.  There's probably a better way, so need to check with Brian.
                    FedExPackageEntity selectedPackageEntity = selectedShipment.FedEx.Packages[0];
                    long originalPackageID = selectedPackageEntity.FedExPackageID;

                    // Set the rated shipment as the FedEx shipment
                    selectedShipment.FedEx = rateShipment;

                    // Update the first package FedExPackgeID to be that of the original persisted package.  If this isn't 
                    // done, we get an ORM exception.  There's probably a better way, so need to check with Brian.
                    selectedShipment.FedEx.Packages[0].FedExPackageID = originalPackageID;

                    // Set the shipment and package to be not new so a copy isn't persisted.
                    selectedShipment.FedEx.Packages[0].IsNew = false;
                    selectedShipment.FedEx.IsNew = false;

                }
            };
        }

        /// <summary>
        /// Gets a list of rates by FedExServiceType
        /// </summary>
        /// <param name="typeGroup">Group </param>
        /// <returns></returns>
        private static IEnumerable<RateResult> RateResultsByServiceLevel(IGrouping<FedExServiceType, RateResult> typeGroup)
        {
            return typeGroup
                .GroupBy(r => r.ServiceLevel)
                .Select(serviceLevelRate => serviceLevelRate.OrderBy(rateToOrder => rateToOrder.Amount)
                    .FirstOrDefault());
        }


        /// <summary>
        /// Updates the shipment settings.
        /// </summary>
        /// <param name="testRateShipment">The test rate shipment.</param>
        /// <param name="contentWeight">The content weight.</param>
        /// <param name="accountID">The account unique identifier.</param>
        private static void UpdateShipmentSettings(ShipmentEntity testRateShipment, double contentWeight, long accountID)
        {
            testRateShipment.FedEx.Packages[0].DimsHeight = testRateShipment.BestRate.DimsHeight;
            testRateShipment.FedEx.Packages[0].DimsWidth = testRateShipment.BestRate.DimsWidth;
            testRateShipment.FedEx.Packages[0].DimsLength = testRateShipment.BestRate.DimsLength;

            // ConfigureNewShipment sets these fields, but we need to make sure they're what we expect
            testRateShipment.FedEx.Packages[0].Weight = contentWeight;
            testRateShipment.FedEx.Packages[0].DimsAddWeight = false;
            //testRateShipment.FedEx.Packages[0]. = (int)FedExPackagingType.Custom;
            testRateShipment.FedEx.Service = (int)FedExServiceType.FedExGround;
            testRateShipment.FedEx.FedExAccountID = accountID;
        }

    }
}
