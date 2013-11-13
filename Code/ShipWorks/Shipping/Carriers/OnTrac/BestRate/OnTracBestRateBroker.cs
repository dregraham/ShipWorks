using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.OnTrac.BestRate
{
    class OnTracBestRateBroker : IBestRateShippingBroker
    {

        private readonly OnTracShipmentType shipmentType;
        private readonly ICarrierAccountRepository<OnTracAccountEntity> accountRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnTracBestRateBroker"/> class.
        /// </summary>
        public OnTracBestRateBroker() : this(new OnTracShipmentType(), new OnTracAccountRepository())
        {}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shipmentType">Type of the shipment.</param>
        /// <param name="accountRepository">The account repository.</param>
        private OnTracBestRateBroker(OnTracShipmentType shipmentType, ICarrierAccountRepository<OnTracAccountEntity> accountRepository)
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

            List<OnTracAccountEntity> accounts = accountRepository.Accounts.ToList();

            Dictionary<RateResult, OnTracShipmentEntity> rateShipments = new Dictionary<RateResult, OnTracShipmentEntity>();

            // Create a clone so we don't have to worry about modifying the original shipment
            ShipmentEntity testRateShipment = EntityUtility.CloneEntity(shipment);
            testRateShipment.ShipmentType = (int)ShipmentTypeCode.OnTrac;

            foreach (OnTracAccountEntity account in accounts)
            {
                testRateShipment.OnTrac = new OnTracShipmentEntity();

                shipmentType.ConfigureNewShipment(testRateShipment);
                UpdateShipmentSettings(testRateShipment, shipment.ContentWeight, account.OnTracAccountID);

                try
                {
                    IEnumerable<RateResult> results = shipmentType.GetRates(testRateShipment).Rates
                                                                  .Where(r => r.Tag != null)
                                                                  .Where(r => r.Amount > 0);

                    // Save a mapping between the rate and the shipment used to get the rate
                    foreach (RateResult result in results)
                    {
                        rateShipments.Add(result, testRateShipment.OnTrac);
                    }

                    allRates.AddRange(results);
                }
                catch (ShippingException ex)
                {
                    // Offload exception handling to the passed in exception handler
                    exceptionHandler(ex);
                }
            }

            // Return all the rates, then group by OnTracServiceType and ServiceLevel
            List<RateResult> filteredRates = allRates
                .GroupBy(r => (OnTracServiceType)r.Tag)
                .SelectMany(RateResultsByServiceLevel)
                .ToList();

            foreach (RateResult rate in filteredRates)
            {
                // Replace the service type with a function that will select the correct shipment type
                rate.Tag = CreateRateSelectionFunction(rateShipments[rate], rate.Tag);
                rate.Description = rate.Description.Contains("OnTrac") ? rate.Description : "OnTrac " + rate.Description;
            }

            return filteredRates.ToList();
        }

        /// <summary>
        /// Creates a function that can be used to select a specific rate
        /// </summary>
        /// <param name="rateShipment">OnTracShipment that was used to get the rate</param>
        /// <param name="originalTag">OnTracServiceType associated with the specific rate</param>
        /// <returns>A function that, when executed, will convert the passed in shipment to a OnTrac shipment
        /// used to create the rate.</returns>
        private static Action<ShipmentEntity> CreateRateSelectionFunction(OnTracShipmentEntity rateShipment, object originalTag)
        {
            return selectedShipment =>
            {
                rateShipment.Service = (int)((OnTracServiceType)originalTag);
                selectedShipment.ShipmentType = (int)ShipmentTypeCode.OnTrac;
                ShippingManager.EnsureShipmentLoaded(selectedShipment);

                if (selectedShipment.OnTrac == null)
                {
                    selectedShipment.OnTrac = rateShipment;
                }
                else
                {
                    // Set the rated shipment as the OnTrac shipment
                    selectedShipment.OnTrac = rateShipment;

                    selectedShipment.OnTrac.IsNew = false;
                }
            };
        }

        /// <summary>
        /// Gets a list of rates by OnTracServiceType
        /// </summary>
        /// <param name="typeGroup">Group </param>
        /// <returns></returns>
        private static IEnumerable<RateResult> RateResultsByServiceLevel(IGrouping<OnTracServiceType, RateResult> typeGroup)
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
            testRateShipment.OnTrac.DimsHeight = testRateShipment.BestRate.DimsHeight;
            testRateShipment.OnTrac.DimsWidth = testRateShipment.BestRate.DimsWidth;
            testRateShipment.OnTrac.DimsLength = testRateShipment.BestRate.DimsLength;

            // ConfigureNewShipment sets these fields, but we need to make sure they're what we expect
            testRateShipment.OnTrac.DimsWeight = contentWeight;
            testRateShipment.OnTrac.DimsAddWeight = false;
            testRateShipment.OnTrac.Service = (int)OnTracServiceType.Ground;
            testRateShipment.OnTrac.OnTracAccountID = accountID;
        }
    }
}
