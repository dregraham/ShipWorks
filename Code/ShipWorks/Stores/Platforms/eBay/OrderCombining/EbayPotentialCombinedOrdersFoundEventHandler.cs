using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using Interapptive.Shared;

namespace ShipWorks.Stores.Platforms.Ebay.OrderCombining
{
    /// <summary>
    /// Delegate for the EbayPotentialCombinedOrdersFound event
    /// </summary>
    public delegate void EbayPotentialCombinedOrdersFoundEventHandler(object sender, EbayPotentialCombinedOrdersFoundEventArgs e);

    /// <summary>
    /// EventArgs for the EbayPotentialCombinedOrdersFound event handler
    /// </summary>
    public class EbayPotentialCombinedOrdersFoundEventArgs : AsyncCompletedEventArgs
    {
        Control owner;
        List<EbayCombinedOrderCandidate> candidates;
        EbayCombinedOrderType combinedOrderType;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public EbayPotentialCombinedOrdersFoundEventArgs(Control owner, Exception error, bool canceled, object userState, EbayCombinedOrderType combinedOrderType, List<EbayCombinedOrderCandidate> candidates) :
            base(error, canceled, userState)
        {
            if (candidates == null)
            {
                throw new ArgumentNullException("candidates");
            }

            this.combinedOrderType = combinedOrderType;
            this.candidates = candidates;
            this.owner = owner;
        }

        /// <summary>
        /// UI owner
        /// </summary>
        public Control Owner
        {
            get { return owner; }
        }

        /// <summary>
        /// The combined orders that were identified
        /// </summary>
        public List<EbayCombinedOrderCandidate> Candidates
        {
            get { return candidates; }
        }

        /// <summary>
        /// The type of order combining operation that was requested
        /// </summary>
        public EbayCombinedOrderType CombinedOrderType
        {
            get { return combinedOrderType; }
        }
    }
}
