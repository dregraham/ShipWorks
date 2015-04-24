using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.Shipping.ShipSense.Settings;
using ShipWorks.UI.Utility;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// A control that prompts a user to administer a list of strings.
    /// </summary>
    public partial class StringListControl : UserControl
    {
        public StringListControl()
        {
            InitializeComponent();

            // Get rid of the ugly bottom border on the toolstrip control
            toolStripAddRule.Renderer = new NoBorderToolStripRenderer();

            // Call update layout, so the add button is up snug against the 
            // top if there aren't any items
            UpdateLayout();

            addValueLine.Text = AddButtonText;
        }

        public string AddButtonText
        {
            get { return addValueLine.Text; }
            set { addValueLine.Text = value; }
        }

        public IEnumerable<String> Values
        {
            get
            {
                return panelValues.Controls.OfType<TextBoxWithDeleteButtonControl>()
                    .Where(c => !string.IsNullOrWhiteSpace(c.Value))
                    .Select(c => c.Value).Reverse();
            }
        }

        /// <summary>
        /// Loads a value for each of the values provided.
        /// </summary>
        public void LoadValues(IEnumerable<string> values)
        {
            foreach (string name in values)
            {
                AddValueControl(name);
            }
        }

        /// <summary>
        /// Adds/displays a new value control to the customization panel populated with the 
        /// values provided.
        /// </summary>
        private void AddValueControl(string value)
        {
            // Add a new value control to the UI
            TextBoxWithDeleteButtonControl valueControl = new TextBoxWithDeleteButtonControl(value);
            valueControl.Width = Width;
            valueControl.Dock = DockStyle.Top;
            valueControl.DeleteClick += OnDelete;

            // Set the child index to zero, so the control gets added to the bottom of the list
            panelValues.Controls.Add(valueControl);
            panelValues.Controls.SetChildIndex(valueControl, 0);

            UpdateLayout();
        }

        /// <summary>
        /// Deletes the value control from the customization panel that generated the Delete event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnDelete(object sender, EventArgs e)
        {
            TextBoxWithDeleteButtonControl valueControl = (TextBoxWithDeleteButtonControl)sender;

            valueControl.DeleteClick -= OnDelete;
            panelValues.Controls.Remove(valueControl);

            UpdateLayout();
        }

        /// <summary>
        /// Called when Add Value is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnAddValue(object sender, EventArgs e)
        {
            AddValueControl(string.Empty);
        }

        /// <summary>
        /// Updates the layout according to the number of value controls in the values panel.
        /// </summary>
        private void UpdateLayout()
        {
            // Slide the panels up/down based on the content of the values panel
            if (panelValues.Controls.Count == 0)
            {
                panelValues.Height = 0;
                panelBottom.Top = 0;
            }
            else
            {
                panelValues.Height = panelValues.Controls.OfType<Control>().Max(c => c.Bottom);
                panelBottom.Top = panelValues.Bottom;
            }

            // Adjust the height of the overall control according to the bottom panel
            Height = panelBottom.Bottom;
        }
    }
}