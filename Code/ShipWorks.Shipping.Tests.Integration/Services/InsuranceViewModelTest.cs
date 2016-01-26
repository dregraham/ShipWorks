using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Configuration;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Loading;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Services
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class InsuranceViewModelTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly DataContext context;
        private readonly OrderEntity order;
        private readonly ShipmentEntity shipment;
        private readonly InsuranceViewModel testObject;

        public InsuranceViewModelTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            mock = context.Mock;

            order = Modify.Order(context.Order)
                .WithOrderNumber(12345)
                .WithItem()
                .WithItem()
                .Save();

            shipment = Create.Shipment(order)
                .AsUps(s => s.WithPackage())
                .AsPostal(x => x.AsUsps().AsEndicia())
                .AsIParcel(s => s.WithPackage())
                .AsOnTrac()
                .AsAmazon()
                .AsBestRate()
                .AsFedEx(s => s.WithPackage())
                .AsOther()
                .Save();

            testObject = mock.Create<InsuranceViewModel>();
        }

        private const bool pennyOneYes = true;
        private const bool pennyOneNo = false;
        private const bool insuredYes = true;
        private const bool insuredNo = false;
        private const InsuranceProvider providerCarrier = InsuranceProvider.Carrier;
        private const InsuranceProvider providerShipWorks = InsuranceProvider.ShipWorks;
        private const Visibility linkVisibleYes = Visibility.Visible;
        private const Visibility linkVisibleNo = Visibility.Collapsed;
        private const Visibility costVisibleYes = Visibility.Visible;
        private const Visibility costVisibleNo = Visibility.Collapsed;
        private const Visibility infoTipVisibleYes = Visibility.Visible;
        private const Visibility infoTipVisibleNo = Visibility.Collapsed;
        private const string noPromoFormat = "";
        private const string noToolTipText = null;
        private const string usCountryCode = "US";
        private const string caCountryCode = "CA";


        [Theory]
        [InlineData(100.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, noPromoFormat, noToolTipText)]
        [InlineData(101.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $1.70)", noToolTipText)]
        [InlineData(201.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $1.15)", noToolTipText)]
        [InlineData(301.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $1.35)", noToolTipText)]
        [InlineData(401.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $1.55)", noToolTipText)]
        [InlineData(501.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $1.75)", noToolTipText)]
        [InlineData(1001.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $2.75)", noToolTipText)]
        [InlineData(5001.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, noPromoFormat, noToolTipText)]

        [InlineData(100.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, noPromoFormat, noToolTipText)]
        [InlineData(101.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $1.15)", noToolTipText)]
        [InlineData(201.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $0.60)", noToolTipText)]
        [InlineData(301.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $0.80)", noToolTipText)]
        [InlineData(401.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $1.00)", noToolTipText)]
        [InlineData(501.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $1.20)", noToolTipText)]
        [InlineData(1001.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $2.20)", noToolTipText)]
        [InlineData(5001.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, noPromoFormat, noToolTipText)]

        [InlineData(100.00,  pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleYes, "Add coverage for the first $100", "The first $100 of coverage is provided by FedEx. Learn how to add protection\nfor the first $100 in the Shipping Settings for FedEx.\n\nNo ShipWorks Insurance coverage will be provided on this shipment\nsince it will be provided by FedEx.")]
        [InlineData(101.00,  pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Compare to $2.25)", "The first $100 of coverage is provided by FedEx. Learn how to add protection\nfor the first $100 in the Shipping Settings for FedEx.")]
        [InlineData(201.00,  pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Compare to $2.25)", "The first $100 of coverage is provided by FedEx. Learn how to add protection\nfor the first $100 in the Shipping Settings for FedEx.")]
        [InlineData(301.00,  pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Compare to $3.00)", "The first $100 of coverage is provided by FedEx. Learn how to add protection\nfor the first $100 in the Shipping Settings for FedEx.")]
        [InlineData(401.00,  pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Compare to $3.75)", "The first $100 of coverage is provided by FedEx. Learn how to add protection\nfor the first $100 in the Shipping Settings for FedEx.")]
        [InlineData(501.00,  pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Compare to $4.50)", "The first $100 of coverage is provided by FedEx. Learn how to add protection\nfor the first $100 in the Shipping Settings for FedEx.")]
        [InlineData(1001.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Compare to $8.25)", "The first $100 of coverage is provided by FedEx. Learn how to add protection\nfor the first $100 in the Shipping Settings for FedEx.")]
        [InlineData(5001.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleNo,  costVisibleNo, infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.")]

        [InlineData(100.00,  pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText)]
        [InlineData(101.00,  pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.25)", noToolTipText)]
        [InlineData(201.00,  pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.25)", noToolTipText)]
        [InlineData(301.00,  pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $3.00)", noToolTipText)]
        [InlineData(401.00,  pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $3.75)", noToolTipText)]
        [InlineData(501.00,  pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $4.50)", noToolTipText)]
        [InlineData(1001.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $8.25)", noToolTipText)]
        [InlineData(5001.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleNo,  costVisibleNo,  infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.")]
        public void FedEx_ValuesMatch_Test(decimal insuredValue, bool pennyOne,
            InsuranceProvider insuranceProvider, bool insured, 
            Visibility expectedLinkVisibility, Visibility expectedCostVisibility, Visibility expectedInfoTipVisibility,
            string expectedLinkDisplayText, string expectedToolTipText)
        {
            shipment.InsuranceProvider = (int) insuranceProvider;
            shipment.ShipmentTypeCode = ShipmentTypeCode.FedEx;
            shipment.Insurance = insured;

            List<IPackageAdapter> packageAdapters = new List<IPackageAdapter>();
            int index = 0;
            foreach (var package in shipment.FedEx.Packages)
            {
                package.InsurancePennyOne = pennyOne;
                package.Insurance = insured;
                package.InsuranceValue = insuredValue;

                packageAdapters.Add(new FedExPackageAdapter(shipment, package, index));
                index++;
            }

            CarrierShipmentAdapterFactory f = new CarrierShipmentAdapterFactory(IoC.UnsafeGlobalLifetimeScope);
            ICarrierShipmentAdapter shipmentAdapter = f.Get(shipment);

            testObject.Load(packageAdapters, packageAdapters.First(), shipmentAdapter);

            InsuranceCost legacyInsuranceCost = InsuranceUtility.GetInsuranceCost(shipment, insuredValue);

            Assert.Equal(legacyInsuranceCost.Carrier, testObject.InsuranceCost.Carrier);
            Assert.Equal(legacyInsuranceCost.ShipWorks, testObject.InsuranceCost.ShipWorks);
            Assert.Equal(legacyInsuranceCost.AdvertisePennyOne, testObject.InsuranceCost.AdvertisePennyOne);
            Assert.Equal(legacyInsuranceCost.InfoMessage, testObject.InsuranceCost.InfoMessage);
            Assert.Equal(expectedLinkVisibility, testObject.LinkVisibility);
            Assert.Equal(expectedCostVisibility, testObject.CostVisibility);
            Assert.Equal(expectedInfoTipVisibility, testObject.InfoTipVisibility);
            Assert.Equal(expectedLinkDisplayText, testObject.InsuranceLinkDisplayText);
            Assert.Equal(expectedToolTipText, testObject.InsuranceInfoTipDisplayText);
        }

        [Theory]
        [InlineData(100.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, noPromoFormat, noToolTipText)]
        [InlineData(101.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $1.70)", noToolTipText)]
        [InlineData(201.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $1.15)", noToolTipText)]
        [InlineData(301.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $1.35)", noToolTipText)]
        [InlineData(401.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $1.55)", noToolTipText)]
        [InlineData(501.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $1.75)", noToolTipText)]
        [InlineData(1001.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $2.75)", noToolTipText)]
        [InlineData(5001.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, noPromoFormat, noToolTipText)]

        [InlineData(100.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, noPromoFormat, noToolTipText)]
        [InlineData(101.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $1.15)", noToolTipText)]
        [InlineData(201.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $0.60)", noToolTipText)]
        [InlineData(301.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $0.80)", noToolTipText)]
        [InlineData(401.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $1.00)", noToolTipText)]
        [InlineData(501.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $1.20)", noToolTipText)]
        [InlineData(1001.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $2.20)", noToolTipText)]
        [InlineData(5001.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, noPromoFormat, noToolTipText)]

        [InlineData(100.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleYes, "Add coverage for the first $100", "The first $100 of coverage is provided by UPS. Learn how to add protection\nfor the first $100 in the Shipping Settings for UPS.\n\nNo ShipWorks Insurance coverage will be provided on this shipment\nsince it will be provided by UPS.")]
        [InlineData(101.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Compare to $2.25)", "The first $100 of coverage is provided by UPS. Learn how to add protection\nfor the first $100 in the Shipping Settings for UPS.")]
        [InlineData(201.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Compare to $2.25)", "The first $100 of coverage is provided by UPS. Learn how to add protection\nfor the first $100 in the Shipping Settings for UPS.")]
        [InlineData(301.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Compare to $3.00)", "The first $100 of coverage is provided by UPS. Learn how to add protection\nfor the first $100 in the Shipping Settings for UPS.")]
        [InlineData(401.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Compare to $3.75)", "The first $100 of coverage is provided by UPS. Learn how to add protection\nfor the first $100 in the Shipping Settings for UPS.")]
        [InlineData(501.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Compare to $4.50)", "The first $100 of coverage is provided by UPS. Learn how to add protection\nfor the first $100 in the Shipping Settings for UPS.")]
        [InlineData(1001.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Compare to $8.25)", "The first $100 of coverage is provided by UPS. Learn how to add protection\nfor the first $100 in the Shipping Settings for UPS.")]
        [InlineData(5001.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.")]

        [InlineData(100.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText)]
        [InlineData(101.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.25)", noToolTipText)]
        [InlineData(201.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.25)", noToolTipText)]
        [InlineData(301.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $3.00)", noToolTipText)]
        [InlineData(401.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $3.75)", noToolTipText)]
        [InlineData(501.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $4.50)", noToolTipText)]
        [InlineData(1001.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $8.25)", noToolTipText)]
        [InlineData(5001.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.")]
        public void Ups_ValuesMatch_Test(decimal insuredValue, bool pennyOne,
            InsuranceProvider insuranceProvider, bool insured,
            Visibility expectedLinkVisibility, Visibility expectedCostVisibility, Visibility expectedInfoTipVisibility,
            string expectedLinkDisplayText, string expectedToolTipText)
        {
            shipment.InsuranceProvider = (int)insuranceProvider;
            shipment.ShipmentTypeCode = ShipmentTypeCode.UpsOnLineTools;
            shipment.Insurance = insured;

            List<IPackageAdapter> packageAdapters = new List<IPackageAdapter>();
            int index = 0;
            foreach (var package in shipment.Ups.Packages)
            {
                package.InsurancePennyOne = pennyOne;
                package.Insurance = insured;
                package.InsuranceValue = insuredValue;

                packageAdapters.Add(new UpsPackageAdapter(shipment, package, index));
                index++;
            }
            
            CarrierShipmentAdapterFactory f = new CarrierShipmentAdapterFactory(IoC.UnsafeGlobalLifetimeScope);
            ICarrierShipmentAdapter shipmentAdapter = f.Get(shipment);
            
            testObject.Load(packageAdapters, packageAdapters.First(), shipmentAdapter);

            InsuranceCost legacyInsuranceCost = InsuranceUtility.GetInsuranceCost(shipment, insuredValue);

            Assert.Equal(legacyInsuranceCost.Carrier, testObject.InsuranceCost.Carrier);
            Assert.Equal(legacyInsuranceCost.ShipWorks, testObject.InsuranceCost.ShipWorks);
            Assert.Equal(legacyInsuranceCost.AdvertisePennyOne, testObject.InsuranceCost.AdvertisePennyOne);
            Assert.Equal(legacyInsuranceCost.InfoMessage, testObject.InsuranceCost.InfoMessage);

            Assert.Equal(expectedLinkVisibility, testObject.LinkVisibility);
            Assert.Equal(expectedCostVisibility, testObject.CostVisibility);
            Assert.Equal(expectedInfoTipVisibility, testObject.InfoTipVisibility);

            Assert.Equal(expectedLinkDisplayText, testObject.InsuranceLinkDisplayText);
            Assert.Equal(expectedToolTipText, testObject.InsuranceInfoTipDisplayText);
        }

        [Theory]
        [InlineData(100.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, noPromoFormat, noToolTipText)]
        [InlineData(101.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, "", noToolTipText)]
        [InlineData(201.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, "", noToolTipText)]
        [InlineData(301.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, "", noToolTipText)]
        [InlineData(401.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, "", noToolTipText)]
        [InlineData(501.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, "", noToolTipText)]
        [InlineData(1001.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, "", noToolTipText)]
        [InlineData(5001.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, noPromoFormat, noToolTipText)]

        [InlineData(100.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, noPromoFormat, noToolTipText)]
        [InlineData(101.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, "", noToolTipText)]
        [InlineData(201.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, "", noToolTipText)]
        [InlineData(301.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, "", noToolTipText)]
        [InlineData(401.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, "", noToolTipText)]
        [InlineData(501.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, "", noToolTipText)]
        [InlineData(1001.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, "", noToolTipText)]
        [InlineData(5001.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, noPromoFormat, noToolTipText)]

        [InlineData(100.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleYes, "Add coverage for the first $100", "The first $100 of coverage is provided by i-parcel. Learn how to add protection\nfor the first $100 in the Shipping Settings for i-parcel.\n\nNo ShipWorks Insurance coverage will be provided on this shipment\nsince it will be provided by i-parcel.")]
        [InlineData(101.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Learn more)", "The first $100 of coverage is provided by i-parcel. Learn how to add protection\nfor the first $100 in the Shipping Settings for i-parcel.")]
        [InlineData(201.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Learn more)", "The first $100 of coverage is provided by i-parcel. Learn how to add protection\nfor the first $100 in the Shipping Settings for i-parcel.")]
        [InlineData(301.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Learn more)", "The first $100 of coverage is provided by i-parcel. Learn how to add protection\nfor the first $100 in the Shipping Settings for i-parcel.")]
        [InlineData(401.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Learn more)", "The first $100 of coverage is provided by i-parcel. Learn how to add protection\nfor the first $100 in the Shipping Settings for i-parcel.")]
        [InlineData(501.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Learn more)", "The first $100 of coverage is provided by i-parcel. Learn how to add protection\nfor the first $100 in the Shipping Settings for i-parcel.")]
        [InlineData(1001.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Learn more)", "The first $100 of coverage is provided by i-parcel. Learn how to add protection\nfor the first $100 in the Shipping Settings for i-parcel.")]
        [InlineData(5001.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.")]

        [InlineData(100.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText)]
        [InlineData(101.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText)]
        [InlineData(201.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText)]
        [InlineData(301.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText)]
        [InlineData(401.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText)]
        [InlineData(501.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText)]
        [InlineData(1001.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText)]
        [InlineData(5001.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.")]
        public void IParcel_ValuesMatch_Test(decimal insuredValue, bool pennyOne,
            InsuranceProvider insuranceProvider, bool insured,
            Visibility expectedLinkVisibility, Visibility expectedCostVisibility, Visibility expectedInfoTipVisibility,
            string expectedLinkDisplayText, string expectedToolTipText)
        {
            shipment.InsuranceProvider = (int)insuranceProvider;
            shipment.ShipmentTypeCode = ShipmentTypeCode.iParcel;
            shipment.Insurance = insured;

            List<IPackageAdapter> packageAdapters = new List<IPackageAdapter>();
            int index = 0;
            foreach (var package in shipment.IParcel.Packages)
            {
                package.InsurancePennyOne = pennyOne;
                package.Insurance = insured;
                package.InsuranceValue = insuredValue;

                packageAdapters.Add(new iParcelPackageAdapter(shipment, package, index));
                index++;
            }
            
            CarrierShipmentAdapterFactory f = new CarrierShipmentAdapterFactory(IoC.UnsafeGlobalLifetimeScope);
            ICarrierShipmentAdapter shipmentAdapter = f.Get(shipment);
            
            testObject.Load(packageAdapters, packageAdapters.First(), shipmentAdapter);

            InsuranceCost legacyInsuranceCost = InsuranceUtility.GetInsuranceCost(shipment, insuredValue);

            Assert.Equal(legacyInsuranceCost.Carrier, testObject.InsuranceCost.Carrier);
            Assert.Equal(legacyInsuranceCost.ShipWorks, testObject.InsuranceCost.ShipWorks);
            Assert.Equal(legacyInsuranceCost.AdvertisePennyOne, testObject.InsuranceCost.AdvertisePennyOne);
            Assert.Equal(legacyInsuranceCost.InfoMessage, testObject.InsuranceCost.InfoMessage);

            Assert.Equal(expectedLinkVisibility, testObject.LinkVisibility);
            Assert.Equal(expectedCostVisibility, testObject.CostVisibility);
            Assert.Equal(expectedInfoTipVisibility, testObject.InfoTipVisibility);

            Assert.Equal(expectedLinkDisplayText, testObject.InsuranceLinkDisplayText);
            Assert.Equal(expectedToolTipText, testObject.InsuranceInfoTipDisplayText);
        }

        [Theory]
        [InlineData(100.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, noPromoFormat, noToolTipText)]
        [InlineData(101.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $0.25)", noToolTipText)]
        [InlineData(201.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $0.50)", noToolTipText)]
        [InlineData(301.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $0.75)", noToolTipText)]
        [InlineData(401.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $1.00)", noToolTipText)]
        [InlineData(501.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $1.25)", noToolTipText)]
        [InlineData(1001.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $2.50)", noToolTipText)]
        [InlineData(5001.00, pennyOneNo, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, noPromoFormat, noToolTipText)]

        [InlineData(100.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, noPromoFormat, noToolTipText)]
        [InlineData(101.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, noPromoFormat, noToolTipText)]
        [InlineData(201.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, noPromoFormat, noToolTipText)]
        [InlineData(301.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $0.20)", noToolTipText)]
        [InlineData(401.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $0.45)", noToolTipText)]
        [InlineData(501.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $0.70)", noToolTipText)]
        [InlineData(1001.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleNo, "(Learn how to save $1.95)", noToolTipText)]
        [InlineData(5001.00, pennyOneYes, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleNo, noPromoFormat, noToolTipText)]

        [InlineData(100.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleNo, infoTipVisibleYes, "Add coverage for the first $100", "The first $100 of coverage is provided by OnTrac. Learn how to add protection\nfor the first $100 in the Shipping Settings for OnTrac.\n\nNo ShipWorks Insurance coverage will be provided on this shipment\nsince it will be provided by OnTrac.")]
        [InlineData(101.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Compare to $0.80)", "The first $100 of coverage is provided by OnTrac. Learn how to add protection\nfor the first $100 in the Shipping Settings for OnTrac.")]
        [InlineData(201.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Compare to $1.60)", "The first $100 of coverage is provided by OnTrac. Learn how to add protection\nfor the first $100 in the Shipping Settings for OnTrac.")]
        [InlineData(301.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Compare to $2.40)", "The first $100 of coverage is provided by OnTrac. Learn how to add protection\nfor the first $100 in the Shipping Settings for OnTrac.")]
        [InlineData(401.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Compare to $3.20)", "The first $100 of coverage is provided by OnTrac. Learn how to add protection\nfor the first $100 in the Shipping Settings for OnTrac.")]
        [InlineData(501.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Compare to $4.00)", "The first $100 of coverage is provided by OnTrac. Learn how to add protection\nfor the first $100 in the Shipping Settings for OnTrac.")]
        [InlineData(1001.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleYes, "(Compare to $8.00)", "The first $100 of coverage is provided by OnTrac. Learn how to add protection\nfor the first $100 in the Shipping Settings for OnTrac.")]
        [InlineData(5001.00, pennyOneNo, providerShipWorks, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.")]

        [InlineData(100.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText)]
        [InlineData(101.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText)]
        [InlineData(201.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText)]
        [InlineData(301.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.40)", noToolTipText)]
        [InlineData(401.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $3.20)", noToolTipText)]
        [InlineData(501.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $4.00)", noToolTipText)]
        [InlineData(1001.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $8.00)", noToolTipText)]
        [InlineData(5001.00, pennyOneYes, providerShipWorks, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.")]
        public void OnTrac_ValuesMatch_Test(decimal insuredValue, bool pennyOne,
            InsuranceProvider insuranceProvider, bool insured,
            Visibility expectedLinkVisibility, Visibility expectedCostVisibility, Visibility expectedInfoTipVisibility,
            string expectedLinkDisplayText, string expectedToolTipText)
        {
            shipment.InsuranceProvider = (int)insuranceProvider;
            shipment.ShipmentTypeCode = ShipmentTypeCode.OnTrac;
            shipment.Insurance = insured;

            shipment.OnTrac.InsurancePennyOne = pennyOne;
            shipment.OnTrac.InsuranceValue = insuredValue;

            List<IPackageAdapter> packageAdapters = new List<IPackageAdapter>();
            packageAdapters.Add(new OnTracPackageAdapter(shipment));

            CarrierShipmentAdapterFactory f = new CarrierShipmentAdapterFactory(IoC.UnsafeGlobalLifetimeScope);
            ICarrierShipmentAdapter shipmentAdapter = f.Get(shipment);

            testObject.Load(packageAdapters, packageAdapters.First(), shipmentAdapter);

            InsuranceCost legacyInsuranceCost = InsuranceUtility.GetInsuranceCost(shipment, insuredValue);

            Assert.Equal(legacyInsuranceCost.Carrier, testObject.InsuranceCost.Carrier);
            Assert.Equal(legacyInsuranceCost.ShipWorks, testObject.InsuranceCost.ShipWorks);
            Assert.Equal(legacyInsuranceCost.AdvertisePennyOne, testObject.InsuranceCost.AdvertisePennyOne);
            Assert.Equal(legacyInsuranceCost.InfoMessage, testObject.InsuranceCost.InfoMessage);

            Assert.Equal(expectedLinkVisibility, testObject.LinkVisibility);
            Assert.Equal(expectedCostVisibility, testObject.CostVisibility);
            Assert.Equal(expectedInfoTipVisibility, testObject.InfoTipVisibility);

            Assert.Equal(expectedLinkDisplayText, testObject.InsuranceLinkDisplayText);
            Assert.Equal(expectedToolTipText, testObject.InsuranceInfoTipDisplayText);
        }

        [Theory]
        [InlineData(50.00,  providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $1.80)", noToolTipText, usCountryCode)]
        [InlineData(100.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.30)", noToolTipText, usCountryCode)]
        [InlineData(101.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.85)", noToolTipText, usCountryCode)]
        [InlineData(201.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $4.75)", noToolTipText, usCountryCode)]
        [InlineData(301.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $5.80)", noToolTipText, usCountryCode)]
        [InlineData(401.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $6.85)", noToolTipText, usCountryCode)]
        [InlineData(501.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $7.90)", noToolTipText, usCountryCode)]
        [InlineData(1001.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $13.15)", noToolTipText, usCountryCode)]
        [InlineData(5001.00, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.", usCountryCode)]

        [InlineData(50.00,  providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.30)", noToolTipText, caCountryCode)]
        [InlineData(100.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $4.60)", noToolTipText, caCountryCode)]
        [InlineData(101.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $6.90)", noToolTipText, caCountryCode)]
        [InlineData(201.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $11.50)", noToolTipText, caCountryCode)]
        [InlineData(301.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $16.10)", noToolTipText, caCountryCode)]
        [InlineData(401.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $20.70)", noToolTipText, caCountryCode)]
        [InlineData(501.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $25.30)", noToolTipText, caCountryCode)]
        [InlineData(1001.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $48.30)", noToolTipText, caCountryCode)]
        [InlineData(5001.00, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.", caCountryCode)]

        [InlineData(50.00,  providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $1.80)", noToolTipText, usCountryCode)]
        [InlineData(100.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.30)", noToolTipText, usCountryCode)]
        [InlineData(101.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.85)", noToolTipText, usCountryCode)]
        [InlineData(201.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $4.75)", noToolTipText, usCountryCode)]
        [InlineData(301.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $5.80)", noToolTipText, usCountryCode)]
        [InlineData(401.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $6.85)", noToolTipText, usCountryCode)]
        [InlineData(501.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $7.90)", noToolTipText, usCountryCode)]
        [InlineData(1001.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $13.15)", noToolTipText, usCountryCode)]
        [InlineData(5001.00, providerShipWorks, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.", usCountryCode)]

        [InlineData(50.00,  providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.30)", noToolTipText, caCountryCode)]
        [InlineData(100.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $4.60)", noToolTipText, caCountryCode)]
        [InlineData(101.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $6.90)", noToolTipText, caCountryCode)]
        [InlineData(201.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $11.50)", noToolTipText, caCountryCode)]
        [InlineData(301.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $16.10)", noToolTipText, caCountryCode)]
        [InlineData(401.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $20.70)", noToolTipText, caCountryCode)]
        [InlineData(501.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $25.30)", noToolTipText, caCountryCode)]
        [InlineData(1001.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $48.30)", noToolTipText, caCountryCode)]
        [InlineData(5001.00, providerShipWorks, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.", caCountryCode)]
        public void Endicia_ValuesMatch_Test(decimal insuredValue, InsuranceProvider insuranceProvider, bool insured,
            Visibility expectedLinkVisibility, Visibility expectedCostVisibility, Visibility expectedInfoTipVisibility,
            string expectedLinkDisplayText, string expectedToolTipText, string shipCountryCode)
        {
            Postal_ValuesMatch(ShipmentTypeCode.Endicia, insuredValue, insuranceProvider, insured,
                expectedLinkVisibility, expectedCostVisibility, expectedInfoTipVisibility, expectedLinkDisplayText, expectedToolTipText, shipCountryCode);
        }

        [Theory]
        [InlineData(50.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $1.80)", noToolTipText, usCountryCode)]
        [InlineData(100.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.30)", noToolTipText, usCountryCode)]
        [InlineData(101.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.85)", noToolTipText, usCountryCode)]
        [InlineData(201.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $4.75)", noToolTipText, usCountryCode)]
        [InlineData(301.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $5.80)", noToolTipText, usCountryCode)]
        [InlineData(401.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $6.85)", noToolTipText, usCountryCode)]
        [InlineData(501.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $7.90)", noToolTipText, usCountryCode)]
        [InlineData(1001.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $13.15)", noToolTipText, usCountryCode)]
        [InlineData(5001.00, providerShipWorks, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.", usCountryCode)]

        [InlineData(50.00,  providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.30)", noToolTipText, caCountryCode)]
        [InlineData(100.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $4.60)", noToolTipText, caCountryCode)]
        [InlineData(101.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $6.90)", noToolTipText, caCountryCode)]
        [InlineData(201.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $11.50)", noToolTipText, caCountryCode)]
        [InlineData(301.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $16.10)", noToolTipText, caCountryCode)]
        [InlineData(401.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $20.70)", noToolTipText, caCountryCode)]
        [InlineData(501.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $25.30)", noToolTipText, caCountryCode)]
        [InlineData(1001.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $48.30)", noToolTipText, caCountryCode)]
        [InlineData(5001.00, providerShipWorks, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.", caCountryCode)]
        public void Express1Endicia_ValuesMatch_Test(decimal insuredValue, InsuranceProvider insuranceProvider, bool insured,
            Visibility expectedLinkVisibility, Visibility expectedCostVisibility, Visibility expectedInfoTipVisibility,
            string expectedLinkDisplayText, string expectedToolTipText, string shipCountryCode)
        {
            Postal_ValuesMatch(ShipmentTypeCode.Express1Endicia, insuredValue, insuranceProvider, insured,
                expectedLinkVisibility, expectedCostVisibility, expectedInfoTipVisibility, expectedLinkDisplayText, expectedToolTipText, shipCountryCode);
        }

        [Theory]
        [InlineData(50.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $1.80)", noToolTipText, usCountryCode)]
        [InlineData(100.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.30)", noToolTipText, usCountryCode)]
        [InlineData(101.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.85)", noToolTipText, usCountryCode)]
        [InlineData(201.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $4.75)", noToolTipText, usCountryCode)]
        [InlineData(301.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $5.80)", noToolTipText, usCountryCode)]
        [InlineData(401.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $6.85)", noToolTipText, usCountryCode)]
        [InlineData(501.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $7.90)", noToolTipText, usCountryCode)]
        [InlineData(1001.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $13.15)", noToolTipText, usCountryCode)]
        [InlineData(5001.00, providerShipWorks, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.", usCountryCode)]

        [InlineData(50.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.30)", noToolTipText, caCountryCode)]
        [InlineData(100.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $4.60)", noToolTipText, caCountryCode)]
        [InlineData(101.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $6.90)", noToolTipText, caCountryCode)]
        [InlineData(201.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $11.50)", noToolTipText, caCountryCode)]
        [InlineData(301.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $16.10)", noToolTipText, caCountryCode)]
        [InlineData(401.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $20.70)", noToolTipText, caCountryCode)]
        [InlineData(501.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $25.30)", noToolTipText, caCountryCode)]
        [InlineData(1001.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $48.30)", noToolTipText, caCountryCode)]
        [InlineData(5001.00, providerShipWorks, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.", caCountryCode)]
        public void Express1Usps_ValuesMatch_Test(decimal insuredValue, InsuranceProvider insuranceProvider, bool insured,
            Visibility expectedLinkVisibility, Visibility expectedCostVisibility, Visibility expectedInfoTipVisibility,
            string expectedLinkDisplayText, string expectedToolTipText, string shipCountryCode)
        {
            Postal_ValuesMatch(ShipmentTypeCode.Express1Usps, insuredValue, insuranceProvider, insured,
                expectedLinkVisibility, expectedCostVisibility, expectedInfoTipVisibility, expectedLinkDisplayText, expectedToolTipText, shipCountryCode);
        }

        [Theory]
        [InlineData(50.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $1.80)", noToolTipText, usCountryCode)]
        [InlineData(100.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.30)", noToolTipText, usCountryCode)]
        [InlineData(101.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.85)", noToolTipText, usCountryCode)]
        [InlineData(201.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $4.75)", noToolTipText, usCountryCode)]
        [InlineData(301.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $5.80)", noToolTipText, usCountryCode)]
        [InlineData(401.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $6.85)", noToolTipText, usCountryCode)]
        [InlineData(501.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $7.90)", noToolTipText, usCountryCode)]
        [InlineData(1001.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $13.15)", noToolTipText, usCountryCode)]
        [InlineData(5001.00, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.", usCountryCode)]

        [InlineData(50.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.30)", noToolTipText, caCountryCode)]
        [InlineData(100.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $4.60)", noToolTipText, caCountryCode)]
        [InlineData(101.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $6.90)", noToolTipText, caCountryCode)]
        [InlineData(201.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $11.50)", noToolTipText, caCountryCode)]
        [InlineData(301.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $16.10)", noToolTipText, caCountryCode)]
        [InlineData(401.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $20.70)", noToolTipText, caCountryCode)]
        [InlineData(501.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $25.30)", noToolTipText, caCountryCode)]
        [InlineData(1001.00, providerCarrier, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $48.30)", noToolTipText, caCountryCode)]
        [InlineData(5001.00, providerCarrier, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.", caCountryCode)]

        [InlineData(50, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $1.80)", noToolTipText, usCountryCode)]
        [InlineData(100.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.30)", noToolTipText, usCountryCode)]
        [InlineData(101.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.85)", noToolTipText, usCountryCode)]
        [InlineData(201.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $4.75)", noToolTipText, usCountryCode)]
        [InlineData(301.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $5.80)", noToolTipText, usCountryCode)]
        [InlineData(401.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $6.85)", noToolTipText, usCountryCode)]
        [InlineData(501.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $7.90)", noToolTipText, usCountryCode)]
        [InlineData(1001.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $13.15)", noToolTipText, usCountryCode)]
        [InlineData(5001.00, providerShipWorks, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.", usCountryCode)]

        [InlineData(50.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.30)", noToolTipText, caCountryCode)]
        [InlineData(100.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $4.60)", noToolTipText, caCountryCode)]
        [InlineData(101.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $6.90)", noToolTipText, caCountryCode)]
        [InlineData(201.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $11.50)", noToolTipText, caCountryCode)]
        [InlineData(301.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $16.10)", noToolTipText, caCountryCode)]
        [InlineData(401.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $20.70)", noToolTipText, caCountryCode)]
        [InlineData(501.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $25.30)", noToolTipText, caCountryCode)]
        [InlineData(1001.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $48.30)", noToolTipText, caCountryCode)]
        [InlineData(5001.00, providerShipWorks, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.", caCountryCode)]
        public void Usps_ValuesMatch_Test(decimal insuredValue, InsuranceProvider insuranceProvider, bool insured,
            Visibility expectedLinkVisibility, Visibility expectedCostVisibility, Visibility expectedInfoTipVisibility,
            string expectedLinkDisplayText, string expectedToolTipText, string shipCountryCode)
        {
            Postal_ValuesMatch(ShipmentTypeCode.Usps, insuredValue, insuranceProvider, insured,
                expectedLinkVisibility, expectedCostVisibility, expectedInfoTipVisibility, expectedLinkDisplayText, expectedToolTipText, shipCountryCode);
        }

        [Theory]
        [InlineData(50, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $1.80)", noToolTipText, usCountryCode)]
        [InlineData(100.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.30)", noToolTipText, usCountryCode)]
        [InlineData(101.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.85)", noToolTipText, usCountryCode)]
        [InlineData(201.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $4.75)", noToolTipText, usCountryCode)]
        [InlineData(301.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $5.80)", noToolTipText, usCountryCode)]
        [InlineData(401.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $6.85)", noToolTipText, usCountryCode)]
        [InlineData(501.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $7.90)", noToolTipText, usCountryCode)]
        [InlineData(1001.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $13.15)", noToolTipText, usCountryCode)]
        [InlineData(5001.00, providerShipWorks, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.", usCountryCode)]

        [InlineData(50.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.30)", noToolTipText, caCountryCode)]
        [InlineData(100.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $4.60)", noToolTipText, caCountryCode)]
        [InlineData(101.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $6.90)", noToolTipText, caCountryCode)]
        [InlineData(201.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $11.50)", noToolTipText, caCountryCode)]
        [InlineData(301.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $16.10)", noToolTipText, caCountryCode)]
        [InlineData(401.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $20.70)", noToolTipText, caCountryCode)]
        [InlineData(501.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $25.30)", noToolTipText, caCountryCode)]
        [InlineData(1001.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $48.30)", noToolTipText, caCountryCode)]
        [InlineData(5001.00, providerShipWorks, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.", caCountryCode)]
        public void PostalWebTools_ValuesMatch_Test(decimal insuredValue, InsuranceProvider insuranceProvider, bool insured,
            Visibility expectedLinkVisibility, Visibility expectedCostVisibility, Visibility expectedInfoTipVisibility,
            string expectedLinkDisplayText, string expectedToolTipText, string shipCountryCode)
        {
            Postal_ValuesMatch(ShipmentTypeCode.PostalWebTools, insuredValue, insuranceProvider, insured,
                expectedLinkVisibility, expectedCostVisibility, expectedInfoTipVisibility, expectedLinkDisplayText, expectedToolTipText, shipCountryCode);
        }

        [Theory]
        [InlineData(50, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $1.80)", usCountryCode)]
        [InlineData(100.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.30)", usCountryCode)]
        [InlineData(101.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.85)", usCountryCode)]
        [InlineData(201.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $4.75)", usCountryCode)]
        [InlineData(301.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $5.80)", usCountryCode)]
        [InlineData(401.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $6.85)", usCountryCode)]
        [InlineData(501.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $7.90)", usCountryCode)]
        [InlineData(1001.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $13.15)", usCountryCode)]
        [InlineData(5001.00, providerShipWorks, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, usCountryCode)]

        [InlineData(50.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $2.30)", caCountryCode)]
        [InlineData(100.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $4.60)", caCountryCode)]
        [InlineData(101.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $6.90)", caCountryCode)]
        [InlineData(201.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $11.50)", caCountryCode)]
        [InlineData(301.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $16.10)", caCountryCode)]
        [InlineData(401.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $20.70)", caCountryCode)]
        [InlineData(501.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $25.30)", caCountryCode)]
        [InlineData(1001.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Compare to $48.30)", caCountryCode)]
        [InlineData(5001.00, providerShipWorks, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, caCountryCode)]
        public void None_ValuesMatch_Test(decimal insuredValue, InsuranceProvider insuranceProvider, bool insured,
            Visibility expectedLinkVisibility, Visibility expectedCostVisibility, Visibility expectedInfoTipVisibility,
            string expectedLinkDisplayText, string shipCountryCode)
        {
            None_ValuesMatch(insuredValue, insuranceProvider, insured, shipCountryCode);
        }


        [Theory]
        [InlineData(50, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText, usCountryCode)]
        [InlineData(100.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText, usCountryCode)]
        [InlineData(101.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText, usCountryCode)]
        [InlineData(201.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText, usCountryCode)]
        [InlineData(301.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText, usCountryCode)]
        [InlineData(401.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText, usCountryCode)]
        [InlineData(501.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText, usCountryCode)]
        [InlineData(1001.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText, usCountryCode)]
        [InlineData(5001.00, providerShipWorks, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.", usCountryCode)]

        [InlineData(50.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText, caCountryCode)]
        [InlineData(100.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText, caCountryCode)]
        [InlineData(101.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText, caCountryCode)]
        [InlineData(201.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText, caCountryCode)]
        [InlineData(301.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText, caCountryCode)]
        [InlineData(401.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText, caCountryCode)]
        [InlineData(501.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText, caCountryCode)]
        [InlineData(1001.00, providerShipWorks, insuredYes, linkVisibleYes, costVisibleYes, infoTipVisibleNo, "(Learn more)", noToolTipText, caCountryCode)]
        [InlineData(5001.00, providerShipWorks, insuredYes, linkVisibleNo, costVisibleNo, infoTipVisibleYes, null, "ShipWorks Insurance can only cover up to $5000 in declared value.", caCountryCode)]
        public void Other_ValuesMatch_Test(decimal insuredValue, InsuranceProvider insuranceProvider, bool insured,
            Visibility expectedLinkVisibility, Visibility expectedCostVisibility, Visibility expectedInfoTipVisibility,
            string expectedLinkDisplayText, string expectedToolTipText, string shipCountryCode)
        {
            Other_ValuesMatch(insuredValue, insuranceProvider, insured,
                expectedLinkVisibility, expectedCostVisibility, expectedInfoTipVisibility, expectedLinkDisplayText, expectedToolTipText, shipCountryCode);
        }

        private void Postal_ValuesMatch(ShipmentTypeCode shipmentTypeCode, decimal insuredValue, InsuranceProvider insuranceProvider, bool insured,
            Visibility expectedLinkVisibility, Visibility expectedCostVisibility, Visibility expectedInfoTipVisibility,
            string expectedLinkDisplayText, string expectedToolTipText, string shipCountryCode)
        {
            shipment.ShipCountryCode = shipCountryCode;
            shipment.InsuranceProvider = (int)insuranceProvider;
            shipment.ShipmentTypeCode = shipmentTypeCode;
            shipment.Insurance = insured;

            shipment.Postal.InsuranceValue = insuredValue;

            CarrierShipmentAdapterFactory f = new CarrierShipmentAdapterFactory(IoC.UnsafeGlobalLifetimeScope);
            ICarrierShipmentAdapter shipmentAdapter = f.Get(shipment);
            
            List<IPackageAdapter> packageAdapters = shipmentAdapter.GetPackageAdapters().ToList();

            testObject.Load(packageAdapters, packageAdapters.First(), shipmentAdapter);

            InsuranceCost legacyInsuranceCost = InsuranceUtility.GetInsuranceCost(shipment, insuredValue);

            Assert.Equal(legacyInsuranceCost.Carrier, testObject.InsuranceCost.Carrier);
            Assert.Equal(legacyInsuranceCost.ShipWorks, testObject.InsuranceCost.ShipWorks);
            Assert.Equal(legacyInsuranceCost.AdvertisePennyOne, testObject.InsuranceCost.AdvertisePennyOne);
            Assert.Equal(legacyInsuranceCost.InfoMessage, testObject.InsuranceCost.InfoMessage);

            Assert.Equal(expectedLinkVisibility, testObject.LinkVisibility);
            Assert.Equal(expectedCostVisibility, testObject.CostVisibility);
            Assert.Equal(expectedInfoTipVisibility, testObject.InfoTipVisibility);

            Assert.Equal(expectedLinkDisplayText, testObject.InsuranceLinkDisplayText);
            Assert.Equal(expectedToolTipText, testObject.InsuranceInfoTipDisplayText);
        }


        private void Other_ValuesMatch(decimal insuredValue, InsuranceProvider insuranceProvider, bool insured,
            Visibility expectedLinkVisibility, Visibility expectedCostVisibility, Visibility expectedInfoTipVisibility,
            string expectedLinkDisplayText, string expectedToolTipText, string shipCountryCode)
        {
            shipment.ShipCountryCode = shipCountryCode;
            shipment.InsuranceProvider = (int)insuranceProvider;
            shipment.ShipmentTypeCode = ShipmentTypeCode.Other;
            shipment.Insurance = insured;

            shipment.Other.InsuranceValue = insuredValue;

            CarrierShipmentAdapterFactory f = new CarrierShipmentAdapterFactory(IoC.UnsafeGlobalLifetimeScope);
            ICarrierShipmentAdapter shipmentAdapter = f.Get(shipment);

            List<IPackageAdapter> packageAdapters = shipmentAdapter.GetPackageAdapters().ToList();

            testObject.Load(packageAdapters, packageAdapters.First(), shipmentAdapter);

            InsuranceCost legacyInsuranceCost = InsuranceUtility.GetInsuranceCost(shipment, insuredValue);

            Assert.Equal(legacyInsuranceCost.Carrier, testObject.InsuranceCost.Carrier);
            Assert.Equal(legacyInsuranceCost.ShipWorks, testObject.InsuranceCost.ShipWorks);
            Assert.Equal(legacyInsuranceCost.AdvertisePennyOne, testObject.InsuranceCost.AdvertisePennyOne);
            Assert.Equal(legacyInsuranceCost.InfoMessage, testObject.InsuranceCost.InfoMessage);

            Assert.Equal(expectedLinkVisibility, testObject.LinkVisibility);
            Assert.Equal(expectedCostVisibility, testObject.CostVisibility);
            Assert.Equal(expectedInfoTipVisibility, testObject.InfoTipVisibility);

            Assert.Equal(expectedLinkDisplayText, testObject.InsuranceLinkDisplayText);
            Assert.Equal(expectedToolTipText, testObject.InsuranceInfoTipDisplayText);
        }

        private void None_ValuesMatch(decimal insuredValue, InsuranceProvider insuranceProvider, bool insured, string shipCountryCode)
        {
            shipment.ShipCountryCode = shipCountryCode;
            shipment.InsuranceProvider = (int)insuranceProvider;
            shipment.ShipmentTypeCode = ShipmentTypeCode.None;
            shipment.Insurance = insured;

            shipment.Postal.InsuranceValue = insuredValue;

            CarrierShipmentAdapterFactory f = new CarrierShipmentAdapterFactory(IoC.UnsafeGlobalLifetimeScope);
            ICarrierShipmentAdapter shipmentAdapter = f.Get(shipment);

            List<IPackageAdapter> packageAdapters = shipmentAdapter.GetPackageAdapters().ToList();

            testObject.Load(packageAdapters, packageAdapters.First(), shipmentAdapter);

            Assert.Null(testObject.InsuranceChoice);
            Assert.Null(testObject.InsuranceCost);

            Assert.Equal(Visibility.Collapsed, testObject.LinkVisibility);
            Assert.Equal(Visibility.Collapsed, testObject.CostVisibility);
            Assert.Equal(Visibility.Collapsed, testObject.InfoTipVisibility);

            Assert.Null(testObject.InsuranceLinkDisplayText);
            Assert.Null(testObject.InsuranceInfoTipDisplayText);
        }

        public void Dispose() => context.Dispose();
    }
}
