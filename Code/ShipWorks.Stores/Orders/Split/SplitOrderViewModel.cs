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

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// View Model for the split orders dialog
    /// </summary>
    [Component(Service = typeof(ISplitOrdersViewModel))]
    public class SplitOrderViewModel : ISplitOrdersViewModel, INotifyPropertyChanged
    {
        private readonly IMessageHelper messageHelper;
        private readonly ISplitOrderDialog splitOrdersDialog;
        //private readonly ISplitOrderAddressComparer addressComparer;
        private readonly PropertyChangedHandler handler;

        private string selectedOrderNumber;
        private string orderNumberPostfix;
        //private IOrderEntity survivingOrder;
        //private string addressName;
        //private string addressStreet;
        //private string addressCityStateZip;
        //private bool allAddressesMatch;

        /// <summary>
        /// Constructor
        /// </summary>
        public SplitOrderViewModel(IMessageHelper messageHelper,
            ISplitOrderDialog splitOrdersDialog
            //ISplitOrderAddressComparer addressComparer
            )
        {
            //this.addressComparer = addressComparer;
            this.splitOrdersDialog = splitOrdersDialog;
            this.messageHelper = messageHelper;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            ConfirmSplit = new RelayCommand(
                () => ConfirmCombineAction()
                //() => SurvivingOrder != null && !string.IsNullOrWhiteSpace(NewOrderNumber)
                );

            CancelSplit = new RelayCommand(() => CancelCombineAction());
        }

        /// <summary>
        /// Confirm split of the orders
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ConfirmSplit { get; }

        /// <summary>
        /// Cancel split orders
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand CancelSplit { get; }

        /// <summary>
        /// A property value has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Selected order number to use for the new order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string SelectedOrderNumber
        {
            get { return selectedOrderNumber; }
            set { handler.Set(nameof(SelectedOrderNumber), ref selectedOrderNumber, value); }
        }

        /// <summary>
        /// Order number postfix to use for the new order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string OrderNumberPostfix
        {
            get { return orderNumberPostfix; }
            set { handler.Set(nameof(OrderNumberPostfix), ref orderNumberPostfix, value); }
        }

        /// <summary>
        /// Order number with postfix to use for the new order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string NewOrderNumber => $"{SelectedOrderNumber}{OrderNumberPostfix}";

        /// <summary>
        /// Order that will be used as the basis for the combined order
        /// </summary>
        //[Obfuscation(Exclude = true)]
        //public IOrderEntity SurvivingOrder
        //{
        //    get { return survivingOrder; }
        //    set
        //    {
        //        if (handler.Set(nameof(SurvivingOrder), ref survivingOrder, value))
        //        {
        //            SetAddress(value);
        //            SelectedOrderNumber = value.OrderNumberComplete;
        //        }
        //    }
        //}

        /// <summary>
        /// Name of the recipient of the currently selected order
        /// </summary>
        //[Obfuscation(Exclude = true)]
        //public string AddressName
        //{
        //    get { return addressName; }
        //    set { handler.Set(nameof(AddressName), ref addressName, value); }
        //}

        /// <summary>
        /// Street of the recipient of the currently selected order
        /// </summary>
        //[Obfuscation(Exclude = true)]
        //public string AddressStreet
        //{
        //    get { return addressStreet; }
        //    set { handler.Set(nameof(AddressStreet), ref addressStreet, value); }
        //}

        /// <summary>
        /// City, state, and zip of the currently selected order
        /// </summary>
        //[Obfuscation(Exclude = true)]
        //public string AddressCityStateZip
        //{
        //    get { return addressCityStateZip; }
        //    set { handler.Set(nameof(AddressCityStateZip), ref addressCityStateZip, value); }
        //}

        /// <summary>
        /// States whether all the order addresses match or not
        /// </summary>
        //[Obfuscation(Exclude = true)]
        //public bool AllAddressesMatch
        //{
        //    get { return allAddressesMatch; }
        //    set { handler.Set(nameof(AllAddressesMatch), ref allAddressesMatch, value); }
        //}

        /// <summary>
        /// Orders that will be combined
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<IOrderEntity> Orders { get; set; }

        /// <summary>
        /// Get order combination details from user
        /// </summary>
        public GenericResult<(long, string)> GetCombinationDetailsFromUser(IEnumerable<IOrderEntity> orders)
        {
            Load(orders);
            splitOrdersDialog.DataContext = this;

            return messageHelper.ShowDialog(splitOrdersDialog) == true ?
                GenericResult.FromSuccess(Tuple.Create(null ,NewOrderNumber)) :
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
            SelectedOrderNumber = firstOrder.OrderNumberComplete;
            OrderNumberPostfix = string.IsNullOrWhiteSpace(OrderNumberPostfix) ? "-C" : OrderNumberPostfix;
            //SurvivingOrder = firstOrder;
        }

        /// <summary>
        /// Handle the confirmation of combining orders
        /// </summary>
        private void ConfirmCombineAction()
        {
            splitOrdersDialog.DialogResult = true;
            splitOrdersDialog.Close();
        }

        /// <summary>
        /// Cancel combining orders
        /// </summary>
        private void CancelCombineAction()
        {
            splitOrdersDialog.Close();
        }
    }
}
