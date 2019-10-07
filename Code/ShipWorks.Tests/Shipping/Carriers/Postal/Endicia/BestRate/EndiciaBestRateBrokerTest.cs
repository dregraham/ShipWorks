﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.BestRate;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Endicia.BestRate
{
    public class EndiciaBestRateBrokerTest
    {
        private readonly EndiciaAccountEntity account1;
        private readonly EndiciaAccountEntity account2;
        private readonly EndiciaAccountEntity account3;
        private RateGroup rateGroup1;
        private RateGroup rateGroup2;
        private RateGroup rateGroup3;
        private RateResult account1Rate1;
        private RateResult account1Rate2;
        private RateResult account1Rate3;
        private readonly RateResult account2Rate1;
        private readonly RateResult account3Rate1;
        private readonly RateResult account3Rate2;
        private Mock<IBestRateBrokerSettings> bestRateBrokerSettings;
        private EndiciaBestRateBroker testObject;
        private ShipmentEntity testShipment;

        /// <summary>
        /// This holds a collection of all the shipment objects that were passed into the EndiciaShipmentType.GetRates method
        /// </summary>
        private List<ShipmentEntity> getRatesShipments;

        private Mock<ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity>> genericRepositoryMock;
        private Mock<EndiciaShipmentType> genericShipmentTypeMock;
        private Dictionary<long, RateGroup> rateResults;

        private int timesGetRatesCalled = 0;

        public EndiciaBestRateBrokerTest()
        {
            account1 = new EndiciaAccountEntity { EndiciaAccountID = 1, CountryCode = "US" };
            account2 = new EndiciaAccountEntity { EndiciaAccountID = 2, CountryCode = "US" };
            account3 = new EndiciaAccountEntity { EndiciaAccountID = 3, CountryCode = "US" };

            account1Rate1 = new RateResult("Account 1a", "4", 12, new PostalRateSelection(PostalServiceType.PriorityMail)) { ServiceLevel = ServiceLevelType.TwoDays };
            account1Rate2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost)) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            account1Rate3 = new RateResult("Account 1c", "1", 15, new PostalRateSelection(PostalServiceType.ExpressMailPremium)) { ServiceLevel = ServiceLevelType.OneDay };
            account2Rate1 = new RateResult("* No rates were returned for the selected Service.", "");
            account3Rate1 = new RateResult("Account 3a", "4", 3, new PostalRateSelection(PostalServiceType.ParcelSelect)) { ServiceLevel = ServiceLevelType.Anytime };
            account3Rate2 = new RateResult("Account 3b", "3", 10, new PostalRateSelection(PostalServiceType.FirstClass)) { ServiceLevel = ServiceLevelType.TwoDays };

            rateGroup1 = new RateGroup(new[] { account1Rate1, account1Rate2, account1Rate3 });
            rateGroup2 = new RateGroup(new[] { account2Rate1 });
            rateGroup3 = new RateGroup(new[] { account3Rate1, account3Rate2 });

            rateResults = new Dictionary<long, RateGroup>
                {
                    {1, rateGroup1},
                    {2, rateGroup2},
                    {3, rateGroup3},
                };

            genericRepositoryMock = new Mock<ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity>>();
            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<EndiciaAccountEntity> { account1, account2, account3 });
            genericRepositoryMock.Setup(r => r.DefaultProfileAccount).Returns(account1);

            getRatesShipments = new List<ShipmentEntity>();

            // Save a copy of all the shipment entities passed into the GetRates method so we can inspect them later
            genericShipmentTypeMock = new Mock<EndiciaShipmentType>();
            genericShipmentTypeMock.Setup(x => x.ShipmentTypeCode).Returns(ShipmentTypeCode.Endicia);

            // Mimic the bare minimum of what the configure method is doing
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                            .Callback<ShipmentEntity>(x => x.Postal = new PostalShipmentEntity { Endicia = new EndiciaShipmentEntity() });
            
            var bestRateExcludedAccountRepositoryMock = new Mock<IBestRateExcludedAccountRepository>();
            bestRateExcludedAccountRepositoryMock.Setup(r => r.GetAll()).Returns(new List<long>());

            testObject = new EndiciaBestRateBroker(genericShipmentTypeMock.Object, genericRepositoryMock.Object, "USPS", bestRateExcludedAccountRepositoryMock.Object)
            {
                GetRatesAction = (shipment, type) =>
                {
                    var rateResult = rateResults[shipment.Postal.Endicia.EndiciaAccountID];
                    getRatesShipments.Add(EntityUtility.CloneEntity(shipment));
                    timesGetRatesCalled++;
                    return rateResult;
                }
            };

            testShipment = new ShipmentEntity
            {
                ShipmentType = (int) ShipmentTypeCode.BestRate,
                ContentWeight = 12.1,
                BestRate = new BestRateShipmentEntity
                {
                    DimsWeight = 3.4,
                    DimsAddWeight = true
                },
                OriginCountryCode = "US"
            };

            bestRateBrokerSettings = new Mock<IBestRateBrokerSettings>();

            bestRateBrokerSettings.Setup(b => b.IsEndiciaDHLEnabled())
                                  .Returns(false);
            bestRateBrokerSettings.Setup(b => b.IsEndiciaConsolidatorEnabled())
                                  .Returns(false);

            testObject.Configure(bestRateBrokerSettings.Object);
        }

        [Fact]
        public void HasAccounts_DelegatesToAccountRepository()
        {
            bool hasAccounts = testObject.HasAccounts;

            genericRepositoryMock.Verify(r => r.AccountsReadOnly, Times.Once());
        }

        [Fact]
        public void HasAccounts_ReturnsTrue_WhenRepositoryHasMoreThanZeroAccounts()
        {
            genericRepositoryMock.Setup(r => r.AccountsReadOnly).Returns(new List<IEndiciaAccountEntity> { new EndiciaAccountEntity() });

            bool hasAccounts = testObject.HasAccounts;

            Assert.True(hasAccounts);
        }

        [Fact]
        public void HasAccounts_ReturnsFalse_WhenRepositoryHasZeroAccounts()
        {
            genericRepositoryMock.Setup(r => r.AccountsReadOnly).Returns(new List<IEndiciaAccountEntity>());

            bool hasAccounts = testObject.HasAccounts;

            Assert.False(hasAccounts);
        }

        [Fact]
        public void GetBestRates_CallsConfigureNewShipmentForOneAccount()
        {
            testObject.GetBestRates(testShipment, new List<BrokerException>());

            genericShipmentTypeMock.Verify(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()), Times.Exactly(3));
        }

        [Fact]
        public void GetBestRates_CallsGetRatesForOneAccount()
        {
            testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.Equal(3, timesGetRatesCalled);

            foreach (var shipment in getRatesShipments)
            {
                Assert.Equal(ShipmentTypeCode.Endicia, (ShipmentTypeCode) shipment.ShipmentType);
                Assert.Equal(12.1, shipment.ContentWeight);
            }
        }

        [Fact]
        public void GetBestRates_ReturnsAllRates_WithNoFilter_ForDefaultProfileAccount()
        {
            rateGroup2.Rates.Clear();
            rateGroup3.Rates.Clear();
            
            RateGroup rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.Equal(3, rates.Rates.Count);
            Assert.True(rates.Rates.Any(r => r.RateID == account1Rate1.RateID));
            Assert.True(rates.Rates.Any(r => r.RateID == account1Rate2.RateID));
            Assert.True(rates.Rates.Any(r => r.RateID == account1Rate3.RateID));
        }

        [Fact]
        public void GetBestRates_ReturnsTwoRates_WhenTwoRatesHaveSameTypeLevelAndPrice()
        {
            rateGroup1.Rates.Clear();
            rateGroup2.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, new PostalRateSelection(PostalServiceType.StandardPost)) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult result2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost)) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup1.Rates.Add(result1);
            rateGroup1.Rates.Add(result2);

            RateGroup rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.True(rates.Rates.Any(r => r.RateID == result1.RateID));
            Assert.Equal(2, rates.Rates.Count);
        }

        [Fact]
        public void GetBestRates_ReturnsTwoRates_WhenTwoRatesHaveSameTypeLevel()
        {
            rateGroup1.Rates.Clear();
            rateGroup2.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, new PostalRateSelection(PostalServiceType.StandardPost)) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult result2 = new RateResult("Account 1b", "3", 2, new PostalRateSelection(PostalServiceType.StandardPost)) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup1.Rates.Add(result1);
            rateGroup1.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.Equal(2, rates.Rates.Count);
            Assert.True(rates.Rates.Any(r => r.RateID == result2.RateID));
        }

        [Fact]
        public void GetBestRates_ReturnsBothRates_WhenTwoRatesHaveSameTypeAndPriceButDifferentLevels()
        {
            rateGroup1.Rates.Clear();
            rateGroup2.Rates.Clear();
            rateGroup3.Rates.Clear();
            
            RateResult result1 = new RateResult("Account 1a", "4", 4, new PostalRateSelection(PostalServiceType.StandardPost)) { ServiceLevel = ServiceLevelType.TwoDays };
            RateResult result2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost)) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup1.Rates.Add(result1);
            rateGroup1.Rates.Add(result2);
            
            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.Equal(2, rates.Rates.Count);
            Assert.True(rates.Rates.Any(r => r.RateID == result1.RateID));
            Assert.True(rates.Rates.Any(r => r.RateID == result2.RateID));
        }

        [Fact]
        public void GetBestRates_DoesNotReturnNonSelectableRates()
        {
            rateGroup1.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4") { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup1.Rates.Add(result1);
            rateGroup1.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());


            Assert.Equal(3, rates.Rates.Count);
            Assert.False(rates.Rates.Any(r => r.RateID == result1.RateID));
            Assert.True(rates.Rates.Any(r => r.RateID == result2.RateID));
        }

        [ExcelData(@"Shipping\Carriers\Postal\Endicia\BestRate\Endicia_GetBestRates_DoesNotIncludeTypes.csv")]
        [Theory]
        public void GetBestRates_ExcludesVariousTypes(DataRow row)
        {
            PostalServiceType excludedServiceType = (PostalServiceType) Enum.Parse(typeof(PostalServiceType), row[0].ToString());

            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(excludedServiceType))
            {
                ServiceLevel = ServiceLevelType.TwoDays
            };

            RateResult result2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost))
            {
                ServiceLevel = ServiceLevelType.OneDay
            };

            rateGroup1.Rates.Add(result1);
            rateGroup1.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.False(rates.Rates.Any(r => r.RateID == result1.RateID), $"Returned rates should not include {EnumHelper.GetDescription(excludedServiceType)}");
            Assert.True(rates.Rates.Any(r => r.RateID == result2.RateID));
            Assert.Equal(1, rates.Rates.Count);
        }

        [Fact]
        public void GetBestRates_OriginalEndiciaShipmentDetailsAreRestoredAfterCall()
        {
            PostalShipmentEntity EndiciaShipment = new PostalShipmentEntity();
            testShipment.Postal = EndiciaShipment;
            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.Equal(EndiciaShipment, testShipment.Postal);
        }

        [Fact]
        public void GetBestRates_CallsHandlerWithInformationError_WhenCustomerEligibleForDhl()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Endicia Ground", "4", 4, new PostalRateSelection(PostalServiceType.ExpressMailPremium)) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Some Service", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup1.Rates.Add(result1);
            rateGroup3.Rates.Add(result2);

            List<BrokerException> calledExceptions = new List<BrokerException>();

            bestRateBrokerSettings.Setup(b => b.IsEndiciaDHLEnabled())
                                  .Returns(true);

            testObject.Configure(bestRateBrokerSettings.Object);

            testObject.GetBestRates(testShipment, calledExceptions);

            Assert.Equal(BrokerExceptionSeverityLevel.Information, calledExceptions.First().SeverityLevel);
        }

        [Fact]
        public void GetBestRates_DoesNotCallHandlerWithInformationError_WhenCustomerNotEligibleForDhl()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Endicia Ground", "4", 4, new PostalRateSelection(PostalServiceType.ExpressMailPremium)) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Some Service", "3", 4, new PostalRateSelection(PostalServiceType.DhlParcelGround)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup1.Rates.Add(result1);
            rateGroup3.Rates.Add(result2);

            List<BrokerException> brokerExceptions = new List<BrokerException>();

            testObject.GetBestRates(testShipment, brokerExceptions);

            Assert.False(brokerExceptions.Any(x => x.GetBaseException().Message.Contains("DHL")));
        }


        [Fact]
        public void GetBestRates_CallsHandlerWithInformationError_WhenCustomerEligibleForConsolidator()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Endicia Ground", "4", 4, new PostalRateSelection(PostalServiceType.ExpressMailPremium)) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Some Service", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup1.Rates.Add(result1);
            rateGroup3.Rates.Add(result2);

            List<BrokerException> brokerExceptions = new List<BrokerException>();

            bestRateBrokerSettings.Setup(b => b.IsEndiciaConsolidatorEnabled())
                                  .Returns(true);

            testObject.Configure(bestRateBrokerSettings.Object);

            testObject.GetBestRates(testShipment, brokerExceptions);

            Assert.Equal(BrokerExceptionSeverityLevel.Information, brokerExceptions.First().SeverityLevel);
        }

        [Fact]
        public void GetBestRates_DoesNotCallHandlerWithInformationError_WhenCustomerNotEligibleForConsolidator()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Endicia Ground", "4", 4, new PostalRateSelection(PostalServiceType.ExpressMailPremium)) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Some Service", "3", 4, new PostalRateSelection(PostalServiceType.DhlParcelGround)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup1.Rates.Add(result1);
            rateGroup3.Rates.Add(result2);

            List<BrokerException> brokerExceptions = new List<BrokerException>();

            testObject.GetBestRates(testShipment, brokerExceptions);

            Assert.False(brokerExceptions.Any(x => x.GetBaseException().Message.Contains("consolidator")));
        }

        [Fact]
        public void GetBestRates_SetsPackageDetails()
        {
            testShipment.BestRate.DimsHeight = 3;
            testShipment.BestRate.DimsWidth = 5;
            testShipment.BestRate.DimsLength = 2;

            testObject.GetBestRates(testShipment, new List<BrokerException>());

            foreach (ShipmentEntity shipment in getRatesShipments)
            {
                Assert.Equal(3, shipment.Postal.DimsHeight);
                Assert.Equal(5, shipment.Postal.DimsWidth);
                Assert.Equal(2, shipment.Postal.DimsLength);
                Assert.Equal(testShipment.BestRate.DimsAddWeight, shipment.Postal.DimsAddWeight);
                Assert.Equal(3.4, shipment.Postal.DimsWeight);
            }
        }

        [Fact]
        public void GetBestRates_OverridesProfileServiceAndPackagingType()
        {
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                                   .Callback<ShipmentEntity>(x =>
                                   {
                                       x.Postal.Service = (int) PostalServiceType.ConsolidatorDomestic;
                                       x.Postal.PackagingType = (int) PostalPackagingType.FlatRateSmallBox;
                                   });

            testObject.GetBestRates(testShipment, new List<BrokerException>());

            foreach (ShipmentEntity shipment in getRatesShipments)
            {
                Assert.Equal(PostalServiceType.PriorityMail, (PostalServiceType) shipment.Postal.Service);
                Assert.Equal(PostalPackagingType.Package, (PostalPackagingType) shipment.Postal.PackagingType);
            }
        }

        [Fact]
        public void GetBestRates_OverridesProfileAccount()
        {
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                                   .Callback<ShipmentEntity>(x =>
                                   {
                                       x.Postal.Endicia.EndiciaAccountID = 999;
                                   });

            testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.True(getRatesShipments.Any(x => x.Postal.Endicia.EndiciaAccountID == 1));
            Assert.False(getRatesShipments.Any(x => x.Postal.Endicia.EndiciaAccountID == 999));
        }

        [Fact]
        public void GetBestRates_OverridesProfileWeight()
        {
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                                   .Callback<ShipmentEntity>(x =>
                                   {
                                       x.ContentWeight = 123;
                                   });

            testObject.GetBestRates(testShipment, new List<BrokerException>());

            foreach (ShipmentEntity shipment in getRatesShipments)
            {
                Assert.Equal(3.4, shipment.Postal.DimsWeight);
            }
        }

        [Fact]
        public void GetBestRates_OverridesDimsAddWeight()
        {
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                                   .Callback<ShipmentEntity>(x =>
                                   {
                                       x.ContentWeight = 123;
                                   });

            testObject.GetBestRates(testShipment, new List<BrokerException>());

            foreach (ShipmentEntity shipment in getRatesShipments)
            {
                Assert.Equal(testShipment.BestRate.DimsAddWeight, shipment.Postal.DimsAddWeight);
            }
        }

        [Fact]
        public void GetBestRates_SetsTagToBestRateResultTag()
        {
            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            foreach (var rate in rates.Rates)
            {
                BestRateResultTag tag = rate.Tag as BestRateResultTag;

                if (tag == null)
                {
                    Assert.False(true);
                }

                Assert.IsAssignableFrom<Action<ShipmentEntity>>(tag.RateSelectionDelegate);
                Assert.NotNull(tag.OriginalTag);
                Assert.NotNull(tag.ResultKey);
            }
        }

        [Fact]
        public void GetBestRates_SetsTagResultKeyToPostalAndServiceType()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Endicia Ground", "4", 4, new PostalRateSelection(PostalServiceType.ExpressMailPremium)) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Some Service", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup1.Rates.Add(result1);
            rateGroup1.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            var resultKeys = rates.Rates.Select(x => x.Tag).Cast<BestRateResultTag>().Select(x => x.ResultKey).ToList();

            Assert.True(resultKeys.Contains("PostalExpress Mail (Premium)"));
            Assert.True(resultKeys.Contains("PostalStandard Post"));
        }

        [Fact]
        public void GetBestRates_AddsEndiciaToDescription_WhenItDoesNotAlreadyExist()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Endicia Ground", "4", 4, new PostalRateSelection(PostalServiceType.ExpressMailPremium)) { ServiceLevel = ServiceLevelType.OneDay };
            
            rateGroup1.Rates.Add(result1);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.True(rates.Rates.Select(x => x.Description).Contains("USPS Endicia Ground"));
            Assert.Equal(1, rates.Rates.Count);
        }

        [Fact]
        public void GetInsuranceProvider_ReturnsShipWorks()
        {
            Assert.Equal(InsuranceProvider.ShipWorks, testObject.GetInsuranceProvider(new ShippingSettingsEntity() { EndiciaInsuranceProvider = (int) InsuranceProvider.ShipWorks }));
        }

        [Fact]
        public void GetInsuranceProvider_ReturnsCarrier()
        {
            Assert.Equal(InsuranceProvider.Carrier, testObject.GetInsuranceProvider(new ShippingSettingsEntity() { EndiciaInsuranceProvider = (int) InsuranceProvider.Carrier }));
        }

        [Fact]
        public void Configure_ShouldCallCheckExpress1RatesOnSettings_WithShipmentType()
        {
            var brokerSettings = new Mock<IBestRateBrokerSettings>();

            testObject.Configure(brokerSettings.Object);

            brokerSettings.Verify(x => x.CheckExpress1Rates(testObject.ShipmentType));
        }

        [Fact]
        public void Configure_SetsRetrieveExpress1RatesToTrue_WhenConfigurationIsTrue()
        {
            Configure_ShouldRetrieveExpress1RatesTest(true);
        }

        [Fact]
        public void Configure_SetsRetrieveExpress1RatesToFalse_WhenConfigurationIsFalse()
        {
            Configure_ShouldRetrieveExpress1RatesTest(false);
        }

        private void Configure_ShouldRetrieveExpress1RatesTest(bool checkExpress1)
        {
            var brokerSettings = new Mock<IBestRateBrokerSettings>();
            brokerSettings.Setup(x => x.CheckExpress1Rates(It.IsAny<ShipmentType>())).Returns(checkExpress1);

            testObject.Configure(brokerSettings.Object);

            Assert.Equal(checkExpress1, ((EndiciaShipmentType) testObject.ShipmentType).ShouldRetrieveExpress1Rates);
        }

        [Fact]
        public void GetBestRates_ReturnsNoRates_WhenShipmentTotalWeightTooHeavy()
        {
            testShipment.TotalWeight = 70.1;
            RateGroup rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.Equal(0, rates.Rates.Count);
        }
    }
}
