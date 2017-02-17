using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.IO;

namespace Interapptive.Shared.Win32
{
    /// <summary>
    /// Helps with access to the registry
    /// </summary>
    public class RegistryHelper
    {
        string basePath = null;

        /// <summary>
        /// Initialize the base registry path from which all access will start
        /// </summary>
        public RegistryHelper(string basePath)
        {
            if (basePath == null)
            {
                throw new ArgumentNullException("basePath");
            }

            this.basePath = basePath;
        }

        /// <summary>
        /// The base registry path that has been initialized
        /// </summary>
        public string BasePath
        {
            get
            {
                return basePath;
            }
        }

        /// <summary>
        /// Open HKEY_CURRENT_USERS\BasePath\subkey
        /// </summary>
        public RegistryKey OpenKey(string subkey)
        {
            string fullKey = string.IsNullOrEmpty(subkey) ? BasePath : Path.Combine(BasePath, subkey);

            return Registry.CurrentUser.OpenSubKey(fullKey);
        }

        /// <summary>
        /// Open or create HKEY_CURRENT_USERS\BasePath\subkey
        /// </summary>
        public RegistryKey CreateKey(string subkey)
        {
            string fullKey = string.IsNullOrEmpty(subkey) ? BasePath : Path.Combine(BasePath, subkey);

            return Registry.CurrentUser.CreateSubKey(fullKey);
        }

        /// <summary>
        /// Get the string value of the given key
        /// </summary>
        public string GetValue(string name, string defaultValue)
        {
            return GetValue((string) null, name, defaultValue);
        }

        /// <summary>
        /// Get the string value of the given key
        /// </summary>
        public string GetValue(string subkey, string name, string defaultValue)
        {
            using (RegistryKey key = OpenKey(subkey))
            {
                if (key == null)
                {
                    return defaultValue;
                }

                return GetValue(key, name, defaultValue);
            }
        }

        /// <summary>
        /// Get the string value of the given key.  
        /// </summary>
        public static string GetValue(RegistryKey key, string name, string defaultValue)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            object value = key.GetValue(name);

            // If it doesn't exist use the default
            if (value == null)
            {
                return defaultValue;
            }

            // If its a string, parse it
            string stringValue = value as string;
            if (stringValue != null)
            {
                return stringValue;
            }

            return value.ToString();
        }

        /// <summary>
        /// Get the int value of the given key
        /// </summary>
        public int GetValue(string name, int defaultValue)
        {
            return GetValue((string) null, name, defaultValue);
        }

        /// <summary>
        /// Get the int value of the given key
        /// </summary>
        public int GetValue(string subkey, string name, int defaultValue)
        {
            using (RegistryKey key = OpenKey(subkey))
            {
                if (key == null)
                {
                    return defaultValue;
                }

                return GetValue(key, name, defaultValue);
            }
        }

        /// <summary>
        /// Get the int value of the given key.  
        /// </summary>
        public static int GetValue(RegistryKey key, string name, int defaultValue)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            object value = key.GetValue(name);

            // If it doesn't exist use the default
            if (value == null)
            {
                return defaultValue;
            }

            // If its a string, parse it
            string stringValue = value as string;
            if (stringValue != null)
            {
                return Convert.ToInt32(stringValue);
            }

            // If its convertible, convert it
            IConvertible convertible = value as IConvertible;
            if (convertible != null)
            {
                return convertible.ToInt32(null);
            }

            throw new InvalidCastException(string.Format("Could not cast '{0}' to type Int32.", value));
        }

        /// <summary>
        /// Get the bool value of the given key
        /// </summary>
        public bool GetValue(string name, bool defaultValue)
        {
            return GetValue((string) null, name, defaultValue);
        }

        /// <summary>
        /// Get the bool value of the given key
        /// </summary>
        public bool GetValue(string subkey, string name, bool defaultValue)
        {
            using (RegistryKey key = OpenKey(subkey))
            {
                if (key == null)
                {
                    return defaultValue;
                }

                return GetValue(key, name, defaultValue);
            }
        }

        /// <summary>
        /// Get the bool value of the given key.  
        /// </summary>
        public static bool GetValue(RegistryKey key, string name, bool defaultValue)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            object value = key.GetValue(name);

            // If it doesn't exist use the default
            if (value == null)
            {
                return defaultValue;
            }

            // If its a string, parse it
            string stringValue = value as string;
            if (stringValue != null)
            {
                return Convert.ToBoolean(stringValue);
            }

            // If its convertible, convert it
            IConvertible convertible = value as IConvertible;
            if (convertible != null)
            {
                return convertible.ToBoolean(null);
            }

            throw new InvalidCastException(string.Format("Could not cast '{0}' to type Boolean.", value));
        }

        /// <summary>
        /// Set the value at the given key with the given name
        /// </summary>
        public void SetValue(string name, object value)
        {
            SetValue(null, name, value);
        }

        /// <summary>
        /// Set the value at the given key with the given name
        /// </summary>
        public void SetValue(string subkey, string name, object value)
        {
            using (RegistryKey key = CreateKey(subkey))
            {
                key.SetValue(name, value);
            }
        }

        /// <summary>
        /// Determines if a given sub key exists 
        /// </summary>
        public static bool RegistrySubKeyExists(RegistryKey key, string subKeyName)
        {
            using (RegistryKey subKey = key.OpenSubKey(subKeyName))
            {
                return subKey != null;
            }
        }
    }
}
