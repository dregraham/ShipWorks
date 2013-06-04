using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Filters;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using ShipWorks.Data.Model.FactoryClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using System.Diagnostics;
using ShipWorks.Data.Grid;
using ShipWorks.UI;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using Interapptive.Shared.UI;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators;

namespace ShipWorks.Stores.Content.Panels
{
    /// <summary>
    /// User control for displaying the payment details of an order
    /// </summary>
    public partial class PaymentDetailsPanel : SingleSelectPanelBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PaymentDetailsPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public override void Initialize(Guid settingsKey, GridColumnDefinitionSet definitionSet, Action<GridColumnLayout> layoutInitializer)
        {
            base.Initialize(settingsKey, definitionSet, layoutInitializer);

            // Load the copy menu
            menuCopy.DropDownItems.AddRange(entityGrid.CreateCopyMenuItems(false));
        }

        /// <summary>
        /// EntityType displayed by this panel
        /// </summary>
        public override EntityType EntityType
        {
            get { return EntityType.OrderPaymentDetailEntity; }
        }

        /// <summary>
        /// The targets this supports
        /// </summary>
        public override FilterTarget[] SupportedTargets
        {
            get { return new FilterTarget[] { FilterTarget.Orders }; }
        }

        /// <summary>
        /// Update the gateway query filter and reload the grid
        /// </summary>
        protected override IEntityGateway CreateGateway(long entityID)
        {
            RelatedKeysEntityGateway gateway = new RelatedKeysEntityGateway(entityID, EntityType.OrderPaymentDetailEntity);

            // Update our ability to add based on what entity we are now displaying
            addLink.Visible = UserSession.Security.HasPermission(PermissionType.OrdersModify, entityID);

            return gateway;
        }

        /// <summary>
        /// An action has occurred in one of the cells, like a hyperlink click.
        /// </summary>
        private void OnGridCellLinkClicked(object sender, GridHyperlinkClickEventArgs e)
        {
            EntityGridRow row = (EntityGridRow) e.Row;

            if (row.EntityID == null)
            {
                MessageHelper.ShowMessage(this, "The charge data is not yet loaded.");
                return;
            }

            long entityID = row.EntityID.Value;

            GridLinkAction action = (GridLinkAction) ((GridActionDisplayType) e.Column.DisplayType).ActionData;

            if (action == GridLinkAction.Delete)
            {
                DeletePaymentDetail(entityID);
            }

            if (action == GridLinkAction.Edit)
            {
                EditPaymentDetail(entityID);
            }
        }

        /// <summary>
        /// Edit the selected detail
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            if (entityGrid.Selection.Count == 1)
            {
                EditPaymentDetail(entityGrid.Selection.Keys.First());
            }
        }

        /// <summary>
        /// Delete the selected detail
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            if (entityGrid.Selection.Count == 1)
            {
                DeletePaymentDetail(entityGrid.Selection.Keys.First());
            }
        }

        /// <summary>
        /// Add a new detail
        /// </summary>
        private void OnAddPaymentDetail(object sender, EventArgs e)
        {
            Debug.Assert(EntityID != null);
            if (EntityID == null)
            {
                return;
            }

            OrderPaymentDetailEntity detail = new OrderPaymentDetailEntity();
            detail.OrderID = EntityID.Value;
            detail.Label = "";
            detail.Value = "";

            using (EditPaymentDetailDlg dlg = new EditPaymentDetailDlg(detail))
            {
                var result = dlg.ShowDialog(this);

                if (result != DialogResult.Cancel)
                {
                    ReloadContent();
                }
            }
        }

        /// <summary>
        /// An entity row has been double clicked
        /// </summary>
        protected override void OnEntityDoubleClicked(long entityID)
        {
            if (UserSession.Security.HasPermission(PermissionType.OrdersModify, entityID))
            {
                EditPaymentDetail(entityID);
            }
        }

        /// <summary>
        /// Edit the detail with the given ID
        /// </summary>
        private void EditPaymentDetail(long detailID)
        {
            OrderPaymentDetailEntity detail = (OrderPaymentDetailEntity) DataProvider.GetEntity(detailID);

            if (detail == null)
            {
                MessageHelper.ShowMessage(this, "The payment detail has been deleted.");
            }
            else
            {
                using (EditPaymentDetailDlg dlg = new EditPaymentDetailDlg(detail))
                {
                    var result = dlg.ShowDialog(this);

                    if (result != DialogResult.Cancel)
                    {
                        ReloadContent();
                    }
                }
            }
        }

        /// <summary>
        /// Delete the given detail
        /// </summary>
        private void DeletePaymentDetail(long detailID)
        {
            DialogResult result = MessageHelper.ShowQuestion(this, "Delete the selected payment detail?");

            if (result == DialogResult.OK)
            {
                OrderUtility.DeletePaymentDetail(detailID);

                ReloadContent();
            }
        }

        /// <summary>
        /// The context menu is opening
        /// </summary>
        private void OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            menuEdit.Enabled = entityGrid.Selection.Count == 1;
            menuDelete.Enabled = entityGrid.Selection.Count == 1;
            menuCopy.Enabled = entityGrid.Selection.Count >= 1;

            menuEdit.Available = EntityID == null || UserSession.Security.HasPermission(PermissionType.OrdersModify, (long) EntityID);
            menuDelete.Available = menuEdit.Available;
            menuSep.Available = menuEdit.Available;
        }
    }
}
