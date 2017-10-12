using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ShipWorks.Shipping.Editing;
using Autofac;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Cache of customs control base objects based on shipment type
    /// </summary>
    public class CustomsControlCache : IDisposable
    {
        private readonly Dictionary<int, CustomsControlBase> customsControlCache = new Dictionary<int, CustomsControlBase>();
        private const int DefaultCustomsControlBaseCacheKey = -1;
        private readonly ILifetimeScope lifetimeScope;

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="lifetimeScope"></param>
        public CustomsControlCache(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        /// <summary>
        /// Get a customs control base by shipment type
        /// </summary>
        public CustomsControlBase Get(ShipmentType shipmentType)
        {
            int cacheKey = CacheKey(shipmentType);
            if (customsControlCache.ContainsKey(cacheKey))
            {
                return customsControlCache[cacheKey];
            }

            CustomsControlBase customsControl = cacheKey == DefaultCustomsControlBaseCacheKey ? new CustomsControlBase() : shipmentType.CreateCustomsControl(lifetimeScope);
            
            customsControl.Initialize();
            customsControl.Dock = DockStyle.Fill;

            customsControlCache.Add(cacheKey, customsControl);

            return customsControl;
        }

        /// <summary>
        /// Get a cache key based on shipment type.
        /// </summary>
        private int CacheKey(ShipmentType shipmentType)
        {
            if (shipmentType == null || shipmentType.ShipmentTypeCode == ShipmentTypeCode.None)
            {
                return DefaultCustomsControlBaseCacheKey;
            }

            switch (shipmentType.ShipmentTypeCode)
            {
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    return (int)ShipmentTypeCode.UpsOnLineTools;

                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                    return (int)ShipmentTypeCode.Usps;

                case ShipmentTypeCode.FedEx:
                    return (int)ShipmentTypeCode.FedEx;

                default:
                    return DefaultCustomsControlBaseCacheKey;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            foreach (CustomsControlBase customsControl in customsControlCache.Values)
            {
                customsControl?.Dispose();
            }

            customsControlCache.Clear();
        }
    }
}
