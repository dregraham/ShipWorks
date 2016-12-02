using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps.LabelRetrieval
{
    /// <summary>
    /// Validate whether a shipment type is allowed to be processed
    /// </summary>
    public class InsuranceValidator : ILabelRetrievalShipmentValidator
    {
        readonly IInsuranceUtility insuranceUtility;

        /// <summary>
        /// Constructor
        /// </summary>
        public InsuranceValidator(IInsuranceUtility insuranceUtility)
        {
            this.insuranceUtility = insuranceUtility;
        }

        /// <summary>
        /// Perform validation
        /// </summary>
        public Result Validate(ShipmentEntity shipment)
        {
            // This method should return validation results instead of throwing, but that would be a bigger refactoring
            // than we're doing here.  So for now, we'll just let it throw if it fails.
            insuranceUtility.ValidateShipment(shipment);

            return Result.FromSuccess();
        }
    }
}
