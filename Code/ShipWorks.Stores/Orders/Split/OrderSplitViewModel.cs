﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Orders.Split.Errors;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// View Model for the split orders dialog
    /// </summary>
    [Component(Service = typeof(IOrderSplitViewModel))]
    public class OrderSplitViewModel : IOrderSplitViewModel, INotifyPropertyChanged
    {
        private readonly IAsyncMessageHelper messageHelper;
        private readonly IOrderSplitDialog splitOrdersDialog;
        private readonly ILicenseService licenseService;
        private readonly PropertyChangedHandler handler;
        private readonly IStoreTypeManager storeTypeManager;

        private string selectedOrderNumber;
        private string orderNumberPostfix;
        private decimal originalTotalCharge;
        private decimal splitTotalCharge;
        private OrderSplitterType splitType = OrderSplitterType.Reroute;
        private bool canChangeSplitType;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitViewModel(IAsyncMessageHelper messageHelper,
            IOrderSplitDialog splitOrdersDialog,
            ILicenseService licenseService,
            IStoreTypeManager storeTypeManager)
        {
            this.splitOrdersDialog = splitOrdersDialog;
            this.licenseService = licenseService;
            this.messageHelper = messageHelper;
            this.storeTypeManager = storeTypeManager;

            //userSession.

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
            get => orderNumberPostfix;
            set => handler.Set(nameof(OrderNumberPostfix), ref orderNumberPostfix, value);
        }

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
        /// Are there any items
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool AnyItems => Items.Any();

        /// <summary>
        /// Are there any charges
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool AnyCharges => Charges.Any();

        /// <summary>
        /// Original total charge
        /// </summary>
        [Obfuscation(Exclude = true)]
        public decimal OriginalTotalCharge
        {
            get => originalTotalCharge;
            set => handler.Set(nameof(OriginalTotalCharge), ref originalTotalCharge, value);
        }

        /// <summary>
        /// Split total charge
        /// </summary>
        [Obfuscation(Exclude = true)]
        public decimal SplitTotalCharge
        {
            get => splitTotalCharge;
            set => handler.Set(nameof(SplitTotalCharge), ref splitTotalCharge, value);
        }

        /// <summary>
        /// Type of split to perform
        /// </summary>
        [Obfuscation(Exclude = true)]
        public OrderSplitterType SplitType
        {
            get => splitType;
            set => handler.Set(nameof(SplitType), ref splitType, value);
        }

        /// <summary>
        /// Is this a Hub customer
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool CanChangeSplitType
        {
            get => canChangeSplitType;
            set => handler.Set(nameof(CanChangeSplitType), ref canChangeSplitType, value);
        }

        [Obfuscation(Exclude = true)]
        public bool ShowItemQuantityDecimals => !Items.All(x => x.TotalQuantity.IsInt());

        /// <summary>
        /// Get order split details from user
        /// </summary>
        public Task<OrderSplitDefinition> GetSplitDetailsFromUser(OrderEntity order, string suggestedOrderNumber)
        {
            Load(order, suggestedOrderNumber);

            return messageHelper
                .ShowDialog(() => SetupDialog())
                .Bind(x => x == true ?
                    Task.FromResult(new OrderSplitDefinition(order, BuildItemQuantities(), BuildItemCharges(), SelectedOrderNumber + OrderNumberPostfix, SplitType)) :
                    Task.FromException<OrderSplitDefinition>(Error.Canceled));
        }

        /// <summary>
        /// Setup the split orders dialog
        /// </summary>
        private IDialog SetupDialog()
        {
            splitOrdersDialog.DataContext = this;
            return splitOrdersDialog;
        }

        /// <summary>
        /// Load the orders into the view model
        /// </summary>
        private void Load(IOrderEntity order, string suggestedOrderNumber)
        {
            MethodConditions.EnsureArgumentIsNotNull(order, nameof(order));

            var storeType = storeTypeManager.GetType(order.Store.StoreTypeCode);

            CanChangeSplitType = licenseService.IsHub &&
                storeType.ShouldUseHub(order.Store) &&
                (order.CombineSplitStatus == CombineSplitStatusType.None || order.CombineSplitStatus == CombineSplitStatusType.Split);
            SplitType = CanChangeSplitType ? OrderSplitterType.Reroute : OrderSplitterType.Local;
            SelectedOrderNumber = order.OrderNumberComplete;
            OrderNumberPostfix = suggestedOrderNumber;

            Items = order.OrderItems?.Select(x => new OrderSplitItemViewModel(x)).ToImmutableList() ??
                Enumerable.Empty<OrderSplitItemViewModel>();
            Charges = order.OrderCharges?.Select(CreateChargeViewModel).ToImmutableList() ??
                Enumerable.Empty<OrderSplitChargeViewModel>();

            UpdateTotalCharges(string.Empty);
        }

        /// <summary>
        /// Create a view model for an order charge
        /// </summary>
        private OrderSplitChargeViewModel CreateChargeViewModel(IOrderChargeEntity charge)
        {
            var viewModel = new OrderSplitChargeViewModel(charge);

            viewModel.PropertyChangedStream
                .Where(x => x == nameof(OrderSplitChargeViewModel.OriginalAmount))
                .Do(UpdateTotalCharges)
                .Subscribe();

            return viewModel;
        }

        /// <summary>
        /// Update the total charges when an original amount has changed
        /// </summary>
        private void UpdateTotalCharges(string _propertyName)
        {
            OriginalTotalCharge = Charges.Select(x => x.OriginalAmount).Sum();
            SplitTotalCharge = Charges.Select(x => x.SplitAmountValue).Sum();
        }

        /// <summary>
        /// Build the list of item charges
        /// </summary>
        private IDictionary<long, decimal> BuildItemCharges() =>
            Charges.ToImmutableDictionary(x => x.OrderChargeID, x => x.SplitAmountValue);

        /// <summary>
        /// Build the list of item quantities
        /// </summary>
        private IDictionary<long, decimal> BuildItemQuantities() =>
            Items.ToImmutableDictionary(x => x.OrderItemID, x => x.SplitQuantityValue);

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
        private void CancelSplitAction() =>
            splitOrdersDialog.Close();
    }
}
