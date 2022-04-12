using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.Stores.Platforms.Platform.OnlineUpdating
{
    /// <summary>
    /// Factory for getting an uploader for platform shipments
    /// </summary>
    [Component]
    public class UploadPlatformShipmentFactory : IUploadPlatformShipmentFactory
    {
        private readonly IIndex<UploadPlatformShipmentType, IUploadPlatformShipment> uploadShipmentImplementations;
        private readonly ILicenseService licenseService;

        /// <summary>
        /// Constructor
        /// </summary>
        public UploadPlatformShipmentFactory(IIndex<UploadPlatformShipmentType, IUploadPlatformShipment> uploadShipmentImplementations,
            ILicenseService licenseService)
        {
            this.uploadShipmentImplementations = uploadShipmentImplementations;
            this.licenseService = licenseService;
        }

        /// <summary>
        /// Get an UploadPlatformShipment implementation based on whether or not the customer is a Hub customer
        /// </summary>
        public IUploadPlatformShipment Create() =>
            licenseService.IsHub ?
                uploadShipmentImplementations[UploadPlatformShipmentType.ThroughHub] :
                uploadShipmentImplementations[UploadPlatformShipmentType.DirectToPlatform];
    }
}
