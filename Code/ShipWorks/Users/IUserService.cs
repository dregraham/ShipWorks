using ShipWorks.Data.Model.EntityClasses;

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
    }
}