using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users.Logon;

namespace ShipWorks.Users
{
    /// <summary>
    /// Wrapper for managing users
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserSession userSession;
        private readonly ILicenseFactory licenseFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public UserService(IUserSession userSession, ILicenseFactory licenseFactory)
        {
            this.userSession = userSession;
            this.licenseFactory = licenseFactory;
        }

        /// <summary>
        /// Creates a user with the given credentials
        /// </summary>
        public UserEntity CreateUser(string username, string password, bool isAdmin)
        {
            return UserUtility.CreateUser(username, username, password, isAdmin);
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
        /// Logs the user in using their saved credentials
        /// </summary>
        public EnumResult<UserServiceLogonResultType> Logon()
        {
            Func<bool> loginWithOutCredentials = () => userSession.LogonLastUser();
            return Logon(loginWithOutCredentials);
        }

        /// <summary>
        /// Check license, if ok, try to login to shipworks.
        /// </summary>
        private EnumResult<UserServiceLogonResultType> Logon(Func<bool> loginAction)
        {
            IEnumerable<ILicense> licenses = licenseFactory.GetLicenses();
            ILicense disallowedLicense = licenses.FirstOrDefault(l => l.AllowsLogOn());

            if (disallowedLicense != null)
            {
                return new EnumResult<UserServiceLogonResultType>(UserServiceLogonResultType.TangoAccountDisabled)
                {
                    Message = disallowedLicense.DisabledReason
                };
            }

            bool loggedOnToShipWorks = loginAction();

            if (loggedOnToShipWorks)
            {
                return new EnumResult<UserServiceLogonResultType>(UserServiceLogonResultType.Success);
            }

            return new EnumResult<UserServiceLogonResultType>(UserServiceLogonResultType.InvalidCredentials)
            {
                Message = "Invalid username and password"
            };
        }
    }
}