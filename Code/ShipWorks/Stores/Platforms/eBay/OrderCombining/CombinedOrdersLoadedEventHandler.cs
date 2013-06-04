using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace ShipWorks.Stores.Platforms.Ebay.OrderCombining
{
    /// <summary>
    /// Delegate for the CandidatesLoaded event
    /// </summary>
    public delegate void CombinedOrdersLoadedEventHandler(object sender, CombinedOrdersLoadedEventArgs e);

    /// <summary>
    /// EventArgs for the CandidatesLoaded event handler
    /// </summary>
    public class CombinedOrdersLoadedEventArgs : AsyncCompletedEventArgs
    {
        // The top-level control
        Control owner;
        public Control Owner
        {
            get { return owner; }
        }

        /// <summary>
        /// The combined payments that were loaded
        /// </summary>
        List<CombinedOrder> combinedPayments;
        public List<CombinedOrder> CombinedPayments
        {
            get { return combinedPayments; }
        }

        /// <summary>
        /// The type of order combining operation that was initiated
        /// </summary>
        CombineType combineType;
        public CombineType CombineType
        {
            get { return combineType; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CombinedOrdersLoadedEventArgs(Control owner, Exception error, bool canceled, object userState, CombineType combineType, List<CombinedOrder> combinedPayments) :
            base(error, canceled, userState)
        {
            if (combinedPayments == null)
            {
                throw new ArgumentNullException("combinedPayments");
            }

            this.combineType = combineType;
            this.combinedPayments = combinedPayments;
            this.owner = owner;
        }
    }
}
