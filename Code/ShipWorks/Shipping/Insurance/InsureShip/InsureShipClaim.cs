using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Insurance.InsureShip.Net.Claim;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// A class encapsulating the logic to submit a claim to InsureShip via their API.
    /// </summary>
    [Component]
    public class InsureShipClaim : IInsureShipClaim
    {
        private readonly IInsureShipSettings settings;
        private readonly ILog log;
        private readonly IInsureShipSubmitClaimRequest createClaimRequest;
        private readonly IInsureShipClaimStatusRequest getStatusRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipClaim" /> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="affiliate">The affiliate.</param>
        /// <param name="requestFactory">The request factory.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="log">The log.</param>
        public InsureShipClaim(IInsureShipSubmitClaimRequest createClaimRequest, IInsureShipClaimStatusRequest getStatusRequest, IInsureShipSettings settings, Func<Type, ILog> createLog)
        {
            this.getStatusRequest = getStatusRequest;
            this.createClaimRequest = createClaimRequest;
            this.settings = settings;
            this.log = createLog(GetType());
        }

        /// <summary>
        /// Submits the claim to InsureShip and updates the shipment entity provided to the
        /// constructor with he updated claim data.
        /// </summary>
        /// <param name="claimType">Type of the claim.</param>
        /// <param name="description"></param>
        /// <param name="items">The items.</param>
        /// <param name="damageAmount">The damage amount.</param>
        public Result Submit(InsureShipClaimType claimType, ShipmentEntity shipment, Action<InsurancePolicyEntity> updatePolicy) =>
            IsShipmentEligibleToSubmitClaim(claimType, shipment)
                .Do(() =>
                {
                    shipment.InsurancePolicy.ClaimType = (int) claimType;
                    updatePolicy(shipment.InsurancePolicy);
                    log.InfoFormat("Submitting claim to InsureShip for shipment {0}.", shipment.ShipmentID);
                })
                .Bind(() => createClaimRequest.CreateInsuranceClaim(shipment))
                .Do(() => log.InfoFormat("Response code from InsureShip for claim submission on shipment {0} was successful.", shipment.ShipmentID))
                .OnFailure(ex => HandleException(ex, shipment));

        /// <summary>
        /// Handle exceptions
        /// </summary>
        private void HandleException(Exception ex, ShipmentEntity shipment)
        {
            string message = string.Format(
                           "An error occurred trying to submit a claim to InsureShip on shipment {0}.",
                           shipment.ShipmentID);

            log.Error(message, ex);
        }

        /// <summary>
        /// Determines whether a shipment is eligible for a claim to be submitted.
        /// </summary>
        private Result IsShipmentEligibleToSubmitClaim(InsureShipClaimType claimType, IShipmentEntity shipment)
        {
            if (shipment.InsurancePolicy?.CreatedWithApi != true)
            {
                // To keep the logic for submitting a claim simpler, don't allow any shipments through
                // that haven't had a claim submitted via the API.
                return Result.FromError(string.Format("A claim cannot be made through ShipWorks for this shipment. Please contact InsureShip at {0} for help submitting a claim.", settings.InsureShipPhoneNumber));
            }

            if (shipment.InsurancePolicy.ClaimID.HasValue)
            {
                return Result.FromError("A claim has already been made for this shipment.");
            }

            DateTime allowedSubmitClaimDate = shipment.ShipDate.Date + settings.ClaimSubmissionWaitingPeriod;
            if (shipment.Processed)
            {
                if (claimType == InsureShipClaimType.Damage || DateTime.Now > allowedSubmitClaimDate)
                {
                    // The appropriate amount of time has passed since the shipment was shipped
                    log.InfoFormat("Shipment {0} is eligible for submitting a claim to InsureShip.", shipment.ShipmentID);
                    return Result.FromSuccess();
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

            var userMessage = string.Format(
                "The shipment may still be in transit. You may submit a \"Lost\" or \"Stolen\" claim on or after {0}.",
                allowedSubmitClaimDate.ToString("MMMM dd, yyyy"));

            return new InsureShipException(userMessage);
        }

        /// <summary>
        /// Checks the status of this claim by issuing a request to InsureShip.
        /// </summary>
        /// <returns>The status obtained from InsureShip.</returns>
        /// <exception cref="InsureShipException">
        /// ShipWorks was unable to check the claim status for this shipment. A claim needs to be submitted first.
        /// or
        /// ShipWorks was not able to check the claim status for this shipment. Please try again or contact InsureShip to check the claim status.
        /// </exception>
        public GenericResult<string> CheckStatus(IShipmentEntity shipment) =>
            CanCheckStatus(shipment)
                .Do(() => log.InfoFormat("Checking claim status with InsureShip for shipment {0}.", shipment.ShipmentID))
                .Bind(() => getStatusRequest.GetClaimStatus(shipment))
                .Do(x => log.InfoFormat("Response code from InsureShip for claim status on shipment {0} was successful.", shipment.ShipmentID));

        /// <summary>
        /// Can the status of a claim be checked
        /// </summary>
        private Result CanCheckStatus(IShipmentEntity shipment) =>
            shipment.InsurancePolicy.ClaimID.HasValue ?
                Result.FromSuccess() :
                new InsureShipException("ShipWorks was unable to check the claim status for this shipment. A claim needs to be submitted first.");
    }
}
