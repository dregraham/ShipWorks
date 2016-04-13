using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users.Logon;

namespace ShipWorks.Users
{
    /// <summary>
    /// Interface for the UserSession wrapper
    /// </summary>
    public interface IUserSession
    {
        /// <summary>
        /// Currently logged in user
        /// </summary>
        UserEntity User { get; }

        /// <summary>
        /// Logs the user in with the given credentials
        /// </summary>
        bool Logon(LogonCredentials credentials);

        /// <summary>
        /// Logs in using the last logged in user
        /// </summary>
        bool LogonLastUser();

        /// <summary>
        /// Is a user logged on
        /// </summary>
        bool IsLoggedOn { get; }
    }
}