using ShipWorks.Filters.Content;

namespace ShipWorks.Filters.Search
{
    /// <summary>
    /// Class for creating empty filter definitions with the given filter target
    /// </summary>
    /// <seealso cref="ShipWorks.Filters.Search.IFilterDefinitionProvider" />
    public class EmptyDefinitionProvider : IFilterDefinitionProvider
    {
        private readonly FilterTarget target;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyDefinitionProvider"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        public EmptyDefinitionProvider(FilterTarget target)
        {
            this.target = target;
        }

        /// <summary>
        /// Returns a new filter definition with the given filter target.
        /// </summary>
        public FilterDefinition GetDefinition(string quickSearchString)
        {
            return new FilterDefinition(target);
        }
    }
}