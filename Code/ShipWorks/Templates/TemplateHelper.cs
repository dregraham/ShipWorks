using System;
using System.Collections.Generic;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Templates.Management;
using System.Drawing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Properties;
using ShipWorks.Data;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Users;
using ShipWorks.Filters;
using ShipWorks.Templates.Controls;
using ShipWorks.Templates.Processing;
using ShipWorks.Data.Connection;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using Interapptive.Shared;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.UI.Controls.Html;
using ShipWorks.Templates.Management.Skeletons;
using ShipWorks.Templates.Media;
using ShipWorks.Templates.Saving;

namespace ShipWorks.Templates
{
    /// <summary>
    /// Helpful functions for working with templates
    /// </summary>
    public static class TemplateHelper
    {
        class TemplateComparer : IEqualityComparer<TemplateEntity>
        {
            public bool Equals(TemplateEntity x, TemplateEntity y)
            {
                return x.TemplateID.Equals(y.TemplateID);
            }

            public int GetHashCode(TemplateEntity template)
            {
                return template.TemplateID.GetHashCode();
            }
        }

        /// <summary>
        /// Translate the given exception to a TemplateException, if we understand it.
        /// </summary>
        public static bool TranslateException(Exception ex)
        {
            if (ex is ORMConcurrencyException || ex is SqlForeignKeyException)
            {
                throw new TemplateConcurrencyException("Another user has recently made changes. ShipWorks cannot save your changes since they would overwrite the other changes.", ex);
            }

            return false;
        }

        /// <summary>
        /// Get the image to use for the template based on its type
        /// </summary>
        public static Image GetTemplateImage(TemplateEntity template)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            if (!TemplateXslProvider.FromTemplate(template).IsValid)
            {
                return Resources.error16;
            }

            if (template.IsSnippet)
            {
                return Resources.template_snippet16;
            }

            return GetTemplateImage((TemplateType) template.Type);
        }

        /// <summary>
        /// Get the image to use for the given template type
        /// </summary>
        public static Image GetTemplateImage(TemplateType templateType)
        {
            switch (templateType)
            {
                case TemplateType.Standard:
                    return Resources.template_standard_doc16;
                case TemplateType.Label:
                    return Resources.template_label16;
                case TemplateType.Report:
                    return Resources.template_report;
                case TemplateType.Thermal:
                    return Resources.barcode;
            }

            throw new InvalidOperationException(string.Format("Invalid template type {0}", templateType));
        }

