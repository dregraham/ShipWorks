using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Interapptive.Shared.Utility;
using Xunit;
using Moq;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Endicia.BestRate
{
    public class EndiciaBestRateBrokerTest
    {
        private EndiciaAccountEntity account1;
        private EndiciaAccountEntity account2;
        private EndiciaAccountEntity account3;
        private RateGroup rateGroup1;
        private RateGroup rateGroup2;
        private RateGroup rateGroup3;
        private RateResult account1Rate1;
        private RateResult account1Rate2;
        private RateResult account1Rate3;
        private RateResult account2Rate1;
        private RateResult account3Rate1;
        private RateResult account3Rate2;
        private Mock<IBestRateBrokerSettings> bestRateBrokerSettings;
        private EndiciaBestRateBroker testObject;
        private ShipmentEntity testShipment;

        /// <summary>
        /// This holds a collection of all the shipment objects that were passed into the EndiciaShipmentType.GetRates method
        /// </summary>
        private List<ShipmentEntity> getRatesShipments;

        private Mock<ICarrierAccountRepository<EndiciaAccountEntity>> genericRepositoryMock;
        private Mock<EndiciaShipmentType> genericShipmentTypeMock;
        private Dictionary<long, RateGroup> rateResults;

        private int timesGetRatesCalled = 0;
        
        public EndiciaBestRateBrokerTest()
        {
            account1 = new EndiciaAccountEntity { EndiciaAccountID = 1, CountryCode = "US" };
            account2 = new EndiciaAccountEntity { EndiciaAccountID = 2, CountryCode = "US" };
            account3 = new EndiciaAccountEntity { EndiciaAccountID = 3, CountryCode = "US" };

            account1Rate1 = new RateResult("Account 1a", "4", 12, new PostalRateSelection(PostalServiceType.PriorityMail, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.TwoDays };
            account1Rate2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            account1Rate3 = new RateResult("Account 1c", "1", 15, new PostalRateSelection(PostalServiceType.ExpressMailPremium, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };
            account2Rate1 = new RateResult("* No rates were returned for the selected Service.", "");
            account3Rate1 = new RateResult("Account 3a", "4", 3, new PostalRateSelection(PostalServiceType.ParcelSelect, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.Anytime };
            account3Rate2 = new RateResult("Account 3b", "3", 10, new PostalRateSelection(PostalServiceType.FirstClass, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.TwoDays };

            rateGroup1 = new RateGroup(new[] { account1Rate1, account1Rate2, account1Rate3 });
            rateGroup2 = new RateGroup(new[] { account2Rate1 });
            rateGroup3 = new RateGroup(new[] { account3Rate1, account3Rate2 });

            rateResults = new Dictionary<long, RateGroup>
                {
                    {1, rateGroup1},
                    {2, rateGroup2},
                    {3, rateGroup3},
                };

            genericRepositoryMock = new Mock<ICarrierAccountRepository<EndiciaAccountEntity>>();
            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<EndiciaAccountEntity> { account1, account2, account3 });
            genericRepositoryMock.Setup(r => r.DefaultProfileAccount).Returns(account1);

            getRatesShipments = new List<ShipmentEntity>();

            // Save a copy of all the shipment entities passed into the GetRates method so we can inspect them later
            genericShipmentTypeMock = new Mock<EndiciaShipmentType>();
            genericShipmentTypeMock.Setup(x => x.ShipmentTypeCode).Returns(ShipmentTypeCode.Endicia);

            // Mimic the bare minimum of what the configure method is doing
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                            .Callback<ShipmentEntity>(x => x.Postal = new PostalShipmentEntity { Endicia = new EndiciaShipmentEntity() });

            testObject = new EndiciaBestRateBroker(genericShipmentTypeMock.Object, genericRepositoryMock.Object)
            {
                GetRatesAction = (shipment, type) =>
                {
                    var rateResult = rateResults[shipment.Postal.Endicia.EndiciaAccountID];
                    getRatesShipments.Add(EntityUtility.CloneEntity(shipment));
                    timesGetRatesCalled++;
                    return rateResult;
                }
            };
            
            testShipment = new ShipmentEntity { ShipmentType = (int)ShipmentTypeCode.BestRate, 
                ContentWeight = 12.1, 
                BestRate = new BestRateShipmentEntity
                {
                    DimsWeight = 3.4,
                    DimsAddWeight = true
                },
                OriginCountryCode = "US"};

            bestRateBrokerSettings = new Mock<IBestRateBrokerSettings>();

            bestRateBrokerSettings.Setup(b => b.IsEndiciaDHLEnabled())
                                  .Returns(false);
            bestRateBrokerSettings.Setup(b => b.IsEndiciaConsolidatorEnabled())
                                  .Returns(false);
            
            testObject.Configure(bestRateBrokerSettings.Object);
        }

        [Fact]
        public void HasAccounts_DelegatesToAccountRepository_Test()
        {
            bool hasAccounts = testObject.HasAccounts;

            genericRepositoryMock.Verify(r => r.Accounts, Times.Once());
        }

        [Fact]
        public void HasAccounts_ReturnsTrue_WhenRepositoryHasMoreThanZeroAccounts_Test()
        {
            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<EndiciaAccountEntity> { new EndiciaAccountEntity() });

            bool hasAccounts = testObject.HasAccounts;

            Assert.True(hasAccounts);
        }

        [Fact]
        public void HasAccounts_ReturnsFalse_WhenRepositoryHasZeroAccounts_Test()
        {
            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<EndiciaAccountEntity>());

            bool hasAccounts = testObject.HasAccounts;

            Assert.False(hasAccounts);
        }

        [Fact]
        public void GetBestRates_RetrievesDefaultProfileAccount_Test()
        {
            testObject.GetBestRates(testShipment, new List<BrokerException>());

            genericRepositoryMock.Verify(x => x.DefaultProfileAccount);
        }

        [Fact]
        public void GetBestRates_CallsConfigureNewShipmentForOneAccount_Test()
        {
            testObject.GetBestRates(testShipment, new List<BrokerException>());

            genericShipmentTypeMock.Verify(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()), Times.Exactly(1));
        }

        [Fact]
        public void GetBestRates_CallsGetRatesForOneAccount_Test()
        {
            testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.Equal(1, timesGetRatesCalled);
            
            foreach (var shipment in getRatesShipments)
            {
                Assert.Equal(ShipmentTypeCode.Endicia, (ShipmentTypeCode)shipment.ShipmentType);
                Assert.Equal(12.1, shipment.ContentWeight);
            }
        }

        [Fact]
        public void GetBestRates_ReturnsAllRates_WithNoFilter_ForDefaultProfileAccount_Test()
        {
            RateGroup rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.Equal(3, rates.Rates.Count);
            Assert.True(rates.Rates.Any(r => r.RateID == account1Rate1.RateID));
            Assert.True(rates.Rates.Any(r => r.RateID == account1Rate2.RateID));
            Assert.True(rates.Rates.Any(r => r.RateID == account1Rate3.RateID));
        }

        [Fact]
        public void GetBestRates_ReturnsTwoRates_WhenTwoRatesHaveSameTypeLevelAndPrice_Test()
        {
            rateGroup1.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult result2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup1.Rates.Add(result1);
            rateGroup1.Rates.Add(result2);

            RateGroup rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.True(rates.Rates.Any(r => r.RateID == result1.RateID));
            Assert.Equal(2, rates.Rates.Count);
        }

        [Fact]
        public void GetBestRates_ReturnsTwoRates_WhenTwoRatesHaveSameTypeLevel_Test()
        {
            rateGroup1.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult result2 = new RateResult("Account 1b", "3", 2, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup1.Rates.Add(result1);
            rateGroup1.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.Equal(2, rates.Rates.Count);
            Assert.True(rates.Rates.Any(r => r.RateID == result2.RateID));
        }

        [Fact]
        public void GetBestRates_ReturnsBothRates_WhenTwoRatesHaveSameTypeAndPriceButDifferentLevels_Test()
        {
            rateGroup1.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.TwoDays };
            RateResult result2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup1.Rates.Add(result1);
            rateGroup1.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.Equal(2, rates.Rates.Count);
            Assert.True(rates.Rates.Any(r => r.RateID == result1.RateID));
            Assert.True(rates.Rates.Any(r => r.RateID == result2.RateID));
        }

        [Fact]
        public void GetBestRates_DoesNotReturnNonSelectableRates_Test()
        {
            rateGroup1.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4") { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup1.Rates.Add(result1);
            rateGroup1.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());


            Assert.Equal(1, rates.Rates.Count);
            Assert.False(rates.Rates.Any(r => r.RateID == result1.RateID));
            Assert.True(rates.Rates.Any(r => r.RateID == result2.RateID));
        }

        //[Fact]
        //public void GetBestRates_DoesNotIncludeMediaMail()
        //{
        //    TestServiceTypeIsExcluded(PostalServiceType.MediaMail);
        //}

        //[Fact]
        //public void GetBestRates_DoesNotIncludeLibrayMail()
        //{
        //    TestServiceTypeIsExcluded(PostalServiceType.LibraryMail);
        //}

        //[Fact]
        //public void GetBestRates_DoesNotIncludeBPM()
        //{
        //    TestServiceTypeIsExcluded(PostalServiceType.BoundPrintedMatter);
        //}
        
        [CsvData(@"Shipping\Carriers\Postal\Endicia\BestRate", "Endicia_GetBestRates_DoesNotIncludeTypes")]
        [Theory]
        public void GetBestRates_ExcludesVariousTypes_Test(DataRow row)
        {
            PostalServiceType excludedServiceType = (PostalServiceType)Enum.Parse(typeof (PostalServiceType), row[0].ToString());

            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(excludedServiceType, PostalConfirmationType.None))
                {
                    ServiceLevel = ServiceLevelType.TwoDays
                };

            RateResult result2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None))
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
        public void GetBestRates_UpdatesDescriptionWhenPartOfRateGroup_Test()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Foo", "4") { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("       Bar", string.Empty, 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            RateResult result3 = new RateResult("Baz", "3") { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result4 = new RateResult("   Other", string.Empty, 4, new PostalRateSelection(PostalServiceType.ExpressMail, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup1.Rates.Add(result1);
            rateGroup1.Rates.Add(result2);
            rateGroup1.Rates.Add(result3);
            rateGroup1.Rates.Add(result4);

            RateGroup bestRates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.Equal("USPS Foo Bar", bestRates.Rates.Single(r => r.RateID == result2.RateID).Description);
            Assert.Equal("USPS Baz Other", bestRates.Rates.Single(r => r.RateID == result4.RateID).Description);
            Assert.Equal("4", bestRates.Rates.Single(r => r.RateID == result2.RateID).Days);
            Assert.Equal("3", bestRates.Rates.Single(r => r.RateID == result4.RateID).Days);
        }

        [Fact]
        public void GetBestRates_OriginalEndiciaShipmentDetailsAreRestoredAfterCall_Test()
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

            RateResult result1 = new RateResult("Endicia Ground", "4", 4, new PostalRateSelection(PostalServiceType.ExpressMailPremium, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Some Service", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

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
        public void GetBestRates_DoesNotCallHandlerWithInformationError_WhenCustomerNotEligibleForDhl_Test()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Endicia Ground", "4", 4, new PostalRateSelection(PostalServiceType.ExpressMailPremium, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Some Service", "3", 4, new PostalRateSelection(PostalServiceType.DhlParcelGround, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup1.Rates.Add(result1);
            rateGroup3.Rates.Add(result2);

            List<BrokerException> brokerExceptions = new List<BrokerException>();

            testObject.GetBestRates(testShipment, brokerExceptions);

            Assert.False(brokerExceptions.Any(x => x.GetBaseException().Message.Contains("DHL")));
        }


        [Fact]
        public void GetBestRates_CallsHandlerWithInformationError_WhenCustomerEligibleForConsolidator_Test()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Endicia Ground", "4", 4, new PostalRateSelection(PostalServiceType.ExpressMailPremium, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Some Service", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

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
        public void GetBestRates_DoesNotCallHandlerWithInformationError_WhenCustomerNotEligibleForConsolidator_Test()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Endicia Ground", "4", 4, new PostalRateSelection(PostalServiceType.ExpressMailPremium, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Some Service", "3", 4, new PostalRateSelection(PostalServiceType.DhlParcelGround, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup1.Rates.Add(result1);
            rateGroup3.Rates.Add(result2);

            List<BrokerException> brokerExceptions = new List<BrokerException>();

            testObject.GetBestRates(testShipment, brokerExceptions);

            Assert.False(brokerExceptions.Any(x => x.GetBaseException().Message.Contains("consolidator")));
        }

        [Fact]
        public void GetBestRates_SetsPackageDetails_Test()
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
        public void GetBestRates_OverridesProfileServiceAndPackagingType_Test()
        {
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                                   .Callback<ShipmentEntity>(x =>
                                   {
                                       x.Postal.Service = (int)PostalServiceType.ConsolidatorDomestic;
                                       x.Postal.PackagingType = (int) PostalPackagingType.FlatRateSmallBox;
                                   });

            testObject.GetBestRates(testShipment, new List<BrokerException>());

            foreach (ShipmentEntity shipment in getRatesShipments)
            {
                Assert.Equal(PostalServiceType.PriorityMail, (PostalServiceType)shipment.Postal.Service);
                Assert.Equal(PostalPackagingType.Package, (PostalPackagingType)shipment.Postal.PackagingType);
            }
        }

        [Fact]
        public void GetBestRates_OverridesProfileAccount_Test()
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
        public void GetBestRates_OverridesProfileWeight_Test()
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
        public void GetBestRates_OverridesDimsAddWeight_Test()
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
        public void GetBestRates_SetsTagResultKeyToPostalAndServiceType_Test()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Endicia Ground", "4", 4, new PostalRateSelection(PostalServiceType.ExpressMailPremium, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Some Service", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup1.Rates.Add(result1);
            rateGroup1.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            var resultKeys = rates.Rates.Select(x => x.Tag).Cast<BestRateResultTag>().Select(x => x.ResultKey).ToList();

            Assert.True(resultKeys.Contains("PostalExpress Mail (Premium)"));
            Assert.True(resultKeys.Contains("PostalStandard Post"));
        }

        [Fact]
        public void GetBestRates_AddsEndiciaToDescription_WhenItDoesNotAlreadyExist_Test()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Endicia Ground", "4", 4, new PostalRateSelection(PostalServiceType.ExpressMailPremium, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Some Service", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup1.Rates.Add(result1);
            rateGroup3.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.True(rates.Rates.Select(x => x.Description).Contains("USPS Endicia Ground"));
            Assert.Equal(1, rates.Rates.Count);
        }

        [Fact]
        public void GetInsuranceProvider_ReturnsShipWorks_Test()
        {
            Assert.Equal(InsuranceProvider.ShipWorks, testObject.GetInsuranceProvider(new ShippingSettingsEntity() {EndiciaInsuranceProvider = (int) InsuranceProvider.ShipWorks}));
        }

        [Fact]
        public void GetInsuranceProvider_ReturnsCarrier_Test()
        {
            Assert.Equal(InsuranceProvider.Carrier, testObject.GetInsuranceProvider(new ShippingSettingsEntity() { EndiciaInsuranceProvider = (int)InsuranceProvider.Carrier }));
        }

        [Fact]
        public void Configure_ShouldCallCheckExpress1RatesOnSettings_WithShipmentType_Test()
        {
            var brokerSettings = new Mock<IBestRateBrokerSettings>();

            testObject.Configure(brokerSettings.Object);

            brokerSettings.Verify(x => x.CheckExpress1Rates(testObject.ShipmentType));
        }

        [Fact]
        public void Configure_SetsRetrieveExpress1RatesToTrue_WhenConfigurationIsTrue_Test()
        {
            Configure_ShouldRetrieveExpress1RatesTest(true);
        }

        [Fact]
        public void Configure_SetsRetrieveExpress1RatesToFalse_WhenConfigurationIsFalse_Test()
        {
            Configure_ShouldRetrieveExpress1RatesTest(false);
        }

        private void Configure_ShouldRetrieveExpress1RatesTest(bool checkExpress1)
        {
            var brokerSettings = new Mock<IBestRateBrokerSettings>();
            brokerSettings.Setup(x => x.CheckExpress1Rates(It.IsAny<ShipmentType>())).Returns(checkExpress1);

            testObject.Configure(brokerSettings.Object);

            Assert.Equal(checkExpress1, ((EndiciaShipmentType)testObject.ShipmentType).ShouldRetrieveExpress1Rates);
        }

        [Fact]
        public void GetBestRates_ReturnsNoRates_WhenShipmentTotalWeightTooHeavy_Test()
        {
            testShipment.TotalWeight = 70.1;
            RateGroup rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.Equal(0, rates.Rates.Count);
        }
    }
}
