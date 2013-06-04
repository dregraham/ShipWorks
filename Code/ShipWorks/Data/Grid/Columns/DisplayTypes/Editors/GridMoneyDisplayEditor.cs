using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes.Editors
{
    public partial class GridMoneyDisplayEditor : GridColumnDisplayEditor
    {
        GridMoneyDisplayType displayType;

        /// <summary>
        /// Constructor
        /// </summary>
        public GridMoneyDisplayEditor(GridMoneyDisplayType displayType)
            : base(displayType)
        {
            InitializeComponent();

            if (displayType == null)
            {
                throw new ArgumentNullException("displayType");
            }

            this.displayType = displayType;

            showCurrency.Checked = displayType.ShowCurrency;
            showThousands.Checked = displayType.ShowThousands;

            showCurrency.CheckedChanged += new EventHandler(OnValueChanged);
            showThousands.CheckedChanged += new EventHandler(OnValueChanged);
        }

        /// <summary>
        /// A checkbox value has changed
        /// </summary>
        private void OnValueChanged(object sender, EventArgs e)
        {
            displayType.ShowCurrency = showCurrency.Checked;
            displayType.ShowThousands = showThousands.Checked;

            OnValueChanged();
        }
    }
}
