using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users.Logon;

namespace ShipWorks.Users
{
    /// <summary>
    /// Wrapper for static UserSession class
    /// </summary>
    public class UserSessionWrapper : IUserSession
    {
        public UserSessionWrapper() : this(UserSession.User)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UserSessionWrapper(UserEntity user)
        {
            User = user;
        }

        /// <summary>
        /// Currently logged in user
        /// </summary>
        public UserEntity User { get; }

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

        /// <summary>
        /// Is a user logged on
        /// </summary>
        public bool IsLoggedOn => UserSession.IsLoggedOn;
    }
}