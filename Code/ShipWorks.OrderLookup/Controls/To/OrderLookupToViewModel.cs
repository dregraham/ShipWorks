using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.AddressValidation;
using ShipWorks.Shipping;
using ShipWorks.UI.Controls.AddressControl;

namespace ShipWorks.OrderLookup.Controls.To
{
    /// <summary>
    /// ViewModel for To panel in the OrderLookup view
    /// </summary>
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.To)]
    public class OrderLookupToViewModel : AddressViewModel
    {
        private string title;
        IDisposable autoSave;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupToViewModel(IViewModelOrchestrator orchestrator, IShippingOriginManager shippingOriginManager, IMessageHelper messageHelper,
            IValidatedAddressScope validatedAddressScope, IAddressValidator validator, IAddressSelector addressSelector)
            : base(shippingOriginManager, messageHelper, validatedAddressScope, validator, addressSelector)
        {
            Orchestrator = orchestrator;
            Orchestrator.PropertyChanged += OrchestratorPropertyChanged;

            IsAddressValidationEnabled = true;
            Title = "To";
        }

        /// <summary>
        ///The addresses title
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Title
        {
            get { return title; }
            set { handler.Set(nameof(Title), ref title, value); }
        }

        /// <summary>
        /// Is address validation enabled or not
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IViewModelOrchestrator Orchestrator { get; private set; }        

        /// <summary>
        /// Save changes to the base entity whenever properties are changed in the view model
        /// </summary>
        private void Save()
        {
            if (Orchestrator?.ShipmentAdapter?.Shipment?.ShipPerson != null)
            {
                SaveToEntity(Orchestrator.ShipmentAdapter.Shipment.ShipPerson);
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

            if (e.PropertyName == "Order" && Orchestrator.Order != null)
            {
                base.Load(Orchestrator.ShipmentAdapter.Shipment.ShipPerson, Orchestrator.ShipmentAdapter.Store);

                autoSave?.Dispose();
                autoSave = handler.PropertyChangingStream.Throttle(TimeSpan.FromMilliseconds(500)).Subscribe(_ => Save());

                handler.RaisePropertyChanged(nameof(Orchestrator));

                string isDomestic = string.Empty;
                if (Orchestrator?.ShipmentAdapter?.IsDomestic != null)
                {
                    isDomestic = Orchestrator.ShipmentAdapter.IsDomestic ? "(Domestic)" : "(International)";
                }
                Title = $"To {FullName} {isDomestic}";
            }
        }
    }
}
