using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi
{
    /// <summary>
    /// Loader for 3dCart item attributes
    /// </summary>
    public interface IThreeDCartRestItemAttributeLoader
    {
        /// <summary>
        /// Loads the item attributes from the item description
        /// </summary>
        void LoadItemNameAndAttributes(ThreeDCartOrderItemEntity item, string itemDescription);
    }
}