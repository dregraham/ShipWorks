using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Utility;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Filters;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.FactoryClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using System.Diagnostics;
using ShipWorks.Data.Grid;
using ShipWorks.UI;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.Data.Connection;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Content.Panels
{
    /// <summary>
    /// Control for displaying the charges of an order
    /// </summary>
    public partial class OrderChargesPanel : SingleSelectPanelBase
    {
        PanelDataMode dataMode;

        // Only valid if the data mode is local
        List<OrderChargeEntity> localCharges;

        /// <summary>
        /// Raised when a charge is edited, added, or removed
        /// </summary>
        public event EventHandler ChargesChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderChargesPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public override void Initialize(Guid settingsKey, GridColumnDefinitionSet definitionSet, Action<GridColumnLayout> layoutInitializer)
        {
            Initialize(settingsKey, definitionSet, PanelDataMode.LiveDatabase, layoutInitializer);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public void Initialize(Guid settingsKey, GridColumnDefinitionSet definitionSet, PanelDataMode dataMode, Action<GridColumnLayout> layoutInitializer)
        {
            this.dataMode = dataMode;

            base.Initialize(settingsKey, definitionSet, layoutInitializer);

            // Load the copy menu
            menuCopy.DropDownItems.AddRange(entityGrid.CreateCopyMenuItems(false));

            if (dataMode == PanelDataMode.LocalPending)
            {
                localCharges = new List<OrderChargeEntity>();
            }
        }

        /// <summary>
        /// The PaleDataMode the panel is in
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PanelDataMode DataMode
        {
            get { return dataMode; }
        }

        /// <summary>
        /// The list of local charges that have been added.  Only valid if data mode is local.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<OrderChargeEntity> LocalCharges
        {
            get { return localCharges; }
        }

        /// <summary>
        /// EntityType displayed by this panel
        /// </summary>
        public override EntityType EntityType
        {
            get { return EntityType.OrderChargeEntity; }
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
            if (dataMode == PanelDataMode.LiveDatabase)
            {
                RelatedKeysEntityGateway gateway = new RelatedKeysEntityGateway(entityID, EntityType.OrderChargeEntity);

                // Update our ability to add based on what entity we are now displaying
                addLink.Visible = UserSession.Security.HasPermission(PermissionType.OrdersModify, entityID);

                return gateway;
            }
            else
            {
                return new LocalCollectionEntityGateway<OrderChargeEntity>(localCharges);
            }
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
                DeleteCharge(entityID);
            }

            if (action == GridLinkAction.Edit)
            {
                EditCharge(entityID);
            }
        }

        /// <summary>
        /// Edit the selected charge
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            if (entityGrid.Selection.Count == 1)
            {
                EditCharge(entityGrid.Selection.Keys.First());
            }
        }

        /// <summary>
        /// Delete the selected charge
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            if (entityGrid.Selection.Count == 1)
            {
                DeleteCharge(entityGrid.Selection.Keys.First());
            }
        }

        /// <summary>
        /// Add a new charge
        /// </summary>
        private void OnAddCharge(object sender, EventArgs e)
        {
            Debug.Assert(EntityID != null);
            if (EntityID == null)
            {
                return;
            }

            OrderChargeEntity charge = new OrderChargeEntity();
            charge.OrderID = EntityID.Value;
            charge.Type = "";
            charge.Description = "";
            charge.Amount = 0;

            // For local ones, they have to have a fakeID until they are ready to save
            if (dataMode == PanelDataMode.LocalPending)
            {
                charge.OrderChargeID = localCharges.Count == 0 ? 
                    -EntityUtility.GetEntitySeed(EntityType.OrderChargeEntity) :
                    localCharges.Min(c => c.OrderChargeID) - 1000;
            }

            using (EditChargeDlg dlg = new EditChargeDlg(charge, dataMode))
            {
                DialogResult result = dlg.ShowDialog(this);

                if (result == DialogResult.OK && dataMode == PanelDataMode.LocalPending)
                {
                    localCharges.Add(charge);
                }

                // OK and Abort both could mean data has changed - reload as long as not cancled
                if (result != DialogResult.Cancel)
                {
                    ReloadContent();

                    RaiseChargesChanged();
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
                EditCharge(entityID);
            }
        }

        /// <summary>
        /// Edit the charge with the given ID
        /// </summary>
        private void EditCharge(long chargeID)
        {
            OrderChargeEntity charge;

            if (dataMode == PanelDataMode.LiveDatabase)
            {
                charge = (OrderChargeEntity) DataProvider.GetEntity(chargeID);

                if (charge == null)
                {
                    MessageHelper.ShowMessage(this, "The charge has been deleted.");
                }
            }

            else
            {
                charge = localCharges.Single(c => c.OrderChargeID == chargeID);
            }

            if (charge != null)
            {
                using (EditChargeDlg dlg = new EditChargeDlg(charge, dataMode))
                {
                    var result = dlg.ShowDialog(this);

                    // OK and Abort both could mean data has changed - reload as long as not cancled
                    if (result != DialogResult.Cancel)
                    {
                        ReloadContent();

                        RaiseChargesChanged();
                    }

                }
            }
        }

        /// <summary>
        /// Delete the given charge
        /// </summary>
        private void DeleteCharge(long chargeID)
        {
            DialogResult result = MessageHelper.ShowQuestion(this, "Delete the selected charge?");

            if (result == DialogResult.OK)
            {
                if (dataMode == PanelDataMode.LiveDatabase)
                {
                    OrderUtility.DeleteCharge(chargeID);
                }
                else
                {
                    localCharges.Remove(localCharges.Single(c => c.OrderChargeID == chargeID));
                }

                ReloadContent();

                RaiseChargesChanged();
            }
        }

        /// <summary>
        /// Raise the charges changed event to notify that a charge has been added edited or deleted
        /// </summary>
        private void RaiseChargesChanged()
        {
            if (ChargesChanged != null)
            {
                ChargesChanged(this, EventArgs.Empty);
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

            menuEdit.Available = EntityID == null || dataMode == PanelDataMode.LocalPending || UserSession.Security.HasPermission(PermissionType.OrdersModify, (long) EntityID);
            menuDelete.Available = menuEdit.Available;
            menuSep.Available = menuEdit.Available;
        }
    }
}
