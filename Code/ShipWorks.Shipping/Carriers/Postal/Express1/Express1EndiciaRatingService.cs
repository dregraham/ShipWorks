using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Rating service for Express1 for Endicia
    /// </summary>
    public class Express1EndiciaRatingService : EndiciaRatingService
    {
        public Express1EndiciaRatingService(
            IIndex<ShipmentTypeCode, IRatingService> ratingServiceFactory,
            IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeManager,
            IIndex<ShipmentTypeCode, ICarrierAccountRepository<EndiciaAccountEntity>> accountRepository,
            ILogEntryFactory logEntryFactory, 
            Func<string, ICertificateInspector> certificateInspectorFactory)
            : base(ratingServiceFactory, shipmentTypeManager, accountRepository, logEntryFactory, certificateInspectorFactory)
        {}

        /// <summary>
        /// Gets the type of the shipment.
        /// </summary>
        protected EndiciaShipmentType GetShipmentType(ShipmentEntity shipment) => 
            (EndiciaShipmentType)shipmentTypeManager[(ShipmentTypeCode)shipment.ShipmentType];
    }
}
