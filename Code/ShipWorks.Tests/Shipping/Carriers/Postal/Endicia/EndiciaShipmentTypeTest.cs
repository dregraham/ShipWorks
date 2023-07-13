﻿using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.BestRate;
using ShipWorks.Shipping.Services;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Endicia
{
    public class EndiciaShipmentTypeTest
    {
        private readonly EndiciaShipmentType testObject;
        private readonly Mock<ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity>> accountRepository;
        private readonly List<PostalServicePackagingCombination> allCombinations = new List<PostalServicePackagingCombination>();
        private readonly List<PostalServicePackagingCombination> adultSignatureCombinationsAllowed = new List<PostalServicePackagingCombination>();
        private readonly Mock<IBestRateExcludedAccountRepository> bestRateExcludedAccountRepositoryMock;

        public EndiciaShipmentTypeTest()
        {
            accountRepository = new Mock<ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity>>();

            testObject = new EndiciaShipmentType();

            LoadAdultSignatureServiceAndPackagingCombinations();

            LoadAllPostalServicePackageTypes();

            bestRateExcludedAccountRepositoryMock = new Mock<IBestRateExcludedAccountRepository>();
            bestRateExcludedAccountRepositoryMock.Setup(r => r.GetAll()).Returns(new List<long>());
        }

        [Fact]
        public void GetShippingBroker_ReturnsNullShippingBroker_WhenNoEndiciaAccountsExist()
        {
            accountRepository.Setup(r => r.Accounts).Returns(new List<EndiciaAccountEntity>());
            testObject.AccountRepository = accountRepository.Object;

            IBestRateShippingBroker broker = testObject.GetShippingBroker(new ShipmentEntity(), bestRateExcludedAccountRepositoryMock.Object);

            Assert.IsType<NullShippingBroker>(broker);
        }

        [Fact]
        public void GetShippingBroker_ReturnsEndiciaShippingBroker_WhenEndiciaAccountsExist()
        {
            accountRepository.Setup(r => r.AccountsReadOnly).Returns(new List<EndiciaAccountEntity>() { new EndiciaAccountEntity(1) });
            testObject.AccountRepository = accountRepository.Object;

            IBestRateShippingBroker broker = testObject.GetShippingBroker(new ShipmentEntity(), bestRateExcludedAccountRepositoryMock.Object);

            Assert.IsType<EndiciaBestRateBroker>(broker);
        }

        [Fact]
        public void AdultSignatureRequred_IsReturned_WhenEndiciaShipmentTypeAndUsingAllowedCombinations()
        {
            EndiciaShipmentType EndiciaShipmentType = new EndiciaShipmentType();

            foreach (PostalServicePackagingCombination combo in adultSignatureCombinationsAllowed)
            {
                Assert.Contains(PostalConfirmationType.AdultSignatureRequired, EndiciaShipmentType.GetAvailableConfirmationTypes("US", combo.ServiceType, combo.PackagingType));
            }
        }

        [Fact]
        public void AdultSignatureRestricted_IsReturned_WhenEndiciaShipmentTypeAndUsingAllowedCombinations()
        {
            EndiciaShipmentType EndiciaShipmentType = new EndiciaShipmentType();

            foreach (PostalServicePackagingCombination combo in adultSignatureCombinationsAllowed)
            {
                Assert.Contains(PostalConfirmationType.AdultSignatureRestricted, EndiciaShipmentType.GetAvailableConfirmationTypes("US", combo.ServiceType, combo.PackagingType));
            }
        }

        [Fact]
        public void AdultSignatureRequred_IsNotReturned_WhenEndiciaShipmentTypeAndUsingCombinationsThatAreNotAllowed()
        {
            EndiciaShipmentType EndiciaShipmentType = new EndiciaShipmentType();

            foreach (PostalServicePackagingCombination combo in allCombinations.Except(adultSignatureCombinationsAllowed))
            {
                List<PostalConfirmationType> returnedConfirmationTypes = EndiciaShipmentType.GetAvailableConfirmationTypes("US", combo.ServiceType, combo.PackagingType);

                Assert.DoesNotContain(PostalConfirmationType.AdultSignatureRequired, returnedConfirmationTypes);
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

            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.GroundAdvantage, PostalPackagingType.Package));

            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.CriticalMail, PostalPackagingType.LargeEnvelope));
        }

        [Theory]
        [InlineData(true, 9.99)]
        [InlineData(false, 6.66)]
        public void Insured_ReturnsInsuranceFromShipment(bool insured, decimal insuranceValue)
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                Insurance = !insured,
                Postal = new PostalShipmentEntity
                {
                    Insurance = !insured,
                    InsuranceValue = insuranceValue,
                    Endicia = new EndiciaShipmentEntity { Insurance = insured}
                }
            };

            ShipmentParcel parcel = new EndiciaShipmentType().GetParcelDetail(shipment, 0);

            Assert.Equal(insured, parcel.Insurance.Insured);
            Assert.Equal(insuranceValue, parcel.Insurance.InsuranceValue);
        }

        [Fact]
        public void GetPackageAdapters_ReturnEndiciaValues()
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                Insurance = false,
                Postal = new PostalShipmentEntity
                {
                    Insurance = false,
                    InsuranceValue = 3,
                    Endicia = new EndiciaShipmentEntity { Insurance = true }
                }
            };

            IEnumerable<IPackageAdapter> packageAdapters = testObject.GetPackageAdapters(shipment);
            Assert.True(packageAdapters.First().InsuranceChoice.Insured);

            shipment.Insurance = true;
            shipment.Postal.Insurance = true;
            shipment.Postal.Endicia.Insurance = false;
            packageAdapters = testObject.GetPackageAdapters(shipment);
            Assert.False(packageAdapters.First().InsuranceChoice.Insured);
        }
    }
}
