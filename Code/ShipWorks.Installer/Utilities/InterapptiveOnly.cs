namespace ShipWorks.Installer.Utilities
{
    /// <summary>
    /// Used to control features and functions that should only be available to Interapptive employees.
    /// </summary>
    public static class InterapptiveOnly
    {
        private static readonly RegistryHelper internalRegistry = new RegistryHelper(@"Software\Interapptive\ShipWorks\Internal");

        /// <summary>
        /// Special section of the Registry for "Internal" Interapptive-Only scoped settings
        /// </summary>
        public static RegistryHelper Registry
        {
            get { return internalRegistry; }
        }

        /// <summary>
        /// Determines if the person currently using ShipWorks is an Interapptive user, and has rights to see
        /// what only Interapptive employees should see.
        /// </summary>
        public static bool IsInterapptiveUser
        {
            get
            {
                return Registry.GetValue("Private", false);
            }
        }

        /// <summary>
        /// Indicates if multiple instances of the application should be allowed to run at the same time
        /// </summary>
        public static bool AllowMultipleInstances
        {
            get
            {
                return Registry.GetValue("AllowMultipleInstances", false);
            }
            set
            {
                Registry.SetValue("AllowMultipleInstances", value);
            }
        }
    }
}
