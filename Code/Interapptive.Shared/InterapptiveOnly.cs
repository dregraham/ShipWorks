using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using Interapptive.Shared;
using Interapptive.Shared.Win32;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Used to control features and functions that should only be available to Interapptive employees.
    /// </summary>
    public static class InterapptiveOnly
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

        /// <summary>
        /// Determines if a special key combination is active.  Can be used
        /// for enabling "hidden" (but not secure!) functionality.
        /// </summary>
        public static bool MagicKeysDown
        {
            get
            {
                return Control.ModifierKeys == (Keys.Control | Keys.Shift) &&
                    (NativeMethods.GetAsyncKeyState(Keys.LWin) & 0x8000) != 0;
            }
        }
    }
}
