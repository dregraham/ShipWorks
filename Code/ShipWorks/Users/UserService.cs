using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users.Logon;

namespace ShipWorks.Users
{
    /// <summary>
    /// Wrapper for managing users
    /// </summary>
    public class UserService : IUserService
    {
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
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Logs the user in using their saved credentials
        /// </summary>
        public GenericResult<LogonCredentials> Logon()
        {
            throw new System.NotImplementedException();
        }
    }
}