using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Form for editing the properties of an attribute
    /// </summary>
    public partial class EditAttributeDlg : Form
    {
        OrderItemAttributeEntity attribute;

        /// <summary>
        /// Constructor
        /// </summary>
        public EditAttributeDlg(OrderItemAttributeEntity attribute)
        {
            InitializeComponent();

            this.attribute = attribute;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            name.Text = attribute.Name;
            description.Text = attribute.Description;
            price.Amount = attribute.UnitPrice;
        }

        /// <summary>
        /// Save and commit
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (name.Text.Trim().Length == 0)
            {
                MessageHelper.ShowMessage(this, "Enter a name for the attribute.");
                return;
            }

            attribute.Name = name.Text;
            attribute.Description = description.Text;
            attribute.UnitPrice = price.Amount;

            DialogResult = DialogResult.OK;
        }
    }
}
