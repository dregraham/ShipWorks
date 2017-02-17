using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps.LabelRetrieval
{
    /// <summary>
    /// Validate whether a shipment type is allowed to be processed
    /// </summary>
    public class AllowedShipmentTypeValidator : ILabelRetrievalShipmentValidator
    {
        private readonly ILicenseService licenseService;

        /// <summary>
        /// Constructor
        /// </summary>
        public AllowedShipmentTypeValidator(ILicenseService licenseService)
        {
            this.licenseService = licenseService;
        }

        /// <summary>
        /// Perform validation
        /// </summary>
        public Result Validate(ShipmentEntity shipment)
        {
            EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.ProcessShipment, shipment.ShipmentTypeCode);

            return restrictionLevel == EditionRestrictionLevel.Forbidden ?
                Result.FromError($"ShipWorks can no longer process {EnumHelper.GetDescription(shipment.ShipmentTypeCode)} shipments. Please try using USPS.") :
                Result.FromSuccess();
        }
    }
}
