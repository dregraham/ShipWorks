using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Templates.Controls
{
    /// <summary>
    /// Window for editing the email settings for a particular template and store
    /// </summary>
    public partial class TemplateEmailStoreSettingsDlg : Form
    {
        TemplateEntity template;
        StoreEntity store;

        TemplateStoreSettingsEntity shared;
        TemplateStoreSettingsEntity unique;

        EntityFields2 sharedOriginal;
        EntityFields2 uniqueOriginal;

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateEmailStoreSettingsDlg(TemplateEntity template, StoreEntity store)
        {
            InitializeComponent();

            this.template = template;
            this.store = store;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            shared = TemplateHelper.GetStoreSettings(template, null);
            unique = TemplateHelper.GetStoreSettings(template, store.StoreID);

            sharedOriginal = shared.Fields.Clone();
            uniqueOriginal = unique.Fields.Clone();

            radioUseShared.Checked = unique.EmailUseDefault;
            radioUseUnique.Checked = !unique.EmailUseDefault;

            sharedSettings.LoadSettings(template, null);
            uniqueSettings.LoadSettings(template, store.StoreID);
        }

        /// <summary>
        /// Changing whether to use shared or not
        /// </summary>
        private void OnCheckChanged(object sender, EventArgs e)
        {
            sharedSettings.Enabled = radioUseShared.Checked;
            uniqueSettings.Enabled = radioUseUnique.Checked;
        }

        /// <summary>
        /// Closing the window
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (!sharedSettings.SaveToEntity())
            {
                return;
            }

            if (!uniqueSettings.SaveToEntity())
            {
                return;
            }

            unique.EmailUseDefault = radioUseShared.Checked;

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            // If not closing for OK, rollback to values on entry (not necessarily a full rollback)
            if (DialogResult != DialogResult.OK)
            {
                shared.Fields = sharedOriginal;
                unique.Fields = uniqueOriginal;
            }
        }
    }
}
