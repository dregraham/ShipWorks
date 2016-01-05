﻿using System;
using System.Linq;
using System.Security;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.UI.Controls;
using ShipWorks.Users;
using Xunit;

namespace ShipWorks.Tests.UI.Controls.CustomerLicenseActivation
{
    public class CustomerLicenseActivationViewModelTest
    {
        [Theory]
        [InlineData("username", "TestPassword", false)]
        [InlineData("support@shipworks.com", "TestPassword", true)]
        [InlineData("", "TestPassword", false)]
        [InlineData(null, "TestPassword", false)]
        [InlineData("support@shipworkscom", "TestPassword", false)]
        [InlineData("supportshipworks.com", "TestPassword", false)]
        public void Save_Validates_Username(string username, string password, bool isValid)
        {
            using (var mock = AutoMock.GetLoose())
            {
                CustomerLicenseActivationViewModel testObject = mock.Create<CustomerLicenseActivationViewModel>();

                testObject.Username = username;

                SecureString securePassword = new SecureString();

                password.ToCharArray().ToList().ForEach(p => securePassword.AppendChar(p));

                testObject.Password = securePassword;

                GenericResult<ICustomerLicense> result = testObject.Save();
                
                Assert.Equal(isValid, result.Success);
                Assert.Equal(isValid ? null : "Please enter a valid username.", result.Message);
            }
        }

        [Theory]
        [InlineData("support@shipworks.com", "TestPassword", true)]
        [InlineData("support@shipworks.com", "", false)]
        public void Save_Validates_Password(string username, string password, bool isValid)
        {
            using (var mock = AutoMock.GetLoose())
            {
                CustomerLicenseActivationViewModel testObject = mock.Create<CustomerLicenseActivationViewModel>();

                testObject.Username = username;

                SecureString securePassword = new SecureString();

                password.ToCharArray().ToList().ForEach(p => securePassword.AppendChar(p));

                testObject.Password = securePassword;

                GenericResult<ICustomerLicense> result = testObject.Save();

                Assert.Equal(isValid, result.Success);
                Assert.Equal(isValid ? null : "Please enter a password.", result.Message);
            }
        }

        [Fact]
        public void SaveCalls_ICustomerLicenseActivate_WithUsername_AndDecriptedPassword()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string username = "support@shipworks.com";
                string password = "TestPassword";

                var testObject = mock.Mock<ICustomerLicense>();

                CustomerLicenseActivationViewModel viewModel = mock.Create<CustomerLicenseActivationViewModel>();
                SecureString securePassword = new SecureString();
                password.ToCharArray().ToList().ForEach(p => securePassword.AppendChar(p));

                viewModel.Username = username;
                viewModel.Password = securePassword;

                viewModel.Save();

                testObject.Verify(l => l.Activate(username, password), Times.Once);
            }
        }

        [Fact]
        public void SaveCalls_IUserManagerWrapperCreateUser_WithUsername_AndDecriptedPassword()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string username = "support@shipworks.com";
                string password = "TestPassword";

                var testObject = mock.Mock<IUserManagerWrapper>();

                CustomerLicenseActivationViewModel viewModel = mock.Create<CustomerLicenseActivationViewModel>();
                SecureString securePassword = new SecureString();
                password.ToCharArray().ToList().ForEach(p => securePassword.AppendChar(p));

                viewModel.Username = username;
                viewModel.Password = securePassword;

                viewModel.Save();

                testObject.Verify(l => l.CreateUser(username, password, true), Times.Once);
            }
        }

        [Fact]
        public void SaveHandels_Exception_WithoutCrashing()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string username = "support@shipworks.com";
                string password = "TestPassword";

                mock.Mock<IUserManagerWrapper>().Setup(u => u.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Throws(new Exception("Random Exception"));

                CustomerLicenseActivationViewModel viewModel = mock.Create<CustomerLicenseActivationViewModel>();
                SecureString securePassword = new SecureString();
                password.ToCharArray().ToList().ForEach(p => securePassword.AppendChar(p));

                viewModel.Username = username;
                viewModel.Password = securePassword;

                GenericResult<ICustomerLicense> testObject = viewModel.Save();
                
                Assert.Equal(false, testObject.Success);
                Assert.Equal("Random Exception", testObject.Message);
            }
        }
    }
}