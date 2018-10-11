using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.HelperClasses;
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
        public OrderLookupToViewModel(IOrderLookupShipmentModel shipmentModel, IShippingOriginManager shippingOriginManager, IMessageHelper messageHelper,
            IValidatedAddressScope validatedAddressScope, IAddressValidator validator, IAddressSelector addressSelector)
            : base(shippingOriginManager, messageHelper, validatedAddressScope, validator, addressSelector)
        {
            ShipmentModel = shipmentModel;
            ShipmentModel.PropertyChanged += ShipmentModelPropertyChanged;

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
        public IOrderLookupShipmentModel ShipmentModel { get; private set; }

        /// <summary>
        /// Save changes to the base entity whenever properties are changed in the view model
        /// </summary>
        private void Save()
        {
            if (ShipmentModel?.ShipmentAdapter?.Shipment?.ShipPerson != null)
            {
                SaveToEntity(ShipmentModel.ShipmentAdapter.Shipment.ShipPerson);
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
                autoSave?.Dispose();
                Load(ShipmentModel.ShipmentAdapter.Shipment.ShipPerson, ShipmentModel.ShipmentAdapter.Store);

                UpdateTitle();

                handler.RaisePropertyChanged(nameof(ShipmentModel));
                autoSave = handler.PropertyChangingStream.Where(p => p != nameof(Title)).Throttle(TimeSpan.FromMilliseconds(100)).Subscribe(_ => Save());
            }

            if (e.PropertyName == ShipmentFields.ShipCountryCode.Name ||
                e.PropertyName == ShipmentFields.ShipLastName.Name ||
                e.PropertyName == ShipmentFields.ShipFirstName.Name)
            {
                UpdateTitle();
            }
        }

        /// <summary>
        /// Update the title of the address
        /// </summary>
        private void UpdateTitle()
        {
            string isDomestic = string.Empty;
            if (ShipmentModel?.ShipmentAdapter?.IsDomestic != null)
            {
                isDomestic = ShipmentModel.ShipmentAdapter.IsDomestic ? "(Domestic)" : "(International)";
            }
            Title = $"To {FullName} {isDomestic}";
        }
    }
}
