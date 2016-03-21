using System;
using Autofac.Extras.Moq;
using Divelements.SandRibbon;
using ShipWorks.Shipping.UI.ShippingRibbon;
using ShipWorks.Tests.Shared;
using TD.SandDock;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingRibbon
{
    public class ShippingRibbonRegistrationTest : IDisposable
    {
        readonly AutoMock mock;

        public ShippingRibbonRegistrationTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Register_DelegatesToShippingRibbonService()
        {
            var testObject = mock.Create<ShippingRibbonRegistration>();

            testObject.Register(mock.Create<SandDockManager>(), mock.Create<Ribbon>());

            mock.Mock<IShippingRibbonService>().Verify(x => x.Register(testObject));
        }

        [Fact]
        public void Register_BuildsRibbon()
        {
            var ribbon = mock.Create<Ribbon>();
            var testObject = mock.Create<ShippingRibbonRegistration>();

            testObject.Register(mock.Create<SandDockManager>(), ribbon);

            Assert.Equal(1, ribbon.Controls.Count);
            var tab = ribbon.Controls[0] as RibbonTab;

            Assert.Equal(3, tab.Chunks.Count);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
