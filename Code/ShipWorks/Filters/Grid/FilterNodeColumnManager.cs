using System;
using System.Collections.Generic;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Search;
using ShipWorks.Users;

namespace ShipWorks.Filters.Grid
{
    /// <summary>
    /// Manages loading and saving of grid column state for filters
    /// </summary>
    public static class FilterNodeColumnManager
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(FilterNodeColumnManager));

        // All column layouts we have loaded so far.
        static Dictionary<long, FilterNodeColumnSettings> userSettings = new Dictionary<long, FilterNodeColumnSettings>();
        static Dictionary<long, FilterNodeColumnSettings> defaultSettings = new Dictionary<long, FilterNodeColumnSettings>();

        /// <summary>
        /// Load user column data.
        /// </summary>
        public static void InitializeForCurrentSession()
        {
            // Clear any remnants from the last user
            userSettings.Clear();
            defaultSettings.Clear();

            // New holder for layouts
            userSettings = new Dictionary<long, FilterNodeColumnSettings>();
            defaultSettings = new Dictionary<long, FilterNodeColumnSettings>();
        }

        /// <summary>
        /// Get the column settings object for the given node for the current user
        /// </summary>
        public static FilterNodeColumnSettings GetUserSettings(FilterNodeEntity filterNode)
        {
            if (!UserSession.IsLoggedOn)
            {
                throw new InvalidOperationException("Cannot load column state when not logged in.");
            }

            FilterTarget filterTarget = (FilterTarget) filterNode.Filter.FilterTarget;

            // If its a search node, we use the common placeholder node, so all searches share the same columns
            if (filterNode.Purpose == (int) FilterNodePurpose.Search)
            {
                filterNode = SearchManager.GetPlaceholder(filterTarget);
            }

            FilterNodeColumnSettings settings;
            if (!userSettings.TryGetValue(filterNode.FilterNodeID, out settings))
            {
                log.DebugFormat("Loading User FilterNodeColumnSettings for node {0} ({1})", filterNode.FilterNodeID, filterNode.Filter.Name);

                // Create the grid settings for the node and user
                settings = new FilterNodeColumnSettings(
                    filterNode,
                    UserSession.User);

                userSettings[filterNode.FilterNodeID] = settings;
            }

            return settings;
        }

        /// <summary>
        /// Get the default column settings object for the given node
        /// </summary>
        public static FilterNodeColumnSettings GetDefaultSettings(FilterNodeEntity filterNode)
        {
            if (!UserSession.IsLoggedOn)
            {
                throw new InvalidOperationException("Cannot load column state when not logged in.");
            }

            FilterTarget filterTarget = (FilterTarget) filterNode.Filter.FilterTarget;

            // If its a search node, we use the common placeholder node, so all searches share the same columns
            if (filterNode.Purpose == (int) FilterNodePurpose.Search)
            {
                filterNode = SearchManager.GetPlaceholder(filterTarget);
            }

            FilterNodeColumnSettings settings;
            if (!defaultSettings.TryGetValue(filterNode.FilterNodeID, out settings))
            {
                log.DebugFormat("Loading Default FilterNodeColumnSettings for node {0} ({1})", filterNode.FilterNodeID, filterNode.Filter.Name);

                // Create the grid settings for the node and null-user (default)
                settings = new FilterNodeColumnSettings(
                    filterNode,
                    null);

                defaultSettings[filterNode.FilterNodeID] = settings;
            }

            return settings;
        }
    }
}
