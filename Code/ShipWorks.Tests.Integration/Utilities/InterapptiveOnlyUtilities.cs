using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Win32;

namespace ShipWorks.Tests.Integration.MSTest.Utilities
{
    /// <summary>
    /// Used to control features and functions that should only be available to Interapptive employees.
    /// </summary>
    static class InterapptiveOnlyUtilities
    {
        static RegistryHelper internalRegistry = new RegistryHelper(@"Software\Interapptive\ShipWorks\Internal");

        /// <summary>
        /// Special section of the Registry for "Internal" Interapptive-Only scoped settings
        /// </summary>
        public static RegistryHelper Registry
        {
            get { return internalRegistry; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to [use list rates] based on a registry setting. Indicates if LIST rates are in
        /// effect, instead of the standard ACCOUNT rates
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use list rates]; otherwise, <c>false</c>.
        /// </value>
        public static bool UseListRates
        {
            get { return Registry.GetValue("FedExListRates", false); }
            set { Registry.SetValue("FedExListRates", value); }
        }

    }
}
