using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps.LabelRetrieval
{
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
            if (shipment.ShipmentTypeCode == ShipmentTypeCode.AmazonSFP)
            {
                var license = licenseService.GetLicense(shipment.Order.Store);

                return license.IsInTrial ?
                    Result.FromError("Amazon SFP shipments are not available during the ShipWorks trial period. Please go to https://hub.shipworks.com/account to add a credit card to your account.") :
                    Result.FromSuccess();    
            }

            return Result.FromSuccess();
        }
    }
}