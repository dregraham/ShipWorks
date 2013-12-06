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
    /// Display editor for the order number column display type
    /// </summary>
    public partial class GridOrderNumberDisplayEditor : GridColumnDisplayEditor
    {
        GridOrderNumberDisplayType displayType;

        /// <summary>
        /// Constructor
        /// </summary>
        public GridOrderNumberDisplayEditor(GridOrderNumberDisplayType displayType)
            : base(displayType)
        {
            InitializeComponent();

            this.displayType = displayType;

            showIcon.Checked = displayType.ShowStoreIcon;
            showIcon.CheckedChanged += new EventHandler(OnChangeFormat);
        }

        /// <summary>
        /// Some of the formatting has changed
        /// </summary>
        void OnChangeFormat(object sender, EventArgs e)
        {
            displayType.ShowStoreIcon = showIcon.Checked;

            OnValueChanged();
        }
    }
}
