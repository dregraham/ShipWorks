using System;
using Autofac.Extras.Moq;
using log4net;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.Carriers.Postal.Usps
{
    public class GlobalPostAvailabilityServiceTest : IDisposable
    {
        private AutoMock mock;

        public GlobalPostAvailabilityServiceTest()
        {
            mock = AutoMock.GetLoose();
            //GlobalPostAvailabilityService testObject = new GlobalPostAvailabilityService();
        }

        [Fact]
        public void InitializeForCurrentSession_AddsGlobalPostServices_WhenAccountSupportsGlobalPostServices()
        {
            UspsAccountEntity account = new UspsAccountEntity();
            AccountInfo accountInfo = new AccountInfo
            {
                Capabilities = new CapabilitiesV11
                {
                    CanPrintGPSmartSaver = false,
                    CanPrintGP = true
                }
            };

            Mock<CarrierAccountRepositoryBase<UspsAccountEntity, IUspsAccountEntity>> accountRepo = mock.Mock<CarrierAccountRepositoryBase<UspsAccountEntity, IUspsAccountEntity>>();
            accountRepo.SetupGet(a => a.Accounts).Returns(new[] { account });

            Mock<IUspsWebClient> webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(w => w.GetAccountInfo(account)).Returns(accountInfo);

            Mock<Func<UspsResellerType, IUspsWebClient>> repo = mock.MockRepository.Create<Func<UspsResellerType, IUspsWebClient>>();
            repo.Setup(x => x(It.IsAny<UspsResellerType>())).Returns(webClient.Object);
            mock.Provide(repo.Object);

            GlobalPostAvailabilityService testObject = mock.Create<GlobalPostAvailabilityService>();
            testObject.InitializeForCurrentSession();

            Assert.Contains(PostalServiceType.GlobalPostPriority, testObject.Services);
            Assert.Contains(PostalServiceType.GlobalPostEconomy, testObject.Services);

            Assert.DoesNotContain(PostalServiceType.GlobalPostSmartSaverEconomy, testObject.Services);
            Assert.DoesNotContain(PostalServiceType.GlobalPostSmartSaverPriority, testObject.Services);
        }

        [Fact]
        public void InitializeForCurrentSession_AddsGlobalPostSmartSaverServices_WhenAccountSupportsGlobalPostSmartSaverServices()
        {
            UspsAccountEntity account = new UspsAccountEntity();
            AccountInfo accountInfo = new AccountInfo
            {
                Capabilities = new CapabilitiesV11
                {
                    CanPrintGPSmartSaver = true,
                    CanPrintGP = false
                }
            };

            Mock<CarrierAccountRepositoryBase<UspsAccountEntity, IUspsAccountEntity>> accountRepo = mock.Mock<CarrierAccountRepositoryBase<UspsAccountEntity, IUspsAccountEntity>>();
            accountRepo.SetupGet(a => a.Accounts).Returns(new[] { account });

            Mock<IUspsWebClient> webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(w => w.GetAccountInfo(account)).Returns(accountInfo);

            Mock<Func<UspsResellerType, IUspsWebClient>> repo = mock.MockRepository.Create<Func<UspsResellerType, IUspsWebClient>>();
            repo.Setup(x => x(It.IsAny<UspsResellerType>())).Returns(webClient.Object);
            mock.Provide(repo.Object);

            GlobalPostAvailabilityService testObject = mock.Create<GlobalPostAvailabilityService>();
            testObject.InitializeForCurrentSession();

            Assert.DoesNotContain(PostalServiceType.GlobalPostPriority, testObject.Services);
            Assert.DoesNotContain(PostalServiceType.GlobalPostEconomy, testObject.Services);

            Assert.Contains(PostalServiceType.GlobalPostSmartSaverEconomy, testObject.Services);
            Assert.Contains(PostalServiceType.GlobalPostSmartSaverPriority, testObject.Services);
        }

        [Fact]
        public void InitializeForCurrentSession_DoesNotAddsGlobalPostServices_WhenAccountDoesNotSupportsGlobalPostServices()
        {
            UspsAccountEntity account = new UspsAccountEntity();
            AccountInfo accountInfo = new AccountInfo
            {
                Capabilities = new CapabilitiesV11
                {
                    CanPrintGPSmartSaver = false,
                    CanPrintGP = false
                }
            };

            Mock<CarrierAccountRepositoryBase<UspsAccountEntity, IUspsAccountEntity>> accountRepo = mock.Mock<CarrierAccountRepositoryBase<UspsAccountEntity, IUspsAccountEntity>>();
            accountRepo.SetupGet(a => a.Accounts).Returns(new[] { account });

            Mock<IUspsWebClient> webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(w => w.GetAccountInfo(account)).Returns(accountInfo);

            Mock<Func<UspsResellerType, IUspsWebClient>> repo = mock.MockRepository.Create<Func<UspsResellerType, IUspsWebClient>>();
            repo.Setup(x => x(It.IsAny<UspsResellerType>())).Returns(webClient.Object);
            mock.Provide(repo.Object);

            GlobalPostAvailabilityService testObject = mock.Create<GlobalPostAvailabilityService>();
            testObject.InitializeForCurrentSession();

            Assert.Empty(testObject.Services);
        }

        [Fact]
        public void Refresh_AddsGlobalPostServices_WhenAccountSupportsGlobalPostServices()
        {
            UspsAccountEntity account = new UspsAccountEntity();
            AccountInfo accountInfo = new AccountInfo
            {
                Capabilities = new CapabilitiesV11
                {
                    CanPrintGPSmartSaver = false,
                    CanPrintGP = true
                }
            };

            Mock<CarrierAccountRepositoryBase<UspsAccountEntity, IUspsAccountEntity>> accountRepo = mock.Mock<CarrierAccountRepositoryBase<UspsAccountEntity, IUspsAccountEntity>>();
            accountRepo.SetupGet(a => a.Accounts).Returns(new[] { account });

            Mock<IUspsWebClient> webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(w => w.GetAccountInfo(account)).Returns(accountInfo);

            Mock<Func<UspsResellerType, IUspsWebClient>> repo = mock.MockRepository.Create<Func<UspsResellerType, IUspsWebClient>>();
            repo.Setup(x => x(It.IsAny<UspsResellerType>())).Returns(webClient.Object);
            mock.Provide(repo.Object);

            GlobalPostAvailabilityService testObject = mock.Create<GlobalPostAvailabilityService>();
            testObject.Refresh();

            Assert.Contains(PostalServiceType.GlobalPostPriority, testObject.Services);
            Assert.Contains(PostalServiceType.GlobalPostEconomy, testObject.Services);

            Assert.DoesNotContain(PostalServiceType.GlobalPostSmartSaverEconomy, testObject.Services);
            Assert.DoesNotContain(PostalServiceType.GlobalPostSmartSaverPriority, testObject.Services);
        }

        [Fact]
        public void Refresh_RefreshesAccountsGlobalPostAvailability_WhenRefreshAccountGlobalPostAvailabilityIsTrue()
        {
            UspsAccountEntity account = new UspsAccountEntity {GlobalPostAvailability = 0};
            AccountInfo accountInfo = new AccountInfo
            {
                Capabilities = new CapabilitiesV11
                {
                    CanPrintGPSmartSaver = true,
                    CanPrintGP = true
                }
            };

            Mock<CarrierAccountRepositoryBase<UspsAccountEntity, IUspsAccountEntity>> accountRepo = mock.Mock<CarrierAccountRepositoryBase<UspsAccountEntity, IUspsAccountEntity>>();
            accountRepo.SetupGet(a => a.Accounts).Returns(new[] { account });

            Mock<IUspsWebClient> webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(w => w.GetAccountInfo(account)).Returns(accountInfo);

            Mock<Func<UspsResellerType, IUspsWebClient>> repo = mock.MockRepository.Create<Func<UspsResellerType, IUspsWebClient>>();
            repo.Setup(x => x(It.IsAny<UspsResellerType>())).Returns(webClient.Object);
            mock.Provide(repo.Object);

            GlobalPostAvailabilityService testObject = mock.Create<GlobalPostAvailabilityService>();
            testObject.Refresh(account, true);

            Assert.Equal(3, account.GlobalPostAvailability);
        }

        [Fact]
        public void Refresh_DoesNotRefreshAccountsGlobalPostAvailability_WhenRefreshAccountGlobalPostAvailabilityIsFalse()
        {
            UspsAccountEntity account = new UspsAccountEntity { GlobalPostAvailability = 0 };
            AccountInfo accountInfo = new AccountInfo
            {
                Capabilities = new CapabilitiesV11
                {
                    CanPrintGPSmartSaver = true,
                    CanPrintGP = true
                }
            };

            Mock<CarrierAccountRepositoryBase<UspsAccountEntity, IUspsAccountEntity>> accountRepo = mock.Mock<CarrierAccountRepositoryBase<UspsAccountEntity, IUspsAccountEntity>>();
            accountRepo.SetupGet(a => a.Accounts).Returns(new[] { account });

            Mock<IUspsWebClient> webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(w => w.GetAccountInfo(account)).Returns(accountInfo);

            Mock<Func<UspsResellerType, IUspsWebClient>> repo = mock.MockRepository.Create<Func<UspsResellerType, IUspsWebClient>>();
            repo.Setup(x => x(It.IsAny<UspsResellerType>())).Returns(webClient.Object);
            mock.Provide(repo.Object);

            GlobalPostAvailabilityService testObject = mock.Create<GlobalPostAvailabilityService>();
            testObject.Refresh(account, false);

            Assert.Equal(0, account.GlobalPostAvailability);
        }

        [Fact]
        public void Refresh_LogsError_WhenUspsWebClientThrows()
        {
            UspsAccountEntity account = new UspsAccountEntity { GlobalPostAvailability = 0 };
            Mock<CarrierAccountRepositoryBase<UspsAccountEntity, IUspsAccountEntity>> accountRepo = mock.Mock<CarrierAccountRepositoryBase<UspsAccountEntity, IUspsAccountEntity>>();
            accountRepo.SetupGet(a => a.Accounts).Returns(new[] { account });

            UspsException ex = new UspsException("Something went wrong");

            Mock<IUspsWebClient> webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(w => w.GetAccountInfo(account)).Throws(ex);

            Mock<Func<UspsResellerType, IUspsWebClient>> repo = mock.MockRepository.Create<Func<UspsResellerType, IUspsWebClient>>();
            repo.Setup(x => x(It.IsAny<UspsResellerType>())).Returns(webClient.Object);
            mock.Provide(repo.Object);

            Mock<ILog> log = mock.Mock<ILog>();

            Mock<Func<Type, ILog>> logFactory = mock.MockRepository.Create<Func<Type, ILog>>();
            logFactory.Setup(x => x(It.IsAny<Type>())).Returns(log.Object);
            mock.Provide(log.Object);

            GlobalPostAvailabilityService testObject = mock.Create<GlobalPostAvailabilityService>();
            testObject.Refresh(account, true);

            log.Verify(l => l.Error("Error updating GlobalPostAvailability", ex));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}