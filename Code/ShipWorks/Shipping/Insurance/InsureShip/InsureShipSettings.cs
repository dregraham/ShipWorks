using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// Class to get InsureShip settings.
    /// </summary>
    [Component]
    public class InsureShipSettings : IInsureShipSettings
    {
        private const string salt = "InsureShip7458";

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
        public string ClientID =>
            UseTestServer ?
                SecureText.Decrypt("tc7+yEwpdGE=", salt) :
                SecureText.Decrypt("tc7+yEwpdGE=", salt);

        /// <summary>
        /// InsureShip Password
        /// </summary>
        public string ApiKey =>
            UseTestServer ?
                SecureText.Decrypt("18VbfzIHoxT2lVYwuCXLF+FXVzokfhjKhUrq3Y2AEoO6tCcznbDtttovCAohTkbaXkIXE/YviEtMnwvDmlGzmfj1XWqNs0WO2WBb+rWfU88MhzxwRAug8ifivvVPpwylJn/VXV+boehh4dmjv+e8VPpEd5Td3hTkF0P7212c6ooGcRYaJYjKIQ==", salt) :
                SecureText.Decrypt("18VbfzIHoxT2lVYwuCXLF+FXVzokfhjKhUrq3Y2AEoO6tCcznbDtttovCAohTkbaXkIXE/YviEtMnwvDmlGzmfj1XWqNs0WO2WBb+rWfU88MhzxwRAug8ifivvVPpwylJn/VXV+boehh4dmjv+e8VPpEd5Td3hTkF0P7212c6ooGcRYaJYjKIQ==", salt);

        /// <summary>
        /// InsureShip Url
        /// </summary>
        public Uri ApiUrl => CertificateUrl;

        /// <summary>
        /// Gets the URL to use when inspecting the certificate data for authenticity.
        /// </summary>
        public Uri CertificateUrl => new Uri("https://api.insureship.com");

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
        public static TimeSpan VoidPolicyMaximumAge => TimeSpan.FromHours(24);

        /// <summary>
        /// Gets the phone number that should be used for a customer to contact InsureShip.
        /// </summary>
        public string InsureShipPhoneNumber
        {
            get { return "1-866-701-3654"; }
        }
    }
}
