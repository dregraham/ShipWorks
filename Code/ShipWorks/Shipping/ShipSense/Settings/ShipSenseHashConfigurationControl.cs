using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.UI.Controls;
using ShipWorks.UI.Utility;

namespace ShipWorks.Shipping.ShipSense.Settings
{
    /// <summary>
    /// A control that allows a user to customize how ShipSense learns how items being shipped
    /// impact shipments. At a technical level, this is basically configuring what data elements
    /// will get used when building the ShipSense hash key.
    /// </summary>
    public partial class ShipSenseHashConfigurationControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShipSenseHashConfigurationControl"/> class.
        /// </summary>
        public ShipSenseHashConfigurationControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the attribute names that should be used when building the ShipSense hash key.
        /// </summary>
        public IEnumerable<string> AttributeNames
        {
            get
            {
                // Reverse the order, so the values appear in the same order as they are on screen
                // since the update layout adjusts sets the index of controls that get added to zero
                return attributes.Values;
            }
        }

        /// <summary>
        /// Gets the selected property names that should be used when building the ShipSense hash key.
        /// </summary>
        public IEnumerable<string> SelectedPropertyNames
        {
            get { return itemProperties.SelectedItemProperties; }
        }

        /// <summary>
        /// Loads an attribute control for each of the attribute name provided.
        /// </summary>
        /// <param name="attributeNames">The attribute names.</param>
        public void LoadAttributeControls(IEnumerable<string> attributeNames)
        {
            attributes.LoadValues(attributeNames);
        }

        /// <summary>
        /// Loads the item property control and selects/checks the check boxes of the 
        /// properties corresponding to the names provided.
        /// </summary>
        /// <param name="selectedPropertyNames">The property names that should be selected/checked.</param>
        public void LoadItemPropertyControl(IEnumerable<string> selectedPropertyNames)
        {
            itemProperties.LoadSelectedProperties(selectedPropertyNames);

        }
    }
}
