using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion;
using ShipWorks.Shipping.Editing.Rating;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps
{
    public class UspsRateGroupConsolidatorTest
    {
        private readonly ExceptionsRateFootnoteFactory exceptionFootnoteFactory1;
        private readonly ExceptionsRateFootnoteFactory exceptionFootnoteFactory2;
        private readonly UspsRatePromotionFootnoteFactory uspsRatePromotionFootnote1;
        private readonly UspsRatePromotionFootnoteFactory uspsRatePromotionFootnote2;
        private readonly List<RateGroup> rateResults;
        private readonly UspsAccountEntity account1;
        private readonly UspsAccountEntity account2;
        private UspsRateGroupConsolidator testObject;
        private RateGroup rateGroup1;
        private RateGroup rateGroup2;

        public UspsRateGroupConsolidatorTest()
        {

            exceptionFootnoteFactory1 = new ExceptionsRateFootnoteFactory(ShipmentTypeCode.Usps, new Exception());
            exceptionFootnoteFactory2 = new ExceptionsRateFootnoteFactory(ShipmentTypeCode.Usps, new Exception());

            var uspsShipmentType = new UspsShipmentType();
            uspsRatePromotionFootnote1 = new UspsRatePromotionFootnoteFactory(uspsShipmentType, new ShipmentEntity(), true);
            uspsRatePromotionFootnote2 = new UspsRatePromotionFootnoteFactory(uspsShipmentType, new ShipmentEntity(), true);

            account1 = new UspsAccountEntity()
            {
                UspsAccountID = 1
            };

            account2 = new UspsAccountEntity()
            {
                UspsAccountID = 2
            };

            rateGroup1 = new RateGroup(new List<RateResult>()
            {
                new RateResult("Rate 1", "2", 10, new UspsPostalRateSelection(PostalServiceType.FirstClass, account1)),
                new RateResult("Express", "2", 10, new UspsPostalRateSelection(PostalServiceType.ExpressMail, account1)),
                new RateResult("Rate 3", "1", 100, new UspsPostalRateSelection(PostalServiceType.MediaMail, account1)),
                new RateResult("Rate 4", "1", 100, new UspsPostalRateSelection(PostalServiceType.PriorityMail, account1))
            });

            rateGroup2 = new RateGroup(new List<RateResult>()
            {
                new RateResult("Rate 1", "2", 11, new UspsPostalRateSelection(PostalServiceType.FirstClass, account2)),
                new RateResult("Express", "2", 11, new UspsPostalRateSelection(PostalServiceType.ExpressMail, account2)),
                new RateResult("Rate 3", "1", 100, new UspsPostalRateSelection(PostalServiceType.MediaMail, account2)),
                new RateResult("Rate 4", "1", 50, new UspsPostalRateSelection(PostalServiceType.PriorityMail, account2))
            });

            rateResults = new List<RateGroup>()
            {
                rateGroup1,
                rateGroup2
            };

            testObject = new UspsRateGroupConsolidator();
        }

        [Fact]
        public void ServiceMatches_ReturnsTrue_ServiceTypeMatch()
        {
            RateResult rate1 = new RateResult("Rate 3", "1", 100, new UspsPostalRateSelection(PostalServiceType.MediaMail, account1));
            RateResult rate2 = new RateResult("Rate 3", "1", 100, new UspsPostalRateSelection(PostalServiceType.MediaMail, account1));

            Assert.True(testObject.ServiceMatches(rate1, rate2), "The services match but ServiceMatches returned false");
        }

        [Fact]
        public void ServiceMatches_ReturnsFalse_ServiceTypeDoesNotMatch()
        {
            RateResult rate1 = new RateResult("Rate 3", "1", 100, new UspsPostalRateSelection(PostalServiceType.ExpressMailPremium, account1));
            RateResult rate2 = new RateResult("Rate 3", "1", 100, new UspsPostalRateSelection(PostalServiceType.MediaMail, account1));

            Assert.False(testObject.ServiceMatches(rate1, rate2), "The services don't match but ServiceMatches returned true");
        }

        [Fact]
        public void Consolidate_MatchingRateAccountsConsolidated()
        {
            RateGroup consolidatedRates = testObject.Consolidate(rateResults);

            RateResult resultToTest = FindByServiceResult(consolidatedRates.Rates, PostalServiceType.MediaMail);

            List<IUspsAccountEntity> accounts = GetTag(resultToTest).Accounts;

            Assert.Equal(2, accounts.Count);
            Assert.True(accounts.Contains(account1));
            Assert.True(accounts.Contains(account2));
        }

        [Fact]
        public void Consolidate_BothAddonsIncludedFromDifferentRateGroup()
        {
            RateGroup consolidatedRates = testObject.Consolidate(rateResults);

            RateResult fromGroup1 = FindByServiceResult(consolidatedRates.Rates, PostalServiceType.ExpressMail);
            RateResult fromGroup2 = FindByServiceResult(consolidatedRates.Rates, PostalServiceType.ExpressMail);

            Assert.NotNull(fromGroup1);
            Assert.NotNull(fromGroup2);
        }

        [Fact]
        public void Consolidate_CheaperRateFromGroup1Chosen()
        {
            RateGroup consolidatedRates = testObject.Consolidate(rateResults);

            RateResult includedRate = FindByServiceResult(consolidatedRates.Rates, PostalServiceType.FirstClass);

            List<IUspsAccountEntity> includedAccounts = GetTag(includedRate).Accounts;

            Assert.Equal(account1, includedAccounts.Single(a => true));
            Assert.Equal(10, includedRate.AmountOrDefault);
        }

        [Fact]
        public void Consolidate_CheaperRateFromGroup2Chosen()
        {
            RateGroup consolidatedRates = testObject.Consolidate(rateResults);

            RateResult includedRate = FindByServiceResult(consolidatedRates.Rates, PostalServiceType.PriorityMail);

            List<IUspsAccountEntity> includedAccounts = GetTag(includedRate).Accounts;

            Assert.Equal(account2, includedAccounts.Single(a => true));
            Assert.Equal(50, includedRate.AmountOrDefault);
        }

        [Fact]
        public void Consolidate_FooterFromOneRateGroupReturned_FooterIsNotOfTypeUspsRatePromotionFootnote()
        {
            rateGroup1.AddFootnoteFactory(exceptionFootnoteFactory1);

            RateGroup consolidatedRates = testObject.Consolidate(rateResults);

            Assert.True(consolidatedRates.FootnoteFactories.Contains(exceptionFootnoteFactory1));
        }

        [Fact]
        public void Consolidate_FooterFromOneRateGroupReturned_OneExceptionFooterInEachGroup()
        {
            rateGroup1.AddFootnoteFactory(exceptionFootnoteFactory1);
            rateGroup2.AddFootnoteFactory(exceptionFootnoteFactory2);

            RateGroup consolidatedRates = testObject.Consolidate(rateResults);

            // It can contain only 1
            Assert.True(consolidatedRates.FootnoteFactories.Contains(exceptionFootnoteFactory1) ^ consolidatedRates.FootnoteFactories.Contains(exceptionFootnoteFactory2));
        }

        [Fact]
        public void Consolidate_FooterFromOneRateGroupReturned_OneUspsRatePromotionFootNoteInEachGroup()
        {
            rateGroup1.AddFootnoteFactory(uspsRatePromotionFootnote1);
            rateGroup2.AddFootnoteFactory(uspsRatePromotionFootnote2);

            RateGroup consolidatedRates = testObject.Consolidate(rateResults);

            // It can contain only 1
            Assert.True(consolidatedRates.FootnoteFactories.Contains(uspsRatePromotionFootnote1) ^ consolidatedRates.FootnoteFactories.Contains(uspsRatePromotionFootnote2));
        }

        [Fact]
        public void Consolidate_NoFooterReturned_UspsRatePromotionFactoryInOneGroupAndNotOther()
        {
            rateGroup1.AddFootnoteFactory(uspsRatePromotionFootnote1);

            RateGroup consolidatedRates = testObject.Consolidate(rateResults);

            // If not in both, don't keep any UspsRatePromotionFactory
            Assert.False(consolidatedRates.FootnoteFactories.Any());
        }


        public UspsPostalRateSelection GetTag(RateResult rate)
        {
            return (UspsPostalRateSelection) rate.Tag;
        }

        public RateResult FindByServiceResult(List<RateResult> rates, PostalServiceType serviceType)
        {
            return rates.SingleOrDefault(r => GetTag(r).ServiceType == serviceType);
        }
    }
}
