using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps.GetLabel
{
    /// <summary>
    /// Check against the postal restriction for APO/FPO only
    /// </summary>
    public class POBoxLicenseValidator : IGetLabelValidator
    {
        private readonly ILicenseService licenseService;

        /// <summary>
        /// Constructor
        /// </summary>
        public POBoxLicenseValidator(ILicenseService licenseService)
        {
            this.licenseService = licenseService;
        }

        /// <summary>
        /// Perform validation
        /// </summary>
        public Result Validate(ShipmentEntity shipment)
        {
            EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.PostalApoFpoPoboxOnly, shipment);

            return restrictionLevel == EditionRestrictionLevel.None ?
                Result.FromSuccess() :
                Result.FromError("Your ShipWorks account is only enabled for using APO, FPO, and P.O. " +
                    "Box postal services.  Please contact Interapptive to enable use of all postal services.");
        }
    }
}
