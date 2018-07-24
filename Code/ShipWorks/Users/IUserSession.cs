using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
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
        /// Currently logged in computer
        /// </summary>
        ComputerEntity Computer { get; }

        /// <summary>
        /// Currently logged in user's settings
        /// </summary>
        IUserSettingsEntity Settings { get; }

        /// <summary>
        /// Logs the user in with the given credentials
        /// </summary>
        bool Logon(LogonCredentials credentials);

        /// <summary>
        /// Log in the specified user
        /// </summary>
        bool Logon(UserEntity user, bool audit);

        /// <summary>
        /// Logs in using the last logged in user
        /// </summary>
        bool LogonLastUser();

        /// <summary>
        /// Log off the currently logged on user.
        /// </summary>
        bool Logoff(bool clearRememberMe);

        /// <summary>
        /// Is a user logged on
        /// </summary>
        bool IsLoggedOn { get; }

        /// <summary>
        /// Update the current user's settings
        /// </summary>
        void UpdateSettings(Action<UserSettingsEntity> updateAction);
    }
}