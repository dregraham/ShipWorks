using System.Collections.Generic;
using System.Linq;
using Moq;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Postal
{
    public class RegistrationPromotionFactoryTest
    {
        MockRepository mock;
        Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> uspsRepository;
        Mock<ICarrierAccountRetriever> uspsExpress1Repository;
        Mock<ICarrierAccountRetriever> endiciaRepository;
        Mock<ICarrierAccountRetriever> endiciaExpress1Repository;
        RegistrationPromotionFactory factory;

        public RegistrationPromotionFactoryTest()
        {
            mock = new MockRepository(MockBehavior.Loose);

            uspsRepository = mock.Create<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();
            uspsExpress1Repository = mock.Create<ICarrierAccountRetriever>();
            endiciaRepository = mock.Create<ICarrierAccountRetriever>();
            endiciaExpress1Repository = mock.Create<ICarrierAccountRetriever>();

            factory = new RegistrationPromotionFactory(uspsRepository.Object, uspsExpress1Repository.Object,
                endiciaRepository.Object, endiciaExpress1Repository.Object);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsExpress1RegistrationPromotion_WhenNoPostalAccountsExist()
        {
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<Express1RegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsExpress1RegistrationPromotion_WhenOnlyUspsExpress1Exists()
        {
            ReturnAccounts(uspsExpress1Repository, 1);

            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<Express1RegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsExpress1RegistrationPromotion_WhenOnlyEndiciaExpress1Exists()
        {
            ReturnAccounts(endiciaExpress1Repository, 1);

            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<Express1RegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsExpress1RegistrationPromotion_WhenBothUspsAndEndiciaExpress1Exists()
        {
            ReturnAccounts(uspsExpress1Repository, 1);
            ReturnAccounts(endiciaExpress1Repository, 1);

            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<Express1RegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsExpress1RegistrationPromotion_WhenOnlyEndiciaAccountsExist()
        {
            ReturnAccounts(endiciaRepository, 1);

            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<Express1RegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsExpress1RegistrationPromotion_WhenEndiciaAndEndiciaExpress1AccountsExist()
        {
            ReturnAccounts(endiciaRepository, 1);
            ReturnAccounts(endiciaExpress1Repository, 1);

            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<Express1RegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsExpress1RegistrationPromotion_WhenEndiciaAndUspsExpress1AccountsExist()
        {
            ReturnAccounts(endiciaRepository, 1);
            ReturnAccounts(uspsExpress1Repository, 1);

            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<Express1RegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsEndiciaCbpRegistrationPromotion_WhenEndiciaAccountAndUspsResellerAccountsExist()
        {
            ReturnAccounts(uspsRepository, new[]
            {
                new UspsAccountEntity
                {
                    ContractType = (int) UspsAccountContractType.Reseller
                }
            });

            ReturnAccounts(endiciaRepository, 1);

            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<EndiciaCbpRegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsExpress1RegistrationPromotion_WhenOnlyNonResellerUspsAccountsExist()
        {
            ReturnAccounts(uspsRepository, new[] { new UspsAccountEntity() });

            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<Express1RegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsUspsCbpRegistrationPromotion_WhenOnlyUspsAccountsExistAndIncludesReseller()
        {
            ReturnAccounts(uspsRepository, new[]
            {
                new UspsAccountEntity
                {
                    ContractType = (int) UspsAccountContractType.CommercialPlus
                },
                new UspsAccountEntity
                {
                    ContractType = (int) UspsAccountContractType.Reseller
                }
            });

            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<UspsCbpRegistrationPromotion>(promotion);
        }

        private void ReturnAccounts(Mock<ICarrierAccountRetriever> repo, int count)
        {
            repo.Setup(x => x.AccountsReadOnly)
                .Returns(Enumerable.Repeat(mock.Create<ICarrierAccount>().Object, count));
        }

        private void ReturnAccounts(Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> repo, IEnumerable<IUspsAccountEntity> accountList)
        {
            repo.Setup(x => x.AccountsReadOnly).Returns(accountList.OfType<IUspsAccountEntity>());
        }
    }
}
