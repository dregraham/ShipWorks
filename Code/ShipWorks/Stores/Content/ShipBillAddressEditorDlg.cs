using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Microsoft.Web.Services3.Addressing;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Connection;
using log4net;
using Interapptive.Shared.UI;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Window for editing the ship\bill address of an entity
    /// </summary>
    public partial class ShipBillAddressEditorDlg : Form
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipBillAddressEditorDlg));

        EntityBase2 entity;
        private readonly bool enableAddressValidation;
        private ValidatedAddressScope validatedAddressScope;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipBillAddressEditorDlg(EntityBase2 entity) : this(entity, false)
        {}

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipBillAddressEditorDlg(EntityBase2 entity, bool enableShipAddressValidation)
        {
            InitializeComponent();

            this.entity = entity;
            this.enableAddressValidation = enableShipAddressValidation;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            validatedAddressScope = new ValidatedAddressScope();
            shipBillControl.EnableAddressValidation = enableAddressValidation;
            shipBillControl.LoadEntity(entity);
        }

        /// <summary>
        /// Save changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            // Save the current address so we can check if it's changed later
            AddressAdapter previousShippingAddress = new AddressAdapter();
            AddressAdapter.Copy(entity, "Ship", previousShippingAddress);

            shipBillControl.SavePendingChanges();

            try
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.SaveAndRefetch(entity);

                    validatedAddressScope.FlushAddressesToDatabase(adapter, EntityUtility.GetEntityId(entity), "Ship");
                    validatedAddressScope.FlushAddressesToDatabase(adapter, EntityUtility.GetEntityId(entity), "Bill");
                    
                    // Propagate address changes to shipments after we've saved all the order details
                    ValidatedAddressManager.PropagateAddressChangesToShipments(adapter, EntityUtility.GetEntityId(entity), previousShippingAddress, new AddressAdapter(entity, "Ship"));
                }
            }
            catch (ORMConcurrencyException ex)
            {
                log.Error("Failed saving address", ex);

                MessageHelper.ShowError(this,
                    string.Format("{0} has been deleted by another user and could not be saved.",
                        ObjectLabelManager.GetLabel((long) entity.Fields.PrimaryKeyFields[0].CurrentValue).GetCustomText(true, false, false)));
            }

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// The form has closed
        /// </summary>
        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            validatedAddressScope.Dispose();
        }
    }
}
