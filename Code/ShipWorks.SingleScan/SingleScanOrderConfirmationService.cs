using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Confirm what the user wants to do when auto printing and multiple matching orders are found
    /// </summary>
    [Component]
    public class SingleScanOrderConfirmationService : ISingleScanOrderConfirmationService
    {
        private readonly IAutoPrintConfirmationDlgFactory dlgFactory;
        private readonly IMessageHelper messageHelper;
        private readonly IStoreManager storeManager;

        public SingleScanOrderConfirmationService(IAutoPrintConfirmationDlgFactory dlgFactory, IMessageHelper messageHelper, IStoreManager storeManager)
        {
            this.dlgFactory = dlgFactory;
            this.messageHelper = messageHelper;
            this.storeManager = storeManager;
        }

        /// <summary>
        /// Confirms that the order with the given orderId should be printed.
        /// </summary>
        public bool Confirm(long orderId, int numberOfMatchedOrders, string scanText)
        {
            // We should never get here with 0 matched orders
            if (numberOfMatchedOrders == 0)
            {
                throw new ShippingException("Unable to locate order for processing.");
            }

            if (numberOfMatchedOrders > 1)
            {
                MessagingText messaging = GetMessageingText(orderId, numberOfMatchedOrders);
                return messageHelper.ShowDialog(() => dlgFactory.Create(scanText, messaging)) == DialogResult.OK;
            }

            return true;
        }

        /// <summary>
        /// Generate the message displayed to the user
        /// </summary>
        private MessagingText GetMessageingText(long orderId, int numberOfMatchedOrders)
        {
            string store = storeManager.GetRelatedStore(orderId)?.StoreName;

            return new MessagingText
            {
                Title = "Multiple Matches Found",
                Body = $"ShipWorks found {numberOfMatchedOrders} orders matching this order number. The most recent order is from your '{store}' store. Scan the bar code again or click 'Use Most Recent Order' to print the label(s) for this order.",
                Continue = "Use Most Recent Order"
            };

        }
    }
}