﻿using System;
using System.Linq;
using System.Security;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.UI.Controls.CustomerLicenseActivation;
using ShipWorks.Users;
using Xunit;

namespace ShipWorks.UI.Tests.Controls.CustomerLicenseActivation
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
        public void Save_ValidatesUsername(string username, string password, bool isValid)
        {
            using (var mock = AutoMock.GetLoose())
            {
                CustomerLicenseActivationViewModel testObject = mock.Create<CustomerLicenseActivationViewModel>();

                var activationService = mock.Mock<ICustomerLicenseActivationService>();
                activationService.Setup(s => s.Activate(It.IsAny<string>(), It.IsAny<string>()));

                testObject.Email = username;

                using (SecureString securePassword = new SecureString())
                {
                    password.ToCharArray().ToList().ForEach(p => securePassword.AppendChar(p));

                    testObject.Password = securePassword;

                    GenericResult<ICustomerLicense> result = testObject.Save(true);

                    Assert.Equal(isValid, result.Success);
                    Assert.Equal(isValid ? null : "Please enter a valid email for the username.", result.Message);
                }
            }
        }

        [Theory]
        [InlineData("support@shipworks.com", "TestPassword", true)]
        [InlineData("support@shipworks.com", "", false)]
        public void Save_ValidatesPassword(string username, string password, bool isValid)
        {
            using (var mock = AutoMock.GetLoose())
            {
                CustomerLicenseActivationViewModel testObject = mock.Create<CustomerLicenseActivationViewModel>();

                var activationService = mock.Mock<ICustomerLicenseActivationService>();
                activationService.Setup(s => s.Activate(It.IsAny<string>(), It.IsAny<string>()));

                testObject.Email = username;

                using (SecureString securePassword = new SecureString())
                {
                    password.ToCharArray().ToList().ForEach(p => securePassword.AppendChar(p));

                    testObject.Password = securePassword;
                    GenericResult<ICustomerLicense> result = testObject.Save(true);
                    Assert.Equal(isValid, result.Success);
                    Assert.Equal(isValid ? null : "Please enter a password.", result.Message);
                }
            }
        }

        [Fact]
        public void SaveCalls_ICustomerLicenseActivate_WithUsernameAndDecryptedPassword()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string username = "support@shipworks.com";
                string password = "TestPassword";

                var activationService = mock.Mock<ICustomerLicenseActivationService>();
                activationService.Setup(s => s.Activate(It.IsAny<string>(), It.IsAny<string>()));

                CustomerLicenseActivationViewModel viewModel = mock.Create<CustomerLicenseActivationViewModel>();
                using (SecureString securePassword = new SecureString())
                {
                    password.ToCharArray().ToList().ForEach(p => securePassword.AppendChar(p));

                    viewModel.Email = username;
                    viewModel.Password = securePassword;
                    viewModel.Save(true);

                    activationService.Verify(l => l.Activate(username, password), Times.Once);
                }
            }
        }

        [Fact]
        public void SaveCalls_IUserManagerWrapperCreateUser_WithUsername_AndDecryptedPassword()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string username = "support@shipworks.com";
                string password = "TestPassword";

                var testObject = mock.Mock<IUserService>();

                CustomerLicenseActivationViewModel viewModel = mock.Create<CustomerLicenseActivationViewModel>();
                using (SecureString securePassword = new SecureString())
                {
                    password.ToCharArray().ToList().ForEach(p => securePassword.AppendChar(p));

                    viewModel.Email = username;
                    viewModel.Password = securePassword;
                    viewModel.Save(true);
                }
                testObject.Verify(l => l.CreateUser(username, password, true), Times.Once);

            }
        }

        [Fact]
        public void SaveCalls_IUserManagerWrapperDoesNotCreateUser_WhenFalseIsPassedIn()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string username = "support@shipworks.com";
                string password = "TestPassword";

                var testObject = mock.Mock<IUserService>();

                CustomerLicenseActivationViewModel viewModel = mock.Create<CustomerLicenseActivationViewModel>();
                using (SecureString securePassword = new SecureString())
                {
                    password.ToCharArray().ToList().ForEach(p => securePassword.AppendChar(p));

                    viewModel.Email = username;
                    viewModel.Password = securePassword;
                    viewModel.Save(false);
                }

                testObject.Verify(l => l.CreateUser(username, password, true), Times.Never);
            }
        }

        [Fact]
        public void SaveHandles_Exception_WithoutCrashing()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string username = "support@shipworks.com";
                string password = "TestPassword";

                mock.Mock<IUserService>().Setup(u => u.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Throws(new Exception("Random Exception"));

                CustomerLicenseActivationViewModel viewModel = mock.Create<CustomerLicenseActivationViewModel>();
                using (SecureString securePassword = new SecureString())
                {
                    password.ToCharArray().ToList().ForEach(p => securePassword.AppendChar(p));

                    viewModel.Email = username;
                    viewModel.Password = securePassword;
                    GenericResult<ICustomerLicense> testObject = viewModel.Save(true);

                    Assert.Equal(false, testObject.Success);
                    Assert.Equal("Random Exception", testObject.Message);
                }
            }
        }

        [Fact]
        public void CreateAccountLink_ReturnsCorrectUrl()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<CustomerLicenseActivationViewModel>();

                Assert.Equal("http://www.shipworks.com/registration/?source=si10883693", testObject.CreateAccountLink);
            }
        }

        [Fact]
        public void PasswordResetLink_ReturnsCorrectUrl()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<CustomerLicenseActivationViewModel>();

                Assert.Equal("https://www.interapptive.com/account/forgotpassword.php", testObject.PasswordResetLink);
            }
        }
    }
}