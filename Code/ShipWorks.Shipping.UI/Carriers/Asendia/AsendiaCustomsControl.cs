using System;
using System.Collections.Generic;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.UI.Controls;

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

            EnumHelper.BindComboBox<TaxIdType>(customsRecipientTINType);
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

            // If the Issuing Authority hasn't been loaded yet do that now
            if (customsRecipientIssuingAuthority.DataSource == null)
            {
                customsRecipientIssuingAuthority.DataSource = Geography.Countries;
            }

            contentType.SelectedIndexChanged -= this.OnChangeOption;
            nonDeliveryType.SelectedIndexChanged -= this.OnChangeOption;
            customsRecipientTIN.TextChanged -= this.OnChangeOption;
            customsRecipientTINType.SelectedIndexChanged -= this.OnChangeOption;
            customsRecipientIssuingAuthority.SelectedIndexChanged -= this.OnChangeOption;

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
                    customsRecipientTIN.ApplyMultiText(shipment.Asendia.CustomsRecipientTin);
                    customsRecipientTINType.ApplyMultiValue((TaxIdType) shipment.Asendia.CustomsRecipientTinType);
                    customsRecipientIssuingAuthority.ApplyMultiText(Geography.GetCountryName(shipment.Asendia.CustomsRecipientIssuingAuthority));
                }
            }

            contentType.SelectedIndexChanged += OnChangeOption;
            nonDeliveryType.SelectedIndexChanged += OnChangeOption;
            customsRecipientTIN.TextChanged += this.OnChangeOption;
            customsRecipientTINType.SelectedIndexChanged += this.OnChangeOption;
            customsRecipientIssuingAuthority.SelectedIndexChanged += this.OnChangeOption;

            sku.Enabled = enableEditing;
        }

        /// <summary>
        /// Loads the form data.
        /// </summary>
        protected override void LoadFormData(ShipmentCustomsItemEntity customsItem)
        {
            base.LoadFormData(customsItem);

            sku.ApplyMultiText(customsItem.SKU);
        }

        /// <summary>
        /// Saves the customs item.
        /// </summary>
        protected override void SaveCustomsItem(ShipmentCustomsItemEntity customsItem, List<long> changedWeights, List<long> changedValues)
        {
            base.SaveCustomsItem(customsItem, changedWeights, changedValues);

            sku.ReadMultiText(s=> customsItem.SKU = s);
        }

        /// <summary>
        /// Content type has changed
        /// </summary>
        void OnChangeOption(object sender, EventArgs e)
        {
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                contentType.ReadMultiValue(v => shipment.Asendia.Contents = (int) (ShipEngineContentsType) v);
                nonDeliveryType.ReadMultiValue(v => shipment.Asendia.NonDelivery = (int) (ShipEngineNonDeliveryType) v);
                customsRecipientTIN.ReadMultiText(t => shipment.Asendia.CustomsRecipientTin = t);
                customsRecipientTINType.ReadMultiValue(v => shipment.Asendia.CustomsRecipientTinType = (int) (TaxIdType) v);
                customsRecipientIssuingAuthority.ReadMultiText(v => shipment.Asendia.CustomsRecipientIssuingAuthority = Geography.GetCountryCode(v));
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
                customsRecipientTIN.ReadMultiText(t => shipment.Asendia.CustomsRecipientTin = t);
                customsRecipientTINType.ReadMultiValue(v => shipment.Asendia.CustomsRecipientTinType = (int) (TaxIdType) v);
                customsRecipientIssuingAuthority.ReadMultiText(v => shipment.Asendia.CustomsRecipientIssuingAuthority = Geography.GetCountryCode(v));
            }
        }
    }
}
