using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates;

namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    /// <summary>
    /// Editor for choosing templates that something was emailed with
    /// </summary>
    public partial class PrintedValueEditor : TemplateValueEditor
    {
        PrintedCondition condition;

        /// <summary>
        /// Constructor
        /// </summary>
        public PrintedValueEditor(PrintedCondition condition) :
            base(condition)
        {
            InitializeComponent();

            this.condition = condition;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            labelQueryType.InitializeFromEnumType(typeof(PrintedQueryType));
            labelQueryType.SelectedValue = condition.QueryType;
            labelQueryType.SelectedValueChanged += new EventHandler(OnValueChanged);

            UpdateValueVisibility();
        }

        /// <summary>
        /// The value of the operator has changed
        /// </summary>
        void OnValueChanged(object sender, EventArgs e)
        {
            PrintedQueryType op = (PrintedQueryType) labelQueryType.SelectedValue;
            condition.QueryType = op;

            UpdateValueVisibility();

            RaiseContentChanged();
        }

        /// <summary>
        /// Update the position and visibility of the controls
        /// </summary>
        protected override void UpdateValueVisibility()
        {
            panelTemplateSelection.Left = labelQueryType.Right + 3;
            Width = panelTemplateSelection.Right + 5;
        }
    }
}
