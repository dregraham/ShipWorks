using System;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip.Net;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// A class encapsulating the logic to submit a claim to InsureShip via their API.
    /// </summary>
    public class InsureShipClaim
    {
        private readonly ShipmentEntity shipment;
        private readonly InsureShipAffiliate affiliate;
        private readonly IInsureShipRequestFactory requestFactory;
        private readonly IInsureShipSettings settings;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipClaim" /> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="affiliate">The affiliate.</param>
        public InsureShipClaim(ShipmentEntity shipment, InsureShipAffiliate affiliate)
            : this(shipment, affiliate, new InsureShipRequestFactory(), new InsureShipSettings(), LogManager.GetLogger(typeof(InsureShipClaim)))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipClaim" /> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="affiliate">The affiliate.</param>
        /// <param name="requestFactory">The request factory.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="log">The log.</param>
        public InsureShipClaim(ShipmentEntity shipment, InsureShipAffiliate affiliate, IInsureShipRequestFactory requestFactory, IInsureShipSettings settings, ILog log)
        {
            this.shipment = shipment;
            this.affiliate = affiliate;
            this.requestFactory = requestFactory;
            this.settings = settings;
            this.log = log;

            if (shipment.InsurancePolicy == null || !shipment.InsurancePolicy.CreatedWithApi)
            {
                // To keep the logic for submitting a claim simpler, don't allow any shipments through 
                // that haven't had a claim submitted via the API.
                string message = string.Format("A claim cannot be made through ShipWorks for this shipment. Please contact InsureShip at {0} for help submitting a claim.", settings.InsureShipPhoneNumber);

                log.Error(message);
                throw new InsureShipException(message);
            }
        }

        /// <summary>
        /// Gets the type of the claim.
        /// </summary>
        public InsureShipClaimType ClaimType
        {
            get 
            { 
                return shipment.InsurancePolicy.ClaimType.HasValue ? 
                    (InsureShipClaimType)shipment.InsurancePolicy.ClaimType : InsureShipClaimType.Damage; 
            }
        }

        /// <summary>
        /// Gets the items being included in the claim.
        /// </summary>
        public string Items
        {
            get { return shipment.InsurancePolicy.ItemName; }
        }

        /// <summary>
        /// Gets the monetary amount of damage being claimed.
        /// </summary>
        public decimal DamageAmount
        {
            get
            {
                return shipment.InsurancePolicy.DamageValue.HasValue ? 
                    shipment.InsurancePolicy.DamageValue.Value : 0.00M;
            }
        }

        /// <summary>
        /// Submits the claim to InsureShip and updates the shipment entity provided to the
        /// constructor with he updated claim data.
        /// </summary>
        /// <param name="claimType">Type of the claim.</param>
        /// <param name="description"></param>
        /// <param name="items">The items.</param>
        /// <param name="damageAmount">The damage amount.</param>
        public void Submit(InsureShipClaimType claimType, string items, string description, decimal damageAmount)
        {   
            if (IsShipmentEligibleToSubmitClaim())
            {
                shipment.InsurancePolicy.ClaimType = (int) claimType;
                shipment.InsurancePolicy.ItemName = items;
                shipment.InsurancePolicy.DamageValue = damageAmount;
                shipment.InsurancePolicy.SubmissionDate = DateTime.UtcNow;
                shipment.InsurancePolicy.Description = description;

                try
                {
                    log.InfoFormat("Submitting claim to InsureShip for shipment {0}.", shipment.ShipmentID);
                    InsureShipRequestBase request = requestFactory.CreateSubmitClaimRequest(shipment, affiliate);
                    IInsureShipResponse response = request.Submit();

                    log.InfoFormat("Processing claim response from InsureShip for shipment {0}.", shipment.ShipmentID);
                    InsureShipResponseCode responseCode = response.Process();

                    // This should never actually get here unless the response was successful, but log it just in case.
                    log.InfoFormat("Response code from InsureShip for claim submission on shipment {0} was {1} successful (response code {2}).",
                                   shipment.ShipmentID, responseCode == InsureShipResponseCode.Success ? string.Empty : "not ", EnumHelper.GetApiValue(responseCode));
                }
                catch (InsureShipResponseException exception)
                {
                    string message = string.Format(
                        "An error occurred trying to submit a claim to InsureShip on shipment {0}. A(n) {1} response code was received from InsureShip.",
                        shipment.ShipmentID, exception.InsureShipResponseCode);

                    log.Error(message, exception);
                    throw new InsureShipException("ShipWorks was not able to submit a claim for this shipment. Please try again or contact InsureShip to file a claim.", exception);
                }
            }
            else
            {
                log.InfoFormat("Shipment {0} is not eligible for submitting a claim.", shipment.ShipmentID);
                throw new InsureShipException(
                    string.Format("A claim cannot be submitted for this shipment. At least {0} days must have passed since the ship date in order to submit a claim.", settings.ClaimSubmissionWaitingPeriod.TotalDays));
            }
        }

        /// <summary>
        /// Determines whether a shipment is eligible for a claim to be submitted.
        /// </summary>
        /// <returns></returns>
        private bool IsShipmentEligibleToSubmitClaim()
        {
            if (shipment.InsurancePolicy.ClaimID.HasValue)
            {
                log.ErrorFormat("A claim has already been submitted for shipment {0}", shipment.ShipmentID);
                throw new InsureShipException("A claim has already been made for this shipment.");
            }

            bool isEligible = false;

            if (shipment.Processed)
            {
                TimeSpan timeSinceShipDate = DateTime.UtcNow.Subtract(shipment.ShipDate);
                if (timeSinceShipDate > settings.ClaimSubmissionWaitingPeriod)
                {
                    // The appropriate amount of time has passed since the shipment was shipped
                    log.InfoFormat("Shipment {0} is eligible for submitting a claim to InsureShip.", shipment.ShipmentID);
                    isEligible = true;
                }
                else
                {
                    log.InfoFormat("A claim cannot be submitted for shipment {0}. It hasn't been {1} days since the ship date.", shipment.ShipmentID, settings.ClaimSubmissionWaitingPeriod.TotalDays);
                }
            }
            else
            {
                log.InfoFormat("Shipment {0} has not been processed. A claim cannot be submitted for an unprocessed shipment.", shipment.ShipmentID);
            }

            return isEligible;
        }
    }
}
