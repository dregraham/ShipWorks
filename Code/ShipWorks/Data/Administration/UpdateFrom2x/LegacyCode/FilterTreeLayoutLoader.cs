using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content;
using ShipWorks.Filters;
using System.Xml;

namespace ShipWorks.Data.Administration.UpdateFrom2x.LegacyCode
{
    /// <summary>
    /// Base node for FilterV2 and and FolderV2
    /// </summary>
    public abstract class NodeV2
    {
        public string Name { get; set; }
    }

    /// <summary>
    /// Represents a V2 filter to be migrated
    /// </summary>
    public class FilterV2 : NodeV2
    {
        public FilterDefinition Definition { get; set; }
    }

    /// <summary>
    /// Represents a V2 filter folder to be migrated
    /// </summary>
    public class FolderV2 : NodeV2
    {
        public List<NodeV2> Children { get; set; }
    }

    /// <summary>
    /// Code taken from FilterTree class of V2 for loading filter layout XML
    /// </summary>
    public static class FilterTreeLayoutLoader
    {
        /// <summary>
        /// Load the layout of the specified layoutXml for the given target and set of filters
        /// </summary>
        public static List<NodeV2> LoadLayout(string layoutXml, FilterTarget filterTarget, List<FilterV2> filters)
        {
            List<FilterV2> allFilters = filters.Where(f => f.Definition.FilterTarget == filterTarget).ToList();
            string xpathQuery = (filterTarget == FilterTarget.Customers) ? "//FilterLayout/Folder[@Name='Customers']" : "//FilterLayout/Folder[@Name='Orders']";

            // Create the list
            List<NodeV2> folder = new List<NodeV2>();

            if (layoutXml != "")
            {
                try
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(layoutXml);

                    XmlElement rootElement = (XmlElement) xmlDocument.SelectSingleNode(xpathQuery);
                    LoadFilterFolderChildren(folder, rootElement, allFilters, filterTarget);

                }
                catch
                {

                }
            }

            // Whichever are left after organizing, add them to the top in order
            allFilters.Reverse();

            foreach (NodeV2 item in allFilters)
            {
                folder.Insert(0, item);
            }

            return folder;
        }

        /// <summary>
        /// Load a filter folder based on the given element
        /// </summary>
        private static void LoadFilterFolderChildren(List<NodeV2> folder, XmlElement element, List<FilterV2> allFilters, FilterTarget target)
        {
            // Look at all children
            foreach (XmlElement child in element.ChildNodes)
            {
                if (child.Name == "Folder")
                {
                    // Create the folder
                    FolderV2 childFolder = new FolderV2();
                    childFolder.Name = child.Attributes["Name"].Value;
                    childFolder.Children = new List<NodeV2>();

                    folder.Add(childFolder);

                    LoadFilterFolderChildren(childFolder.Children, child, allFilters, target);
                }
                else if (child.Name == "Filter")
                {
                    FilterV2 filter = FindFilter(child.Attributes["Name"].Value, allFilters);

                    if (filter != null)
                    {
                        allFilters.Remove(filter);

                        folder.Add(filter);
                    }
                }
            }
        }

        /// <summary>
        /// Find the filter by the given name in the given list of filters.
        /// </summary>
        private static FilterV2 FindFilter(string name, List<FilterV2> filters)
        {
            foreach (FilterV2 filter in filters)
            {
                if (filter.Name == name)
                {
                    return filter;
                }
            }

            return null;
        }
    }
}
