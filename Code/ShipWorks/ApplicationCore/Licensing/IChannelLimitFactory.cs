namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for ChannelLimitFactory
    /// </summary>
    public interface IChannelLimitFactory
    {
        /// <summary>
        /// Creates the control
        /// </summary>
        IChannelLimitControl CreateControl(ICustomerLicense customerLicense);
    }
}