﻿using System;
using System.Collections.Generic;
using System.Net;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Platforms.GenericModule.LegacyAdapter;

namespace ShipWorks.Stores.Platforms.SearchFit
{
    /// <summary>
    /// Search fit store
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.SearchFit)]
    [Component(RegistrationType.Self)]
    public class SearchFitStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SearchFitStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager) :
            base(store, messageHelper, orderManager)
        {

        }

        /// <summary>
        /// Directs SearchFit to leave orders on the server
        /// </summary>
        public static bool LeaveOnServer
        {
            get { return InterapptiveOnly.Registry.GetValue("SearchFitLeaveOnServer", false); }
            set { InterapptiveOnly.Registry.SetValue("SearchFitLeaveOnServer", value); }
        }

        /// <summary>
        /// Identifying typecode for SearchFit
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.SearchFit;

        /// <summary>
        /// Log source
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.SearchFit;

        /// <summary>
        /// Get the required module version
        /// </summary>
        public override Version GetRequiredModuleVersion()
        {
            return new Version("2.9.42");
        }

        /// <summary>
        /// Create a legacy-capable web client that can communicate with X-Cart derivative
        /// SearchFit
        /// </summary>
        public override IGenericStoreWebClient CreateWebClient()
        {
            // register variable transforming and renaming
            Dictionary<string, VariableTransformer> transformers = new Dictionary<string, VariableTransformer>
            {
                {"order", new VariableTransformer("orderid")}
            };

            // get custom store capabilities
            GenericModuleCapabilities capabilities = LegacyAdapterCapabilities.CreateXCartDerivativeDefaults();

            LegacyAdapterStoreWebClient client = new LegacyAdapterStoreWebClient((GenericModuleStoreEntity) Store, LegacyStyleSheets.XCartStyleSheet, capabilities, transformers);

            // SearchFit has a feature to allow us to not delete a customer's pending orders during support
            if (LeaveOnServer)
            {
                client.AdditionalVariables.Add("LeaveOnServer", "1");
            }

            // SearchFit reports ServiceUnavailable when parameters are missing
            client.VersionProbeCompatibilityIndicator = HttpStatusCode.ServiceUnavailable;

            return client;
        }

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl => "https://shipworks.zendesk.com/hc/en-us/articles/360022654051";
    }
}
