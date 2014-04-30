using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Web.Services3.Addressing;
using ShipWorks.AddressValidation;
using ShipWorks.Filters.Content.Conditions;

namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    public partial class AddressValidationStatusValueEditor : ValueEditor
    {
        private readonly AddressValidationStatusCondition condition;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressValidationStatusValueEditor"/> class.
        /// </summary>
        public AddressValidationStatusValueEditor(AddressValidationStatusCondition condition)
        {
            InitializeComponent();

            this.condition = condition;

            // Load the equality operator
            equalityOperator.InitializeFromEnumType(typeof(EqualityOperator));
            equalityOperator.SelectedValue = condition.Operator;

            addressValidationStatus.SelectStatuses(condition.StatusTypes.ToArray());
            addressValidationStatus.StatusChanged = () =>
            {
                condition.StatusTypes = addressValidationStatus.GetSelectedStatuses();
                RaiseContentChanged();
            };

            UpdateValueVisibility();
        }

        /// <summary>
        /// Called when [equality operator changed].
        /// </summary>
        private void OnEqualityOperatorChanged(object sender, EventArgs e)
        {
            condition.Operator = (EqualityOperator)equalityOperator.SelectedValue;

            UpdateValueVisibility();

            RaiseContentChanged();
        }

        /// <summary>
        /// Updates the value visibility.
        /// </summary>
        private void UpdateValueVisibility()
        {
            addressValidationStatus.Left = equalityOperator.Right + 3;
            Width = addressValidationStatus.Right;
        }
    }
}
