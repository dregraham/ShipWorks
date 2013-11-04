using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Business;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// User control for editing the contact details of a store
    /// </summary>
    partial class StoreContactControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StoreContactControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the contact information from the given store
        /// </summary>
        public void LoadStore(StoreEntity store)
        {
            email.Text = store.Email;
            phone.Text = store.Phone;
            website.Text = store.Website;
        }

        /// <summary>
        /// Save the contact information for the given store
        /// </summary>
        public void SaveToEntity(StoreEntity store)
        {
            store.Email = email.Text;
            store.Phone = phone.Text;
            store.Website = website.Text;
        }
    }
}
