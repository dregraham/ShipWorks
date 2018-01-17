using System;
using System.Collections.Generic;
using ShipWorks.Shipping.Editing;
using Interapptive.Shared.Utility;
using ShipWorks.UI.Controls;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipEngine;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping.UI.Carriers.Asendia
{
    /// <summary>
    /// Asendia specific customs stuff
    /// </summary>
    [KeyedComponent(typeof(CustomsControlBase), ShipmentTypeCode.Asendia)]
    public partial class AsendiaCustomsControl : CustomsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AsendiaCustomsControl()
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

            contentType.SelectedIndexChanged -= this.OnChangeOption;
            nonDeliveryType.SelectedIndexChanged -= this.OnChangeOption;

            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in shipments)
                {
                    if (shipment.Asendia == null)
                    {
                        ShippingManager.EnsureShipmentLoaded(shipment);
                    }

                    MethodConditions.EnsureArgumentIsNotNull(shipment.Asendia, "Asendia");

                    contentType.ApplyMultiValue((ShipEngineContentsType) shipment.Asendia.Contents);
                    nonDeliveryType.ApplyMultiValue((ShipEngineNonDeliveryType) shipment.Asendia.NonDelivery);
                }
            }

            contentType.SelectedIndexChanged += OnChangeOption;
            nonDeliveryType.SelectedIndexChanged += OnChangeOption;
        }

        /// <summary>
        /// Content type has changed
        /// </summary>
        void OnChangeOption(object sender, EventArgs e)
        {
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                contentType.ReadMultiValue(v => shipment.Asendia.Contents = (int) (ShipEngineContentsType) v);
                contentType.ReadMultiValue(v => shipment.Asendia.NonDelivery = (int) (ShipEngineNonDeliveryType) v);
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
                contentType.ReadMultiValue(v => shipment.Asendia.Contents = (int) (ShipEngineContentsType) v);
                nonDeliveryType.ReadMultiValue(v => shipment.Asendia.NonDelivery = (int) (ShipEngineNonDeliveryType) v);
            }
        }
    }
}
