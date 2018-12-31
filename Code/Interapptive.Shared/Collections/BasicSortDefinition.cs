using System.ComponentModel;

namespace Interapptive.Shared.Collections
{
    /// <summary>
    /// Defines a sort
    /// </summary>
    public class BasicSortDefinition : IBasicSortDefinition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BasicSortDefinition(string name, ListSortDirection? direction)
        {
            Name = name;
            Direction = direction ?? ListSortDirection.Ascending;
        }

        /// <summary>
        /// Name of the thing to sort
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Direction of the sort
        /// </summary>
        public ListSortDirection Direction { get; }
    }
}
