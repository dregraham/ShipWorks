using System.Collections.Generic;
using Xunit;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;

namespace ShipWorks.Tests.Shipping.Carriers.Postal
{
    public class RegistrationPromotionFactoryTest
    {
        MockRepository mockRepository;
        Mock<ICarrierAccountRepository<UspsAccountEntity>> uspsRepository;
        Mock<ICarrierAccountRepository<UspsAccountEntity>> uspsExpress1Repository;
        Mock<ICarrierAccountRepository<EndiciaAccountEntity>> endiciaRepository;
        Mock<ICarrierAccountRepository<EndiciaAccountEntity>> endiciaExpress1Repository;

        public RegistrationPromotionFactoryTest()
        {
            mockRepository = new MockRepository(MockBehavior.Loose);
            uspsRepository = CreateEmptyRepository<UspsAccountEntity>();
            uspsExpress1Repository = CreateEmptyRepository<UspsAccountEntity>();
            endiciaRepository = CreateEmptyRepository<EndiciaAccountEntity>();
            endiciaExpress1Repository = CreateEmptyRepository<EndiciaAccountEntity>();
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
            uspsExpress1Repository = CreateRepositoryWithAccounts(new List<UspsAccountEntity> { new UspsAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<Express1RegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsExpress1RegistrationPromotion_WhenOnlyEndiciaExpress1Exists()
        {
            endiciaExpress1Repository = CreateRepositoryWithAccounts(new List<EndiciaAccountEntity> { new EndiciaAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<Express1RegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsExpress1RegistrationPromotion_WhenBothUspsAndEndiciaExpress1Exists()
        {
            uspsExpress1Repository = CreateRepositoryWithAccounts(new List<UspsAccountEntity> { new UspsAccountEntity() });
            endiciaExpress1Repository = CreateRepositoryWithAccounts(new List<EndiciaAccountEntity> { new EndiciaAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<Express1RegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsExpress1RegistrationPromotion_WhenOnlyEndiciaAccountsExist()
        {
            endiciaRepository = CreateRepositoryWithAccounts(new List<EndiciaAccountEntity> { new EndiciaAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<Express1RegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsExpress1RegistrationPromotion_WhenEndiciaAndEndiciaExpress1AccountsExist()
        {
            endiciaRepository = CreateRepositoryWithAccounts(new List<EndiciaAccountEntity> { new EndiciaAccountEntity() });
            endiciaExpress1Repository = CreateRepositoryWithAccounts(new List<EndiciaAccountEntity> { new EndiciaAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<Express1RegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsExpress1RegistrationPromotion_WhenEndiciaAndUspsExpress1AccountsExist()
        {
            endiciaRepository = CreateRepositoryWithAccounts(new List<EndiciaAccountEntity> { new EndiciaAccountEntity() });
            uspsExpress1Repository = CreateRepositoryWithAccounts(new List<UspsAccountEntity> { new UspsAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<Express1RegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsEndiciaCbpRegistrationPromotion_WhenEndiciaAccountAndUspsResellerAccountsExist()
        {
            uspsRepository = CreateRepositoryWithAccounts(new List<UspsAccountEntity>
            {
                new UspsAccountEntity
                {
                    ContractType = (int) UspsAccountContractType.Reseller
                }
            });
            endiciaRepository = CreateRepositoryWithAccounts(new List<EndiciaAccountEntity> { new EndiciaAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<EndiciaCbpRegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsExpress1RegistrationPromotion_WhenOnlyNonResellerUspsAccountsExist()
        {
            uspsRepository = CreateRepositoryWithAccounts(new List<UspsAccountEntity> { new UspsAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsAssignableFrom<Express1RegistrationPromotion>(promotion);
        }

        [Fact]
        public void CreateRegistrationPromotion_ReturnsUspsCbpRegistrationPromotion_WhenOnlyUspsAccountsExistAndIncludesReseller()
        {
            uspsRepository = CreateRepositoryWithAccounts(new List<UspsAccountEntity>
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
        private Mock<ICarrierAccountRepository<T>> CreateEmptyRepository<T>() where T : ICarrierAccount
        {
            return CreateRepositoryWithAccounts(new List<T>());
        }

        /// <summary>
        /// Create a repository that has the accounts in the given list
        /// </summary>
        private Mock<ICarrierAccountRepository<T>> CreateRepositoryWithAccounts<T>(IEnumerable<T> accountList) where T : ICarrierAccount
        {
            Mock<ICarrierAccountRepository<T>> repo = mockRepository.Create<ICarrierAccountRepository<T>>();
            repo.Setup(x => x.Accounts).Returns(accountList);
            return repo;
        }
    }
}
