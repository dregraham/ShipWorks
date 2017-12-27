using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Usps specific Shipment preprocessor
    /// </summary>
    [KeyedComponent(typeof(IShipmentPreProcessor), ShipmentTypeCode.Usps)]
    public class UspsShipmentPreProcessor : IShipmentPreProcessor
    {
        private readonly IDefaultShipmentPreProcessor defaultPreProcessor;
        private readonly IUspsFirstClassInternationalShipmentValidator firstClassInternationalShipmentValidator;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsShipmentPreProcessor(IDefaultShipmentPreProcessor defaultPreProcessor, 
            IUspsFirstClassInternationalShipmentValidator firstClassInternationalShipmentValidator)
        {
            this.defaultPreProcessor = defaultPreProcessor;
            this.firstClassInternationalShipmentValidator = firstClassInternationalShipmentValidator;
        }

        /// <summary>
        /// Run the preprocessor for USPS
        /// </summary>
        public IEnumerable<ShipmentEntity> Run(ShipmentEntity shipment, RateResult selectedRate, Action configurationCallback)
        {
            // do this check after 1/21/218
            if (DateTime.Now >= new DateTime(2018, 01, 21))
            {
                firstClassInternationalShipmentValidator.ValidateShipment(shipment);
            }

            return defaultPreProcessor.Run(shipment, selectedRate, configurationCallback);
        }
    }
}
