﻿using System;
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
        }

        [Fact]
        public void Refresh_AddsGlobalPostServices_WhenAccountSupportsGlobalPostServices()
        {
            UspsAccountEntity account = new UspsAccountEntity();
            AccountInfoV41 accountInfo = new AccountInfoV41
            {
                Capabilities = new CapabilitiesV30
                {
                    CanPrintGPSmartSaver = false,
                    CanPrintGP = true
                }
            };

            Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> accountRepo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();
            accountRepo.SetupGet(a => a.Accounts).Returns(new[] { account });

            Mock<IUspsWebClient> webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(w => w.GetAccountInfo(account)).Returns(accountInfo);

            Mock<Func<UspsResellerType, IUspsWebClient>> repo = mock.MockRepository.Create<Func<UspsResellerType, IUspsWebClient>>();
            repo.Setup(x => x(It.IsAny<UspsResellerType>())).Returns(webClient.Object);
            mock.Provide(repo.Object);

            GlobalPostAvailabilityService testObject = mock.Create<GlobalPostAvailabilityService>();
            testObject.Refresh();

            Assert.Contains(PostalServiceType.GlobalPostStandardIntl, testObject.Services);
            Assert.Contains(PostalServiceType.GlobalPostEconomyIntl, testObject.Services);

            Assert.DoesNotContain(PostalServiceType.GlobalPostSmartSaverEconomyIntl, testObject.Services);
            Assert.DoesNotContain(PostalServiceType.GlobalPostSmartSaverStandardIntl, testObject.Services);
        }

        [Fact]
        public void Refresh_AddsAllGlobalPostServices_WhenAccountSupportsAllGlobalPostServices()
        {
            UspsAccountEntity account = new UspsAccountEntity();
            AccountInfoV41 accountInfo = new AccountInfoV41
            {
                Capabilities = new CapabilitiesV30()
                {
                    CanPrintGPSmartSaver = true,
                    CanPrintGP = true
                }
            };

            Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> accountRepo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();
            accountRepo.SetupGet(a => a.Accounts).Returns(new[] { account });

            Mock<IUspsWebClient> webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(w => w.GetAccountInfo(account)).Returns(accountInfo);

            Mock<Func<UspsResellerType, IUspsWebClient>> repo = mock.MockRepository.Create<Func<UspsResellerType, IUspsWebClient>>();
            repo.Setup(x => x(It.IsAny<UspsResellerType>())).Returns(webClient.Object);
            mock.Provide(repo.Object);

            GlobalPostAvailabilityService testObject = mock.Create<GlobalPostAvailabilityService>();
            testObject.Refresh();

            Assert.Contains(PostalServiceType.GlobalPostStandardIntl, testObject.Services);
            Assert.Contains(PostalServiceType.GlobalPostEconomyIntl, testObject.Services);

            Assert.Contains(PostalServiceType.GlobalPostSmartSaverEconomyIntl, testObject.Services);
            Assert.Contains(PostalServiceType.GlobalPostSmartSaverStandardIntl, testObject.Services);
        }

        [Fact]
        public void Refresh_AddsGlobalPostSmartSaverServices_WhenAccountSupportsGlobalPostSmartSaverServices()
        {
            UspsAccountEntity account = new UspsAccountEntity();
            AccountInfoV41 accountInfo = new AccountInfoV41
            {
                Capabilities = new CapabilitiesV30
                {
                    CanPrintGPSmartSaver = true,
                    CanPrintGP = false
                }
            };

            Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> accountRepo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();
            accountRepo.SetupGet(a => a.Accounts).Returns(new[] { account });

            Mock<IUspsWebClient> webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(w => w.GetAccountInfo(account)).Returns(accountInfo);

            Mock<Func<UspsResellerType, IUspsWebClient>> repo = mock.MockRepository.Create<Func<UspsResellerType, IUspsWebClient>>();
            repo.Setup(x => x(It.IsAny<UspsResellerType>())).Returns(webClient.Object);
            mock.Provide(repo.Object);

            GlobalPostAvailabilityService testObject = mock.Create<GlobalPostAvailabilityService>();
            testObject.Refresh();

            Assert.DoesNotContain(PostalServiceType.GlobalPostStandardIntl, testObject.Services);
            Assert.DoesNotContain(PostalServiceType.GlobalPostEconomyIntl, testObject.Services);

            Assert.Contains(PostalServiceType.GlobalPostSmartSaverEconomyIntl, testObject.Services);
            Assert.Contains(PostalServiceType.GlobalPostSmartSaverStandardIntl, testObject.Services);
        }

        [Fact]
        public void Refresh_DoesNotAddsGlobalPostServices_WhenAccountDoesNotSupportsGlobalPostServices()
        {
            UspsAccountEntity account = new UspsAccountEntity();
            AccountInfoV41 accountInfo = new AccountInfoV41
            {
                Capabilities = new CapabilitiesV30
                {
                    CanPrintGPSmartSaver = false,
                    CanPrintGP = false
                }
            };

            Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> accountRepo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();
            accountRepo.SetupGet(a => a.Accounts).Returns(new[] { account });

            Mock<IUspsWebClient> webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(w => w.GetAccountInfo(account)).Returns(accountInfo);

            Mock<Func<UspsResellerType, IUspsWebClient>> repo = mock.MockRepository.Create<Func<UspsResellerType, IUspsWebClient>>();
            repo.Setup(x => x(It.IsAny<UspsResellerType>())).Returns(webClient.Object);
            mock.Provide(repo.Object);

            GlobalPostAvailabilityService testObject = mock.Create<GlobalPostAvailabilityService>();
            testObject.Refresh();

            Assert.Empty(testObject.Services);
        }

        [Fact]
        public void InitializeForCurrentSession_AddsGlobalPostServices_WhenAccountSupportsGlobalPostServices()
        {
            UspsAccountEntity account = new UspsAccountEntity();
            AccountInfoV41 accountInfo = new AccountInfoV41
            {
                Capabilities = new CapabilitiesV30
                {
                    CanPrintGPSmartSaver = false,
                    CanPrintGP = true
                }
            };

            Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> accountRepo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();
            accountRepo.SetupGet(a => a.Accounts).Returns(new[] { account });

            Mock<IUspsWebClient> webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(w => w.GetAccountInfo(account)).Returns(accountInfo);

            Mock<Func<UspsResellerType, IUspsWebClient>> repo = mock.MockRepository.Create<Func<UspsResellerType, IUspsWebClient>>();
            repo.Setup(x => x(It.IsAny<UspsResellerType>())).Returns(webClient.Object);
            mock.Provide(repo.Object);

            GlobalPostAvailabilityService testObject = mock.Create<GlobalPostAvailabilityService>();
            testObject.InitializeForCurrentSession();

            Assert.Contains(PostalServiceType.GlobalPostStandardIntl, testObject.Services);
            Assert.Contains(PostalServiceType.GlobalPostEconomyIntl, testObject.Services);

            Assert.DoesNotContain(PostalServiceType.GlobalPostSmartSaverEconomyIntl, testObject.Services);
            Assert.DoesNotContain(PostalServiceType.GlobalPostSmartSaverStandardIntl, testObject.Services);
        }

        [Fact]
        public void InitializeForCurrentSession_AddsAllGlobalPostServices_WhenAccountSupportsAllGlobalPostServices()
        {
            UspsAccountEntity account = new UspsAccountEntity();
            AccountInfoV41 accountInfo = new AccountInfoV41
            {
                Capabilities = new CapabilitiesV30
                {
                    CanPrintGPSmartSaver = true,
                    CanPrintGP = true
                }
            };

            Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> accountRepo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();
            accountRepo.SetupGet(a => a.Accounts).Returns(new[] { account });

            Mock<IUspsWebClient> webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(w => w.GetAccountInfo(account)).Returns(accountInfo);

            Mock<Func<UspsResellerType, IUspsWebClient>> repo = mock.MockRepository.Create<Func<UspsResellerType, IUspsWebClient>>();
            repo.Setup(x => x(It.IsAny<UspsResellerType>())).Returns(webClient.Object);
            mock.Provide(repo.Object);

            GlobalPostAvailabilityService testObject = mock.Create<GlobalPostAvailabilityService>();
            testObject.InitializeForCurrentSession();

            Assert.Contains(PostalServiceType.GlobalPostStandardIntl, testObject.Services);
            Assert.Contains(PostalServiceType.GlobalPostEconomyIntl, testObject.Services);

            Assert.Contains(PostalServiceType.GlobalPostSmartSaverEconomyIntl, testObject.Services);
            Assert.Contains(PostalServiceType.GlobalPostSmartSaverStandardIntl, testObject.Services);
        }

        [Fact]
        public void InitializeForCurrentSession_AddsGlobalPostSmartSaverServices_WhenAccountSupportsGlobalPostSmartSaverServices()
        {
            UspsAccountEntity account = new UspsAccountEntity();
            AccountInfoV41 accountInfo = new AccountInfoV41
            {
                Capabilities = new CapabilitiesV30
                {
                    CanPrintGPSmartSaver = true,
                    CanPrintGP = false
                }
            };

            Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> accountRepo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();
            accountRepo.SetupGet(a => a.Accounts).Returns(new[] { account });

            Mock<IUspsWebClient> webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(w => w.GetAccountInfo(account)).Returns(accountInfo);

            Mock<Func<UspsResellerType, IUspsWebClient>> repo = mock.MockRepository.Create<Func<UspsResellerType, IUspsWebClient>>();
            repo.Setup(x => x(It.IsAny<UspsResellerType>())).Returns(webClient.Object);
            mock.Provide(repo.Object);

            GlobalPostAvailabilityService testObject = mock.Create<GlobalPostAvailabilityService>();
            testObject.InitializeForCurrentSession();

            Assert.DoesNotContain(PostalServiceType.GlobalPostStandardIntl, testObject.Services);
            Assert.DoesNotContain(PostalServiceType.GlobalPostEconomyIntl, testObject.Services);

            Assert.Contains(PostalServiceType.GlobalPostSmartSaverEconomyIntl, testObject.Services);
            Assert.Contains(PostalServiceType.GlobalPostSmartSaverStandardIntl, testObject.Services);
        }

        [Fact]
        public void InitializeForCurrentSession_DoesNotAddsGlobalPostServices_WhenAccountDoesNotSupportsGlobalPostServices()
        {
            UspsAccountEntity account = new UspsAccountEntity();
            AccountInfoV41 accountInfo = new AccountInfoV41
            {
                Capabilities = new CapabilitiesV30
                {
                    CanPrintGPSmartSaver = false,
                    CanPrintGP = false
                }
            };

            Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> accountRepo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();
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
        public void Refresh_LogsError_WhenUspsWebClientThrows()
        {
            UspsAccountEntity account = new UspsAccountEntity { GlobalPostAvailability = 0 };
            Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> accountRepo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();
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
            testObject.Refresh(account);

            log.Verify(l => l.Error("Error updating GlobalPostAvailability", ex));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}