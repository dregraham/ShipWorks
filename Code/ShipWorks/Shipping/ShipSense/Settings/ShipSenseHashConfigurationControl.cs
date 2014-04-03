using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Shipping.ShipSense.Settings
{
    /// <summary>
    /// A control that allows a user to customize how ShipSense learns how items being shipped
    /// impact shipments. At a technical level, this is basically configuring what data elements
    /// will get used when building the ShipSense hash key.
    /// </summary>
    public partial class ShipSenseHashConfigurationControl : UserControl
    {
        private readonly List<ShipSenseItemAttributeControl> attributeControls;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipSenseHashConfigurationControl"/> class.
        /// </summary>
        /// <param name="attributeNames">The attribute names.</param>
        public ShipSenseHashConfigurationControl()
        {
            InitializeComponent();

            attributeControls = new List<ShipSenseItemAttributeControl>();
        }

        /// <summary>
        /// Gets the attribute names that should be used when building the ShipSense hash key.
        /// </summary>
        public IEnumerable<string> AttributeNames
        {
            get
            {
                return attributeControls.Select(c => c.AttributeName);
            }
        }

        /// <summary>
        /// Loads an attribute controls for each of the attribute name provided.
        /// </summary>
        /// <param name="attributeNames">The attribute names.</param>
        public void LoadAttributeControls(IEnumerable<string> attributeNames)
        {
            attributeControls.Clear();

            foreach (string name in attributeNames)
            {
                AddAttributeControl(name);
            }
        }

        /// <summary>
        /// Adds/displays a new attribute control to this control.
        /// </summary>
        public void AddAttribute()
        {
            AddAttributeControl(string.Empty);
        }

        /// <summary>
        /// Adds/displays a new attribute control to this control.
        /// </summary>
        private void AddAttributeControl(string attributeName)
        {
            // TODO: Add an attribute control to this form and the list of attribute controls and adjust the UI layout accordingly
        }
    }
}
