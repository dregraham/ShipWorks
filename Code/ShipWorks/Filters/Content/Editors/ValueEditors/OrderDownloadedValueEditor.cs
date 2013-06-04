using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Filters.Content.Conditions.Orders;
using Interapptive.Shared.Utility;

namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    /// <summary>
    /// ValueEditor for the condition for when an order was downloaded
    /// </summary>
    public partial class OrderDownloadedValueEditor : DateValueEditor
    {
        DownloadedCondition condition;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderDownloadedValueEditor(DownloadedCondition condition)
            : base(condition)
        {
            InitializeComponent();

            this.condition = condition;

            presenceOperator.InitializeFromEnumType(typeof(DownloadedPresenceType));
            presenceOperator.SelectedValue = condition.Presence;

            dateSpecifiedOperator.InitializeFromEnumType(typeof(DownloadedDateRangeType));
            dateSpecifiedOperator.SelectedValue = condition.DateRangeSpecified ? DownloadedDateRangeType.Specified : DownloadedDateRangeType.Anytime;

            presenceOperator.SelectedValueChanged += new EventHandler(OnChangePresence);
            dateSpecifiedOperator.SelectedValueChanged += new EventHandler(OnChangeDateSpecified);

            UpdateValueVisibility();
        }

        /// <summary>
        /// The selected presence value has changed
        /// </summary>
        void OnChangePresence(object sender, EventArgs e)
        {
            condition.Presence = (DownloadedPresenceType) presenceOperator.SelectedValue;

            UpdateValueVisibility();
            RaiseContentChanged();
        }

        /// <summary>
        /// The selected date range specification value has changed
        /// </summary>
        void OnChangeDateSpecified(object sender, EventArgs e)
        {
            condition.DateRangeSpecified = ((DownloadedDateRangeType) dateSpecifiedOperator.SelectedValue) == DownloadedDateRangeType.Specified;

            UpdateValueVisibility();
            RaiseContentChanged();
        }

        /// <summary>
        /// Update the visibility and positioning of the controls
        /// </summary>
        protected override void UpdateValueVisibility()
        {
            base.UpdateValueVisibility();

            dateSpecifiedOperator.Left = presenceOperator.Right;
            panelDateControls.Left = dateSpecifiedOperator.Right;

            if (condition.DateRangeSpecified)
            {
                panelDateControls.Visible = true;
                Width = panelDateControls.Right;
            }
            else
            {
                panelDateControls.Visible = false;
                Width = dateSpecifiedOperator.Right;
            }
        }
    }
}
