﻿using System.Collections.Generic;
using System.Windows.Input;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Stores;
using ShipWorks.UI.Controls.ChannelLimit;
using Xunit;

namespace ShipWorks.UI.Tests.Controls.ChannelLimit
{
    public class ChannelLimitViewModelTest
    {
        [Fact]
        public void Load_WithStoreLicense_ThrowsShipWorksLicenseException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicense> storeLicense = mock.Mock<ILicense>();
                Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
                licenseService.Setup(l => l.GetLicenses()).Returns(new List<ILicense> { storeLicense.Object });

                ChannelLimitViewModel testObject = mock.Create<ChannelLimitViewModel>();

                // The constructor of ChannelLimitViewModel checks to see if the license is a ICustomerLicense
                // Creating the concrete class will throw the exception
                ShipWorksLicenseException ex = Assert.Throws<ShipWorksLicenseException>(() => testObject.Load((ICustomerLicense)storeLicense.Object));

                // Check to make sure the right message is thrown
                Assert.Equal("Store licenses do not have channel limits.", ex.Message);
            }
        }

        [Fact]
        public void Load_SetsSelectedStoreTypeToInvalid()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ICustomerLicense> customerLicense = mock.Mock<ICustomerLicense>();
                Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
                licenseService.Setup(l => l.GetLicenses()).Returns(new List<ILicense> { customerLicense.Object});
                
                ChannelLimitViewModel testObject = mock.Create<ChannelLimitViewModel>();

                // Set the SelectedStoreType to something other than invalid
                testObject.SelectedStoreType = StoreTypeCode.Amazon;
                
                // Call load
                testObject.Load(customerLicense.Object);

                // check the SelectedStoreType and ensure it is set to invalid
                Assert.Equal(StoreTypeCode.Invalid, testObject.SelectedStoreType);
            }
        }

        [Fact]
        public void Load_RefreshesLicense()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ICustomerLicense> customerLicense = mock.Mock<ICustomerLicense>();
                Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
                licenseService.Setup(l => l.GetLicenses()).Returns(new List<ILicense> { customerLicense.Object });

                ChannelLimitViewModel testObject = mock.Create<ChannelLimitViewModel>();

                // Call load
                testObject.Load(customerLicense.Object);

                customerLicense.Verify(c => c.Refresh(), Times.Once);
            }
        }

        [Fact]
        public void UpgradeAccount_ShowsDialog()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var dialog = mock.Mock<IDialog>();

                var testObject = mock.Create<ChannelLimitViewModel>();
                ICommand upgradeClickCommand = testObject.UpgradeClickCommand;
                upgradeClickCommand.Execute(null);

                dialog.Verify(d=>d.ShowDialog(), Times.Once);
            }
        }
    }
}
