using System;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Factory to create a ShippingProfileApplicationStrategy
    /// </summary>
    [Component]
    public class ShippingProfileApplicationStrategyFactory : IShippingProfileApplicationStrategyFactory
    {
        private readonly IIndex<ShipmentTypeCode, IShippingProfileApplicationStrategy> strategyChooser;
        private readonly Func<IShippingProfileApplicationStrategy> createGlobalStrategy;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileApplicationStrategyFactory(IIndex<ShipmentTypeCode, IShippingProfileApplicationStrategy> strategyChooser,
            Func<IShippingProfileApplicationStrategy> createGlobalStrategy)
        {
            this.strategyChooser = strategyChooser;
            this.createGlobalStrategy = createGlobalStrategy;
        }

        /// <summary>
        /// Create an Profile Application Strategy
        /// </summary>
        public IShippingProfileApplicationStrategy Create(ShipmentTypeCode? shipmentType) => 
            shipmentType == null ? createGlobalStrategy() : strategyChooser[shipmentType.Value];
    }
}