using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Templates.Media;
using ShipWorks.UI.Controls;
using ShipWorks.Templates.Processing;
using System.Linq;
using ShipWorks.Properties;
using System.Drawing.Printing;
using System.Collections;
using ShipWorks.Templates.Saving;
using ShipWorks.UI;
using ShipWorks.Stores;
using ShipWorks.Templates.Emailing;
using ShipWorks.Email;
using ShipWorks.Email.Accounts;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using Interapptive.Shared.UI;
using Divelements.SandGrid;
using Interapptive.Shared;

namespace ShipWorks.Templates.Controls
{
    /// <summary>
    /// UserControl for editing template settings
    /// </summary>
    [NDependIgnoreLongTypes]
    public partial class TemplateSettingsControl : UserControl
    {
        TemplateEntity template;

        List<OptionPage> allOptionPages = new List<OptionPage>();

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateSettingsControl()
        {
            InitializeComponent();

            EnumHelper.BindComboBox<TemplateInputContext>(templateContext);
            LoadTemplateFormats();

            LoadSaveFileControls();

            allOptionPages.AddRange(optionControl.OptionPages.Cast<OptionPage>());
        }

        /// <summary>
        /// Load the xsl output format combo box
        /// </summary>
        private void LoadTemplateFormats()
        {
            xslOutput.DisplayMember = "Text";
            xslOutput.ValueMember = "Value";

            xslOutput.DataSource = new TemplateOutputFormat[] { TemplateOutputFormat.Html, TemplateOutputFormat.Text, TemplateOutputFormat.Xml }
                .Select(f => new ImageComboBoxItem(EnumHelper.GetDescription(f), f, GetOutputFormatImage(f))).ToList();
        }

        /// <summary>
        /// Get the image to use for the givne output format
        /// </summary>
        private Image GetOutputFormatImage(TemplateOutputFormat format)
        {
            switch (format)
            {
                case TemplateOutputFormat.Html: return Resources.template_html16;
                case TemplateOutputFormat.Xml: return Resources.template_general_16;
                case TemplateOutputFormat.Text: return Resources.template_text;
            }

            throw new InvalidOperationException(string.Format("Invalid value {0}", format));
        }

        /// <summary>
        /// Populate the dropdowns for the save file seciton
        /// </summary>
        private void LoadSaveFileControls()
        {
            // Fill the encoding box
            fileEncoding.DisplayMember = "Display";
            fileEncoding.ValueMember = "Value";
            fileEncoding.DataSource = new ArrayList {
                new { Display = "Unicode (UTF-7)",  Value = "utf-7" },
                new { Display = "Unicode (UTF-8)",  Value = "utf-8" },
                new { Display = "Unicode (UTF-16)", Value = "utf-16" },
                new { Display = "Unicode (UTF-32)", Value = "utf-32" },
                new { Display = "Windows (ISO-8859-1)", Value = "iso-8859-1" },
                new { Display = "ASCII",            Value = "us-ascii" }};
        }

        /// <summary>
        /// Load the settings for the specified template
        /// </summary>
        public void LoadSettings(TemplateEntity template)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            labelEmailAccounts.Visible = UserSession.Security.HasPermission(PermissionType.ManageEmailAccounts);
            manageEmailAccounts.Visible = UserSession.Security.HasPermission(PermissionType.ManageEmailAccounts);

            this.template = template;

            LoadSettingsFromTemplate();
            UpdateTemplateTypeUI();
        }

