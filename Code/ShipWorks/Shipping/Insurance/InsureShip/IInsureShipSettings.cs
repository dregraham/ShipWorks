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
        Uri Url { get; }
    }
}