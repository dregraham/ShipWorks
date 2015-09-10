using ShipWorks.Shipping.Insurance.InsureShip.Net;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using System;

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
        private readonly IInsureShipSettings insureShipSettings;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipPolicy"/> class.
        /// </summary>
        /// <param name="affiliate">The affiliate.</param>
        public InsureShipPolicy(InsureShipAffiliate affiliate)
            : this(affiliate, new InsureShipRequestFactory(), LogManager.GetLogger(typeof(InsureShipPolicy)), new InsureShipSettings())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipPolicy" /> class. This is constructor is primarily
        /// for testing purposes.
        /// </summary>
        /// <param name="affiliate">The affiliate.</param>
        /// <param name="requestFactory">The request factory.</param>
        /// <param name="log">The log to write diagnostic messages to.</param>
        /// <param name="insureShipSettings">The InsureShip settings.</param>
        public InsureShipPolicy(InsureShipAffiliate affiliate, IInsureShipRequestFactory requestFactory, ILog log, IInsureShipSettings insureShipSettings)
        {
            this.affiliate = affiliate;
            this.requestFactory = requestFactory;
            this.log = log;
            this.insureShipSettings = insureShipSettings;
        }

        /// <summary>
        /// Insures the shipment with InsureShip and sets the InsuredWith property of the shipment based
        /// on the response from InsureShip.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <exception cref="InsureShipException">This shipment was not insured. An error occurred trying to insure this shipment.</exception>
        public void Insure(ShipmentEntity shipment)
        {
            try
            {
                log.InfoFormat("Submitting shipment information to InsureShip for shipment {0}.", shipment.ShipmentID);
                InsureShipRequestBase request = requestFactory.CreateInsureShipmentRequest(shipment, affiliate);
                IInsureShipResponse response = request.Submit();

                log.InfoFormat("Processing response from InsureShip for shipment {0}.", shipment.ShipmentID);
                InsureShipResponseCode responseCode = response.Process();

                // This should never actually get here unless the response was successful, but log it just in case.
                log.InfoFormat("Response code from InsureShip was {0} successful (response code {1}).",
                    responseCode == InsureShipResponseCode.Success ? string.Empty : "not ", responseCode);
            }
            catch (InsureShipResponseException exception)
            {
                string message =
                    string.Format(
                        "An error occurred trying to insure shipment {0} with InsureShip. A(n) {1} response code was received from InsureShip.",
                        shipment.ShipmentID, exception.InsureShipResponseCode);
                log.Error(message, exception);
            }
            catch (InsureShipException exception)
            {
                log.Error(exception);
            }
        }

        /// <summary>
        /// Voids the policy associated with the given shipment if it has been insured via the InsureShip API. The 
        /// shipment should be fully populated prior to invoking this method.
        /// </summary>
        /// <param name="shipment">The shipment being voided.</param>
        public void Void(ShipmentEntity shipment)
        {
            if (IsVoidable(shipment))
            {
                try
                {
                    log.InfoFormat("Submitting void request to InsureShip for shipment {0}.", shipment.ShipmentID);
                    InsureShipRequestBase request = requestFactory.CreateVoidPolicyRequest(shipment, affiliate);
                    IInsureShipResponse response = request.Submit();

                    log.InfoFormat("Processing void response for shipment {0}.", shipment.ShipmentID);
                    InsureShipResponseCode responseCode = response.Process();

                    // This should never actually get here unless the response was successful, but log it just in case.
                    log.InfoFormat("Response code from InsureShip was {0} successful (response code {1}).",
                                   responseCode == InsureShipResponseCode.Success ? string.Empty : "not ", responseCode);
                }
                catch (InsureShipResponseException exception)
                {
                    string message = string.Format(
                        "An error occurred trying to void shipment {0} with the InsureShip API. A(n) {1} response code was received from InsureShip.",
                        shipment.ShipmentID, exception.InsureShipResponseCode);

                    log.Error(message, exception);
                    throw new InsureShipException(message, exception);
                }
            }
        }

        /// <summary>
        /// Determines whether the shipment can be voided via the InsureShip API.
        /// </summary>
        private bool IsVoidable(ShipmentEntity shipment)
        {
            bool voidable = false;
            
            if (shipment.Processed && shipment.InsurancePolicy != null && shipment.InsurancePolicy.CreatedWithApi)
            {
                // Shipment was insured with the API, so check whether the age of the policy falls within the 
                // grace period for voiding
                TimeSpan gracePeriod = insureShipSettings.VoidPolicyMaximumAge;
                if (DateTime.UtcNow.Subtract(shipment.ShipDate) < gracePeriod)
                {
                    log.InfoFormat("The policy for shipment {0} is eligible for voiding with the InsureShip API.", shipment.ShipmentID);
                    voidable = true;
                }
                else
                {
                    string errorMessage = string.Format("The policy for shipment {0} cannot be voided with the InsureShip API. The policy was created more than {1} hours ago.", 
                        shipment.ShipmentID,
                        gracePeriod.TotalHours);

                    InsureShipException insureShipException = new InsureShipException(errorMessage);

                    log.Info(errorMessage, insureShipException);
                    throw insureShipException;
                }
            }
            else
            {
                log.InfoFormat("Shipment {0} was not insured with the API.", shipment.ShipmentID);
            }

            return voidable;
        }
    }
}
