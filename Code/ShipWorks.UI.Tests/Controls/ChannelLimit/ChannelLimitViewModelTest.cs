﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.UI.Controls.ChannelConfirmDelete;
using ShipWorks.UI.Controls.ChannelLimit;
using ShipWorks.UI.Controls.WebBrowser;
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
                ShipWorksLicenseException ex = Assert.Throws<ShipWorksLicenseException>(() => testObject.Load(storeLicense.Object as ICustomerLicense));

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
                var webBrowserFactory = mock.Mock<IWebBrowserFactory>();
                webBrowserFactory.Setup(w => w.Create(It.IsAny<Uri>(), It.IsAny<string>())).Returns(dialog.Object);
                
                var testObject = mock.Create<ChannelLimitViewModel>();
                ICommand upgradeClickCommand = testObject.UpgradeClickCommand;
                upgradeClickCommand.Execute(null);

                dialog.Verify(d=>d.ShowDialog(), Times.Once);
            }
        }

        [Fact]
        public void Deletechannel_WithOneChannel_ShowsError()
        {
            using (var mock = AutoMock.GetLoose())
            {
                StoreEntity store = new StoreEntity {TypeCode = (int) StoreTypeCode.Amazon};

                IEnumerable<StoreEntity> stores = new List<StoreEntity> {store};

                Mock<IMessageHelper> messageHelper = mock.Mock<IMessageHelper>();

                Mock<IStoreManager> storeManager = mock.Mock<IStoreManager>();
                storeManager.Setup(s => s.GetAllStores())
                    .Returns(stores);
                
                var testObject = mock.Create<ChannelLimitViewModel>();
                testObject.SelectedStoreType = StoreTypeCode.Amazon;

                ICommand deleteCommand = testObject.DeleteStoreClickCommand;
                deleteCommand.Execute(null);

                messageHelper.Verify(m => m.ShowError("You cannot remove Amazon because it is the only channel in your ShipWorks database."), Times.Once);
            }
        }

        [Fact]
        public void Deletechannel_ShowsConfirmDeleteDlg()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Setup
                StoreEntity store = new StoreEntity {TypeCode = (int) StoreTypeCode.Amazon};

                IEnumerable<StoreEntity> stores = new List<StoreEntity> {store};
                
                Mock<IStoreManager> storeManager = mock.Mock<IStoreManager>();
                storeManager.Setup(s => s.GetAllStores())
                    .Returns(stores);

                Mock<IChannelConfirmDeleteDlg> confirmDelete = mock.Mock<IChannelConfirmDeleteDlg>();
                confirmDelete.Setup(d => d.DialogResult)
                    .Returns(false);

                Mock<IChannelConfirmDeleteFactory> confirmDeleteFactgory = mock.Mock<IChannelConfirmDeleteFactory>();
                confirmDeleteFactgory.Setup(c => c.GetConfirmDeleteDlg(It.IsAny<StoreTypeCode>()))
                    .Returns(confirmDelete.Object);
                
                // Test
                var testObject = mock.Create<ChannelLimitViewModel>();
                testObject.SelectedStoreType = StoreTypeCode.Ebay;

                ICommand deleteCommand = testObject.DeleteStoreClickCommand;
                deleteCommand.Execute(null);
                
                // Verify
                confirmDelete.Verify(d => d.ShowDialog(), Times.Once);
            }
        }

        [Fact]
        public void Deletechannel_WithConfirmDeleteTrue_DoesDeleteChannel()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Setup
                StoreEntity store = new StoreEntity { TypeCode = (int)StoreTypeCode.Amazon };

                IEnumerable<StoreEntity> stores = new List<StoreEntity> { store };

                Mock<IStoreManager> storeManager = mock.Mock<IStoreManager>();
                storeManager.Setup(s => s.GetAllStores())
                    .Returns(stores);

                Mock<IChannelConfirmDeleteDlg> confirmDelete = mock.Mock<IChannelConfirmDeleteDlg>();
                confirmDelete.Setup(d => d.DialogResult)
                    .Returns(true);

                Mock<IChannelConfirmDeleteFactory> confirmDeleteFactgory = mock.Mock<IChannelConfirmDeleteFactory>();
                confirmDeleteFactgory.Setup(c => c.GetConfirmDeleteDlg(It.IsAny<StoreTypeCode>()))
                    .Returns(confirmDelete.Object);

                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();

                // Test
                var testObject = mock.Create<ChannelLimitViewModel>();
                testObject.Load(license.Object);
                testObject.SelectedStoreType = StoreTypeCode.Ebay;

                ICommand deleteCommand = testObject.DeleteStoreClickCommand;
                deleteCommand.Execute(null);

                // Verify
                license.Verify(l => l.DeleteChannel(StoreTypeCode.Ebay), Times.Once);
            }
        }

        [Fact]
        public void Deletechannel_WithDeclindedConfirmDelete_DoesNotDeleteChannel()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Setup
                StoreEntity store = new StoreEntity { TypeCode = (int)StoreTypeCode.Amazon };

                IEnumerable<StoreEntity> stores = new List<StoreEntity> { store };

                Mock<IStoreManager> storeManager = mock.Mock<IStoreManager>();
                storeManager.Setup(s => s.GetAllStores())
                    .Returns(stores);

                Mock<IChannelConfirmDeleteDlg> confirmDelete = mock.Mock<IChannelConfirmDeleteDlg>();
                confirmDelete.Setup(d => d.DialogResult)
                    .Returns(false);

                Mock<IChannelConfirmDeleteFactory> confirmDeleteFactgory = mock.Mock<IChannelConfirmDeleteFactory>();
                confirmDeleteFactgory.Setup(c => c.GetConfirmDeleteDlg(It.IsAny<StoreTypeCode>()))
                    .Returns(confirmDelete.Object);

                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();

                // Test
                var testObject = mock.Create<ChannelLimitViewModel>();
                testObject.Load(license.Object);
                testObject.SelectedStoreType = StoreTypeCode.Ebay;

                ICommand deleteCommand = testObject.DeleteStoreClickCommand;
                deleteCommand.Execute(null);

                // Verify
                license.Verify(l => l.DeleteChannel(StoreTypeCode.Ebay), Times.Never);
            }
        }
    }
}
