using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using log4net;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// Abstract Base of an InsureShipRequest
    /// </summary>
    public abstract class InsureShipRequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipRequestBase"/> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="affiliate">The affiliate.</param>
        protected InsureShipRequestBase(ShipmentEntity shipment, InsureShipAffiliate affiliate)
        {
            //todo call other constructor
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipRequestBase"/> class.
        /// </summary>
        /// <param name="responseFactory">The response factory.</param>
        /// <param name="shipment">The shipment.</param>
        /// <param name="affiliate">The affiliate.</param>
        /// <param name="log">The log.</param>
        protected InsureShipRequestBase(IInsureShipResponseFactory responseFactory, ShipmentEntity shipment, InsureShipAffiliate affiliate, ILog log)
        {
            this.ResponseFactory = responseFactory;
            this.Shipment = shipment;
            this.Affiliate = affiliate;
            this.Log = log;
        }

        /// <summary>
        /// Gets or sets the response factory.
        /// </summary>
        protected IInsureShipResponseFactory ResponseFactory
        {
            get; 
            private set;
        }

        /// <summary>
        /// Gets the log.
        /// </summary>
        protected ILog Log
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the shipment.
        /// </summary>
        public ShipmentEntity Shipment
        {
            get; 
            private set;
        }

        /// <summary>
        /// Gets or sets the affiliate.
        /// </summary>
        public InsureShipAffiliate Affiliate 
        { 
            get; 
            private set;
        }

        /// <summary>
        /// Gets or sets the raw response.
        /// </summary>
        /// <value>
        /// The raw response.
        /// </value>
        public virtual HttpResponse RawResponse
        {
            get; 
            set;
        }

        /// <summary>
        /// Submits this request to InsureShip
        /// </summary>
        public abstract IInsureShipResponse Submit();

        /// <summary>
        /// Gets the unique shipment identifier.
        /// </summary>
        public static string GetUniqueShipmentId(ShipmentEntity shipment)
        {
            return string.Format("{0}-{1}",shipment.OrderID, shipment.ShipmentID);    
        }
    }
}
