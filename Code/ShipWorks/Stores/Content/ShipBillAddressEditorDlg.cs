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

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipBillAddressEditorDlg(EntityBase2 entity)
        {
            InitializeComponent();

            this.entity = entity;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
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
                    ResetAddressValidationIfNecessary(previousShippingAddress, adapter);
                    ValidatedAddressManager.PropagateAddressChangesToShipments(adapter, (long) entity.PrimaryKeyFields[0].CurrentValue, previousShippingAddress, new AddressAdapter(entity, "Ship"));
                    adapter.SaveAndRefetch(entity);
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
        /// If the shipping address has changed, reset the validation status and delete existing suggestions
        /// </summary>
        private void ResetAddressValidationIfNecessary(AddressAdapter previousShippingAddress, SqlAdapter adapter)
        {
            AddressAdapter currentShippingAddress = new AddressAdapter(entity, "Ship");

            if (!previousShippingAddress.Equals(currentShippingAddress))
            {
                if (ValidatedAddressManager.EnsureAddressCanBeValidated(currentShippingAddress))
                {
                    currentShippingAddress.AddressValidationStatus = (int)AddressValidationStatusType.NotChecked;
                    currentShippingAddress.AddressValidationError = string.Empty;
                }

                currentShippingAddress.ResidentialStatus = (int) ValidationDetailStatusType.Unknown;
                currentShippingAddress.POBox = (int)ValidationDetailStatusType.Unknown;
                currentShippingAddress.InternationalTerritory = (int)ValidationDetailStatusType.Unknown;
                currentShippingAddress.MilitaryAddress = (int)ValidationDetailStatusType.Unknown;

                currentShippingAddress.AddressValidationSuggestionCount = 0;
                ValidatedAddressManager.DeleteExistingAddresses(adapter, (long) entity.Fields.PrimaryKeyFields[0].CurrentValue);
            }
        }
    }
}
