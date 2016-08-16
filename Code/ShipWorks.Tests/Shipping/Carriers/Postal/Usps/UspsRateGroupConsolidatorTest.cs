using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps
{
    public class UspsRateGroupConsolidatorTest
    {
        ExceptionsRateFootnoteFactory exceptionFootnoteFactory1;
        ExceptionsRateFootnoteFactory exceptionFootnoteFactory2;
        UspsRatePromotionFootnoteFactory uspsRatePromotionFootnote1;
        UspsRatePromotionFootnoteFactory uspsRatePromotionFootnote2;


        List<RateGroup> rateResults;
        UspsAccountEntity account1;
        UspsAccountEntity account2;

        UspsRateGroupConsolidator testObject;

        RateGroup rateGroup1;
        RateGroup rateGroup2;

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
                new RateResult("Rate Header 1", "2") { Tag = new UspsPostalRateSelection(PostalServiceType.FirstClass, PostalConfirmationType.None, account1) },
                new RateResult("  1 with signature", "2", 10, new UspsPostalRateSelection(PostalServiceType.FirstClass, PostalConfirmationType.Signature, account1)),
                new RateResult("  1 without signature", "2", 20, new UspsPostalRateSelection(PostalServiceType.FirstClass, PostalConfirmationType.Delivery, account1)),
                new RateResult("Express", "2") { Tag = new UspsPostalRateSelection(PostalServiceType.ExpressMail, PostalConfirmationType.None, account1) },
                new RateResult("  Express without signature", "2", 10, new UspsPostalRateSelection(PostalServiceType.ExpressMail, PostalConfirmationType.Delivery, account1)),
                new RateResult("Rate 3", "1", 100, new UspsPostalRateSelection(PostalServiceType.MediaMail, PostalConfirmationType.Delivery, account1))
            });

            rateGroup2 = new RateGroup(new List<RateResult>()
            {
                new RateResult("Rate Header 1", "2") { Tag = new UspsPostalRateSelection(PostalServiceType.FirstClass, PostalConfirmationType.None, account2) },
                new RateResult("  1 with signature", "2", 11, new UspsPostalRateSelection(PostalServiceType.FirstClass, PostalConfirmationType.Signature, account2)),
                new RateResult("  1 without signature", "2", 19, new UspsPostalRateSelection(PostalServiceType.FirstClass, PostalConfirmationType.Delivery, account2)),
                new RateResult("Express", "2") { Tag = new UspsPostalRateSelection(PostalServiceType.ExpressMail, PostalConfirmationType.None, account2) },
                new RateResult("  Express with signature", "2", 11, new UspsPostalRateSelection(PostalServiceType.ExpressMail, PostalConfirmationType.Signature, account2)),
                new RateResult("Rate 3", "1", 100, new UspsPostalRateSelection(PostalServiceType.MediaMail, PostalConfirmationType.Delivery, account2))
            });

            rateResults = new List<RateGroup>()
            {
                rateGroup1,
                rateGroup2
            };

            testObject = new UspsRateGroupConsolidator();
        }

        [Fact]
        public void ServiceMatches_ReturnsTrue_ConfirmationAndServiceTypeMatch()
        {
            RateResult rate1 = new RateResult("Rate 3", "1", 100, new UspsPostalRateSelection(PostalServiceType.MediaMail, PostalConfirmationType.Delivery, account1));
            RateResult rate2 = new RateResult("Rate 3", "1", 100, new UspsPostalRateSelection(PostalServiceType.MediaMail, PostalConfirmationType.Delivery, account1));

            Assert.True(testObject.ServiceMatches(rate1, rate2), "The services match but ServiceMatches returned false");
        }

        [Fact]
        public void ServiceMatches_ReturnsFalse_ServiceTypeDoesNotMatch()
        {
            RateResult rate1 = new RateResult("Rate 3", "1", 100, new UspsPostalRateSelection(PostalServiceType.ExpressMailPremium, PostalConfirmationType.Delivery, account1));
            RateResult rate2 = new RateResult("Rate 3", "1", 100, new UspsPostalRateSelection(PostalServiceType.MediaMail, PostalConfirmationType.Delivery, account1));

            Assert.False(testObject.ServiceMatches(rate1, rate2), "The services don't match but ServiceMatches returned true");
        }

        [Fact]
        public void ServiceMatches_ReturnsFalse_ConfirmationTypeDoesNotMatch()
        {
            RateResult rate1 = new RateResult("Rate 3", "1", 100, new UspsPostalRateSelection(PostalServiceType.MediaMail, PostalConfirmationType.Delivery, account1));
            RateResult rate2 = new RateResult("Rate 3", "1", 100, new UspsPostalRateSelection(PostalServiceType.MediaMail, PostalConfirmationType.Signature, account1));

            Assert.False(testObject.ServiceMatches(rate1, rate2), "The services don't match but ServiceMatches returned true");
        }

        
        [Fact]
        public void Consolidation_NonSelectableComesBeforeAddons()
        {
            RateGroup consolidatedRates = testObject.Consolidate(rateResults);

            RateResult header = FindByServiceAndConfirmationResult(consolidatedRates.Rates, PostalServiceType.FirstClass, PostalConfirmationType.None);
            RateResult addon = FindByServiceAndConfirmationResult(consolidatedRates.Rates, PostalServiceType.FirstClass, PostalConfirmationType.Signature);
            
            Assert.False(header.Selectable);
            Assert.True(addon.Selectable);
            Assert.True(consolidatedRates.Rates.IndexOf(header) < consolidatedRates.Rates.IndexOf(addon));
        }

        [Fact]
        public void Consolidate_MatchingRateAccountsConsolidated()
        {
            RateGroup consolidatedRates = testObject.Consolidate(rateResults);

            RateResult resultToTest = FindByServiceAndConfirmationResult(consolidatedRates.Rates, PostalServiceType.MediaMail, PostalConfirmationType.Delivery);

            List<UspsAccountEntity> accounts = GetTag(resultToTest).Accounts;

            Assert.Equal(2, accounts.Count);
            Assert.True(accounts.Contains(account1));
            Assert.True(accounts.Contains(account2));
        }

        [Fact]
        public void Consolidate_BothAddonsIncludedFromDifferentRateGroup()
        {
            RateGroup consolidatedRates = testObject.Consolidate(rateResults);

            RateResult fromGroup1 = FindByServiceAndConfirmationResult(consolidatedRates.Rates, PostalServiceType.ExpressMail, PostalConfirmationType.Delivery);
            RateResult fromGroup2 = FindByServiceAndConfirmationResult(consolidatedRates.Rates, PostalServiceType.ExpressMail, PostalConfirmationType.Signature);

            Assert.NotNull(fromGroup1);
            Assert.NotNull(fromGroup2);
        }

        [Fact]
        public void Consolidate_CheaperRateFromGroup1Chosen()
        {
            RateGroup consolidatedRates = testObject.Consolidate(rateResults);

            RateResult includedRate = FindByServiceAndConfirmationResult(consolidatedRates.Rates, PostalServiceType.FirstClass, PostalConfirmationType.Signature);

            List<UspsAccountEntity> includedAccounts = GetTag(includedRate).Accounts;

            Assert.Equal(account1, includedAccounts.Single(a => true));
            Assert.Equal(10, includedRate.AmountOrDefault);
        }

        [Fact]
        public void Consolidate_CheaperRateFromGroup2Chosen()
        {
            RateGroup consolidatedRates = testObject.Consolidate(rateResults);

            RateResult includedRate = FindByServiceAndConfirmationResult(consolidatedRates.Rates, PostalServiceType.FirstClass, PostalConfirmationType.Delivery);

            List<UspsAccountEntity> includedAccounts = GetTag(includedRate).Accounts;

            Assert.Equal(account2, includedAccounts.Single(a => true));
            Assert.Equal(19, includedRate.AmountOrDefault);
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
            return (UspsPostalRateSelection)rate.Tag;
        }

        public RateResult FindByServiceAndConfirmationResult(List<RateResult> rates, PostalServiceType serviceType, PostalConfirmationType confirmationType)
        {
            return rates.SingleOrDefault(r => ((UspsPostalRateSelection)r.Tag).ConfirmationType == confirmationType && ((UspsPostalRateSelection)r.Tag).ServiceType == serviceType);
        }
    }
}
