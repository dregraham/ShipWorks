using ShipWorks.Filters.Content;

namespace ShipWorks.Filters.Search
{
    /// <summary>
    /// Creates SearchDefinitionProviderFactory
    /// </summary>
    public interface ISearchDefinitionProviderFactory
    {
        /// <summary>
        /// Creates a SearchDefinitionProvider for the specified target.
        /// </summary>
        ISearchDefinitionProvider Create(FilterTarget target, bool isBarcodeSearch);

        /// <summary>
        /// Creates a SearchDefinitionProvider for the specified target and FilterDefinition
        /// </summary>
        ISearchDefinitionProvider Create(FilterTarget target, FilterDefinition advancedSearchDefinition, bool isBarcodeSearch);
    }
}