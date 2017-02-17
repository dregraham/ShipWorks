using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps.LabelRetrieval
{
    /// <summary>
    /// Ensure license allows specified carrier
    /// </summary>
    public class EditionLicenseValidator : ILabelRetrievalShipmentValidator
    {
        private readonly ILicenseService licenseService;

        /// <summary>
        /// Constructor
        /// </summary>
        public EditionLicenseValidator(ILicenseService licenseService)
        {
            this.licenseService = licenseService;
        }

        /// <summary>
        /// Perform validation
        /// </summary>
        public Result Validate(ShipmentEntity shipment)
        {
            EditionRestrictionLevel restrictionLevel =
                licenseService.CheckRestriction(EditionFeature.ShipmentType, shipment.ShipmentTypeCode);

            return restrictionLevel == EditionRestrictionLevel.None ?
                Result.FromSuccess() :
                Result.FromError($"Your edition of ShipWorks does not support shipping with '{EnumHelper.GetDescription(shipment.ShipmentTypeCode)}'.");
        }
    }
}
