using System;
using NAudio.Wave.SampleProviders;

namespace ShipWorks.Shipping.Carriers.Amazon.Api
{
    /// <summary>
    /// Result from validating Amazon credentials
    /// </summary>
    public struct AmazonValidateCredentialsResponse : IEquatable<AmazonValidateCredentialsResponse>
    {
        private static readonly AmazonValidateCredentialsResponse successfulResponse = new AmazonValidateCredentialsResponse(true, string.Empty);

        private readonly bool success;
        private readonly string message;

        /// <summary>
        /// Constructor
        /// </summary>
        private AmazonValidateCredentialsResponse(bool success, string message)
        {
            this.success = success;
            this.message = message ?? string.Empty;
        }

        /// <summary>
        /// Was validation successful
        /// </summary>
        public bool Success
        {
            get { return success; }
        }

        /// <summary>
        /// Message from validation
        /// </summary>
        public string Message
        {
            get { return message; }
        }

        /// <summary>
        /// Test for equality
        /// </summary>
        public bool Equals(AmazonValidateCredentialsResponse other)
        {
            return success == other.success && message == other.message;
        }

        /// <summary>
        /// Test for equality
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is AmazonValidateCredentialsResponse && Equals((AmazonValidateCredentialsResponse) obj);
        }

        /// <summary>
        /// Get the hashcode for the object
        /// </summary>
        public override int GetHashCode()
        {
            return success.GetHashCode() ^ Message.GetHashCode();
        }

        /// <summary>
        /// Are objects unequal
        /// </summary>
        public static bool operator !=(AmazonValidateCredentialsResponse left, AmazonValidateCredentialsResponse right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Are ojects equal
        /// </summary>
        public static bool operator ==(AmazonValidateCredentialsResponse left, AmazonValidateCredentialsResponse right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Get a successful response
        /// </summary>
        public static AmazonValidateCredentialsResponse Succeeded()
        {
            return successfulResponse;
        }

        /// <summary>
        /// Get a failed response
        /// </summary>
        public static AmazonValidateCredentialsResponse Failed(string message)
        {
            return new AmazonValidateCredentialsResponse(false, message);
        }
    }
}
