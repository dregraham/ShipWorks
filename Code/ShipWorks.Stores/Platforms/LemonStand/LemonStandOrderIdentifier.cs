using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.LemonStand
{
    /// <summary>
    ///     Uniquely identifies an LemonStand Store order in the database
    /// </summary>
    public class LemonStandOrderIdentifier : OrderIdentifier
    {
        // LemonStandStore's Order ID
        private readonly string lemonStandStoreOrderID;

        /// <summary>
        ///     Constructor
        /// </summary>
        public LemonStandOrderIdentifier(string lemonStandStoreOrderID)
        {
            this.lemonStandStoreOrderID = lemonStandStoreOrderID;
        }

        /// <summary>
        ///     Apply the order number to the order provided
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            LemonStandOrderEntity lemonStandStoreOrder = order as LemonStandOrderEntity;

            if (lemonStandStoreOrder == null)
            {
                throw new InvalidOperationException(
                    "A non LemonStandStore order was passed to the LemonStandStore order identifier.");
            }

            lemonStandStoreOrder.LemonStandOrderID = lemonStandStoreOrderID;
        }

        /// <summary>
        ///     Apply the order number to the download log entity
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            if (downloadDetail == null)
            {
                throw new ArgumentNullException("downloadDetail");
            }
            downloadDetail.ExtraStringData1 = lemonStandStoreOrderID;
        }

        /// <summary>
        ///     String representation
        /// </summary>
        public override string ToString()
        {
            return string.Format("LemonStandStoreOrderID:{0}", lemonStandStoreOrderID);
        }
    }
}