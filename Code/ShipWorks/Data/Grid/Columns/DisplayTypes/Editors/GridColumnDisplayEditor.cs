using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes.Editors
{
    /// <summary>
    /// Base for editors used to configure column type properties
    /// </summary>
    [ToolboxItem(false)]
    public partial class GridColumnDisplayEditor : UserControl
    {
        GridColumnDisplayType displayType;

        /// <summary>
        /// Raised when a property value of the object changes
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// Default constructor.  Only here to make visual inheritance work.
        /// </summary>
        protected GridColumnDisplayEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GridColumnDisplayEditor(GridColumnDisplayType displayType)
        {
            InitializeComponent();

            this.displayType = displayType;

            comboAlignment.DataSource = new KeyValuePair<string, StringAlignment>[]
                {
                    new KeyValuePair<string, StringAlignment>("Left", StringAlignment.Near),
                    new KeyValuePair<string, StringAlignment>("Center", StringAlignment.Center),
                    new KeyValuePair<string, StringAlignment>("Right", StringAlignment.Far)
                };

            comboAlignment.DisplayMember = "Key";
            comboAlignment.ValueMember = "Value";

            comboAlignment.SelectedValue = displayType.Alignment;
            comboAlignment.SelectedIndexChanged += new EventHandler(OnChangeAlignment);
        }

        /// <summary>
        /// Alignment is changing
        /// </summary>
        private void OnChangeAlignment(object sender, EventArgs e)
        {
            displayType.Alignment = (StringAlignment) comboAlignment.SelectedValue;

            OnValueChanged();
        }

        /// <summary>
        /// Raise the ValueChanged event
        /// </summary>
        protected void OnValueChanged()
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, EventArgs.Empty);
            }
        }
    }
}
