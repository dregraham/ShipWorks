using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.iParcel.BestRate
{
    class iParcelBestRateBroker : IBestRateShippingBroker
    {
        private readonly iParcelShipmentType shipmentType;
        private readonly ICarrierAccountRepository<IParcelAccountEntity> accountRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelBestRateBroker"/> class.
        /// </summary>
        public iParcelBestRateBroker() : this(new iParcelShipmentType(), new iParcelAccountRepository())
        {}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shipmentType">Type of the shipment.</param>
        /// <param name="accountRepository">The account repository.</param>
        private iParcelBestRateBroker(iParcelShipmentType shipmentType, ICarrierAccountRepository<IParcelAccountEntity> accountRepository)
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

            List<IParcelAccountEntity> accounts = accountRepository.Accounts.ToList();

            Dictionary<RateResult, IParcelShipmentEntity> rateShipments = new Dictionary<RateResult, IParcelShipmentEntity>();

            // Create a clone so we don't have to worry about modifying the original shipment
            ShipmentEntity testRateShipment = EntityUtility.CloneEntity(shipment);
            testRateShipment.ShipmentType = (int)ShipmentTypeCode.iParcel;

            foreach (IParcelAccountEntity account in accounts)
            {
                testRateShipment.IParcel = new IParcelShipmentEntity();

                shipmentType.ConfigureNewShipment(testRateShipment);
                UpdateShipmentSettings(testRateShipment, shipment.ContentWeight, account.IParcelAccountID);

                try
                {
                    IEnumerable<RateResult> results = shipmentType.GetRates(testRateShipment).Rates
                                                                  .Where(r => r.Tag != null)
                                                                  .Where(r => r.Amount > 0);

                    // Save a mapping between the rate and the shipment used to get the rate
                    foreach (RateResult result in results)
                    {
                        rateShipments.Add(result, testRateShipment.IParcel);
                    }

                    allRates.AddRange(results);
                }
                catch (ShippingException ex)
                {
                    // Offload exception handling to the passed in exception handler
                    exceptionHandler(ex);
                }
            }

            // Return all the rates, then group by iParcelServiceType and ServiceLevel
            List<RateResult> filteredRates = allRates
                .GroupBy(r => ((iParcelRateSelection)r.Tag).ServiceType)
                .SelectMany(RateResultsByServiceLevel)
                .ToList();

            foreach (RateResult rate in filteredRates)
            {
                // Replace the service type with a function that will select the correct shipment type
                rate.Tag = CreateRateSelectionFunction(rateShipments[rate], rate.Tag);
                rate.Description = rate.Description.Contains("iParcel") ? rate.Description : "iParcel " + rate.Description;
            }

            return filteredRates.ToList();
        }

        /// <summary>
        /// Gets a list of rates by iParcelServiceType
        /// </summary>
        /// <param name="typeGroup">Group </param>
        /// <returns></returns>
        private static IEnumerable<RateResult> RateResultsByServiceLevel(IGrouping<iParcelServiceType, RateResult> typeGroup)
        {
            return typeGroup
                .GroupBy(r => r.ServiceLevel)
                .Select(serviceLevelRate => serviceLevelRate.OrderBy(rateToOrder => rateToOrder.Amount)
                    .FirstOrDefault());
        }

        /// <summary>
        /// Creates a function that can be used to select a specific rate
        /// </summary>
        /// <param name="rateShipment">iParcelShipment that was used to get the rate</param>
        /// <param name="originalTag">iParcelServiceType associated with the specific rate</param>
        /// <returns>A function that, when executed, will convert the passed in shipment to a iParcel shipment
        /// used to create the rate.</returns>
        private static Action<ShipmentEntity> CreateRateSelectionFunction(IParcelShipmentEntity rateShipment, object originalTag)
        {
            return selectedShipment =>
            {
                rateShipment.Service = (int)((iParcelRateSelection)originalTag).ServiceType;
                selectedShipment.ShipmentType = (int)ShipmentTypeCode.iParcel;
                ShippingManager.EnsureShipmentLoaded(selectedShipment);

                if (selectedShipment.IParcel == null)
                {
                    selectedShipment.IParcel = rateShipment;
                }
                else
                {
                    // Grab the original iParcel package so we can get it's iParcelPackageID, as we'll need to set it on the
                    // cloned package.  There's probably a better way, so need to check with Brian.
                    IParcelPackageEntity selectedPackageEntity = selectedShipment.IParcel.Packages[0];
                    long originalPackageID = selectedPackageEntity.IParcelPackageID;

                    // Set the rated shipment as the iParcel shipment
                    selectedShipment.IParcel = rateShipment;

                    // Update the first package iParcelPackgeID to be that of the original persisted package.  If this isn't 
                    // done, we get an ORM exception.  There's probably a better way, so need to check with Brian.
                    selectedShipment.IParcel.Packages[0].IParcelPackageID = originalPackageID;

                    // Set the shipment and package to be not new so a copy isn't persisted.
                    selectedShipment.IParcel.Packages[0].IsNew = false;
                    selectedShipment.IParcel.IsNew = false;

                }
            };
        }

        /// <summary>
        /// Updates the shipment settings.
        /// </summary>
        /// <param name="testRateShipment">The test rate shipment.</param>
        /// <param name="contentWeight">The content weight.</param>
        /// <param name="accountID">The account unique identifier.</param>
        private static void UpdateShipmentSettings(ShipmentEntity testRateShipment, double contentWeight, long accountID)
        {
            testRateShipment.IParcel.Packages[0].DimsHeight = testRateShipment.BestRate.DimsHeight;
            testRateShipment.IParcel.Packages[0].DimsWidth = testRateShipment.BestRate.DimsWidth;
            testRateShipment.IParcel.Packages[0].DimsLength = testRateShipment.BestRate.DimsLength;

            // ConfigureNewShipment sets these fields, but we need to make sure they're what we expect
            testRateShipment.IParcel.Packages[0].Weight = contentWeight;
            testRateShipment.IParcel.Packages[0].DimsAddWeight = false;
            testRateShipment.IParcel.Service = (int)iParcelServiceType.Saver;
            testRateShipment.IParcel.IParcelAccountID = accountID;
        }
    }
}
