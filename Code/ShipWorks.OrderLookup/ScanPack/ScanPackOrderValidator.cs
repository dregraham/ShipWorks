using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Settings;
using ShipWorks.Shipping.Services;

namespace ShipWorks.OrderLookup.ScanPack
{
    /// <summary>
    /// Validate scan pack orders
    /// </summary>
    [Component]
    public class ScanPackOrderValidator : IScanPackOrderValidator
    {
        private readonly ILicenseService licenseService;
        private readonly IMainForm mainForm;

        public ScanPackOrderValidator(ILicenseService licenseService, IMainForm mainForm)
        {
            this.licenseService = licenseService;
            this.mainForm = mainForm;
        }

        /// <summary>
        /// Can the order be processed
        /// </summary>
        public Result CanProcessShipment(OrderEntity order)
        {
            EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.Warehouse, null);

            if (restrictionLevel == EditionRestrictionLevel.None &&
                mainForm.UIMode == UIMode.OrderLookup &&
                !order.Verified)
            {
                return Result.FromError("This order must be scanned and packed before a label can be printed.");

            }

            return Result.FromSuccess();
        }
    }
}
