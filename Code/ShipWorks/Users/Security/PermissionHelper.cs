using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model;
using System.Reflection;

namespace ShipWorks.Users.Security
{
    /// <summary>
    /// Utility class to help with permission stuff
    /// </summary>
    public static class PermissionHelper
    {
        static Dictionary<PermissionType, PermissionScope> scopeCache = new Dictionary<PermissionType, PermissionScope>();

        /// <summary>
        /// Get the "real" PermissionType represented by the given indirect permission and the specified EntityType
        /// </summary>
        public static PermissionType GetIndirectEntityActualPermission(PermissionType permission, EntityType entityType)
        {
            switch (permission)
            {
                case PermissionType.EntityTypeEditNotes:
                    return (entityType == EntityType.CustomerEntity) ? PermissionType.CustomersEditNotes : PermissionType.OrdersEditNotes;

                case PermissionType.EntityTypeSendEmail:
                    return (entityType == EntityType.CustomerEntity) ? PermissionType.CustomersSendEmail : PermissionType.OrdersSendEmail;

                default:
                    throw new InvalidOperationException("Unhandled indirect entity permission type.");
            }
        }

        /// <summary>
        /// Get the scope of the given permission type
        /// </summary>
        public static PermissionScope GetScope(PermissionType type)
        {
            lock (scopeCache)
            {
                if (scopeCache.Count == 0)
                {
                    foreach (System.Reflection.FieldInfo fieldInfo in typeof(PermissionType).GetFields(BindingFlags.Public | BindingFlags.Static))
                    {
                        PermissionScopeAttribute attribute = (PermissionScopeAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(PermissionScopeAttribute));
                        if (attribute == null)
                        {
                            throw new InvalidOperationException("Did not find PermissionScopeAttribute on PermissionType");
                        }

                        scopeCache[(PermissionType) Convert.ToInt32(fieldInfo.GetRawConstantValue())] = attribute.Scope;
                    }
                }
            }

            return scopeCache[type];
        }

    }
}
