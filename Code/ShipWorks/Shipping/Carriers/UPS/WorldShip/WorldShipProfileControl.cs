using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.UPS.WorldShip
{
    /// <summary>
    /// WorldShip specific version of the UpsProfile control
    /// </summary>
    [KeyedComponent(typeof(ShippingProfileControlBase), ShipmentTypeCode.UpsWorldShip)]
    public class WorldShipProfileControl : UpsProfileControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WorldShipProfileControl()
        {
            // Because WorldShip handles its own printing, we want to hide the label format group
            GroupLabels.Visible = false;

            MoveSubsequentControlsUp(GroupLabels);
        }

        private void MoveSubsequentControlsUp(Control referenceControl)
        {
            // Get a list of controls that will need to be moved
            List<Control> controlsBelowReference = TabPageSettings.Controls.OfType<Control>()
                .Where(c => c.Location.Y > referenceControl.Location.Y)
                .ToList();

            // Find how much we'll need to move the controls by
            int deltaY = controlsBelowReference.Min(c => c.Location.Y) - referenceControl.Location.Y;

            foreach (Control control in TabPageSettings.Controls.OfType<Control>().Where(c => c.Location.Y > referenceControl.Location.Y))
            {
                control.Location = new Point(control.Location.X, control.Location.Y - deltaY);
            }
        }
    }
}
