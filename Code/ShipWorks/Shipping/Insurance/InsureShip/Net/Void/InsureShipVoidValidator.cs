using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Void
{
    /// <summary>
    /// Validates whether a policy can be voided
    /// </summary>
    [Component]
    public class InsureShipVoidValidator : IInsureShipVoidValidator
    {
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public InsureShipVoidValidator(IDateTimeProvider dateTimeProvider, Func<Type, ILog> createLog)
        {
            this.dateTimeProvider = dateTimeProvider;
            log = createLog(GetType());
        }

        /// <summary>
        /// Is the policy voidable
        /// </summary>
        public GenericResult<bool> IsVoidable(IShipmentEntity shipment)
        {
            if (!shipment.Processed || shipment.InsurancePolicy == null || !shipment.InsurancePolicy.CreatedWithApi)
            {
                log.InfoFormat("Shipment {0} was not insured with the API.", shipment.ShipmentID);
                return false;
            }

            // Shipment was insured with the API, so check whether the age of the policy falls within the
            // grace period for voiding
            TimeSpan gracePeriod = InsureShipSettings.VoidPolicyMaximumAge;
            if (dateTimeProvider.UtcNow.Subtract(shipment.ShipDate) < gracePeriod)
            {
                log.InfoFormat("The policy for shipment {0} is eligible for voiding with the InsureShip API.", shipment.ShipmentID);
                return true;
            }

            string errorMessage = string.Format("The policy for shipment {0} cannot be voided with the InsureShip API. The policy was created more than {1} hours ago.",
                shipment.ShipmentID,
                gracePeriod.TotalHours);

            InsureShipException insureShipException = new InsureShipException(errorMessage);

            log.Info(errorMessage, insureShipException);
            return insureShipException;
        }
    }
}