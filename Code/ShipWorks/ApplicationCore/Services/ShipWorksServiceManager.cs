using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Services
{
    /// <summary>
    /// Utility class for managing all ShipWorks service types
    /// </summary>
    public static class ShipWorksServiceManager
    {
        static Type[] serviceTypesCache;

        /// <summary>
        /// Gets service instances for all ShipWorks service types.
        /// </summary>
        public static ShipWorksServiceBase[] GetAllServices()
        {
            if (serviceTypesCache == null)
            {
                serviceTypesCache = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t.BaseType == typeof(ShipWorksServiceBase))
                    .ToArray();
            }

            return serviceTypesCache
                .Select(Activator.CreateInstance)
                .Cast<ShipWorksServiceBase>()
                .ToArray();
        }

        /// <summary>
        /// Gets a service instance for a ShipWorks service type.
        /// </summary>
        public static ShipWorksServiceBase GetService(ShipWorksServiceType serviceType)
        {
            return GetAllServices().Single(s => s.ServiceType == serviceType);
        }

        /// <summary>
        /// Gets a service instance for a ShipWorks service type name.
        /// </summary>
        public static ShipWorksServiceBase GetService(string serviceTypeName)
        {
            var results = EnumHelper.GetEnumList<ShipWorksServiceType>().Where(e => string.Compare(e.ApiValue, serviceTypeName, true) == 0).Select(e => e.Value);
            if (results.Count() != 1)
            {
                throw new NotFoundException("A ShipWorks service of the name '" + serviceTypeName + "' was not found.");
            }

            return GetService(results.First());
        }

        /// <summary>
        /// Gets the instance-specific service name for a ShipWorks service type.
        /// </summary>
        public static string GetServiceName(ShipWorksServiceType serviceType)
        {
            return GetServiceName(serviceType, ShipWorksSession.InstanceID);
        }

        /// <summary>
        /// Gets the instance-specific service name for a ShipWorks service type.
        /// </summary>
        public static string GetServiceName(ShipWorksServiceType serviceType, Guid instanceID)
        {
            return "ShipWorks" + EnumHelper.GetApiValue(serviceType) + "$" + instanceID.ToString("N");
        }

        /// <summary>
        /// Gets the instance-specific display name for a ShipWorks service type.
        /// </summary>
        public static string GetDisplayName(ShipWorksServiceType serviceType)
        {
            return GetDisplayName(serviceType, ShipWorksSession.InstanceID);
        }

        /// <summary>
        /// Gets the instance-specific display name for a ShipWorks service type.
        /// </summary>
        public static string GetDisplayName(ShipWorksServiceType serviceType, Guid instanceID)
        {
            return EnumHelper.GetDescription(serviceType) + " " + instanceID.ToString("B").ToUpper();
        }
    }
}
