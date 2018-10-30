using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.Custom.EntityClasses;
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
    [KeyedComponent(typeof(IFromViewModel), ShipmentTypeCode.BestRate)]
    [KeyedComponent(typeof(IFromViewModel), ShipmentTypeCode.Endicia)]
    [KeyedComponent(typeof(IFromViewModel), ShipmentTypeCode.UpsOnLineTools)]
    [WpfView(typeof(GenericFromControl))]
    public class GenericFromViewModel : IFromViewModel
    {
        private string title;
        private IDisposable autoSave;
        private readonly PropertyChangedHandler handler;
        private readonly IShipmentTypeManager shipmentTypeManager;

        private readonly AddressViewModel addressViewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFromViewModel(
            IOrderLookupShipmentModel shipmentModel,
            IShipmentTypeManager shipmentTypeManager,
            ICarrierAccountRetrieverFactory carrierAccountRetrieverFactory,
            AddressViewModel addressViewModel)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            this.addressViewModel = addressViewModel;

            ShipmentModel = shipmentModel;
            this.shipmentTypeManager = shipmentTypeManager;

            Accounts = GetAccounts(carrierAccountRetrieverFactory);

            if (!ShipmentModel.ShipmentAdapter.AccountId.HasValue || 
                Accounts.TryGetValue(ShipmentModel.ShipmentAdapter.AccountId.Value, out string x))
            {
                ShipmentModel.ShipmentAdapter.AccountId = Accounts.Select(e=>e.Key).FirstOrDefault();
            }
            
            ShipmentModel.PropertyChanged += ShipmentModelPropertyChanged;

            InitializeForChangedShipment();
        }

        /// <summary>
        /// Get a list of accounts
        /// </summary>
        private Dictionary<long, string> GetAccounts(ICarrierAccountRetrieverFactory carrierAccountRetrieverFactory)
        {
            IEnumerable<ICarrierAccount> accounts = carrierAccountRetrieverFactory.Create(ShipmentModel.ShipmentAdapter.ShipmentTypeCode).Accounts;
            if (accounts.None())
            {
                accounts = new List<ICarrierAccount> { new NullCarrierAccount() };
            }

            return accounts.ToDictionary(a => a.AccountId, a => a.AccountDescription);
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
        /// Is the section visible
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Visible => true;

        /// <summary>
        /// Basic address details
        /// </summary>
        [Obfuscation(Exclude = true)]
        public AddressViewModel Address => addressViewModel;

        /// <summary>
        /// Carrier accounts
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Dictionary<long, string> Accounts { get; }

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
                addressViewModel.SaveToEntity(ShipmentModel.ShipmentAdapter.Shipment.OriginPerson);
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

                addressViewModel.SetAddressFromOrigin(originId, orderId, accountId, shipmentTypeCode, store);

                UpdateTitle();
            }
        }

        /// <summary>
        /// Initialize UI for a changed or new shipment
        /// </summary>
        private void InitializeForChangedShipment()
        {
            autoSave?.Dispose();
            addressViewModel.Load(ShipmentModel.ShipmentAdapter.Shipment.OriginPerson, ShipmentModel.ShipmentAdapter.Store);

            UpdateTitle();

            handler.RaisePropertyChanged(nameof(ShipmentModel));
            autoSave = handler.PropertyChangingStream
                .Merge(addressViewModel.PropertyChangeStream)
                .Where(p => p != nameof(Title))
                .Throttle(TimeSpan.FromMilliseconds(100))
                .Subscribe(_ => Save());
        }

        /// <summary>
        /// Update the title
        /// </summary>
        protected void UpdateTitle()
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

                string accountDescription = GetHeaderAccountText();

                if (!string.IsNullOrWhiteSpace(accountDescription) && !string.IsNullOrWhiteSpace(originDescription))
                {
                    newTitle = $"From Account: {accountDescription}, {originDescription}";
                }
                else if (string.IsNullOrWhiteSpace(accountDescription) && !string.IsNullOrWhiteSpace(originDescription))
                {
                    newTitle = $"From Account: {originDescription}";
                }
            }

            Title = newTitle;
        }

        /// <summary>
        /// Get the text for the account header
        /// </summary>
        protected virtual string GetHeaderAccountText()
        {
            return ShipmentModel.ShipmentAdapter.AccountId.HasValue && ShipmentModel.ShipmentAdapter.AccountId.Value != 0 ? 
                Accounts.SingleOrDefault(a => a.Key == ShipmentModel.ShipmentAdapter.AccountId).Value :
                "(None)";
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose()
        {
            autoSave?.Dispose();
            addressViewModel.Dispose();
            ShipmentModel.PropertyChanged -= ShipmentModelPropertyChanged;
        }
    }
}
