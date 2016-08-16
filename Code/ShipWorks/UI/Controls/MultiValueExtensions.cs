using System;
using System.Drawing;
using System.Windows.Forms;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Utility class for working with controls that can have multiple values due to multiple selection.
    /// </summary>
    public static class MultiValueExtensions
    {
        public static string MultiText
        {
            get { return "(Multiple Values)"; }
        }

        public static Color MultiColor
        {
            get { return SystemColors.GrayText; }
        }

        /// <summary>
        /// Read a value from the given text box.  If its multiValued, the assigner is not called.
        /// </summary>
        public static void ReadMultiText(this MultiValueTextBox textBox, Action<string> assigner)
        {
            if (!textBox.MultiValued)
            {
                assigner(textBox.Text);
            }
        }

        /// <summary>
        /// Read a value from the given ComboBox  If its multiValued, the assigner is not called.
        /// </summary>
        public static void ReadMultiText(this MultiValueComboBox comboBox, Action<string> assigner)
        {
            if (!comboBox.MultiValued)
            {
                assigner(comboBox.Text);
            }
        }

        /// <summary>
        /// Read a value from the given ComboBox  If its multiValued, the assigner is not called.
        /// </summary>
        public static void ReadMultiValue(this MultiValueComboBox comboBox, Action<object> assigner)
        {
            if (!comboBox.MultiValued)
            {
                assigner(comboBox.SelectedValue);
            }
        }

        /// <summary>
        /// Read a value from the given checkbox.  If its multiValued, the assigner is not called.
        /// </summary>
        public static void ReadMultiCheck(this CheckBox checkBox, Action<bool> assigner)
        {
            if (checkBox.CheckState != CheckState.Indeterminate)
            {
                assigner(checkBox.Checked);
            }
        }

        /// <summary>
        /// Read a value from the given DateTimePicker.  If its multiValued, the assigner is not called.
        /// </summary>
        public static void ReadMultiDate(this MultiValueDateTimePicker datePicker, Action<DateTime> assigner)
        {
            if (!datePicker.MultiValued)
            {
                assigner(datePicker.Value);
            }
        }

        /// <summary>
        /// Read a value from the given WeightControl.  If its multiValued, the assigner is not called.
        /// </summary>
        public static void ReadMultiWeight(this WeightControl weight, Action<double> assigner)
        {
            if (!weight.MultiValued)
            {
                assigner(weight.Weight);
            }
        }

        /// <summary>
        /// Read a value from the MoneyTextBox.  If its multiValues, the assigner is not called
        /// </summary>
        public static void ReadMultiAmount(this MoneyTextBox moneyTextBox, Action<decimal> assigner)
        {
            if (!moneyTextBox.MultiValued)
            {
                assigner(moneyTextBox.Amount);
            }
        }

        /// <summary>
        /// Read a value from the TemplateTokenTextBoix.  If its multiValues, the assigner is not called
        /// </summary>
        public static void ReadMultiText(this TemplateTokenTextBox tokenBox, Action<string> assigner)
        {
            if (!tokenBox.MultiValued)
            {
                assigner(tokenBox.Text);
            }
        }

        /// <summary>
        /// Check to see if assigning the given value would make the specified TextBox be in mulitvalue mode
        /// </summary>
        public static void ApplyMultiText(this MultiValueTextBox textBox, string value)
        {
            if (MultiValueScope.Current == null)
            {
                throw new InvalidOperationException("Cannot be used when there is no MultiValueScope in scope.");
            }

            MultiValueScope.Current.ApplyMultiText(textBox, value);
        }

        /// <summary>
        /// Check to see if assigning the given text would make the specified ComboBox be in mulitvalue mode
        /// </summary>
        public static void ApplyMultiText(this MultiValueComboBox comboBox, string text)
        {
            if (MultiValueScope.Current == null)
            {
                throw new InvalidOperationException("Cannot be used when there is no MultiValueScope in scope.");
            }

            MultiValueScope.Current.ApplyMultiText(comboBox, text);
        }

        /// <summary>
        /// Check to see if assigning the given text would make the specified ComboBox be in mulitvalue mode
        /// </summary>
        public static void ApplyMultiValue(this MultiValueComboBox comboBox, object value)
        {
            if (MultiValueScope.Current == null)
            {
                throw new InvalidOperationException("Cannot be used when there is no MultiValueScope in scope.");
            }

            MultiValueScope.Current.ApplyMultiValue(comboBox, value);
        }

        /// <summary>
        /// Check to see if assigning the given value would make the specified CheckBox be in mulitvalue mode
        /// </summary>
        public static void ApplyMultiCheck(this CheckBox checkBox, bool value)
        {
            if (MultiValueScope.Current == null)
            {
                throw new InvalidOperationException("Cannot be used when there is no MultiValueScope in scope.");
            }

            MultiValueScope.Current.ApplyMultiCheck(checkBox, value);
        }

        /// <summary>
        /// Check to see if assigning the given value would make the specified DatePicker in multivalue mode
        /// </summary>
        public static void ApplyMultiDate(this MultiValueDateTimePicker datePicker, DateTime value)
        {
            if (MultiValueScope.Current == null)
            {
                throw new InvalidOperationException("Cannot be used when there is no MultiValueScope in scope.");
            }

            MultiValueScope.Current.ApplyMultiDate(datePicker, value);
        }

        /// <summary>
        /// Check to see if assigning the given value would make the specified WeightControl in multivalue mode
        /// </summary>
        public static void ApplyMultiWeight(this WeightControl weightControl, double value)
        {
            if (MultiValueScope.Current == null)
            {
                throw new InvalidOperationException("Cannot be used when there is no MultiValueScope in scope.");
            }

            MultiValueScope.Current.ApplyMultiWeight(weightControl, value);
        }

        /// <summary>
        /// Check to see if assigning the given value would make the MoneyTextBox in multivalue mode
        /// </summary>
        public static void ApplyMultiAmount(this MoneyTextBox moneyTextBox, decimal amount)
        {
            if (MultiValueScope.Current == null)
            {
                throw new InvalidOperationException("Cannot be used when there is no MultiValueScope in scope.");
            }

            MultiValueScope.Current.ApplyMultiAmount(moneyTextBox, amount);
        }

        /// <summary>
        /// Check to see if assigning the given value would make the TemplateTokenTextBox in multivalue mode
        /// </summary>
        public static void ApplyMultiText(this TemplateTokenTextBox tokenBox, string text)
        {
            if (MultiValueScope.Current == null)
            {
                throw new InvalidOperationException("Cannot be used when there is no MultiValueScope in scope.");
            }

            MultiValueScope.Current.ApplyMultiText(tokenBox, text);
        }
    }
}
