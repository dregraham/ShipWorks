using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;
using ShipWorks.Shipping;
using Xunit.Abstractions;

namespace ShipWorks.Tests.Core
{
    public class EnumTest
    {
        private readonly ITestOutputHelper output;

        public EnumTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Verify_EnumObfuscation_IsSet_Test()
        {
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains("ShipWorks")).OrderBy(a => a.FullName);
            IEnumerable<Type> types = assemblies
                .SelectMany(t => t.GetTypes())
                .Where(t => t.Namespace != null &&
                            t.IsEnum &&
                            t.Namespace.ToUpperInvariant().Contains("ShipWorks".ToUpperInvariant()) &&
                            ignoreShipmentTypeNameParts.All(istn => !t.FullName.ToUpperInvariant().Contains(istn)) &&
                            ignoreShipmentTypeNames.All(istn => t.FullName.ToUpperInvariant() != istn)
                    )
                .OrderBy(t => t.FullName);

            string missingObfuscationAttribute = string.Empty;

            foreach (Type type in types)
            {
                List<ObfuscationAttribute> customAttributes = type.GetCustomAttributes(false).ToList().OfType<ObfuscationAttribute>().ToList();

                if (!customAttributes.Any())
                {
                    missingObfuscationAttribute += string.Format("{0} is missing the ObfuscationAttribute.{1}", type.FullName, Environment.NewLine);
                }
                else
                {
                    foreach (var enumValueName in Enum.GetNames(type))
                    {
                        var memberInfos = type.GetMember(enumValueName);

                        var descriptionAttributes = memberInfos[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                        
                        if (!descriptionAttributes.Any())
                        {
                            missingObfuscationAttribute += string.Format("{0}.{1} is missing the DescriptionAttribute.{2}", type.FullName, enumValueName, Environment.NewLine);
                        }
                    }
                }
            }
            output.WriteLine(missingObfuscationAttribute);
            Assert.Equal(0, missingObfuscationAttribute.Length);
        }

        /// <summary>
        /// If namespace begins with these values, they are ignored.
        /// </summary>
        private List<string> ignoreShipmentTypeNameParts = new List<string>
            {
                "ShipWorks.Data.Model".ToUpperInvariant(),
                "FedEx.WebServices".ToUpperInvariant(),
                "Ebay.WebServices".ToUpperInvariant(),
                "NetworkSolutions.WebServices".ToUpperInvariant(),
                "Endicia.WebServices".ToUpperInvariant(),
                "Express1.WebServices".ToUpperInvariant(),
                "Usps.WebServices".ToUpperInvariant(),
                "Amazon.WebServices".ToUpperInvariant(),
                "AmeriCommerce.WebServices".ToUpperInvariant(),
                "ChannelAdvisor.WebServices".ToUpperInvariant(),
                "MarketplaceAdvisor.WebServices".ToUpperInvariant(),
                "PayPal.WebServices".ToUpperInvariant(),
                "ShipWorks.Stores.Content.Panels.MapPanelType".ToUpperInvariant()
            };