        /// <summary>
        /// Load the settings from the template
        /// </summary>
        private void LoadSettingsFromTemplate()
        {
            // General
            templateType.SelectedValue = (TemplateType) template.Type;
            templateContext.SelectedValue = (TemplateInputContext) template.Context;
            xslOutput.SelectedValue = (TemplateOutputFormat) template.OutputFormat;
            fileEncoding.SelectedValue = template.OutputEncoding;
            if (fileEncoding.Text.Length == 0)
            {
                fileEncoding.Text = template.OutputEncoding;
            }

            // Page settings
            pageSettings.PaperWidth = template.PageWidth;
            pageSettings.PaperHeight = template.PageHeight;
            pageSettings.MarginLeft = template.PageMarginLeft;
            pageSettings.MarginRight = template.PageMarginRight;
            pageSettings.MarginTop = template.PageMarginTop;
            pageSettings.MarginBottom = template.PageMarginBottom;

            // Labels
            labelSheetControl.LabelSheetID = template.LabelSheetID;

            // Printing
            TemplateComputerSettingsEntity templateSettings = TemplateHelper.GetComputerSettings(template);
            printerControl.LoadPrinters(templateSettings.PrinterName, templateSettings.PaperSource, PrinterSelectionInvalidPrinterBehavior.AlwaysPreserve);

            // Copies
            copiesControl.Copies = template.PrintCopies;
            copiesControl.Collate = template.PrintCollate;

            // Saving
            LoadSaveFileSettingsFromTemplate();

            // Email
            LoadEmailSettingsFromTemplate();
        }

