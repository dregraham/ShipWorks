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

namespace ShipWorks.OrderLookup.Controls.From
{
    /// <summary>
    /// View model for the From address 
    /// </summary>
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.From)]
    public class OrderLookupFromViewModel : AddressViewModel
    {
        IDisposable autoSave;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupFromViewModel(IViewModelOrchestrator orchestrator, IShippingOriginManager shippingOriginManager, IMessageHelper messageHelper,
            IValidatedAddressScope validatedAddressScope, IAddressValidator validator, IAddressSelector addressSelector)
            : base(shippingOriginManager, messageHelper, validatedAddressScope, validator, addressSelector)
        {
            Orchestrator = orchestrator;
            Orchestrator.PropertyChanged += OrchestratorPropertyChanged;
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
                autoSave = handler.PropertyChangingStream.Throttle(TimeSpan.FromMilliseconds(500)).Subscribe(_ => Save());
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
                handler.RaisePropertyChanged(nameof(Orchestrator));
            }
        }
    }
}
