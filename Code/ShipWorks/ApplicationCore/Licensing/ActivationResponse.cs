using System;
using System.Xml;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.Activation;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Contains customer activation response
    /// </summary>
    public class ActivationResponse : IActivationResponse
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ActivationResponse(CustomerLicenseInfoV1 response)
        {
            MethodConditions.EnsureArgumentIsNotNull(response, nameof(response));

            Key = response.CustomerLicenseKey;
            AssociatedStampsUserName = response.AssociatedStampsUserName;
        }

        /// <summary>
        /// The customer Key
        /// </summary>
        public string Key { get;  }

        /// <summary>
        /// The associated stamps username. If empty, do not create new Stamps account in ShipWorks
        /// </summary>
        public string AssociatedStampsUserName { get; }
    }
}