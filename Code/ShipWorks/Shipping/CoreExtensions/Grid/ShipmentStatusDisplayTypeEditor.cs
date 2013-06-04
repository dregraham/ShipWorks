using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;

namespace ShipWorks.Shipping.CoreExtensions.Grid
{
    /// <summary>
    /// DisplayType editor for the shipment status column
    /// </summary>
    public partial class ShipmentStatusDisplayTypeEditor : GridColumnDisplayEditor
    {
        ShipmentStatusDisplayType displayType;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentStatusDisplayTypeEditor(ShipmentStatusDisplayType displayType)
            : base(displayType)
        {
            InitializeComponent();

            this.displayType = displayType;

            showIcon.Checked = displayType.ShowIcon;
            showIcon.CheckedChanged += new EventHandler(OnChangeFormat);
        }

        /// <summary>
        /// Some of the formatting has changed
        /// </summary>
        void OnChangeFormat(object sender, EventArgs e)
        {
            displayType.ShowIcon = showIcon.Checked;

            OnValueChanged();
        }
    }
}
