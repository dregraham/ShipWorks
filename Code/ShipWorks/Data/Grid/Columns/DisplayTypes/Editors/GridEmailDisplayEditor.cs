using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes.Editors
{
    public partial class GridEmailDisplayEditor : GridColumnDisplayEditor
    {
        GridEmailAddressDisplayType displayType;

        /// <summary>
        /// Constructor
        /// </summary>
        public GridEmailDisplayEditor(GridEmailAddressDisplayType displayType)
            : base(displayType)
        {
            InitializeComponent();

            if (displayType == null)
            {
                throw new ArgumentNullException("displayType");
            }

            this.displayType = displayType;

            nameOnly.Checked = (displayType.EmailDisplayFormat == EmailDisplayFormat.NameOnly);
            addressOnly.Checked = (displayType.EmailDisplayFormat == EmailDisplayFormat.AddressOnly);
            nameAndAddress.Checked = (displayType.EmailDisplayFormat == EmailDisplayFormat.NameAndAddress);
        }

        /// <summary>
        /// A checkbox value has changed
        /// </summary>
        private void OnValueChanged(object sender, EventArgs e)
        {
            displayType.EmailDisplayFormat = GetSelectedEmailDisplayFormat();

            OnValueChanged();
        }

        /// <summary>
        /// Get the display format selected from the radios
        /// </summary>
        private EmailDisplayFormat GetSelectedEmailDisplayFormat()
        {
            if (addressOnly.Checked)
            {
                return EmailDisplayFormat.AddressOnly;
            }

            if (nameAndAddress.Checked)
            {
                return EmailDisplayFormat.NameAndAddress;
            }

            return EmailDisplayFormat.NameOnly;
        }
    }
}
