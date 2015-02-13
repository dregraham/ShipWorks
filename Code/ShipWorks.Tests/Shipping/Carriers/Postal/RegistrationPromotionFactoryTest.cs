using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration.Promotion;
using ShipWorks.Stores.Platforms.Amazon.WebServices.Associates;

namespace ShipWorks.Tests.Shipping.Carriers.Postal
{
    [TestClass]
    public class RegistrationPromotionFactoryTest
    {
        MockRepository mockRepository;
        Mock<ICarrierAccountRepository<UspsAccountEntity>> uspsRepository;
        Mock<ICarrierAccountRepository<UspsAccountEntity>> stampsRepository;
        Mock<ICarrierAccountRepository<UspsAccountEntity>> stampsExpress1Repository;
        Mock<ICarrierAccountRepository<EndiciaAccountEntity>> endiciaRepository;
        Mock<ICarrierAccountRepository<EndiciaAccountEntity>> endiciaExpress1Repository;

        [TestInitialize]
        public void Setup()
        {
            mockRepository = new MockRepository(MockBehavior.Loose);
            uspsRepository = CreateEmptyRepository<UspsAccountEntity>();
            stampsRepository = CreateEmptyRepository<UspsAccountEntity>();
            stampsExpress1Repository = CreateEmptyRepository<UspsAccountEntity>();
            endiciaRepository = CreateEmptyRepository<EndiciaAccountEntity>();
            endiciaExpress1Repository = CreateEmptyRepository<EndiciaAccountEntity>();
        }

        [TestMethod]
        public void CreateRegistrationPromotion_ReturnsNewPostalCustomerRegistrationPromotion_WhenNoPostalAccountsExist()
        {
            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsInstanceOfType(promotion, typeof(NewPostalCustomerRegistrationPromotion));
        }

