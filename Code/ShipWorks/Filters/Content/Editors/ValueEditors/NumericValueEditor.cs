using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared;
using ShipWorks.Filters.Content.Conditions;
using System.Globalization;
using Interapptive.Shared.Utility;
using ComponentFactory.Krypton.Toolkit;

namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    /// <summary>
    /// Base editor for numeric values
    /// </summary>
    public partial class NumericValueEditor<T> : ValueEditor where T: struct, IComparable
    {
        NumericCondition<T> condition;

        string format = "G";

        T? minimumValue = null;
        T? maximumValue = null;

        bool suspendChangeNotification = false;

        // Indicates if the operator should be hidden.  This is useful for scenarios in 
        // which this control is reused.
        bool hideOperator = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public NumericValueEditor(NumericCondition<T> condition)
        {
            InitializeComponent();

            this.condition = condition;

            labelOperator.InitializeFromEnumType(typeof(NumericOperator));
            labelOperator.SelectedValue = condition.Operator;

            UpdateValueVisibility();
        }

        /// <summary>
        /// Loading
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            PopulateValues();

            value1.TextChanged += new EventHandler(OnTargetTextChanged);
            value2.TextChanged += new EventHandler(OnTargetTextChanged);
        }

        /// <summary>
        /// Put the values into the boxes
        /// </summary>
        private void PopulateValues()
        {
            value1.Text = FormatValue(condition.Value1);
            value2.Text = FormatValue(condition.Value2);
        }

        /// <summary>
        /// The format specifier to use in displaying the value
        /// </summary>
        public string Format
        {
            get
            {
                return format;
            }
            set
            {
                format = value;
            }
        }

        /// <summary>
        /// The minimum value a field is allowed to be. 
        /// </summary>
        public T? MinimumValue
        {
            get
            {
                return minimumValue;
            }
            set
            {
                minimumValue = value;
            }
        }

        /// <summary>
        /// The minimum value a field is allowed to be. 
        /// </summary>
        public T? MaximumValue
        {
            get
            {
                return maximumValue;
            }
            set
            {
                maximumValue = value;
            }
        }

        /// <summary>
        /// Hide the operator selector.  This is useful for scenarios in which the value portion
        /// is reused by another control.
        /// </summary>
        public bool HideOperator
        {
            get
            {
                return hideOperator;
            }
            set
            {
                if (hideOperator != value)
                {
                    hideOperator = value;

                    UpdateValueVisibility();
                }
            }
        }

        /// <summary>
        /// The NumericOperator currently shown by the editor.
        /// </summary>
        public NumericOperator NumericOperator
        {
            get
            {
                return (NumericOperator) labelOperator.SelectedValue;
            }
            set
            {
                if (value != (NumericOperator) labelOperator.SelectedValue)
                {
                    labelOperator.SelectedValue = value;

                    OnChangeOperator(labelOperator, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// The current values
        /// </summary>
        public T Value1
        {
            get
            {
                return condition.Value1;
            }
            set
            {
                condition.Value1 = value;
                value1.Text = FormatValue(value);
            }
        }

        /// <summary>
        /// The current values
        /// </summary>
        public T Value2
        {
            get
            {
                return condition.Value2;
            }
            set
            {
                condition.Value2 = value;
                value2.Text = FormatValue(value);
            }
        }

        /// <summary>
        /// Update the visibility of the value operators
        /// </summary>
        private void UpdateValueVisibility()
        {
            NumericOperator op = condition.Operator;

            bool isComparison =
             (op == NumericOperator.Between) ||
             (op == NumericOperator.NotBetween);

            labelAnd.Visible = isComparison;
            value2.Visible = isComparison;

            if (isComparison)
            {
                valuePanel.Width = value2.Right + errorSpace;
            }
            else
            {
                valuePanel.Width = value1.Right + errorSpace;
            }

            labelOperator.Visible = !hideOperator;

            valuePanel.Left = hideOperator ? 3 : labelOperator.Right + 3;
            Width = valuePanel.Right;
        }

        /// <summary>
        /// Operator type is changing
        /// </summary>
        private void OnChangeOperator(object sender, EventArgs e)
        {
            NumericOperator op = (NumericOperator) labelOperator.SelectedValue;

            condition.Operator = op;

            UpdateValueVisibility();

            RaiseContentChanged();
        }

        /// <summary>
        /// The text in the value boxes has changed
        /// </summary>
        void OnTargetTextChanged(object sender, EventArgs e)
        {
            if (!suspendChangeNotification)
            {
                RaiseContentChanged();
            }
        }

        /// <summary>
        /// Format the value based on the configured format specifier
        /// </summary>
        private string FormatValue(T value)
        {
            return string.Format("{0:" + format + "}", value);
        }

        /// <summary>
        /// Value1 is being validated
        /// </summary>
        private void OnValidatingValue1(object sender, CancelEventArgs e)
        {
            T value;
            if (TryValidate(value1, out value))
            {
                condition.Value1 = value;
            }
            else
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Value2 is being validated
        /// </summary>
        private void OnValidatingValue2(object sender, CancelEventArgs e)
        {
            T value;
            if (TryValidate(value2, out value))
            {
                condition.Value2 = value;
            }
            else
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Try to validate the context of the control, and return the value if successful
        /// </summary>
        private bool TryValidate(TextBox textBox, out T value)
        {
            if (TryParse(textBox.Text, out value))
            {
                // Now we have to validate the range
                if (ValidateRange(value))
                {
                    ClearError(textBox);

                    // Update the formatting, if the user's not in the box right now
                    if (!textBox.Focused)
                    {
                        // Since the value isnt changing, dont raise the value change event when we do this
                        suspendChangeNotification = true;
                        textBox.Text = FormatValue(value);
                        suspendChangeNotification = false;
                    }

                    return true;
                }
                else
                {
                    SetError(textBox, "The value entered is out of range:\n    " + GetRangeDescription());
                }
            }
            else
            {
                SetError(textBox, "The value entered is not valid.");
            }

            return false;
        }

        /// <summary>
        /// Validate that the value falls within the configured range
        /// </summary>
        private bool ValidateRange(T value)
        {
            if (minimumValue != null && value.CompareTo(minimumValue.Value) < 0)
            {
                return false;
            }

            if (maximumValue != null && value.CompareTo(maximumValue.Value) > 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get the description of the range restrictions
        /// </summary>
        private string GetRangeDescription()
        {
            if (minimumValue != null && maximumValue == null)
            {
                return string.Format("The value cannot be less than {0}.", minimumValue);
            }

            if (maximumValue != null && minimumValue == null)
            {
                return string.Format("The value cannot be greater than {0}.", maximumValue);
            }

            if (minimumValue != null && maximumValue != null)
            {
                return string.Format("The value must be between {0} and {1}.", minimumValue, maximumValue);
            }

            return "";
        }

        /// <summary>
        /// Try to parse the given text into a numeric value
        /// </summary>
        public static bool TryParse(string text, out T value)
        {
            bool result = false;
            object parsed = default(T);

            switch (typeof(T).Name)
            {
                case "Int32":
                    {
                        int parsedInt;
                        result = Int32.TryParse(text, out parsedInt);

                        if (result)
                        {
                            parsed = parsedInt;
                        }
                        
                        break;
                    }

                case "Int64":
                    {
                        long parsedLong;
                        result = Int64.TryParse(text, out parsedLong);

                        if (result)
                        {
                            parsed = parsedLong;
                        }

                        break;
                    }

                case "Double":
                    {
                        double parsedDouble;
                        result = Double.TryParse(text, out parsedDouble);

                        if (result)
                        {
                            parsed = parsedDouble;
                        }

                        break;
                    }

                case "Decimal":
                    {
                        decimal parsedDecimal;
                        result = Decimal.TryParse(text, NumberStyles.Currency, null, out parsedDecimal);

                        if (result)
                        {
                            parsed = parsedDecimal;
                        }

                        break;
                    }

                default:
                    {
                        try
                        {
                            parsed = Convert.ChangeType(text, typeof(T));
                            result = true;
                        }
                        catch (InvalidCastException)
                        {
                            result = false;
                        }

                        break;
                    }
            }

            value = (T) parsed;
            return result;
        }
    }
}
