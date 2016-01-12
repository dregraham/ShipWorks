using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using Divelements.SandGrid;
using Interapptive.Shared;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor.WebServices.Oms;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    /// <summary>
    /// Utility class for dealing with Maretkworks flags
    /// </summary>
    public static class MarketplaceAdvisorOmsFlagManager
    {
        // Maps marketorks user names to the custom flags for that account
        static Dictionary<string, OMFlags> customFlagsMap = new Dictionary<string, OMFlags>();

        /// <summary>
        /// Load the grid with flags from the given store
        /// </summary>
        public static void LoadFlagGrid(MarketplaceAdvisorStoreEntity store, MarketplaceAdvisorOmsFlagTypes currentFlags, SandGrid sandGrid, bool includeStockFlags)
        {
            sandGrid.Rows.Clear();

            OMFlags customFlags = GetCustomFlags(store);

            LoadFlagGrid(customFlags, currentFlags, sandGrid, includeStockFlags);
        }

        /// <summary>
        /// Load the grid with the given flags
        /// </summary>
        public static void LoadFlagGrid(OMFlags customFlags, MarketplaceAdvisorOmsFlagTypes currentFlags, SandGrid sandGrid, bool includeStockFlags)
        {
            if (includeStockFlags)
            {
                AddFlagToGrid(sandGrid, currentFlags, "Payment Cleared", MarketplaceAdvisorOmsFlagTypes.PaymentCleared);
                AddFlagToGrid(sandGrid, currentFlags, "Payment Method Changed", MarketplaceAdvisorOmsFlagTypes.PayMethodChanged);
                AddFlagToGrid(sandGrid, currentFlags, "Order Partially Shipped", MarketplaceAdvisorOmsFlagTypes.PartiallyShipped);
            }

            AddFlagToGrid(sandGrid, currentFlags, customFlags.FlagName1, MarketplaceAdvisorOmsFlagTypes.Custom1);
            AddFlagToGrid(sandGrid, currentFlags, customFlags.FlagName2, MarketplaceAdvisorOmsFlagTypes.Custom2);
            AddFlagToGrid(sandGrid, currentFlags, customFlags.FlagName3, MarketplaceAdvisorOmsFlagTypes.Custom3);
            AddFlagToGrid(sandGrid, currentFlags, customFlags.FlagName4, MarketplaceAdvisorOmsFlagTypes.Custom4);
            AddFlagToGrid(sandGrid, currentFlags, customFlags.FlagName5, MarketplaceAdvisorOmsFlagTypes.Custom5);
            AddFlagToGrid(sandGrid, currentFlags, customFlags.FlagName6, MarketplaceAdvisorOmsFlagTypes.Custom6);
            AddFlagToGrid(sandGrid, currentFlags, customFlags.FlagName7, MarketplaceAdvisorOmsFlagTypes.Custom7);
            AddFlagToGrid(sandGrid, currentFlags, customFlags.FlagName8, MarketplaceAdvisorOmsFlagTypes.Custom8);
            AddFlagToGrid(sandGrid, currentFlags, customFlags.FlagName9, MarketplaceAdvisorOmsFlagTypes.Custom9);
            AddFlagToGrid(sandGrid, currentFlags, customFlags.FlagName10, MarketplaceAdvisorOmsFlagTypes.Custom10);
        }

        /// <summary>
        /// Read the list of selected flags from the grid
        /// </summary>
        public static MarketplaceAdvisorOmsFlagTypes ReadFlagGrid(SandGrid sandGrid)
        {
            int flags = 0;

            foreach (GridRow row in sandGrid.Rows)
            {
                if (row.Checked)
                {
                    flags |= (int) row.Tag;
                }
            }

            return (MarketplaceAdvisorOmsFlagTypes) flags;
        }

        /// <summary>
        /// Get the custom flags for the given store.  This offers caching if they have already been retrieved, vs. the client.GetCustomFlags which gets
        /// them every time.
        /// </summary>
        public static OMFlags GetCustomFlags(MarketplaceAdvisorStoreEntity store)
        {
            lock (customFlagsMap)
            {
                OMFlags customFlags;
                if (!customFlagsMap.TryGetValue(store.Username, out customFlags))
                {
                    customFlags = MarketplaceAdvisorOmsClient.Create(store).GetCustomFlags();
                    customFlagsMap[store.Username] = customFlags;
                }

                return customFlags;
            }
        }

        /// <summary>
        /// Add the given flag to the grid for user selection
        /// </summary>
        private static void AddFlagToGrid(SandGrid sandGrid, MarketplaceAdvisorOmsFlagTypes currentFlags, string name, MarketplaceAdvisorOmsFlagTypes flagType)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            GridRow row = new GridRow(name);
            row.Tag = flagType;

            if (IsFlagSet(currentFlags, flagType))
            {
                row.Checked = true;
            }

            sandGrid.Rows.Add(row);
        }

        /// <summary>
        /// Indicates if the given flag is set in the current group of flags
        /// </summary>
        private static bool IsFlagSet(MarketplaceAdvisorOmsFlagTypes currentFlags, MarketplaceAdvisorOmsFlagTypes flagType)
        {
            return (((int) currentFlags) & ((int) flagType)) == (int) flagType;
        }

        /// <summary>
        /// Build the order flags object to be used for downloading orders.
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        public static OMOrderFlags BuildOMOrderFlags(MarketplaceAdvisorOmsFlagTypes flagsOn, MarketplaceAdvisorOmsFlagTypes flagsOff)
        {
            OMOrderFlags orderFlags = new OMOrderFlags();

            // Default all of them to ignore
            orderFlags.PaymentCleared = FlagState.Ignore;
            orderFlags.PaymentMethodChanged = FlagState.Ignore;
            orderFlags.OrderPartiallyShipped = FlagState.Ignore;

            orderFlags.FlagStatuses = new OMCustomFlags();
            orderFlags.FlagStatuses.Flag1Status = FlagState.Ignore;
            orderFlags.FlagStatuses.Flag2Status = FlagState.Ignore;
            orderFlags.FlagStatuses.Flag3Status = FlagState.Ignore;
            orderFlags.FlagStatuses.Flag4Status = FlagState.Ignore;
            orderFlags.FlagStatuses.Flag5Status = FlagState.Ignore;
            orderFlags.FlagStatuses.Flag6Status = FlagState.Ignore;
            orderFlags.FlagStatuses.Flag7Status = FlagState.Ignore;
            orderFlags.FlagStatuses.Flag8Status = FlagState.Ignore;
            orderFlags.FlagStatuses.Flag9Status = FlagState.Ignore;
            orderFlags.FlagStatuses.Flag10Status = FlagState.Ignore;

            // Set all the ones to on
            if (IsFlagSet(flagsOn, MarketplaceAdvisorOmsFlagTypes.PaymentCleared)) orderFlags.PaymentCleared = FlagState.On;
            if (IsFlagSet(flagsOn, MarketplaceAdvisorOmsFlagTypes.PayMethodChanged)) orderFlags.PaymentMethodChanged = FlagState.On;
            if (IsFlagSet(flagsOn, MarketplaceAdvisorOmsFlagTypes.PartiallyShipped)) orderFlags.OrderPartiallyShipped = FlagState.On;
            if (IsFlagSet(flagsOn, MarketplaceAdvisorOmsFlagTypes.Custom1)) orderFlags.FlagStatuses.Flag1Status = FlagState.On;
            if (IsFlagSet(flagsOn, MarketplaceAdvisorOmsFlagTypes.Custom2)) orderFlags.FlagStatuses.Flag2Status = FlagState.On;
            if (IsFlagSet(flagsOn, MarketplaceAdvisorOmsFlagTypes.Custom3)) orderFlags.FlagStatuses.Flag3Status = FlagState.On;
            if (IsFlagSet(flagsOn, MarketplaceAdvisorOmsFlagTypes.Custom4)) orderFlags.FlagStatuses.Flag4Status = FlagState.On;
            if (IsFlagSet(flagsOn, MarketplaceAdvisorOmsFlagTypes.Custom5)) orderFlags.FlagStatuses.Flag5Status = FlagState.On;
            if (IsFlagSet(flagsOn, MarketplaceAdvisorOmsFlagTypes.Custom6)) orderFlags.FlagStatuses.Flag6Status = FlagState.On;
            if (IsFlagSet(flagsOn, MarketplaceAdvisorOmsFlagTypes.Custom7)) orderFlags.FlagStatuses.Flag7Status = FlagState.On;
            if (IsFlagSet(flagsOn, MarketplaceAdvisorOmsFlagTypes.Custom8)) orderFlags.FlagStatuses.Flag8Status = FlagState.On;
            if (IsFlagSet(flagsOn, MarketplaceAdvisorOmsFlagTypes.Custom9)) orderFlags.FlagStatuses.Flag9Status = FlagState.On;
            if (IsFlagSet(flagsOn, MarketplaceAdvisorOmsFlagTypes.Custom10)) orderFlags.FlagStatuses.Flag10Status = FlagState.On;

            // Set all the ones to ff
            if (IsFlagSet(flagsOff, MarketplaceAdvisorOmsFlagTypes.PaymentCleared)) orderFlags.PaymentCleared = FlagState.Off;
            if (IsFlagSet(flagsOff, MarketplaceAdvisorOmsFlagTypes.PayMethodChanged)) orderFlags.PaymentMethodChanged = FlagState.Off;
            if (IsFlagSet(flagsOff, MarketplaceAdvisorOmsFlagTypes.PartiallyShipped)) orderFlags.OrderPartiallyShipped = FlagState.Off;
            if (IsFlagSet(flagsOff, MarketplaceAdvisorOmsFlagTypes.Custom1)) orderFlags.FlagStatuses.Flag1Status = FlagState.Off;
            if (IsFlagSet(flagsOff, MarketplaceAdvisorOmsFlagTypes.Custom2)) orderFlags.FlagStatuses.Flag2Status = FlagState.Off;
            if (IsFlagSet(flagsOff, MarketplaceAdvisorOmsFlagTypes.Custom3)) orderFlags.FlagStatuses.Flag3Status = FlagState.Off;
            if (IsFlagSet(flagsOff, MarketplaceAdvisorOmsFlagTypes.Custom4)) orderFlags.FlagStatuses.Flag4Status = FlagState.Off;
            if (IsFlagSet(flagsOff, MarketplaceAdvisorOmsFlagTypes.Custom5)) orderFlags.FlagStatuses.Flag5Status = FlagState.Off;
            if (IsFlagSet(flagsOff, MarketplaceAdvisorOmsFlagTypes.Custom6)) orderFlags.FlagStatuses.Flag6Status = FlagState.Off;
            if (IsFlagSet(flagsOff, MarketplaceAdvisorOmsFlagTypes.Custom7)) orderFlags.FlagStatuses.Flag7Status = FlagState.Off;
            if (IsFlagSet(flagsOff, MarketplaceAdvisorOmsFlagTypes.Custom8)) orderFlags.FlagStatuses.Flag8Status = FlagState.Off;
            if (IsFlagSet(flagsOff, MarketplaceAdvisorOmsFlagTypes.Custom9)) orderFlags.FlagStatuses.Flag9Status = FlagState.Off;
            if (IsFlagSet(flagsOff, MarketplaceAdvisorOmsFlagTypes.Custom10)) orderFlags.FlagStatuses.Flag10Status = FlagState.Off;

            return orderFlags;
        }
    }
}
