using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autofac;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.Editions;
using ShipWorks.Stores;
using ShipWorks.UI.Controls.ChannelLimit;
using Xunit;

namespace ShipWorks.UI.Tests.Controls.ChannelLimit
{
    public class ChannelLimitDlgFactoryTest
    {
        [Fact]
        public void GetChannelLimitDlg_GetsLicenseFromLicenseService()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
                Mock<ICustomerLicense> customerlicense = mock.Mock<ICustomerLicense>();
                Mock<IChannelLimitDlg> control = mock.Mock<IChannelLimitDlg>();
                Mock<IChannelLimitBehavior> behavior = mock.Mock<IChannelLimitBehavior>();
                Mock<IWin32Window> owner = mock.Mock<IWin32Window>();

                licenseService.Setup(l => l.GetLicenses())
                    .Returns(new List<ILicense> {customerlicense.Object});

                Mock<Func<IWin32Window, IChannelLimitDlg>> dlgFactoryRepo = mock.MockRepository.Create<Func<IWin32Window, IChannelLimitDlg>>();
                dlgFactoryRepo.Setup(x => x(It.IsAny<IWin32Window>()))
                    .Returns(control.Object);
                mock.Provide(dlgFactoryRepo.Object);

                Mock<IIndex<EditionFeature, IChannelLimitBehavior>> channelLimitBehaviorRepo = mock.MockRepository.Create<IIndex<EditionFeature, IChannelLimitBehavior>>();
                channelLimitBehaviorRepo.Setup(x => x[It.IsAny<EditionFeature>()])
                    .Returns(behavior.Object);
                mock.Provide(channelLimitBehaviorRepo.Object);

                ChannelLimitDlgFactory testObject = mock.Create<ChannelLimitDlgFactory>();

                testObject.GetChannelLimitDlg(owner.Object, EditionFeature.ActionLimit, It.IsAny<EnforcementContext>());

