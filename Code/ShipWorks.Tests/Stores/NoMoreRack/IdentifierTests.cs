using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms;

namespace ShipWorks.Tests.Stores.NoMoreRack
{
    [TestClass]
    public class IdentifierTests
    {
        /// <summary>
        /// Check that the given url is the desired identifier
        /// </summary>
        private void DoTest(string url)
        {
            DoTest(url, "staging.vendor-api.nomorerack.com/api/v1/shipworks/60", "1.0.0.0");
        }
        
        /// <summary>
        /// Check that the the given url is the desired identifier
        /// </summary>
        private void DoTest(string url, string identifier, string schemaVersion)
        {
            GenericModuleStoreEntity nmrStore = new GenericModuleStoreEntity();
            nmrStore.ModuleUrl = url;
            nmrStore.TypeCode = (int)StoreTypeCode.NoMoreRack;
            nmrStore.SchemaVersion = schemaVersion;

            StoreType storeType = StoreTypeManager.GetType(nmrStore);

            Assert.AreEqual(identifier, storeType.LicenseIdentifier);
        }

        [TestMethod]
        public void InternalLicenseIdentifier_ReturnsNoMoreRack_WhenModuleUrlIsChoxi()
        {
            DoTest("http://staging.vendor-api.choxi.com/api/v1/shipworks/60");
        }

        [TestMethod]
        public void InternalLicenseIdentifier_ReturnsNoMoreRack_WhenModuleUrlIsNoMoreRack()
        {
            DoTest("http://staging.vendor-api.nomorerack.com/api/v1/shipworks/60");
        }
   
   
    }
}
