using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Label Service for the USPS carrier
    /// </summary>
    public class UspsLabelService : ILabelService
    {
        private readonly IIndex<ShipmentTypeCode, IUspsShipmentType> uspsShipmentTypes;
        private readonly IIndex<ShipmentTypeCode, ILabelService> labelServices;
        private readonly IUspsRatingService uspsRatingService;
        private readonly Func<UspsLabelResponse, UspsDownloadedLabelData> createDownloadedLabelData;
        private readonly IUspsTermsAndConditions termsAndConditions;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsLabelService(IIndex<ShipmentTypeCode, IUspsShipmentType> uspsShipmentTypes,
            IIndex<ShipmentTypeCode, ILabelService> labelServices,
            IUspsRatingService uspsRatingService,
            Func<UspsLabelResponse, UspsDownloadedLabelData> createDownloadedLabelData,
            IUspsTermsAndConditions termsAndConditions)
        {
            this.uspsShipmentTypes = uspsShipmentTypes;
            this.labelServices = labelServices;

            this.uspsRatingService = uspsRatingService;
            this.createDownloadedLabelData = createDownloadedLabelData;
            this.termsAndConditions = termsAndConditions;
        }

        /// <summary>
        /// Creates a label for the given Shipment
        /// </summary>
        public async Task<TelemetricResult<IDownloadedLabelData>> Create(ShipmentEntity shipment)
        {
            TelemetricResult<IDownloadedLabelData> telemetricResult = new TelemetricResult<IDownloadedLabelData>("API.ResponseTimeInMilliseconds");
            
            IUspsShipmentType uspsShipmentType = uspsShipmentTypes[ShipmentTypeCode.Usps];
            uspsShipmentType.ValidateShipment(shipment);

            try
            {
                if (uspsShipmentType.ShouldRateShop(shipment) || uspsShipmentType.ShouldTestExpress1Rates(shipment))
                {
                    TelemetricResult<IDownloadedLabelData> uspsDownloadedLabelData = await ProcessShipmentWithRates(shipment).ConfigureAwait(false);
                    return uspsDownloadedLabelData;
                }
                else
                {
                    termsAndConditions.Validate(shipment);

                    TelemetricResult<UspsLabelResponse> telemetricLabelResponse =
                        await uspsShipmentType.CreateWebClient().ProcessShipment(shipment).ConfigureAwait(false);
                    
                    telemetricLabelResponse.CopyTo(telemetricResult);

                    IDownloadedLabelData uspsDownloadedLabelData = createDownloadedLabelData(telemetricLabelResponse.Value);
                    telemetricResult.SetValue(uspsDownloadedLabelData);
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

            return telemetricResult;
        }

        /// <summary>
        /// Voids the given Shipment
        /// </summary>
        public void Void(ShipmentEntity shipment)
        {
            try
            {
                IUspsShipmentType uspsShipmentType = uspsShipmentTypes[ShipmentTypeCode.Usps];

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
        private async Task<TelemetricResult<IDownloadedLabelData>> ProcessShipmentWithRates(ShipmentEntity shipment)
        {
            TelemetricResult<IDownloadedLabelData> telemetricResult = new TelemetricResult<IDownloadedLabelData>("API.ResponseTimeInMilliseconds");

            IUspsShipmentType uspsShipmentType = uspsShipmentTypes[ShipmentTypeCode.Usps];

            IEnumerable<IUspsAccountEntity> accounts = uspsRatingService.GetRates(shipment, telemetricResult).Rates
                    .OrderBy(x => x.AmountOrDefault)
                    .Select(x => x.OriginalTag)
                    .OfType<IUspsPostalRateSelection>()
                    .Where(x => x.IsRateFor(shipment))
                    .Select(x => x.Accounts)
                    .FirstOrDefault();

            if (accounts == null)
            {
                throw new UspsException("Could not get rates for the specified service type");
            }

            foreach (IUspsAccountEntity account in accounts.ToList())
            {
                try
                {
                    if (account.UspsReseller == (int) UspsResellerType.Express1)
                    {
                        shipment.ShipmentType = (int) ShipmentTypeCode.Express1Usps;

                        shipment.Postal.Usps.OriginalUspsAccountID = shipment.Postal.Usps.UspsAccountID;
                        uspsShipmentType.UseAccountForShipment(account, shipment);

                        IUspsShipmentType express1UspsShipmentType = uspsShipmentTypes[ShipmentTypeCode.Express1Usps];
                        express1UspsShipmentType.UpdateDynamicShipmentData(shipment);

                        TelemetricResult<IDownloadedLabelData> uspsDownloadedLabelData =
                            await labelServices[ShipmentTypeCode.Express1Usps].Create(shipment).ConfigureAwait(false);
                        telemetricResult.CopyFrom(uspsDownloadedLabelData, true);
                    }
                    else
                    {
                        termsAndConditions.Validate(shipment);

                        uspsShipmentType.UseAccountForShipment(account, shipment);

                        IUspsWebClient client = uspsShipmentType.CreateWebClient();
                        
                        TelemetricResult<UspsLabelResponse> telemetricUspsLabelResponse = await client.ProcessShipment(shipment).ConfigureAwait(false);
                        
                        telemetricUspsLabelResponse.CopyTo(telemetricResult);
                        telemetricResult.SetValue(createDownloadedLabelData(telemetricUspsLabelResponse.Value));
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

            return telemetricResult;
        }
    }
}