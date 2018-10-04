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
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.UI.Controls.AddressControl;

namespace ShipWorks.OrderLookup.Controls.From
{
    /// <summary>
    /// View model for the From address
    /// </summary>
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.From)]
    public class OrderLookupFromViewModel : AddressViewModel
    {
        private string title;
        private bool rateShop;
        IDisposable autoSave;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly ICarrierAccountRetrieverFactory carrierAccountRetrieverFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupFromViewModel(IViewModelOrchestrator orchestrator, IShippingOriginManager shippingOriginManager, IMessageHelper messageHelper,
            IShipmentTypeManager shipmentTypeManager, ICarrierAccountRetrieverFactory carrierAccountRetrieverFactory,
            IValidatedAddressScope validatedAddressScope, IAddressValidator validator, IAddressSelector addressSelector)
            : base(shippingOriginManager, messageHelper, validatedAddressScope, validator, addressSelector)
        {
            Orchestrator = orchestrator;
            this.shipmentTypeManager = shipmentTypeManager;
            this.carrierAccountRetrieverFactory = carrierAccountRetrieverFactory;
            Orchestrator.PropertyChanged += OrchestratorPropertyChanged;

            UpdateTitle();
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
        public IViewModelOrchestrator Orchestrator { get; }

        /// <summary>
        /// Save changes to the base entity whenever properties are changed in the view model
        /// </summary>
        private void Save()
        {
            if (Orchestrator?.ShipmentAdapter?.Shipment?.OriginPerson != null)
            {
                SaveToEntity(Orchestrator.ShipmentAdapter.Shipment.OriginPerson);
            }

            if (Orchestrator?.ShipmentAdapter?.ShipmentTypeCode == ShipmentTypeCode.Usps)
            {
                Orchestrator.ShipmentAdapter.Shipment.Postal.Usps.RateShop = RateShop;
            }
        }

        /// <summary>
        /// Update when the order changes
        /// </summary>
        private void OrchestratorPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Orchestrator.SelectedOrder == null)
            {
                autoSave?.Dispose();
            }

            if (e.PropertyName == "SelectedOrder" && Orchestrator.SelectedOrder != null)
            {
                Load(Orchestrator.ShipmentAdapter.Shipment.OriginPerson, Orchestrator.ShipmentAdapter.Store);
                autoSave?.Dispose();
                autoSave = handler.PropertyChangingStream.Throttle(TimeSpan.FromMilliseconds(100)).Subscribe(_ => Save());

                RateShop = Orchestrator.ShipmentAdapter.SupportsRateShopping;

                UpdateTitle();

                handler.RaisePropertyChanged(nameof(Orchestrator));
            }

            if (e.PropertyName == "OriginOriginID")
            {
                long originId = Orchestrator.ShipmentAdapter.Shipment.OriginOriginID;
                long orderId = Orchestrator.SelectedOrder.OrderID;
                long accountId = Orchestrator.ShipmentAdapter.AccountId.GetValueOrDefault();
                ShipmentTypeCode shipmentTypeCode = Orchestrator.ShipmentTypeCode;
                StoreEntity store = Orchestrator.ShipmentAdapter.Store;

                SetAddressFromOrigin(originId, orderId, accountId, shipmentTypeCode, store);

                UpdateTitle();

                handler.RaisePropertyChanged(nameof(Orchestrator));
            }
        }

        /// <summary>
        /// Update the title
        /// </summary>
        private void UpdateTitle()
        {
            string newTitle = "From";

            if (Orchestrator.ShipmentAdapter != null)
            {
                ShipmentTypeCode shipmentTypeCode = Orchestrator.ShipmentAdapter.ShipmentTypeCode;
                long originID = Orchestrator.ShipmentAdapter.Shipment.OriginOriginID;

                List<KeyValuePair<string, long>> origins = shipmentTypeManager.Get(shipmentTypeCode).GetOrigins();

                string originDescription = string.Empty;
                if (origins.Any(o => o.Value == originID))
                {
                    originDescription = origins.First(w => w.Value == originID).Key;
                }

                string accountDescription = carrierAccountRetrieverFactory?.Create(shipmentTypeCode)?
                                                .GetAccountReadOnly(Orchestrator.ShipmentAdapter.Shipment)?.AccountDescription ?? string.Empty;

                string headerAccountText = RateShop ? "(Rate Shopping)" : accountDescription;

                if (!string.IsNullOrWhiteSpace(headerAccountText) && !string.IsNullOrWhiteSpace(originDescription))
                {
                    newTitle = $"From Account: {headerAccountText}, {originDescription}";
                }
            }

            Title = newTitle;
        }
    }
}
