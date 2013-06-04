using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using log4net;
using Interapptive.Shared.UI;
using ShipWorks.Data;

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
            shipBillControl.SavePendingChanges();

            try
            {
                using (SqlAdapter adpater = new SqlAdapter())
                {
                    adpater.SaveAndRefetch(entity);
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
    }
}
