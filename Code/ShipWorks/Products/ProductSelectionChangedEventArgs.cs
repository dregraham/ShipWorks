using System;

namespace ShipWorks.Products
{
    /// <summary>
    /// Delegate for the ProductSelectionChanged event
    /// </summary>
    public delegate void ProductSelectionChangedEventHandler(object sender, ProductSelectionChangedEventArgs e);

    /// <summary>
    /// Event data for the ProductSelectionChanged event
    /// </summary>
    public class ProductSelectionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of the class
        /// </summary>
        public ProductSelectionChangedEventArgs(bool singleSelection)
        {
            SingleSelection = singleSelection;
        }

        /// <summary>
        /// Whether or not there is only one product selected
        /// </summary>
        public bool SingleSelection { get; private set; }
    }
}
