using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Filters.Management
{
    /// <summary>
    /// A class to encapsulate logic for searching locations where a filter node is referenced.
    /// </summary>
    public class FilterNodeReferenceRepository
    {
        /// <summary>Finds the references to the given filter node.</summary>
        /// <param name="filterNode">The filter node.</param>
        /// <returns>A List of strings indicating each place the filter node is used.</returns>
        public List<string> Find(FilterNodeEntity filterNode)
        {
            List<FilterNodeEntity> linkNodesToDelete = new List<FilterNodeEntity>();

            // Generate a list of all nodes to be deleted, if only the link is deleted
            linkNodesToDelete.AddRange(FilterHelper.GetNodesUsingSequence(filterNode.FilterSequence));

            List<string> linkReasons = ObjectReferenceManager.GetReferenceReasons(linkNodesToDelete.Select(n => n.FilterNodeID).ToList());
            return linkReasons;
        }
    }
}
