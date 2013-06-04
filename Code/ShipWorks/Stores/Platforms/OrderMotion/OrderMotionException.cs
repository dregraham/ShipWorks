using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Platforms.OrderMotion
{
    /// <summary>
    /// Exception class for OrderMotion errors
    /// </summary>
    [Serializable]
    public class OrderMotionException : Exception
    {
         /// <summary>
        /// Constructor
        /// </summary>
        public OrderMotionException(string message, Exception inner)
            : base(message, inner)
        {
        }


        /// <summary>
        /// Constructor for just message
        /// </summary>
        public OrderMotionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected OrderMotionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Deserilization 
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
