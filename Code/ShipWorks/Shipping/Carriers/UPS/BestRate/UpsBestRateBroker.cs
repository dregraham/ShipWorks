using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.UPS.BestRate
{
    /// <summary>
    /// Rate broker that finds the best rates for UPS accounts
    /// </summary>
    public class UpsBestRateBroker : IBestRateShippingBroker
    {
        private readonly UpsShipmentType shipmentType;
        private readonly ICarrierAccountRepository<UpsAccountEntity> accountRepository;

        /// <summary>
        /// Creates a broker with the default shipment type and account repository
        /// </summary>
        /// <remarks>This is designed to be used within ShipWorks</remarks>
        public UpsBestRateBroker() : this(new UpsOltShipmentType(), new UpsAccountRepository())
        {
        }

        /// <summary>
        /// Creates a broker with the specified shipment type and account repository
        /// </summary>
        /// <param name="shipmentType">Instance of a UPS shipment type that will be used to get rates</param>
        /// <param name="accountRepository">Instance of an account repository that will get UPS accounts</param>
        /// <remarks>This is designed to be used by tests</remarks>
        public UpsBestRateBroker(UpsShipmentType shipmentType, ICarrierAccountRepository<UpsAccountEntity> accountRepository)
        {
            this.shipmentType = shipmentType;
            this.accountRepository = accountRepository;
        }
        /// <summary>
        /// Gets the single best rate for each UPS account based 
        /// on the configuration of the best rate shipment data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A list of RateResults composed of the single best rate for each UPS account.</returns>
        public List<RateResult> GetBestRates(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            List<RateResult> accountRates = new List<RateResult>();
            List<UpsAccountEntity> upsAccounts = accountRepository.Accounts.ToList();

            // Save a reference to the current UPS shipment, if there is one, since we'll be overwriting it
            UpsShipmentEntity originalUpsShipment = shipment.Ups;

            foreach (UpsAccountEntity account in upsAccounts)
            {
                // Create the UpsShipment that will be used to get rates
                shipment.Ups = new UpsShipmentEntity { UpsAccountID = account.UpsAccountID };
                shipmentType.ConfigureNewShipment(shipment);

                try
                {
                    RateGroup rates = shipmentType.GetRates(shipment);
                    RateResult lowestRate = rates.Rates.Where(r => r.Amount > 0).OrderBy(r => r.Amount).FirstOrDefault();

                    if (lowestRate != null)
                    {
                        lowestRate.HoverText = "Ups - " + lowestRate.Description;
                        accountRates.Add(lowestRate);
                    }
                }
                catch (ShippingException)
                {
                    // We will handle exceptions in a future story.  For now, just eat them.
                }
            }

            // Restore the original ups version of this shipment
            shipment.Ups = originalUpsShipment;

            return accountRates;
        }
    }
}
