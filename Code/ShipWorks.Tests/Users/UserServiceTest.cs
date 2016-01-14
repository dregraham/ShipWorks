using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Users;
using ShipWorks.Users.Logon;
using Xunit;

namespace ShipWorks.Tests.Users
{
    public class UserServiceTest
    {
        [Fact]
        public void LogOnWithCredentials_AllowLogonIsCalledOnce()
        {
            using (var mock = AutoMock.GetLoose())
            {
                LogonCredentials credentials = new LogonCredentials("foo", "bar", false);

                Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
                licenseService.Setup(s => s.AllowsLogOn())
                    .Returns(new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.Forbidden, "Some Disabled Reason"));

                UserService userService = mock.Create<UserService>();

                userService.Logon(credentials);

                licenseService.Verify(f => f.AllowsLogOn(), Times.Once);
            }
        }

        [Fact]
        public void LogOnWithCredentials_MessageIsDisabledReason_WhenLicenseServiceReturnsAllowsLogOnNo()
        {
            using (var mock = AutoMock.GetLoose())
            {
                LogonCredentials credentials = new LogonCredentials("foo", "bar", false);

                Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
                licenseService.Setup(s => s.AllowsLogOn())
                    .Returns(new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.Forbidden, "Some Disabled Reason"));

                UserService userService = mock.Create<UserService>();

                EnumResult<UserServiceLogonResultType> testobject = userService.Logon(credentials);

                Assert.Equal("Some Disabled Reason", testobject.Message);
            }
        }

        [Fact]
        public void LogOnWithCredentials_DelegatesToUserSession()
        {
            using (var mock = AutoMock.GetLoose())
            {
                LogonCredentials credentials = new LogonCredentials("foo", "bar", false);

                Mock<ILicenseService> licenseFactory = mock.Mock<ILicenseService>();
                licenseFactory.Setup(lf => lf.AllowsLogOn())
                    .Returns(new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.None));

                Mock<IUserSession> userSessionWrapper = mock.Mock<IUserSession>();
                userSessionWrapper.Setup(u => u.Logon(It.IsAny<LogonCredentials>()))
                    .Returns(true);

                UserService userService = mock.Create<UserService>();

                userService.Logon(credentials);

                userSessionWrapper.Verify(u => u.Logon(credentials), Times.Once);
            }
        }

        [Fact]
        public void LogOnWithCredentials_SetsValueToTrue_WhenAllowsLogonAndCredentialsAreValidated()
        {
            using (var mock = AutoMock.GetLoose())
            {
                LogonCredentials credentials = new LogonCredentials("foo", "bar", false);

                mock.Mock<ILicenseService>()
                    .Setup(lf => lf.AllowsLogOn())
                    .Returns(new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.None));

                mock.Mock<IUserSession>()
                    .Setup(u => u.Logon(It.IsAny<LogonCredentials>()))
                    .Returns(true);

                UserService userService = mock.Create<UserService>();

                EnumResult<UserServiceLogonResultType> testobject = userService.Logon(credentials);

                Assert.Equal(UserServiceLogonResultType.Success, testobject.Value);
            }
        }

        [Fact]
        public void LogOnWithCredentials_ReturnsInvalidCredentialsValue_WhenLogonFails()
        {
            using (var mock = AutoMock.GetLoose())
            {
                LogonCredentials credentials = new LogonCredentials("foo", "bar", false);

                mock.Mock<ILicenseService>()
                    .Setup(lf => lf.AllowsLogOn())
                    .Returns(new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.None));

                mock.Mock<IUserSession>()
                    .Setup(u => u.Logon(It.IsAny<LogonCredentials>()))
                    .Returns(false);

                UserService userService = mock.Create<UserService>();

                EnumResult<UserServiceLogonResultType> testobject = userService.Logon(credentials);

                Assert.Equal(UserServiceLogonResultType.InvalidCredentials, testobject.Value);
            }
        }

        [Fact]
        public void LogOnWithCredentials_ReturnsInvalidCredentialsMessage_WhenLogonFails()
        {
            using (var mock = AutoMock.GetLoose())
            {
                LogonCredentials credentials = new LogonCredentials("foo", "bar", false);

                mock.Mock<ILicenseService>()
                    .Setup(lf => lf.AllowsLogOn())
                    .Returns(new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.None));

                mock.Mock<IUserSession>()
                    .Setup(u => u.Logon(It.IsAny<LogonCredentials>()))
                    .Returns(false);

                UserService userService = mock.Create<UserService>();

                EnumResult<UserServiceLogonResultType> testobject = userService.Logon(credentials);

                Assert.Equal("Incorrect username or password.", testobject.Message);
            }
        }


        [Fact]
        public void LogOn_DelegatesToLicenseServiceAllowsLogOn_ForAllLicenses()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
                licenseService.Setup(lf => lf.AllowsLogOn())
                    .Returns(new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.Forbidden, "Some Disabled Reason"));

                UserService testObject = mock.Create<UserService>();

                testObject.Logon();

                licenseService.Verify(f => f.AllowsLogOn(), Times.Once);
            }
        }

        [Fact]
        public void LogOn_ReturnsAccountDisabled_WhenLicenseServiceAllowsLogOnReturnsNo()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
                licenseService.Setup(lf => lf.AllowsLogOn())
                    .Returns(new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.Forbidden, "Some Disabled Reason" ));

                UserService testObject = mock.Create<UserService>();

                EnumResult<UserServiceLogonResultType> logonResult = testObject.Logon();

                Assert.Equal(UserServiceLogonResultType.TangoAccountDisabled, logonResult.Value);
            }
        }

        [Fact]
        public void LogOn_ReturnsCorrectMessage_WhenLicenseServiceAllowsLogOnReturnsForbiddenAndMessage()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
                licenseService.Setup(lf => lf.AllowsLogOn())
                    .Returns(new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.Forbidden, "Some Disabled Reason" ));

                UserService testObject = mock.Create<UserService>();

                EnumResult<UserServiceLogonResultType> logonResult = testObject.Logon();

                Assert.Equal("Some Disabled Reason", logonResult.Message);
            }
        }

        [Fact]
        public void LogOn_DelegatesToUserSession()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseService> licenseFactory = mock.Mock<ILicenseService>();
                licenseFactory.Setup(lf => lf.AllowsLogOn())
                    .Returns(new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.None));
                
                Mock<IUserSession> userSessionWrapper = mock.Mock<IUserSession>();
                userSessionWrapper.Setup(u => u.LogonLastUser())
                    .Returns(true);

                UserService userService = mock.Create<UserService>();
                userService.Logon();

                userSessionWrapper.Verify(u => u.LogonLastUser(), Times.Once);
            }
        }

        [Fact]
        public void LogOn_ReturnsSuccess_WhenAllowsLogonAndUserSessionReturnsTrue()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseService> licenseFactory = mock.Mock<ILicenseService>();
                licenseFactory.Setup(lf => lf.AllowsLogOn())
                    .Returns(new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.None));

                Mock<IUserSession> userSessionWrapper = mock.Mock<IUserSession>();
                userSessionWrapper.Setup(u => u.LogonLastUser())
                    .Returns(true);

                UserService userService = mock.Create<UserService>();

                EnumResult<UserServiceLogonResultType> testobject = userService.Logon();

                Assert.Equal(UserServiceLogonResultType.Success, testobject.Value);
            }
        }

        [Fact]
        public void LogOn_ReturnsIncorrectUsernameMessage_WhenLogonFails()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseService> licenseFactory = mock.Mock<ILicenseService>();
                licenseFactory.Setup(lf => lf.AllowsLogOn())
                    .Returns(new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.None));

                Mock<IUserSession> userSessionWrapper = mock.Mock<IUserSession>();
                userSessionWrapper.Setup(u => u.LogonLastUser())
                    .Returns(false);

                UserService userService = mock.Create<UserService>();

                EnumResult<UserServiceLogonResultType> testobject = userService.Logon();

                userSessionWrapper.Verify(u => u.LogonLastUser(), Times.Once);
                Assert.Equal("Incorrect username or password.", testobject.Message);
            }
        }

        [Fact]
        public void LogOn_ReturnsInvalidCredentialsValue_WhenLogonFails()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseService> licenseFactory = mock.Mock<ILicenseService>();
                licenseFactory.Setup(lf => lf.AllowsLogOn())
                    .Returns(new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.None));

                Mock<IUserSession> userSessionWrapper = mock.Mock<IUserSession>();
                userSessionWrapper.Setup(u => u.LogonLastUser())
                    .Returns(false);

                UserService userService = mock.Create<UserService>();

                EnumResult<UserServiceLogonResultType> testobject = userService.Logon();

                userSessionWrapper.Verify(u => u.LogonLastUser(), Times.Once);
                Assert.Equal(UserServiceLogonResultType.InvalidCredentials, testobject.Value);
            }
        }
    }
}
