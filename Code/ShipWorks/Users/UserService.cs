using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users.Logon;
using System.Collections.Generic;

namespace ShipWorks.Users
{
    /// <summary>
    /// Wrapper for managing users
    /// </summary>
    public class UserService : IUserService
    {
        IUserSessionWrapper userSession;
        ILicenseFactory licenseFactory; 

        public UserService(IUserSessionWrapper userSession, ILicenseFactory licenseFactory)
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
        public GenericResult<LogonCredentials> Logon(LogonCredentials credentials)
         {
            GenericResult<LogonCredentials> result = new GenericResult<LogonCredentials>(credentials) { Success = true };
            
            // Check to see if each license allows logon
            CheckLicenseAllowsLogOn(result);

            if (!result.Success)
            {
                return result;
            }

            // All of the licenses allow logon, go ahead and do the logon
            if (userSession.Logon(credentials))
            {
                return result;
            }

            // if we got this far we must not be able to log on
            result.Success = false;
            result.Message = "Incorrect username or password.";
            return result;
        }

        /// <summary>
        /// Logs the user in using their saved credentials
        /// </summary>
        public GenericResult<LogonCredentials> Logon()
        {
            GenericResult<LogonCredentials> result = new GenericResult<LogonCredentials>(null) { Success = true};

            // Check to see if each license allows logon
            CheckLicenseAllowsLogOn(result);

            if(!result.Success)
            {
                return result;
            }

            // All of the licenses allow logon, go ahead and do the logon
            if (userSession.LogonLastUser())
            {
                return result;
            }

            // if we got this far we must not be able to log on
            result.Success = false;
            result.Message = "Incorrect username or password.";
            return result;
        }

        /// <summary>
        /// Checks to see if each license allows logon, if not it updates the result
        /// </summary>
        /// <param name="result"></param>
        private void CheckLicenseAllowsLogOn(GenericResult<LogonCredentials> result)
        {
            IEnumerable<ILicense> licenses = licenseFactory.GetLicenses();

            // check to see if each license allows logon
            foreach (ILicense license in licenses)
            {
                if (!license.AllowsLogOn())
                {
                    result.Success = false;
                    result.Message = license.DisabledReason;

                    // Return here if there are multiple licenses
                    // and one fails we want to return the failure 
                    return;
                }
            }
        }

    }
}