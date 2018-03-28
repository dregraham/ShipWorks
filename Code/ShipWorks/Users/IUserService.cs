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
        EnumResult<UserServiceLogonResultType> Logon(LogonCredentials credentials);

        /// <summary>
        /// Logs the user in using their saved credentials
        /// </summary>
        EnumResult<UserServiceLogonResultType> Logon();

        /// <summary>
        /// Logs the user in with the given UserEntity
        /// </summary>
        /// <remarks>
        /// This method goes through MainForm so that we get the full logon flow
        /// </remarks>
        EnumResult<UserServiceLogonResultType> Logon(UserEntity user, bool audit);
    }
}