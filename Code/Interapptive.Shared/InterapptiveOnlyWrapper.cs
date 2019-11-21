using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Win32;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Used to control features and functions that should only be available to Interapptive employees.
    /// </summary>
    [Component]
    public class InterapptiveOnlyWrapper : IInterapptiveOnly
    {
        /// <summary>
        /// Special section of the Registry for "Internal" Interapptive-Only scoped settings
        /// </summary>
        public RegistryHelper Registry => InterapptiveOnly.Registry;

        /// <summary>
        /// Determines if the person currently using ShipWorks is an Interapptive user, and has rights to see
        /// what only Interapptive employees should see.
        /// </summary>
        public bool IsInterapptiveUser => InterapptiveOnly.IsInterapptiveUser;

        /// <summary>
        /// Indicates if multiple instances of the application should be allowed to run at the same time
        /// </summary>
        public bool AllowMultipleInstances
        {
            get => InterapptiveOnly.AllowMultipleInstances;
            set => InterapptiveOnly.AllowMultipleInstances = value;
        }

        /// <summary>
        /// Determines if a special key combination is active.  Can be used
        /// for enabling "hidden" (but not secure!) functionality.
        /// </summary>
        public bool MagicKeysDown => InterapptiveOnly.MagicKeysDown;

        /// <summary>
        /// Determines if the given store registry key is set to use fake stores
        /// </summary>
        public bool UseFakeAPI(string registryKey) =>
            InterapptiveOnly.IsInterapptiveUser && !InterapptiveOnly.Registry.GetValue(registryKey, true);
    }
}
