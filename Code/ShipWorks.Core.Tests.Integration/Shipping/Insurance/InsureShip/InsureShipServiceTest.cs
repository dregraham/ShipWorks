using System;
using Autofac;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Shipping.Insurance.InsureShip
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class InsureShipServiceTest : IDisposable
    {
        private readonly DataContext context;
        private readonly SqlAdapter adapter;
        private ShipmentEntity shipment;
        private IInsureShipService testObject;

        public InsureShipServiceTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));


            adapter = new SqlAdapter(false);
        }

        [Theory]
        [InlineData(false, 0, InsuranceProvider.Carrier, false, false)]
        [InlineData(false, 50, InsuranceProvider.Carrier, false, false)]
        [InlineData(false, 100, InsuranceProvider.Carrier, false, false)]
        [InlineData(false, 100, InsuranceProvider.ShipWorks, false, false)]

        [InlineData(true, 0, InsuranceProvider.Carrier, false, false)]
        [InlineData(true, 50, InsuranceProvider.Carrier, false, false)]
        [InlineData(true, 100, InsuranceProvider.Carrier, false, false)]
        [InlineData(true, 100.01, InsuranceProvider.ShipWorks, false, true)]

        [InlineData(true, 0, InsuranceProvider.Carrier, true, false)]
        [InlineData(true, 50, InsuranceProvider.Carrier, true, false)]
        [InlineData(true, 100, InsuranceProvider.Carrier, true, false)]
        [InlineData(true, 100, InsuranceProvider.ShipWorks, true, true)]

        [InlineData(true, 1, InsuranceProvider.ShipWorks, true, true)]
        [InlineData(true, 50, InsuranceProvider.ShipWorks, true, true)]
        public void IsInsuredByInsureShip_ReturnsTrue_FedEx_WhenSwInsuranceAndValueOver100(bool insured, decimal insureValue, InsuranceProvider insuranceProvider, bool pennyOne, bool result)
        {
            shipment = Create.Shipment(context.Order)
                .AsFedEx(x => x.WithPackage(p => 
                    p.Set(ps => ps.DeclaredValue, insureValue)
                        .Set(ps => ps.Insurance, insured)
                        .Set(ps => ps.InsuranceValue, insureValue)
                        .Set(ps => ps.InsurancePennyOne, pennyOne)))
                .Set(s => s.Insurance, insured)
                .Set(s => s.InsuranceProvider, (int) insuranceProvider)
                .Save();

            using (var scope = context.Container.BeginLifetimeScope())
            {
                testObject = scope.Resolve<IInsureShipService>();
                Assert.Equal(result, testObject.IsInsuredByInsureShip(shipment));
            }
        }

        [Theory]
        [InlineData(false, 0, InsuranceProvider.Carrier, false, false)]
        [InlineData(false, 50, InsuranceProvider.Carrier, false, false)]
        [InlineData(false, 100, InsuranceProvider.Carrier, false, false)]
        [InlineData(false, 100, InsuranceProvider.ShipWorks, false, false)]

        [InlineData(true, 0, InsuranceProvider.Carrier, false, false)]
        [InlineData(true, 50, InsuranceProvider.Carrier, false, false)]
        [InlineData(true, 100, InsuranceProvider.Carrier, false, false)]
        [InlineData(true, 100.01, InsuranceProvider.ShipWorks, false, true)]

        [InlineData(true, 0, InsuranceProvider.Carrier, true, false)]
        [InlineData(true, 50, InsuranceProvider.Carrier, true, false)]
        [InlineData(true, 100, InsuranceProvider.Carrier, true, false)]
        [InlineData(true, 100, InsuranceProvider.ShipWorks, true, true)]

        [InlineData(true, 1, InsuranceProvider.ShipWorks, true, true)]
        [InlineData(true, 50, InsuranceProvider.ShipWorks, true, true)]
        public void IsInsuredByInsureShip_ReturnsTrue_UPS_WhenSwInsuranceAndValueOver100(bool insured, decimal insureValue, InsuranceProvider insuranceProvider, bool pennyOne, bool result)
        {
            shipment = Create.Shipment(context.Order)
                .AsUps(x => x.WithPackage(p =>
                    p.Set(ps => ps.DeclaredValue, insureValue)
                        .Set(ps => ps.Insurance, insured)
                        .Set(ps => ps.InsuranceValue, insureValue)
                        .Set(ps => ps.InsurancePennyOne, pennyOne)))
                .Set(s => s.Insurance, insured)
                .Set(s => s.InsuranceProvider, (int)insuranceProvider)
                .Save();

            using (var scope = context.Container.BeginLifetimeScope())
            {
                testObject = scope.Resolve<IInsureShipService>();
                Assert.Equal(result, testObject.IsInsuredByInsureShip(shipment));
            }
        }

        [Theory]
        [InlineData(false, 0, InsuranceProvider.Carrier, false, false)]
        [InlineData(false, 50, InsuranceProvider.Carrier, false, false)]
        [InlineData(false, 100, InsuranceProvider.Carrier, false, false)]
        [InlineData(false, 100, InsuranceProvider.ShipWorks, false, false)]

        [InlineData(true, 0, InsuranceProvider.Carrier, false, false)]
        [InlineData(true, 50, InsuranceProvider.Carrier, false, false)]
        [InlineData(true, 100, InsuranceProvider.Carrier, false, false)]
        [InlineData(true, 100.01, InsuranceProvider.ShipWorks, false, true)]

        [InlineData(true, 0, InsuranceProvider.Carrier, true, false)]
        [InlineData(true, 50, InsuranceProvider.Carrier, true, false)]
        [InlineData(true, 100, InsuranceProvider.Carrier, true, false)]
        [InlineData(true, 100, InsuranceProvider.ShipWorks, true, true)]

        [InlineData(true, 1, InsuranceProvider.ShipWorks, true, true)]
        [InlineData(true, 50, InsuranceProvider.ShipWorks, true, true)]
        public void IsInsuredByInsureShip_ReturnsTrue_OnTrac_WhenSwInsuranceAndValueOver100(bool insured, decimal insureValue, InsuranceProvider insuranceProvider, bool pennyOne, bool result)
        {
            shipment = Create.Shipment(context.Order)
                .AsOnTrac(p =>
                    p.Set(ps => ps.DeclaredValue, insureValue)
                        .Set(ps => ps.Insurance, insured)
                        .Set(ps => ps.InsuranceValue, insureValue)
                        .Set(ps => ps.InsurancePennyOne, pennyOne))
                .Set(s => s.Insurance, insured)
                .Set(s => s.InsuranceProvider, (int)insuranceProvider)
                .Save();

            using (var scope = context.Container.BeginLifetimeScope())
            {
                testObject = scope.Resolve<IInsureShipService>();
                Assert.Equal(result, testObject.IsInsuredByInsureShip(shipment));
            }
        }

        [Theory]
        [InlineData(false, 0, InsuranceProvider.Carrier, false, false)]
        [InlineData(false, 50, InsuranceProvider.Carrier, false, false)]
        [InlineData(false, 100, InsuranceProvider.Carrier, false, false)]
        [InlineData(false, 100, InsuranceProvider.ShipWorks, false, false)]

        [InlineData(true, 0, InsuranceProvider.Carrier, false, false)]
        [InlineData(true, 50, InsuranceProvider.Carrier, false, false)]
        [InlineData(true, 100, InsuranceProvider.Carrier, false, false)]
        [InlineData(true, 100.01, InsuranceProvider.ShipWorks, false, true)]

        [InlineData(true, 0, InsuranceProvider.Carrier, true, false)]
        [InlineData(true, 50, InsuranceProvider.Carrier, true, false)]
        [InlineData(true, 100, InsuranceProvider.Carrier, true, false)]
        [InlineData(true, 100, InsuranceProvider.ShipWorks, true, true)]

        [InlineData(true, 1, InsuranceProvider.ShipWorks, true, true)]
        [InlineData(true, 50, InsuranceProvider.ShipWorks, true, true)]
        public void IsInsuredByInsureShip_ReturnsTrue_iParcel_WhenSwInsuranceAndValueOver100(bool insured, decimal insureValue, InsuranceProvider insuranceProvider, bool pennyOne, bool result)
        {
            shipment = Create.Shipment(context.Order)
                .AsIParcel(x => x.WithPackage(p =>
                    p.Set(ps => ps.DeclaredValue, insureValue)
                        .Set(ps => ps.Insurance, insured)
                        .Set(ps => ps.InsuranceValue, insureValue)
                        .Set(ps => ps.InsurancePennyOne, pennyOne)))
                .Set(s => s.Insurance, insured)
                .Set(s => s.InsuranceProvider, (int)insuranceProvider)
                .Save();

            using (var scope = context.Container.BeginLifetimeScope())
            {
                testObject = scope.Resolve<IInsureShipService>();
                Assert.Equal(result, testObject.IsInsuredByInsureShip(shipment));
            }
        }

        [Theory]
        [InlineData(false, 0, InsuranceProvider.Carrier, false, false)]
        [InlineData(false, 50, InsuranceProvider.Carrier, false, false)]
        [InlineData(false, 100, InsuranceProvider.Carrier, false, false)]
        [InlineData(false, 100, InsuranceProvider.ShipWorks, false, false)]

        [InlineData(true, 0, InsuranceProvider.Carrier, false, false)]
        [InlineData(true, 50, InsuranceProvider.Carrier, false, false)]
        [InlineData(true, 100, InsuranceProvider.Carrier, false, false)]
        [InlineData(true, 100.01, InsuranceProvider.ShipWorks, false, true)]

        [InlineData(true, 0, InsuranceProvider.Carrier, true, false)]
        [InlineData(true, 50, InsuranceProvider.Carrier, true, false)]
        [InlineData(true, 100, InsuranceProvider.Carrier, true, false)]
        [InlineData(true, 100, InsuranceProvider.ShipWorks, true, false)]

        [InlineData(true, 1, InsuranceProvider.ShipWorks, true, false)]
        [InlineData(true, 50, InsuranceProvider.ShipWorks, true, false)]
        public void IsInsuredByInsureShip_ReturnsTrue_AmazonSfp_WhenSwInsuranceAndValueOver100(bool insured, decimal insureValue, InsuranceProvider insuranceProvider, bool pennyOne, bool result)
        {
            shipment = Create.Shipment(context.Order)
                .AsAmazonSFP(p =>
                    p.Set(ps => ps.DeclaredValue, insureValue)
                        .Set(ps => ps.Insurance, insured)
                        .Set(ps => ps.InsuranceValue, insureValue)
                        .Set(ps => ps.CarrierName = "FedEx")
                        )
                .Set(s => s.Insurance, insured)
                .Set(s => s.InsuranceProvider, (int)insuranceProvider)
                .Save();
            
            using (var scope = context.Container.BeginLifetimeScope())
            {
                testObject = scope.Resolve<IInsureShipService>();
                Assert.Equal(result, testObject.IsInsuredByInsureShip(shipment));
            }
        }

        [Theory]
        [InlineData(false, 0, InsuranceProvider.Carrier, false)]
        [InlineData(false, 50, InsuranceProvider.Carrier, false)]
        [InlineData(false, 100, InsuranceProvider.Carrier, false)]
        [InlineData(false, 100, InsuranceProvider.ShipWorks, false)]

        [InlineData(true, 0, InsuranceProvider.Carrier, false)]
        [InlineData(true, 50, InsuranceProvider.Carrier, false)]
        [InlineData(true, 100, InsuranceProvider.Carrier, false)]
        [InlineData(true, 100.01, InsuranceProvider.ShipWorks, true)]

        [InlineData(true, 1, InsuranceProvider.ShipWorks, true)]
        [InlineData(true, 50, InsuranceProvider.ShipWorks, true)]

        public void IsInsuredByInsureShip_ReturnsTrue_Usps_WhenSwInsuranceAndValueOver100(bool insured, decimal insureValue, InsuranceProvider insuranceProvider, bool result)
        {
            shipment = Create.Shipment(context.Order)
                .AsPostal(x => x.AsUsps(p =>
                    p.Set(ps => ps.Insurance = insured))
                    .Set(p => p.InsuranceValue, insureValue))
                .Set(s => s.Insurance, insured)
                .Set(s => s.InsuranceProvider, (int) insuranceProvider)
                .Save();

            using (var scope = context.Container.BeginLifetimeScope())
            {
                testObject = scope.Resolve<IInsureShipService>();
                Assert.Equal(result, testObject.IsInsuredByInsureShip(shipment));
            }
        }

        public void Dispose()
        {
            adapter.Dispose();
            context.Dispose();
        }
    }
}
