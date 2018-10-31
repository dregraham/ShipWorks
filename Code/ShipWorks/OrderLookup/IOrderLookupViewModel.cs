using System;
using System.ComponentModel;
using ShipWorks.OrderLookup.FieldManager;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Generic view model for order lookup
    /// </summary>
    public interface IOrderLookupViewModel : IOrderLookupFieldLayoutProviderHost, INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Title of the section
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Is the section visible
        /// </summary>
        bool Visible { get; }

        /// <summary>
        /// Panel ID 
        /// </summary>
        SectionLayoutIDs PanelID { get; }
    }
}