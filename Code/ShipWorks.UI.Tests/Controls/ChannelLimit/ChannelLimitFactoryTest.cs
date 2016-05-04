using System;
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
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace ShipWorks.UI.Tests.Controls.ChannelLimit
{
    public class ChannelLimitFactoryTest
    {
        [Fact]
        public void CreateControl_SetsViewModelChannelToAdd()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IChannelLimitViewModel> viewModel = mock.Mock<IChannelLimitViewModel>();
                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                Mock<IChannelLimitControl> control = mock.Mock<IChannelLimitControl>();
                Mock<IChannelLimitBehavior> behavior = mock.Mock<IChannelLimitBehavior>();
                Mock<IWin32Window> owner = mock.Mock<IWin32Window>();

                Mock<Func<IChannelLimitControl>> channelLimitCtrlRepo = mock.MockRepository.Create<Func<IChannelLimitControl>>();
                channelLimitCtrlRepo.Setup(x => x())
                    .Returns(control.Object);
                mock.Provide(channelLimitCtrlRepo.Object);

                Mock<IIndex<EditionFeature, IChannelLimitBehavior>> channelLimitBehaviorRepo = mock.MockRepository.Create<IIndex<EditionFeature, IChannelLimitBehavior>>();
                channelLimitBehaviorRepo.Setup(x => x[It.IsAny<EditionFeature>()])
                    .Returns(behavior.Object);
                mock.Provide(channelLimitBehaviorRepo.Object);

                ChannelLimitFactory testObject = mock.Create<ChannelLimitFactory>();

                testObject.CreateControl(license.Object, StoreTypeCode.Amazon, EditionFeature.EndiciaAccountLimit, owner.Object);

                viewModel.VerifySet(m => m.ChannelToAdd = StoreTypeCode.Amazon);
            }
        }

        [Fact]
        public void CreateControl_SetsViewModelEnforcementContext()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IChannelLimitViewModel> viewModel = mock.Mock<IChannelLimitViewModel>();
                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                Mock<IChannelLimitControl> control = mock.Mock<IChannelLimitControl>();
                Mock<IChannelLimitBehavior> behavior = mock.Mock<IChannelLimitBehavior>();
                Mock<IWin32Window> owner = mock.Mock<IWin32Window>();

                Mock<Func<IChannelLimitControl>> channelLimitCtrlRepo = mock.MockRepository.Create<Func<IChannelLimitControl>>();
                channelLimitCtrlRepo.Setup(x => x())
                    .Returns(control.Object);
                mock.Provide(channelLimitCtrlRepo.Object);

                Mock<IIndex<EditionFeature, IChannelLimitBehavior>> channelLimitBehaviorRepo = mock.MockRepository.Create<IIndex<EditionFeature, IChannelLimitBehavior>>();
                channelLimitBehaviorRepo.Setup(x => x[It.IsAny<EditionFeature>()])
                    .Returns(behavior.Object);
                mock.Provide(channelLimitBehaviorRepo.Object);

                ChannelLimitFactory testObject = mock.Create<ChannelLimitFactory>();

                testObject.CreateControl(license.Object, StoreTypeCode.Amazon, EditionFeature.EndiciaAccountLimit, owner.Object);

                viewModel.VerifySet(m => m.EnforcementContext = EnforcementContext.ExceedingChannelLimit);
            }
        }

        [Fact]
        public void CreateControl_GetsLimitControlFromChannelLimitControlFactory()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IChannelLimitViewModel>();
                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                Mock<IChannelLimitControl> control = mock.Mock<IChannelLimitControl>();
                Mock<IChannelLimitBehavior> behavior = mock.Mock<IChannelLimitBehavior>();
                Mock<IWin32Window> owner = mock.Mock<IWin32Window>();

                Mock<Func<IChannelLimitControl>> channelLimitCtrlRepo = mock.MockRepository.Create<Func<IChannelLimitControl>>();
                channelLimitCtrlRepo.Setup(x => x())
                    .Returns(control.Object);
                mock.Provide(channelLimitCtrlRepo.Object);

                Mock<IIndex<EditionFeature, IChannelLimitBehavior>> channelLimitBehaviorRepo = mock.MockRepository.Create<IIndex<EditionFeature, IChannelLimitBehavior>>();
                channelLimitBehaviorRepo.Setup(x => x[It.IsAny<EditionFeature>()])
                    .Returns(behavior.Object);
                mock.Provide(channelLimitBehaviorRepo.Object);

                ChannelLimitFactory testObject = mock.Create<ChannelLimitFactory>();

                testObject.CreateControl(license.Object, StoreTypeCode.Amazon, EditionFeature.EndiciaAccountLimit, owner.Object);

                channelLimitCtrlRepo.Verify(f => f(), Times.Once);
            }
        }

        [Fact]
        public void CreateControl_SetsDataContextOnChannelLimitControl()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IChannelLimitViewModel> viewModel = mock.Mock<IChannelLimitViewModel>();
                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                Mock<IChannelLimitControl> control = mock.Mock<IChannelLimitControl>();
                Mock<IChannelLimitBehavior> behavior = mock.Mock<IChannelLimitBehavior>();
                Mock<IWin32Window> owner = mock.Mock<IWin32Window>();

                Mock<Func<IChannelLimitControl>> channelLimitCtrlRepo = mock.MockRepository.Create<Func<IChannelLimitControl>>();
                channelLimitCtrlRepo.Setup(x => x())
                    .Returns(control.Object);
                mock.Provide(channelLimitCtrlRepo.Object);

                Mock<IIndex<EditionFeature, IChannelLimitBehavior>> channelLimitBehaviorRepo = mock.MockRepository.Create<IIndex<EditionFeature, IChannelLimitBehavior>>();
                channelLimitBehaviorRepo.Setup(x => x[It.IsAny<EditionFeature>()])
                    .Returns(behavior.Object);
                mock.Provide(channelLimitBehaviorRepo.Object);

                ChannelLimitFactory testObject = mock.Create<ChannelLimitFactory>();

                testObject.CreateControl(license.Object, StoreTypeCode.Amazon, EditionFeature.EndiciaAccountLimit, owner.Object);

                control.VerifySet(c => c.DataContext = viewModel.Object);
            }
        }

        [Fact]
        public void CreateControl_CallsLoadWithCustomerLicense()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IChannelLimitViewModel> viewModel = mock.Mock<IChannelLimitViewModel>();
                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                Mock<IChannelLimitControl> control = mock.Mock<IChannelLimitControl>();
                Mock<IChannelLimitBehavior> behavior = mock.Mock<IChannelLimitBehavior>();
                Mock<IWin32Window> owner = mock.Mock<IWin32Window>();

                Mock<Func<IChannelLimitControl>> channelLimitCtrlRepo = mock.MockRepository.Create<Func<IChannelLimitControl>>();
                channelLimitCtrlRepo.Setup(x => x())
                    .Returns(control.Object);
                mock.Provide(channelLimitCtrlRepo.Object);

                Mock<IIndex<EditionFeature, IChannelLimitBehavior>> channelLimitBehaviorRepo = mock.MockRepository.Create<IIndex<EditionFeature, IChannelLimitBehavior>>();
                channelLimitBehaviorRepo.Setup(x => x[It.IsAny<EditionFeature>()])
                    .Returns(behavior.Object);
                mock.Provide(channelLimitBehaviorRepo.Object);

                ChannelLimitFactory testObject = mock.Create<ChannelLimitFactory>();

                testObject.CreateControl(license.Object, StoreTypeCode.Amazon, EditionFeature.EndiciaAccountLimit, owner.Object);

                viewModel.Verify(v => v.Load(license.Object, It.IsAny<IChannelLimitBehavior>()), Times.Once);
            }
        }

        [Fact]
        public void CreateControl_CallsLoadWithBehavior()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IChannelLimitViewModel> viewModel = mock.Mock<IChannelLimitViewModel>();
                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                Mock<IChannelLimitControl> control = mock.Mock<IChannelLimitControl>();
                Mock<IChannelLimitBehavior> behavior = mock.Mock<IChannelLimitBehavior>();
                Mock<IWin32Window> owner = mock.Mock<IWin32Window>();

                Mock<Func<IChannelLimitControl>> channelLimitCtrlRepo = mock.MockRepository.Create<Func<IChannelLimitControl>>();
                channelLimitCtrlRepo.Setup(x => x())
                    .Returns(control.Object);
                mock.Provide(channelLimitCtrlRepo.Object);

                Mock<IIndex<EditionFeature, IChannelLimitBehavior>> channelLimitBehaviorRepo = mock.MockRepository.Create<IIndex<EditionFeature, IChannelLimitBehavior>>();
                channelLimitBehaviorRepo.Setup(x => x[It.IsAny<EditionFeature>()])
                    .Returns(behavior.Object);
                mock.Provide(channelLimitBehaviorRepo.Object);

                ChannelLimitFactory testObject = mock.Create<ChannelLimitFactory>();

                testObject.CreateControl(license.Object, StoreTypeCode.Amazon, EditionFeature.EndiciaAccountLimit, owner.Object);

                viewModel.Verify(v => v.Load(It.IsAny<ICustomerLicense>(), behavior.Object), Times.Once);
            }
        }
    }
}