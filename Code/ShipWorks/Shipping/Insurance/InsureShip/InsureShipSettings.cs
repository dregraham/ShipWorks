using System;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// Class to get InsureShip settings.
    /// </summary>
    public class InsureShipSettings : IInsureShipSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether [use test server] based on a registry setting.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use test server]; otherwise, <c>false</c>.
        /// </value>
        public bool UseTestServer
        {
            get
            {
                return InterapptiveOnly.Registry.GetValue("InsureShipTestServer", false);
            }
            set
            {
                InterapptiveOnly.Registry.SetValue("InsureShipTestServer", value);
            }
        }

        /// <summary>
        /// InsureShip Distributor ID
        /// </summary>
        public string DistributorID
        {
            get
            {
                return "D00050";
            }
        }

        /// <summary>
        /// InsureShip Username
        /// </summary>
        public string Username
        {
            get
            {
                return SecureText.Decrypt("xYGNUSctosMN3kr2vZw1cg==", "InsureShip7458");
            }
        }

        /// <summary>
        /// InsureShip Password
        /// </summary>
        public string Password
        {
            get
            {
                return UseTestServer ? SecureText.Decrypt("xYGNUSctosMiU8bYJtpOOA==", "InsureShip7458") : SecureText.Decrypt("byJ2OXi5odKK0PVy7VPB6zKGm6aI++SX", "InsureShip7458");
            }
        }

        /// <summary>
        /// InsureShip Url
        /// </summary>
        public Uri ApiUrl
        {
            get { return new Uri(CertificateUrl.AbsoluteUri + "api/"); }
        }

        /// <summary>
        /// Gets the URL to use when inspecting the certificate data for authenticity.
        /// </summary>
        public Uri CertificateUrl
        {
            get
            {
                return UseTestServer ? new Uri("https://int.insureship.com") : new Uri("https://api2.insureship.com");
            }
        }

        /// <summary>
        /// Gets the amount of time after a shipment has been processed before a claim can be submitted.
        ///
        /// Technically, it is 7 days, but we don't know when the shipment was shipped during the day.
        /// </summary>
        public TimeSpan ClaimSubmissionWaitingPeriod
        {
            get { return TimeSpan.FromDays(8); }
        }

        /// <summary>
        /// Gets the maximum age of a policy that is allowed to be voided.
        /// </summary>
        public TimeSpan VoidPolicyMaximumAge
        {
            get
            {
                return TimeSpan.FromHours(24);
            }
        }

        /// <summary>
        /// Gets the phone number that should be used for a customer to contact InsureShip.
        /// </summary>
        public string InsureShipPhoneNumber
        {
            get { return "1-866-701-3654"; }
        }
    }
}
