﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ShipWorks.Shipping.Carriers.Amazon.SFP;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Express1.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Express1.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Settings;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.XUnitExtensions.STAThreadAttributes;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Carriers
{
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "IoCRegistration")]
    public class ShipmentTypeSetupWizardRegistrationTest : IDisposable
    {
        readonly IContainer container;

        public ShipmentTypeSetupWizardRegistrationTest()
        {
            container = ContainerInitializer.Build();
        }

        [Theory]
        [InlineData(ShipmentTypeCode.AmazonSFP, typeof(AmazonSFPShipmentSetupWizard))]
        [InlineData(ShipmentTypeCode.Endicia, typeof(EndiciaSetupWizard))]
        [InlineData(ShipmentTypeCode.Express1Endicia, typeof(Express1EndiciaSetupWizard))]
        [InlineData(ShipmentTypeCode.Express1Usps, typeof(Express1UspsSetupWizard))]
        [InlineData(ShipmentTypeCode.FedEx, typeof(FedExSetupWizard))]
        [InlineData(ShipmentTypeCode.iParcel, typeof(iParcelSetupWizard))]
        [InlineData(ShipmentTypeCode.OnTrac, typeof(OnTracSetupWizard))]
        [InlineData(ShipmentTypeCode.Other, typeof(OtherSetupWizard))]
        [InlineData(ShipmentTypeCode.PostalWebTools, typeof(PostalWebSetupWizard))]
        [InlineData(ShipmentTypeCode.UpsOnLineTools, typeof(UpsSetupWizard))]
        [InlineData(ShipmentTypeCode.UpsWorldShip, typeof(WorldShipSetupWizard))]
        [InlineData(ShipmentTypeCode.Usps, typeof(UspsSetupWizard))]
        public void EnsureSetupWizardsAreRegisteredCorrectly(ShipmentTypeCode shipmentType, Type expectedServiceType)
        {
            IShipmentTypeSetupWizard retriever = container.ResolveKeyed<IShipmentTypeSetupWizard>(shipmentType);
            Assert.Equal(expectedServiceType, retriever.GetType());
        }

        [STAFact]
        public void EnsureAllShipmentTypesThatShouldHaveWizardHaveOneRegistered()
        {
            IEnumerable<ShipmentTypeCode> excludedTypes = new[] { ShipmentTypeCode.BestRate, ShipmentTypeCode.None };

            foreach (var value in Enum.GetValues(typeof(ShipmentTypeCode)).OfType<ShipmentTypeCode>().Except(excludedTypes))
            {
                IShipmentTypeSetupWizard service = container.ResolveKeyed<IShipmentTypeSetupWizard>(value);
                Assert.NotNull(service);
            }
        }

        public void Dispose() => container?.Dispose();
    }
}
