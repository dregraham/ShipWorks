﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps
{
    [TestClass]
    public class UspsRateGroupConsolidatorTest
    {
        ExceptionsRateFootnoteFactory exceptionFootnoteFactory1;
        ExceptionsRateFootnoteFactory exceptionFootnoteFactory2;
        UspsRatePromotionFootnoteFactory uspsRatePromotionFootnote1;
        UspsRatePromotionFootnoteFactory uspsRatePromotionFootnote2;


        List<RateGroup> rateResults;
        StampsAccountEntity account1;
        StampsAccountEntity account2;

        UspsRateGroupConsolidator testObject;

        RateGroup rateGroup1;
        RateGroup rateGroup2;

        UspsShipmentType uspsShipmentType;

        [TestInitialize]
        public void Initialize()
        {
            exceptionFootnoteFactory1 = new ExceptionsRateFootnoteFactory(uspsShipmentType, new Exception());
            exceptionFootnoteFactory2 = new ExceptionsRateFootnoteFactory(uspsShipmentType, new Exception());

            uspsRatePromotionFootnote1 = new UspsRatePromotionFootnoteFactory(uspsShipmentType, new ShipmentEntity(), true);
            uspsRatePromotionFootnote2 = new UspsRatePromotionFootnoteFactory(uspsShipmentType, new ShipmentEntity(), true);
            
            uspsShipmentType = new UspsShipmentType();

            account1 = new StampsAccountEntity()
            {
                StampsAccountID = 1
            };

            account2 = new StampsAccountEntity()
            {
                StampsAccountID = 2
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

        [TestMethod]
        public void ServiceMatches_ReturnsTrue_ConfirmationAndServiceTypeMatch_Test()
        {
            RateResult rate1 = new RateResult("Rate 3", "1", 100, new UspsPostalRateSelection(PostalServiceType.MediaMail, PostalConfirmationType.Delivery, account1));
            RateResult rate2 = new RateResult("Rate 3", "1", 100, new UspsPostalRateSelection(PostalServiceType.MediaMail, PostalConfirmationType.Delivery, account1));

            Assert.IsTrue(testObject.ServiceMatches(rate1, rate2), "The services match but ServiceMatches returned false");
        }

        [TestMethod]
        public void ServiceMatches_ReturnsFalse_ServiceTypeDoesNotMatch_Test()
        {
            RateResult rate1 = new RateResult("Rate 3", "1", 100, new UspsPostalRateSelection(PostalServiceType.ExpressMailPremium, PostalConfirmationType.Delivery, account1));
            RateResult rate2 = new RateResult("Rate 3", "1", 100, new UspsPostalRateSelection(PostalServiceType.MediaMail, PostalConfirmationType.Delivery, account1));

            Assert.IsFalse(testObject.ServiceMatches(rate1, rate2), "The services don't match but ServiceMatches returned true");
        }

        [TestMethod]
        public void ServiceMatches_ReturnsFalse_ConfirmationTypeDoesNotMatch_Test()
        {
            RateResult rate1 = new RateResult("Rate 3", "1", 100, new UspsPostalRateSelection(PostalServiceType.MediaMail, PostalConfirmationType.Delivery, account1));
            RateResult rate2 = new RateResult("Rate 3", "1", 100, new UspsPostalRateSelection(PostalServiceType.MediaMail, PostalConfirmationType.Signature, account1));

            Assert.IsFalse(testObject.ServiceMatches(rate1, rate2), "The services don't match but ServiceMatches returned true");
        }

        
        [TestMethod]
        public void Consolidation_NonSelectableComesBeforeAddons_Test()
        {
            RateGroup consolidatedRates = testObject.Consolidate(rateResults);

            RateResult header = FindByServiceAndConfirmationResult(consolidatedRates.Rates, PostalServiceType.FirstClass, PostalConfirmationType.None);
            RateResult addon = FindByServiceAndConfirmationResult(consolidatedRates.Rates, PostalServiceType.FirstClass, PostalConfirmationType.Signature);
            
            Assert.IsFalse(header.Selectable);
            Assert.IsTrue(addon.Selectable);
            Assert.IsTrue(consolidatedRates.Rates.IndexOf(header) < consolidatedRates.Rates.IndexOf(addon));
        }

        [TestMethod]
        public void Consolidate_MatchingRateAccountsConsolidated_Test()
        {
            RateGroup consolidatedRates = testObject.Consolidate(rateResults);

            RateResult resultToTest = FindByServiceAndConfirmationResult(consolidatedRates.Rates, PostalServiceType.MediaMail, PostalConfirmationType.Delivery);

            List<StampsAccountEntity> accounts = GetTag(resultToTest).Accounts;

            Assert.AreEqual(2, accounts.Count);
            Assert.IsTrue(accounts.Contains(account1));
            Assert.IsTrue(accounts.Contains(account2));
        }

        [TestMethod]
        public void Consolidate_BothAddonsIncludedFromDifferentRateGroup_Test()
        {
            RateGroup consolidatedRates = testObject.Consolidate(rateResults);

            RateResult fromGroup1 = FindByServiceAndConfirmationResult(consolidatedRates.Rates, PostalServiceType.ExpressMail, PostalConfirmationType.Delivery);
            RateResult fromGroup2 = FindByServiceAndConfirmationResult(consolidatedRates.Rates, PostalServiceType.ExpressMail, PostalConfirmationType.Signature);

            Assert.IsNotNull(fromGroup1, "Rate from group 1 not included in consolidatedRates");
            Assert.IsNotNull(fromGroup2, "Rate from group 2 not included in consolidatedRates");
        }

        [TestMethod]
        public void Consolidate_CheaperRateFromGroup1Chosen_Test()
        {
            RateGroup consolidatedRates = testObject.Consolidate(rateResults);

            RateResult includedRate = FindByServiceAndConfirmationResult(consolidatedRates.Rates, PostalServiceType.FirstClass, PostalConfirmationType.Signature);

            List<StampsAccountEntity> includedAccounts = GetTag(includedRate).Accounts;

            Assert.AreEqual(account1, includedAccounts.Single(a => true));
            Assert.AreEqual(10, includedRate.Amount);
        }

        [TestMethod]
        public void Consolidate_CheaperRateFromGroup2Chosen_Test()
        {
            RateGroup consolidatedRates = testObject.Consolidate(rateResults);

            RateResult includedRate = FindByServiceAndConfirmationResult(consolidatedRates.Rates, PostalServiceType.FirstClass, PostalConfirmationType.Delivery);

            List<StampsAccountEntity> includedAccounts = GetTag(includedRate).Accounts;

            Assert.AreEqual(account2, includedAccounts.Single(a => true));
            Assert.AreEqual(19, includedRate.Amount);
        }

        [TestMethod]
        public void Consolidate_FooterFromOneRateGroupReturned_FooterIsNotOfTypeUspsRatePromotionFootnote_Test()
        {
            rateGroup1.AddFootnoteFactory(exceptionFootnoteFactory1);

            RateGroup consolidatedRates = testObject.Consolidate(rateResults);

            Assert.IsTrue(consolidatedRates.FootnoteFactories.Contains(exceptionFootnoteFactory1));
        }

        [TestMethod]
        public void Consolidate_FooterFromOneRateGroupReturned_OneExceptionFooterInEachGroup_Test()
        {
            rateGroup1.AddFootnoteFactory(exceptionFootnoteFactory1);
            rateGroup2.AddFootnoteFactory(exceptionFootnoteFactory2);
            
            RateGroup consolidatedRates = testObject.Consolidate(rateResults);

            // It can contain only 1
            Assert.IsTrue(consolidatedRates.FootnoteFactories.Contains(exceptionFootnoteFactory1) ^ consolidatedRates.FootnoteFactories.Contains(exceptionFootnoteFactory2));
        }

        [TestMethod]
        public void Consolidate_FooterFromOneRateGroupReturned_OneUspsRatePromotionFootNoteInEachGroup_Test()
        {
            rateGroup1.AddFootnoteFactory(uspsRatePromotionFootnote1);
            rateGroup2.AddFootnoteFactory(uspsRatePromotionFootnote2);

            RateGroup consolidatedRates = testObject.Consolidate(rateResults);

            // It can contain only 1
            Assert.IsTrue(consolidatedRates.FootnoteFactories.Contains(uspsRatePromotionFootnote1) ^ consolidatedRates.FootnoteFactories.Contains(uspsRatePromotionFootnote2));
        }

        [TestMethod]
        public void Consolidate_NoFooterReturned_UspsRatePromotionFactoryInOneGroupAndNotOther_Test()
        {
            rateGroup1.AddFootnoteFactory(uspsRatePromotionFootnote1);

            RateGroup consolidatedRates = testObject.Consolidate(rateResults);

            // If not in both, don't keep any UspsRatePromotionFactory
            Assert.IsFalse(consolidatedRates.FootnoteFactories.Any());
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