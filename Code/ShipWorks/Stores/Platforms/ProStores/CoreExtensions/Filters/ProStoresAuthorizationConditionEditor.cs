using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Filters.Content.Editors.ValueEditors;

namespace ShipWorks.Stores.Platforms.ProStores.CoreExtensions.Filters
{
    /// <summary>
    /// Editor for the ProStoresAuthorizationCondition
    /// </summary>
    public partial class ProStoresAuthorizationConditionEditor : DateValueEditor
    {
        ProStoresAuthorizationCondition condition;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProStoresAuthorizationConditionEditor(ProStoresAuthorizationCondition condition)
            : base(condition)
        {
            InitializeComponent();

            this.condition = condition;

            authorizedOperator.InitializeFromEnumType(typeof(ProStoresAuthorizationStatus));
            authorizedOperator.SelectedValue = condition.AuthorizationStatus;

            authorizedOperator.SelectedValueChanged += new EventHandler(OnChangePresence);

            UpdateValueVisibility();
        }

        /// <summary>
        /// The selected presence value has changed
        /// </summary>
        void OnChangePresence(object sender, EventArgs e)
        {
            condition.AuthorizationStatus = (ProStoresAuthorizationStatus) authorizedOperator.SelectedValue;

            UpdateValueVisibility();
            RaiseContentChanged();
        }

        /// <summary>
        /// Update the visibility and positioning of the controls
        /// </summary>
        protected override void UpdateValueVisibility()
        {
            base.UpdateValueVisibility();

            if (condition.AuthorizationStatus == ProStoresAuthorizationStatus.AuthorizedOn)
            {
                panelDateControls.Left = authorizedOperator.Right;
                Width = panelDateControls.Right;

                panelDateControls.Visible = true;
            }
            else
            {
                panelDateControls.Visible = false;
                Width = authorizedOperator.Right + 4;
            }
        }
    }
}
