using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Editing.Rating
{
    /// <summary>
    /// Parameters that can be used by footnotes to interact with the rate grid
    /// </summary>
    public class FootnoteParameters : IFootnoteParameters
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reloadRatesAction">Action that will cause rates to reload</param>
        /// <param name="getStoreAction">Function that will get the current store</param>
        public FootnoteParameters(Action reloadRatesAction, Func<StoreEntity> getStoreAction)
        {
            ReloadRatesAction = reloadRatesAction;
            GetStoreAction = getStoreAction;
        }

        /// <summary>
        /// Reload the rates in the rates grid
        /// </summary>
        public Action ReloadRatesAction
        {
            get; 
            private set;
        }

        /// <summary>
        /// Get the current store
        /// </summary>
        public Func<StoreEntity> GetStoreAction
        {
            get;
            private set;
        }
    }
}