        /// <summary>
        /// Save the current settings to the configured template
        /// </summary>
        public bool SaveSettingsToTemplate()
        {
            // General
            if (!SaveGeneralSettingsToTemplate())
            {
                return false;
            }

            // Dimensions
            template.PageWidth = pageSettings.PaperWidth;
            template.PageHeight = pageSettings.PaperHeight;
            template.PageMarginLeft = pageSettings.MarginLeft;
            template.PageMarginRight = pageSettings.MarginRight;
            template.PageMarginTop = pageSettings.MarginTop;
            template.PageMarginBottom = pageSettings.MarginBottom;

            // Sheet
            if (!SaveLabelSettingsToTemplate())
            {
                return false;
            }

            // Printing
            TemplateComputerSettingsEntity templateSettings = TemplateHelper.GetComputerSettings(template);
            templateSettings.PrinterName = printerControl.PrinterName;
            templateSettings.PaperSource = printerControl.PaperSource;

            // Copies
            template.PrintCopies = copiesControl.Copies;
            template.PrintCollate = copiesControl.Collate;

            // Saving
            if (!SaveSaveFileSettingsToTemplate())
            {
                return false;
            }

            // Email
            if (!SaveEmailSettingsToTemplate())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Save settings from the general tab
        /// </summary>
        private bool SaveGeneralSettingsToTemplate()
        {
            template.Context = (int) templateContext.SelectedValue;
            template.OutputFormat = (int) xslOutput.SelectedValue;

            if (fileEncoding.SelectedValue != null)
            {
                template.OutputEncoding = (string) fileEncoding.SelectedValue;
            }
            else
            {
                template.OutputEncoding = fileEncoding.Text;
            }

            try
            {
                Encoding.GetEncoding(template.OutputEncoding);
            }
            catch (ArgumentException)
            {
                MessageHelper.ShowError(this, string.Format("'{0}' is not a supported encoding.", template.OutputEncoding));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Save the label sheet settings to the template
        /// </summary>
        private bool SaveLabelSettingsToTemplate()
        {
            template.LabelSheetID = labelSheetControl.LabelSheetID;

            if (template.Type == (int) TemplateType.Label)
            {
                LabelSheetEntity labelSheet = LabelSheetManager.GetLabelSheet(labelSheetControl.LabelSheetID);
                if (labelSheet == null)
                {
                    MessageHelper.ShowError(this, "The template does not have a selected label sheet.");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Load the settings for saving files
        /// </summary>
        private void LoadSaveFileSettingsFromTemplate()
        {
            fileName.Text = template.SaveFileName;
            fileFolder.Text = template.SaveFileFolder;

            switch ((SavePromptWhen) template.SaveFilePrompt)
            {
                case SavePromptWhen.Never:
                    promptNever.Checked = true;
                    break;

                case SavePromptWhen.Once:
                    promptOnce.Checked = true;
                    break;

                case SavePromptWhen.Always:
                default:
                    promptAlways.Checked = true;
                    break;
            }

            fileSaveOnlineImages.Checked = template.SaveFileOnlineResources;
            fileWriteBOM.Checked = template.SaveFileBOM;
        }

        /// <summary>
        /// Save the settings for saving files
        /// </summary>
        private bool SaveSaveFileSettingsToTemplate()
        {
            if (!TemplateXslProvider.FromToken(fileName.Text).IsValid)
            {
                MessageHelper.ShowError(this, "The file name selected for saving has token errors.");
                return false;
            }

            if (!TemplateXslProvider.FromToken(fileFolder.Text).IsValid)
            {
                MessageHelper.ShowError(this, "The folder selected for saving has token errors.");
                return false;
            }

            template.SaveFileName = fileName.Text;
            template.SaveFileFolder = fileFolder.Text;

            if (promptNever.Checked) template.SaveFilePrompt = (int) SavePromptWhen.Never;
            if (promptOnce.Checked) template.SaveFilePrompt = (int) SavePromptWhen.Once;
            if (promptAlways.Checked) template.SaveFilePrompt = (int) SavePromptWhen.Always;

            template.SaveFileBOM = fileWriteBOM.Checked;
            template.SaveFileOnlineResources = fileSaveOnlineImages.Checked;

            return true;
        }

        /// <summary>
        /// Load the email settings from the template
        /// </summary>
        private void LoadEmailSettingsFromTemplate()
        {
            int storeCount = StoreManager.GetAllStores().Count;

            if (storeCount > 1)
            {
                LoadEmailSettingsMultiStores();
            }
            else
            {
                LoadEmailSettingsSingleStore();
            }

            emailSingleStoreSettings.Visible = storeCount <= 1;
            emailMultiStoreLabel.Visible = storeCount > 1;
            emailMultiSettingsGrid.Visible = storeCount > 1;
            emailMultiEditSettings.Visible = storeCount > 1;
        }

        /// <summary>
        /// Load email settings for the single store secnario
        /// </summary>
        private void LoadEmailSettingsSingleStore()
        {
            // Only way there wouldnt be one is if a user on another machine had just deleted the last one. Not likely, may as well crash.
            StoreEntity store = StoreManager.GetAllStores()[0];

            if (TemplateHelper.GetStoreSettings(template, store.StoreID).EmailUseDefault)
            {
                emailSingleStoreSettings.LoadSettings(template, null);
            }
            else
            {
                emailSingleStoreSettings.LoadSettings(template, store.StoreID);
            }

            // Positioning
            emailSingleStoreSettings.Location = emailMultiStoreLabel.Location;

            // Update the manage acccounts button
            labelEmailAccounts.Top = emailSingleStoreSettings.Bottom + 12;
            manageEmailAccounts.Top = labelEmailAccounts.Bottom + 4;
        }

        /// <summary>
        /// Load the UI based on the current number of stores
        /// </summary>
        [NDependIgnoreLongMethod]
        private void LoadEmailSettingsMultiStores()
        {
            // Save the previous selection (if any)
            long selectedStoreID = 0;
            if (emailMultiSettingsGrid.SelectedElements.Count == 1)
            {
                var selectedStore = emailMultiSettingsGrid.SelectedElements[0].Tag as StoreEntity;
                if (selectedStore != null)
                {
                    selectedStoreID = selectedStore.StoreID;
                }
            }

            emailMultiSettingsGrid.Rows.Clear();
            emailMultiEditSettings.Enabled = false;

            List<StoreEntity> stores = StoreManager.GetAllStores().ToList();

            // If there is only one store, we don't need to confuse people with the option to override settings.
            foreach (StoreEntity store in stores)
            {
                TemplateStoreSettingsEntity settings = TemplateHelper.GetStoreSettings(template, store.StoreID);
                TemplateStoreSettingsEntity effective = settings.EmailUseDefault ? TemplateHelper.GetStoreSettings(template, null) : settings;

                var displayData = new List<Tuple<string, string>>();
                displayData.Add(new Tuple<string, string>("Account:", GetEmailAccountName(store, effective)));
                displayData.Add(new Tuple<string, string>("To:", effective.EmailTo));
                displayData.Add(new Tuple<string, string>("Cc:", effective.EmailCc));
                displayData.Add(new Tuple<string, string>("Bcc:", effective.EmailBcc));
                displayData.Add(new Tuple<string, string>("Subject:", effective.EmailSubject));

                GridRow row = new GridRow(new string[] 
                    {
                        store.StoreName,
                        settings.EmailUseDefault ? "Shared" : "Unique",
                        string.Join("\r\n", displayData.Where(t => !string.IsNullOrEmpty(t.Item2)).Select(t => t.Item1)),
                        string.Join("\r\n", displayData.Where(t => !string.IsNullOrEmpty(t.Item2)).Select(t => t.Item2))
                    });
                row.Height = 0;
                row.Tag = store;

                emailMultiSettingsGrid.Rows.Add(row);

                // Try to reselect the previous
                if (store.StoreID == selectedStoreID)
                {
                    row.Selected = true;
                }
            }

            // Make it fit the rows, but cap at a max size
            emailMultiSettingsGrid.PerformElementLayout();
            emailMultiSettingsGrid.Height = Math.Min(emailMultiSettingsGrid.Rows.Cast<GridRow>().Max(r => r.Bounds.Bottom + 5), 300);
            emailMultiEditSettings.Top = emailMultiSettingsGrid.Bottom + 8;

            // Update the manage acccounts button
            labelEmailAccounts.Top = emailMultiEditSettings.Bottom + 12;
            manageEmailAccounts.Top = labelEmailAccounts.Bottom + 4;
        }

        /// <summary>
        /// Get the email account name based on the given store and its settings
        /// </summary>
        private static string GetEmailAccountName(StoreEntity store, TemplateStoreSettingsEntity settings)
        {
            if (EmailAccountManager.EmailAccounts.Count == 0)
            {
                return "";
            }

            if (settings.EmailAccountID == -1)
            {
                return string.Format("Store Default ({0})", EmailAccountManager.GetStoreDefault(store.StoreID).AccountName);
            }
            else
            {
                var account = EmailAccountManager.GetAccount(settings.EmailAccountID);

                return account != null ? account.AccountName : "(Deleted)";
            }
        }

        /// <summary>
        /// The selected row in the email settings grid has changed
        /// </summary>
        private void OnEmailSettingsGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            emailMultiEditSettings.Enabled = emailMultiSettingsGrid.SelectedElements.Count == 1;
        }

        /// <summary>
        /// User double-clicked a row in the email settings grid
        /// </summary>
        private void OnEmailSettingsGridRowActivated(object sender, GridRowEventArgs e)
        {
            OpenEmailSettingsEditor((StoreEntity) e.Row.Tag);
        }

        /// <summary>
        /// Edit the email settings for the store row selected in the grid
        /// </summary>
        private void OnEmailEditSettings(object sender, EventArgs e)
        {
            if (emailMultiSettingsGrid.SelectedElements.Count == 1)
            {
                OpenEmailSettingsEditor((StoreEntity) emailMultiSettingsGrid.SelectedElements[0].Tag);
            }
        }

        /// <summary>
        /// Open the settings editor for the given store
        /// </summary>
        private void OpenEmailSettingsEditor(StoreEntity store)
        {
            using (var dlg = new TemplateEmailStoreSettingsDlg(template, store))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    LoadEmailSettingsMultiStores();
                }
            }
        }

        /// <summary>
        /// Save the email settings to the template
        /// </summary>
        private bool SaveEmailSettingsToTemplate()
        {
            if (emailSingleStoreSettings.Visible)
            {
                if (!emailSingleStoreSettings.SaveToEntity())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Open the manage email accounts window
        /// </summary>
        private void OnManageEmailAccounts(object sender, EventArgs e)
        {
            if (!SaveEmailSettingsToTemplate())
            {
                return;
            }

            using (EmailAccountManagerDlg dlg = new EmailAccountManagerDlg())
            {
                dlg.ShowDialog(this);
            }

            LoadEmailSettingsFromTemplate();
        }

        /// <summary>
        /// The current page is being deselected
        /// </summary>
        private void OnPageDeselecting(object sender, OptionControlCancelEventArgs e)
        {
            if (e.OptionPage == optionPageGeneral && !SaveGeneralSettingsToTemplate())
            {
                e.Cancel = true;
            }

            if (e.OptionPage == optionPageLabelSetup && !SaveLabelSettingsToTemplate())
            {
                e.Cancel = true;
            }

            if (e.OptionPage == optionPageSaving && !SaveSaveFileSettingsToTemplate())
            {
                e.Cancel = true;
            }

            if (e.OptionPage == optionPageEmailing && !SaveEmailSettingsToTemplate())
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Type of template is changing.
        /// </summary>
        private void OnChangeTemplateType(object sender, EventArgs e)
        {
            template.Type = (int) templateType.SelectedValue;

            UpdateTemplateTypeUI();
        }

        /// <summary>
        /// Update the UI that is dependant on template type
        /// </summary>
        [NDependIgnoreLongMethod]
        private void UpdateTemplateTypeUI()
        {
            TemplateType type = (TemplateType) template.Type;

            List<OptionPage> neededPages = new List<OptionPage> { optionPageGeneral };

            if (type == TemplateType.Thermal)
            {
                neededPages.Add(optionPagePrinting);
            }
            else
            {
                // Need to show label tab
                if (type == TemplateType.Label)
                {
                    neededPages.Add(optionPageLabelSetup);
                }

                else
                {
                    neededPages.Add(optionPagePageSetup);
                }

                neededPages.Add(optionPagePrinting);
                neededPages.Add(optionPageSaving);
                neededPages.Add(optionPageEmailing);
            }

            // Remove the pages that are not needed
            foreach (OptionPage excludePage in allOptionPages.Except(neededPages))
            {
                if (optionControl.OptionPages.Contains(excludePage))
                {
                    optionControl.OptionPages.Remove(excludePage);

                    // Add them as a child of us, way out of site.  If they don't have a parent, databinding doesnt work.
                    optionControlFake.OptionPages.Add(excludePage);
                }
            }

            // Ensure the right pages are shown for the templat type
            foreach (OptionPage neededPage in neededPages)
            {
                if (!optionControl.OptionPages.Contains(neededPage))
                {
                    optionControl.OptionPages.Insert(neededPages.IndexOf(neededPage), neededPage);
                }
            }

            // Hard-coded thermal values
            if (type == TemplateType.Thermal)
            {
                xslOutput.Enabled = false;
                xslOutput.SelectedValue = TemplateOutputFormat.Xml;

                fileEncoding.Enabled = false;
                fileEncoding.SelectedValue = "utf-8";

                // Thermals are always collated
                copiesControl.AllowChangeCollate = false;
                copiesControl.Collate = true;
            }
            else
            {
                // Special case for invalid template xsl
                xslOutput.Enabled = TemplateXslProvider.FromTemplate(template).IsValid;
                infotipOutputType.Visible = !xslOutput.Enabled;

                fileEncoding.Enabled = true;

                copiesControl.AllowChangeCollate = true;
            }

            printerControl.ShowPrinterCalibration = (type == TemplateType.Label);
        }

        /// <summary>
        /// The size of the printer selection control has changed.  This happens when the calibration button
        /// is shown or hidden.
        /// </summary>
        private void OnPrinterControlSizeChanged(object sender, EventArgs e)
        {
            groupBoxPrinter.Height = printerControl.Bottom + 3;
            groupBoxCopies.Top = groupBoxPrinter.Bottom + 6;
        }

        /// <summary>
        /// Browse for the location of the default folder to save files to
        /// </summary>
        private void OnBrowseFileSaveFolder(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.SelectedPath = fileFolder.Text;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    fileFolder.Text = dlg.SelectedPath;
                }
            }
        }
    }
}
