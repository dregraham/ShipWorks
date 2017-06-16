using Autofac.Features.Indexed;
using ShipWorks.ApplicationCore;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Factory for creating label services
    /// </summary>
    [Component]
    public class LabelServiceFactory : Factory<ShipmentTypeCode, ILabelService>, ILabelServiceFactory
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LabelServiceFactory(IIndex<ShipmentTypeCode, ILabelService> lookup) : base(lookup)
        {
        }
    }
}
