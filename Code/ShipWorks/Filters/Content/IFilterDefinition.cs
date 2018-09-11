using System.Collections.Generic;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Stores;

namespace ShipWorks.Filters.Content
{
    public interface IFilterDefinition
    {
        FilterDefinitionSourceType FilterDefinitionSource { get; }
        FilterTarget FilterTarget { get; }
        ConditionGroupContainer RootContainer { get; set; }

        string GetXml();
        bool HasRelativeDateCondition();
        bool IsEmpty();
        bool IsRelevantToStoreTypes(List<StoreTypeCode> storeTypes);

        string GenerateRootSql(FilterTarget fitlerTarget);
    }
}