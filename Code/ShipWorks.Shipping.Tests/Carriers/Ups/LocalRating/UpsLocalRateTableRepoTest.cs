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
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class UpsLocalRateTableRepoTest : IDisposable
    {
        readonly AutoMock mock;
        readonly UpsLocalRateTableRepo testObject;

        public UpsLocalRateTableRepoTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testObject = mock.Create<UpsLocalRateTableRepo>();
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
        public void Save_DelegatesToAdapterSaveAndRefetchWithRateTable()
        {
            UpsRateTableEntity rateTable = new UpsRateTableEntity() { UpsRateTableID = 123 };
            UpsAccountEntity account = new UpsAccountEntity() { UpsRateTable = rateTable, UpsRateTableID = 123 };
            Mock<ISqlAdapter> adapter = mock.Mock<ISqlAdapter>();

            mock.Mock<ISqlAdapterFactory>().Setup(f => f.Create()).Returns(adapter);

            testObject.Save(rateTable, account);

            adapter.Verify(a => a.SaveAndRefetch(rateTable));
        }

        [Fact]
        public void Save_DelegatesToAdapterSaveAndRefetchWithAccount()
        {
            UpsRateTableEntity rateTable = new UpsRateTableEntity() { UpsRateTableID = 123 };
            UpsAccountEntity account = new UpsAccountEntity() { UpsRateTable = rateTable, UpsRateTableID = 123 };
            Mock<ISqlAdapter> adapter = mock.Mock<ISqlAdapter>();

            mock.Mock<ISqlAdapterFactory>().Setup(f => f.Create()).Returns(adapter);

            testObject.Save(rateTable, account);

            adapter.Verify(a => a.SaveAndRefetch(account));
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

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
