using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions;
using ShipWorks.Editions;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.FeatureRestrictions
{
    public class FeatureRestrictionTest
    {
        [Fact]
        public void Handle_ReturnsFalse_WhenEditionRestrictionLevelIsHidden()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IWin32Window> owner = mock.Mock<IWin32Window>();
                TestHiddenRestriction testObject = mock.Create<TestHiddenRestriction>();

                Assert.False(testObject.Handle(owner.Object, It.IsAny<ILicenseCapabilities>(), null));
            }
        }

        [Fact]
        public void Handle_ReturnsFalse_WhenEditionRestrictionLevelIsForbidden()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IWin32Window> owner = mock.Mock<IWin32Window>();
                TestForbiddenRestriction testObject = mock.Create<TestForbiddenRestriction>();

                Assert.False(testObject.Handle(owner.Object, It.IsAny<ILicenseCapabilities>(), null));
            }
        }

        [Fact]
        public void Handle_ReturnsTrue_WhenEditionRestrictionLevelNone()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IWin32Window> owner = mock.Mock<IWin32Window>();
                TestNoneRestriction testObject = mock.Create<TestNoneRestriction>();

                Assert.True(testObject.Handle(owner.Object, It.IsAny<ILicenseCapabilities>(), null));
            }
        }

        [Fact]
        public void Handle_ShowsError_WhenEditionFeatureIsShipmentTypeAndRestrictionLevelIsForbidden()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IMessageHelper> messageHelper = mock.Mock<IMessageHelper>();
                Mock<IWin32Window> owner = mock.Mock<IWin32Window>();
                TestForbiddenShipmentTypeRestriction testObject = mock.Create<TestForbiddenShipmentTypeRestriction>();

                testObject.Handle(owner.Object, It.IsAny<ILicenseCapabilities>(), null);

                messageHelper.Verify(m => m.ShowError(owner.Object, "You must contact Interapptive to use additional shipping carriers."), Times.Once);
            }
        }
    }

    /// <summary>
    /// Empty class to be able to test the abstract FeatureRestriction class
    /// </summary>
    public class TestForbiddenShipmentTypeRestriction : FeatureRestriction
    {
        public TestForbiddenShipmentTypeRestriction(IMessageHelper messageHelper) : base(messageHelper)
        {
        }
        public override EditionFeature EditionFeature => EditionFeature.ShipmentType;
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return EditionRestrictionLevel.Forbidden;
        }
    }

    /// <summary>
    /// Empty class to be able to test the abstract FeatureRestriction class
    /// </summary>
    public class TestHiddenRestriction : FeatureRestriction
    {
        public TestHiddenRestriction(IMessageHelper messageHelper) : base(messageHelper)
        {
        }
        public override EditionFeature EditionFeature => EditionFeature.None;
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return EditionRestrictionLevel.Hidden;
        }
    }

    /// <summary>
    /// Empty class to be able to test the abstract FeatureRestriction class
    /// </summary>
    public class TestForbiddenRestriction : FeatureRestriction
    {
        public TestForbiddenRestriction(IMessageHelper messageHelper) : base(messageHelper)
        {
        }
        public override EditionFeature EditionFeature => EditionFeature.None;
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return EditionRestrictionLevel.Forbidden;
        }
    }

    /// <summary>
    /// Empty class to be able to test the abstract FeatureRestriction class
    /// </summary>
    public class TestNoneRestriction : FeatureRestriction
    {
        public TestNoneRestriction(IMessageHelper messageHelper) : base(messageHelper)
        {
        }
        public override EditionFeature EditionFeature => EditionFeature.None;
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return EditionRestrictionLevel.None;
        }
    }

}
