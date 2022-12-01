using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Etsy
{
	/// <summary>
	/// Interface for an EtsyAccountSettingsViewModel
	/// </summary>
	public interface IEtsyAccountSettingsViewModel
	{
        /// <summary>
        /// Load the viewmodel from a store
        /// </summary>
        void Load(IEtsyStoreEntity etsyStoreEntity);
    }
}