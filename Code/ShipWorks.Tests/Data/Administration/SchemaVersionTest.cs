using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Administration.Versioning;

namespace ShipWorks.Tests.Data.Administration
{
    [TestClass]
    public class SchemaVersionTest
    {
        private SchemaVersion OlderVersionTwo;
        private SchemaVersion NewerVersionTwo;
        private SchemaVersion OlderVersionThree;
        private SchemaVersion NewerVersionThree;

        private Mock<ISchemaVersionManager> mockSchemaVersionManager;

        [TestInitialize]
        public void TestInitialize()
        {
            mockSchemaVersionManager = new Mock<ISchemaVersionManager>();

            OlderVersionTwo = new SchemaVersion("2.2.2.1", mockSchemaVersionManager.Object);
            NewerVersionTwo = new SchemaVersion("2.2.4.1", mockSchemaVersionManager.Object);

            OlderVersionThree = new SchemaVersion("3.4.1.2", mockSchemaVersionManager.Object);
            NewerVersionThree = new SchemaVersion("NewerThree", mockSchemaVersionManager.Object);

            mockSchemaVersionManager
                .Setup(s => s.GetUpdateScripts(It.IsAny<SchemaVersion>(), It.IsAny<SchemaVersion>()))
                .Throws(new FindVersionUpgradePathException("oops"));

            mockSchemaVersionManager
                .Setup(s => s.GetUpdateScripts(OlderVersionThree, NewerVersionThree))
                .Returns(new List<SqlUpdateScript>() { new SqlUpdateScript("UpdatePath", "name") });
        }

        [TestMethod]
        public void Compare_ReturnsNewer_BothVersionTwoAndCompareVersionIsNewer_Test()
        {
            Assert.AreEqual(SchemaVersionComparisonResult.Newer, OlderVersionTwo.Compare(NewerVersionTwo));
        }

        [TestMethod]
        public void Compare_ReturnsOlder_BothVersionTwoAndCompareVersionIsOlder_Test()
        {
            Assert.AreEqual(SchemaVersionComparisonResult.Older, NewerVersionTwo.Compare(OlderVersionTwo));
        }

        [TestMethod]
        public void Compare_ReturnsNewer_VersionTwoAndCompareVersionIsThree_Test()
        {
            Assert.AreEqual(SchemaVersionComparisonResult.Newer, OlderVersionTwo.Compare(NewerVersionThree));
        }

        [TestMethod]
        public void Compare_ReturnsOlder_VersionThreeAndCompareVersionIsTwo_Test()
        {
            Assert.AreEqual(SchemaVersionComparisonResult.Older, NewerVersionThree.Compare(OlderVersionTwo));
        }


        [TestMethod]
        public void Compare_ReturnsNewer_BothVersionThreeAndCompareVersionIsNewer_Test()
        {
            Assert.AreEqual(SchemaVersionComparisonResult.Newer, OlderVersionThree.Compare(NewerVersionThree));
        }

        [TestMethod]
        public void Compare_ReturnsOlder_BothVersionThreeAndCompareVersionIsOlder_Test()
        {
            Assert.AreEqual(SchemaVersionComparisonResult.Older, NewerVersionThree.Compare(OlderVersionThree));
        }
    }
}
