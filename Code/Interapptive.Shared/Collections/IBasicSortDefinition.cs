using System.ComponentModel;

namespace Interapptive.Shared.Collections
{
    /// <summary>
    /// Defines a sort
    /// </summary>
    public interface IBasicSortDefinition
    {
        /// <summary>
        /// Name of the thing to sort
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Direction of the sort
        /// </summary>
        ListSortDirection Direction { get; }
    }
}
