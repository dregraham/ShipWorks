using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Creates a scope in which multiple values can be assigned, and if they are all the same, then the assignment takes place,
    /// otherwise the control is set to multi value.
    /// </summary>
    public class MultiValueScope : IDisposable
    {
        static MultiValueScope current;

        // Maps the controls to their current value set in scope
        Dictionary<object, object> valueMap = new Dictionary<object, object>();

        #region class ComboBoxData

        // Helper class to deal with the fact that ComboBox "values" can be the Text of the ComboBox, or the Value property
        class ComboBoxData
        {
            string text;
            object value;

            public ComboBoxData(string text)
            {
                this.text = text;
            }

            public ComboBoxData(object value)
            {
                this.value = value;
            }

            public string Text
            {
                get { return text; }
            }

            public object Value
            {
                get { return value; }
            }

            /// <summary>
            /// Equals
            /// </summary>
            public override bool Equals(object obj)
            {
                ComboBoxData other = obj as ComboBoxData;
                if ((object) other == null)
                {
                    return false;
                }

                if (value != null)
                {
                    return value.Equals(other.value);
                }

                return text == other.text;
            }

            /// <summary>
            /// Operator==
            /// </summary>
            public static bool operator ==(ComboBoxData left, ComboBoxData right)
            {
                return left.Equals(right);
            }

            /// <summary>
            /// Operator!=
            /// </summary>
            public static bool operator !=(ComboBoxData left, ComboBoxData right)
            {
                return !(left.Equals(right));
            }

            /// <summary>
            /// Hash code
            /// </summary>
            public override int GetHashCode()
            {
                return (text != null ? text.GetHashCode() : 0) + (value != null ? value.GetHashCode() : 0);
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public MultiValueScope()
        {
            if (current != null)
            {
                throw new InvalidOperationException("A MultiValueScope is already in scope.");
            }

            current = this;
        }

        /// <summary>
        /// Get the current object in scope.
        /// </summary>
        public static MultiValueScope Current
        {
            get { return current; }
        }

        /// <summary>
        /// Apply the given value to the TextBox
        /// </summary>
        public void ApplyMultiText(MultiValueTextBox textBox, string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            SaveToValueMap(textBox, value);
        }

        /// <summary>
        /// Apply the given text to the ComboBox
        /// </summary>
        public void ApplyMultiText(MultiValueComboBox comboBox, string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            SaveToValueMap(comboBox, new ComboBoxData(text));
        }

        /// <summary>
        /// Apply the given value to the ComboBox
        /// </summary>
        public void ApplyMultiValue(MultiValueComboBox comboBox, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            SaveToValueMap(comboBox, new ComboBoxData(value));
        }

        /// <summary>
        /// Apply the given value to the CheckBox
        /// </summary>
        public void ApplyMultiCheck(CheckBox checkBox, bool value)
        {
            SaveToValueMap(checkBox, value);
        }

        /// <summary>
        /// Apply the given value to the DateTimePicker
        /// </summary>
        public void ApplyMultiDate(MultiValueDateTimePicker datePicker, DateTime value)
        {
            SaveToValueMap(datePicker, value);
        }

        /// <summary>
        /// Apply the given value to the WeightControl
        /// </summary>
        public void ApplyMultiWeight(WeightControl weightControl, double value)
        {
            SaveToValueMap(weightControl, value);
        }

        /// <summary>
        /// Apply the given amount to the MoneyTextBox
        /// </summary>
        public void ApplyMultiAmount(MoneyTextBox moneyTextBox, decimal amount)
        {
            SaveToValueMap(moneyTextBox, amount);
        }

        /// <summary>
        /// Apply the given text to the TemplateTokenTextBox
        /// </summary>
        public void ApplyMultiText(TemplateTokenTextBox tokenBox, string text)
        {
            SaveToValueMap(tokenBox, text);
        }

        /// <summary>
        /// Apply the given value to the given control
        /// </summary>
        private void SaveToValueMap(object control, object value)
        {
            object currentValue;
            if (valueMap.TryGetValue(control, out currentValue))
            {
                // If it were null, its already considered a multivalue
                if (currentValue != null)
                {
                    if (!currentValue.Equals(value))
                    {
                        valueMap[control] = null;
                    }
                }
            }
            else
            {
                valueMap[control] = value;
            }
        }

        /// <summary>
        /// Terminate the scope
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        public void Dispose()
        {
            if (this == current)
            {
                // We have to clear us as the current scope before applying values.  Applying the values can kickoff events, which may cause 
                // other code to enter into a MultiValueScope, which means this one needs to be out of scope.
                current = null;

                foreach (KeyValuePair<object, object> pair in valueMap)
                {
                    MoneyTextBox moneyBox = pair.Key as MoneyTextBox;
                    if (moneyBox != null)
                    {
                        if (pair.Value != null)
                        {
                            if (pair.Value is string)
                            {
                                moneyBox.Text = (string) pair.Value;
                            }
                            else
                            {
                                moneyBox.Amount = (decimal) pair.Value;
                            }
                        }
                        else
                        {
                            moneyBox.MultiValued = true;
                        }

                        continue;
                    }

                    MultiValueTextBox textBox = pair.Key as MultiValueTextBox;
                    if (textBox != null)
                    {
                        if (pair.Value != null)
                        {
                            textBox.Text = (string) pair.Value;
                        }
                        else
                        {
                            textBox.MultiValued = true;
                        }

                        continue;
                    }

                    MultiValueComboBox comboBox = pair.Key as MultiValueComboBox;
                    if (comboBox != null)
                    {
                        if (pair.Value != null)
                        {
                            ComboBoxData data = (ComboBoxData) pair.Value;

                            if (data.Text != null)
                            {
                                comboBox.Text = data.Text;
                            }
                            else
                            {
                                comboBox.SelectedValue = data.Value;

                                // If the value is not a valid ComboBox value, pick the first one
                                if (comboBox.SelectedIndex == -1 && comboBox.Items.Count > 0)
                                {
                                    comboBox.SelectedIndex = 0;
                                }
                            }
                        }
                        else
                        {
                            comboBox.MultiValued = true;
                        }

                        continue;
                    }

                    CheckBox checkBox = pair.Key as CheckBox;
                    if (checkBox != null)
                    {
                        if (pair.Value != null)
                        {
                            checkBox.CheckState = (bool) pair.Value ? CheckState.Checked : CheckState.Unchecked;
                        }
                        else
                        {
                            checkBox.CheckState = CheckState.Indeterminate;
                        }

                        continue;
                    }

                    MultiValueDateTimePicker picker = pair.Key as MultiValueDateTimePicker;
                    if (picker != null)
                    {
                        if (pair.Value != null)
                        {
                            picker.Value = (DateTime) pair.Value;
                        }
                        else
                        {
                            picker.MultiValued = true;
                        }

                        continue;
                    }

                    WeightControl weightControl = pair.Key as WeightControl;
                    if (weightControl != null)
                    {
                        if (pair.Value != null)
                        {
                            weightControl.Weight = (double) pair.Value;
                        }
                        else
                        {
                            weightControl.MultiValued = true;
                        }

                        continue;
                    }

                    TemplateTokenTextBox tokenBox = pair.Key as TemplateTokenTextBox;
                    if (tokenBox != null)
                    {
                        if (pair.Value != null)
                        {
                            tokenBox.Text = (string) pair.Value;
                        }
                        else
                        {
                            tokenBox.MultiValued = true;
                        }

                        continue;
                    }
                }

                valueMap = null;
            }
        }
    }
}
