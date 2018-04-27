using System;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Microsoft.ApplicationInsights.DataContracts;
using ShipWorks.Common.IO.Hardware.Scanner;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Messaging.Messages.Dialogs;
using ShipWorks.Templates.Printing;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore.Options
{
    /// <summary>
    /// Option page for keyboard and barcode shortcuts
    /// </summary>
    public partial class OptionPageShortcuts : OptionPageBase
    {
        private readonly IScannerConfigurationRepository scannerRepo;
        private readonly IUserSession userSession;
        private readonly IMessenger messenger;
        private readonly ICurrentUserSettings currentUserSettings;
        private readonly IMessageHelper messageHelper;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly UserSettingsEntity settings;
        private readonly IWin32Window owner;
        private SingleScanSettings singleScanSettingsOnLoad;
        private IPrintJobFactory pringJobFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public OptionPageShortcuts(IWin32Window owner, ILifetimeScope scope)
        {
            InitializeComponent();
            scannerRepo = scope.Resolve<IScannerConfigurationRepository>();
            messageHelper = scope.Resolve<IMessageHelper>();
            sqlAdapterFactory = scope.Resolve<ISqlAdapterFactory>();
            userSession = scope.Resolve<IUserSession>();
            messenger = scope.Resolve<IMessenger>();
            pringJobFactory = scope.Resolve<IPrintJobFactory>();
            currentUserSettings = scope.Resolve<ICurrentUserSettings>();
            settings = userSession.User.Settings;
            this.owner = owner;
        }

        /// <summary>
        /// Save the selected settings
        /// </summary>
        public override void Save()
        {
            if (userSession.IsLoggedOn)
            {
                if (displayShortcutIndicator.Checked)
                {
                    currentUserSettings.StartShowingNotification(UserConditionalNotificationType.ShortcutIndicator);
                }
                else
                {
                    currentUserSettings.StopShowingNotification(UserConditionalNotificationType.ShortcutIndicator);
                }

                if (autoPrint.Checked)
                {
                    settings.SingleScanSettings = (int) SingleScanSettings.AutoPrint;
                }
                else if (singleScan.Checked)
                {
                    settings.SingleScanSettings = (int) SingleScanSettings.Scan;
                }
                else
                {
                    settings.SingleScanSettings = (int) SingleScanSettings.Disabled;
                }

                settings.AutoWeigh = autoWeigh.Checked;

                using (ISqlAdapter adapter = sqlAdapterFactory.Create())
                {
                    adapter.SaveAndRefetch(settings);
                }

                UpdateSingleScanTelemetry(settings);
            }
        }

        /// <summary>
        /// Load settings and populate controls
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            if (userSession.IsLoggedOn)
            {
                displayShortcutIndicator.Checked =
                    currentUserSettings.ShouldShowNotification(UserConditionalNotificationType.ShortcutIndicator);

                // Load single scan settings and update ui
                singleScan.Checked = (SingleScanSettings) settings.SingleScanSettings != SingleScanSettings.Disabled;
                autoPrint.Checked = (SingleScanSettings) settings.SingleScanSettings == SingleScanSettings.AutoPrint;
                autoWeigh.Checked = settings.AutoWeigh;
                UpdateSingleScanSettingsUI();

                singleScanSettingsOnLoad = (SingleScanSettings) settings.SingleScanSettings;
            }
        }

        /// <summary>
        /// Update auto print checkbox state based on single scan setting
        /// </summary>
        private void UpdateSingleScanSettingsUI()
        {
            // Only allow auto print to be checked when single scan is enabled
            if (!singleScan.Checked)
            {
                autoPrint.Checked = false;
                autoPrint.Enabled = false;
                autoWeigh.Checked = false;
                autoWeigh.Enabled = false;
                registerScannerLabel.Visible = false;
            }
            else
            {
                autoPrint.Enabled = true;
                registerScannerLabel.Visible = string.IsNullOrWhiteSpace(scannerRepo.GetScannerName().Value);
                autoWeigh.Enabled = true;
            }
        }

        /// <summary>
        /// Update UI when single scan is enabled
        /// </summary>
        private void OnChangeSingleScanSettings(object sender, EventArgs e) => UpdateSingleScanSettingsUI();

        /// <summary>
        /// Called when [click register scanner].
        /// </summary>
        private void OnClickRegisterScanner(object sender, EventArgs e)
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                Form findScanner = scope.ResolveNamed<Form>("ScannerRegistrationDialog");
                try
                {
                    findScanner.ShowDialog(owner);
                }
                catch (TargetInvocationException ex)
                {
                    messageHelper.ShowError(owner, ex.InnerException.Message);
                }

                UpdateSingleScanSettingsUI();
            }
        }

        /// <summary>
        /// Log the Single Scan Settings to Telemetry
        /// </summary>
        /// <param name="settings"></param>
        private void UpdateSingleScanTelemetry(IUserSettingsEntity settings)
        {
            if (settings.SingleScanSettings != (int) singleScanSettingsOnLoad)
            {
                EventTelemetry telemetryEvent = new EventTelemetry("SingleScan.Settings.Changed");
                string telemetryValue = EnumHelper.GetApiValue((SingleScanSettings) settings.SingleScanSettings);
                telemetryEvent.Properties.Add("SingleScan.Settings.Value", telemetryValue);

                Telemetry.TrackEvent(telemetryEvent);
            }
        }

        /// <summary>
        /// Opens profile manager dialog
        /// </summary>
        private void OnClickManageProfiles(object sender, EventArgs e)
        {
            messenger.Send(new OpenProfileManagerDialogMessage(this));
        }

        /// <summary>
        /// Print all barcodes
        /// </summary>
        private void OnClickPrintShortcuts(object sender, EventArgs e)
        {
            pringJobFactory.CreateBarcodePrintJob().PreviewAsync((Form) owner);
        }
    }
}
