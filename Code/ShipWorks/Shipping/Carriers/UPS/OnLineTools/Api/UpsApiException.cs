using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api
{
    /// <summary>
    /// Thrown when a UPS API response has an error
    /// </summary>
    public class UpsApiException : UpsException
    {
        readonly UpsApiResponseStatus status;
        readonly string errorCode;
        readonly string description;
        readonly string errorLocation;

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
            this.status = status;
            this.errorCode = errorCode;
            this.description = description;
            this.errorLocation = errorLocation;
        }

        /// <summary>
        /// Status returned by UPS
        /// </summary>
        public UpsApiResponseStatus Status
        {
            get { return status; }
        }

        /// <summary>
        /// Error code, specific to the online tool
        /// </summary>
        public override string ErrorCode
        {
            get { return errorCode; }
        }

        /// <summary>
        /// Optional description
        /// </summary>
        public string Description
        {
            get { return description; }
        }

        /// <summary>
        /// Node location of the bad request elmeent.
        /// </summary>
        public string ErrorLocation
        {
            get { return errorLocation; }

        }
    }
}
