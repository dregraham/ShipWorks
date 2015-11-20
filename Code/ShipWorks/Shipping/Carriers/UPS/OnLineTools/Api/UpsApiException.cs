using System;
using System.Runtime.Serialization;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api
{
    /// <summary>
    /// Thrown when a UPS API response has an error
    /// </summary>
    [Serializable]
    public class UpsApiException : UpsException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsApiException(UpsApiResponseStatus status, string errorCode, string description)
            : this (status, errorCode, description, "")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsApiException(UpsApiResponseStatus status, string errorCode, string description, string errorLocation)
            : base(string.IsNullOrEmpty(description) ? "UPS Error: " + errorCode : description)
        {
            Status = status;
            ErrorCode = errorCode;
            Description = description;
            ErrorLocation = errorLocation;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected UpsApiException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext)
        {

        }

        /// <summary>
        /// Status returned by UPS
        /// </summary>
        public UpsApiResponseStatus Status { get; }

        /// <summary>
        /// Error code, specific to the on line tool
        /// </summary>
        public override string ErrorCode { get; }

        /// <summary>
        /// Optional description
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Node location of the bad request element.
        /// </summary>
        public string ErrorLocation { get; }
    }
}
