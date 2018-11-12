using Autofac;
using ShipWorks.AddressValidation;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// IoC registration overrides needed for the order lookup mode
    /// </summary>
    public class CoreOrderLookupRegistrationOverride : IOrderLookupRegistrationOverride
    {
        /// <summary>
        /// Register the changes
        /// </summary>
        public void Register(ContainerBuilder builder) =>
            builder.RegisterType<ValidatedAddressScope>().AsImplementedInterfaces().SingleInstance();
    }
}
