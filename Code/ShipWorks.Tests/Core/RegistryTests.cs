using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using System.IO;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32;

namespace ShipWorks.Tests.Core
{
    [TestClass]
    public class RegistryTests
    {
        RegistryHelper registry = new RegistryHelper(@"Software\ShipWorks\UnitTests");

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadIntWithNullKey()
        {
            RegistryHelper.GetValue((RegistryKey) null, null, 0);
        }

        [TestMethod]
        public void ReadIntWithNonexistent()
        {
            int defaultValue = 10;

            using (RegistryKey key = registry.CreateKey("Testing"))
            {
                int read = RegistryHelper.GetValue(key, "Missing", defaultValue);
                Assert.AreEqual(defaultValue, read);
            }
        }

        [TestMethod]
        public void ReadIntFromStringFromPath()
        {
            int value = 10;
            string keyName = "StringValue";

            using (RegistryKey key = registry.CreateKey("Testing"))
            {
                key.SetValue(keyName, value.ToString());

                // Read it back
                int read = registry.GetValue("Testing", keyName, 0);

                // Cleanup
                key.DeleteValue(keyName);

                Assert.AreEqual(value, read);
            }
        }

        [TestMethod]
        public void ReadIntFromString()
        {
            int value = 10;
            string keyName = "StringValue";

            using (RegistryKey key = registry.CreateKey("Testing"))
            {
                key.SetValue(keyName, value.ToString());

                // Read it back
                int read = RegistryHelper.GetValue(key, keyName, 0);

                // Cleanup
                key.DeleteValue(keyName);

                Assert.AreEqual(value, read);
            }
        }

        [TestMethod]
        public void ReadIntFromNonPath()
        {
            int value = 103;
            string keyName = "StringValue";

            // Read it back
            int read = registry.GetValue("DoesNotExist", keyName, value);

            Assert.AreEqual(value, read);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ReadIntFromBadString()
        {
            int deafultValue = 10;
            string keyName = "StringValue";

            using (RegistryKey key = registry.CreateKey("Testing"))
            {
                key.SetValue(keyName, "NonInt");

                try
                {
                    // Read it back
                    int read = RegistryHelper.GetValue(key, keyName, deafultValue);
                }
                finally
                {
                    // Cleanup
                    key.DeleteValue(keyName);
                }
            }
        }

        [TestMethod]
        public void ReadIntFromConvertible()
        {
            int deafultValue = 1024;
            string keyName = "Value";

            using (RegistryKey key = registry.CreateKey("Testing"))
            {
                key.SetValue(keyName, deafultValue);

                // Read it back
                int read = RegistryHelper.GetValue(key, keyName, 0);

                // Cleanup
                key.DeleteValue(keyName);

                Assert.AreEqual(deafultValue, read);
            }
        }
    }
}
