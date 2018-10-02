using System.ComponentModel;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.AddressValidation;
using ShipWorks.Shipping;
using ShipWorks.UI.Controls.AddressControl;
using System.Reflection;
using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Reactive.Linq;
using Interapptive.Shared.Business;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping.Carriers;

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

            Title = "From";
        }

        /// <summary>
        /// The addresses title
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Title
        {
            get { return title; }
            set { handler.Set(nameof(Title), ref title, value); }
        }

        /// <summary>
        /// Origin Rate shopping
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool RateShop
        {
            get { return rateShop; }
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
            if (Orchestrator.Order == null)
            {
                autoSave?.Dispose();
            }

            if (e.PropertyName == "Order" &&
                Orchestrator.Order != null)
            {
                base.Load(Orchestrator.ShipmentAdapter.Shipment.OriginPerson, Orchestrator.ShipmentAdapter.Store);
                autoSave?.Dispose();
                autoSave = handler.PropertyChangingStream.Throttle(TimeSpan.FromMilliseconds(100)).Subscribe(_ => Save());

                RateShop = Orchestrator.ShipmentAdapter.SupportsRateShopping;

                UpdateTitle();

                handler.RaisePropertyChanged(nameof(Orchestrator));
            }

            if (e.PropertyName == "OriginOriginID")
            {
                long originId = Orchestrator.ShipmentAdapter.Shipment.OriginOriginID;
                long orderId = Orchestrator.Order.OrderID;
                long accountId = Orchestrator.ShipmentAdapter.AccountId.GetValueOrDefault();
                ShipmentTypeCode shipmentTypeCode = Orchestrator.ShipmentAdapter.ShipmentTypeCode;
                StoreEntity store = Orchestrator.ShipmentAdapter.Store;

                base.SetAddressFromOrigin(originId, orderId, accountId, shipmentTypeCode, store);

                UpdateTitle();

                handler.RaisePropertyChanged(nameof(Orchestrator));
            }
        }

        /// <summary>
        /// Update the title
        /// </summary>
        private void UpdateTitle()
        {
            ShipmentTypeCode shipmentTypeCode = Orchestrator.ShipmentAdapter.ShipmentTypeCode;

            string originDescription = shipmentTypeManager.Get(shipmentTypeCode)
                .GetOrigins().First(w => w.Value == Orchestrator.ShipmentAdapter.Shipment.OriginOriginID).Key;

            ICarrierAccount account = carrierAccountRetrieverFactory.Create(shipmentTypeCode)
                .GetAccountReadOnly(Orchestrator.ShipmentAdapter.Shipment);

            string accountDescription = account == null ? "No Accounts" : account.AccountDescription;
            string headerAccountText = RateShop ? "(Rate Shopping)" : accountDescription;
            Title = $"From Account: {headerAccountText}, {originDescription}";
        }
    }
}
