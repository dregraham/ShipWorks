using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;
using ShipWorks.Shipping.Services;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps
{
    public class UspsShipmentTypeTest
    {
        private readonly UspsShipmentType testObject;
        private readonly Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> accountRepository;
        private readonly List<PostalServicePackagingCombination> allCombinations = new List<PostalServicePackagingCombination>();
        private readonly List<PostalServicePackagingCombination> adultSignatureCombinationsAllowed = new List<PostalServicePackagingCombination>();
        private readonly Mock<IBestRateExcludedAccountRepository> bestRateExcludedAccountRepositoryMock;

        public UspsShipmentTypeTest()
        {
            accountRepository = new Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();

            testObject = new UspsShipmentType();

            LoadAdultSignatureServiceAndPackagingCombinations();

            LoadAllPostalServicePackageTypes();

            bestRateExcludedAccountRepositoryMock = new Mock<IBestRateExcludedAccountRepository>();
            bestRateExcludedAccountRepositoryMock.Setup(r => r.GetAll()).Returns(new List<long>());
        }

        [Fact]
        public void GetShippingBroker_ReturnsNullShippingBroker_WhenNoUspsAccountsExist()
        {
            accountRepository.Setup(r => r.Accounts).Returns(new List<UspsAccountEntity>());
            testObject.AccountRepository = accountRepository.Object;

            IBestRateShippingBroker broker = testObject.GetShippingBroker(new ShipmentEntity(), bestRateExcludedAccountRepositoryMock.Object);

            Assert.IsType<NullShippingBroker>(broker);
        }

        [Fact]
        public void GetShippingBroker_ReturnsUspsRateBroker_WhenUspsAccountExists()
        {
            accountRepository.Setup(r => r.AccountsReadOnly).Returns(new[] { new UspsAccountEntity() });

            testObject.AccountRepository = accountRepository.Object;

            IBestRateShippingBroker broker = testObject.GetShippingBroker(new ShipmentEntity(), bestRateExcludedAccountRepositoryMock.Object);

            Assert.IsType<UspsBestRateBroker>(broker);
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
        [InlineData(PostalServiceType.GlobalPostSmartSaverEconomyIntl, "USPS GlobalPost SmartSaver Economy Intl")]
        [InlineData(PostalServiceType.GlobalPostEconomyIntl, "USPS GlobalPost Economy Intl")]
        [InlineData(PostalServiceType.GlobalPostSmartSaverStandardIntl, "USPS GlobalPost SmartSaver Standard Intl")]
        [InlineData(PostalServiceType.GlobalPostStandardIntl, "USPS GlobalPost Standard Intl")]
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
                    Usps = new UspsShipmentEntity { Insurance = insured }
                }
            };

            ShipmentParcel parcel = new UspsShipmentType().GetParcelDetail(shipment, 0);

            Assert.Equal(insured, parcel.Insurance.Insured);
            Assert.Equal(insuranceValue, parcel.Insurance.InsuranceValue);
        }

        [Fact]
        public void GetPackageAdapters_ReturnUspsValues()
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                Insurance = false,
                Postal = new PostalShipmentEntity
                {
                    Insurance = false,
                    InsuranceValue = 3,
                    Usps = new UspsShipmentEntity { Insurance = true }
                }
            };

            IEnumerable<IPackageAdapter> packageAdapters = testObject.GetPackageAdapters(shipment);
            Assert.True(packageAdapters.First().InsuranceChoice.Insured);

            shipment.Insurance = true;
            shipment.Postal.Insurance = true;
            shipment.Postal.Usps.Insurance = false;
            packageAdapters = testObject.GetPackageAdapters(shipment);
            Assert.False(packageAdapters.First().InsuranceChoice.Insured);
        }

        [Theory]
        [InlineData(PostalServiceType.ExpressMail, "", true, "")]
        [InlineData(PostalServiceType.ExpressMail,"foo", false, "")]
        [InlineData(PostalServiceType.ExpressMail, "foo", true, "https://tools.usps.com/go/TrackConfirmAction.action?tLabels=foo")]
        [InlineData(PostalServiceType.DhlParcelPlusGround, "foo", true, "http://webtrack.dhlglobalmail.com/?mobile=&amp;trackingnumber=foo")]
        public void GetCarrierTrackingUrl_ReturnsCorrectTrackingUrl(PostalServiceType serviceType, string trackingNumber, bool processed, string expectedUrl)
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                Postal = new PostalShipmentEntity
                {
                    Service = (int) serviceType
                },
                TrackingNumber = trackingNumber,
                Processed = processed
            };

            var trackingUrl = testObject.GetCarrierTrackingUrl(shipment);
            Assert.Equal(expectedUrl, trackingUrl);
        }
    }
}
