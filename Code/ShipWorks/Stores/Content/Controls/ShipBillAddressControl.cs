using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Controls;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using Interapptive.Shared.Business;

namespace ShipWorks.Stores.Content.Controls
{
    /// <summary>
    /// UserControl for editing the billing and shipping address of an entity
    /// </summary>
    public partial class ShipBillAddressControl : UserControl
    {
        private bool enableAddressValidation = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipBillAddressControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Controls if the address is always editable, or if it needs to go into edit mode.
        /// </summary>
        [DefaultValue(PersonEditStyle.Normal)]
        public PersonEditStyle EditStyle
        {
            get
            {
                // They should both be the same, just return one of them
                return personBilling.EditStyle;
            }
            set
            {
                personBilling.EditStyle = value;
                personShipping.EditStyle = value;

                linkCopyFromBilling.Visible = value != PersonEditStyle.ReadOnly;
                linkCopyFromShipping.Visible = value != PersonEditStyle.ReadOnly;
            }
        }

        /// <summary>
        /// Enable address validation for the addresses
        /// </summary>
        public bool EnableAddressValidation
        {
            get
            {
                return enableAddressValidation;
            }
            set
            {
                enableAddressValidation = value;
                personBilling.EnableValidationControls = enableAddressValidation;
                personShipping.EnableValidationControls = enableAddressValidation;
            }
        }

        /// <summary>
        /// Load the data from the given entity into the control
        /// </summary>
        public void LoadEntity(EntityBase2 entity)
        {
            personShipping.LoadEntity(new PersonAdapter(entity, "Ship"));
            personBilling.LoadEntity(new PersonAdapter(entity, "Bill"));
        }

        /// <summary>
        /// Copy the billing address to the shipping address
        /// </summary>
        private void OnCopyFromBilling(object sender, EventArgs e)
        {
            personShipping.CopyFrom(personBilling);
        }

        /// <summary>
        /// Copy the shipping address to the billing address
        /// </summary>
        private void OnCopyFromShipping(object sender, EventArgs e)
        {
            personBilling.CopyFrom(personShipping);
        }

        /// <summary>
        /// Save any pending changes if the user is in edit mode
        /// </summary>
        public void SavePendingChanges()
        {
            if (!personShipping.IsReadonly)
            {
                personShipping.SaveToEntity();
            }

            if (!personBilling.IsReadonly)
            {
                personBilling.SaveToEntity();
            }
        }
    }
}
