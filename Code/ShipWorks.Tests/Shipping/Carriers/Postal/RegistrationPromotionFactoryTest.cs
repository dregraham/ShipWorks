using System.Collections.Generic;
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
        MockRepository mockRepository;
        Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> uspsRepository;
        Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> uspsExpress1Repository;
        Mock<ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity>> endiciaRepository;
        Mock<ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity>> endiciaExpress1Repository;

        public RegistrationPromotionFactoryTest()
        {
            mockRepository = new MockRepository(MockBehavior.Loose);
            uspsRepository = CreateEmptyRepository<UspsAccountEntity, IUspsAccountEntity>();
            uspsExpress1Repository = CreateEmptyRepository<UspsAccountEntity, IUspsAccountEntity>();
            endiciaRepository = CreateEmptyRepository<EndiciaAccountEntity, IEndiciaAccountEntity>();
            endiciaExpress1Repository = CreateEmptyRepository<EndiciaAccountEntity, IEndiciaAccountEntity>();
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsExpress1RegistrationPromotion_WhenNoPostalAccountsExist()
        {
            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<Express1RegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsExpress1RegistrationPromotion_WhenOnlyUspsExpress1Exists()
        {
            uspsExpress1Repository = CreateRepositoryWithAccounts<UspsAccountEntity, IUspsAccountEntity>(new List<UspsAccountEntity> { new UspsAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<Express1RegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsExpress1RegistrationPromotion_WhenOnlyEndiciaExpress1Exists()
        {
            endiciaExpress1Repository = CreateRepositoryWithAccounts<EndiciaAccountEntity, IEndiciaAccountEntity>(new List<EndiciaAccountEntity> { new EndiciaAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<Express1RegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsExpress1RegistrationPromotion_WhenBothUspsAndEndiciaExpress1Exists()
        {
            uspsExpress1Repository = CreateRepositoryWithAccounts<UspsAccountEntity, IUspsAccountEntity>(new List<UspsAccountEntity> { new UspsAccountEntity() });
            endiciaExpress1Repository = CreateRepositoryWithAccounts<EndiciaAccountEntity, IEndiciaAccountEntity>(new List<EndiciaAccountEntity> { new EndiciaAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<Express1RegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsExpress1RegistrationPromotion_WhenOnlyEndiciaAccountsExist()
        {
            endiciaRepository = CreateRepositoryWithAccounts<EndiciaAccountEntity, IEndiciaAccountEntity>(new List<EndiciaAccountEntity> { new EndiciaAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<Express1RegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsExpress1RegistrationPromotion_WhenEndiciaAndEndiciaExpress1AccountsExist()
        {
            endiciaRepository = CreateRepositoryWithAccounts<EndiciaAccountEntity, IEndiciaAccountEntity>(new List<EndiciaAccountEntity> { new EndiciaAccountEntity() });
            endiciaExpress1Repository = CreateRepositoryWithAccounts<EndiciaAccountEntity, IEndiciaAccountEntity>(new List<EndiciaAccountEntity> { new EndiciaAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<Express1RegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsExpress1RegistrationPromotion_WhenEndiciaAndUspsExpress1AccountsExist()
        {
            endiciaRepository = CreateRepositoryWithAccounts<EndiciaAccountEntity, IEndiciaAccountEntity>(new List<EndiciaAccountEntity> { new EndiciaAccountEntity() });
            uspsExpress1Repository = CreateRepositoryWithAccounts<UspsAccountEntity, IUspsAccountEntity>(new List<UspsAccountEntity> { new UspsAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<Express1RegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsEndiciaCbpRegistrationPromotion_WhenEndiciaAccountAndUspsResellerAccountsExist()
        {
            uspsRepository = CreateRepositoryWithAccounts<UspsAccountEntity, IUspsAccountEntity>(new List<UspsAccountEntity>
            {
                new UspsAccountEntity
                {
                    ContractType = (int) UspsAccountContractType.Reseller
                }
            });
            endiciaRepository = CreateRepositoryWithAccounts<EndiciaAccountEntity, IEndiciaAccountEntity>(new List<EndiciaAccountEntity> { new EndiciaAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<EndiciaCbpRegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsExpress1RegistrationPromotion_WhenOnlyNonResellerUspsAccountsExist()
        {
            uspsRepository = CreateRepositoryWithAccounts<UspsAccountEntity, IUspsAccountEntity>(new List<UspsAccountEntity> { new UspsAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<Express1RegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsUspsCbpRegistrationPromotion_WhenOnlyUspsAccountsExistAndIncludesReseller()
        {
            uspsRepository = CreateRepositoryWithAccounts<UspsAccountEntity, IUspsAccountEntity>(new List<UspsAccountEntity>
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

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<UspsCbpRegistrationPromotion>(promotion);
        }

        /// <summary>
        /// Create the registration promotion factory using the currently created repositories
        /// </summary>
        private RegistrationPromotionFactory CreateRegistrationPromotionFactory()
        {
            return new RegistrationPromotionFactory(uspsRepository.Object, uspsExpress1Repository.Object,
                endiciaRepository.Object, endiciaExpress1Repository.Object);
        }

        /// <summary>
        /// Create a repository with no accounts
        /// </summary>
        private Mock<ICarrierAccountRepository<T, TInterface>> CreateEmptyRepository<T, TInterface>()
            where T : TInterface
            where TInterface : ICarrierAccount
        {
            return CreateRepositoryWithAccounts<T, TInterface>(new List<T>());
        }

        /// <summary>
        /// Create a repository that has the accounts in the given list
        /// </summary>
        private Mock<ICarrierAccountRepository<T, TInterface>> CreateRepositoryWithAccounts<T, TInterface>(IEnumerable<T> accountList)
            where T : TInterface
            where TInterface : ICarrierAccount
        {
            Mock<ICarrierAccountRepository<T, TInterface>> repo = mockRepository.Create<ICarrierAccountRepository<T, TInterface>>();
            repo.Setup(x => x.Accounts).Returns(accountList);
            return repo;
        }
    }
}
