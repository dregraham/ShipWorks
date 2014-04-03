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
        /// Adds/displays a new attribute control to the customization panel populated with the 
        /// attribute name provided.
        /// </summary>
        private void AddAttributeControl(string attributeName)
        {
            // TODO: Add an attribute control to this control and the list of attribute controls and adjust the UI layout accordingly
            // Add an attribute control to the UI and the list of attribute controls
            ShipSenseItemAttributeControl attributeControl = new ShipSenseItemAttributeControl(attributeName);
            attributeControl.Width = panelMain.Width;
            attributeControl.Dock = DockStyle.Top;
            attributeControl.DeleteAttributeClick += OnDeleteAttribute;
            
            attributeControls.Add(attributeControl);

            panelMain.Controls.Add(attributeControl);
            panelMain.Controls.SetChildIndex(attributeControl, 0);
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

        private void UpdateLayout()
        {
            foreach (ShipSenseItemAttributeControl attributeControl in panelMain.Controls.OfType<ShipSenseItemAttributeControl>())
            {
                
            }
        }
    }
}
