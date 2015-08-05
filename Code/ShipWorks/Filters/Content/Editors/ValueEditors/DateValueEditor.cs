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
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    /// <summary>
    /// Base editor for dates
    /// </summary>
    public partial class DateValueEditor : ValueEditor
    {
        DateCondition condition;

        /// <summary>
        /// To use visual inheritance there must be a constructor with zero arguments
        /// </summary>
        private DateValueEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DateValueEditor(DateCondition condition) : this()
        {
            this.condition = condition;

            labelOperator.InitializeFromEnumType(typeof(DateOperator));
            labelOperator.SelectedValue = condition.Operator;

            EnumHelper.BindComboBox<DateWithinUnit>(withinUnits);
            EnumHelper.BindComboBox<DateRelativeUnit>(relativeUnits);

            value1.Value = condition.Value1.Date;
            value2.Value = condition.Value2.Date;

            withinAmount.Text = condition.WithinAmount.ToString();
            withinUnits.SelectedValue = condition.WithinUnit;

            relativeUnits.SelectedValue = condition.RelativeUnit;

            withinRangeType.InitializeFromEnumType(typeof(DateWithinRangeType));
            withinRangeType.SelectedValue = condition.WithinRangeType;

            withinUnits.SelectedValueChanged += new EventHandler(OnTargetValueChanged);
            relativeUnits.SelectedValueChanged += new EventHandler(OnTargetValueChanged);

            value1.ValueChanged += new EventHandler(OnTargetValueChanged);
            value2.ValueChanged += new EventHandler(OnTargetValueChanged);

            withinAmount.TextChanged += new EventHandler(OnTargetValueChanged);

            withinUnits.SelectedValueChanged += new EventHandler(OnWithinUnitsChanged);
            UpdateWithinRangeText();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            // Can't be called inthe constructor since its virtual
            if (!DesignModeDetector.IsDesignerHosted())
            {
                UpdateValueVisibility();
            }

            base.OnLoad(e);
        }

        /// <summary>
        /// Update the visibility of the value operators
        /// </summary>
        protected virtual void UpdateValueVisibility()
        {
            Panel visiblePanel = null;

            DateOperator op = condition.Operator;

            absolutePanel.Visible = false;
            relativePanel.Visible = false;
            withinPanel.Visible = false;

            if (!DateCondition.IsRelativeOperator(op))
            {
                visiblePanel = absolutePanel;

                bool isComparison =
                 (op == DateOperator.Between) ||
                 (op == DateOperator.NotBetween);

                labelAnd.Visible = isComparison;
                value2.Visible = isComparison;

                if (isComparison)
                {
                    absolutePanel.Width = value2.Right + errorSpace;
                }
                else
                {
                    absolutePanel.Width = value1.Right + errorSpace;
                }
            }
            else
            {
                if (op == DateOperator.WithinTheLast ||
                    op == DateOperator.Next)
                {
                    visiblePanel = withinPanel;
                    withinPanel.Width = withinRangeType.Right + errorSpace;
                }

                if (op == DateOperator.This ||
                    op == DateOperator.Last)
                {
                    visiblePanel = relativePanel;
                }
            }

            if (visiblePanel != null)
            {
                visiblePanel.Visible = true;

                visiblePanel.Left = labelOperator.Right + 3;
                panelDateControls.Width = visiblePanel.Right;
            }
            else
            {
                panelDateControls.Width = labelOperator.Right + 3;
            }

            Width = panelDateControls.Right + 2;
        }

        /// <summary>
        /// Operator type is changing
        /// </summary>
        private void OnChangeOperator(object sender, EventArgs e)
        {
            DateOperator op = (DateOperator) labelOperator.SelectedValue;

            condition.Operator = op;

            UpdateValueVisibility();

            RaiseContentChanged();
        }

        /// <summary>
        /// The Inclusive\Exclusive of Within is changing
        /// </summary>
        private void OnChangeWithinInclusive(object sender, EventArgs e)
        {
            DateWithinRangeType value = (DateWithinRangeType) withinRangeType.SelectedValue;

            condition.WithinRangeType = value;

            UpdateValueVisibility();

            RaiseContentChanged();
        }

        /// <summary>
        /// Any of the combo's or text boxes we are tracking have changed
        /// </summary>
        void OnTargetValueChanged(object sender, EventArgs e)
        {
            RaiseContentChanged();
        }

        /// <summary>
        /// Validate and save the within amount
        /// </summary>
        private void OnValidateWithinAmount(object sender, CancelEventArgs e)
        {
            int value;
            if (int.TryParse(withinAmount.Text, out value))
            {
                ClearError(withinRangeType);

                condition.WithinAmount = value;
            }
            else
            {
                SetError(withinRangeType, "The value entered is not valid.");
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Validating the dropdown\date controls
        /// </summary>
        private void OnValidateControls(object sender, CancelEventArgs e)
        {
            condition.Value1 = value1.Value.Date;
            condition.Value2 = value2.Value.Date;

            condition.WithinUnit = (DateWithinUnit) withinUnits.SelectedValue;
            condition.RelativeUnit = (DateRelativeUnit) relativeUnits.SelectedValue;
        }

        /// <summary>
        /// The WithinUnits is changing (Weeks, Months, etc)
        /// </summary>
        void OnWithinUnitsChanged(object sender, EventArgs e)
        {
            UpdateWithinRangeText();
        }

        /// <summary>
        /// Update the text for indicating what type or ranging to use for Within
        /// </summary>
        private void UpdateWithinRangeText()
        {
            withinRangeType.InitializeFromEnumType(
                typeof(DateWithinRangeType),
                v => string.Format(EnumHelper.GetDescription(v), EnumHelper.GetDescription((Enum) withinUnits.SelectedValue).ToLower().TrimEnd('s')));
        }
    }
}
