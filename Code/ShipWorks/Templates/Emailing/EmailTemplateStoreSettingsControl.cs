using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Email.Accounts;
using ShipWorks.Templates.Processing;

namespace ShipWorks.Templates.Emailing
{
    /// <summary>
    /// UserControl for configuring email settings for a template for a particular store
    /// </summary>
    public partial class EmailTemplateStoreSettingsControl : UserControl
    {
        TemplateStoreSettingsEntity settings = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailTemplateStoreSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the settings entity that is loaded in the control, or null if nothing is loaded.
        /// </summary>
        public TemplateStoreSettingsEntity SettingsEntity
        {
            get { return settings; }
        }

        /// <summary>
        /// Load the settings for the given template and store
        /// </summary>
        public void LoadSettings(TemplateEntity template, long? storeID)
        {
            settings = TemplateHelper.GetStoreSettings(template, storeID);

            LoadEmailAccounts();
            LoadSettings();
        }

        /// <summary>
        /// Load all the email accounts available to choose from
        /// </summary>
        private void LoadEmailAccounts()
        {
            account.DisplayMember = "Key";
            account.ValueMember = "Value";

            List<EmailAccountEntity> accounts = EmailAccountManager.EmailAccounts.ToList();

            if (accounts.Count > 0)
            {
                string defaultName = settings.StoreID == null ? "Store Default" : "Store Default (" + EmailAccountManager.GetStoreDefault(settings.StoreID.Value).AccountName + ")";

                List<DictionaryEntry> data = accounts.Select(a => new DictionaryEntry(a.AccountName, a.EmailAccountID)).ToList();
                data.Insert(0, new DictionaryEntry(defaultName, -1));

                account.DataSource = data;
            }
            else
            {
                account.DataSource = new DictionaryEntry[] { new DictionaryEntry("No Accounts", -1) };
            }
        }

        /// <summary>
        /// Load the settings from the given entity
        /// </summary>
        private void LoadSettings()
        {
            account.SelectedValue = settings.EmailAccountID;
            if (account.SelectedIndex < 0)
            {
                account.SelectedIndex = 0;
            }

            to.Text = settings.EmailTo;
            cc.Text = settings.EmailCc;
            bcc.Text = settings.EmailBcc;
            subject.Text = settings.EmailSubject;
        }

        /// <summary>
        /// Save the settings to the entity.  If an error occurs, the error is displayed and false is returned.
        /// </summary>
        public bool SaveToEntity()
        {
            if (settings == null)
            {
                throw new InvalidOperationException("No settings are loaded to be saved.");
            }

            if (!TemplateXslProvider.FromToken(to.Text).IsValid)
            {
                MessageHelper.ShowInformation(this, "The To field has token errors.");
                return false;
            }

            if (!TemplateXslProvider.FromToken(cc.Text).IsValid)
            {
                MessageHelper.ShowInformation(this, "The CC field has token errors.");
                return false;
            }

            if (!TemplateXslProvider.FromToken(bcc.Text).IsValid)
            {
                MessageHelper.ShowInformation(this, "The BCC field has token errors.");
                return false;
            }

            if (!TemplateXslProvider.FromToken(subject.Text).IsValid)
            {
                MessageHelper.ShowInformation(this, "The subject field has token errors.");
                return false;
            }

            settings.EmailAccountID = Convert.ToInt64(account.SelectedValue);
            settings.EmailTo = to.Text;
            settings.EmailCc = cc.Text;
            settings.EmailBcc = bcc.Text;
            settings.EmailSubject = subject.Text;

            return true;
        }
    }
}
