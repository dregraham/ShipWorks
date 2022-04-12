namespace ShipWorks.Stores.Platforms.Platform.OnlineUpdating
{
    /// <summary>
    /// Factory for getting an uploader for platform shipments
    /// </summary>
    public interface IUploadPlatformShipmentFactory
    {
        /// <summary>
        /// Get an UploadPlatformShipment implementation based on whether or not the customer is a Hub customer
        /// </summary>
        IUploadPlatformShipment Create();
    }
}