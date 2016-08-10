﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Divelements.SandGrid;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Shows provider and allows user to choose a different provider.
    /// </summary>
    internal class GridProviderDisplayType : GridEnumDisplayType<ShipmentTypeCode>
    {
        private readonly IMessenger messenger;

        /// <summary>
        /// Default constructor
        /// </summary>
        public GridProviderDisplayType(EnumSortMethod sortMethod)
            : this(sortMethod, Messenger.Current)
        { }

        /// <summary>
        /// Default constructor
        /// </summary>
        public GridProviderDisplayType(EnumSortMethod sortMethod, IMessenger messenger)
            : base(sortMethod)
        {
            this.messenger = messenger;
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
                ShipmentEntity shipment = (ShipmentEntity) gridHyperlinkClickEventArgs.Row.Entity;
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
        public void ShowProviderOptionMenu(GridRow row, ShipmentEntity shipment, Point displayPosition)
        {
            if (shipment.Processed)
            {
                MessageHelper.ShowInformation(Program.MainForm, "Cannot change provider after shipment has been processed.");
            }

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            ContextMenuStrip menu = new ContextMenuStrip();

            List<ShipmentType> enabledShipmentTypes = ShipmentTypeManager.EnabledShipmentTypes;

            if (UpsAccountManager.Accounts.Count == 0 || !settings.ConfiguredTypes.Contains(ShipmentTypeCode.UpsWorldShip))
            {
                enabledShipmentTypes.RemoveAll(s => s.ShipmentTypeCode == ShipmentTypeCode.UpsWorldShip);
            }

            // Initially this was to remove Amazon Shipment Type from non applicable shipments
            enabledShipmentTypes.RemoveAll(s => !s.IsAllowedFor(shipment));

            bool postalNotSetup = !PostalUtility.IsPostalSetup();

            if (postalNotSetup)
            {
                // Exclude all other USPS-based providers except the USPS shipment type code
                // when no postal accounts exist in the system (i.e. prefer USPS shipment type
                // over the others in this case)
                enabledShipmentTypes.RemoveAll(s =>
                    s.ShipmentTypeCode == ShipmentTypeCode.Express1Usps ||
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
        private void SelectProvider(ShipmentEntity shipment, ShipmentType type)
        {
            shipment.ShipmentType = (int) type.ShipmentTypeCode;
            shipment.Order = (OrderEntity) DataProvider.GetEntity(shipment.OrderID);

            using (SqlAdapter sqlAdapter = new SqlAdapter())
            {
                sqlAdapter.SaveAndRefetch(shipment);

                // Perform this after the save otherwise customs items will be duplicated on
                // international shipments
                ShippingManager.EnsureShipmentLoaded(shipment);
                CustomsManager.LoadCustomsItems(shipment, false, sqlAdapter);
            }

            Program.MainForm.ForceHeartbeat();

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                messenger.Send(new ShipmentChangedMessage(this, lifetimeScope.Resolve<ICarrierShipmentAdapterFactory>().Get(shipment)));
            }
        }
    }
}
