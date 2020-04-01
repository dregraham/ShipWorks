using System;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Microsoft.ApplicationInsights.DataContracts;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Common.IO.Hardware.Scanner;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Editions;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Messaging.Messages.Dialogs;
using ShipWorks.Templates.Printing;
using ShipWorks.Users;
using ShipWorks.Settings;

namespace ShipWorks.ApplicationCore.Settings
{
    /// <summary>
    /// Option page for keyboard and barcode shortcuts
    /// </summary>
    public partial class SettingsPageScanToShip : SettingsPageBase
    {
        private readonly IScannerConfigurationRepository scannerRepo;
        private IUserSession userSession;
        private readonly IMessenger messenger;
        private readonly IMessageHelper messageHelper;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private UserSettingsEntity settings;
        private readonly IWin32Window owner;
        private readonly ILifetimeScope scope;
        private SingleScanSettings singleScanSettingsOnLoad;
        private readonly IPrintJobFactory printJobFactory;
        private IDisposable singleScanShortcutMessage;
        private readonly bool warehouseEnabled;

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsPageScanToShip(IWin32Window owner, ILifetimeScope scope)
        {
            InitializeComponent();
            scannerRepo = scope.Resolve<IScannerConfigurationRepository>();
            messageHelper = scope.Resolve<IMessageHelper>();
            sqlAdapterFactory = scope.Resolve<ISqlAdapterFactory>();
            userSession = scope.Resolve<IUserSession>();
            messenger = scope.Resolve<IMessenger>();
            printJobFactory = scope.Resolve<IPrintJobFactory>();
            settings = userSession?.User?.Settings;
            this.owner = owner;
            this.scope = scope;

            warehouseEnabled = scope.Resolve<ILicenseService>().CheckRestriction(EditionFeature.Warehouse, null) == EditionRestrictionLevel.None;
        }

        /// <summary>
        /// Save the selected settings
        /// </summary>
        public override void Save()
        {
            if (userSession.IsLoggedOn)
            {
                if (autoPrint.Checked)
                {
                    settings.SingleScanSettings = (int) SingleScanSettings.AutoPrint;
                }
                else if (enableScanner.Checked)
                {
                    settings.SingleScanSettings = (int) SingleScanSettings.Scan;
                }
                else
                {
                    settings.SingleScanSettings = (int) SingleScanSettings.Disabled;
                }

                settings.AutoWeigh = autoWeigh.Checked;
                settings.RequireVerificationToShip = requireVerificationToShip.Checked;

                settings.SingleScanConfirmationMode = (SingleScanConfirmationMode) Enum.Parse(typeof(SingleScanConfirmationMode), 
                    singleScanConfirmation.SelectedValue.ToString());

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
                singleScanShortcutMessage?.Dispose();

                // Listen for the single scan setting changing so we can reload it
                // wait 250ms so that the pipeline that is making the change has time to make it
                singleScanShortcutMessage = messenger.OfType<ShortcutMessage>()
                    .Where(s => s.AppliesTo(KeyboardShortcutCommand.ToggleAutoPrint))
                    .Delay(TimeSpan.FromMilliseconds(250))
                    .ObserveOn(this)
                    .Subscribe(ReloadSingleScanSetting);

                // Load single scan settings and update ui
                enableScanner.Checked = (SingleScanSettings) settings.SingleScanSettings != SingleScanSettings.Disabled;
                autoPrint.Checked = (SingleScanSettings) settings.SingleScanSettings == SingleScanSettings.AutoPrint;
                autoWeigh.Checked = settings.AutoWeigh;

                EnumHelper.BindComboBox<SingleScanConfirmationMode>(singleScanConfirmation);
                singleScanConfirmation.SelectedValue = (SingleScanConfirmationMode) settings.SingleScanConfirmationMode;

                LoadRequireVerificationSetting();

                requireVerificationToShip.Checked = settings.RequireVerificationToShip;

                UpdateSingleScanSettingsUI();

                singleScanSettingsOnLoad = (SingleScanSettings) settings.SingleScanSettings;
                UpdateToolTipHotkey();
            }
            else
            {
                Controls.Clear();

                Label label = new Label();
                label.Text = "You are not logged on.";
                label.Location = new System.Drawing.Point(10, 10);
                label.AutoSize = true;
                label.Font = new Font(Font, FontStyle.Bold);
                Controls.Add(label);
            }
        }

        /// <summary>
        /// Load the require verification before auto print setting
        /// </summary>
        private void LoadRequireVerificationSetting()
        {
            // If the user is on a warehouse plan, enable the require validation for auto print checkbox. If not,
            // disable the checkbox and set its value to false.
            if (warehouseEnabled)
            {
                infoTipVerificationWarehouseOnly.Visible = false;
                requireVerificationToShip.Enabled = true;
            }
            else
            {
                settings.RequireVerificationToShip = false;
                infoTipVerificationWarehouseOnly.Visible = true;
                requireVerificationToShip.Checked = false;
                requireVerificationToShip.Enabled = false;
            }
        }

        /// <summary>
        /// Update tool tips hotkey text
        /// </summary>
        private void UpdateToolTipHotkey()
        {
            IShortcutEntity toggleAutoPrintShortcut = scope.Resolve<IShortcutManager>()
                .Shortcuts.FirstOrDefault(s => s.Action == KeyboardShortcutCommand.ToggleAutoPrint);
            if (toggleAutoPrintShortcut != null)
            {
                infoTipAutoPrint.Title = $"Automatically Print Labels on Barcode Scan Search ({new KeyboardShortcutData(toggleAutoPrintShortcut).ShortcutText})";
            }
        }

        /// <summary>
        /// Unsubscribe from shortcut messages
        /// </summary>
        protected override void OnHandleDestroyed(EventArgs eventArgs)
        {
            base.OnHandleDestroyed(eventArgs);

            singleScanShortcutMessage?.Dispose();
        }

        /// <summary>
        /// Toggle the single scan setting
        /// </summary>
        private void ReloadSingleScanSetting(ShortcutMessage message)
        {
            // at this point we know the user settings have changed so we need a fresh copy
            // to save our changes to otherwise we will get a concurrency error
            userSession = scope.Resolve<IUserSession>();
            if (userSession.IsLoggedOn)
            {
                settings = userSession.User.Settings;
                autoPrint.Checked = (SingleScanSettings) settings.SingleScanSettings == SingleScanSettings.AutoPrint;
            }
        }

        /// <summary>
        /// Update auto print checkbox state based on single scan setting
        /// </summary>
        private void UpdateSingleScanSettingsUI()
        {
            // Only allow auto print to be checked when single scan is enabled
            if (!enableScanner.Checked)
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
            printJobFactory.CreateBarcodePrintJob().PreviewAsync((Form) owner);
        }
    }
}
