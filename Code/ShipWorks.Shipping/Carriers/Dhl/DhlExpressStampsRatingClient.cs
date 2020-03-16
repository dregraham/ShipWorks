using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using Interapptive.Shared.ComponentRegistration;
using System.Collections.Generic;
using ShipWorks.Shipping.Carriers.Dhl.API.Stamps;
using Autofac.Features.Indexed;
using System.Linq;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Dhl Express Stamps rating client
    /// </summary>
    [Component(RegistrationType.Self)]
    public class DhlExpressStampsRatingClient
    {
        private readonly IDhlExpressStampsWebClient dhlExpressStampsWebClient;
        private readonly IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressStampsRatingClient(IDhlExpressStampsWebClient dhlExpressStampsWebClient, IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeManager)
        {
            this.dhlExpressStampsWebClient = dhlExpressStampsWebClient;
            this.shipmentTypeManager = shipmentTypeManager;
        }

        /// <summary>
        /// Get rates from DHL Express via Stamps.com
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            try
            {
                var (rates, errors) = dhlExpressStampsWebClient.GetRates(shipment);

                RateGroup rateGroup = new RateGroup(FilterRatesByExcludedServices(shipment, rates));

                foreach (Exception error in errors)
                {
                    rateGroup.AddFootnoteFactory(new ExceptionsRateFootnoteFactory(ShipmentTypeCode.Usps, error));
                }

                return rateGroup;
            }
            catch (Exception ex)
            {
                throw new ShippingException(ex);
            }
        }

        /// <summary>
        /// Gets the filtered rates based on any excluded services configured for this postal shipment type.
        /// </summary>
        protected virtual IEnumerable<RateResult> FilterRatesByExcludedServices(ShipmentEntity shipment, IEnumerable<RateResult> rates)
        {
            IEnumerable<DhlExpressServiceType> availableServiceTypes = shipmentTypeManager[shipment.ShipmentTypeCode]
                    .GetAvailableServiceTypes()
                    .Cast<DhlExpressServiceType>();
           // List<RateResult> results = rates.Where(r => r.Tag is PostalRateSelection && availableServiceTypes.Contains(((PostalRateSelection) r.Tag).ServiceType)).ToList();

            return rates;
        }
    }
}
