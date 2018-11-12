using ShipWorks.Filters.Content;

namespace ShipWorks.Filters.Search
{
    /// <summary>
    /// Class for creating filter definitions
    /// </summary>
    public interface ISearchDefinitionProvider
    {
        /// <summary>
        /// Gets a filter definition based on the provided quick search string.
        /// </summary>
        IFilterDefinition GetDefinition(string quickSearchString);
    }
}