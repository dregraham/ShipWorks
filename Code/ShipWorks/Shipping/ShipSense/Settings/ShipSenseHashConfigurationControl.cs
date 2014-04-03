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
                return panelMain.Controls.OfType<ShipSenseItemAttributeControl>().Select(c => c.AttributeName);
            }
        }

        /// <summary>
        /// Loads an attribute controls for each of the attribute name provided.
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
        /// Adds/displays a new attribute control to this control.
        /// </summary>
        public void AddAttribute()
        {
            AddAttributeControl(string.Empty);
        }

        /// <summary>
        /// Adds/displays a new attribute control to the customization panel populated with the 
        /// attribute name provided.
        /// </summary>
        private void AddAttributeControl(string attributeName)
        {
            // Add a new attribute control to the UI
            ShipSenseItemAttributeControl attributeControl = new ShipSenseItemAttributeControl(attributeName);
            attributeControl.Width = panelMain.Width;
            attributeControl.Dock = DockStyle.Top;
            attributeControl.DeleteAttributeClick += OnDeleteAttribute;
            
            panelMain.Controls.Add(attributeControl);
            panelMain.Controls.SetChildIndex(attributeControl, 0);

            UpdateLayout();
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
            panelMain.Controls.Remove(attributeControl);
            
            UpdateLayout();
        }

        /// <summary>
        /// Updates the layout according to the number of attribute controls in the main panel.
        /// </summary>
        private void UpdateLayout()
        {
            // Slide the panels up/down based on the content of the main panel
            panelMain.Height = panelMain.Controls.Count == 0 ? 0 : panelMain.Controls.OfType<Control>().Max(c => c.Bottom);
            panelBottom.Top = panelMain.Bottom;

            // Adjust the height of the overall control according to the bottom panel
            Height = panelBottom.Bottom;
        }
    }
}
