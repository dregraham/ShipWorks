using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Templates.Tokens;
using ShipWorks.UI.Controls;

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
            public CheckBox Parent { get; set; } = null;
        }

        /// <summary>
        /// A map setting the relationship between a parent / parent state and its child / child state
        /// </summary>
        class ParentChildMapping
        {
            public CheckBox ParentState { get; set; }
            public CheckBox Parent { get; set; }
            public CheckBox ChildState { get; set; }
            public Control Child { get; set; }
        }

        List<CheckStateMapping> checkStateMap = new List<CheckStateMapping>();
        List<ParentChildMapping> parentChildMap = new List<ParentChildMapping>();

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

            // If its checked, and has a value apply the current value
            if (checkBox.Checked && entity.GetCurrentFieldValue(field.FieldIndex) != null)
            {
                ReadFieldValue(entity, field, dataControl);
                OnStateCheckChanged(checkBox, EventArgs.Empty);
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
            checkBox.Checked = (entity.GetCurrentFieldValue(field.FieldIndex) != null) || !allowChangeCheckState;

            // Remove all potential event handlers to
            // make sure we only return a single event handler.
            checkBox.CheckedChanged -= OnStateCheckChanged;

            // Update the UI and start listening for changes
            checkBox.CheckedChanged += OnStateCheckChanged;
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
                    // Only set the field value if the state is checked and no parent is set
                    // or if both the state and the parent are checked
                    if (mapping.CheckBox.Checked && (mapping.Parent == null || mapping.Parent.Checked))
                    {
                        SetFieldValue(mapping.Entity, mapping.Field, mapping.DataControl);
                    }
                    // Set reasonable defaults for disabled child controls
                    else if (mapping.CheckBox.Checked && mapping.Parent != null)
                    {
                        if (mapping.DataControl is ComboBox)
                        {
                            mapping.Entity.SetNewFieldValue(mapping.Field.FieldIndex, 0);
                        }
                        else if (mapping.DataControl is TextBox || mapping.DataControl is TemplateTokenTextBox)
                        {
                            mapping.Entity.SetNewFieldValue(mapping.Field.FieldIndex, String.Empty);
                        }
                        else if (mapping.DataControl is CheckBox)
                        {
                            mapping.Entity.SetNewFieldValue(mapping.Field.FieldIndex, false);
                        }
                        else
                        {
                            mapping.Entity.SetNewFieldValue(mapping.Field.FieldIndex, null);
                        }
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
        [NDependIgnoreLongMethod]
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
        [NDependIgnoreLongMethod]
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
                    if (comboBox.SelectedIndex < 0 && comboBox.Items.Count > 0)
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
        [NDependIgnoreLongMethod]
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
                dataType = Nullable.GetUnderlyingType(dataType) ?? dataType;

                var convertedValue = dataType.IsEnum ?
                    Enum.ToObject(dataType, value) :
                    Convert.ChangeType(value, dataType);

                entity.SetNewFieldValue(field.FieldIndex, convertedValue);
            }
            else
            {
                throw new NotSupportedException("Unsupported control type: " + control.GetType().Name);
            }
        }

        /// <summary>
        /// One of the checkboxes that controls state has changed
        /// </summary>
        protected void OnStateCheckChanged(object sender, EventArgs e)
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

        /// <summary>
        /// Set a parent checkbox for a control to be automatically enabled/disabled based on its parent state
        /// </summary>
        protected void SetParentCheckBox(CheckBox parentState, CheckBox parent, CheckBox childState, Control child)
        {
            bool active = false;

            ParentChildMapping parentChild = new ParentChildMapping
            {
                ParentState = parentState,
                Parent = parent,
                ChildState = childState,
                Child = child
            };

            parentChildMap.Add(parentChild);

            foreach (CheckStateMapping mapping in checkStateMap.Where(x => x.DataControl == child))
            {
                mapping.Parent = parent;
            }

            // Remove original event handler
            childState.CheckedChanged -= OnStateCheckChanged;

            // Add new event handlers, but make sure we only add them once
            parentState.CheckedChanged -= OnParentStateChanged;
            parent.CheckedChanged -= OnParentCheckChanged;
            childState.CheckedChanged -= OnChildStateChanged;
            parentState.CheckedChanged += OnParentStateChanged;
            parent.CheckedChanged += OnParentCheckChanged;
            childState.CheckedChanged += OnChildStateChanged;

            // Set initial child state
            active = childState.Checked && parent.Checked;
            SetChildState(child, active);
        }

        /// <summary>
        /// Click of the parent state checkbox
        /// </summary>
        protected void OnParentStateChanged(object sender, EventArgs e)
        {
            CheckBox parentState = (CheckBox) sender;

            if (!parentState.Checked)
            {
                foreach (ParentChildMapping mapping in parentChildMap.Where(x => x.ParentState == parentState))
                {
                    SetChildState(mapping.Child, false);
                }
            }
        }

        /// <summary>
        /// Click of the parent checkbox
        /// </summary>
        protected void OnParentCheckChanged(object sender, EventArgs e)
        {
            CheckBox parent = (CheckBox) sender;
            bool active;

            foreach (ParentChildMapping mapping in parentChildMap.Where(x => x.Parent == parent))
            {
                active = parent.Checked ? mapping.ChildState.Checked : false;

                SetChildState(mapping.Child, active);
            }
        }

        /// <summary>
        /// Click of the child state checkbox
        /// </summary>
        protected void OnChildStateChanged(object sender, EventArgs e)
        {
            CheckBox childState = (CheckBox) sender;
            bool active;

            foreach (ParentChildMapping mapping in parentChildMap.Where(x => x.ChildState == childState))
            {
                active = childState.Checked ? mapping.Parent.Checked : false;

                SetChildState(mapping.Child, active);
            }
        }

        /// <summary>
        /// Set child control's state and, if possible, a default value
        /// </summary>
        private void SetChildState(Control child, bool active)
        {
            try
            {
                UpdateState(child, active);
            }
            catch (NotSupportedException)
            {
                child.Enabled = active;
            }
        }
    }
}
