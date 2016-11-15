﻿using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps
{
    public class UspsShipmentTypeTest
    {
        private readonly UspsShipmentType testObject;
        private readonly Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> accountRepository;
        private readonly List<PostalServicePackagingCombination> allCombinations = new List<PostalServicePackagingCombination>();
        private readonly List<PostalServicePackagingCombination> adultSignatureCombinationsAllowed = new List<PostalServicePackagingCombination>();

        public UspsShipmentTypeTest()
        {
            accountRepository = new Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();

            testObject = new UspsShipmentType();

            LoadAdultSignatureServiceAndPackagingCombinations();

            LoadAllPostalServicePackageTypes();
        }

        [Fact]
        public void GetShippingBroker_ReturnsNullShippingBroker_WhenNoUspsAccountsExist()
        {
            accountRepository.Setup(r => r.Accounts).Returns(new List<UspsAccountEntity>());
            testObject.AccountRepository = accountRepository.Object;

            IBestRateShippingBroker broker = testObject.GetShippingBroker(new ShipmentEntity());

            Assert.IsType<NullShippingBroker>(broker);
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

            Assert.IsType<UspsBestRateBroker>(broker);
        }

        [Fact]
        public void GetShippingBroker_ReturnsNullShippingBroker_WhenUspsAccountExists_AndPendingStatusIsCreate()
        {
            accountRepository.Setup(r => r.AccountsReadOnly)
                .Returns
                (
                    new List<UspsAccountEntity>
                    {
                        new UspsAccountEntity() { PendingInitialAccount = (int) UspsPendingAccountType.Create }
                    }
                );

            testObject.AccountRepository = accountRepository.Object;

            IBestRateShippingBroker broker = testObject.GetShippingBroker(new ShipmentEntity());

            Assert.IsType<NullShippingBroker>(broker);
        }

        [Fact]
        public void GetShippingBroker_ReturnsNullShippingBroker_WhenUspsAccountExists_AndPendingStatusIsExisting()
        {
            accountRepository.Setup(r => r.AccountsReadOnly)
                .Returns
                (
                    new List<UspsAccountEntity>
                    {
                        new UspsAccountEntity() { PendingInitialAccount = (int) UspsPendingAccountType.Existing }
                    }
                );

            testObject.AccountRepository = accountRepository.Object;

            IBestRateShippingBroker broker = testObject.GetShippingBroker(new ShipmentEntity());

            Assert.IsType<NullShippingBroker>(broker);
        }

        [Fact]
        public void AdultSignatureRequred_IsReturned_WhenUspsShipmentTypeAndUsingAllowedCombinations()
        {
            UspsShipmentType uspsShipmentType = new UspsShipmentType();

            foreach (PostalServicePackagingCombination combo in adultSignatureCombinationsAllowed)
            {
                Assert.Contains(PostalConfirmationType.AdultSignatureRequired, uspsShipmentType.GetAvailableConfirmationTypes("US", combo.ServiceType, combo.PackagingType));
            }
        }

        [Fact]
        public void AdultSignatureRestricted_IsReturned_WhenUspsShipmentTypeAndUsingAllowedCombinations()
        {
            UspsShipmentType uspsShipmentType = new UspsShipmentType();

            foreach (PostalServicePackagingCombination combo in adultSignatureCombinationsAllowed)
            {
                Assert.Contains(PostalConfirmationType.AdultSignatureRestricted, uspsShipmentType.GetAvailableConfirmationTypes("US", combo.ServiceType, combo.PackagingType));
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
            }
        }

        [Theory]
        [InlineData(PostalServiceType.GlobalPostSmartSaverEconomy, "USPS GlobalPost SmartSaver Economy")]
        [InlineData(PostalServiceType.GlobalPostEconomy, "USPS GlobalPost Economy")]
        [InlineData(PostalServiceType.GlobalPostSmartSaverPriority, "USPS GlobalPost SmartSaver Priority")]
        [InlineData(PostalServiceType.GlobalPostPriority, "USPS GlobalPost Priority")]
        [InlineData(PostalServiceType.InternationalFirst, "USPS International First")]
        [InlineData(PostalServiceType.InternationalPriority, "USPS International Priority")]
        [InlineData(PostalServiceType.FirstClass, "USPS First Class")]
        public void GetServiceDescription_ReturnsExpectedDescription(PostalServiceType serviceType, string expectedServiceDescription)
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity() { Postal = new PostalShipmentEntity() { Service = (int) serviceType } };

            Assert.Equal(expectedServiceDescription, testObject.GetServiceDescription(shipmentEntity));
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
