using System;
using System.ComponentModel;
using System.Reflection;
using Autofac;
using ShipWorks.OrderLookup.FieldManager;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Basic Order Lookup view model
    /// </summary>
    [Obfuscation(Exclude = true, StripAfterObfuscation = true, ApplyToMembers = true)]
    public interface IOrderLookupPanelViewModel<out T> : INotifyPropertyChanged where T : IOrderLookupViewModel
    {
        /// <summary>
        /// Carrier specific context
        /// </summary>
        T Context { get; }

        /// <summary>
        /// Update the view model with new inner view models if necessary
        /// </summary>
        void UpdateViewModel(IOrderLookupShipmentModel shipmentModel, ILifetimeScope innerScope, Func<SectionLayoutIDs, bool> isPanelVisible);

        /// <summary>
        /// Name of the panel
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Whether or not the panel is expanded
        /// </summary>
        bool Expanded { get; set; }

        /// <summary>
        /// Whether or not the panel is visible
        /// </summary>
        bool Visible { get; set; }
    }
}
