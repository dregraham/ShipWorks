using System.Collections.Generic;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Manages and provides database access to the shipping provider rules.
    /// </summary>
    public interface IShippingProviderRuleManager : ICheckForChangesNeeded
    {
        /// <summary>
        /// Apply the given provider rule
        /// </summary>
        void SaveRule(ShippingProviderRuleEntity rule);

        /// <summary>
        /// Apply the given profile to the given shipment
        /// </summary>
        void SaveRule(ShippingProviderRuleEntity rule, SqlAdapter adapter);

        /// <summary>
        /// Delete the specified rule
        /// </summary>
        void DeleteRule(ShippingProviderRuleEntity rule);

        /// <summary>
        /// Return the active list of all rules for the given shipment type
        /// </summary>
        IEnumerable<ShippingProviderRuleEntity> GetRules();
    }
}