        [TestMethod]
        public void CreateRegistrationPromotion_ReturnsExpress1OnlyRegistrationPromotion_WhenOnlyStampsExpress1Exists()
        {
            stampsExpress1Repository = CreateRepositoryWithAccounts(new List<UspsAccountEntity> { new UspsAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsInstanceOfType(promotion, typeof(Express1OnlyRegistrationPromotion));
        }

        [TestMethod]
        public void CreateRegistrationPromotion_ReturnsExpress1OnlyRegistrationPromotion_WhenOnlyEndiciaExpress1Exists()
        {
            endiciaExpress1Repository = CreateRepositoryWithAccounts(new List<EndiciaAccountEntity> { new EndiciaAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsInstanceOfType(promotion, typeof(Express1OnlyRegistrationPromotion));
        }

        [TestMethod]
        public void CreateRegistrationPromotion_ReturnsExpress1OnlyRegistrationPromotion_WhenBothStampsAndEndiciaExpress1Exists()
        {
            stampsExpress1Repository = CreateRepositoryWithAccounts(new List<UspsAccountEntity> { new UspsAccountEntity() });
            endiciaExpress1Repository = CreateRepositoryWithAccounts(new List<EndiciaAccountEntity> { new EndiciaAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsInstanceOfType(promotion, typeof(Express1OnlyRegistrationPromotion));
        }

        [TestMethod]
        public void CreateRegistrationPromotion_ReturnsEndiciaRegistrationPromotion_WhenOnlyEndiciaAccountsExist()
        {
            endiciaRepository = CreateRepositoryWithAccounts(new List<EndiciaAccountEntity> { new EndiciaAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsInstanceOfType(promotion, typeof(EndiciaIntuishipRegistrationPromotion)); 
        }

        [TestMethod]
        public void CreateRegistrationPromotion_ReturnsEndiciaRegistrationPromotion_WhenEndiciaAndEndiciaExpress1AccountsExist()
        {
            endiciaRepository = CreateRepositoryWithAccounts(new List<EndiciaAccountEntity> { new EndiciaAccountEntity() });
            endiciaExpress1Repository = CreateRepositoryWithAccounts(new List<EndiciaAccountEntity> { new EndiciaAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsInstanceOfType(promotion, typeof(EndiciaIntuishipRegistrationPromotion));
        }

        [TestMethod]
        public void CreateRegistrationPromotion_ReturnsEndiciaRegistrationPromotion_WhenEndiciaAndStampsExpress1AccountsExist()
        {
            endiciaRepository = CreateRepositoryWithAccounts(new List<EndiciaAccountEntity> { new EndiciaAccountEntity() });
            stampsExpress1Repository = CreateRepositoryWithAccounts(new List<UspsAccountEntity> { new UspsAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsInstanceOfType(promotion, typeof(EndiciaIntuishipRegistrationPromotion));
        }

        [TestMethod]
        public void CreateRegistrationPromotion_ReturnsEndiciaCbpRegistrationPromotion_WhenEndiciaAccountAndUspsResellerAccountsExist()
        {
            uspsRepository = CreateRepositoryWithAccounts(new List<UspsAccountEntity>
            {
                new UspsAccountEntity
                {
                    ContractType = (int) StampsAccountContractType.Reseller
                }
            });
            endiciaRepository = CreateRepositoryWithAccounts(new List<EndiciaAccountEntity> { new EndiciaAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsInstanceOfType(promotion, typeof(EndiciaCbpRegistrationPromotion));
        }

        [TestMethod]
        public void CreateRegistrationPromotion_ReturnsStampsIntuishipRegistrationPromotion_WhenOnlyNonResellerUspsAccountsExist()
        {
            uspsRepository = CreateRepositoryWithAccounts(new List<UspsAccountEntity> { new UspsAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsInstanceOfType(promotion, typeof(StampsIntuishipRegistrationPromotion));
        }

        [TestMethod]
        public void CreateRegistrationPromotion_ReturnsStampsCbpRegistrationPromotion_WhenOnlyUspsAccountsExistAndIncludesReseller()
        {
            uspsRepository = CreateRepositoryWithAccounts(new List<UspsAccountEntity>
            {
                new UspsAccountEntity
                {
                    ContractType = (int) StampsAccountContractType.CommercialPlus
                },
                new UspsAccountEntity
                {
                    ContractType = (int) StampsAccountContractType.Reseller
                }
            });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsInstanceOfType(promotion, typeof(StampsCbpRegistrationPromotion));
        }

        [TestMethod]
        public void CreateRegistrationPromotion_ReturnsStampsIntuishipRegistrationPromotion_WhenOnlyNonResellerStampsAccountsExist()
        {
            stampsRepository = CreateRepositoryWithAccounts(new List<UspsAccountEntity> { new UspsAccountEntity() });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsInstanceOfType(promotion, typeof(StampsIntuishipRegistrationPromotion));
        }

        [TestMethod]
        public void CreateRegistrationPromotion_ReturnsStampsCbpRegistrationPromotion_WhenOnlyStampsAccountsExistAndIncludesReseller()
        {
            stampsRepository = CreateRepositoryWithAccounts(new List<UspsAccountEntity>
            {
                new UspsAccountEntity
                {
                    ContractType = (int) StampsAccountContractType.CommercialPlus
                },
                new UspsAccountEntity
                {
                    ContractType = (int) StampsAccountContractType.Reseller
                }
            });

            RegistrationPromotionFactory factory = CreateRegistrationPromotionFactory();
            IRegistrationPromotion promotion = factory.CreateRegistrationPromotion();
            Assert.IsInstanceOfType(promotion, typeof(StampsCbpRegistrationPromotion));
        }

        /// <summary>
        /// Create the registration promotion factory using the currently created repositories
        /// </summary>
        private RegistrationPromotionFactory CreateRegistrationPromotionFactory()
        {
            return new RegistrationPromotionFactory(uspsRepository.Object, stampsRepository.Object,
                stampsExpress1Repository.Object, endiciaRepository.Object, endiciaExpress1Repository.Object);
        }

        /// <summary>
        /// Create a repository with no accounts
        /// </summary>
        private Mock<ICarrierAccountRepository<T>> CreateEmptyRepository<T>() where T : IEntity2
        {
            return CreateRepositoryWithAccounts(new List<T>());
        }

        /// <summary>
        /// Create a repository that has the accounts in the given list
        /// </summary>
        private Mock<ICarrierAccountRepository<T>> CreateRepositoryWithAccounts<T>(IEnumerable<T> accountList) where T : IEntity2
        {
            Mock<ICarrierAccountRepository<T>> repo = mockRepository.Create<ICarrierAccountRepository<T>>();
            repo.Setup(x => x.Accounts).Returns(accountList);
            return repo;
        } 
    }
}
