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
        public void LogOnWithCredentials_Checks_AllowsLogOn_ForAllLicenses()
        {
            using (var mock = AutoMock.GetLoose())
            {
                LogonCredentials credentials = new LogonCredentials("foo", "bar", false);

                Mock<ILicenseService> licenseService = mock.Mock<ILicenseService>();
                licenseService.Setup(s => s.AllowsLogOn())
                    .Returns(new EnumResult<AllowsLogOn>(AllowsLogOn.No) {Message = "Some Disabled Reason"});

                UserService userService = mock.Create<UserService>();

                EnumResult<UserServiceLogonResultType> testobject = userService.Logon(credentials);

                licenseService.Verify(f => f.AllowsLogOn(), Times.Once);
                Assert.Equal("Some Disabled Reason", testobject.Message);
            }
        }

        [Fact]
        public void LogOnWithCredentials_Calls_UserSessionLogOnWithCredentials()
        {
            using (var mock = AutoMock.GetLoose())
            {
                LogonCredentials credentials = new LogonCredentials("foo", "bar", false);

                Mock<ILicenseService> licenseFactory = mock.Mock<ILicenseService>();
                licenseFactory.Setup(lf => lf.AllowsLogOn())
                    .Returns(new EnumResult<AllowsLogOn>(AllowsLogOn.Yes));

                Mock<IUserSession> userSessionWrapper = mock.Mock<IUserSession>();
                userSessionWrapper.Setup(u => u.Logon(It.IsAny<LogonCredentials>()))
                    .Returns(true);

                UserService userService = mock.Create<UserService>();

                EnumResult<UserServiceLogonResultType> testobject = userService.Logon(credentials);

                userSessionWrapper.Verify(u => u.Logon(credentials), Times.Once);
                Assert.Equal(UserServiceLogonResultType.Success, testobject.Value);
            }
        }

        [Fact]
        public void LogOnWithCredentials_Returns_IncorrectUsernameMessage_WhenLogonFails()
        {
            using (var mock = AutoMock.GetLoose())
            {
                LogonCredentials credentials = new LogonCredentials("foo", "bar", false);

                Mock<ILicenseService> licenseFactory = mock.Mock<ILicenseService>();
                licenseFactory.Setup(lf => lf.AllowsLogOn())
                    .Returns(new EnumResult<AllowsLogOn>(AllowsLogOn.Yes));

                Mock<IUserSession> userSessionWrapper = mock.Mock<IUserSession>();
                userSessionWrapper.Setup(u => u.Logon(It.IsAny<LogonCredentials>()))
                    .Returns(false);

                UserService userService = mock.Create<UserService>();

                EnumResult<UserServiceLogonResultType> testobject = userService.Logon(credentials);

                userSessionWrapper.Verify(u => u.Logon(credentials), Times.Once);
                Assert.Equal(UserServiceLogonResultType.InvalidCredentials, testobject.Value);
                Assert.Equal("Incorrect username or password.", testobject.Message);
            }
        }

        [Fact]
        public void LogOn_Checks_AllowsLogOn_ForAllLicenses()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseService> licenseFactory = mock.Mock<ILicenseService>();

                licenseFactory.Setup(lf => lf.AllowsLogOn())
                    .Returns(new EnumResult<AllowsLogOn>(AllowsLogOn.No) {Message = "Some Disabled Reason"});

                UserService userService = mock.Create<UserService>();

                EnumResult<UserServiceLogonResultType> testobject = userService.Logon();

                licenseFactory.Verify(f => f.AllowsLogOn(), Times.Once);
                Assert.Equal(UserServiceLogonResultType.TangoAccountDisabled, testobject.Value);
                Assert.Equal("Some Disabled Reason", testobject.Message);
            }
        }

        [Fact]
        public void LogOn_Calls_UserSessionLogOnLastUser()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseService> licenseFactory = mock.Mock<ILicenseService>();
                licenseFactory.Setup(lf => lf.AllowsLogOn())
                    .Returns(new EnumResult<AllowsLogOn>(AllowsLogOn.Yes));


                Mock<IUserSession> userSessionWrapper = mock.Mock<IUserSession>();
                userSessionWrapper.Setup(u => u.LogonLastUser())
                    .Returns(true);

                UserService userService = mock.Create<UserService>();

                EnumResult<UserServiceLogonResultType> testobject = userService.Logon();

                userSessionWrapper.Verify(u => u.LogonLastUser(), Times.Once);
                Assert.Equal(UserServiceLogonResultType.Success, testobject.Value);
            }
        }

        [Fact]
        public void LogOn_Returns_IncorrectUsernameMessage_WhenLogonFails()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ILicenseService> licenseFactory = mock.Mock<ILicenseService>();
                licenseFactory.Setup(lf => lf.AllowsLogOn())
                    .Returns(new EnumResult<AllowsLogOn>(AllowsLogOn.Yes));

                Mock<IUserSession> userSessionWrapper = mock.Mock<IUserSession>();
                userSessionWrapper.Setup(u => u.LogonLastUser())
                    .Returns(false);

                UserService userService = mock.Create<UserService>();

                EnumResult<UserServiceLogonResultType> testobject = userService.Logon();

                userSessionWrapper.Verify(u => u.LogonLastUser(), Times.Once);
                Assert.Equal(UserServiceLogonResultType.InvalidCredentials, testobject.Value);
                Assert.Equal("Incorrect username or password.", testobject.Message);
            }
        }
    }
}
