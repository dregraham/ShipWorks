﻿using System;
using System.ComponentModel;
using System.Drawing;
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
using ShipWorks.IO.KeyboardShortcuts;
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
        private WeightDisplayFormat displayFormat = WeightDisplayFormat.FractionalPounds;

        // The last valid result that was provided
        private ScaleReadResult? currentResult = null;

        // The last display we showed to the user.  Sometimes the display is a rounded (inaccurate)
        // version of the actual.  This allows us to see if the display has not changed, and instead
        // of parsing it (the rounded version) we just keep the accurate version.
        private string lastDisplay = "";

        // Indicates if the weight control is "cleared", displaying no content
        private bool cleared;

        // Controls if the weigh button and live weight display is shown
        private bool showWeighButton = true;
        private bool ignoreWeightChanges;
        private const int WeightButtonArea = 123;
        private IDisposable scaleSubscription;

        // Raised whenever the value changes
        public event EventHandler<WeightChangedEventArgs> WeightChanged;

        private bool showShortcutInfo;
        private string weighShortcutText;
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

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IShortcutManager shortcutManager = lifetimeScope.Resolve<IShortcutManager>();
                ShortcutEntity weighShortcut = shortcutManager.GetWeighShortcut();
                weighShortcutText = "(" + new KeyboardShortcutData(weighShortcut).ShortcutText + ")";
            }
            
            startDurationEvent = IoC.UnsafeGlobalLifetimeScope.Resolve<Func<string, ITrackedDurationEvent>>();

            weightInfo.Visible = ShowWeighButton;

            UpdateUiWithWeighShortcuts();

            weightInfo.Text = applyWeightShortcutText;

            scaleSubscription?.Dispose();

            // Start the background thread that will monitor the current weight
            scaleSubscription = ScaleReader.ReadEvents
                .TakeWhile(_ => !(Program.MainForm.Disposing || Program.MainForm.IsDisposed || CrashDialog.IsApplicationCrashed))
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(x => UpdateLiveWeight(x.Status == ScaleReadStatus.Success ? x.Weight : (double?) null));

            keyboardShortcutSubscription?.Dispose();

            keyboardShortcutSubscription = Messenger.Current.OfType<ShortcutMessage>()
                .Where(m => WeighShortCutsAllowed &&
                            m.AppliesTo(KeyboardShortcutCommand.ApplyWeight) &&
                            Visible &&
                            Enabled)
                .Subscribe(async _ => await ApplyWeightFromScaleAsync(KeyboardShortcutTelemetryKey));
        }

        /// <summary>
        /// Updates the UI to display any weigh shortcuts
        /// </summary>
        private void UpdateUiWithWeighShortcuts()
        {
            // If we have shortcuts, display them
            if (WeighShortCutsAllowed)
            {
                bool wasBlank = applyWeightShortcutText.IsNullOrWhiteSpace();

                // Only display the first shortcut.  The rest will be in a tool tip.
                applyWeightShortcutText = weighShortcutText;

                if (wasBlank || TopLevelControl == null || !TopLevelControl.Visible)
                {
                    weightInfo.Text = applyWeightShortcutText;
                }
            }
        }

        /// <summary>
        /// Should we concern ourselves with weigh shortcuts
        /// </summary>
        private bool WeighShortCutsAllowed => !string.IsNullOrEmpty(weighShortcutText) && ShowWeighButton && !ReadOnly && ShowShortcutInfo;

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
                return currentResult?.Weight ?? 0D;
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
                    else
                    {
                        SetCurrentWeight(value.Clamp(RangeMin, RangeMax));
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
                    textBox.Width = Width - WeightButtonArea;
                }

                UpdateUiWithWeighShortcuts();
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

                UpdateUiWithWeighShortcuts();
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

                UpdateUiWithWeighShortcuts();
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
                    SetCurrentWeight(0);

                    cleared = true;
                }
                else
                {
                    SetCurrentWeight(0);
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
                parsedWeight = parsedWeight.Value.Clamp(RangeMin, RangeMax);

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
                SetCurrentWeight(0);
                lastDisplay = "";
                return;
            }

            FormatWeightText(currentResult?.Weight ?? 0);
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
                        SetCurrentWeight(result);
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
            return WeighShortCutsAllowed ?
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

            SetError("The input value was out of range.");

            return false;
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
        /// Sets the current weight. Raises event if it has changed
        /// </summary>
        private void SetCurrentWeight(ScaleReadResult result)
        {
            if (currentResult == null || 
                currentResult.Value.Weight != result.Weight ||
                (result.HasVolumeDimensions &&
                    (currentResult.Value.Length != result.Length ||
                    currentResult.Value.Width != result.Width ||
                    currentResult.Value.Height != result.Height)))
            {
                currentResult = result;
                OnWeightChanged(result);
            }
        }

        /// <summary>
        /// Set the value of the current weight to the given value.
        /// </summary>
        private void SetCurrentWeight(double newWeight)
        {
            SetCurrentWeight(ScaleReadResult.Success(newWeight, ScaleType.None));
        }

        /// <summary>
        /// Called whenever the weight changes.  Raises the WeightChanged event.
        /// </summary>
        protected virtual void OnWeightChanged(ScaleReadResult result)
        {
            WeightChanged?.Invoke(this, new WeightChangedEventArgs(result));
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
        private void UpdateLiveWeight(double? weight)
        {
            if (TopLevelControl == null || !TopLevelControl.Visible)
            {
                return;
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
            var weight = WeightConverter.Current.ParseWeight(textBox.Text);
            if (weight.HasValue)
            {
                ignoreWeightChanges = true;
                OnWeightChanged(ScaleReadResult.Success(weight.Value, ScaleType.None));
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
                keyboardShortcutSubscription?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
