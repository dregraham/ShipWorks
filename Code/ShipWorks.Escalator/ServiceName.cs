using System;
using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Service Name resolver
    /// </summary>
    public static class ServiceName
    {
        /// <summary>
        /// Resolves the service name
        /// </summary>
        public static string Resolve() => $"ShipWorksUpdater - {GetInstanceID()}";

        /// <summary>
        /// Gets the instance id of the service based on registry value. Throws if it cannot find the registry value.
        /// </summary>
        /// <returns></returns>
        private static Guid GetInstanceID()
        {
            return GetRegistryLocalMachineValue(@"Software\Interapptive\ShipWorks\Instances",
             Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
            "ShipWorks could not load the InstanceID.\n\n" +
            "To fix this problem:\n" +
                "   (1)  Reinstall the application.\n" +
                "   (2)  For further support, contact Interapptive.");
        }

        /// <summary>
        /// Queries Registry.LocalMachine for a Guid to return.  
        /// </summary>
        /// <param name="subKeyPath">The path to the key starting from registryBaseKey.</param>
        /// <param name="keyName">The name of the key to parse into a Guid.</param>
        /// <param name="errorMessage">If the guild cannot be found, this is the error message to be returned.</param>
        /// <returns>If the key is found and it's value is a valid guid, the guid is returned.  Otherwise an InstallationException is thrown with errorMessage.</returns>
        private static Guid GetRegistryLocalMachineValue(string subKeyPath, string keyName, string errorMessage)
        {
            Guid guid = GetRegistryValue(Registry.LocalMachine, subKeyPath, keyName);

            if (guid == Guid.Empty)
            {
                if (Is64BitWindows && !Is64BitProcess)
                {
                    // This is for integration tests on 64 bit machines.  Try to open the Registry64 view.  If it throws,
                    // Just let the code flow to the InstallationException below.
                    using (RegistryKey registryBaseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                    {
                        guid = GetRegistryValue(registryBaseKey, subKeyPath, keyName);
                    }
                }

                if (guid == Guid.Empty)
                {
                    throw new InvalidOperationException(errorMessage);
                }
            }

            return guid;
        }


        /// <summary>
        /// Queries the registry for a Guid to return.  
        /// </summary>
        /// <param name="registryBaseKey">The base key from which to start.</param>
        /// <param name="subKeyPath">The path to the key starting from registryBaseKey.</param>
        /// <param name="keyName">The name of the key to parse into a Guid.</param>
        /// <returns>If the key is found and it's value is a valid guid, the guid is returned.  Otherwise an empty guid is returned.</returns>
        private static Guid GetRegistryValue(RegistryKey registryBaseKey, string subKeyPath, string keyName)
        {
            using (RegistryKey key = registryBaseKey.OpenSubKey(subKeyPath))
            {
                if (key != null)
                {
                    string value = key.GetValue(keyName) as string;

                    if (value != null)
                    {
                        Guid guid;
                        if (Guid.TryParse(value, out guid))
                        {
                            return guid;
                        }
                    }
                }
            }

            return Guid.Empty;
        }

        /// <summary>
        /// Indicates if the running version of windows is 64bit.  This works regardless of if we
        /// are running as 64bit or as 32bit Wow64.
        /// </summary>
        private static bool Is64BitWindows => Environment.Is64BitOperatingSystem;

        /// <summary>
        /// Indicates if the current process is loaded as a 64bit process.
        /// </summary>
        private static bool Is64BitProcess => Environment.Is64BitProcess;
    }
}