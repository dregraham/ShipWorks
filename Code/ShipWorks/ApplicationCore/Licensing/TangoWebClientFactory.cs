﻿using System;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing
{

    /// <summary>
    /// A factory for creating ITangoWebClient instances. This is useful for testing/simulating responses
    /// from Tango before they have been implemented on the Tango side. To safeguard against a custom Tango
    /// web client from being introduced in the wild, a user must be an Interapptive user and also have a
    /// registry value that contains the resolves to a valid type that implements the ITangoWebClient interface.
    /// </summary>
    public class TangoWebClientFactory : ITangoWebClientFactory
    {
        //private const string CustomizedTangoRegistryKeyName = "TangoWebClient";
        private readonly Func<TangoWebRequestClient> createWebRequestClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public TangoWebClientFactory(Func<TangoWebRequestClient> createWebRequestClient)
        {
            this.createWebRequestClient = createWebRequestClient;
        }

        /// <summary>
        /// Creates an instance of ITangoWebClient. This allows us to return a mock/fake instance of an
        /// ITangoWebClient for Interapptive users that have the appropriate registry setting configured;
        /// otherwise the normal web client will be returned.
        /// </summary>
        public ITangoWebClient CreateWebClient()
        {
            if (InterapptiveOnly.IsInterapptiveUser)
            {
                // Check to see if the TangoWebClient key exists and try to use the
                // web client indicated by the key
                string webClientTypeName = InterapptiveOnly.Registry.GetValue("TangoWebClient", string.Empty);
                if (!string.IsNullOrWhiteSpace(webClientTypeName))
                {
                    // We have an entry for the custom tango client
                    Type type = Type.GetType(webClientTypeName);

                    if (type != null)
                    {
                        return Activator.CreateInstance(type) as ITangoWebClient;
                    }
                }
            }

            // Fall back to the TangoWebClientWrapper in case we tried to create a customized web client
            // that could not be resolved by the Activator
            return new TangoWebClientWrapper();
        }

        /// <summary>
        /// Creates an instance of ITangoWebClient. This allows us to return a mock/fake instance of an
        /// ITangoWebClient for Interapptive users that have the appropriate registry setting configured;
        /// otherwise the normal web client will be returned.
        /// </summary>
        public ITangoWebRequestClient CreateWebRequestClient() =>
            GetFakeClient<ITangoWebRequestClient>("TangoWebRequestClient").OrElse(createWebRequestClient);

        private Maybe<T> GetFakeClient<T>(string key) where T : class
        {
            if (InterapptiveOnly.IsInterapptiveUser)
            {
                // Check to see if the TangoWebClient key exists and try to use the
                // web client indicated by the key
                string webClientTypeName = InterapptiveOnly.Registry.GetValue(key, string.Empty);
                if (!string.IsNullOrWhiteSpace(webClientTypeName))
                {
                    // We have an entry for the custom tango client
                    Type type = Type.GetType(webClientTypeName);

                    if (type != null)
                    {
                        return Activator.CreateInstance(type) as T;
                    }
                }
            }

            return Maybe.Empty<T>();
        }
    }
}
