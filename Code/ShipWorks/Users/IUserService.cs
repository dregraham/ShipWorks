using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users.Logon;

namespace ShipWorks.Users
{
    /// <summary>
    /// Interface for managing users
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Create a user for the given credentials
        /// </summary>
        UserEntity CreateUser(string username, string password, bool isAdmin);

        /// <summary>
        /// Logs the user in
        /// </summary>
        GenericResult<LogonCredentials> Logon(LogonCredentials credentials);

        /// <summary>
        /// Logs the user in using their saved credentials
        /// </summary>
        GenericResult<LogonCredentials> Logon();
    }
}