using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Users.Security
{
    /// <summary>
    /// Factory to create a security context
    /// </summary>
    [Component]
    public class SecurityContextFactory : ISecurityContextFactory
    {
        private readonly IConfigurationData configurationData;

        /// <summary>
        /// Constructor
        /// </summary>
        public SecurityContextFactory(IConfigurationData configurationData)
        {
            this.configurationData = configurationData;
        }

        /// <summary>
        /// Create a security context for the given user
        /// </summary>
        public ISecurityContext Create(UserEntity user)
        {
            ISecurityContext context = new SecurityContext(user);

            return IsArchive() ? new ArchiveSecurityContext(context) : context;
        }

        /// <summary>
        /// Are we currently in an archive database?
        /// </summary>
        private bool IsArchive() => false;
    }
}