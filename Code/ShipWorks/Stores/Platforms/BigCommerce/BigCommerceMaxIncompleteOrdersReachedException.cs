using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// BigCommerce specific exception that occurs when downloading orders and the current page of orders all have
    /// the status of InComplete.  
    /// </summary>
    [Serializable]
    public class BigCommerceMaxIncompleteOrdersReachedException : BigCommerceException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceMaxIncompleteOrdersReachedException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceMaxIncompleteOrdersReachedException(string message) 
            : base(message)
        {

        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected BigCommerceMaxIncompleteOrdersReachedException(SerializationInfo serializationInfo, StreamingContext streamingContext) : 
            base(serializationInfo, streamingContext)
        { }

        /// <summary>
        /// Override to implement GetObjectData for serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
