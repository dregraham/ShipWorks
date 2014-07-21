using ShipWorks.Shipping.Insurance.InsureShip.Net;
using log4net;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// An insurance policy through InsureShip. 
    /// </summary>
    public class InsureShipPolicy
    {
        private readonly InsureShipAffiliate affiliate;
        private readonly IInsureShipRequestFactory requestFactory;
        private readonly ILog log;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipPolicy"/> class.
        /// </summary>
        /// <param name="affiliate">The affiliate.</param>
        public InsureShipPolicy(InsureShipAffiliate affiliate)
            : this(affiliate, new InsureShipRequestFactory(), LogManager.GetLogger(typeof(InsureShipPolicy)))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipPolicy" /> class. This is constructor is primarily 
        /// for testing purposes.
        /// </summary>
        /// <param name="affiliate">The affiliate.</param>
        /// <param name="requestFactory">The request factory.</param>
        /// <param name="log">The log to write diagnostic messages to.</param>
        public InsureShipPolicy(InsureShipAffiliate affiliate, IInsureShipRequestFactory requestFactory, ILog log)
        {
            this.affiliate = affiliate;
            this.requestFactory = requestFactory;
            this.log = log;            
        }

        /// <summary>
        /// Insures the shipment with InsureShip.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns></returns>
        /// <exception cref="InsureShipException">This shipment was not insured. An error occurred trying to insure this shipment.</exception>
        public bool Insure(ShipmentEntity shipment)
        {
            bool success = false;

            try
            {
                log.InfoFormat("Submitting shipment information to InsureShip for shipment {0}.", shipment.ShipmentID);
                InsureShipRequestBase request = requestFactory.CreateInsureShipmentRequest(shipment, affiliate);
                IInsureShipResponse response = request.Submit();

                log.InfoFormat("Processing response from InsureShip for shipment {0}.", shipment.ShipmentID);
                InsureShipResponseCode responseCode = response.Process();

                // This should never actually get here unless the response was successful, but log it just in case.
                log.InfoFormat("Response code from InsureShip was {0}successful (response code {1}).", responseCode == InsureShipResponseCode.Success ? string.Empty : "not ", responseCode);
                success = true;
            }
            catch (InsureShipResponseException exception)
            {
                string message = string.Format("An error occurred trying to insure shipment {0} with InsureShip. A(n) {1} response code was received from InsureShip.", shipment.ShipmentID, exception.InsureShipResponseCode);
                log.Error(message, exception);
            }

            return success;
        }
    }
}
