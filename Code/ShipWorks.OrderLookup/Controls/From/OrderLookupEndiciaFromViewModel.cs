using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.UI;
using ShipWorks.UI.Controls.AddressControl;

namespace ShipWorks.OrderLookup.Controls.From
{
    /// <summary>
    /// View model for the From address
    /// </summary>
    [KeyedComponent(typeof(IOrderLookupFromViewModel), ShipmentTypeCode.Endicia)]
    [WpfView(typeof(OrderLookupEndiciaFromControl))]
    public class OrderLookupEndiciaFromViewModel : AddressViewModel, IOrderLookupFromViewModel
    {
        private string title;
        private bool rateShop;
        private IDisposable autoSave;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly ICarrierAccountRetrieverFactory carrierAccountRetrieverFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupEndiciaFromViewModel(IOrderLookupShipmentModel shipmentModel, IShippingOriginManager shippingOriginManager, IMessageHelper messageHelper,
            IShipmentTypeManager shipmentTypeManager, ICarrierAccountRetrieverFactory carrierAccountRetrieverFactory,
            IValidatedAddressScope validatedAddressScope, IAddressValidator validator, IAddressSelector addressSelector)
            : base(shippingOriginManager, messageHelper, validatedAddressScope, validator, addressSelector)
        {
            ShipmentModel = shipmentModel;
            this.shipmentTypeManager = shipmentTypeManager;
            this.carrierAccountRetrieverFactory = carrierAccountRetrieverFactory;
            ShipmentModel.PropertyChanged += ShipmentModelPropertyChanged;

            InitializeForChangedShipment();
        }

        /// <summary>
        /// The addresses title
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Title
        {
            get => title;
            set => handler.Set(nameof(Title), ref title, value);
        }

        /// <summary>
        /// Origin Rate shopping
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool RateShop
        {
            get => rateShop;
            set
            {
                handler.Set(nameof(RateShop), ref rateShop, value);
                UpdateTitle();
            }
        }

        /// <summary>
        /// Is address validation enabled or not
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderLookupShipmentModel ShipmentModel { get; }

        /// <summary>
        /// Save changes to the base entity whenever properties are changed in the view model
        /// </summary>
        private void Save()
        {
            if (ShipmentModel?.ShipmentAdapter?.Shipment?.OriginPerson != null)
            {
                SaveToEntity(ShipmentModel.ShipmentAdapter.Shipment.OriginPerson);
            }
        }

        /// <summary>
        /// Update when the order changes
        /// </summary>
        private void ShipmentModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ShipmentModel.SelectedOrder == null)
            {
                autoSave?.Dispose();
            }

            if (e.PropertyName == nameof(ShipmentModel.SelectedOrder) && ShipmentModel.SelectedOrder != null)
            {
                InitializeForChangedShipment();
            }

            if (e.PropertyName == ShipmentFields.OriginOriginID.Name)
            {
                long originId = ShipmentModel.ShipmentAdapter.Shipment.OriginOriginID;
                long orderId = ShipmentModel.SelectedOrder.OrderID;
                long accountId = ShipmentModel.ShipmentAdapter.AccountId.GetValueOrDefault();
                ShipmentTypeCode shipmentTypeCode = ShipmentModel.ShipmentAdapter.ShipmentTypeCode;
                StoreEntity store = ShipmentModel.ShipmentAdapter.Store;

                SetAddressFromOrigin(originId, orderId, accountId, shipmentTypeCode, store);

                UpdateTitle();

                handler.RaisePropertyChanged(nameof(ShipmentModel));
            }
        }

        /// <summary>
        /// Initialize UI for a changed or new shipment
        /// </summary>
        private void InitializeForChangedShipment()
        {
            autoSave?.Dispose();
            Load(ShipmentModel.ShipmentAdapter.Shipment.OriginPerson, ShipmentModel.ShipmentAdapter.Store);

            UpdateTitle();

            handler.RaisePropertyChanged(nameof(ShipmentModel));
            autoSave = handler.PropertyChangingStream.Where(p => p != nameof(Title)).Throttle(TimeSpan.FromMilliseconds(100)).Subscribe(_ => Save());
        }

        /// <summary>
        /// Update the title
        /// </summary>
        private void UpdateTitle()
        {
            string newTitle = "From";

            if (ShipmentModel.ShipmentAdapter != null)
            {
                ShipmentTypeCode shipmentTypeCode = ShipmentModel.ShipmentAdapter.ShipmentTypeCode;
                long originID = ShipmentModel.ShipmentAdapter.Shipment.OriginOriginID;

                List<KeyValuePair<string, long>> origins = shipmentTypeManager.Get(shipmentTypeCode).GetOrigins();

                string originDescription = string.Empty;
                if (origins.Any(o => o.Value == originID))
                {
                    originDescription = origins.First(w => w.Value == originID).Key;
                }

                string accountDescription = carrierAccountRetrieverFactory.Create(shipmentTypeCode)?
                                                .GetAccountReadOnly(ShipmentModel.ShipmentAdapter.Shipment)?.AccountDescription ?? string.Empty;

                string headerAccountText = RateShop ? "(Rate Shopping)" : accountDescription;

                if (!string.IsNullOrWhiteSpace(headerAccountText) && !string.IsNullOrWhiteSpace(originDescription))
                {
                    newTitle = $"From Account: {headerAccountText}, {originDescription}";
                }
            }

            Title = newTitle;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            autoSave?.Dispose();
            ShipmentModel.PropertyChanged -= ShipmentModelPropertyChanged;
        }
    }
}
