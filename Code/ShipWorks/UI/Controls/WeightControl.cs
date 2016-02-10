using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Interapptive.Shared.IO.Hardware.Scales;
using ShipWorks.ApplicationCore.Crashes;
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

            liveWeight.Visible = false;

            scaleSubscription?.Dispose();

            // Start the background thread that will monitor the current weight
            scaleSubscription = ScaleReader.ReadEvents
                .TakeWhile(_ => !(Program.MainForm.Disposing || Program.MainForm.IsDisposed || CrashWindow.IsApplicationCrashed))
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(x => UpdateLiveWeight(x.Status == ScaleReadStatus.Success ? x.Weight : (double?) null));
        }

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

                liveWeight.Visible = showWeighButton;
                weighToolbar.Visible = showWeighButton;

                if (!showWeighButton)
                {
                    textBox.Width = Width;
                }
                else
                {
                    textBox.Width = Width - weightButtonArea;
                }
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
            Cursor.Current = Cursors.WaitCursor;
            weighButton.Enabled = false;

            ScaleReadResult result = await ScaleReader.ReadScale();

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
                    liveWeight.Text = string.Format("({0})", WeightConverter.Current.FormatWeight(weight.Value));
                    liveWeight.Visible = true;

                    // Make sure the error is clear
                    ClearError();
                }
                else
                {
                    liveWeight.Visible = false;
                }
            }
            else
            {
                liveWeight.Visible = false;
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

            // Make sure live weight is not visible
            liveWeight.Visible = false;
        }

        /// <summary>
        /// Clear the current error provider display
        /// </summary>
        private void ClearError()
        {
            errorProvider.SetError(weighToolbar, null);
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
    }
}
