using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.UI;
using Interapptive.Shared.Business;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.UI;
using ShipWorks.Messages;
using ShipWorks.Shipping.Carriers;

namespace ShipWorks.Shipping.Settings.Origin
{
    /// <summary>
    /// Edit a single postal shipper
    /// </summary>
    public partial class ShippingOriginEditorDlg : Form
    {
        ShippingOriginEntity shipper;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingOriginEditorDlg(ShippingOriginEntity shipper)
        {
            InitializeComponent();

            this.shipper = shipper;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            if (shipper.Description != GetDefaultDescription(shipper))
            {
                description.Text = shipper.Description;
            }

            description.PromptText = GetDefaultDescription(shipper);

            personControl.LoadEntity(new PersonAdapter(shipper, ""));
        }

        /// <summary>
        /// Get the default description to use for the given shipper
        /// </summary>
        private string GetDefaultDescription(ShippingOriginEntity shipper)
        {
            StringBuilder description = new StringBuilder(new PersonName(new PersonAdapter(shipper, "")).FullName);

            if (shipper.Street1.Length > 0)
            {
                if (description.Length > 0)
                {
                    description.Append(", ");
                }

                description.Append(shipper.Street1);
            }

            if (shipper.PostalCode.Length > 0)
            {
                if (description.Length > 0)
                {
                    description.Append(", ");
                }

                description.Append(shipper.PostalCode);
            }

            return description.ToString();
        }

        /// <summary>
        /// The text in the person control has changed
        /// </summary>
        private void OnPersonContentChanged(object sender, EventArgs e)
        {
            personControl.SaveToEntity();
            description.PromptText = GetDefaultDescription(shipper);
        }

        /// <summary>
        /// User is ready to save the changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            bool isNew = shipper.IsNew;

            personControl.SaveToEntity();

            if (shipper.FirstName.Length == 0 || shipper.LastName.Length == 0)
            {
                MessageHelper.ShowError(this, "Enter a first and last name for the shipper.");
                return;
            }

            if (shipper.Street1.Length == 0)
            {
                MessageHelper.ShowError(this, "Enter a street address for the shipper.");
                return;
            }

            if (description.Text.Trim().Length > 0)
            {
                shipper.Description = description.Text.Trim();
            }
            else
            {
                shipper.Description = GetDefaultDescription(shipper);
            }

            try
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.SaveAndRefetch(shipper);
                }

                if (isNew)
                {
                    ShippingOriginManager.CheckForChangesNeeded();
                }

                // Send message when a new shipper is added
                Messenger.Current.Send(new OriginAddressChangedMessage(null));
                DialogResult = DialogResult.OK;
            }
            catch (ORMQueryExecutionException ex)
            {
                if (ex.Message.Contains("IX_ShippingOrigin_Description"))
                {
                    MessageHelper.ShowMessage(this, "A shipper with the selected name or description already exists.");
                }
                else
                {
                    throw;
                }
            }
            catch (ORMConcurrencyException)
            {
                MessageHelper.ShowError(this, "Your changes cannot be saved because another use has deleted the shipper.");

                DialogResult = DialogResult.Abort;
            }
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            // Rollback changes if not saved
            if (DialogResult != DialogResult.OK)
            {
                shipper.RollbackChanges();
            }
        }
    }
}
