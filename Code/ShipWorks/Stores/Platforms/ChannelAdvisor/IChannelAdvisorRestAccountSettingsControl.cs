using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Represents the ChannelAdvisor Account settings control from Rest store
    /// </summary>
    public interface IChannelAdvisorRestAccountSettingsControl
    {
        /// <summary>
        /// Load the store into the control
        /// </summary>
        void LoadStore(StoreEntity store);

        /// <summary>
        /// Save to the store entity
        /// </summary>
        bool SaveToEntity(StoreEntity store);
    }
}