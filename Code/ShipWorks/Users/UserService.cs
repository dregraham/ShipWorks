using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users.Logon;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Users
{
    /// <summary>
    /// Wrapper for managing users
    /// </summary>
    public class UserService : IUserService
    {
        IUserSession userSession;
        ILicenseService licenseService; 

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
        /// Check license, if ok, try to login to shipworks.
        /// </summary>
        private EnumResult<UserServiceLogonResultType> Logon(Func<bool> loginAction)
        {
            EnumResult<AllowsLogOn> allowsLogOn = licenseService.AllowsLogOn();
            
            if (allowsLogOn.Value == AllowsLogOn.No)
            {
                return new EnumResult<UserServiceLogonResultType>(UserServiceLogonResultType.TangoAccountDisabled)
                {
                    Message = allowsLogOn.Message
                };
            }

            bool loggedOnToShipWorks = loginAction();

            if (loggedOnToShipWorks)
            {
                return new EnumResult<UserServiceLogonResultType>(UserServiceLogonResultType.Success);
            }

            return new EnumResult<UserServiceLogonResultType>(UserServiceLogonResultType.InvalidCredentials)
            {
                Message = "Incorrect username or password."
            };
        }
    }
}