using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Users.Logon;

namespace ShipWorks.Users
{
    /// <summary>
    /// Wrapper for static UserSession class
    /// </summary>
    [Component]
    public class UserSessionWrapper : IUserSession
    {
        /// <summary>
        /// Currently logged in user
        /// </summary>
        public UserEntity User => UserSession.User;

        /// <summary>
        /// Currently logged in computer
        /// </summary>
        public ComputerEntity Computer => UserSession.Computer;

        /// <summary>
        /// Currently logged in user's settings
        /// </summary>
        public IUserSettingsEntity Settings => UserSession.User?.Settings;

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