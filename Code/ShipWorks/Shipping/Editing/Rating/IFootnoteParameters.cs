using ShipWorks.Data.Model.EntityClasses;
using System;

namespace ShipWorks.Shipping.Editing.Rating
{
    /// <summary>
    /// Parameters that can be used by footnotes to interact with the rate grid
    /// </summary>
    public interface IFootnoteParameters
    {
        /// <summary>
        /// Get the current store
        /// </summary>
        Func<StoreEntity> GetStoreAction { get; }

        /// <summary>
        /// Reload the rates
        /// </summary>
        Action ReloadRatesAction { get; }
    }
}