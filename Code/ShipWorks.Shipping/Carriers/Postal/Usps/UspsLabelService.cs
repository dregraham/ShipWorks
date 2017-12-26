using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Label Service for the USPS carrier
    /// </summary>
    public class UspsLabelService : ILabelService
    {
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly Func<Express1UspsLabelService> express1UspsLabelService;
        private readonly UspsRatingService uspsRatingService;
        private readonly Func<UspsLabelResponse, UspsDownloadedLabelData> createDownloadedLabelData;
        private readonly IUspsTermsAndConditions termsAndConditions;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsLabelService(IShipmentTypeManager shipmentTypeManager,
            Func<Express1UspsLabelService> express1UspsLabelService, 
            UspsRatingService uspsRatingService,
            Func<UspsLabelResponse, UspsDownloadedLabelData> createDownloadedLabelData,
            IUspsTermsAndConditions termsAndConditions)
        {
            this.shipmentTypeManager = shipmentTypeManager;
            this.express1UspsLabelService = express1UspsLabelService;
            this.uspsRatingService = uspsRatingService;
            this.createDownloadedLabelData = createDownloadedLabelData;
            this.termsAndConditions = termsAndConditions;
        }

        /// <summary>
        /// Creates a label for the given Shipment
        /// </summary>
        public async Task<IDownloadedLabelData> Create(ShipmentEntity shipment)
        {
            IDownloadedLabelData uspsDownloadedLabelData;

            UspsShipmentType uspsShipmentType = (UspsShipmentType) shipmentTypeManager.Get(ShipmentTypeCode.Usps);
            uspsShipmentType.ValidateShipment(shipment);
            try
            {
                if (uspsShipmentType.ShouldRateShop(shipment) || uspsShipmentType.ShouldTestExpress1Rates(shipment))
                {
                    uspsDownloadedLabelData = await ProcessShipmentWithRates(shipment).ConfigureAwait(false);
                }
                else
                {
                    termsAndConditions.Validate(shipment);

                    UspsLabelResponse uspsLabelResponse = await uspsShipmentType.CreateWebClient().ProcessShipment(shipment).ConfigureAwait(false);
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
                UspsShipmentType uspsShipmentType = (UspsShipmentType) shipmentTypeManager.Get(ShipmentTypeCode.Usps);

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
        private async Task<IDownloadedLabelData> ProcessShipmentWithRates(ShipmentEntity shipment)
        {
            IDownloadedLabelData uspsDownloadedLabelData = null;

            UspsShipmentType uspsShipmentType = (UspsShipmentType) shipmentTypeManager.Get(ShipmentTypeCode.Usps);

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

                        ShipmentType express1UspsShipmentType = shipmentTypeManager.Get(ShipmentTypeCode.Express1Usps);
                        express1UspsShipmentType.UpdateDynamicShipmentData(shipment);

                        uspsDownloadedLabelData = await express1UspsLabelService().Create(shipment).ConfigureAwait(false);
                    }
                    else
                    {
                        termsAndConditions.Validate(shipment);

                        uspsShipmentType.UseAccountForShipment(account, shipment);

                        IUspsWebClient client = uspsShipmentType.CreateWebClient();
                        UspsLabelResponse uspsLabelResponse = await client.ProcessShipment(shipment).ConfigureAwait(false);

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