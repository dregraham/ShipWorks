using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Control that displays a list of shipment type checkboxes
    /// </summary>
    public class ShippingTypeCheckBoxesControl : Panel
    {
        private List<ShipmentType> shipmentTypes;

        /// <summary>
        /// Load the providers panel with the checkboxes for selection
        /// </summary>
        public void LoadProviders(IEnumerable<ShipmentType> shipmentTypesToLoad, Func<ShipmentTypeCode, bool> isShipmentSelected)
        {
            if (shipmentTypesToLoad == null)
            {
                throw new ArgumentNullException("shipmentTypesToLoad");
            }

            if (isShipmentSelected == null)
            {
                throw new ArgumentNullException("isShipmentSelected");
            }

            shipmentTypes = shipmentTypesToLoad.ToList();

            int top = 3;
            const int left = 3;

            // This is called more than once since we may need to add an Express1 entry depending on 
            // whether we need to show both Express1 for Endicia and Express1 for USPS, so we need
            // to clear and reload any existing check boxes
            foreach (CheckBox checkBox in Controls.OfType<CheckBox>())
            {
                checkBox.CheckedChanged -= OnChangeEnabledShipmentTypes;
            }
            Controls.Clear();

            // Add a check box for each shipment type
            foreach (ShipmentType shipmentType in shipmentTypes)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.AutoSize = true;
                checkBox.Location = new Point(left, top);

                checkBox.Tag = shipmentType.ShipmentTypeCode;
                checkBox.Text = shipmentType.ShipmentTypeName;
                checkBox.Checked = isShipmentSelected(shipmentType.ShipmentTypeCode);
                checkBox.CheckedChanged += OnChangeEnabledShipmentTypes;

                Controls.Add(checkBox);

                top = checkBox.Bottom + 5;
            }

            Height = top;
        }

        /// <summary>
        /// Handle when any checkbox changes
        /// </summary>
        private void OnChangeEnabledShipmentTypes(object sender, EventArgs e)
        {
            if (ChangeEnabledShipmentTypes != null)
            {
                ChangeEnabledShipmentTypes(this, new EventArgs());
            }
        }

        /// <summary>
        /// A shipment type has been either selected or unselected
        /// </summary>
        public event EventHandler ChangeEnabledShipmentTypes;

        /// <summary>
        /// Get the enabled shipment types (enabled in our UI, not necessarily in the database)
        /// </summary>
        public List<ShipmentType> SelectedShipmentTypes
        {
            get
            {
                return shipmentTypes.Where(st => Controls.OfType<CheckBox>().Any(c => c.Checked && (ShipmentTypeCode) c.Tag == st.ShipmentTypeCode)).ToList();    
            }
        }

        /// <summary>
        /// Gets the shipment types that are currently not selected
        /// </summary>
        public List<ShipmentType> UnselectedShipmentTypes
        {
            get
            {
                return shipmentTypes.Except(SelectedShipmentTypes).ToList();
            }
        }
    }
}
