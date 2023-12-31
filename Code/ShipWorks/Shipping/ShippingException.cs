﻿using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Thrown when a known error occurs during shipping
    /// </summary>
    [Description("ShippingException")]
    public class ShippingException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingException(Exception inner)
            : base(inner.Message, inner)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingException(string message, Exception inner)
            : base(message, inner)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected ShippingException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext)
        {

        }
    }
}
