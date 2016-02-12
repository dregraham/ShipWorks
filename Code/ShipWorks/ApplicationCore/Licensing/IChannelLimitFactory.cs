using System.Windows.Controls;
using ShipWorks.Stores;

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

        /// <summary>
        /// Creates the control
        /// </summary>
        /// <remarks>
        /// This instance will take the store being added into account
        /// It shouldn't be available to delete and be counted against the customer's limit.
        /// </remarks>
        IChannelLimitControl CreateControl(ICustomerLicense customerLicense, StoreTypeCode channelToAdd);
    }
}