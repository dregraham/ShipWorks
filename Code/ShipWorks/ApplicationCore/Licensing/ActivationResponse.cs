﻿using System;
using System.Xml;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.Activation.WebServices;

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
            LegacyKey = response.LegacyCustomerLicenseKey;
            IsLegacyUser = response.IsLegacyUser;
            AssociatedStampsUsername = response.AssociatedStampsUserName;
            StampsUsername = response.StampsUserName;
        }

        /// <summary>
        /// The customer Key
        /// </summary>
        public string Key { get;  }
        
        /// <summary>
        /// The customer Key for legacy users
        /// </summary>
        public string LegacyKey { get;  }

        /// <summary>
        /// Whether or not this is a legacy account
        /// </summary>
        public bool IsLegacyUser { get; }
        
        /// <summary>
        /// The associated stamps username. If empty, do not create new Stamps account in ShipWorks
        /// </summary>
        public string AssociatedStampsUsername { get; }

        /// <summary>
        /// The stamps username to use when creating the first stamps account.
        /// </summary>
        public string StampsUsername { get; set; }
    }
}