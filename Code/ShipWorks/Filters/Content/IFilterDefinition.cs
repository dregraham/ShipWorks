using System.Collections.Generic;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Stores;

namespace ShipWorks.Filters.Content
{
    /// <summary>
    /// The object model of a filter definition, which is the set of conditions that make up a filter.
    /// </summary>
    public interface IFilterDefinition
    {
        /// <summary>
        /// The source of the filter definition
        /// </summary>
        FilterDefinitionSourceType FilterDefinitionSource { get; }

        /// <summary>
        /// The FilterTarget the definition applies to
        /// </summary>
        FilterTarget FilterTarget { get; }

        /// <summary>
        /// The root container of the filter definition
        /// </summary>
        ConditionGroupContainer RootContainer { get; set; }

        /// <summary>
        /// Get this filter definition in XML form.
        /// </summary>
        string GetXml();

        /// <summary>
        /// Returns true if the filter contains a date condition that uses a relative operator
        /// </summary>
        bool HasRelativeDateCondition();

        /// <summary>
        /// Indiciates if no conditions are configured and the definition should match zero content.
        /// </summary>
        bool IsEmpty();

        /// <summary>
        /// Indicates if the definition is relevant given the list of storetypes.  If for example the definition contained an eBay only condition,
        /// but there was no ebay store type, then it's not relevant.
        /// </summary>
        bool IsRelevantToStoreTypes(List<StoreTypeCode> storeTypes);

        /// <summary>
        /// Generate SQL from the root container
        /// </summary>
        string GenerateRootSql(FilterTarget fitlerTarget);
    }
}