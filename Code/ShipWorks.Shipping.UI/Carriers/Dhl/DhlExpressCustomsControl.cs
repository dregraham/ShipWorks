using System;
using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Enums;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.UI.Controls;
using Interapptive.Shared.Business.Geography;

namespace ShipWorks.Shipping.UI.Carriers.Dhl
{
    /// <summary>
    /// DhlExpress specific customs stuff
    /// </summary>
    [KeyedComponent(typeof(CustomsControlBase), ShipmentTypeCode.DhlExpress)]
    public partial class DhlExpressCustomsControl : CustomsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressCustomsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            EnumHelper.BindComboBox<ShipEngineContentsType>(contentType);
            EnumHelper.BindComboBox<ShipEngineNonDeliveryType>(nonDeliveryType);
            EnumHelper.BindComboBox<TaxIdType>(taxIdType);
        }

        /// <summary>
        /// Load the shipments into the controls
        /// </summary>
        public override void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool enableEditing)
        {
            // A null reference error was being thrown.  Discoverred by Crash Reports.
            // Let's figure out what is null....
            if (shipments == null)
            {
                throw new ArgumentNullException("shipments");
            }

            base.LoadShipments(shipments, enableEditing);

            if (customsTinIssuingAuthority.DataSource == null)
            {
                customsTinIssuingAuthority.DataSource = Geography.Countries;
            }

            contentType.SelectedIndexChanged -= this.OnChangeOption;
            nonDeliveryType.SelectedIndexChanged -= this.OnChangeOption;

            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in shipments)
                {
                    if (shipment.DhlExpress == null)
                    {
                        ShippingManager.EnsureShipmentLoaded(shipment);
                    }

                    MethodConditions.EnsureArgumentIsNotNull(shipment.DhlExpress, "DhlExpress");

                    contentType.ApplyMultiValue((ShipEngineContentsType) shipment.DhlExpress.Contents);
                    nonDeliveryType.ApplyMultiValue((ShipEngineNonDeliveryType) shipment.DhlExpress.NonDelivery);

                    customsRecipientTin.ApplyMultiText(shipment.DhlExpress.CustomsRecipientTin);
                    taxIdType.ApplyMultiValue((TaxIdType) shipment.DhlExpress.CustomsTaxIdType);
                    customsTinIssuingAuthority.ApplyMultiText(Geography.GetCountryName(shipment.DhlExpress.CustomsTinIssuingAuthority));
                }
            }

            contentType.SelectedIndexChanged += new EventHandler(OnChangeOption);
            nonDeliveryType.SelectedIndexChanged += new EventHandler(OnChangeOption);
        }

        /// <summary>
        /// Content type has changed
        /// </summary>
        void OnChangeOption(object sender, EventArgs e)
        {
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                contentType.ReadMultiValue(v => shipment.DhlExpress.Contents = (int) (ShipEngineContentsType) v);
                contentType.ReadMultiValue(v => shipment.DhlExpress.NonDelivery = (int) (ShipEngineNonDeliveryType) v);
            }

        }

        /// <summary>
        /// Save the data in the control to the loaded shipments
        /// </summary>
        public override void SaveToShipments()
        {
            base.SaveToShipments();

            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                contentType.ReadMultiValue(v => shipment.DhlExpress.Contents = (int) (ShipEngineContentsType) v);
                nonDeliveryType.ReadMultiValue(v => shipment.DhlExpress.NonDelivery = (int) (ShipEngineNonDeliveryType) v);

                customsRecipientTin.ReadMultiText(s => shipment.DhlExpress.CustomsRecipientTin = s);
                taxIdType.ReadMultiValue(v => shipment.DhlExpress.CustomsTaxIdType = (int) (TaxIdType) v);
                customsTinIssuingAuthority.ReadMultiText(v => shipment.DhlExpress.CustomsTinIssuingAuthority = Geography.GetCountryCode(v));
            }
        }
    }
}