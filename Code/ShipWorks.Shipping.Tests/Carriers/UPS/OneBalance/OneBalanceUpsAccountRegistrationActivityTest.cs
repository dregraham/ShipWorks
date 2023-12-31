﻿using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Ups.OneBalance;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.OneBalance
{
    public class OneBalanceUpsAccountRegistrationActivityTest
    {
        private readonly AutoMock mock;
        private readonly Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> uspsAccountRepo;
        private readonly Mock<IShipEngineWebClient> seWebClient;
        private readonly OneBalanceUpsAccountRegistrationActivity testObject;

        public OneBalanceUpsAccountRegistrationActivityTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            var secureTextEncryptionProvider = mock.Mock<IEncryptionProvider>();
            secureTextEncryptionProvider.Setup(e => e.Decrypt(It.IsAny<string>())).Returns<string>(s => s);

            var encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
            encryptionProviderFactory.Setup(f => f.CreateSecureTextEncryptionProvider(It.IsAny<string>())).Returns(secureTextEncryptionProvider);

            uspsAccountRepo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();
            uspsAccountRepo.SetupGet(r => r.Accounts).Returns(new[] { new UspsAccountEntity() { ShipEngineCarrierId = "1" } });

            seWebClient = mock.Mock<IShipEngineWebClient>();
            seWebClient.Setup(s => s.RegisterUpsAccount(It.IsAny<PersonAdapter>(), AnyString))
                .ReturnsAsync(GenericResult.FromSuccess("123"));

            testObject = mock.Create<OneBalanceUpsAccountRegistrationActivity>();
        }

        [Fact]
        public async Task Execute_ValidatesNameFieldLength()
        {
            UpsAccountEntity upsAccount = new UpsAccountEntity()
            {
                FirstName = "",
                LastName = "",
                Company = "FooBar",
                Street1 = "1 South Memorial Drive",
                Street2 = "",
                Street3 = "",
                City = "St. Louis",
                StateProvCode = "MO",
                CountryCode = "US",
                Phone = "1-314-821-5888"
            };

            // empty should fail
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Failure);

            upsAccount.FirstName = "123456789012345678901";
            // over 20 should fail
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Failure);

            upsAccount.FirstName = "Joe";
            upsAccount.LastName = "blow";
            // between 1 and 20 should succeed
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Success);
        }

        [Fact]
        public async Task Execute_ValidatesCompanyFieldLength()
        {
            UpsAccountEntity upsAccount = new UpsAccountEntity()
            {
                FirstName = "Joe",
                LastName = "Blow",
                Company = "",
                Street1 = "1 South Memorial Drive",
                Street2 = "",
                Street3 = "",
                City = "St. Louis",
                StateProvCode = "MO",
                CountryCode = "US",
                Phone = "1-314-821-5888"
            };

            // empty should succeed
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Success);

            upsAccount.Company = "1234567890123456789012345678901";

            // over 30 should fail also succeed because LLBLGen is limiting the filed
            // length to 30
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Success);
            Assert.Equal(upsAccount.Company, "123456789012345678901234567890");


            upsAccount.Company = "foo bar";
            // between 1 and 30 should succeed
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Success);
        }

        [Fact]
        public async Task Execute_ValidatesStreet1FieldLength()
        {
            UpsAccountEntity upsAccount = new UpsAccountEntity()
            {
                FirstName = "Joe",
                LastName = "Blow",
                Company = "Foo Bar",
                Street1 = "",
                Street2 = "",
                Street3 = "",
                City = "St. Louis",
                StateProvCode = "MO",
                CountryCode = "US",
                Phone = "1-314-821-5888"
            };

            // empty should fail
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Failure);

            upsAccount.Street1 = "1234567890123456789012345678901";

            // over 30 should fail
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Failure);

            upsAccount.Street1 = "1 South Memorial Drive";
            // between 1 and 30 should succeed
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Success);
        }

        [Fact]
        public async Task Execute_ValidatesStreet2FieldLength()
        {
            UpsAccountEntity upsAccount = new UpsAccountEntity()
            {
                FirstName = "Joe",
                LastName = "Blow",
                Company = "Foo Bar",
                Street1 = "1 South Memorial Drive",
                Street2 = "",
                Street3 = "",
                City = "St. Louis",
                StateProvCode = "MO",
                CountryCode = "US",
                Phone = "1-314-821-5888"
            };

            // empty should succeed
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Success);

            upsAccount.Street2 = "1234567890123456789012345678901";

            // over 30 should fail
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Failure);

            upsAccount.Street2 = "1 South Memorial Drive";
            // between 1 and 30 should succeed
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Success);
        }

        [Fact]
        public async Task Execute_ValidatesStreet3FieldLength()
        {
            UpsAccountEntity upsAccount = new UpsAccountEntity()
            {
                FirstName = "Joe",
                LastName = "Blow",
                Company = "Foo Bar",
                Street1 = "1 South Memorial Drive",
                Street2 = "",
                Street3 = "",
                City = "St. Louis",
                StateProvCode = "MO",
                CountryCode = "US",
                Phone = "1-314-821-5888"
            };

            // empty should succeed
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Success);

            upsAccount.Street3 = "1234567890123456789012345678901";

            // over 30 should fail
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Failure);

            upsAccount.Street3 = "1 South Memorial Drive";
            // between 1 and 30 should succeed
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Success);
        }

        [Fact]
        public async Task Execute_ValidatesCityFieldLength()
        {
            UpsAccountEntity upsAccount = new UpsAccountEntity()
            {
                FirstName = "Joe",
                LastName = "Blow",
                Company = "Foo Bar",
                Street1 = "1 South Memorial Drive",
                Street2 = "",
                Street3 = "",
                City = "",
                StateProvCode = "MO",
                CountryCode = "US",
                Phone = "1-314-821-5888"
            };

            // empty should fail
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Failure);

            upsAccount.City = "1234567890123456789012345678901";

            // over 30 should fail
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Failure);

            upsAccount.City = "St Louis";
            // between 1 and 30 should succeed
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Success);
        }

        [Fact]
        public async Task Execute_ValidatesStateProvCodeFieldLength()
        {
            UpsAccountEntity upsAccount = new UpsAccountEntity()
            {
                FirstName = "Joe",
                LastName = "Blow",
                Company = "Foo Bar",
                Street1 = "1 South Memorial Drive",
                Street2 = "",
                Street3 = "",
                City = "St. Louis",
                StateProvCode = "",
                CountryCode = "US",
                Phone = "1-314-821-5888"
            };

            // empty should fail
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Failure);

            upsAccount.StateProvCode = "MOO";

            // over 2 should fail
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Failure);

            upsAccount.StateProvCode = "MO";
            // 2 should succeed
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Success);
        }

        [Fact]
        public async Task Execute_ValidatesCountryCodeFieldLength()
        {
            UpsAccountEntity upsAccount = new UpsAccountEntity()
            {
                FirstName = "Joe",
                LastName = "Blow",
                Company = "Foo Bar",
                Street1 = "1 South Memorial Drive",
                Street2 = "",
                Street3 = "",
                City = "St. Louis",
                StateProvCode = "MO",
                CountryCode = "",
                Phone = "1-314-821-5888"
            };

            // empty should fail
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Failure);

            upsAccount.CountryCode = "USSR";

            // over 2 should fail
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Failure);

            upsAccount.CountryCode = "US";
            // 2 should succeed
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Success);
        }

        [Fact]
        public async Task Execute_ValidatesSupportPhoneNumberIsCorrect_WhenTooManyAccounts()
        {
            UpsAccountEntity upsAccount = new UpsAccountEntity()
            {
                FirstName = "Joe",
                LastName = "Blow",
                Company = "Foo Bar",
                Street1 = "1 South Memorial Drive",
                Street2 = "",
                Street3 = "",
                City = "St. Louis",
                StateProvCode = "MO",
                CountryCode = "US",
                Phone = "1-314-821-5888"
            };
            uspsAccountRepo.SetupGet(r => r.Accounts).Returns(new[] 
            { 
                new UspsAccountEntity() { ShipEngineCarrierId = "1" }, 
                new UspsAccountEntity() { ShipEngineCarrierId = "2" }
            });

            OneBalanceUpsAccountRegistrationActivity testObject2 = mock.Create<OneBalanceUpsAccountRegistrationActivity>();
            var result = await testObject2.Execute(upsAccount, AnyString);

            Assert.True(result.Failure);
            Assert.True(result.Message.Contains("314-821-5888"));
        }

        [Fact]
        public async Task Execute_ValidatesPhoneFieldLength()
        {
            UpsAccountEntity upsAccount = new UpsAccountEntity()
            {
                FirstName = "Joe",
                LastName = "Blow",
                Company = "Foo Bar",
                Street1 = "1 South Memorial Drive",
                Street2 = "",
                Street3 = "",
                City = "St. Louis",
                StateProvCode = "MO",
                CountryCode = "US",
                Phone = ""
            };

            // empty should fail
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Failure);

            upsAccount.Phone = "123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123123";

            // over 200 should succeed
            Assert.True((await testObject.Execute(upsAccount, AnyString)).Success);
        }

        [Fact]
        public async Task Execute_CreatesOneBalanceAccount_WhenOneDoesNotExist()
        {
            UpsAccountEntity upsAccount = new UpsAccountEntity()
            {
                FirstName = "Joe",
                LastName = "Blow",
                Company = "Foo Bar",
                Street1 = "1 South Memorial Drive",
                Street2 = "",
                Street3 = "",
                City = "St. Louis",
                StateProvCode = "MO",
                CountryCode = "US",
                Phone = "13148215888"
            };

            UspsAccountEntity uspsAccount = new UspsAccountEntity() { Username = "foo", Password = "bar", ShipEngineCarrierId = null };

            uspsAccountRepo.SetupGet(r => r.Accounts).Returns(new[] { uspsAccount });
            seWebClient.Setup(c => c.ConnectStampsAccount("foo", "bar")).ReturnsAsync(GenericResult.FromSuccess("abcd"));

            await testObject.Execute(upsAccount, AnyString);

            seWebClient.Verify(s => s.ConnectStampsAccount("foo", "bar"));

            Assert.Equal("abcd", uspsAccount.ShipEngineCarrierId);
        }


        [Fact]
        public async Task Execute_CreatesUpsAccount_WhenOneBalanceAccountExists()
        {
            UpsAccountEntity upsAccount = new UpsAccountEntity()
            {
                FirstName = "Joe",
                LastName = "Blow",
                Company = "Foo Bar",
                Street1 = "1 South Memorial Drive",
                Street2 = "",
                Street3 = "",
                City = "St. Louis",
                StateProvCode = "MO",
                CountryCode = "US",
                Phone = "13148215888"
            };

            UspsAccountEntity uspsAccount = new UspsAccountEntity() { Username = "foo", Password = "bar", ShipEngineCarrierId = "abcd" };
            uspsAccountRepo.SetupGet(r => r.Accounts).Returns(new[] { uspsAccount });

            await testObject.Execute(upsAccount, "boo");

            seWebClient.Verify(s => s.RegisterUpsAccount(upsAccount.Address, "boo"));
        }
    }
}
