using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using ShipWorks.AddressValidation;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping;
using ShipWorks.UI;
using ShipWorks.UI.Controls.AddressControl;

namespace ShipWorks.OrderLookup.Controls.To
{
    /// <summary>
    /// ViewModel for To panel in the OrderLookup view
    /// </summary>
    [KeyedComponent(typeof(IToViewModel), ShipmentTypeCode.BestRate)]
    [KeyedComponent(typeof(IToViewModel), ShipmentTypeCode.Endicia)]
    [WpfView(typeof(GenericToControl))]
    public class GenericToViewModel : IToViewModel
    {
        private readonly PropertyChangedHandler handler;
        private string title;
        private IDisposable autoSave;
        private readonly AddressViewModel addressViewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericToViewModel(IOrderLookupShipmentModel shipmentModel, AddressViewModel addressViewModel)
        {
            this.addressViewModel = addressViewModel;
            ShipmentModel = shipmentModel;
            ShipmentModel.PropertyChanged += ShipmentModelPropertyChanged;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            if (ShipmentModel?.ShipmentAdapter?.Store != null)
            { 
                addressViewModel.IsAddressValidationEnabled = ShipmentModel.ShipmentAdapter.IsDomestic ? 
                    ShipmentModel.ShipmentAdapter.Store.DomesticAddressValidationSetting != AddressValidationStoreSettingType.ValidationDisabled :
                    ShipmentModel.ShipmentAdapter.Store.InternationalAddressValidationSetting != AddressValidationStoreSettingType.ValidationDisabled;
            }

            InitializeForChangedShipment();
        }

        /// <summary>
        /// Panel ID
        /// </summary>
        public SectionLayoutIDs PanelID => SectionLayoutIDs.To;

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
        /// Is the section visible
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Visible => true;

        /// <summary>
        /// Shipment model
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderLookupShipmentModel ShipmentModel { get; private set; }

        /// <summary>
        /// Address view model
        /// </summary>
        [Obfuscation(Exclude = true)]
        public AddressViewModel Address => addressViewModel;

        /// <summary>
        /// Save changes to the base entity whenever properties are changed in the view model
        /// </summary>
        private void Save()
        {
            if (ShipmentModel?.ShipmentAdapter?.Shipment?.ShipPerson != null)
            {
                addressViewModel.SaveToEntity(ShipmentModel.ShipmentAdapter.Shipment.ShipPerson);
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

            if (e.PropertyName == ShipmentFields.ShipCountryCode.Name ||
                e.PropertyName == ShipmentFields.OriginCountryCode.Name)
            {
                addressViewModel.IsAddressValidationEnabled = AddressValidationPolicy.IsValidationEnabled(ShipmentModel.ShipmentAdapter?.Store, ShipmentModel.ShipmentAdapter);
            }

            if (e.PropertyName == ShipmentFields.ShipCountryCode.Name ||
                e.PropertyName == ShipmentFields.OriginCountryCode.Name ||
                e.PropertyName == ShipmentFields.ShipLastName.Name ||
                e.PropertyName == ShipmentFields.ShipFirstName.Name)
            {
                UpdateTitle();
            }
        }

        /// <summary>
        /// Initialize view for changed or new shipment
        /// </summary>
        private void InitializeForChangedShipment()
        {
            autoSave?.Dispose();
            addressViewModel.Load(ShipmentModel.ShipmentAdapter.Shipment.ShipPerson, ShipmentModel.ShipmentAdapter.Store);

            UpdateTitle();

            handler.RaisePropertyChanged(nameof(ShipmentModel));
            autoSave = handler
                .Merge(addressViewModel.PropertyChangeStream)
                .Where(p => p != nameof(Title))
                .Throttle(TimeSpan.FromMilliseconds(100))
                .Subscribe(_ => Save());
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
            Title = $"To {addressViewModel.FullName} {isDomestic}";
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose()
        {
            autoSave?.Dispose();
            ShipmentModel.PropertyChanged -= ShipmentModelPropertyChanged;
        }
    }
}
