using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Warehouse.Configuration.Filters.DTO;

namespace ShipWorks.Warehouse.Configuration.Filters
{
    /// <summary>
    /// Class to configure filters downloaded from the Hub
    /// </summary>
    [Component]
    public class HubFilterConfigurator : IHubFilterConfigurator
    {
        private readonly IFilterHelper filterHelper;
        private int progressIndex = 1;
        private int totalFiltersToImport;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubFilterConfigurator(IFilterHelper filterHelper)
        {
            this.filterHelper = filterHelper;
        }

        /// <summary>
        /// Configure the filters downloaded from Hub
        /// </summary>
        public void Configure(IEnumerable<GetFiltersResponse> filtersResponse, IProgressReporter filterProgress)
        {
            totalFiltersToImport = filtersResponse.Count();

            try
            {
                FilterLayoutContext.PushScope();

                var ordersNode = FilterLayoutContext.Current.FindNode(BuiltinFilter.GetTopLevelKey(FilterTarget.Orders));
                var newFilters = new List<(FilterEntity filter, Guid parentID)>();

                foreach (var response in filtersResponse)
                {
                    if (response.Path.Equals("root"))
                    {
                        ordersNode.Filter.HubFilterID = response.HubFilterID.ToString("N");
                    }
                    else
                    {
                        var definition = new FilterDefinition(response.DefinitionXML);

                        var newFilter = filterHelper.CreateFilterEntity(response.Name, definition);
                        newFilter.HubFilterID = response.HubFilterID.ToString("N");

                        if (filtersResponse.Any(x => x.ParentFilterID == response.HubFilterID))
                        {
                            newFilter.IsFolder = true;
                        }

                        newFilters.Add((newFilter, response.ParentFilterID));
                    }
                }

                var parentNodes = new List<FilterNodeEntity> { ordersNode };

                do
                {
                    AddFilters(newFilters, ref parentNodes, filterProgress);
                }
                while (parentNodes.Any());
            }
            catch (Exception ex)
            {
                /// TODO: Add logging / error handling
            }
            finally
            {
                FilterLayoutContext.PopScope();
            }

            filterProgress.Detail = "Done";
            filterProgress.Completed();
        }

        /// <summary>
        /// Add the filters to the database
        /// </summary>
        private void AddFilters(List<(FilterEntity filter, Guid parentID)> newFilters, ref List<FilterNodeEntity> parentNodes, IProgressReporter filterProgress)
        {
            var newParents = new List<FilterNodeEntity>();

            foreach (var parent in parentNodes)
            {
                var parentID = Guid.Parse(parent.Filter.HubFilterID);

                var children = newFilters.Where(x => x.parentID == parentID);

                int childIndex = 0;

                foreach (var child in children)
                {
                    filterProgress.Detail = $"Importing '{child.filter.Name}' ({progressIndex} of {totalFiltersToImport})";
                    newParents.Add(FilterLayoutContext.Current.AddFilter(child.filter, parent, childIndex)[0]);
                    childIndex++;
                    progressIndex++;
                }
            }

            parentNodes = newParents;
        }
    }
}
