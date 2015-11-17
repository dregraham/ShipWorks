using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.UI.Controls;
using ShipWorks.Shipping.Editing;
using ShipWorks.Templates.Tokens;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Core shipping profile UI functionality
    /// </summary>
    public partial class ShippingProfileControlCore : UserControl
    {
        class CheckStateMapping
        {
            public EntityBase2 Entity { get; set; }
            public EntityField2 Field { get; set; }
            public CheckBox CheckBox { get; set; }
            public Control DataControl { get; set; }
            public Control[] OtherControls { get; set; }
            public bool IsValueMapping { get; set; }
        }

        List<CheckStateMapping> checkStateMap = new List<CheckStateMapping>();

        bool allowChangeCheckState = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileControlCore()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Indicates if the check state is allowed to change (the checkstate mapping checkboxes are enabled)
        /// </summary>
        protected bool AllowChangeCheckState
        {
            get { return allowChangeCheckState; }
            set { allowChangeCheckState = value; }
        }

        /// <summary>
        /// Add a mapping entry that maps the given CheckBox to a data entry control, and any other controls
        /// that should be enabled\disabled based on the checkbox.  The value from the data entry control is automatically
        /// read and set back to the given field.
        /// </summary>
        protected void AddValueMapping(EntityBase2 entity, EntityField2 field, CheckBox checkBox, Control dataControl, params Control[] otherControls)
        {
            CheckStateMapping mapping = AddMapping(entity, field, checkBox, dataControl, otherControls);

            mapping.IsValueMapping = true;

            // If its checked, apply the current value
            if (checkBox.Checked)
            {
                ReadFieldValue(entity, field, dataControl);
            }
        }

        /// <summary>
        /// Add a mapping entry that maps the given CheckBox to a data entry control, and any other controls
        /// that should be enabled\disabled based on the checkbox.  The value from the data entry control is not read or set - only
        /// its enabled state is updated.
        /// </summary>
        protected void AddEnabledStateMapping(EntityBase2 entity, EntityField2 field, CheckBox checkBox, Control dataControl, params Control[] otherControls)
        {
            AddMapping(entity, field, checkBox, dataControl, otherControls);
        }

        /// <summary>
        /// Adds a new mapping
        /// </summary>
        private CheckStateMapping AddMapping(EntityBase2 entity, EntityField2 field, CheckBox checkBox, Control dataControl, Control[] otherControls)
        {
            CheckStateMapping mapping = new CheckStateMapping
            {
                Entity = entity,
                Field = field,
                CheckBox = checkBox,
                DataControl = dataControl,
                OtherControls = otherControls
            };

            // Add the mapping entry
            checkStateMap.Add(mapping);

            // Update the check state
            checkBox.Checked = entity.GetCurrentFieldValue(field.FieldIndex) != null;

            // Update the UI and start listening for changes
            checkBox.CheckedChanged += new EventHandler(OnStateCheckChanged);
            OnStateCheckChanged(checkBox, EventArgs.Empty);

            checkBox.Enabled = allowChangeCheckState;

            return mapping;
        }

        /// <summary>
        /// Save all the loaded check state mappings to their entities
        /// </summary>
        protected void SaveMappingsToEntities()
        {
            foreach (CheckStateMapping mapping in checkStateMap)
            {
                if (mapping.IsValueMapping)
                {
                    if (mapping.CheckBox.Checked)
                    {
                        SetFieldValue(mapping.Entity, mapping.Field, mapping.DataControl);
                    }
                    else
                    {
                        mapping.Entity.SetNewFieldValue(mapping.Field.FieldIndex, null);
                    }
                }
            }
        }

        /// <summary>
        /// Read the value of the specified field of the specified entity to the given control
        /// </summary>
        private void ReadFieldValue(EntityBase2 entity, EntityField2 field, Control control)
        {
            MoneyTextBox moneyBox = control as MoneyTextBox;
            if (moneyBox != null)
            {
                moneyBox.Amount = (decimal) entity.GetCurrentFieldValue(field.FieldIndex);
                return;
            }

            // likely linked to an integer field
            NumericTextBox numericTextBox = control as NumericTextBox;
            if (numericTextBox != null)
            {
                numericTextBox.Text = ((int) entity.GetCurrentFieldValue(field.FieldIndex)).ToString();
                return;
            }

            TextBox textBox = control as TextBox;
            if (textBox != null)
            {
                textBox.Text = (string) entity.GetCurrentFieldValue(field.FieldIndex);
                return;
            }

            ComboBox comboBox = control as ComboBox;
            if (comboBox != null)
            {
                object value = entity.GetCurrentFieldValue(field.FieldIndex);

                // We have to figure out of the value type of the combobox is actually an enum
                if (comboBox.Items.Count > 0)
                {
                    comboBox.SelectedIndex = 0;

                    Type valueType = comboBox.SelectedValue.GetType();
                    if (valueType.IsEnum)
                    {
                        value = Enum.ToObject(valueType, value);
                    }
                }

                comboBox.SelectedValue = value;
                return;
            }

            CheckBox checkBox = control as CheckBox;
            if (checkBox != null)
            {
                checkBox.Checked = (bool) entity.GetCurrentFieldValue(field.FieldIndex);
                return;
            }

            WeightControl weightControl = control as WeightControl;
            if (weightControl != null)
            {
                weightControl.Weight = Convert.ToDouble(entity.GetCurrentFieldValue(field.FieldIndex));
                return;
            }

            DimensionsControl dimensions = control as DimensionsControl;
            if (dimensions != null)
            {
                DimensionsAdapter adapter = new DimensionsAdapter(entity);
                dimensions.LoadDimensions(new DimensionsAdapter[] { adapter });
                return;
            }

            InsuranceProfileControl insurance = control as InsuranceProfileControl;
            if (insurance != null)
            {
                ShippingProfileEntity profile = (ShippingProfileEntity) entity;

                insurance.UseInsurance = profile.Insurance.Value;
                insurance.Source = (InsuranceInitialValueSource) profile.InsuranceInitialValueSource.Value;
                insurance.OtherAmount = profile.InsuranceInitialValueAmount.Value;

                return;
            }

            TemplateTokenTextBox tokenBox = control as TemplateTokenTextBox;
            if (tokenBox != null)
            {
                tokenBox.Text = (string) entity.GetCurrentFieldValue(field.FieldIndex);
                return;
            }

            IShippingProfileControl profileControlState = control as IShippingProfileControl;
            if (profileControlState != null)
            {
                profileControlState.LoadFromEntity(entity);
                return;
            }

            throw new NotSupportedException("Unsupported control type: " + control.GetType().Name);
        }

        /// <summary>
        /// Clear the value from the given control to appear disabled
        /// </summary>
        private void UpdateState(Control control, bool active)
        {
            control.Enabled = active;

            TextBox textBox = control as TextBox;
            if (textBox != null)
            {
                if (!active)
                {
                    textBox.Text = "";
                }

                return;
            }

            ComboBox comboBox = control as ComboBox;
            if (comboBox != null)
            {
                if (active)
                {
                    if (comboBox.SelectedIndex < 0)
                    {
                        comboBox.SelectedIndex = 0;
                    }
                }
                else
                {
                    comboBox.SelectedIndex = -1;
                }

                return;
            }

            CheckBox checkBox = control as CheckBox;
            if (checkBox != null)
            {
                if (!active)
                {
                    checkBox.Checked = false;
                }

                return;
            }

            WeightControl weightControl = control as WeightControl;
            if (weightControl != null)
            {
                weightControl.Cleared = !active;

                return;
            }

            DimensionsControl dimensions = control as DimensionsControl;
            if (dimensions != null)
            {
                dimensions.Cleared = !active;

                return;
            }

            TemplateTokenTextBox tokenBox = control as TemplateTokenTextBox;
            if (tokenBox != null)
            {
                if (!active)
                {
                    tokenBox.Text = "";
                }

                return;
            }

            InsuranceProfileControl insuranceControl = control as InsuranceProfileControl;
            if (insuranceControl != null)
            {
                insuranceControl.Cleared = !active;

                return;
            }

            IShippingProfileControl profileControlState = control as IShippingProfileControl;
            if (profileControlState != null)
            {
                profileControlState.State = active;

                return;
            }

            throw new NotSupportedException("Unsupported control type: " + control.GetType().Name);
        }

        /// <summary>
        /// Set the field value to the value which is in the given control
        /// </summary>
        private void SetFieldValue(EntityBase2 entity, EntityField2 field, Control control)
        {
            object value = null;

            TextBox textBox = control as TextBox;
            if (textBox != null)
            {
                value = textBox.Text;
            }

            ComboBox comboBox = control as ComboBox;
            if (comboBox != null)
            {
                value = comboBox.SelectedValue;

                if (value is Enum)
                {
                    value = (int) value;
                }
            }

            CheckBox checkBox = control as CheckBox;
            if (checkBox != null)
            {
                value = checkBox.Checked;
            }

            WeightControl weightControl = control as WeightControl;
            if (weightControl != null)
            {
                value = weightControl.Weight;
            }

            MoneyTextBox moneyBox = control as MoneyTextBox;
            if (moneyBox != null)
            {
                value = moneyBox.Amount;
            }

            DimensionsControl dimensions = control as DimensionsControl;
            if (dimensions != null)
            {
                DimensionsAdapter adapter = new DimensionsAdapter(entity);

                // Default values are needed, b\c it uses them in case the user inputs are invalid
                adapter.ProfileID = 0;
                adapter.Length = 0;
                adapter.Height = 0;
                adapter.Width = 0;
                adapter.Weight = 0;
                adapter.AddWeight = true;

                dimensions.SaveToEntity(adapter);

                // Special case for dimensions where we dont drop through
                return;
            }

            InsuranceProfileControl insuranceControl = control as InsuranceProfileControl;
            if (insuranceControl != null)
            {
                ShippingProfileEntity profile = (ShippingProfileEntity) entity;

                profile.Insurance = insuranceControl.UseInsurance;
                profile.InsuranceInitialValueSource = (int) insuranceControl.Source;
                profile.InsuranceInitialValueAmount = insuranceControl.OtherAmount;

                return;
            }

            TemplateTokenTextBox tokenBox = control as TemplateTokenTextBox;
            if (tokenBox != null)
            {
                value = tokenBox.Text;
            }

            IShippingProfileControl shippingProfileControlState = control as IShippingProfileControl;
            if (shippingProfileControlState != null)
            {
                shippingProfileControlState.SaveToEntity(entity);
                return;
            }

            if (value != null)
            {
                // if datatype is nullable, llblgen gets confused. This settles the confusion by defining the datatype.
                Type dataType = entity.Fields[field.FieldIndex].DataType;

                if (dataType.Name.Contains("Nullable"))
                {
                    string dataTypeName = dataType.FullName.Substring(dataType.FullName.IndexOf("[[", StringComparison.OrdinalIgnoreCase) + "[[".Length, dataType.FullName.IndexOf(",", StringComparison.OrdinalIgnoreCase) - dataType.FullName.IndexOf("[[", StringComparison.OrdinalIgnoreCase) - "[[".Length);
                    dataType = Type.GetType(dataTypeName);
                }

                entity.SetNewFieldValue(field.FieldIndex, Convert.ChangeType(value, dataType));
            }
            else
            {
                throw new NotSupportedException("Unsupported control type: " + control.GetType().Name);
            }
        }

        /// <summary>
        /// One of the checkboxes that controls state has changed
        /// </summary>
        void OnStateCheckChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox) sender;

            foreach (CheckStateMapping mapping in checkStateMap.Where(m => m.CheckBox == checkBox))
            {
                // Update the primary control state
                UpdateState(mapping.DataControl, checkBox.Checked);

                // Update the other controls
                foreach (Control control in mapping.OtherControls)
                {
                    control.Enabled = checkBox.Checked;
                }
            }
        }
    }
}
