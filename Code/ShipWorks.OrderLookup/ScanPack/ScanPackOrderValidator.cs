﻿using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Settings;
using ShipWorks.Shipping.Services;
using ShipWorks.SingleScan;

namespace ShipWorks.OrderLookup.ScanPack
{
    /// <summary>
    /// Validate scan pack orders
    /// </summary>
    [Component]
    public class ScanPackOrderValidator : IScanPackOrderValidator
    {
        private readonly ILicenseService licenseService;
        private readonly ISingleScanAutomationSettings singleScanAutomationSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScanPackOrderValidator(
            ILicenseService licenseService,
            ISingleScanAutomationSettings singleScanAutomationSettings)
        {
            this.licenseService = licenseService;
            this.singleScanAutomationSettings = singleScanAutomationSettings;
        }

        /// <summary>
        /// Can the order be processed
        /// </summary>
        public Result CanProcessShipment(OrderEntity order)
        {
            EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.Warehouse, null);

            if (restrictionLevel == EditionRestrictionLevel.None &&
                singleScanAutomationSettings.RequireVerificationToShip &&
                !order.Verified)
            {
                return Result.FromError("This order must be verified before a label can be printed.");
            }

            return Result.FromSuccess();
        }
    }
}
