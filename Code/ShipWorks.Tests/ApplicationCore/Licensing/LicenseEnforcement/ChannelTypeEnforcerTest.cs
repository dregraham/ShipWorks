using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using System.Collections.Generic;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.LicenseEnforcement
{
    public class ChannelTypeEnforcerTest
    {

        // We can't use autofac to create an abstract class. Online suggestions suggest creating a test class
        // since the inheriting classes are so light as is, I decided to use the GenericFileEnforcer for testing

        [Fact]
        public void Enforce_ReturnCompliant_WhenStoreTypeExistsAndIsAllowed()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IStoreManager>()
                    .Setup(s => s.GetAllStores())
                    .Returns(new List<StoreEntity>() {new StoreEntity() {TypeCode = (int) StoreTypeCode.GenericFile}});

                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.Setup(l => l.IsChannelAllowed(StoreTypeCode.GenericFile))
                    .Returns(true);

                var testObject = mock.Create<GenericFileEnforcer>();

                var enumResult = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.NotSpecified);

                Assert.Equal(ComplianceLevel.Compliant, enumResult.Value);
            }
        }

        [Fact]
        public void Enforce_ReturnNotCompliant_WhenStoreTypeExistsAndIsNotAllowed()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IStoreManager>()
                    .Setup(s => s.GetAllStores())
                    .Returns(new List<StoreEntity>() { new StoreEntity() { TypeCode = (int)StoreTypeCode.GenericFile } });

                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.Setup(l => l.IsChannelAllowed(StoreTypeCode.GenericFile))
                    .Returns(false);

                var testObject = mock.Create<GenericFileEnforcer>();

                var enumResult = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.NotSpecified);

                Assert.Equal(ComplianceLevel.NotCompliant, enumResult.Value);
            }
        }

        [Fact]
        public void Enforce_ReturnCompliant_WhenStoreDoesNotExistsAndItIsNotAllowed()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IStoreManager>()
                    .Setup(s => s.GetAllStores())
                    .Returns(new List<StoreEntity>() { new StoreEntity() { TypeCode = (int)StoreTypeCode.Magento } });

                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.Setup(l => l.IsChannelAllowed(StoreTypeCode.GenericFile))
                    .Returns(false);

                var testObject = mock.Create<GenericFileEnforcer>();

                var enumResult = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.NotSpecified);

                Assert.Equal(ComplianceLevel.Compliant, enumResult.Value);
            }
        }


        [Fact]
        public void Enforce_ReturnStoreInErrorMessage_WhenStoreExistsAndItIsNotAllowed()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IStoreManager>()
                    .Setup(s => s.GetAllStores())
                    .Returns(new List<StoreEntity> { new StoreEntity { TypeCode = (int)StoreTypeCode.GenericFile } });

                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.Setup(l => l.IsChannelAllowed(StoreTypeCode.GenericFile))
                    .Returns(false);

                var testObject = mock.Create<GenericFileEnforcer>();

                var enumResult = testObject.Enforce(licenseCapabilities.Object, EnforcementContext.NotSpecified);

                Assert.Contains(EnumHelper.GetDescription(StoreTypeCode.GenericFile), enumResult.Message);
            }
        }
    }
}
