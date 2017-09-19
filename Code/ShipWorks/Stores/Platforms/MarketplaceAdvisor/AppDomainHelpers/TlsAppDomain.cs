using System;
using System.Net;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.AppDomainHelpers
{
    /// <summary>
    /// Allow objects to be created on a Tls only app domain
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    public static class TlsAppDomain
    {
        private static readonly AppDomain appDomain;

        /// <summary>
        /// Constructor
        /// </summary>
        static TlsAppDomain()
        {
            appDomain = AppDomain.CreateDomain("TLS AppDomain");

            // Creating this object should set the security to
            Create<SecurityProtocolSettings>();
        }

        /// <summary>
        /// Create an object that will get TLS1 and SSL3 security protocols only
        /// </summary>
        public static T Create<T>(params object[] constructorParameters)
        {
            return (T) appDomain.CreateInstanceAndUnwrap(
                Assembly.GetExecutingAssembly().FullName,
                typeof(T).FullName,
                false,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                null,
                constructorParameters,
                null,
                null);
        }

        /// <summary>
        /// Class that allows the security protocols to be set on the app domain
        /// </summary>
        /// <remarks>This class is only instantiated using reflection, and it should only be instantiated
        /// on a separate app domain.</remarks>
        [Serializable]
        private class SecurityProtocolSettings
        {
            /// <summary>
            /// Constructor
            /// </summary>
            private SecurityProtocolSettings()
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            }
        }
    }
}