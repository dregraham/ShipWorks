﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Utility;
using System.Text.RegularExpressions;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using ShipWorks.Users;
using System.Threading;
using ShipWorks.ApplicationCore.Crashes;
using System.Diagnostics;
using ShipWorks.UI.Controls.Design;
using Interapptive.Shared.IO.Hardware.Scales;
using ShipWorks.Common.Threading;
using Interapptive.Shared.Business;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Control for entering weight
    /// </summary>
    public partial class WeightControl : UserControl
    {
        // Match any integer or floating point number
        static string numberRegex = @"-?([0-9]+(\.[0-9]*)?|\.[0-9]+)";

        // Match the case where both pounds and ounces are present
        Regex poundsOzRegex = new Regex(
            @"^(?<Pounds>" + numberRegex + @")\s*(lbs.|lbs|lb.|lb|l|pounds|pound)?\s+" +
            @"(?<Ounces>" + numberRegex + @")\s*(ounces|ounce|oz.|oz|o)?\s*$",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        // Match the case were only ounces is present
        Regex ouncesRegex = new Regex(
            @"^(?<Ounces>" + numberRegex + @")\s*(ounces|ounce|oz.|oz|o)\s*$",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        // Match the case were only pounds is present
        Regex poundsRegex = new Regex(
            @"^(?<Pounds>" + numberRegex + @")\s*(lbs.|lbs|lb.|lb|l|pounds|pound)?\s*$",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        // Allowable ranges for the weight
        double minRange = 0.0;
        double maxRange = 300.0;

        // Display formatting
        WeightDisplayFormat displayFormat = WeightDisplayFormat.FractionalPounds;

        // The last valid weight that was entered
        double currentWeight = 0.0;

        // The last display we showed to the user.  Sometimes the display is a rounded (innacurate)
        // version of the actual.  This allows us to see if the display has not changed, and instead
        // of parsing it (the rounded version) we just keep the accurate version.
        string lastDisplay = "";

        // Indicates if the weight control is "cleared", displaying no content
        bool cleared = false;

        // Controls if the weigh button and live weight display is shown
        bool showWeighButton = true;
        private bool ignoreWeightChanges;
        const int weightButtonArea = 123;

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
            if (DesignModeDetector.IsDesignerHosted(this))
            {
                return;
            }

            if (UserSession.IsLoggedOn)
            {
                displayFormat = (WeightDisplayFormat) UserSession.User.Settings.ShippingWeightFormat;
            }

            liveWeight.Visible = false;

            // Start the background thread that will monitor the current weight
            Thread thread = new Thread(ExceptionMonitor.WrapThread(ThreadWeightMonitor));
            thread.IsBackground = true;
            thread.Name = "WeightMonitor";
            thread.Start();
        }

        /// <summary>
        /// Get \ set the total weight
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(0)]
        public double Weight
        {
            get
            {
                double? parsedWeight = ParseWeight(textBox.Text);

                return parsedWeight.HasValue ? parsedWeight.Value : currentWeight;
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
        public double RangeMin
        {
            get
            {
                return minRange;
            }
            set
            {
                minRange = value;
            }
        }

        /// <summary>
        /// Maxiumum allowed range
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(150)]
        public double RangeMax
        {
            get
            {
                return maxRange;
            }
            set
            {
                maxRange = value;
            }
        }

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
        /// Controls if the weight box is readonly or not
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
        /// the user types something or the Weight propery is assigned.
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

        private double? ParseWeight(string weight)
        {
            double? newWeight = 0.0;

            try
            {
                // See if both pounds and ounces are present
                Match poundsOzMatch = poundsOzRegex.Match(weight);

                // Did it match
                if (poundsOzMatch.Success)
                {
                    double pounds = double.Parse(poundsOzMatch.Groups["Pounds"].Value);
                    double ounces = double.Parse(poundsOzMatch.Groups["Ounces"].Value);

                    newWeight = pounds + ounces / 16.0;
                }

                else
                {
                    // Now see if just ounces are present
                    Match ouncesMatch = ouncesRegex.Match(weight);

                    // Did it match
                    if (ouncesMatch.Success)
                    {
                        newWeight = double.Parse(ouncesMatch.Groups["Ounces"].Value) / 16.0;
                    }

                    else
                    {
                        // Now see if just pounds are present
                        Match poundsMatch = poundsRegex.Match(weight);

                        // Did it match
                        if (poundsMatch.Success)
                        {
                            newWeight = double.Parse(poundsMatch.Groups["Pounds"].Value);
                        }
                        else
                        {
                            // Nothing worked!
                            newWeight = null;
                        }
                    }
                }

                // Ensure the range is valid
                if (newWeight.HasValue && ValidateRange(newWeight.Value))
                {
                    return newWeight;
                }
            }
            catch (FormatException)
            {
                // There's nothing to do here; a failed parsed will just return null
            }

            return null;
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

            double? parsedWeight = ParseWeight(textBox.Text);

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
        /// Format the weight value
        /// </summary>
        private void FormatWeightText()
        {
            if (MultiValued || cleared)
            {
                currentWeight = 0;
                lastDisplay = "";
                return;
            }

            string result = FormatWeight(currentWeight, (WeightDisplayFormat)UserSession.User.Settings.ShippingWeightFormat);

            lastDisplay = result;

            textBox.Text = result;
            textBox.SelectionStart = textBox.Text.Length;
        }

        /// <summary>
        /// Format the given weight based on the specified display format.
        /// </summary>
        public static string FormatWeight(double weight, WeightDisplayFormat displayFormat)
        {
            string result;
            
            if (displayFormat == WeightDisplayFormat.FractionalPounds)
            {
                result = string.Format("{0:0.0#} lbs", weight);
            }
            else
            {
                WeightValue weightValue = new WeightValue(weight);

                result = string.Format("{0} lbs  {1} oz", weightValue.PoundsOnly, Math.Round(weightValue.OuncesOnly, 1, MidpointRounding.AwayFromZero));
            }

            return result;
        }

        /// <summary>
        /// Grab the weight from the scale
        /// </summary>
        private void OnWeigh(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            ScaleReadResult result = ScaleReader.ReadScale();

            if (result.Status == ScaleReadStatus.Success)
            {
                double newWeight = result.Weight;

                if (ValidateRange(newWeight))
                {
                    SetCurrentWeight(newWeight);
                    ClearError();

                    MultiValued = false;
                    cleared = false;
                }

                FormatWeightText();
            }
            else
            {
                SetError(result.Message);
            }
        }

        /// <summary>
        /// Validate the range of the given value
        /// </summary>
        private bool ValidateRange(double weight)
        {
            if (weight >= minRange && weight <= maxRange)
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
        /// Set the value of the curret weight to the given value.
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
            if (WeightChanged != null)
            {
                WeightChanged(this, EventArgs.Empty);
            }
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
            if (!MultiValued)
            {
                SetParsedWeight();
            }

 	        base.OnLeave(e);
        }

        /// <summary>
        /// Background thread to monitor and poll for the current weight
        /// </summary>
        private void ThreadWeightMonitor()
        {
            ScaleReadResult lastResult = null;

            while (true)
            {
                ScaleReadResult thisResult = ScaleReader.ReadScale(true);

                // Quit when disposed or crashed
                if (this.IsDisposed || this.Disposing || Program.MainForm.Disposing || Program.MainForm.IsDisposed || CrashWindow.IsApplicationCrashed)
                {
                    break;
                }
                
                // Don't bother if nothing has changed or we arent visible
                if (Visible && Program.MainForm.Visible && !object.Equals(lastResult, thisResult))
                {
                    bool keepGoing = (bool) Program.MainForm.Invoke(new Func<double?, bool>(UpdateLiveWeight), (thisResult.Status == ScaleReadStatus.Success) ? thisResult.Weight : (double?) null);

                    if (!keepGoing)
                    {
                        break;
                    }

                    lastResult = thisResult;
                }

                // Poll frequency controlled by this
                Thread.Sleep(250);
            }
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
                    liveWeight.Text = string.Format("({0})", FormatWeight(weight.Value, (WeightDisplayFormat)UserSession.User.Settings.ShippingWeightFormat));
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

        private void OnTextBoxChanged(object sender, EventArgs e)
        {
            if (ParseWeight(textBox.Text).HasValue)
            {
                ignoreWeightChanges = true;
                OnWeightChanged();
                ignoreWeightChanges = false;
            }
        }
    }
}
