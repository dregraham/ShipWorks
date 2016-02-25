using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.FeatureRestrictions
{
    public class FeatureRestrictionTest
    {
        [Fact]
        public void Handle_ReturnsFalse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<FeatureRestriction>();
                var owner = mock.Mock<IWin32Window>();

                Assert.False(testObject.Handle(owner.Object, It.IsAny<ILicenseCapabilities>(), null));
            }
        }
    }
}
