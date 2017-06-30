using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.Business;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Core.Stores.Content;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// View Model for the combine orders dialog
    /// </summary>
    [Component(Service = typeof(ICombineOrdersViewModel))]
    public class CombineOrdersViewModel : ICombineOrdersViewModel, INotifyPropertyChanged
    {
        private readonly IMessageHelper messageHelper;
        private readonly ICombineOrdersDialog combineOrdersDialog;
        private readonly IOrderCombineAddressComparer addressComparer;
        private readonly PropertyChangedHandler handler;

        private string newOrderNumber;
        private IOrderEntity survivingOrder;
        private string addressName;
        private string addressStreet;
        private string addressCityStateZip;
        private bool allAddressesMatch;

        /// <summary>
        /// Constructor
        /// </summary>
        public CombineOrdersViewModel(IMessageHelper messageHelper,
            ICombineOrdersDialog combineOrdersDialog,
            IOrderCombineAddressComparer addressComparer)
        {
            this.addressComparer = addressComparer;
            this.combineOrdersDialog = combineOrdersDialog;
            this.messageHelper = messageHelper;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            ConfirmCombine = new RelayCommand(
                () => ConfirmCombineAction(),
                () => SurvivingOrder != null && !string.IsNullOrWhiteSpace(NewOrderNumber));

            CancelCombine = new RelayCommand(() => CancelCombineAction());
        }

        /// <summary>
        /// Confirm combination of the orders
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ConfirmCombine { get; }

        /// <summary>
        /// Cancel combining orders
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand CancelCombine { get; }

        /// <summary>
        /// A property value has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Order number to use for the new order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string NewOrderNumber
        {
            get { return newOrderNumber; }
            set { handler.Set(nameof(NewOrderNumber), ref newOrderNumber, value); }
        }

        /// <summary>
        /// Order that will be used as the basis for the combined order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderEntity SurvivingOrder
        {
            get { return survivingOrder; }
            set
            {
                if (handler.Set(nameof(SurvivingOrder), ref survivingOrder, value))
                {
                    SetAddress(value);
                }
            }
        }

        /// <summary>
        /// Name of the recipient of the currently selected order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string AddressName
        {
            get { return addressName; }
            set { handler.Set(nameof(AddressName), ref addressName, value); }
        }

        /// <summary>
        /// Street of the recipient of the currently selected order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string AddressStreet
        {
            get { return addressStreet; }
            set { handler.Set(nameof(AddressStreet), ref addressStreet, value); }
        }

        /// <summary>
        /// City, state, and zip of the currently selected order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string AddressCityStateZip
        {
            get { return addressCityStateZip; }
            set { handler.Set(nameof(AddressCityStateZip), ref addressCityStateZip, value); }
        }

        /// <summary>
        /// States whether all the order addresses match or not
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool AllAddressesMatch
        {
            get { return allAddressesMatch; }
            set { handler.Set(nameof(AllAddressesMatch), ref allAddressesMatch, value); }
        }

        /// <summary>
        /// Orders that will be combined
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<IOrderEntity> Orders { get; set; }

        /// <summary>
        /// Get order combination details from user
        /// </summary>
        public GenericResult<Tuple<long, string>> GetCombinationDetailsFromUser(IEnumerable<IOrderEntity> orders)
        {
            Load(orders);
            combineOrdersDialog.DataContext = this;

            return messageHelper.ShowDialog(combineOrdersDialog) == true ?
                GenericResult.FromSuccess(Tuple.Create(SurvivingOrder.OrderID, NewOrderNumber)) :
                GenericResult.FromError<Tuple<long, string>>("Canceled");
        }

        /// <summary>
        /// Load the orders into the view model
        /// </summary>
        private void Load(IEnumerable<IOrderEntity> orders)
        {
            if (orders?.Any() != true)
            {
                throw new ArgumentException("Orders cannot be null or empty");
            }

            Orders = orders.ToReadOnly();

            IOrderEntity firstOrder = orders.First();
            NewOrderNumber = firstOrder.OrderNumberComplete + "-C";
            SurvivingOrder = firstOrder;
            SetAddress(SurvivingOrder);

            AllAddressesMatch = orders.Select(x => x.ShipPerson)
                .Distinct(addressComparer).IsCountEqualTo(1);
        }

        /// <summary>
        /// Handle the confirmation of combining orders
        /// </summary>
        private void ConfirmCombineAction()
        {
            combineOrdersDialog.DialogResult = true;
            combineOrdersDialog.Close();
        }

        /// <summary>
        /// Cancel combining orders
        /// </summary>
        private void CancelCombineAction()
        {
            combineOrdersDialog.Close();
        }

        /// <summary>
        /// Set the address properties from the given person adapter
        /// </summary>
        private void SetAddress(IOrderEntity selectedOrder)
        {
            PersonAdapter address = selectedOrder.ShipPerson;

            AddressName = address.ParsedName.FullName;
            AddressStreet = JoinAddressPieces(", ", address.Street1, address.Street2);

            string stateZip = JoinAddressPieces(" ", address.StateProvCode, address.PostalCode);
            AddressCityStateZip = JoinAddressPieces(", ", address.City, stateZip);
        }

        /// <summary>
        /// Join pieces of an address using the specified separator
        /// </summary>
        private string JoinAddressPieces(string separator, params string[] pieces) =>
            string.Join(separator, pieces.Where(x => !string.IsNullOrWhiteSpace(x)));
    }
}
