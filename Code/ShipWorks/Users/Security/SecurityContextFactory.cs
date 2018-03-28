using System;
using System.Xml;
using System.Xml.Linq;
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
        private readonly Func<UserEntity, SecurityContext> createBaseContext;

        /// <summary>
        /// Constructor
        /// </summary>
        public SecurityContextFactory(IConfigurationData configurationData, Func<UserEntity, SecurityContext> createBaseContext)
        {
            this.createBaseContext = createBaseContext;
            this.configurationData = configurationData;
        }

        /// <summary>
        /// Create a security context for the given user
        /// </summary>
        public ISecurityContext Create(UserEntity user)
        {
            ISecurityContext context = createBaseContext(user);

            return IsArchive() ? new ArchiveSecurityContext(context) : context;
        }

        /// <summary>
        /// Are we currently in an archive database?
        /// </summary>
        private bool IsArchive()
        {
            var xml = configurationData.FetchReadOnly().ArchivalSettingsXml;

            if (string.IsNullOrWhiteSpace(xml))
            {
                return false;
            }

            try
            {
                return XDocument.Parse(xml).Root.HasElements;
            }
            catch (XmlException)
            {
                return false;
            }
        }
    }
}