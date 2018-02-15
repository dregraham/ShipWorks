using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Services;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Services
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class ShippingManagerWrapperTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly DataContext context;
        private ShipmentEntity shipment;

        public ShippingManagerWrapperTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            mock = context.Mock;

            var customerLicense = mock.Create<CustomerLicense>(new TypedParameter(typeof(string), "someKey"));

            // Mock up the CustomerLicense constructor parameter Func<string, ICustomerLicense>
            var repo = mock.MockRepository.Create<Func<string, ICustomerLicense>>();
            repo.Setup(x => x(It.IsAny<string>()))
                .Returns(customerLicense);
            mock.Provide(repo.Object);

            var writer = mock.MockRepository.Create<ICustomerLicenseWriter>();
            writer.Setup(w => w.Write(It.IsAny<ICustomerLicense>())).Callback(() => { });
            mock.Provide(writer.Object);

            var licenseService = mock.MockRepository.Create<ILicenseService>();
            licenseService.Setup(ls => ls.CheckRestriction(It.IsAny<EditionFeature>(), It.IsAny<object>()))
                .Returns(EditionRestrictionLevel.None);
            mock.Provide(licenseService.Object);

            shipment = Create.Shipment(context.Order).Save();
        }

        [Fact]
        public void ChangeShipmentType_CreatesPostalAndUspsEntities_WhenChangedToUsps()
        {
            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();

            wrapper.ChangeShipmentType(ShipmentTypeCode.Usps, shipment);

            using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
            {
                IPrefetchPath2 shipmentPrefetchPath = new PrefetchPath2(EntityType.ShipmentEntity);
                shipmentPrefetchPath.Add(ShipmentEntity.PrefetchPathPostal)
                    .SubPath.Add(PostalShipmentEntity.PrefetchPathUsps);

                ShipmentEntity loadedShipment = new ShipmentEntity(shipment.ShipmentID);
                sqlAdapter.FetchEntity(loadedShipment, shipmentPrefetchPath);

                Assert.NotNull(loadedShipment.Postal);
                Assert.NotNull(loadedShipment.Postal.Usps);
            }
        }

        [Fact]
        public void ChangeShipmentType_DoesNotDeleteOtherEntity_WhenChangedToUsps()
        {
            Modify.Shipment(shipment).AsOther().Save();

            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();

            wrapper.ChangeShipmentType(ShipmentTypeCode.Usps, shipment);

            using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
            {
                IPrefetchPath2 shipmentPrefetchPath = new PrefetchPath2(EntityType.ShipmentEntity);
                shipmentPrefetchPath.Add(ShipmentEntity.PrefetchPathOther);

                ShipmentEntity loadedShipment = new ShipmentEntity(shipment.ShipmentID);
                sqlAdapter.FetchEntity(loadedShipment, shipmentPrefetchPath);

                Assert.NotNull(loadedShipment.Other);
            }
        }

        [Fact]
        public void ChangeShipmentType_AppliesDefaultProfile_WhenChangedToNewShipmentType()
        {
            Create.Profile()
                .AsPrimary()
                .AsOnTrac(o => o.Set(x => x.Reference2, "FOO"))
                .SetDefaultsOnNullableFields()
                .Save();
               
            ShippingProfileManager.CheckForChangesNeeded();

            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();

            wrapper.ChangeShipmentType(ShipmentTypeCode.OnTrac, shipment);

            using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
            {
                IPrefetchPath2 shipmentPrefetchPath = new PrefetchPath2(EntityType.ShipmentEntity);
                shipmentPrefetchPath.Add(ShipmentEntity.PrefetchPathOnTrac);

                ShipmentEntity loadedShipment = new ShipmentEntity(shipment.ShipmentID);
                sqlAdapter.FetchEntity(loadedShipment, shipmentPrefetchPath);

                Assert.Equal("FOO", loadedShipment.OnTrac.Reference2);
            }
        }

        [Fact]
        public void ChangeShipmentType_DoesNotApplyDefaultProfile_WhenChangedToExistingShipmentType()
        {
            Modify.Shipment(shipment).AsOnTrac().Set(x => x.ShipmentTypeCode, ShipmentTypeCode.Other).Save();

            OnTracProfileEntity onTracProfile = Create.Entity<OnTracProfileEntity>()
                .SetDefaultsOnNullableFields()
                .Set(x => x.Reference2, "FOO")
                .Build();

            Create.Entity<ShippingProfileEntity>()
                .SetDefaultsOnNullableFields()
                .Set(x => x.ShipmentTypePrimary, true)
                .Set(x => x.ShipmentTypeCode, ShipmentTypeCode.OnTrac)
                .Set(x => x.OnTrac, onTracProfile)
                .Save();

            ShippingProfileManager.CheckForChangesNeeded();

            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();

            wrapper.ChangeShipmentType(ShipmentTypeCode.OnTrac, shipment);

            using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
            {
                IPrefetchPath2 shipmentPrefetchPath = new PrefetchPath2(EntityType.ShipmentEntity);
                shipmentPrefetchPath.Add(ShipmentEntity.PrefetchPathOnTrac);

                ShipmentEntity loadedShipment = new ShipmentEntity(shipment.ShipmentID);
                sqlAdapter.FetchEntity(loadedShipment, shipmentPrefetchPath);

                Assert.NotEqual("FOO", loadedShipment.OnTrac.Reference2);
            }
        }

        [Fact]
        public async Task ChangeShipmentType_InsuranceIsSetCorrectly()
        {
            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();
            bool originalInsuredValue = true;

            IEnumerable<ShipmentTypeCode> shipmentTypeCodes = EnumHelper.GetEnumList<ShipmentTypeCode>()
                .Where(x => x.Value != ShipmentTypeCode.None) 
                .Select(s => s.Value);

            // Assert if new shipment types were added to make sure they get updated for insurance changing on shipment type change.
            Assert.Equal(15, shipmentTypeCodes.Count());

            foreach (ShipmentTypeCode startShipmentTypeCode in shipmentTypeCodes)
            {
                foreach (ShipmentTypeCode changeToShipmentTypeCode in shipmentTypeCodes)
                {
                    if (startShipmentTypeCode == changeToShipmentTypeCode ||
                        (IsUps(startShipmentTypeCode) && IsUps(changeToShipmentTypeCode)) ||
                        (IsEndicia(startShipmentTypeCode) && IsEndicia(changeToShipmentTypeCode)) ||
                        (IsUsps(startShipmentTypeCode) && IsUsps(changeToShipmentTypeCode)))
                    {
                        continue;
                    }

                    shipment = CreateShipment(startShipmentTypeCode, originalInsuredValue);

                    var shipmentAdapter = wrapper.ChangeShipmentType(changeToShipmentTypeCode, shipment);

                    using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
                    {
                        await sqlAdapter.SaveEntityAsync(shipmentAdapter.Shipment).ConfigureAwait(false);

                        QueryFactory factory = new QueryFactory();
                        EntityQuery<ShipmentEntity> query = factory.Shipment
                            .Where(ShipmentFields.ShipmentID == shipmentAdapter.Shipment.ShipmentID);
                        query = FullShipmentPrefetchPath(query);

                        IEntityCollection2 shipments = await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
                        var loadedShipment = shipments[0] as ShipmentEntity;

                        Assert.Equal(changeToShipmentTypeCode, loadedShipment.ShipmentTypeCode);

                        bool newInsuranceValue = GetInsuranceValue(loadedShipment);
                        Assert.NotEqual(originalInsuredValue, newInsuranceValue);
                    }
                }
            }
        }

        private bool IsEndicia(ShipmentTypeCode shipmentTypeCode)
        {
            return shipmentTypeCode == ShipmentTypeCode.Express1Endicia || shipmentTypeCode == ShipmentTypeCode.Endicia;
        }

        private bool IsUsps(ShipmentTypeCode shipmentTypeCode)
        {
            return shipmentTypeCode == ShipmentTypeCode.Express1Usps || shipmentTypeCode == ShipmentTypeCode.Usps;
        }

        private bool IsUps(ShipmentTypeCode shipmentTypeCode)
        {
            return shipmentTypeCode == ShipmentTypeCode.UpsWorldShip || shipmentTypeCode == ShipmentTypeCode.UpsOnLineTools;
        }

        private bool GetInsuranceValue(ShipmentEntity shipment)
        {
            switch (shipment.ShipmentTypeCode)
            {
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    return shipment.Ups.Packages.Any(p => p.Insurance);

                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.Express1Endicia:
                    return shipment.Postal.Endicia.Insurance;

                case ShipmentTypeCode.PostalWebTools:
                    return shipment.Postal.Insurance;
                    
                case ShipmentTypeCode.FedEx:
                    return shipment.FedEx.Packages.Any(p => p.Insurance);

                case ShipmentTypeCode.OnTrac:
                    return shipment.OnTrac.Insurance;

                case ShipmentTypeCode.iParcel:
                    return shipment.IParcel.Packages.Any(p => p.Insurance);

                case ShipmentTypeCode.Other:
                    return shipment.Other.Insurance;

                case ShipmentTypeCode.BestRate:
                    return shipment.BestRate.Insurance;

                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Express1Usps:
                    return shipment.Postal.Usps.Insurance;

                case ShipmentTypeCode.Amazon:
                    return shipment.Amazon.Insurance;

                case ShipmentTypeCode.DhlExpress:
                    return shipment.DhlExpress.Packages.Any(p => p.Insurance);

                case ShipmentTypeCode.Asendia:
                    return shipment.Asendia.Insurance;

                case ShipmentTypeCode.None:
                    return false;
                default:
                    return false;
            }
        }

        private ShipmentEntity CreateShipment(ShipmentTypeCode shipmentTypeCode, bool insured)
        {
            switch (shipmentTypeCode)
            {
                case ShipmentTypeCode.UpsOnLineTools:
                    return Create.Shipment(context.Order).AsUps(f => f.WithPackage(p => p.Set(pkg => pkg.Insurance, insured)))
                        .Set(s => s.Insurance = true).Save();
                case ShipmentTypeCode.UpsWorldShip:
                    return Create.Shipment(context.Order).AsUpsWorldShip(f => f.WithPackage(p => p.Set(pkg => pkg.Insurance, insured)))
                        .Set(s => s.Insurance = true).Save();
                case ShipmentTypeCode.Endicia:
                    return Create.Shipment(context.Order).AsPostal(p => p.AsEndicia(e => e.Set(es => es.Insurance, insured)))
                        .Set(s => s.Insurance = true).Save();
                case ShipmentTypeCode.PostalWebTools:
                    return Create.Shipment(context.Order).AsPostal(p => p.Set(es => es.Insurance, insured))
                        .Set(s => s.Insurance = true).Save();
                case ShipmentTypeCode.Express1Endicia:
                    return Create.Shipment(context.Order).AsPostal(p => p.AsExpress1Endicia(e => e.Set(es => es.Insurance, insured)))
                        .Set(s => s.Insurance = true).Save();
                case ShipmentTypeCode.Express1Usps:
                    return Create.Shipment(context.Order).AsPostal(p => p.AsExpress1Usps(e => e.Set(es => es.Insurance, insured)))
                        .Set(s => s.Insurance = true).Save();
                case ShipmentTypeCode.FedEx:
                    return Create.Shipment(context.Order).AsFedEx(f => f.WithPackage(p => p.Set(pkg => pkg.Insurance, insured)))
                        .Set(s => s.Insurance = true).Save();
                case ShipmentTypeCode.OnTrac:
                    return Create.Shipment(context.Order).AsOnTrac(f => f.Set(pkg => pkg.Insurance, insured))
                        .Set(s => s.Insurance = true).Save();
                case ShipmentTypeCode.iParcel:
                    return Create.Shipment(context.Order).AsIParcel(f => f.WithPackage(p => p.Set(pkg => pkg.Insurance, insured)))
                        .Set(s => s.Insurance = true).Save();
                case ShipmentTypeCode.Other:
                    return Create.Shipment(context.Order).AsOther(f => f.Set(pkg => pkg.Insurance, insured))
                        .Set(s => s.Insurance = true).Save();
                case ShipmentTypeCode.BestRate:
                    return Create.Shipment(context.Order).AsBestRate(f => f.Set(pkg => pkg.Insurance, insured))
                        .Set(s => s.Insurance = true).Save();
                case ShipmentTypeCode.Usps:
                    return Create.Shipment(context.Order).AsPostal(p => p.AsUsps(e => e.Set(es => es.Insurance, insured)))
                        .Set(s => s.Insurance = true).Save();
                case ShipmentTypeCode.Amazon:
                    return Create.Shipment(context.Order).AsAmazon(f => f.Set(pkg => pkg.Insurance, insured))
                        .Set(s => s.Insurance = true).Save();

                case ShipmentTypeCode.DhlExpress:
                    return Create.Shipment(context.Order).AsDhlExpress(f => f.WithPackage(p => p.Set(pkg => pkg.Insurance, insured)))
                        .Set(s => s.Insurance = true).Save();

                case ShipmentTypeCode.Asendia:
                    return Create.Shipment(context.Order).AsAsendia(p => p.Set(es => es.Insurance, insured))
                        .Set(s => s.Insurance = true).Save();

                default:
                    return null;
            }
        }

        /// <summary>
        /// Create the pre-fetch path used to load a shipment
        /// </summary>
        private static EntityQuery<ShipmentEntity> FullShipmentPrefetchPath(EntityQuery<ShipmentEntity> query)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                return Enum.GetValues(typeof(ShipmentTypeCode))
                    .OfType<ShipmentTypeCode>()
                    .Where(x => lifetimeScope.IsRegisteredWithKey<IShipmentTypePrefetchProvider>(x))
                    .Select(x => lifetimeScope.ResolveKeyed<IShipmentTypePrefetchProvider>(x))
                    .Aggregate(ShipmentTypePrefetchPath.Empty, (path, x) => path.With(x))
                    .ApplyTo(query)
                    .WithPath(ShipmentEntity.PrefetchPathCustomsItems);
            }
        }

        public void Dispose()
        {
            mock.Dispose();
            context.Dispose();
        }
    }
}