                licenseService.Verify(l => l.GetLicenses(), Times.Once);
            }
        }

        [Fact]
        public void GetChannelLimitDlg_ThrowsInvalidCastException_WhenLicenseServiceReturnsStoreLicense()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
                Mock<ILicense> storeLicense = mock.Mock<ILicense>();
                Mock<IChannelLimitDlg> control = mock.Mock<IChannelLimitDlg>();
                Mock<IChannelLimitBehavior> behavior = mock.Mock<IChannelLimitBehavior>();
                Mock<IWin32Window> owner = mock.Mock<IWin32Window>();

                licenseService.Setup(l => l.GetLicenses())
                    .Returns(new List<ILicense> { storeLicense.Object });

                Mock<Func<IWin32Window, IChannelLimitDlg>> dlgFactoryRepo = mock.MockRepository.Create<Func<IWin32Window, IChannelLimitDlg>>();
                dlgFactoryRepo.Setup(x => x(It.IsAny<IWin32Window>()))
                    .Returns(control.Object);
                mock.Provide(dlgFactoryRepo.Object);

                Mock<IIndex<EditionFeature, IChannelLimitBehavior>> channelLimitBehaviorRepo = mock.MockRepository.Create<IIndex<EditionFeature, IChannelLimitBehavior>>();
                channelLimitBehaviorRepo.Setup(x => x[It.IsAny<EditionFeature>()])
                    .Returns(behavior.Object);
                mock.Provide(channelLimitBehaviorRepo.Object);

                ChannelLimitDlgFactory testObject = mock.Create<ChannelLimitDlgFactory>();

                InvalidCastException ex =
                    Assert.Throws<InvalidCastException>(
                        () => testObject.GetChannelLimitDlg(owner.Object, EditionFeature.ActionLimit, It.IsAny<EnforcementContext>()));
                Assert.Equal("Expected a ICustomerLicense from the LicenseService", ex.Message);
            }
        }
        
        [Fact]
        public void GetChannelLimitDlg_CallsViewModelLoadWithCustomerLicense()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IChannelLimitViewModel> viewModel = mock.Mock<IChannelLimitViewModel>();
                Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
                Mock<ICustomerLicense> customerlicense = mock.Mock<ICustomerLicense>();
                Mock<IChannelLimitDlg> control = mock.Mock<IChannelLimitDlg>();
                Mock<IChannelLimitBehavior> behavior = mock.Mock<IChannelLimitBehavior>();
                Mock<IWin32Window> owner = mock.Mock<IWin32Window>();

                licenseService.Setup(l => l.GetLicenses())
                    .Returns(new List<ILicense> { customerlicense.Object });

                Mock<Func<IWin32Window, IChannelLimitDlg>> dlgFactoryRepo = mock.MockRepository.Create<Func<IWin32Window, IChannelLimitDlg>>();
                dlgFactoryRepo.Setup(x => x(It.IsAny<IWin32Window>()))
                    .Returns(control.Object);
                mock.Provide(dlgFactoryRepo.Object);

                Mock<IIndex<EditionFeature, IChannelLimitBehavior>> channelLimitBehaviorRepo = mock.MockRepository.Create<IIndex<EditionFeature, IChannelLimitBehavior>>();
                channelLimitBehaviorRepo.Setup(x => x[It.IsAny<EditionFeature>()])
                    .Returns(behavior.Object);
                mock.Provide(channelLimitBehaviorRepo.Object);

                ChannelLimitDlgFactory testObject = mock.Create<ChannelLimitDlgFactory>();

                testObject.GetChannelLimitDlg(owner.Object, EditionFeature.ActionLimit, It.IsAny<EnforcementContext>());

                viewModel.Verify(v => v.Load(customerlicense.Object, It.IsAny<IChannelLimitBehavior>()));
            }
        }

        [Fact]
        public void GetChannelLimitDlg_CallsViewModelLoadWithBehavior()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IChannelLimitViewModel> viewModel = mock.Mock<IChannelLimitViewModel>();
                Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
                Mock<ICustomerLicense> customerlicense = mock.Mock<ICustomerLicense>();
                Mock<IChannelLimitDlg> control = mock.Mock<IChannelLimitDlg>();
                Mock<IChannelLimitBehavior> behavior = mock.Mock<IChannelLimitBehavior>();
                Mock<IWin32Window> owner = mock.Mock<IWin32Window>();

                licenseService.Setup(l => l.GetLicenses())
                    .Returns(new List<ILicense> { customerlicense.Object });

                Mock<Func<IWin32Window, IChannelLimitDlg>> dlgFactoryRepo = mock.MockRepository.Create<Func<IWin32Window, IChannelLimitDlg>>();
                dlgFactoryRepo.Setup(x => x(It.IsAny<IWin32Window>()))
                    .Returns(control.Object);
                mock.Provide(dlgFactoryRepo.Object);

                Mock<IIndex<EditionFeature, IChannelLimitBehavior>> channelLimitBehaviorRepo = mock.MockRepository.Create<IIndex<EditionFeature, IChannelLimitBehavior>>();
                channelLimitBehaviorRepo.Setup(x => x[It.IsAny<EditionFeature>()])
                    .Returns(behavior.Object);
                mock.Provide(channelLimitBehaviorRepo.Object);

                ChannelLimitDlgFactory testObject = mock.Create<ChannelLimitDlgFactory>();

                testObject.GetChannelLimitDlg(owner.Object, EditionFeature.ActionLimit, It.IsAny<EnforcementContext>());

                viewModel.Verify(v => v.Load(It.IsAny<ICustomerLicense>(), behavior.Object));
            }
        }

        [Fact]
        public void GetChannelLimitDlg_GetsDlgFromDlgFactoryUsingOwner()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
                Mock<ICustomerLicense> customerlicense = mock.Mock<ICustomerLicense>();
                Mock<IChannelLimitDlg> control = mock.Mock<IChannelLimitDlg>();
                Mock<IChannelLimitBehavior> behavior = mock.Mock<IChannelLimitBehavior>();
                Mock<IWin32Window> owner = mock.Mock<IWin32Window>();

                licenseService.Setup(l => l.GetLicenses())
                    .Returns(new List<ILicense> { customerlicense.Object });

                Mock<Func<IWin32Window, IChannelLimitDlg>> dlgFactoryRepo = mock.MockRepository.Create<Func<IWin32Window, IChannelLimitDlg>>();
                dlgFactoryRepo.Setup(x => x(It.IsAny<IWin32Window>()))
                    .Returns(control.Object);
                mock.Provide(dlgFactoryRepo.Object);

                Mock<IIndex<EditionFeature, IChannelLimitBehavior>> channelLimitBehaviorRepo = mock.MockRepository.Create<IIndex<EditionFeature, IChannelLimitBehavior>>();
                channelLimitBehaviorRepo.Setup(x => x[It.IsAny<EditionFeature>()])
                    .Returns(behavior.Object);
                mock.Provide(channelLimitBehaviorRepo.Object);

                ChannelLimitDlgFactory testObject = mock.Create<ChannelLimitDlgFactory>();

                testObject.GetChannelLimitDlg(owner.Object, EditionFeature.ActionLimit, It.IsAny<EnforcementContext>());

                dlgFactoryRepo.Verify(f => f(owner.Object));
            }
        }

        [Fact]
        public void GetChannelLimitDlg_SetsDataContextOnDialog()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IChannelLimitViewModel> viewModel = mock.Mock<IChannelLimitViewModel>();
                Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
                Mock<ICustomerLicense> customerlicense = mock.Mock<ICustomerLicense>();
                Mock<IChannelLimitDlg> dialog = mock.Mock<IChannelLimitDlg>();
                Mock<IChannelLimitBehavior> behavior = mock.Mock<IChannelLimitBehavior>();
                Mock<IWin32Window> owner = mock.Mock<IWin32Window>();

                licenseService.Setup(l => l.GetLicenses())
                    .Returns(new List<ILicense> { customerlicense.Object });

                Mock<Func<IWin32Window, IChannelLimitDlg>> dlgFactoryRepo = mock.MockRepository.Create<Func<IWin32Window, IChannelLimitDlg>>();
                dlgFactoryRepo.Setup(x => x(It.IsAny<IWin32Window>()))
                    .Returns(dialog.Object);
                mock.Provide(dlgFactoryRepo.Object);

                Mock<IIndex<EditionFeature, IChannelLimitBehavior>> channelLimitBehaviorRepo = mock.MockRepository.Create<IIndex<EditionFeature, IChannelLimitBehavior>>();
                channelLimitBehaviorRepo.Setup(x => x[It.IsAny<EditionFeature>()])
                    .Returns(behavior.Object);
                mock.Provide(channelLimitBehaviorRepo.Object);

                ChannelLimitDlgFactory testObject = mock.Create<ChannelLimitDlgFactory>();

                testObject.GetChannelLimitDlg(owner.Object, EditionFeature.ActionLimit, It.IsAny<EnforcementContext>());

                dialog.VerifySet(d => d.DataContext = viewModel.Object);
            }
        }
    }
}