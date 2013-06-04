using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data
{
    /// <summary>
    /// Provides header information (StoreID, IsManual) about a single Order
    /// </summary>
    public class OrderHeader
    {
        long orderID;
        long storeID;
        bool isManual;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderHeader(long orderID, long storeID, bool isManual)
        {
            this.orderID = orderID;
            this.storeID = storeID;
            this.isManual = isManual;
        }

        /// <summary>
        /// The OrderID
        /// </summary>
        public long OrderID
        {
            get { return orderID; }
        }

        /// <summary>
        /// The StoreID
        /// </summary>
        public long StoreID
        {
            get { return storeID; }
        }

        /// <summary>
        /// If its manual
        /// </summary>
        public bool IsManual
        {
            get { return isManual; }
        }
    }
}
