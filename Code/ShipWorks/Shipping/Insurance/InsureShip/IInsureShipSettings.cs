using System;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// Interface for InsureShip settings.
    /// </summary>
    public interface IInsureShipSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether [use test server] based on a registry setting.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use test server]; otherwise, <c>false</c>.
        /// </value>
        bool UseTestServer { get; set; }

        /// <summary>
        /// InsureShip Distributor ID
        /// </summary>
        string DistributorID { get; }

        /// <summary>
        /// InsureShip Username
        /// </summary>
        string Username { get; }

        /// <summary>
        /// InsureShip Password
        /// </summary>
        string Password { get; }

        /// <summary>
        /// InsureShip Url
        /// </summary>
        Uri ApiUrl { get; }

        /// <summary>
        /// Gets the URL to use when inspecting the certificate data for authenticity.
        /// </summary>
        Uri CertificateUrl { get; }

        /// <summary>
        /// Gets the amount of time after a shipment has been processed before a claim can be submitted.
        /// </summary>
        TimeSpan ClaimSubmissionWaitingPeriod { get; }

        /// <summary>
        /// Gets the maximum age of a policy that is allowed to be voided.
        /// </summary>
        TimeSpan VoidPolicyMaximumAge { get; }
    }
}