using System.Collections.Generic;

namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// View model for notifying the user of the change in insurance behavior
    /// </summary>
    public interface IInsuranceBehaviorChangeViewModel
    {
        /// <summary>
        /// Notify the user of the change
        /// </summary>
        void Notify(bool originalInsuranceSelection, bool newInsuranceSelection);

        /// <summary>
        /// Notify the user of the change
        /// </summary>
        void Notify(IDictionary<long, bool> originalInsuranceSelection, IDictionary<long, bool> newInsuranceSelection);
        
        /// <summary>
        /// Notify the user of the change
        /// </summary>
        void Notify(IDictionary<long, (bool before, bool after)> insuranceSelection);
    }
}