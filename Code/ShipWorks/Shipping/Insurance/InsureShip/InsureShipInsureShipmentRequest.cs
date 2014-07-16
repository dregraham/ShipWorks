using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// InsureShip request class for insuring a shipment
    /// </summary>
    public class InsureShipInsureShipmentRequest : InsureShipRequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipRequestBase"/> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="affiliate">The affiliate.</param>
        public InsureShipInsureShipmentRequest(ShipmentEntity shipment, InsureShipAffiliate affiliate) : base(shipment, affiliate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipRequestBase"/> class.
        /// </summary>
        /// <param name="responseFactory">The response factory.</param>
        /// <param name="shipment">The shipment.</param>
        /// <param name="affiliate">The affiliate.</param>
        /// <param name="log">The log.</param>
        public InsureShipInsureShipmentRequest(IInsureShipResponseFactory responseFactory, ShipmentEntity shipment, InsureShipAffiliate affiliate, ILog log) : base(responseFactory, shipment, affiliate, log)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Submits this request to InsureShip
        /// </summary>
        public override IInsureShipResponse Submit()
        {
            throw new NotImplementedException();
        }
    }
}
