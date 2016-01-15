using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users.Logon;
using System;

namespace ShipWorks.Users
{
    /// <summary>
    /// Wrapper for managing users
    /// </summary>
    public class UserService : IUserService
    {
        readonly IUserSession userSession;
        readonly ILicenseService licenseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userSession">The user session.</param>
        /// <param name="licenseService">The license service.</param>
        public UserService(IUserSession userSession, ILicenseService licenseService)
        {
            this.userSession = userSession;
            this.licenseService = licenseService;
        }

        /// <summary>
        /// Creates a user with the given credentials
        /// </summary>
        public UserEntity CreateUser(string username, string password, bool isAdmin)
        {
            return UserUtility.CreateUser(username, username, password, isAdmin);
        }

        /// <summary>
        /// Logs the user in using their saved credentials
        /// </summary>
        public EnumResult<UserServiceLogonResultType> Logon()
        {
            Func<bool> loginWithOutCredentials = () => userSession.LogonLastUser();
            return Logon(loginWithOutCredentials);
        }

        /// <summary>
        /// Logs the user in
        /// </summary>
        public EnumResult<UserServiceLogonResultType> Logon(LogonCredentials credentials)
        {
            Func<bool> loginWithCredentials = () => userSession.Logon(credentials);
            return Logon(loginWithCredentials);
        }

        /// <summary>
        /// Check license, if ok, try to login to ShipWorks.
        /// </summary>
        private EnumResult<UserServiceLogonResultType> Logon(Func<bool> loginAction)
        {
            EnumResult<LogOnRestrictionLevel> allowsLogOn = licenseService.AllowsLogOn();

            // If Tango says the account is not allowed to logon, get the reason why
            if (allowsLogOn.Value == LogOnRestrictionLevel.Forbidden)
            {
                return new EnumResult<UserServiceLogonResultType>(UserServiceLogonResultType.TangoAccountDisabled,
                    allowsLogOn.Message);
            }

            bool loggedOnToShipWorks = loginAction();

            // At this point Tango says the user is allowed to logon to ShipWorks, but we still need
            // to check for valid credentials.
            return loggedOnToShipWorks ?
                new EnumResult<UserServiceLogonResultType>(UserServiceLogonResultType.Success) :
                new EnumResult<UserServiceLogonResultType>(UserServiceLogonResultType.InvalidCredentials,
                   "Incorrect username or password.");
        }
    }
}