using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using System.Reflection;
using System.Diagnostics;

namespace ShipWorks.Data.Model.Custom
{
    /// <summary>
    /// Manages the inherited\derived tables that are currently active.  Any that are not active will not be considered in polymorphic fetches.
    /// </summary>
    public static class ActiveTableInheritanceManager
    {
        static Dictionary<string, string[]> activeInheritanceMap = new Dictionary<string, string[]>();

        /// <summary>
        /// Static constructor
        /// </summary>
        static ActiveTableInheritanceManager()
        {
            Type llblSingleton = typeof(InheritanceInfoProviderSingleton);
            FieldInfo providerField = null;
            
            // We have to search for it rather than ask by name since it gets obfuscated
            foreach (FieldInfo field in llblSingleton.GetFields(BindingFlags.Static | BindingFlags.NonPublic))
            {
                if (field.FieldType == typeof(SD.LLBLGen.Pro.ORMSupportClasses.IInheritanceInfoProvider))
                {
                    providerField = field;
                    break;
                }
            }

            if (providerField == null)
            {
                throw new InvalidOperationException("Unable to reflect on InheritanceInfoProviderSingleton._providerInstance");
            }

            providerField.SetValue(null, new ActiveTableInheritanceInfoProvider());
        }

        /// <summary>
        /// Sets the active tables for the given root entity.  activeDerivecTypes can be null to clear the list (so they are all active).  Otherwise
        /// only entity types in the list will be considered for inheritance polymorphic fetches.
        /// For example, rootEntityType could be "OrderEntity", and the list could include "ProStoresOrderEntity" and "EbayOrderEntity"
        /// </summary>
        public static void SetActiveTables(string rootEntityName, IEnumerable<string> activeDerivedNames)
        {
            lock (activeInheritanceMap)
            {
                if (activeDerivedNames == null)
                {
                    activeInheritanceMap.Remove(rootEntityName);
                }
                else
                {
                    activeInheritanceMap[rootEntityName] = activeDerivedNames.ToArray();
                }
            }
        }

        /// <summary>
        /// Get the active derived tables for the given root entity name.  If there is no restriction for the given root null is returned.
        /// </summary>
        public static string[] GetActiveTables(string rootEntityName)
        {
            lock (activeInheritanceMap)
            {
                string[] activeList;
                if (activeInheritanceMap.TryGetValue(rootEntityName, out activeList))
                {
                    return activeList;
                }

                return null;
            }
        }
    }
}
