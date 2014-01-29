using System;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Parameters that can be used by footnotes to interact with the rate grid
    /// </summary>
    public class FootnoteParameters
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reloadRatesAction">Action that will cause rates to reload</param>
        /// <param name="getStoreTypeAction">Function that will get the current store</param>
        public FootnoteParameters(Action reloadRatesAction, Func<StoreType> getStoreTypeAction)
        {
            ReloadRatesAction = reloadRatesAction;
            GetStoreTypeAction = getStoreTypeAction;
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
        public Func<StoreType> GetStoreTypeAction
        {
            get;
            private set;
        }
    }
}
