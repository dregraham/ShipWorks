using ShipWorks.Data.Model.EntityClasses;

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

        public UserSessionWrapper(UserEntity user)
        {
            User = user;
        }

        public UserEntity User { get; }
    }
}