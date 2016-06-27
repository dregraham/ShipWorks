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
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.BestRate;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Endicia
{
    public class EndiciaShipmentTypeTest
    {
        private readonly EndiciaShipmentType testObject;
        private readonly Mock<ICarrierAccountRepository<EndiciaAccountEntity>> accountRepository;
        private readonly List<PostalServicePackagingCombination> allCombinations = new List<PostalServicePackagingCombination>();
        private readonly List<PostalServicePackagingCombination> adultSignatureCombinationsAllowed = new List<PostalServicePackagingCombination>();

        public EndiciaShipmentTypeTest()
        {
            accountRepository = new Mock<ICarrierAccountRepository<EndiciaAccountEntity>>();

            testObject = new EndiciaShipmentType();

            LoadAdultSignatureServiceAndPackagingCombinations();

            LoadAllPostalServicePackageTypes();
        }

        [Fact]
        public void GetShippingBroker_ReturnsNullShippingBroker_WhenNoEndiciaAccountsExist()
        {
            accountRepository.Setup(r => r.Accounts).Returns(new List<EndiciaAccountEntity>());
            testObject.AccountRepository = accountRepository.Object;

            IBestRateShippingBroker broker = testObject.GetShippingBroker(new ShipmentEntity());

            Assert.IsType<NullShippingBroker>(broker);
        }

        [Fact]
        public void GetShippingBroker_ReturnsEndiciaShippingBroker_WhenEndiciaAccountsExist()
        {
            accountRepository.Setup(r => r.Accounts).Returns(new List<EndiciaAccountEntity>() {new EndiciaAccountEntity(1)});
            testObject.AccountRepository = accountRepository.Object;

            IBestRateShippingBroker broker = testObject.GetShippingBroker(new ShipmentEntity());

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

            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.CriticalMail, PostalPackagingType.LargeEnvelope));
        }
    }
}
