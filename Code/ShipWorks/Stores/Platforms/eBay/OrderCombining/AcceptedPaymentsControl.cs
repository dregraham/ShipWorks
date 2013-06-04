using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay.OrderCombining
{
    public partial class AcceptedPaymentsControl : UserControl
    {
        // UI wrapper for displaying the payment type with custom text
        class ListItemWrapper
        {
            public BuyerPaymentMethodCodeType Code
            {
                get;
                set;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="code"></param>
            public ListItemWrapper(BuyerPaymentMethodCodeType code)
            {
                this.Code = code;
            }

            /// <summary>
            /// Output string representation
            /// </summary>
            public override string ToString()
            {
                return AcceptedPayments.GetBuyerPaymentMethodString(Code);
            }
        }

        // Collection of the possible values that we'll expose the user to.  Limited because many
        // just don't make sense for our customers or are reserved for eBay future use
        List<BuyerPaymentMethodCodeType> displayedPaymentTypes = new List<BuyerPaymentMethodCodeType>()
        {
            BuyerPaymentMethodCodeType.AmEx,
            BuyerPaymentMethodCodeType.CashOnPickup,
            BuyerPaymentMethodCodeType.CCAccepted,
            BuyerPaymentMethodCodeType.COD,
            BuyerPaymentMethodCodeType.Discover,
            BuyerPaymentMethodCodeType.MOCC,
            BuyerPaymentMethodCodeType.MoneyXferAccepted,
            BuyerPaymentMethodCodeType.OtherOnlinePayments,
            BuyerPaymentMethodCodeType.PaymentSeeDescription,
            BuyerPaymentMethodCodeType.PayPal,
            BuyerPaymentMethodCodeType.VisaMC
        };

        /// <summary>
        /// Constructor
        /// </summary>
        public AcceptedPaymentsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the UI from the provided store entity
        /// </summary>
        public void LoadStore(EbayStoreEntity ebayStore)
        {
            List<BuyerPaymentMethodCodeType> selectedCodes = AcceptedPayments.ParseList(ebayStore.AcceptedPaymentList);

            // Populate the checkboxlist with the possible values
            foreach (BuyerPaymentMethodCodeType code in displayedPaymentTypes)
            {
                acceptedPaymentsList.Items.Add(new ListItemWrapper(code), selectedCodes.Contains(code));
            }
        }

        /// <summary>
        /// Saves the UI selections to the store entity provide
        /// </summary>
        public bool SaveToEntity(EbayStoreEntity ebayStore)
        {
            // get the selected items
            List<BuyerPaymentMethodCodeType> checkedCodes = acceptedPaymentsList.CheckedItems.Cast<ListItemWrapper>().Select(w => w.Code).ToList();

            // build the string and save it to the store
            ebayStore.AcceptedPaymentList = AcceptedPayments.AssembleValue(checkedCodes);

            // OK to continue
            return true;
        }
    }
}
