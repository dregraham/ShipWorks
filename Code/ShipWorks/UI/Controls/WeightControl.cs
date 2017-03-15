using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.IO.Hardware.Scales;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Crashes;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shared.IO.KeyboardShortcuts;
using ShipWorks.UI.Controls.Design;
using ShipWorks.UI.Utility;
using ShipWorks.Users;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Control for entering weight
    /// </summary>
    public partial class WeightControl : UserControl
    {
        public const string KeyboardShortcutTelemetryKey = "KeyboardShortcut";
        public const string ButtonTelemetryKey = "Button";
        public const string ShipmentQuantityTelemetryKey = "Shipment.Scale.Weight.Applied.ShipmentQuantity";
        public const string PackageQuantityTelemetryKey = "Shipment.Scale.Weight.Applied.PackageQuantity";

        // Display formatting
        WeightDisplayFormat displayFormat = WeightDisplayFormat.FractionalPounds;

        // The last valid weight that was entered
        double currentWeight = 0.0;

        // The last display we showed to the user.  Sometimes the display is a rounded (inaccurate)
        // version of the actual.  This allows us to see if the display has not changed, and instead
        // of parsing it (the rounded version) we just keep the accurate version.
        string lastDisplay = "";

        // Indicates if the weight control is "cleared", displaying no content
        bool cleared = false;

        // Controls if the weigh button and live weight display is shown
        bool showWeighButton = true;
        private bool ignoreWeightChanges;
        const int weightButtonArea = 123;
        private IDisposable scaleSubscription;

        // Raised whenever the value changes
        public event EventHandler WeightChanged;

        private bool showShortcutInfo = false;
        private IKeyboardShortcutTranslator keyboardShortcutTranslator = null;
        private string autoWeighShortcut;
        private string applyWeightShortcutText = string.Empty;
        private IDisposable keyboardShortcutSubscription;
        private Func<string, ITrackedDurationEvent> startDurationEvent;

        /// <summary>
        /// Constructor
        /// </summary>
        public WeightControl()
        {
            InitializeComponent();

            weighToolbar.Renderer = new NoBorderToolStripRenderer();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            if (DesignModeDetector.IsDesignerHosted())
            {
                return;
            }

            if (UserSession.IsLoggedOn)
            {
                displayFormat = (WeightDisplayFormat) UserSession.User.Settings.ShippingWeightFormat;
            }

            keyboardShortcutTranslator = IoC.UnsafeGlobalLifetimeScope.Resolve<IKeyboardShortcutTranslator>();
            autoWeighShortcut = "(" + keyboardShortcutTranslator.GetShortcut(KeyboardShortcutCommand.ApplyWeight) + ")";
            startDurationEvent = IoC.UnsafeGlobalLifetimeScope.Resolve<Func<string, ITrackedDurationEvent>>();

            weightInfo.Visible = ShowWeighButton;

            UpdateUiWithAutoWeighShortcuts();

            weightInfo.Text = applyWeightShortcutText;

            scaleSubscription?.Dispose();

            // Start the background thread that will monitor the current weight
            scaleSubscription = ScaleReader.ReadEvents
                .TakeWhile(_ => !(Program.MainForm.Disposing || Program.MainForm.IsDisposed || CrashDialog.IsApplicationCrashed))
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(x => UpdateLiveWeight(x.Status == ScaleReadStatus.Success ? x.Weight : (double?) null));

            keyboardShortcutSubscription?.Dispose();

            keyboardShortcutSubscription = Messenger.Current.OfType<KeyboardShortcutMessage>()
                .Where(m => m.AppliesTo(KeyboardShortcutCommand.ApplyWeight) &&
                            Visible &&
                            Enabled &&
                            AutoWeighShortCutsAllowed)
                .Subscribe(async _ => await ApplyWeightFromScaleAsync(KeyboardShortcutTelemetryKey));
        }

        /// <summary>
        /// Updates the UI to display any auto weigh shortcuts
        /// </summary>
        private void UpdateUiWithAutoWeighShortcuts()
        {
            // If we have shortcuts, display them
            if (AutoWeighShortCutsAllowed)
            {
                bool wasBlank = applyWeightShortcutText.IsNullOrWhiteSpace();
                
                // Only display the first shortcut.  The rest will be in a tool tip.
                applyWeightShortcutText = autoWeighShortcut;

                if (wasBlank || TopLevelControl == null || !TopLevelControl.Visible)
                {
                    weightInfo.Text = applyWeightShortcutText;
                }
            }
        }

        /// <summary>
        /// Should we concern ourselves with autoweigh shortcuts
        /// </summary>
        private bool AutoWeighShortCutsAllowed => !string.IsNullOrEmpty(autoWeighShortcut) && ShowWeighButton && !ReadOnly && ShowShortcutInfo;

        /// <summary>
        /// Get \ set the total weight
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(0)]
        [Obfuscation(Exclude = true)]
        public double Weight
        {
            get
            {
                // Always return the current weight rather than the parsed weight since
                // the current weight will be the most precise (i.e. it is not impacted
                // by rounding for display purposes).
                return currentWeight;
            }
            set
            {
                if (!ignoreWeightChanges)
                {
                    MultiValued = false;
                    cleared = false;

                    if (ValidateRange(value))
                    {
                        SetCurrentWeight(value);
                        ClearError();
                    }

                    FormatWeightText();
                }
            }
        }

        /// <summary>
        /// Get \ set whether the box represents a null value.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        [Obfuscation(Exclude = true)]
        public bool MultiValued
        {
            get { return textBox.MultiValued; }
            set { textBox.MultiValued = value; }
        }

        /// <summary>
        /// Minimum allowed range
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(0)]
        public double RangeMin { get; set; }

        /// <summary>
        /// Maximum allowed range
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(150)]
        public double RangeMax { get; set; } = 300;

        /// <summary>
        /// Determine the format to display the weight
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(WeightDisplayFormat.FractionalPounds)]
        public WeightDisplayFormat DisplayFormat
        {
            get
            {
                return displayFormat;
            }
            set
            {
                if (displayFormat == value)
                {
                    return;
                }

                displayFormat = value;

                if (UserSession.IsLoggedOn)
                {
                    UserSession.User.Settings.ShippingWeightFormat = (int) displayFormat;
                }

                if (!MultiValued && !cleared)
                {
                    FormatWeightText();
                }
            }
        }

        /// <summary>
        /// Controls if the weigh button and live weight display is visible
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool ShowWeighButton
        {
            get
            {
                return showWeighButton;
            }
            set
            {
                if (showWeighButton == value)
                {
                    return;
                }

                showWeighButton = value;

                weightInfo.Visible = showWeighButton;
                weighToolbar.Visible = showWeighButton;

                if (!showWeighButton)
                {
                    textBox.Width = Width;
                }
                else
                {
                    textBox.Width = Width - weightButtonArea;
                }

                UpdateUiWithAutoWeighShortcuts();
            }
        }

        /// <summary>
        /// Controls if the weight box is read only or not
        /// </summary>
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get
            {
                return textBox.ReadOnly;
            }
            set
            {
                textBox.ReadOnly = value;

                weighButton.Enabled = !value;

                UpdateUiWithAutoWeighShortcuts();
            }
        }

        /// <summary>
        /// Controls if the shortcut info is displayed
        /// </summary>
        [DefaultValue(false)]
        public bool ShowShortcutInfo
        {
            get
            {
                return showShortcutInfo;
            }
            set
            {
                showShortcutInfo = value;

                UpdateUiWithAutoWeighShortcuts();
            }
        }

        /// <summary>
        /// Clear the contents of the weight control.  The content will remained clear until
        /// the user types something or the Weight proper is assigned.
        /// </summary>
        [DefaultValue(false)]
        public bool Cleared
        {
            get
            {
                return cleared;
            }
            set
            {
                if (cleared == value)
                {
                    return;
                }

                if (value)
                {
                    textBox.Text = "";
                    lastDisplay = "";
                    currentWeight = 0;

                    cleared = true;
                }
                else
                {
                    currentWeight = 0;
                    cleared = false;

                    FormatWeightText();
                }
            }
        }

        /// <summary>
        /// Configure entity counts for telemetry
        /// </summary>
        public Action<ITrackedDurationEvent> ConfigureTelemetryEntityCounts { get; set; }

        /// <summary>
        /// Cut
        /// </summary>
        private void OnCut(object sender, EventArgs e)
        {
            textBox.Cut();
        }

        /// <summary>
        /// Copy
        /// </summary>
        private void OnCopy(object sender, EventArgs e)
        {
            textBox.Copy();
        }

        /// <summary>
        /// Paste
        /// </summary>
        private void OnPaste(object sender, EventArgs e)
        {
            textBox.Paste();
        }

        /// <summary>
        /// Switch to fractional pounds view
        /// </summary>
        private void OnFractionalPounds(object sender, EventArgs e)
        {
            DisplayFormat = WeightDisplayFormat.FractionalPounds;
        }

        /// <summary>
        /// Switch to pounds\ounces view
        /// </summary>
        private void OnPoundsOunces(object sender, EventArgs e)
        {
            DisplayFormat = WeightDisplayFormat.PoundsOunces;
        }

        /// <summary>
        /// Sets the parsed weight in the text
        /// </summary>
        private void SetParsedWeight()
        {
            // If its cleared, then no need to parse
            if (textBox.Text.Length == 0 && cleared)
            {
                return;
            }

            string text = textBox.Text;

            // If it has not changed since our last display, we do not have to do anything
            if (text == lastDisplay)
            {
                return;
            }

            double? parsedWeight = WeightConverter.Current.ParseWeight(textBox.Text);

            if (parsedWeight.HasValue)
            {
                SetCurrentWeight(parsedWeight.Value);
                ClearError();
            }
            else
            {
                SetError("The input was not valid.");
            }

            FormatWeightText();
        }

        /// <summary>
        /// Format the current weight value and insert it into the text box
        /// </summary>
        private void FormatWeightText()
        {
            if (MultiValued || cleared)
            {
                currentWeight = 0;
                lastDisplay = "";
                return;
            }

            FormatWeightText(currentWeight);
        }

        /// <summary>
        /// Format the specified weight value and insert it into the text box
        /// </summary>
        private void FormatWeightText(double weight)
        {
            if (DesignModeDetector.IsDesignerHosted())
            {
                return;
            }

            string result = WeightConverter.Current.FormatWeight(weight, displayFormat);

            lastDisplay = result;

            textBox.Text = result;
            textBox.SelectionStart = textBox.Text.Length;
        }

        /// <summary>
        /// Grab the weight from the scale
        /// </summary>
        private async void OnWeigh(object sender, EventArgs e)
        {
            await ApplyWeightFromScaleAsync(ButtonTelemetryKey);
        }

        /// <summary>
        /// Grab the weight from the scale
        /// </summary>
        private async Task<bool> ApplyWeightFromScaleAsync(string invocationMethod)
        {
            Cursor.Current = Cursors.WaitCursor;
            weighButton.Enabled = false;
            bool appliedWeight = false;

            using (ITrackedDurationEvent durationEvent = StartTrackingApplyWeight())
            {
                ScaleReadResult result = await ScaleReader.ReadScale();
                SetTelemetryProperties(durationEvent, result, invocationMethod);

                if (result.Status == ScaleReadStatus.Success)
                {
                    double newWeight = result.Weight;

                    if (ValidateRange(newWeight))
                    {
                        MultiValued = false;
                        cleared = false;

                        FormatWeightText(newWeight);
                        SetCurrentWeight(newWeight);
                        ClearError();
                        appliedWeight = true;

                        textBox.FlashBackground(250, Color.LightGray, 3);
                    }
                    else
                    {
                        FormatWeightText();
                    }
                }
                else
                {
                    SetError(result.Message);
                }

                weighButton.Enabled = true;
                return appliedWeight;
            }
        }

        /// <summary>
        /// Start tracking the apply weight command
        /// </summary>
        private ITrackedDurationEvent StartTrackingApplyWeight()
        {
            return AutoWeighShortCutsAllowed ?
                startDurationEvent("Shipment.Scale.Weight.Applied") :
                TrackedDurationEvent.Dummy;
        }

        /// <summary>
        /// Set telemetry properties when applying weight
        /// </summary>
        private void SetTelemetryProperties(ITrackedDurationEvent telemetryEvent, ScaleReadResult result, string invocationMethod)
        {
            telemetryEvent.AddProperty("Shipment.Scale.Weight.Applied.Source", ParentForm?.Name);
            telemetryEvent.AddProperty("Shipment.Scale.Weight.Applied.InvocationMethod", invocationMethod);
            telemetryEvent.AddProperty("Shipment.Scale.Weight.Applied.ScaleType", result.ScaleType.ToString());
            telemetryEvent.AddProperty("Shipment.Scale.Weight.Applied.ShortcutKey.Used",
                invocationMethod == KeyboardShortcutTelemetryKey ?
                    keyboardShortcutTranslator.GetShortcut(KeyboardShortcutCommand.ApplyWeight) :
                    "N/A");
            telemetryEvent.AddMetric("Shipment.Scale.Weight.Applied.ShortcutKey.ConfiguredQuantity", 1);
            ConfigureTelemetryEntityCounts?.Invoke(telemetryEvent);
        }

        /// <summary>
        /// Validate the range of the given value
        /// </summary>
        private bool ValidateRange(double weight)
        {
            if (weight >= RangeMin && weight <= RangeMax)
            {
                return true;
            }
            else
            {
                SetError("The input value was out of range.");

                return false;
            }
        }

        /// <summary>
        /// Finish editing so that the weight can be updated and saved
        /// </summary>
        public void FlushChanges()
        {
            if (!MultiValued)
            {
                SetParsedWeight();
            }
        }

        /// <summary>
        /// Set the value of the current weight to the given value.
        /// </summary>
        private void SetCurrentWeight(double newWeight)
        {
            if (currentWeight != newWeight)
            {
                currentWeight = newWeight;

                OnWeightChanged();
            }
        }

        /// <summary>
        /// Called whenever the weight changes.  Raises the WeightChanged event.
        /// </summary>
        protected virtual void OnWeightChanged()
        {
            WeightChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Trap the enter key
        /// </summary>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                SetParsedWeight();

                return true;
            }

            return base.ProcessDialogKey(keyData);
        }

        /// <summary>
        /// Control is losing focus
        /// </summary>
        protected override void OnLeave(EventArgs e)
        {
            FlushChanges();

            base.OnLeave(e);
        }

        /// <summary>
        /// Update the live weight display
        /// </summary>
        private bool UpdateLiveWeight(double? weight)
        {
            if (TopLevelControl == null || !TopLevelControl.Visible)
            {
                return false;
            }

            if (showWeighButton)
            {
                if (weight != null)
                {
                    weightInfo.Text = $@"{WeightConverter.Current.FormatWeight(weight.Value)}  {applyWeightShortcutText}";
                    weightInfo.Visible = true;

                    // Make sure the error is clear
                    ClearError();
                }
            }
            else
            {
                weightInfo.Visible = false;
            }

            return true;
        }

        /// <summary>
        /// Set the error message on the error provider to the given string
        /// </summary>
        private void SetError(string error)
        {
            errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            errorProvider.SetIconPadding(weighToolbar, 3);
            errorProvider.SetError(weighToolbar, error);
            
            // Position the live weight/shortcut to the side of the error provider.
            weightInfo.Left = weighToolbar.Right + 21;
        }

        /// <summary>
        /// Clear the current error provider display
        /// </summary>
        private void ClearError()
        {
            if (!errorProvider.GetError(weighToolbar).IsNullOrWhiteSpace())
            {
                errorProvider.SetError(weighToolbar, null);

                // Position the live weight/shortcut back where it belongs.
                weightInfo.Left = weighToolbar.Right;
            }
        }

        /// <summary>
        /// Contents of the weight entry box have changed
        /// </summary>
        private void OnTextBoxChanged(object sender, EventArgs e)
        {
            if (WeightConverter.Current.ParseWeight(textBox.Text).HasValue)
            {
                ignoreWeightChanges = true;
                OnWeightChanged();
                ignoreWeightChanges = false;
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                scaleSubscription?.Dispose();
                components?.Dispose();
                keyboardShortcutTranslator = null;
                keyboardShortcutSubscription?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