        /// <summary>
        /// Actual namespace of the enum
        /// </summary>
        private List<string> ignoreShipmentTypeNames = new List<string>
            {
                "ShipWorks.Filters.FilterNodePurpose".ToUpperInvariant(),
                "ShipWorks.SqlServer.Data.Rollups.RollupMethod".ToUpperInvariant(),
                "ShipWorks.SqlServer.Filters.DirtyCounts.FilterNodeColumnMaskTable".ToUpperInvariant(),
                "ShipWorks.SqlServer.Filters.DirtyCounts.FilterNodeJoinType".ToUpperInvariant(),
                "ShipWorks.SqlServer.Filters.FilterCountCheckpointState".ToUpperInvariant(),
                "ShipWorks.Users.Audit.AuditState".ToUpperInvariant(),
                "ShipWorks.Filters.FilterCountStatus".ToUpperInvariant(),
                "ShipWorks.Users.Audit.AuditState".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.Amazon.AmazonApi".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.Amazon.Mws.AmazonMwsActivities".ToUpperInvariant(),
                "ShipWorks.Templates.Printing.PrintJobPriority".ToUpperInvariant(),
                "ShipWorks.ApplicationCore.Logging.ApiLogSource".ToUpperInvariant(),
                "ShipWorks.Users.Security.PermissionType".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.Amazon.Mws.SigningAlgorithm".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.Amazon.Mws.AmazonMwsApiCall".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.Amazon.AmazonWeightField".ToUpperInvariant(),
                "ShipWorks.Shipping.Settings.Origin.ShipmentOriginSource".ToUpperInvariant(),
                "ShipWorks.Filters.Content.Conditions.StringOperator".ToUpperInvariant(),
                "ShipWorks.Filters.Content.Conditions.EqualityOperator".ToUpperInvariant(),
                "ShipWorks.Actions.ActionQueueStatus".ToUpperInvariant(),
                "ShipWorks.Actions.ActionQueueStepStatus".ToUpperInvariant(),
                "ShipWorks.Actions.ActionQueueType".ToUpperInvariant(),
                "ShipWorks.Actions.ActionRunnerResult".ToUpperInvariant(),
                "ShipWorks.Actions.ActionStepPostponementActivity".ToUpperInvariant(),
                "ShipWorks.Actions.ComputerLimitedType".ToUpperInvariant(),
                "ShipWorks.Actions.Tasks.ActionTaskInputSource".ToUpperInvariant(),
                "ShipWorks.Actions.Tasks.Common.ActionTaskInputRequirement".ToUpperInvariant(),
                "ShipWorks.Actions.Tasks.Common.Enums.EmailDelayType".ToUpperInvariant(),
                "ShipWorks.Actions.Triggers.FilterContentChangeDirection".ToUpperInvariant(),
                "ShipWorks.Actions.Triggers.OrderDownloadedRestriction".ToUpperInvariant(),
                "ShipWorks.ApplicationCore.Appearance.ColorScheme".ToUpperInvariant(),
                "ShipWorks.ApplicationCore.Dashboard.Content.DashboardMessageImageType".ToUpperInvariant(),
                "ShipWorks.ApplicationCore.Dashboard.Content.ServerMessageResponseAction".ToUpperInvariant(),
                "ShipWorks.ApplicationCore.Enums.HeartbeatOptions".ToUpperInvariant(),
                "ShipWorks.ApplicationCore.HeartbeatPace".ToUpperInvariant(),
                "ShipWorks.ApplicationCore.Interaction.MenuCommandResult".ToUpperInvariant(),
                "ShipWorks.ApplicationCore.Interaction.SelectionDependentType".ToUpperInvariant(),
                "ShipWorks.ApplicationCore.Interaction.StartupAction".ToUpperInvariant(),
                "ShipWorks.ApplicationCore.Licensing.Decoding.LicenseKeyPatternType".ToUpperInvariant(),
                "ShipWorks.ApplicationCore.Licensing.LicenseActivationState".ToUpperInvariant(),
                "ShipWorks.ApplicationCore.Logging.ApiLogCategory".ToUpperInvariant(),
                "ShipWorks.ApplicationCore.Logging.ApiLogEncryption".ToUpperInvariant(),
                "ShipWorks.ApplicationCore.Logging.LogActionType".ToUpperInvariant(),
                "ShipWorks.ApplicationCore.Nudges.NudgeOptionActionType".ToUpperInvariant(),
                "ShipWorks.Common.IO.Hardware.Printers.PrinterTechnology".ToUpperInvariant(),
                "ShipWorks.Common.Net.FileDownloadStatus".ToUpperInvariant(),
                "ShipWorks.Common.Threading.BackgroundResultStatus".ToUpperInvariant(),
                "ShipWorks.Common.Threading.ProgressItemStatus".ToUpperInvariant(),
                "ShipWorks.Data.Administration.DetailedDatabaseSetupWizard+ChooseWiselyOption".ToUpperInvariant(),
                "ShipWorks.Data.Administration.DetailedDatabaseSetupWizard+SetupMode".ToUpperInvariant(),
                "ShipWorks.Data.Administration.SimpleDatabaseSetupWizard+ElevatedPreparationType".ToUpperInvariant(),
                "ShipWorks.Data.Administration.SqlDatabaseStatus".ToUpperInvariant(),
                "ShipWorks.Data.Administration.SqlServerSetup.SqlServerInstallerPurpose".ToUpperInvariant(),
                "ShipWorks.Data.Administration.SqlServerSetup.WindowsInstallerInstaller+InstallerPlatformTarget".ToUpperInvariant(),
                "ShipWorks.Data.Administration.UpdateFrom2x.Configuration.ConfigurationMigrationAction".ToUpperInvariant(),
                "ShipWorks.Data.Administration.UpdateFrom2x.Configuration.ConfigurationMigrationSource".ToUpperInvariant(),
                "ShipWorks.Data.Administration.UpdateFrom2x.Configuration.ShipWorks2xApplicationDataSourceType".ToUpperInvariant(),
                "ShipWorks.Data.Administration.UpdateFrom2x.Database.MigrationState".ToUpperInvariant(),
                "ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.MigrationRowKeyType".ToUpperInvariant(),
                "ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.MigrationTaskExecutionPhase".ToUpperInvariant(),
                "ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.MigrationTaskInstancing".ToUpperInvariant(),
                "ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.MigrationTaskRunPattern".ToUpperInvariant(),
                "ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.MigrationTaskTypeCode".ToUpperInvariant(),
                "ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.Post2xMigrationStep".ToUpperInvariant(),
                "ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.Specialized.UpsNotificationUpgradeMigrationTask+NotificationTarget".ToUpperInvariant(),
                "ShipWorks.Data.Connection.ConnectionMonitorStatus".ToUpperInvariant(),
                "ShipWorks.Data.Connection.ReconnectResult".ToUpperInvariant(),
                "ShipWorks.Data.Connection.SqlSessionPermissionSet".ToUpperInvariant(),
                "ShipWorks.Data.Controls.PersonEditStyle".ToUpperInvariant(),
                "ShipWorks.Data.Controls.PersonFields".ToUpperInvariant(),
                "ShipWorks.Data.Grid.Columns.DisplayTypes.AbbreviationFormat".ToUpperInvariant(),
                "ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators.GridRollupStrategy".ToUpperInvariant(),
                "ShipWorks.Data.Grid.Columns.DisplayTypes.EnumSortMethod".ToUpperInvariant(),
                "ShipWorks.Data.Grid.Columns.DisplayTypes.GridLinkAction".ToUpperInvariant(),
                "ShipWorks.Data.Grid.Columns.DisplayTypes.StoreProperty".ToUpperInvariant(),
                "ShipWorks.Data.Grid.Columns.DisplayTypes.TimeDisplayFormat".ToUpperInvariant(),
                "ShipWorks.Data.Grid.Columns.GridColumnPreviewInputType".ToUpperInvariant(),
                "ShipWorks.Data.Grid.Columns.GridInitialSortMethod".ToUpperInvariant(),
                "ShipWorks.Data.Grid.Paging.PagedDataState".ToUpperInvariant(),
                "ShipWorks.Data.Grid.Paging.PagedEntityGrid+SelectionInterceptorArrayList+ClearAction".ToUpperInvariant(),
                "ShipWorks.Data.Import.Spreadsheet.GenericSpreadsheetTimeZoneAssumption".ToUpperInvariant(),
                "ShipWorks.Data.Import.Xml.Schema.ShipWorksSchema".ToUpperInvariant(),
                "ShipWorks.Data.Model.Custom.EntityPersistedAction".ToUpperInvariant(),
                "ShipWorks.Data.Utility.EntityFieldLengthSource".ToUpperInvariant(),
                "ShipWorks.Editions.Brown.BrownPostalAvailability".ToUpperInvariant(),
                "ShipWorks.Editions.EditionRestrictionLevel".ToUpperInvariant(),
                "ShipWorks.Editions.Freemium.FreemiumAccountType".ToUpperInvariant(),
                "ShipWorks.Email.Accounts.EmailIncomingSecurityType".ToUpperInvariant(),
                "ShipWorks.Email.Accounts.EmailSmtpCredentialSource".ToUpperInvariant(),
                "ShipWorks.Email.EmailOutboundRelationType".ToUpperInvariant(),
                "ShipWorks.Email.EmailOutboundVisibility".ToUpperInvariant(),
                "ShipWorks.Email.Outlook.EmailOutlookDlg+Folder".ToUpperInvariant(),
                "ShipWorks.FileTransfer.FtpSecurityType".ToUpperInvariant(),
                "ShipWorks.Filters.Content.Editors.ChoiceLabelUsage".ToUpperInvariant(),
                "ShipWorks.Filters.Content.SqlGeneration.SqlGenerationScopeType".ToUpperInvariant(),
                "ShipWorks.Filters.Controls.FilterScope".ToUpperInvariant(),
                "ShipWorks.Filters.FilterHelper+FilterImageType".ToUpperInvariant(),
                "ShipWorks.Filters.Management.FilterEditingResult".ToUpperInvariant(),
                "ShipWorks.Shipping.Carriers.FedEx.Enums.FedExEmailNotificationType".ToUpperInvariant(),
                "ShipWorks.Shipping.Carriers.FedEx.Enums.FedExMaskedDataType".ToUpperInvariant(),
                "ShipWorks.Shipping.Carriers.OnTrac.Schemas.Shipment.codType".ToUpperInvariant(),
                "ShipWorks.Shipping.Carriers.Postal.Endicia.Account.EndiciaAccountType".ToUpperInvariant(),
                "ShipWorks.Shipping.Carriers.Postal.Endicia.EndiciaReseller".ToUpperInvariant(),
                "ShipWorks.Shipping.Carriers.Postal.PostalCustomsForm".ToUpperInvariant(),
                "ShipWorks.Shipping.Carriers.UPS.Enums.UpsEmailNotificationType".ToUpperInvariant(),
                "ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api.UpsApiResponseStatus".ToUpperInvariant(),
                "ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api.UpsOnLineToolType".ToUpperInvariant(),
                "ShipWorks.Shipping.Carriers.UPS.WorldShip.WorldShipStatusType".ToUpperInvariant(),
                "ShipWorks.Shipping.Editing.Rating.RatingExceptionType".ToUpperInvariant(),
                "ShipWorks.Shipping.Editing.RequestedLabelFormatOptionControl+DisplayMode".ToUpperInvariant(),
                "ShipWorks.Shipping.InitialShippingTabDisplay".ToUpperInvariant(),
                "ShipWorks.Shipping.Settings.ShipmentBlankPhoneOption".ToUpperInvariant(),
                "ShipWorks.Shipping.Settings.ShipmentTypeSettingsControl+Page".ToUpperInvariant(),
                "ShipWorks.Stores.ComputerDownloadAllowed".ToUpperInvariant(),
                "ShipWorks.Stores.Content.Panels.PanelDataMode".ToUpperInvariant(),
                "ShipWorks.Stores.InitialDownloadRestrictionType".ToUpperInvariant(),
                "ShipWorks.Stores.Management.StoreSettingsDlg+Section".ToUpperInvariant(),
                "ShipWorks.Stores.OnlineGridColumnSupport".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.Amazon.Mws.AmazonMwsServiceStatusColor".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Grid.GridEbayFeedbackDirection".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.Ebay.EbayStoreType+EbayOnlineAction".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.Ebay.OrderCombining.EbayCombinedOrderType".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.GenericFile.GenericFileErrorAction".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.GenericFile.GenericFileFormat".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.GenericFile.GenericFileSuccessAction".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.GenericFile.Sources.GenericFileSourceTypeCode".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.GenericModule.GenericOnlineStatusSupport".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.GenericModule.GenericStoreDownloadStrategy".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.GenericModule.GenericVariantDataType".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.Infopia.InfopiaStatusType".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.Infopia.WebServices.ItemChoiceType".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.MarketplaceAdvisor.MarketplaceAdvisorAccountType".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.MarketplaceAdvisor.MarketplaceAdvisorOmsFlagTypes".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.Miva.MivaOnlineUpdateStrategy".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.NetworkSolutions.NetworkSolutionsFailureSeverity".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.Newegg.Enums.NeweggItemShippingStatus".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.Newegg.Enums.NeweggOrderStatus".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.Newegg.Enums.NeweggOrderType".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.Newegg.Net.Orders.Cancellation.CancellationReason".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.OrderDynamics.WebServices.ShipmentQueueStatus".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.OrderMotion.Udi.UdiRequestName".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.PayPal.PayPalCredentialType".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.PayPal.PayPalTransactionExclusionType".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.ProStores.ProStoresLoginMethod".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.Volusion.VolusionCodeImportMode".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.Volusion.VolusionSetupConfiguration".ToUpperInvariant(),
                "ShipWorks.Stores.Platforms.Volusion.VolusionWebSession+ReportType".ToUpperInvariant(),
                "ShipWorks.Stores.StatusPresetTarget".ToUpperInvariant(),
                "ShipWorks.Templates.Controls.TemplateTreeSnippetDisplayType".ToUpperInvariant(),
                "ShipWorks.Templates.Distribution.TemplateVersionType".ToUpperInvariant(),
                "ShipWorks.Templates.Emailing.EmailComposerDlg+MessageDraftState".ToUpperInvariant(),
                "ShipWorks.Templates.Media.PrinterSelectionInvalidPrinterBehavior".ToUpperInvariant(),
                "ShipWorks.Templates.Printing.PrintAction".ToUpperInvariant(),
                "ShipWorks.Templates.Processing.TemplateResultUsage".ToUpperInvariant(),
                "ShipWorks.Templates.Saving.SaveFileNamePart".ToUpperInvariant(),
                "ShipWorks.Templates.Saving.SavePromptWhen".ToUpperInvariant(),
                "ShipWorks.Templates.Tokens.TokenSelectionMode".ToUpperInvariant(),
                "ShipWorks.Templates.Tokens.TokenUsage".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Colors.ColorChooser+ChangeStyle".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Colors.ColorWheel+MouseState".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Html.Core.HtmlApi+CARET_DIRECTION".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Html.Core.HtmlApi+COORD_SYSTEM".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Html.Core.HtmlApi+DISPLAY_GRAVITY".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Html.Core.HtmlApi+DISPLAY_MOVEUNIT".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Html.Core.HtmlApi+ELEMENT_ADJACENCY".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Html.Core.HtmlApi+ELEMENT_CORNER".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Html.Core.HtmlApi+ELEMENT_TAG_ID".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Html.Core.HtmlApi+HtmlPainter".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Html.Core.HtmlApi+HtmlZOrder".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Html.Core.HtmlApi+MARKUP_CONTEXT_TYPE".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Html.Core.HtmlApi+MOVEUNIT_ACTION".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Html.Core.HtmlApi+POINTER_GRAVITY".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Html.Core.HtmlApi+SELECTION_TYPE".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Html.Core.OleApi+DOCHOSTUIFLAG".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Html.Core.OleApi+OLECLOSE".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Html.Core.OleApi+OLECMDEXECOPT".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Html.Core.OleApi+OLECMDF".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Html.Core.OleApi+OLECMDID".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Html.Core.OleApi+SELECTION_TYPE".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Html.Glyphs.GlyphType".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Html.HtmlControl+HtmlCommandStatus".ToUpperInvariant(),
                "ShipWorks.UI.Controls.Html.HtmlReadyState".ToUpperInvariant(),
                "ShipWorks.UI.Controls.SandGrid.DropTargetState".ToUpperInvariant(),
                "ShipWorks.UI.PopupAnimation".ToUpperInvariant(),
                "ShipWorks.UI.PopupSizerLocation".ToUpperInvariant(),
                "ShipWorks.UI.PopupSizerStyle".ToUpperInvariant(),
                "ShipWorks.UI.Wizard.WizardStepReason".ToUpperInvariant(),
                "ShipWorks.Users.Audit.AuditBehaviorUser".ToUpperInvariant(),
                "ShipWorks.Users.Audit.AuditChangeGroup".ToUpperInvariant(),
                "ShipWorks.Users.Security.PermissionScope".ToUpperInvariant(),
                "ShipWorks.Users.Security.StorePermissionCoverage".ToUpperInvariant(),
                "ShipWorks.Users.UserSettingsDlg+Section".ToUpperInvariant(),
                "ShipWorks.Filters.FilterState".ToUpperInvariant(),
            };
    }
}
