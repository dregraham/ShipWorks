using Interapptive.Shared.Win32;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Used to control features and functions that should only be available to Interapptive employees.
    /// </summary>
    public interface IInterapptiveOnly
    {
        /// <summary>
        /// Special section of the Registry for "Internal" Interapptive-Only scoped settings
        /// </summary>
        RegistryHelper Registry { get; }

        /// <summary>
        /// Determines if the person currently using ShipWorks is an Interapptive user, and has rights to see
        /// what only Interapptive employees should see.
        /// </summary>
        bool IsInterapptiveUser { get; }

        /// <summary>
        /// Indicates if multiple instances of the application should be allowed to run at the same time
        /// </summary>
        bool AllowMultipleInstances { get; set; }

        /// <summary>
        /// Determines if a special key combination is active.  Can be used
        /// for enabling "hidden" (but not secure!) functionality.
        /// </summary>
        bool MagicKeysDown { get; }

        /// <summary>
        /// Determines if the given store registry key is set to use fake stores
        /// </summary>
        bool UseFakeAPI(string registryKey);
    }
}