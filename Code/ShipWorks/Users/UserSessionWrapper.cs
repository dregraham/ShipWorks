using ShipWorks.Users.Logon;

namespace ShipWorks.Users
{
    /// <summary>
    /// Wrapper for the static UserSession class
    /// </summary>
    public class UserSessionWrapper : IUserSession
    {
        /// <summary>
        /// Logs the user in with the given credentials 
        /// </summary>
        public bool Logon(LogonCredentials credentials)
        {
            return UserSession.Logon(credentials.Username, credentials.Password, credentials.Remember);
        }

        /// <summary>
        /// Logs in using the last logged in user
        /// </summary>
        /// <returns></returns>
        public bool LogonLastUser()
        {
            return UserSession.LogonLastUser();
        }
    }
}