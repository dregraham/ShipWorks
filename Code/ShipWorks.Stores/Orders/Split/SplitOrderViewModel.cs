using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
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
        private readonly PropertyChangedHandler handler;

        private string selectedOrderNumber;
        private string orderNumberPostfix;

        /// <summary>
        /// Constructor
        /// </summary>
        public SplitOrderViewModel(IMessageHelper messageHelper,
            ISplitOrderDialog splitOrdersDialog
            )
        {
            this.splitOrdersDialog = splitOrdersDialog;
            this.messageHelper = messageHelper;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            ConfirmSplit = new RelayCommand(() => ConfirmSplitAction());
            CancelSplit = new RelayCommand(() => CancelSplitAction());
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
        /// Get order split details from user
        /// </summary>
        public GenericResult<SplitOrderDefinition> GetSplitDetailsFromUser(IOrderEntity order)
        {
            Load(order);
            splitOrdersDialog.DataContext = this;

            return messageHelper.ShowDialog(splitOrdersDialog) == true ?
                GenericResult.FromSuccess(new SplitOrderDefinition(null, null, SelectedOrderNumber + OrderNumberPostfix)) :
                GenericResult.FromError<SplitOrderDefinition>("Canceled");
        }

        /// <summary>
        /// Load the orders into the view model
        /// </summary>
        private void Load(IOrderEntity order)
        {
            MethodConditions.EnsureArgumentIsNotNull(order, nameof(order));
            SelectedOrderNumber = order.OrderNumberComplete;
            OrderNumberPostfix = "-1";
        }

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
