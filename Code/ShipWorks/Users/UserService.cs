using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Users
{
    /// <summary>
    /// Wrapper for managing users
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// Creates a user with the given credentials
        /// </summary>
        public UserEntity CreateUser(string username, string password, bool isAdmin)
        {
            return UserUtility.CreateUser(username, username, password, isAdmin);
        }
    }
}