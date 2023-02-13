using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Etsy
{
    /// <summary>
    /// Viewmodel to set up an Etsy Order Source
    /// </summary>
    public interface IEtsyCreateOrderSourceViewModel
    {
        /// <summary>
        /// Load the store
        /// </summary>
        void Load(EtsyStoreEntity store);
        
        /// <summary>
        /// Save the store
        /// </summary>
        bool Save(EtsyStoreEntity store);

        string EncodedOrderSource { get; set; }
    }
}