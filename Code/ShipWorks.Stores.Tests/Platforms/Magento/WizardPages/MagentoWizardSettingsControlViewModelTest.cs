using System;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.Stores.Platforms.Magento.Enums;
using ShipWorks.Stores.UI.Platforms.Magento.WizardPages;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Magento.WizardPages
{
    public class MagentoWizardSettingsControlViewModelTest : IDisposable
    {
        readonly AutoMock mock;
        readonly MagentoStoreEntity store;
        readonly MagentoStoreSetupControlViewModel testObject;
        readonly GenericResult<Uri> successResult;
        readonly Mock<IIndex<MagentoVersion, IMagentoProbe>> probeIIndex;

        public MagentoWizardSettingsControlViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            store = new MagentoStoreEntity();
            successResult = GenericResult.FromSuccess<Uri>(new Uri("http://www.shipworks.com"));

            probeIIndex = mock.MockRepository.Create<IIndex<MagentoVersion, IMagentoProbe>>();
            mock.Provide(probeIIndex.Object);

            mock.Mock<IStoreTypeManager>()
                .Setup(s => s.GetType(It.IsAny<StoreEntity>()))
                .Returns(new MockMagentoStoreType());

            testObject = mock.Create<MagentoStoreSetupControlViewModel>();
            testObject.Username = "testUsername";
            testObject.Password = "testPassword".ToSecureString();
            testObject.StoreUrl = "http://www.shipworks.com";
        }

        private void SetupProbeResult(MagentoVersion magentoVersion, bool success, string url = "http://www.shipworks.com/")
        {
            GenericResult<Uri> result = success ? GenericResult.FromSuccess<Uri>(new Uri(url)) : GenericResult.FromError<Uri>("error");

            var myProbe = mock.MockRepository.Create<IMagentoProbe>();
            myProbe.Setup(p => p.FindCompatibleUrl(store)).Returns(result);

            probeIIndex.Setup(i => i[magentoVersion]).Returns(myProbe.Object);
        }

        [Fact]
        public void Save_ReturnsUnsuccessful_WhenUrlNotWellFormed()
        {
            testObject.StoreUrl = "blah";
            GenericResult<MagentoStoreEntity> saveResult = testObject.Save(store);

            Assert.False(saveResult.Success);
            Assert.Contains(MagentoStoreSetupControlViewModel.UrlNotInValidFormat, saveResult.Message);
        }

        [Fact]
        public void Save_UsesBothMagentoOneProbes_WhenSetToMagentoOne_AndMagentoConnectFails()
        {
            testObject.StoreUrl = "http://www.shipworks.com/";
            SetupProbeResult(MagentoVersion.MagentoConnect, false);
            SetupProbeResult(MagentoVersion.PhpFile, true);

            testObject.Save(store);

            probeIIndex.Verify(x => x[MagentoVersion.MagentoConnect], Times.Once);
            probeIIndex.Verify(x => x[MagentoVersion.PhpFile], Times.Once);
        }

        [Fact]
        public void Save_SavesUrlFromPhpFile_WhenSetToMagentoOne_AndMagentoConnectFails()
        {
            string phpUrl = "http://www.shipworks.com/shipworks.php";

            testObject.StoreUrl = "http://www.shipworks.com/";
            SetupProbeResult(MagentoVersion.MagentoConnect, false);
            SetupProbeResult(MagentoVersion.PhpFile, true, phpUrl);

            testObject.Save(store);

            Assert.Equal(phpUrl, store.ModuleUrl);
        }

        [Fact]
        public void Save_SavesUrlFromConnectProbe_WhenSetToMagentoOne_AndMagentoConnectSucceeds()
        {
            string expectedUrl = "http://www.shipworks.com/shipworks.php";

            testObject.StoreUrl = "http://www.shipworks.com/";
            SetupProbeResult(MagentoVersion.MagentoConnect, true, expectedUrl);

            testObject.Save(store);

            Assert.Equal(expectedUrl, store.ModuleUrl);
        }

        [Fact]
        public void Save_UsesBothMagentoTwoProbes_WhenSetToMagentoTwo_AndMagentoTwoFails()
        {
            testObject.StoreUrl = "http://www.shipworks.com/";
            testObject.IsMagento1 = false;

            SetupProbeResult(MagentoVersion.MagentoTwo, false);
            SetupProbeResult(MagentoVersion.MagentoTwoREST, true);

            testObject.Save(store);

            probeIIndex.Verify(x => x[MagentoVersion.MagentoTwo], Times.Once);
            probeIIndex.Verify(x => x[MagentoVersion.MagentoTwoREST], Times.Once);

            probeIIndex.Verify(x => x[MagentoVersion.MagentoConnect], Times.Never);
            probeIIndex.Verify(x => x[MagentoVersion.PhpFile], Times.Never);
        }

        [Fact]
        public void Save_SavesUrlFromMagentoTwoRest_WhenSetToMagentoTwo_AndMagentoTwoFails()
        {
            string expectedUrl = "http://www.shipworks.com/shipworks.php";
            testObject.IsMagento1 = false;

            testObject.StoreUrl = "http://www.shipworks.com/";
            SetupProbeResult(MagentoVersion.MagentoTwo, false);
            SetupProbeResult(MagentoVersion.MagentoTwoREST, true, expectedUrl);

            testObject.Save(store);

            Assert.Equal(expectedUrl, store.ModuleUrl);
        }

        [Fact]
        public void Save_SavesUrlFromMagentoTwo_WhenSetToMagentoTwo_AndMagentoTwoProbeSucceeds()
        {
            string expectedUrl = "http://www.shipworks.com/shipworks.php";
            testObject.IsMagento1 = false;

            testObject.StoreUrl = "http://www.shipworks.com/";
            SetupProbeResult(MagentoVersion.MagentoTwo, true, expectedUrl);

            testObject.Save(store);

            Assert.Equal(expectedUrl, store.ModuleUrl);
        }

        public void Dispose()
        {
            mock.Dispose();
        }

        public class MockMagentoStoreType : StoreType, IGenericModuleStoreType
        {
            public override StoreTypeCode TypeCode
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            protected override string InternalLicenseIdentifier
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
            {
                throw new NotImplementedException();
            }

            public override StoreEntity CreateStoreInstance()
            {
                throw new NotImplementedException();
            }

            public void InitializeFromOnlineModule()
            {
                // Do Nothing
            }
        }
    }
}
