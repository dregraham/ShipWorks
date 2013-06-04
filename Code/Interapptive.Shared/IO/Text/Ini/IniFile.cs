using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using Interapptive.Shared.Win32;

namespace Interapptive.Shared.IO.Text.Ini
{
    /// <summary>
    /// Create a New INI file to store or load data
    /// </summary>
    public class IniFile
    {
        private string iniPath;

        /// <summary>
        /// IniFile Constructor.
        /// </summary>
        public IniFile(string iniPath)
        {
            this.iniPath = iniPath;
        }

        /// <summary>
        /// Write Data to the INI File
        /// The INI file will be created if it does not exist.
        /// </summary>
        /// <param name="section">The ini file section in which to write</param>
        /// <param name="key">The key to write</param>
        /// <param name="value">The value to write</param>
        /// <exception cref="IniFileException" />
        public void WriteValue(string section, string key, string value)
        {
            int result = NativeMethods.WritePrivateProfileString(section, key, value, iniPath);
            if (result == 0)
            {
                int lastWin32Error = Marshal.GetLastWin32Error();
                if (lastWin32Error != 0)
                {
                    throw new IniFileException(string.Format(CultureInfo.InvariantCulture, "Unable to write to ini file '{0}'.  Error code: '{1}'", iniPath, lastWin32Error));
                }
                else
                {
                    throw new IniFileException(string.Format(CultureInfo.InvariantCulture, "Unable to write to ini file '{0}'.", iniPath));
                }
            }
        }

        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        /// <param name="section">The ini file section in which to write</param>
        /// <param name="key">The key to write</param>
        /// <returns>The value of the key in the section, if it exists.  If it does not exist, string.Empty is returned.</returns>
        /// <exception cref="IniFileException" />
        public string ReadValue(string section, string key)
        {
            StringBuilder returnValue = new StringBuilder(255);

            // Read the value
            int result = NativeMethods.GetPrivateProfileString(section, key, "", returnValue, 255, iniPath);

            // result is the number of characters read, so if it is 0, there could have been an error.
            if (result == 0)
            {
                // Get the last Win32 Error
                int lastWin32Error = Marshal.GetLastWin32Error();

                // If it's not 0, there was an error, so throw.  If it is 0, then result was 0 because there was no value to read but no error either.
                // Have noticed that if the key is not found, error code 2 can be returned.
                if (lastWin32Error != 0 && lastWin32Error != 2)
                {
                    throw new IniFileException(string.Format(CultureInfo.InvariantCulture, "Unable to read ini file '{0}'.  Error code: '{1}'", iniPath, lastWin32Error));
                }
            }

            return returnValue.ToString();
        }

        /// <summary>
        /// Helper method to write to the ini file IF the setting value is empty
        /// </summary>
        /// <param name="section">The ini section where the key is to be found</param>
        /// <param name="key">The key in the section to check/write value to</param>
        /// <param name="value">The value to set key equal to</param>
        public void WriteIniValueIfMissing(string section, string key, string value)
        {
            // Read the value of the key
            string iniValue = ReadValue(section, key);

            // If the value is null/blank, write value to it.
            if (string.IsNullOrWhiteSpace(iniValue))
            {
                WriteValue(section, key, value);
            }
        }
    }
}