        /// <summary>
        /// Load the user specific settings for the template
        /// </summary>
        public static TemplateUserSettingsEntity GetUserSettings(TemplateEntity template)
        {
            if (template.IsNew)
            {
                throw new InvalidOperationException("Not that this is impossible, it will just need worked out if needed.");
            }

            // See if it needs loaded
            if (template.UserSettings.Count == 0)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    // Get the settings for this tempalte and user
                    adapter.FetchEntityCollection(template.UserSettings, new RelationPredicateBucket(
                        TemplateUserSettingsFields.TemplateID == template.TemplateID &
                        TemplateUserSettingsFields.UserID == UserSession.User.UserID));
                }
            }

            // If we still don't have it, it needs created
            if (template.UserSettings.Count == 0)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    TemplateUserSettingsEntity settings = new TemplateUserSettingsEntity();
                    settings.TemplateID = template.TemplateID;
                    settings.UserID = UserSession.User.UserID;
                    settings.PreviewSource = (int) TemplatePreviewSource.Order;
                    settings.PreviewCount = 1;
                    settings.PreviewFilterNodeID = BuiltinFilter.GetTopLevelKey(FilterTarget.Orders);
                    settings.PreviewZoom = (template.OutputFormat == (int) TemplateOutputFormat.Html) ? TemplatePreviewControl.FitWidth : "100%";

                    adapter.SaveAndRefetch(settings);
                    template.UserSettings.Add(settings);
                }
            }

            return template.UserSettings[0];
        }

        /// <summary>
        /// Load the computer specific settings for the template
        /// </summary>
        public static TemplateComputerSettingsEntity GetComputerSettings(TemplateEntity template)
        {
            if (template.IsNew)
            {
                throw new InvalidOperationException("Not that this is impossible, it will just need worked out if needed.");
            }

            // See if it needs loaded
            if (template.ComputerSettings.Count == 0)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    // Get the settings for this template and computer
                    adapter.FetchEntityCollection(template.ComputerSettings, new RelationPredicateBucket(
                        TemplateComputerSettingsFields.TemplateID == template.TemplateID &
                        TemplateComputerSettingsFields.ComputerID == UserSession.Computer.ComputerID));
                }
            }

            // If we still don't have it, it needs created
            if (template.ComputerSettings.Count == 0)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    TemplateComputerSettingsEntity settings = new TemplateComputerSettingsEntity();
                    settings.TemplateID = template.TemplateID;
                    settings.ComputerID = UserSession.Computer.ComputerID;
                    settings.PrinterName = "";
                    settings.PaperSource = (int) PaperSourceKind.AutomaticFeed;

                    adapter.SaveAndRefetch(settings);
                    template.ComputerSettings.Add(settings);
                }
            }

            return template.ComputerSettings[0];
        }

        /// <summary>
        /// Load the store specific settings for the template
        /// </summary>
        [NDependIgnoreLongMethod]
        public static TemplateStoreSettingsEntity GetStoreSettings(TemplateEntity template, long? storeID)
        {
            TemplateStoreSettingsEntity settings = null;

            if (!template.IsNew)
            {
                // See if any are loaded
                if (template.StoreSettings.Count == 0)
                {
                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        // Get the settings for this template and computer
                        adapter.FetchEntityCollection(template.StoreSettings, new RelationPredicateBucket(
                            TemplateStoreSettingsFields.TemplateID == template.TemplateID));
                    }
                }

                // Try to find the one we need
                settings = template.StoreSettings.Where(s => s.StoreID == storeID).SingleOrDefault();

                // If its null, it may be that its new since we loaded the whole collection
                if (settings == null)
                {
                    TemplateStoreSettingsCollection collection = TemplateStoreSettingsCollection.Fetch(SqlAdapter.Default,
                        TemplateStoreSettingsFields.TemplateID == template.TemplateID & TemplateStoreSettingsFields.StoreID == storeID);

                    if (collection.Count != 0)
                    {
                        settings = collection[0];
                        template.StoreSettings.Add(settings);
                    }
                }
            }

            // If we still don't have it, it needs created
            if (settings == null)
            {
                settings = new TemplateStoreSettingsEntity();
                settings.TemplateID = template.TemplateID;
                settings.StoreID = storeID;

                settings.EmailUseDefault = true;
                settings.EmailAccountID = -1;

                TemplateType type = (TemplateType) template.Type;

                if (type == TemplateType.Standard || type == TemplateType.Thermal)
                {
                    settings.EmailTo = "{//Order/Address[@type='bill']/Email}";
                    settings.EmailSubject = "Your order: {//Order/Number}";
                }
                else
                {
                    settings.EmailTo = "";
                    settings.EmailSubject = (type == TemplateType.Report) ? "Report" : "Labels";
                }

                settings.EmailCc = "";
                settings.EmailBcc = "";

                if (!template.IsNew)
                {
                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        adapter.SaveAndRefetch(settings);
                    }
                }

                template.StoreSettings.Add(settings);
            }

            return settings;
        }

        /// <summary>
        /// Get the deafult file extension to use (without the .) for the given template type when saving.
        /// </summary>
        public static string GetDefaultFileExtension(TemplateOutputFormat format)
        {
            switch (format)
            {
                case TemplateOutputFormat.Html: return "html";
                case TemplateOutputFormat.Text: return "txt";
                case TemplateOutputFormat.Xml: return "xml";
            }

            throw new ArgumentException("Invalid TemplateOutputFormat value. (" + format + ")", "format");
        }

        /// <summary>
        /// Get the effective output format for the given template.  For label templates, it is always html.  Otherwise the configured
        /// template output format is returned.
        /// </summary>
        public static TemplateOutputFormat GetEffectiveOutputFormat(TemplateEntity template)
        {
            TemplateOutputFormat effectiveFormat = (TemplateOutputFormat) template.OutputFormat;

            if (template.Type == (int) TemplateType.Label)
            {
                effectiveFormat = TemplateOutputFormat.Html;
            }

            return effectiveFormat;
        }

        /// <summary>
        /// Extract just the folder portion of the template full name
        /// </summary>
        public static string GetTemplateFolder(string templateFullName)
        {
            if (string.IsNullOrWhiteSpace(templateFullName))
            {
                throw new ArgumentException("Must specify the temlate name", "templateFullName");
            }

            List<string> folderPath = templateFullName.Split('\\').ToList();

            // The last one will be the actual template name
            folderPath.RemoveAt(folderPath.Count - 1);

            return string.Join(@"\", folderPath.ToArray());
        }

        /// <summary>
        /// Apply the default settings for templates of the given type and output format
        /// </summary>
        public static void ApplyDefaultSettings(TemplateEntity template, TemplateType templateType, TemplateOutputFormat outputFormat)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            // User selections
            template.Type = (int) templateType;
            template.OutputFormat = (int) outputFormat;

            // Defaults
            template.Context = (int) TemplateInputContext.Auto;
            template.OutputEncoding = "utf-8";

            // Default label sheet
            template.LabelSheetID = LabelSheetManager.DefaultLabelSheetID;

            // Margins
            template.PageMarginLeft = .5;
            template.PageMarginTop = .5;
            template.PageMarginRight = .5;
            template.PageMarginBottom = .5;

            // Size
            template.PageHeight = PaperDimensions.Default.Height;
            template.PageWidth = PaperDimensions.Default.Width;

            // Copies
            template.PrintCopies = 1;
            template.PrintCollate = false;

            // Saving
            template.SaveFileName = "Order {//Order/Number}." + TemplateHelper.GetDefaultFileExtension(TemplateHelper.GetEffectiveOutputFormat(template));
            template.SaveFileFolder = TemplateHelper.DefaultTemplateSaveDirectory;
            template.SaveFilePrompt = (int) SavePromptWhen.Once;
            template.SaveFileBOM = false;
            template.SaveFileOnlineResources = false;
        }

        /// <summary>
        /// The default directory where template save output will go
        /// </summary>
        public static string DefaultTemplateSaveDirectory
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ShipWorks\"); }
        }

        /// <summary>
        /// The error to display when processing an object with a template yields no results (and thus no output)
        /// </summary>
        public static string NoResultsErrorMessage
        {
            get { return "The template had no output for the selection due to the context."; }
        }

        /// <summary>
        /// Get the html to be displayed when you view a thermal template
        /// </summary>
        public static string ThermalTemplateDisplayHtml
        {
            get
            {
                return @"
                    <html>
                    <body style='font: normal 9pt tahoma; color: rgb(50, 50, 50); padding-left: 15px;' >"
                    + HtmlControl.ZoomDivStartTag + 
                    @"This template is for printing thermal labels.<br/><br/>
                    Thermal label data must be sent directly to a thermal printer, and cannot be previewed.
                    </div>
                    </body>
                    </html>";
            }
        }

        /// <summary>
        /// Get the html to use when a resource has been purged
        /// </summary>
        public static string ContentPurgedDisplayHtml
        {
            get
            {
                return @"
                    <html>
                    <body style='font: normal 9pt tahoma; color: rgb(50, 50, 50); padding-left: 15px;' >"
                    + HtmlControl.ZoomDivStartTag +
                    @"<b>Content Removed</b><br/><br/>
                    The content of this {0} has been deleted by the 'Delete old data' action task.
                    </div>
                    </body>
                    </html>";
            }
        }

        /// <summary>
        /// Get the html to be displayed when you view a thermal template
        /// </summary>
        public static string SnippetTemplateDisplayHtml
        {
            get
            {
                return @"
                    <html>
                    <body style='font: normal 9pt tahoma; color: rgb(50, 50, 50); padding-left: 15px;' >"
                    + HtmlControl.ZoomDivStartTag +
                    @"This template is contains a snippet.<br/><br/>
                    Snippets are meant to be used by other tempates and cannot be previewed.
                    </div>
                    </body>
                    </html>";
            }
        }

        /// <summary>
        /// Error to display if someone tries to create a System\Snippets template.
        /// </summary>
        public static string SnippetTemplateReservedNameError
        {
            get { return "The template name 'System\\Snippets' is reserved as a shortcut for importing all templates."; }
        }
        
        /// <summary>
        /// The maximum memory we want to reach for storing template processing results in memory.  If processing would
        /// result in more memory required than this, we output to file instead.
        /// </summary>
        public static long MaxMemoryForXslOutput
        {
            // 50,000k
            get { return 50000 * 1000; }
        }

        /// <summary>
        /// The maximum size of a single html document we will generate at a time.  So for instance if a giant ass template processing
        /// result list is being printed, each time it hits this, a new job will need to be started.
        /// </summary>
        public static long MaxMemoryForHtml
        {
            // 500k
            get { return 500 * 1000; }
        }

        /// <summary>
        /// The maximum number of results to use when generating a preview.
        /// </summary>
        public static int MaxResultsForPreview
        {
            get { return 100; }
        }

        /// <summary>
        /// Used for comparing templates based on their TemplateID, instead of object reference
        /// </summary>
        public static IEqualityComparer<TemplateEntity> TemplateEqualityComparer
        {
            get
            {
                return new TemplateComparer();
            }
        }
    }
}
