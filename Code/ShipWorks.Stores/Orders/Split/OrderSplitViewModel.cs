﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// View Model for the split orders dialog
    /// </summary>
    [Component(Service = typeof(IOrderSplitViewModel))]
    public class OrderSplitViewModel : IOrderSplitViewModel, INotifyPropertyChanged
    {
        private readonly IMessageHelper messageHelper;
        private readonly IOrderSplitDialog splitOrdersDialog;
        private readonly PropertyChangedHandler handler;

        private string selectedOrderNumber;
        private string orderNumberPostfix;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitViewModel(IMessageHelper messageHelper, IOrderSplitDialog splitOrdersDialog)
        {
            this.splitOrdersDialog = splitOrdersDialog;
            this.messageHelper = messageHelper;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            ConfirmSplit = new RelayCommand(() => ConfirmSplitAction());
            CancelSplit = new RelayCommand(() => CancelSplitAction());
        }

        /// <summary>
        /// A property value has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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
        /// Order items
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<OrderSplitItemViewModel> Items { get; set; }

        /// <summary>
        /// Order charges
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<OrderSplitChargeViewModel> Charges { get; set; }

        /// <summary>
        /// Get order split details from user
        /// </summary>
        public GenericResult<OrderSplitDefinition> GetSplitDetailsFromUser(OrderEntity order, string suggestedOrderNumber)
        {
            Load(order, suggestedOrderNumber);
            splitOrdersDialog.DataContext = this;

            return messageHelper.ShowDialog(splitOrdersDialog) == true ?
                new OrderSplitDefinition(order, BuildItemQuantities(), BuildItemCharges(), SelectedOrderNumber + OrderNumberPostfix) :
                GenericResult.FromError<OrderSplitDefinition>("Canceled");
        }

        /// <summary>
        /// Load the orders into the view model
        /// </summary>
        private void Load(IOrderEntity order, string suggestedOrderNumber)
        {
            MethodConditions.EnsureArgumentIsNotNull(order, nameof(order));

            SelectedOrderNumber = order.OrderNumberComplete;
            OrderNumberPostfix = suggestedOrderNumber;

            Items = order.OrderItems?.Select(x => new OrderSplitItemViewModel(x)).ToImmutableList() ??
                Enumerable.Empty<OrderSplitItemViewModel>();
            Charges = order.OrderCharges?.Select(x => new OrderSplitChargeViewModel(x)).ToImmutableList() ??
                Enumerable.Empty<OrderSplitChargeViewModel>();
        }

        /// <summary>
        /// Build the list of item charges
        /// </summary>
        private IDictionary<long, decimal> BuildItemCharges() =>
            Charges.ToImmutableDictionary(x => x.OrderChargeID, x => x.SplitAmount);

        /// <summary>
        /// Build the list of item quantities
        /// </summary>
        private IDictionary<long, double> BuildItemQuantities() =>
            Items.ToImmutableDictionary(x => x.OrderItemID, x => x.SplitQuantity);

        /// <summary>
        /// Handle the confirmation of combining orders
        /// </summary>
        private void ConfirmSplitAction()
        {
            splitOrdersDialog.DialogResult = true;
            splitOrdersDialog.Close();
        }

        /// <summary>
        /// Cancel combining orders
        /// </summary>
        private void CancelSplitAction()
        {
            splitOrdersDialog.Close();
        }
    }
}
