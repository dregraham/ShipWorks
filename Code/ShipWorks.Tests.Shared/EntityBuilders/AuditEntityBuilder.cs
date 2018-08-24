using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Tests.Shared.EntityBuilders
{
    /// <summary>
    /// Build an audit entity
    /// </summary>
    public class AuditEntityBuilder : EntityBuilder<AuditEntity>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AuditEntityBuilder(AuditEntity audit) : base(audit)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AuditEntityBuilder(IUserEntity user, IComputerEntity computer)
        {
            Set(x => x.UserID, user.UserID);
            Set(x => x.ComputerID, computer.ComputerID);
        }

        /// <summary>
        /// Add a change to the audit
        /// </summary>
        public AuditEntityBuilder WithChange() => WithChange(null);

        /// <summary>
        /// Add a change to the audit
        /// </summary>
        public AuditEntityBuilder WithChange(Action<EntityBuilder<AuditChangeEntity>> builderConfiguration)
        {
            EntityBuilder<AuditChangeEntity> builder = new EntityBuilder<AuditChangeEntity>();
            builderConfiguration?.Invoke(builder);

            Set(x => x.AuditChanges.Add(builder.Build()));

            return this;
        }
    }
}