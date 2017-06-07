using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class UpsLocalRateTableRepositoryTest : IDisposable
    {
        readonly AutoMock mock;
        readonly UpsLocalRateTableRepository testObject;

        public UpsLocalRateTableRepositoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testObject = mock.Create<UpsLocalRateTableRepository>();
        }

        [Fact]
        public void GetRateTable_ReturnsUpsAccountEntityUpsRateTable_WhenAccountEntityRateTableIsNotNull()
        {
            UpsRateTableEntity rateTable = new UpsRateTableEntity();
            UpsAccountEntity account = new UpsAccountEntity() { UpsRateTable = rateTable };
            
            Assert.Equal(testObject.GetRateTable(account), rateTable);
        }

        [Fact]
        public void GetRateTable_ReturnsUpsAccountEntityUpsRateTable_WhenAccountEntityRateTableIDIsNotNull()
        {
            UpsAccountEntity account = new UpsAccountEntity() { UpsRateTable = null, UpsRateTableID = 123 };
            Mock<ISqlAdapter> adapter = mock.Mock<ISqlAdapter>();

            mock.Mock<ISqlAdapterFactory>().Setup(f => f.Create()).Returns(adapter);

            testObject.GetRateTable(account);

            adapter.Verify(s => s.FetchEntity(It.Is<UpsRateTableEntity>(t => t.UpsRateTableID == 123)));
        }

        [Fact]
        public void GetRateTable_ReturnsNull_WhenUpsRateTableAndUpsRateTableIDAreNull()
        {
            UpsAccountEntity account = new UpsAccountEntity() { UpsRateTable = null, UpsRateTableID = null };

            Assert.Null(testObject.GetRateTable(account));
        }

        [Fact]
        public void SaveRateTable_SetsAccountsUpsRateTableId()
        {
            UpsRateTableEntity rateTable = new UpsRateTableEntity() { UpsRateTableID = 123 };
            UpsAccountEntity account = new UpsAccountEntity();
            
            testObject.Save(rateTable, account);

            Assert.Equal(rateTable.UpsRateTableID, account.UpsRateTableID);
        }

        [Fact]
        public void SaveRateTable_DelegatesToAccountRepo()
        {
            UpsRateTableEntity rateTable = new UpsRateTableEntity() { UpsRateTableID = 123 };
            UpsAccountEntity account = new UpsAccountEntity();

            testObject.Save(rateTable, account);

            mock.Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>()
                .Verify(r=>r.Save(account));
        }

        [Fact]
        public void SaveRateTable_SetsAccountsUpsRateTable()
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

        [Fact]
        public void GetSurcharges_GetsAccountFromAccountRepository()
        {
            UpsAccountEntity account = new UpsAccountEntity() {UpsRateTableID = 42};

            Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>> accountRepo = mock.Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>();
            accountRepo.Setup(a => a.GetAccount(It.Is<long>(l => l == 123123))).Returns(account);
            
            testObject.GetSurcharges(123123);

            accountRepo.Verify(r => r.GetAccount(123123));
        }

        [Fact]
        public void GetSurcharges_ReturnsEmptySurcharges_WhenAccountHasNoRateTable()
        {
            UpsAccountEntity account = new UpsAccountEntity() {UpsRateTableID = 42};
            
            Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>> accountRepo = mock.Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>();
            accountRepo.Setup(a => a.GetAccount(It.Is<long>(l => l == 123123))).Returns(account);

            IDictionary<UpsSurchargeType, double> result = testObject.GetSurcharges(123123);
            Assert.Empty(result);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
