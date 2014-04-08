using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <summary>
        /// Initializes a new instance of the <see cref="ShipSenseHashConfigurationControl"/> class.
        /// </summary>
        public ShipSenseHashConfigurationControl()
        {
            InitializeComponent();
            
            // Call update layout, so the add attribute button is up snug against the 
            // instructional text if there aren't any items
            UpdateLayout();
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
                return panelAttributes.Controls.OfType<ShipSenseItemAttributeControl>()
                                      .Where(c => !string.IsNullOrWhiteSpace(c.AttributeName))
                                      .Select(c => c.AttributeName).Reverse();
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
            foreach (string name in attributeNames)
            {
                AddAttributeControl(name);
            }
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
        
        /// <summary>
        /// Adds/displays a new attribute control to the customization panel populated with the 
        /// attribute name provided.
        /// </summary>
        private void AddAttributeControl(string attributeName)
        {
            // Add a new attribute control to the UI
            ShipSenseItemAttributeControl attributeControl = new ShipSenseItemAttributeControl(attributeName);
            attributeControl.Width = panelProperties.Width;
            attributeControl.Dock = DockStyle.Top;
            attributeControl.DeleteAttributeClick += OnDeleteAttribute;
            
            // Set the child index to zero, so the control gets added to the bottom of the list
            panelAttributes.Controls.Add(attributeControl);
            panelAttributes.Controls.SetChildIndex(attributeControl, 0);

            UpdateLayout();
        }

        /// <summary>
        /// Updates the layout according to the number of attribute controls in the attributes panel.
        /// </summary>
        private void UpdateLayout()
        {
            // Slide the panels up/down based on the content of the attributes panel
            panelAttributes.Height = panelAttributes.Controls.Count == 0 ? 0 : panelAttributes.Controls.OfType<Control>().Max(c => c.Bottom);
            panelBottom.Top = panelAttributes.Bottom;

            // Adjust the height of the overall control according to the bottom panel
            Height = panelBottom.Bottom;
        }

        /// <summary>
        /// Called when Add Attribute is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnAddAttribute(object sender, EventArgs e)
        {
            AddAttributeControl(string.Empty);
        }

        /// <summary>
        /// Deletes the attribute control from the customization panel that generated the RemoveAttribute event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnDeleteAttribute(object sender, EventArgs e)
        {
            ShipSenseItemAttributeControl attributeControl = (ShipSenseItemAttributeControl)sender;
            
            attributeControl.DeleteAttributeClick -= OnDeleteAttribute;
            panelAttributes.Controls.Remove(attributeControl);
            
            UpdateLayout();
        }
    }
}
