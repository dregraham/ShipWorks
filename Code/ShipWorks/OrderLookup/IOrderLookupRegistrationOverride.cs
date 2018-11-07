using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// IoC registration overrides needed for the order lookup mode
    /// </summary>
    [Service]
    public interface IOrderLookupRegistrationOverride : IRegistrationOverride
    {

    }
}
