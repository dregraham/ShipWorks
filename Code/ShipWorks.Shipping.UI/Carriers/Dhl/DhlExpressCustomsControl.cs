using System;
using System.Collections.Generic;
using ShipWorks.Shipping.Editing;
using Interapptive.Shared.Utility;
using ShipWorks.UI.Controls;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipEngine;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping.UI.Carriers.Dhl
{
    /// <summary>
    /// DhlExpress specific customs stuff
    /// </summary>
    [KeyedComponent(typeof(CustomsControlBase),ShipmentTypeCode.DhlExpress)]
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
                    if (shipment.DhlExpress == null)
                    {
                        ShippingManager.EnsureShipmentLoaded(shipment);
                    }

                    if (shipment.DhlExpress == null)
                    {
                        throw new NullReferenceException("shipment.DhlExpress cannot be null.");
                    }
                    
                    contentType.ApplyMultiValue((ShipEngineContentsType) shipment.DhlExpress.Contents);
                    contentType.ApplyMultiValue((ShipEngineNonDeliveryType)shipment.DhlExpress.NonDelivery);
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
                contentType.ReadMultiValue(v => shipment.DhlExpress.Contents = (int)(ShipEngineContentsType)v);
                contentType.ReadMultiValue(v => shipment.DhlExpress.NonDelivery = (int)(ShipEngineNonDeliveryType)v);
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
                nonDeliveryType.ReadMultiValue(v => shipment.DhlExpress.NonDelivery = (int)(ShipEngineNonDeliveryType)v);
            }
        }
    }
}
