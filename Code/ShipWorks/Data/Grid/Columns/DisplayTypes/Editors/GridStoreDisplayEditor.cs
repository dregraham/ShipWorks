using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes.Editors
{
    /// <summary>
    /// Display editor for the store column display type
    /// </summary>
    public partial class GridStoreDisplayEditor : GridColumnDisplayEditor
    {
        GridStoreDisplayType displayType;

        /// <summary>
        /// Constructor
        /// </summary>
        public GridStoreDisplayEditor(GridStoreDisplayType displayType)
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
