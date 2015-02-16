using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Divelements.SandGrid;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Shows provider and allows user to choose a different provider.
    /// </summary>
    internal class GridProviderDisplayType : GridEnumDisplayType<ShipmentTypeCode>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public GridProviderDisplayType(EnumSortMethod sortMethod) 
            : base(sortMethod)
        {
            GridHyperlinkDecorator gridHyperlinkDecorator = new GridHyperlinkDecorator();
            gridHyperlinkDecorator.QueryEnabled += (sender, args) => args.Enabled = LinkEnabled(args.Entity);
            gridHyperlinkDecorator.LinkClicked += OnHyperlinkDecoratorLinkClicked;
            Decorate(gridHyperlinkDecorator);
        }

        /// <summary>
        /// Called when [hyperlink decorator link clicked].
        /// </summary>
        private void OnHyperlinkDecoratorLinkClicked(object sender, GridHyperlinkClickEventArgs gridHyperlinkClickEventArgs)
        {
            GridProviderDisplayType gridProviderDisplayType = gridHyperlinkClickEventArgs.Column.DisplayType as GridProviderDisplayType;
            if (gridProviderDisplayType != null)
            {
                ShipmentEntity shipment = (ShipmentEntity)gridHyperlinkClickEventArgs.Row.Entity;
                if (shipment == null)
                {
                    return;
                }

                SandGrid grid = (SandGrid) gridHyperlinkClickEventArgs.Row.Grid.SandGrid;
                Debug.Assert(grid != null);

                ShowProviderOptionMenu(gridHyperlinkClickEventArgs.Row, shipment, new Point(gridHyperlinkClickEventArgs.MouseArgs.X - grid.HScrollOffset, gridHyperlinkClickEventArgs.MouseArgs.Y - grid.VScrollOffset));
            }
        }

        /// <summary>
        /// Only allowed to be a link if shipment isn't processed.
        /// </summary>
        private static bool LinkEnabled(EntityBase2 argShipment)
        {
            ShipmentEntity shipment = argShipment as ShipmentEntity;

            if (shipment == null)
            {
                return false;
            }

            return !shipment.Processed;
        }

        /// <summary>
        /// Shows the provider option menu.
        /// </summary>
        public static void ShowProviderOptionMenu(GridRow row, ShipmentEntity shipment, Point displayPosition)
        {
            if (shipment.Processed)
            {
                MessageHelper.ShowInformation(Program.MainForm, "Cannot change provider after shipment has been processed.");
            }

            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            
            ContextMenuStrip menu = new ContextMenuStrip();

            List<ShipmentType> enabledShipmentTypes = ShipmentTypeManager.EnabledShipmentTypes;

            if (UpsAccountManager.Accounts.Count == 0 || !settings.ConfiguredTypes.Contains((int)ShipmentTypeCode.UpsWorldShip))
            {
                enabledShipmentTypes.RemoveAll(s => s.ShipmentTypeCode == ShipmentTypeCode.UpsWorldShip);
            }

            bool postalNotSetup = !PostalUtility.IsPostalSetup();

            if (postalNotSetup)
            {
                enabledShipmentTypes.RemoveAll(s =>
                    s.ShipmentTypeCode == ShipmentTypeCode.Usps ||
                    s.ShipmentTypeCode == ShipmentTypeCode.Express1Stamps ||
                    s.ShipmentTypeCode == ShipmentTypeCode.PostalWebTools ||
                    s.ShipmentTypeCode == ShipmentTypeCode.Endicia ||
                    s.ShipmentTypeCode == ShipmentTypeCode.Express1Endicia);
            }

            enabledShipmentTypes.ForEach(shipmentType => menu.Items.Add(
                EnumHelper.GetDescription(shipmentType.ShipmentTypeCode),
                EnumHelper.GetImage(shipmentType.ShipmentTypeCode),
                (sender, args) => SelectProvider(shipment, shipmentType)));

            menu.Show(row.Grid.SandGrid, displayPosition);
        }

        /// <summary>
        /// Selects the provider.
        /// </summary>
        private static void SelectProvider(ShipmentEntity shipment, ShipmentType type)
        {
            shipment.ShipmentType = (int)type.ShipmentTypeCode;
            shipment.Order = (OrderEntity)DataProvider.GetEntity(shipment.OrderID);

            using (SqlAdapter sqlAdapter = new SqlAdapter())
            {
                sqlAdapter.SaveAndRefetch(shipment);
            }

            // Perform this after the save otherwise customs items will be duplicated on 
            // international shipments
            ShippingManager.EnsureShipmentLoaded(shipment);
            CustomsManager.LoadCustomsItems(shipment, false);

            Program.MainForm.ForceHeartbeat();
        }
    }
}
