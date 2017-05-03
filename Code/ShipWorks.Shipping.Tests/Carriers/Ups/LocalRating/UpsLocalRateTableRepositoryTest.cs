using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.UPS.Enums;
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

        [Fact]
        public void GetSurcharges_GetsAccountFromAccountRepository()
        {
            UpsAccountEntity account = new UpsAccountEntity();

            Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>> accountRepo = mock.Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>();
            accountRepo.Setup(a => a.GetAccount(It.Is<long>(l => l == 123123))).Returns(account);
            
            testObject.GetSurcharges(123123);

            accountRepo.Verify(r => r.GetAccount(123123));
        }

        [Fact]
        public void GetSurcharges_ReturnsSurchargesForAccount()
        {
            UpsAccountEntity account = new UpsAccountEntity {UpsRateTable = new UpsRateTableEntity()};

            account.UpsRateTable.UpsRateSurcharge.Add(new UpsRateSurchargeEntity {SurchargeType = (int) UpsSurchargeType.LargePackage, Amount = 12.21});
            account.UpsRateTable.UpsRateSurcharge.Add(new UpsRateSurchargeEntity {SurchargeType = (int) UpsSurchargeType.AdditionalHandling, Amount = 44.21});
            account.UpsRateTable.UpsRateSurcharge.Add(new UpsRateSurchargeEntity {SurchargeType = (int) UpsSurchargeType.AdultSignatureRequired, Amount = 9.21});

            Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>> accountRepo = mock.Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>();
            accountRepo.Setup(a => a.GetAccount(It.Is<long>(l => l == 123123))).Returns(account);

            IDictionary<UpsSurchargeType, double> result = testObject.GetSurcharges(123123);

            Assert.Contains(new KeyValuePair<UpsSurchargeType, double>(UpsSurchargeType.LargePackage, 12.21), result);
            Assert.Contains(new KeyValuePair<UpsSurchargeType, double>(UpsSurchargeType.AdditionalHandling, 44.21), result);
            Assert.Contains(new KeyValuePair<UpsSurchargeType, double>(UpsSurchargeType.AdultSignatureRequired, 9.21), result);
        }

        [Fact]
        public void GetSurcharges_ReturnsEmptySurcharges_WhenAccountHasNoRateTable()
        {
            UpsAccountEntity account = new UpsAccountEntity();
            
            Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>> accountRepo = mock.Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>();
            accountRepo.Setup(a => a.GetAccount(It.Is<long>(l => l == 123123))).Returns(account);

            IDictionary<UpsSurchargeType, double> result = testObject.GetSurcharges(123123);
            Assert.Empty(result);
        }

        [Fact]
        public void GetServiceRates_ReturnsEmptyList_WhenShipmentHasMultiplePackages()
        {
            UpsShipmentEntity shipment =
                new UpsShipmentEntity { Packages = { new UpsPackageEntity(), new UpsPackageEntity() } };

            IEnumerable<UpsLocalServiceRate> result = testObject.GetServiceRates(shipment, new[] { UpsServiceType.Ups2DayAir, UpsServiceType.UpsGround });

            Assert.Empty(result);
        }

        [Fact]
        public void GetServiceRates_ThrowsUpsLocalRatingException_WhenShipmentPostalCodeIsNotValid()
        {
            UpsShipmentEntity upsShipment =
                new UpsShipmentEntity
                {
                    Packages = { new UpsPackageEntity() },
                    Shipment = new ShipmentEntity()
                };

            upsShipment.Shipment.OriginPostalCode = "12345";
            upsShipment.Shipment.ShipPostalCode = "abcde";

            UpsLocalRatingException ex = Assert.Throws<UpsLocalRatingException>(
                () => testObject.GetServiceRates(upsShipment,
                    new[] {UpsServiceType.Ups2DayAir, UpsServiceType.UpsGround}));


            Assert.Equal("Unable to find zone using destination postal code abcde." , ex.Message);
        }

        [Fact]
        public void GetServiceRates_ThrowsUpsLocalRatingException_WhenZoneCannotBeFoundBetweenDestinationAndOrigin()
        {
            UpsShipmentEntity upsShipment =
                new UpsShipmentEntity
                {
                    Packages = { new UpsPackageEntity() },
                    Shipment = new ShipmentEntity()
                };

            upsShipment.Shipment.ShipStateProvCode = "MO";
            upsShipment.Shipment.ShipPostalCode = "12345";
            upsShipment.Shipment.OriginPostalCode = "12345";

            UpsLocalRatingZoneFileEntity zoneFile = new UpsLocalRatingZoneFileEntity { UploadDate = DateTime.UtcNow };
            Mock<ISqlAdapter> adapter = mock.Mock<ISqlAdapter>();
            adapter.Setup(a => a.FetchEntityCollection(It.IsAny<UpsLocalRatingZoneFileCollection>(), null))
                .Callback<IEntityCollection2, IRelationPredicateBucket>((c, a) =>
                {
                    UpsLocalRatingZoneFileCollection c2 = (UpsLocalRatingZoneFileCollection)c;

                    c2.Items.Add(zoneFile);
                });
            mock.Mock<ISqlAdapterFactory>().Setup(f => f.Create()).Returns(adapter);

            UpsLocalRatingException ex = Assert.Throws<UpsLocalRatingException>(
                () => testObject.GetServiceRates(upsShipment,
                    new[] { UpsServiceType.Ups2DayAir, UpsServiceType.UpsGround }));

            Assert.Equal("Unable to find zone using oringin postal code 12345 and destination postal code 12345.", ex.Message);
        }

        [Fact]
        public void GetServiceRates_ReturnsEmptyList_WhenNoRatesExistForZone()
        {
            UpsShipmentEntity upsShipment =
                new UpsShipmentEntity
                {
                    Packages = { new UpsPackageEntity() },
                    Shipment = new ShipmentEntity()
                };

            upsShipment.Shipment.ShipStateProvCode = "MO";
            upsShipment.Shipment.ShipPostalCode = "12345";
            upsShipment.Shipment.OriginPostalCode = "12345";

            UpsLocalRatingZoneFileEntity zoneFile = new UpsLocalRatingZoneFileEntity { UploadDate = DateTime.UtcNow };
            zoneFile.UpsLocalRatingZone.Add(new UpsLocalRatingZoneEntity
            {
                Zone = "ABCD",
                OriginZipFloor = 12345,
                OriginZipCeiling = 12345,
                DestinationZipFloor = 12345,
                DestinationZipCeiling = 12345
            });

            Mock<ISqlAdapter> adapter = mock.Mock<ISqlAdapter>();
            adapter.Setup(a => a.FetchEntityCollection(It.IsAny<UpsLocalRatingZoneFileCollection>(), null))
                .Callback<IEntityCollection2, IRelationPredicateBucket>((c, a) =>
                {
                    UpsLocalRatingZoneFileCollection c2 = (UpsLocalRatingZoneFileCollection)c;

                    c2.Items.Add(zoneFile);
                });
            mock.Mock<ISqlAdapterFactory>().Setup(f => f.Create()).Returns(adapter);

            Assert.Empty(testObject.GetServiceRates(upsShipment, new[] { UpsServiceType.Ups2DayAir, UpsServiceType.UpsGround }));
        }

        [Fact]
        public void GetServiceRates_ReturnsRateMatchingZone()
        {
            UpsAccountEntity account = new UpsAccountEntity {UpsRateTable = new UpsRateTableEntity()};
            account.UpsRateTable.UpsPackageRate.Add(new UpsPackageRateEntity
            {
                Zone = "ABC",
                Rate = 99,
                WeightInPounds = 0,
                Service = (int) UpsServiceType.UpsGround
            });

            UpsShipmentEntity upsShipment =
                new UpsShipmentEntity
                {
                    Packages = {new UpsPackageEntity {Weight = 0}},
                    Shipment = new ShipmentEntity()
                };

            Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>> accountRepo = mock.Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>();
            accountRepo.Setup(a => a.GetAccount(It.Is<ShipmentEntity>(s => Equals(s, upsShipment.Shipment)))).Returns(account);

            upsShipment.Shipment.ShipStateProvCode = "MO";
            upsShipment.Shipment.ShipPostalCode = "12345";
            upsShipment.Shipment.OriginPostalCode = "12345";
            
            UpsLocalRatingZoneFileEntity zoneFile = new UpsLocalRatingZoneFileEntity { UploadDate = DateTime.UtcNow };
            zoneFile.UpsLocalRatingZone.Add(new UpsLocalRatingZoneEntity
            {
                Zone = "ABC",
                OriginZipFloor = 12345,
                OriginZipCeiling = 12345,
                DestinationZipFloor = 12345,
                DestinationZipCeiling = 12345
            });

            Mock<ISqlAdapter> adapter = mock.Mock<ISqlAdapter>();
            adapter.Setup(a => a.FetchEntityCollection(It.IsAny<UpsLocalRatingZoneFileCollection>(), null))
                .Callback<IEntityCollection2, IRelationPredicateBucket>((c, a) =>
                {
                    UpsLocalRatingZoneFileCollection c2 = (UpsLocalRatingZoneFileCollection)c;

                    c2.Items.Add(zoneFile);
                });
            mock.Mock<ISqlAdapterFactory>().Setup(f => f.Create()).Returns(adapter);

            IEnumerable<UpsLocalServiceRate> result = testObject.GetServiceRates(upsShipment,
                new[] {UpsServiceType.Ups2DayAir, UpsServiceType.UpsGround});

            Assert.Equal(result.First().Amount, 99);
        }

        [Fact]
        public void GetServiceRates_ReturnsLetterRate_WhenPackageTypeIsLetter()
        {
            UpsAccountEntity account = new UpsAccountEntity { UpsRateTable = new UpsRateTableEntity() };
            account.UpsRateTable.UpsPackageRate.Add(new UpsPackageRateEntity
            {
                Zone = "ABC",
                Rate = 99,
                WeightInPounds = 0,
                Service = (int) UpsServiceType.UpsGround
            });

            account.UpsRateTable.UpsLetterRate.Add(new UpsLetterRateEntity()
            {
                Zone = "AAA",
                Rate = 12,
                Service = (int) UpsServiceType.UpsGround
            });

            UpsShipmentEntity upsShipment =
                new UpsShipmentEntity
                {
                    Packages = { new UpsPackageEntity { Weight = 0, PackagingType = (int) UpsPackagingType.Letter} },
                    Shipment = new ShipmentEntity()
                };

            Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>> accountRepo = mock.Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>();
            accountRepo.Setup(a => a.GetAccount(It.Is<ShipmentEntity>(s => Equals(s, upsShipment.Shipment)))).Returns(account);

            upsShipment.Shipment.ShipStateProvCode = "MO";
            upsShipment.Shipment.ShipPostalCode = "12345";
            upsShipment.Shipment.OriginPostalCode = "12345";

            UpsLocalRatingZoneFileEntity zoneFile = new UpsLocalRatingZoneFileEntity { UploadDate = DateTime.UtcNow };
            zoneFile.UpsLocalRatingZone.Add(new UpsLocalRatingZoneEntity
            {
                Zone = "ABC",
                OriginZipFloor = 45678,
                OriginZipCeiling = 45678,
                DestinationZipFloor = 45678,
                DestinationZipCeiling = 45678
            });

            zoneFile.UpsLocalRatingZone.Add(new UpsLocalRatingZoneEntity
            {
                Zone = "AAA",
                OriginZipFloor = 12345,
                OriginZipCeiling = 12345,
                DestinationZipFloor = 12345,
                DestinationZipCeiling = 12345
            });

            Mock<ISqlAdapter> adapter = mock.Mock<ISqlAdapter>();
            adapter.Setup(a => a.FetchEntityCollection(It.IsAny<UpsLocalRatingZoneFileCollection>(), null))
                .Callback<IEntityCollection2, IRelationPredicateBucket>((c, a) =>
                {
                    UpsLocalRatingZoneFileCollection c2 = (UpsLocalRatingZoneFileCollection)c;

                    c2.Items.Add(zoneFile);
                });
            mock.Mock<ISqlAdapterFactory>().Setup(f => f.Create()).Returns(adapter);

            IEnumerable<UpsLocalServiceRate> result = testObject.GetServiceRates(upsShipment,
                new[] { UpsServiceType.Ups2DayAir, UpsServiceType.UpsGround });

            Assert.Equal(result.First().Amount, 12);
        }


        [Fact]
        public void GetServiceRates_ReturnsPricePerPound_WhenShipmentIsMoreThan150LBS()
        {
            UpsAccountEntity account = new UpsAccountEntity { UpsRateTable = new UpsRateTableEntity() };
            account.UpsRateTable.UpsPackageRate.Add(new UpsPackageRateEntity
            {
                Zone = "ABC",
                Rate = 999,
                WeightInPounds = 150,
                Service = (int)UpsServiceType.UpsGround
            });

            account.UpsRateTable.UpsPricePerPound.Add(new UpsPricePerPoundEntity()
            {
                Zone = "ABC",
                Rate = 12,
                Service = (int)UpsServiceType.UpsGround
            });

            UpsShipmentEntity upsShipment =
                new UpsShipmentEntity
                {
                    Packages = { new UpsPackageEntity { Weight = 151, PackagingType = (int)UpsPackagingType.Custom } },
                    Shipment = new ShipmentEntity()
                };

            Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>> accountRepo = mock.Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>();
            accountRepo.Setup(a => a.GetAccount(It.Is<ShipmentEntity>(s => Equals(s, upsShipment.Shipment)))).Returns(account);

            upsShipment.Shipment.ShipStateProvCode = "MO";
            upsShipment.Shipment.ShipPostalCode = "12345";
            upsShipment.Shipment.OriginPostalCode = "12345";

            UpsLocalRatingZoneFileEntity zoneFile = new UpsLocalRatingZoneFileEntity { UploadDate = DateTime.UtcNow };
            zoneFile.UpsLocalRatingZone.Add(new UpsLocalRatingZoneEntity
            {
                Zone = "ABC",
                OriginZipFloor = 12345,
                OriginZipCeiling = 12345,
                DestinationZipFloor = 12345,
                DestinationZipCeiling = 12345
            });

            Mock<ISqlAdapter> adapter = mock.Mock<ISqlAdapter>();
            adapter.Setup(a => a.FetchEntityCollection(It.IsAny<UpsLocalRatingZoneFileCollection>(), null))
                .Callback<IEntityCollection2, IRelationPredicateBucket>((c, a) =>
                {
                    UpsLocalRatingZoneFileCollection c2 = (UpsLocalRatingZoneFileCollection)c;

                    c2.Items.Add(zoneFile);
                });
            mock.Mock<ISqlAdapterFactory>().Setup(f => f.Create()).Returns(adapter);

            IEnumerable<UpsLocalServiceRate> result = testObject.GetServiceRates(upsShipment,
                new[] { UpsServiceType.Ups2DayAir, UpsServiceType.UpsGround });

            Assert.Equal(result.First().Amount, 1011);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
