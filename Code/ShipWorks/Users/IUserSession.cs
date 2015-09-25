using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Users
{
    /// <summary>
    /// Interface for the UserSession wrapper
    /// </summary>
    public interface IUserSession
    {
        UserEntity User { get; }
    }
}