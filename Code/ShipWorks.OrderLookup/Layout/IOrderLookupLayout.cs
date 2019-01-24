using Autofac;
using ShipWorks.OrderLookup.Controls.OrderLookup;

namespace ShipWorks.OrderLookup.Layout
{
    /// <summary>
    /// Represents the OrderLookup Layout
    /// </summary>
    public interface IOrderLookupLayout
    {
        /// <summary>
        /// Apply the layout to the view model
        /// </summary>
        void Apply(IMainOrderLookupViewModel orderLookupViewModel, ILifetimeScope scope);

        /// <summary>
        /// Save the view model
        /// </summary>
        void Save(IMainOrderLookupViewModel orderLookupViewModel);
    }
}