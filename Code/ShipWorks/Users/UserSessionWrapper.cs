using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
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
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public UserSessionWrapper(ISqlAdapterFactory sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

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

        /// <summary>
        /// Update the current user's settings
        /// </summary>
        public void UpdateSettings(Action<UserSettingsEntity> updateAction)
        {
            UserSettingsEntity userSettings = UserSession.User?.Settings;

            if (userSettings == null)
            {
                return;
            }

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                updateAction(userSettings);
                sqlAdapter.SaveAndRefetch(userSettings);
            }
        }
    }
}