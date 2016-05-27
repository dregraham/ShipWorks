using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ShipWorks.Stores;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms;

namespace ShipWorks.Tests.Stores.osCommerce
{
    public class IdentifierTests
    {
        /// <summary>
        /// Check that the normalization of the given url is the desired identifier
        /// </summary>
        private void DoTest(string url)
        {
            DoTest(url, "www.interapptive.com/osc/", "1.0.0.0");
        }
        
        /// <summary>
        /// Check that the normalization of the given url is the desired identifier
        /// </summary>
        private void DoTest(string url, string identifier, string schemaVersion)
        {
            GenericModuleStoreEntity oscStore = new GenericModuleStoreEntity();
            oscStore.ModuleUrl = url;
            oscStore.TypeCode = (int) StoreTypeCode.osCommerce;
            oscStore.SchemaVersion = schemaVersion;

            StoreType storeType = StoreTypeManager.GetType(oscStore);

            Assert.Equal(identifier, storeType.LicenseIdentifier);
        }

        [Fact]
        public void TestShipWorksPhp()
        {
            DoTest("www.interapptive.com/osc/admin/shipworks.php");
        }

        [Fact]
        public void TestXyzPhp()
        {
            DoTest("https://www.interapptive.com/osc/admin/xyz.php");
        }

        [Fact]
        public void TestAbcExtension()
        {
            DoTest("http://www.interapptive.com/OSC/ABC.PhP");
        }

        [Fact]
        public void TestNoExtension()
        {
            DoTest("www.interapptive.com/OSC/ADMIN/ABC");
        }

        [Fact]
        public void TestWithAdmin()
        {
            DoTest("www.interapptive.com/osc/admin/dont.matter");
        }

        [Fact]
        public void TestWithoutAdmin()
        {
            DoTest("https://www.interapptive.com/osc/dont.matter");
        }

        [Fact]
        public void TestMultiLevelShipWorksPhp()
        {
            DoTest("www.interapptive.com/osc/mysite/whoever/shipworks.php", "www.interapptive.com/osc/mysite/whoever/", "1.0.0.0");
            DoTest("www.interapptive.com/osc/mysite/whoever/shipworks.php", "www.interapptive.com/osc/mysite/whoever/", "1.1.0.0");
        }

        [Fact]
        public void TestMultiLevelSomethingElse()
        {
            DoTest("www.interapptive.com/osc/admin/mysite/whoever/shipworks.php", "www.interapptive.com/osc/admin/mysite/whoever/", "1.0.0.0");
            DoTest("www.interapptive.com/osc/admin/mysite/whoever/shipworks.php", "www.interapptive.com/osc/admin/mysite/whoever/", "1.1.0.0");
        }

        [Fact]
        public void TestTwoAdmins()
        {
            DoTest("www.interapptive.com/osc/admin/mysite/whoever/admin/donk", "www.interapptive.com/osc/admin/mysite/whoever/", "1.0.0.0");
            DoTest("www.interapptive.com/osc/admin/mysite/whoever/admin/donk", "www.interapptive.com/osc/admin/mysite/whoever/admin/donk", "1.1.0.0");
        }

    }
}
