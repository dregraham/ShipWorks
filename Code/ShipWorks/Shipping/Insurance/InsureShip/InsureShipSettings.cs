using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// Class to get InsureShip settings.
    /// </summary>
    [Component]
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
