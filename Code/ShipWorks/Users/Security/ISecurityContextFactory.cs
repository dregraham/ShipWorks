using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Users.Security
{
    /// <summary>
    /// Factory to create a security context
    /// </summary>
    public interface ISecurityContextFactory
    {
        /// <summary>
        /// Create a security context for the given user
        /// </summary>
        ISecurityContext Create(UserEntity user);
    }
}