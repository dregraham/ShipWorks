using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.UI.Carriers.DhlEcommerce
{
    /// <summary>
    /// DHL eCommerce specific customs stuff
    /// </summary>
    [KeyedComponent(typeof(CustomsControlBase), ShipmentTypeCode.DhlEcommerce)]
    public partial class DhlEcommerceCustomsControl : CustomsControlBase
    {
        private readonly IShippingManager shippingManager;

        /// <summary>
        /// Constructor for Visual Studio Designer
        /// </summary>
        public DhlEcommerceCustomsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceCustomsControl(IShippingManager shippingManager) : this()
        {
            this.shippingManager = shippingManager;
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
                customsRecipientIssuingAuthority.DisplayMember = "Key";
                customsRecipientIssuingAuthority.ValueMember = "Value";
                customsRecipientIssuingAuthority.DataSource = Geography.Countries.Select(n => new KeyValuePair<string, string>(n, Geography.GetCountryCode(n))).ToList();
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
                    if (shipment.DhlEcommerce == null)
                    {
                        shippingManager.EnsureShipmentLoaded(shipment);
                    }

                    MethodConditions.EnsureArgumentIsNotNull(shipment.DhlEcommerce, nameof(shipment.DhlEcommerce));

                    contentType.ApplyMultiValue((ShipEngineContentsType) shipment.DhlEcommerce.Contents);
                    nonDeliveryType.ApplyMultiValue((ShipEngineNonDeliveryType) shipment.DhlEcommerce.NonDelivery);
                    customsRecipientTIN.ApplyMultiText(shipment.DhlEcommerce.CustomsRecipientTin);
                    customsRecipientTINType.ApplyMultiValue((TaxIdType) shipment.DhlEcommerce.CustomsTaxIdType);
                    customsRecipientIssuingAuthority.ApplyMultiValue(shipment.DhlEcommerce.CustomsTinIssuingAuthority);
                }
            }

            contentType.SelectedIndexChanged += OnChangeOption;
            nonDeliveryType.SelectedIndexChanged += OnChangeOption;
            customsRecipientTIN.TextChanged += this.OnChangeOption;
            customsRecipientTINType.SelectedIndexChanged += this.OnChangeOption;
            customsRecipientIssuingAuthority.SelectedIndexChanged += this.OnChangeOption;
        }

        /// <summary>
        /// An option has changed
        /// </summary>
        void OnChangeOption(object sender, EventArgs e)
        {
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                contentType.ReadMultiValue(v => shipment.DhlEcommerce.Contents = (int) (ShipEngineContentsType) v);
                nonDeliveryType.ReadMultiValue(v => shipment.DhlEcommerce.NonDelivery = (int) (ShipEngineNonDeliveryType) v);
                customsRecipientTIN.ReadMultiText(t => shipment.DhlEcommerce.CustomsRecipientTin = t);
                customsRecipientTINType.ReadMultiValue(v => shipment.DhlEcommerce.CustomsTaxIdType = (int) (TaxIdType) v);
                customsRecipientIssuingAuthority.ReadMultiValue(v => shipment.DhlEcommerce.CustomsTinIssuingAuthority = (string) v);
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
                contentType.ReadMultiValue(v => shipment.DhlEcommerce.Contents = (int) (ShipEngineContentsType) v);
                nonDeliveryType.ReadMultiValue(v => shipment.DhlEcommerce.NonDelivery = (int) (ShipEngineNonDeliveryType) v);
                customsRecipientTIN.ReadMultiText(t => shipment.DhlEcommerce.CustomsRecipientTin = t);
                customsRecipientTINType.ReadMultiValue(v => shipment.DhlEcommerce.CustomsTaxIdType = (int) (TaxIdType) v);
                customsRecipientIssuingAuthority.ReadMultiValue(v => shipment.DhlEcommerce.CustomsTinIssuingAuthority = (string) v);
            }
        }
    }
}
