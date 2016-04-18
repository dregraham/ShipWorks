using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using Xunit;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps
{
    public class UspsShipmentTypeTest
    {
        private UspsShipmentType testObject;
        private Mock<ICarrierAccountRepository<UspsAccountEntity>> accountRepository;

        private List<PostalServicePackagingCombination> allCombinations = new List<PostalServicePackagingCombination>();
        private List<PostalServicePackagingCombination> adultSignatureCombinationsNotAllowed = new List<PostalServicePackagingCombination>();
        private List<PostalServicePackagingCombination> adultSignatureCombinationsAllowed = new List<PostalServicePackagingCombination>();

        public UspsShipmentTypeTest()
        {
            accountRepository = new Mock<ICarrierAccountRepository<UspsAccountEntity>>();

            testObject = new UspsShipmentType();

            LoadAdultSignatureServiceAndPackagingCombinations();

            LoadAllPostalServicePackageTypes();

            adultSignatureCombinationsNotAllowed = allCombinations.Except(adultSignatureCombinationsAllowed).ToList();
        }

        [Fact]
        public void GetShippingBroker_ReturnsNullShippingBroker_WhenNoUspsAccountsExist()
        {
            accountRepository.Setup(r => r.Accounts).Returns(new List<UspsAccountEntity>());
            testObject.AccountRepository = accountRepository.Object;

            IBestRateShippingBroker broker = testObject.GetShippingBroker(new ShipmentEntity());

            Assert.IsAssignableFrom<NullShippingBroker>(broker);
        }

        [Fact]
        public void GetShippingBroker_ReturnsUspsRateBroker_WhenUspsAccountExists_AndPendingStatusIsNone()
        {
            accountRepository.Setup(r => r.Accounts)
                .Returns
                (
                    new List<UspsAccountEntity>
                    {
                        new UspsAccountEntity() { PendingInitialAccount = (int) UspsPendingAccountType.None }
                    }
                );

            testObject.AccountRepository = accountRepository.Object;

            IBestRateShippingBroker broker = testObject.GetShippingBroker(new ShipmentEntity());

            Assert.IsAssignableFrom<UspsBestRateBroker>(broker);
        }

        [Fact]
        public void GetShippingBroker_ReturnsNullShippingBroker_WhenUspsAccountExists_AndPendingStatusIsCreate()
        {
            accountRepository.Setup(r => r.Accounts)
                .Returns
                (
                    new List<UspsAccountEntity>
                    {
                        new UspsAccountEntity() { PendingInitialAccount = (int) UspsPendingAccountType.Create }
                    }
                );

            testObject.AccountRepository = accountRepository.Object;

            IBestRateShippingBroker broker = testObject.GetShippingBroker(new ShipmentEntity());

            Assert.IsAssignableFrom<NullShippingBroker>(broker);
        }

        [Fact]
        public void GetShippingBroker_ReturnsNullShippingBroker_WhenUspsAccountExists_AndPendingStatusIsExisting()
        {
            accountRepository.Setup(r => r.Accounts)
                .Returns
                (
                    new List<UspsAccountEntity>
                    {
                        new UspsAccountEntity() { PendingInitialAccount = (int) UspsPendingAccountType.Existing }
                    }
                );

            testObject.AccountRepository = accountRepository.Object;

            IBestRateShippingBroker broker = testObject.GetShippingBroker(new ShipmentEntity());

            Assert.IsAssignableFrom<NullShippingBroker>(broker);
        }

        [Fact]
        public void AdultSignatureRequred_IsReturned_WhenUspsShipmentTypeAndUsingAllowedCombinations()
        {
            UspsShipmentType uspsShipmentType = new UspsShipmentType();

            foreach (PostalServicePackagingCombination combo in adultSignatureCombinationsAllowed)
            {
                Assert.Contains(PostalConfirmationType.AdultSignatureRequired, uspsShipmentType.GetAvailableConfirmationTypes("US", combo.ServiceType, combo.PackagingType));
                //Assert.True(uspsShipmentType.GetAvailableConfirmationTypes("US", combo.ServiceType, combo.PackagingType).Any(ct => ct == PostalConfirmationType.AdultSignatureRequired), "{0}, {1} should be included in the allowed confirmation types", combo.ServiceType, combo.PackagingType);
            }
        }

        [Fact]
        public void AdultSignatureRestricted_IsReturned_WhenUspsShipmentTypeAndUsingAllowedCombinations()
        {
            UspsShipmentType uspsShipmentType = new UspsShipmentType();

            foreach (PostalServicePackagingCombination combo in adultSignatureCombinationsAllowed)
            {
                Assert.Contains(PostalConfirmationType.AdultSignatureRestricted, uspsShipmentType.GetAvailableConfirmationTypes("US", combo.ServiceType, combo.PackagingType));
                //Assert.True(uspsShipmentType.GetAvailableConfirmationTypes("US", combo.ServiceType, combo.PackagingType).Any(ct => ct == PostalConfirmationType.AdultSignatureRestricted), "{0}, {1} should be included in the allowed confirmation types", combo.ServiceType, combo.PackagingType);
            }
        }

        [Fact]
        public void AdultSignatureRequred_IsNotReturned_WhenUspsShipmentTypeAndUsingCombinationsThatAreNotAllowed()
        {
            UspsShipmentType uspsShipmentType = new UspsShipmentType();

            foreach (PostalServicePackagingCombination combo in allCombinations.Except(adultSignatureCombinationsAllowed))
            {
                List<PostalConfirmationType> returnedConfirmationTypes = uspsShipmentType.GetAvailableConfirmationTypes("US", combo.ServiceType, combo.PackagingType);

                Assert.DoesNotContain(PostalConfirmationType.AdultSignatureRequired, returnedConfirmationTypes);
                //Assert.True(returnedConfirmationTypes.All(ct => ct != PostalConfirmationType.AdultSignatureRequired), "AdultSignatureRequired should not have been returned for {0}, {1}.", combo.ServiceType, combo.PackagingType);
            }
        }

        private void LoadAllPostalServicePackageTypes()
        {
            allCombinations.Clear();
            foreach (var postalServiceType in EnumHelper.GetEnumList<PostalServiceType>())
            {
                foreach (var postalPackageType in EnumHelper.GetEnumList<PostalPackagingType>())
                {
                    allCombinations.Add(new PostalServicePackagingCombination(postalServiceType.Value, postalPackageType.Value));
                }
            }
        }

        /// <summary>
        /// Add adult signature restricted values
        /// </summary>
        private void LoadAdultSignatureServiceAndPackagingCombinations()
        {
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.LargeEnvelope));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.FlatRateEnvelope));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.Package));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.FlatRateSmallBox));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.FlatRateMediumBox));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.FlatRateLargeBox));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.FlatRatePaddedEnvelope));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.FlatRateLegalEnvelope));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.RateRegionalBoxA));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.RateRegionalBoxB));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.RateRegionalBoxC));

            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.ExpressMail, PostalPackagingType.LargeEnvelope));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.ExpressMail, PostalPackagingType.Package));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.ExpressMail, PostalPackagingType.FlatRateEnvelope));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.ExpressMail, PostalPackagingType.FlatRateMediumBox));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.ExpressMail, PostalPackagingType.FlatRatePaddedEnvelope));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.ExpressMail, PostalPackagingType.FlatRateLegalEnvelope));

            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.ParcelSelect, PostalPackagingType.Package));

            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.CriticalMail, PostalPackagingType.LargeEnvelope));
        }
    }
}
