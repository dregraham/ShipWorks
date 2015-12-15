using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac.Features.Indexed;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    public class Express1EndiciaRatingService : EndiciaRatingService
    {
        public Express1EndiciaRatingService(
            IIndex<ShipmentTypeCode, IRatingService> ratingServiceFactory,
            IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeFactory,
            IIndex<ShipmentTypeCode, ICarrierAccountRepository<EndiciaAccountEntity>> accountRepository,
            LogEntryFactory logEntryFactory, Func<string, ICertificateInspector> certificateInspectorFactory)
            : base(ratingServiceFactory, shipmentTypeFactory, accountRepository, logEntryFactory, certificateInspectorFactory)
        {}

        /// <summary>
        /// Gets the type of the shipment.
        /// </summary>
        protected EndiciaShipmentType GetShipmentType(ShipmentEntity shipment) => 
            (EndiciaShipmentType)shipmentTypeFactory[(ShipmentTypeCode)shipment.ShipmentType];

        /// <summary>
        /// Gets the filtered rates based on any excluded services configured for this postal shipment type.
        /// </summary>
        protected override List<RateResult> FilterRatesByExcludedServices(ShipmentEntity shipment, List<RateResult> rates)
        {
            List<PostalServiceType> availableServiceTypes = GetShipmentType(shipment).GetAvailableServiceTypes().Select(s => (PostalServiceType)s).ToList(); ;

            if (shipment.Postal.Endicia.OriginalEndiciaAccountID == null)
            {
                availableServiceTypes.Add((PostalServiceType)shipment.Postal.Service);
            }

            List<RateResult> rateResults = rates.Where(r => r.Tag is PostalRateSelection && availableServiceTypes.Contains(((PostalRateSelection)r.Tag).ServiceType)).ToList();

            rateResults.ForEach(r => r.ShipmentType = ShipmentTypeCode.Express1Endicia);

            return rateResults;
        }
    }
}
