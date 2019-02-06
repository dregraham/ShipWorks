using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// Validator for making an InsureShip claim
    /// </summary>
    [Component]
    public class InsureShipClaimValidator : IInsureShipClaimValidator
    {
        private readonly IInsureShipSettings settings;
        private readonly ILog log;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public InsureShipClaimValidator(
            IInsureShipSettings settings,
            IDateTimeProvider dateTimeProvider,
            Func<Type, ILog> createLog)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.settings = settings;
            log = createLog(GetType());
        }

        /// <summary>
        /// Determines whether a shipment is eligible for a claim to be submitted.
        /// </summary>
        public Result IsShipmentEligibleToSubmitClaim(InsureShipClaimType claimType, IShipmentEntity shipment)
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
                if (claimType == InsureShipClaimType.Damage || dateTimeProvider.Now > allowedSubmitClaimDate)
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
    }
}