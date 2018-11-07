using Autofac;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// A registration override
    /// </summary>
    public interface IRegistrationOverride
    {
        /// <summary>
        /// Register the changes
        /// </summary>
        void Register(ContainerBuilder builder);
    }
}
