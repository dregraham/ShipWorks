using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// CLR/.Net version detection from
    /// https://msdn.microsoft.com/en-us/library/hh925568(v=vs.110).aspx#net_a
    /// </summary>
    public class ClrHelper : IClrHelper
    {
        private const string NetVersionRegistryKey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\";
        private readonly List<Version> clrVersions = new List<Version>();

        /// <summary>
        /// Constructor
        /// </summary>
        public ClrHelper()
        {
            GetVersionFromRegistry(NetVersionRegistryKey);
        }

        /// <summary>
        /// The installed CLR Versions
        /// </summary>
        public IEnumerable<Version> ClrVersions => clrVersions.Distinct();

        /// <summary>
        /// Reloads the current list so that ClrVersions will be repopulated.
        /// </summary>
        public void Reload()
        {
            clrVersions.Clear();

            GetVersionFromRegistry(NetVersionRegistryKey);
        }

        /// <summary>
        /// Populate the list of CLR versions
        /// </summary>
        private void GetVersionFromRegistry(string keyName)
        {
            // Opens the registry key for the .NET Framework entry.
            using (RegistryKey ndpKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").OpenSubKey(keyName))
            {
                // As an alternative, if you know the computers you will query are running .NET Framework 4.5 
                // or later, you can use:
                // using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, 
                // RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
                foreach (string versionKeyName in ndpKey.GetSubKeyNames())
                {
                    if (versionKeyName.StartsWith("v") || 
                        versionKeyName.StartsWith("Client") ||
                        versionKeyName.StartsWith("Full"))
                    {
                        RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);

                        string releaseString = versionKey?.GetValue("Release", "").ToString();
                        int release;

                        if (!string.IsNullOrWhiteSpace(releaseString) && Int32.TryParse(releaseString, out release))
                        {
                            clrVersions.Add(CheckFor45PlusVersion(release));
                        }
                        else
                        {
                            string versionString = versionKey?.GetValue("Version", "").ToString();
                            Version version;

                            if (!Version.TryParse(versionString, out version))
                            {
                                GetVersionFromRegistry($"{keyName}{versionKeyName}\\");
                            }
                            else
                            {
                                clrVersions.Add(version);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checking the version using >= will enable forward compatibility.
        /// </summary>
        private Version CheckFor45PlusVersion(int releaseKey)
        {
            if (releaseKey >= 394802)
            {
                return new Version(4, 6, 2, 0); //"4.6.2 or later";
            }
            if (releaseKey >= 394254)
            {
                return new Version(4, 6, 1, 0); //"4.6.1";
            }
            if (releaseKey >= 393295)
            {
                return new Version(4, 6, 0, 0); //"4.6";
            }
            if ((releaseKey >= 379893))
            {
                return new Version(4, 5, 2, 0); //"4.5.2";
            }
            if ((releaseKey >= 378675))
            {
                return new Version(4, 5, 1, 0); //"4.5.1";
            }
            if ((releaseKey >= 378389))
            {
                return new Version(4, 5, 0, 0); //"4.5";
            }

            // This code should never execute. A non-null release key should mean
            // that 4.5 or later is installed.  So we'll return 4.6.2.
            return new Version(4, 6, 2, 0);
        }
    }
}
