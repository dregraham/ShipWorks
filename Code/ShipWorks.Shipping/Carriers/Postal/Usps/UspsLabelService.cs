using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Label Service for the USPS carrier
    /// </summary>
    public class UspsLabelService : ILabelService
    {
        private readonly UspsShipmentType uspsShipmentType;
        private readonly Func<Express1UspsShipmentType> express1UspsShipmentType;
        private readonly Func<Express1UspsLabelService> express1UspsLabelService;
        private readonly UspsRatingService uspsRatingService;
        private readonly Func<UspsLabelResponse, UspsDownloadedLabelData> createDownloadedLabelData;
         
        /// <summary>
        ///     Constructor
        /// </summary>
        public UspsLabelService(UspsShipmentType uspsShipmentType,
            Func<Express1UspsShipmentType> express1UspsShipmentType,
            Func<Express1UspsLabelService> express1UspsLabelService, 
            UspsRatingService uspsRatingService,
            Func<UspsLabelResponse, UspsDownloadedLabelData> createDownloadedLabelData)
        {
            this.uspsShipmentType = uspsShipmentType;
            this.express1UspsShipmentType = express1UspsShipmentType;
            this.express1UspsLabelService = express1UspsLabelService;
            this.uspsRatingService = uspsRatingService;
            this.createDownloadedLabelData = createDownloadedLabelData;
        }

        /// <summary>
        /// Creates a label for the given Shipment
        /// </summary>
        public IDownloadedLabelData Create(ShipmentEntity shipment)
        {
            IDownloadedLabelData uspsDownloadedLabelData;

            uspsShipmentType.ValidateShipment(shipment);
            try
            {
                if (uspsShipmentType.ShouldRateShop(shipment) || uspsShipmentType.ShouldTestExpress1Rates(shipment))
                {
                    uspsDownloadedLabelData = ProcessShipmentWithRates(shipment);
                }
                else
                {
                    UspsLabelResponse uspsLabelResponse = uspsShipmentType.CreateWebClient().ProcessShipment(shipment);
                    uspsDownloadedLabelData = createDownloadedLabelData(uspsLabelResponse);
                }
            }
            catch (UspsException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
            catch (AddressValidationException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }

            return uspsDownloadedLabelData;
        }

        /// <summary>
        /// Voids the given Shipment
        /// </summary>
        public void Void(ShipmentEntity shipment)
        {
            try
            {
                uspsShipmentType.CreateWebClient().VoidShipment(shipment);
            }
            catch (UspsException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Process the shipment using the account with the cheapest rate for the requested service
        /// </summary>
        private IDownloadedLabelData ProcessShipmentWithRates(ShipmentEntity shipment)
        {
            IDownloadedLabelData uspsDownloadedLabelData = null;

            IUspsWebClient client = uspsShipmentType.CreateWebClient();
            IEnumerable<UspsAccountEntity> accounts = uspsRatingService.GetRates(shipment).Rates
                    .OrderBy(x => x.AmountOrDefault)
                    .Select(x => x.OriginalTag)
                    .OfType<UspsPostalRateSelection>()
                    .Where(x => x.IsRateFor(shipment))
                    .Select(x => x.Accounts)
                    .FirstOrDefault();

            if (accounts == null)
            {
                throw new UspsException("Could not get rates for the specified service type");
            }

            foreach (UspsAccountEntity account in accounts.ToList())
            {
                try
                {
                    if (account.UspsReseller == (int) UspsResellerType.Express1)
                    {
                        shipment.ShipmentType = (int) ShipmentTypeCode.Express1Usps;

                        shipment.Postal.Usps.OriginalUspsAccountID = shipment.Postal.Usps.UspsAccountID;
                        uspsShipmentType.UseAccountForShipment(account, shipment);

                        express1UspsShipmentType().UpdateDynamicShipmentData(shipment);
                        uspsDownloadedLabelData = express1UspsLabelService().Create(shipment);
                    }
                    else
                    {
                        uspsShipmentType.UseAccountForShipment(account, shipment);
                        UspsLabelResponse uspsLabelResponse = client.ProcessShipment(shipment);
                        uspsDownloadedLabelData = createDownloadedLabelData(uspsLabelResponse);
                    }

                    break;
                }
                catch (UspsInsufficientFundsException)
                {
                    if (ReferenceEquals(account, accounts.Last()))
                    {
                        throw;
                    }
                }
            }

            return uspsDownloadedLabelData;
        }
    }
}