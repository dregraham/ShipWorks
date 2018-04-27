using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Users
{
    /// <summary>
    /// User login workflow
    /// </summary>
    public interface IUserLoginWorkflow
    {
        /// <summary>
        /// Get the current user
        /// </summary>
        UserEntity CurrentUser { get; }

        /// <summary>
        /// Logs the user in with the given UserEntity
        /// </summary>
        /// <remarks>
        /// This method goes through MainForm so that we get the full logon flow
        /// </remarks>
        void Logon(UserEntity user);

        /// <summary>
        /// Log off the currently logged on user.
        /// </summary>
        /// <remarks>
        /// This method goes through MainForm so that we get the full logoff flow
        /// </remarks>
        bool Logoff(bool clearRememberMe);
    }
}