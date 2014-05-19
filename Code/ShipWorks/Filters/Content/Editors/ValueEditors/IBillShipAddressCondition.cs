using ShipWorks.Filters.Content.Conditions;

namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    /// <summary>
    /// Defines the basic interface for a condition that can be used as a BillShipAddress condition
    /// </summary>
    public interface IBillShipAddressCondition
    {
        /// <summary>
        /// Operator that should be used to compare the billing and/or shipping address with each other or a value
        /// </summary>
        BillShipAddressOperator AddressOperator { get; set; }
    }
}