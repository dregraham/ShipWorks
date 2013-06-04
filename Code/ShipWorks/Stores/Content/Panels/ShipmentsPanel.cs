using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model;
using ShipWorks.Filters;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI;
using System.Diagnostics;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Shipping;
using ShipWorks.Data.Connection;
using ShipWorks.Properties;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using Interapptive.Shared.UI;
using System.Runtime.InteropServices;
using log4net;

namespace ShipWorks.Stores.Content.Panels
{
    /// <summary>
    /// Control for displaying shipments of an order
    /// </summary>
    public partial class ShipmentsPanel : SingleSelectPanelBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipmentsPanel));

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentsPanel()
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
            get { return EntityType.ShipmentEntity; }
        }

        /// <summary>
        /// The targets this supports
        /// </summary>
        public override FilterTarget[] SupportedTargets
        {
            get { return new FilterTarget[] { FilterTarget.Orders, FilterTarget.Customers }; }
        }

        /// <summary>
        /// Update the gateway query filter and reload the grid
        /// </summary>
        protected override IEntityGateway CreateGateway(long entityID)
        {
            EntityType type = EntityUtility.GetEntityType(entityID);

            RelatedKeysEntityGateway gateway = new RelatedKeysEntityGateway(entityID, EntityType.ShipmentEntity);

            if (type == EntityType.OrderEntity)
            {
                entityGrid.EmptyText = "The order has no shipments.";
            }
            else
            {
                entityGrid.EmptyText = "The customer has no shipments.";
            }

            // Can't add shipments directly to a customer
            addLink.Visible = 
                (type == EntityType.OrderEntity) &&
                UserSession.Security.HasPermission(PermissionType.ShipmentsCreateEditProcess, entityID);
            
            return gateway;
        }

        /// <summary>
        /// Add a new shipment
        /// </summary>
        private void OnAddShipment(object sender, EventArgs e)
        {
            Debug.Assert(EntityID != null);
            if (EntityID == null)
            {
                return;
            }

            try
            {
                ShipmentEntity shipment = ShippingManager.CreateShipment(EntityID.Value);

                using (ShippingDlg dlg = new ShippingDlg(new List<ShipmentEntity> { shipment }))
                {
                    dlg.ShowDialog(this);
                }
            }
            catch (SqlForeignKeyException)
            {
                MessageHelper.ShowMessage(this, "The order of the shipment has been deleted.");
            }

            ReloadContent();
        }

        /// <summary>
        /// An action has occurred in one of the cells, like a hyperlink click.
        /// </summary>
        private void OnGridCellLinkClicked(object sender, GridHyperlinkClickEventArgs e)
        {
            EntityGridRow row = (EntityGridRow) e.Row;

            if (row.EntityID == null)
            {
                MessageHelper.ShowMessage(this, "The shipment data is not yet loaded.");
                return;
            }

            long entityID = row.EntityID.Value;

            GridLinkAction action = (GridLinkAction) ((GridActionDisplayType) e.Column.DisplayType).ActionData;

            if (action == GridLinkAction.Delete)
            {
                DeleteShipment(entityID);
            }

            if (action == GridLinkAction.Edit)
            {
                EditShipments( new List<long> { entityID } );
            }
        }

        /// <summary>
        /// An entity row has been double clicked
        /// </summary>
        protected override void OnEntityDoubleClicked(long entityID)
        {
            EditShipments(new long[] { entityID });
        }

        /// <summary>
        /// Edit the selected shipment
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            if (entityGrid.Selection.Count > 0)
            {
                EditShipments(entityGrid.Selection.Keys);
            }
        }

        /// <summary>
        /// Delete the selected shipment
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            if (entityGrid.Selection.Count == 1)
            {
                DeleteShipment(entityGrid.Selection.Keys.First());
            }
        }

        /// <summary>
        /// Copy the tracking number of the selected shipment
        /// </summary>
        private void OnCopyTracking(object sender, EventArgs e)
        {
            if (entityGrid.Selection.Count == 1)
            {
                ShipmentEntity shipment = ShippingManager.GetShipment(entityGrid.Selection.Keys.First());
                if (shipment != null)
                {
                    try
                    {
                        if (shipment.TrackingNumber.Length > 0)
                        {
                            Clipboard.SetText(shipment.TrackingNumber);
                        }
                        else
                        {
                            Clipboard.Clear();
                        }
                    }
                    catch (ExternalException ex)
                    {
                        log.Error("Copy tracking", ex);

                        MessageHelper.ShowError(this, "ShipWorks could not copy to the clipboard because it is in use by another application.");
                    }
                }
            }
        }

        /// <summary>
        /// Track the selected shipment
        /// </summary>
        private void OnTrackShipment(object sender, EventArgs e)
        {
            if (entityGrid.Selection.Count == 1)
            {
                ShipmentEntity shipment = ShippingManager.GetShipment(entityGrid.Selection.Keys.First());
                if (shipment != null)
                {
                    // Show the shipping window
                    using (ShippingDlg dlg = new ShippingDlg(new List<ShipmentEntity> { shipment }, true))
                    {
                        dlg.ShowDialog(this);
                    }

                    ReloadContent();
                }
            }
        }

        /// <summary>
        /// Edit the shipment with the given ID
        /// </summary>
        private void EditShipments(IEnumerable<long> shipmentKeys)
        {
            if (shipmentKeys.Count() > ShipmentsLoader.MaxAllowedOrders)
            {
                MessageHelper.ShowInformation(this, string.Format("You can only ship up to {0} orders at a time.", ShipmentsLoader.MaxAllowedOrders));
                return;
            }

            ShipmentsLoader loader = new ShipmentsLoader(this);
            loader.LoadCompleted += OnShipOrdersLoadShipmentsCompleted;
            loader.LoadAsync(shipmentKeys);
        }

        /// <summary>
        /// The async loading of shipments for shipping has completed
        /// </summary>
        void OnShipOrdersLoadShipmentsCompleted(object sender, ShipmentsLoadedEventArgs e)
        {
            if (IsDisposed)
            {
                return;
            }

            if (e.Cancelled)
            {
                return;
            }

            // Show the shipping window
            using (ShippingDlg dlg = new ShippingDlg(e.Shipments))
            {
                dlg.ShowDialog(this);
            }

            ReloadContent();
        }

        /// <summary>
        /// Delete the given shipment
        /// </summary>
        private void DeleteShipment(long shipmentID)
        {
            DialogResult result = MessageHelper.ShowQuestion(this, "Delete the selected shipment?");

            if (result == DialogResult.OK)
            {
                ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);

                if (shipment == null)
                {
                    MessageHelper.ShowMessage(this, "The shipment has already been deleted.");
                }
                else
                {
                    ShippingManager.DeleteShipment(shipment);
                }

                ReloadContent();
            }
        }

        /// <summary>
        /// The context menu is opening
        /// </summary>
        private void OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            menuEdit.Enabled = entityGrid.Selection.Count >= 1;
            menuDelete.Enabled = entityGrid.Selection.Count == 1;
            menuTrack.Enabled = entityGrid.Selection.Count == 1;
            menuCopyTracking.Enabled = entityGrid.Selection.Count == 1;
            menuCopy.Enabled = entityGrid.Selection.Count >= 1;

            string editText = "Edit";
            Image editImage = Resources.edit16;

            if (EntityID != null && !UserSession.Security.HasPermission(PermissionType.ShipmentsCreateEditProcess, EntityID.Value))
            {
                editText = "View";
                editImage = Resources.view;
            }
            else
            {
                // If there's multiple, just show edit, even if there all processed.  Probably for no better
                // reason than I'm being lazy right now.
                if (entityGrid.Selection.Count == 1)
                {
                    ShipmentEntity shipment = (ShipmentEntity) DataProvider.GetEntity(entityGrid.Selection.Keys.First());
                    if (shipment != null)
                    {
                        if (shipment.Processed || shipment.Voided)
                        {
                            editText = "View";
                            editImage = Resources.view;
                        }
                    }
                }
            }

            menuEdit.Text = editText;
            menuEdit.Image = editImage;

            menuDelete.Visible = EntityID == null || UserSession.Security.HasPermission(PermissionType.ShipmentsVoidDelete, EntityID.Value);
        }
    }
}
