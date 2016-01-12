using ShipWorks.Users.Logon;

namespace ShipWorks.Users
{
    /// <summary>
    /// Interface for wrapper for static user session class
    /// </summary>
    public interface IUserSessionWrapper
    {
        /// <summary>
        /// Logs the user in with the given credentials 
        /// </summary>
        bool Logon(LogonCredentials credentials);

        /// <summary>
        /// Logs in using the last logged in user
        /// </summary>
        bool LogonLastUser();
    }
}
