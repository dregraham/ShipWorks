using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps.LabelRetrieval
{
    /// <summary>
    /// Validate that the user is allowed to process Amazon SFP shipments
    /// </summary>
    public class AmazonSfpValidator : ILabelRetrievalShipmentValidator
    {
        private readonly ILicenseService licenseService;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSfpValidator(ILicenseService licenseService)
        {
            this.licenseService = licenseService;
        }

        /// <summary>
        /// Perform validation
        /// </summary>
        public Result Validate(ShipmentEntity shipment)
        {
            if (shipment.ShipmentTypeCode != ShipmentTypeCode.AmazonSFP)
            {
                return Result.FromSuccess();
            }

            var license = licenseService.GetLicense(shipment.Order.Store);

            return license.TrialDetails.IsInTrial ?
                Result.FromError("Amazon Buy Shipping API shipments are not available during the ShipWorks trial period. Please go to https://hub.shipworks.com/account to add a credit card to your account.") :
                Result.FromSuccess();
        }
    }
}