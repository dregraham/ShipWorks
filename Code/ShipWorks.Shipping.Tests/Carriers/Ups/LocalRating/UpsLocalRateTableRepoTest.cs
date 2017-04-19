using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.Custom;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class UpsLocalRateTableRepoTest : IDisposable
    {
        readonly AutoMock mock;
        readonly UpsLocalRateTableRepository testObject;

        public UpsLocalRateTableRepoTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testObject = mock.Create<UpsLocalRateTableRepository>();
        }

        [Fact]
        public void Get_ReturnsUpsAccountEntityUpsRateTable_WhenAccountEntityRateTableIsNotNull()
        {
            UpsRateTableEntity rateTable = new UpsRateTableEntity();
            UpsAccountEntity account = new UpsAccountEntity() { UpsRateTable = rateTable };
            
            Assert.Equal(testObject.Get(account), rateTable);
        }

        [Fact]
        public void Get_ReturnsUpsAccountEntityUpsRateTable_WhenAccountEntityRateTableIDIsNotNull()
        {
            UpsAccountEntity account = new UpsAccountEntity() { UpsRateTable = null, UpsRateTableID = 123 };
            Mock<ISqlAdapter> adapter = mock.Mock<ISqlAdapter>();

            mock.Mock<ISqlAdapterFactory>().Setup(f => f.Create()).Returns(adapter);

            testObject.Get(account);

            adapter.Verify(s => s.FetchEntity(It.Is<UpsRateTableEntity>(t => t.UpsRateTableID == 123)));
        }

        [Fact]
        public void Get_ReturnsNull_WhenUpsRateTableAndUpsRateTableIDAreNull()
        {
            UpsAccountEntity account = new UpsAccountEntity() { UpsRateTable = null, UpsRateTableID = null };

            Assert.Null(testObject.Get(account));
        }

        [Fact]
        public void Save_SetsAccountsUpsRateTableId()
        {
            UpsRateTableEntity rateTable = new UpsRateTableEntity() { UpsRateTableID = 123 };
            UpsAccountEntity account = new UpsAccountEntity();
            
            testObject.Save(rateTable, account);

            Assert.Equal(rateTable.UpsRateTableID, account.UpsRateTableID);
        }

        [Fact]
        public void Save_SetsAccountsUpsRateTable()
        {
            UpsRateTableEntity rateTable = new UpsRateTableEntity() { UpsRateTableID = 123 };
            UpsAccountEntity account = new UpsAccountEntity();

            testObject.Save(rateTable, account);

            Assert.Equal(rateTable, account.UpsRateTable);
        }

        [Fact]
        public void SaveZoneFile_DelegatesToAdapterWithZoneFile()
        {
            var adapter = mock.Mock<ISqlAdapter>();
            mock.Mock<ISqlAdapterFactory>().Setup(f => f.Create()).Returns(adapter);
            var zoneFile = new UpsLocalRatingZoneFileEntity();

            testObject.Save(zoneFile);

            adapter.Verify(t => t.SaveEntity(zoneFile, false, true));
        }

        [Fact]
        public void SaveZoneFile_ThrowsUpsLocalRatingException_WhenAdapterThrowsOrmException()
        {
            var zoneFile = new UpsLocalRatingZoneFileEntity();
            var adapter = mock.Mock<ISqlAdapter>();
            adapter.Setup(a => a.SaveEntity(zoneFile, false, true)).Throws(new ORMException("Something went wrong"));
            mock.Mock<ISqlAdapterFactory>().Setup(f => f.Create()).Returns(adapter);
            
            UpsLocalRatingException ex = Assert.Throws<UpsLocalRatingException>(() => testObject.Save(zoneFile));

            Assert.Equal("Error saving zones:\r\n\r\nSomething went wrong", ex.Message);
        }

        [Fact]
        public void GetLatestZoneFile_ThrowsUpsLocalRatingException_WhenAdapterThrowsOrmException()
        {
            var adapter = mock.Mock<ISqlAdapter>();
            adapter.Setup(a => a.FetchEntityCollection(It.IsAny<IEntityCollection2>(), null)).Throws(new ORMException("Something went wrong"));
            mock.Mock<ISqlAdapterFactory>().Setup(f => f.Create()).Returns(adapter);

            UpsLocalRatingException ex = Assert.Throws<UpsLocalRatingException>(() => testObject.GetLatestZoneFile());

            Assert.Equal("Error retrieving zones:\r\n\r\nSomething went wrong", ex.Message);
        }

        [Fact]
        public void GetLatestZoneFile_ReturnsTheNewstZoneFile()
        {
            UpsLocalRatingZoneFileEntity file1 = new UpsLocalRatingZoneFileEntity { UploadDate = DateTime.UtcNow };
            UpsLocalRatingZoneFileEntity file2 = new UpsLocalRatingZoneFileEntity { UploadDate = DateTime.UtcNow.AddDays(-1) };
            UpsLocalRatingZoneFileEntity file3 = new UpsLocalRatingZoneFileEntity { UploadDate = DateTime.UtcNow.AddDays(-2) };
            UpsLocalRatingZoneFileEntity file4 = new UpsLocalRatingZoneFileEntity { UploadDate = DateTime.UtcNow.AddDays(-3) };
            UpsLocalRatingZoneFileEntity file5 = new UpsLocalRatingZoneFileEntity { UploadDate = DateTime.UtcNow.AddDays(-4) };

            Mock<ISqlAdapter> adapter = mock.Mock<ISqlAdapter>();
            adapter.Setup(a => a.FetchEntityCollection(It.IsAny<UpsLocalRatingZoneFileCollection>(), null))
                .Callback<IEntityCollection2, IRelationPredicateBucket>((c, a) =>
                {
                    UpsLocalRatingZoneFileCollection c2 = (UpsLocalRatingZoneFileCollection) c;

                    c2.Items.Add(file1);
                    c2.Items.Add(file2);
                    c2.Items.Add(file3);
                    c2.Items.Add(file4);
                    c2.Items.Add(file5);
                });
            mock.Mock<ISqlAdapterFactory>().Setup(f => f.Create()).Returns(adapter);
            
            Assert.Equal(file1, testObject.GetLatestZoneFile());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
