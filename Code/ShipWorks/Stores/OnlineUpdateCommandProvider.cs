using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore.Interaction;
using System.Collections.ObjectModel;
using ShipWorks.Data.Utility;
using System.Windows.Forms;
using System.ComponentModel;
using Interapptive.Shared;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.UI;
using ShipWorks.Data;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Utility class for working with the Online Update commands for stores
    /// </summary>
    public class OnlineUpdateCommandProvider
    {
        #region class OnlineUpdateCommands

        class OnlineUpdateCommandSet
        {
            public StoreTypeCode StoreTypeCode;

            public List<MenuCommand> CommonCommands;
            public Dictionary<StoreType, List<MenuCommand>> InstanceCommands;
        }

        #endregion

        // This is the command set, broken down by storetype, that is used to generate the final menu structure
        Dictionary<StoreTypeCode, OnlineUpdateCommandSet> storeTypeCommands = new Dictionary<StoreTypeCode, OnlineUpdateCommandSet>();

        // This is the actual list of commands that was generated in the order and structure that they are displayed in the ui
        List<MenuCommand> commands = new List<MenuCommand>();

        /// <summary>
        /// Constructor
        /// </summary>
        public OnlineUpdateCommandProvider()
        {

        }

        /// <summary>
        /// Creates a new set of Online Update commands based on the current stores.  The new set is saved and tracked for update state operations.
        /// Any previously created set is discarded.
        /// </summary>
        public List<MenuCommand> CreateOnlineUpdateCommands(IEnumerable<long> selected)
        {
            storeTypeCommands = CreateCommandsByStoreType(selected);
            commands = BuildCommandLayout(storeTypeCommands);

            if (commands.Count == 0)
            {
                commands.Add(new MenuCommand("The selected orders cannot be updated online.") { Enabled = false });
            }

            return commands;
        }

        /// <summary>
        /// Execute the command.  Returns true if all headers were processed by the command, or false if some could not be processed because they did not
        /// match the store type supported by the command.
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        public void ExecuteCommandAsync(MenuCommand command, Control owner, IEnumerable<long> selectedKeys, MenuCommandCompleteEventHandler callback)
        {
            OnlineUpdateCommandSet commandSet = FindCommandSet(command);

            List<long> relevantKeys;

            // If its a common command, it just has to match store types
            if (commandSet.CommonCommands.Contains(command))
            {
                // Build the set of headers to process
                relevantKeys = selectedKeys.Select(orderID => DataProvider.GetOrderHeader(orderID)).Where(
                    header => StoreManager.GetStore(header.StoreID).TypeCode == (int) commandSet.StoreTypeCode && !header.IsManual).Select(header => header.OrderID).ToList();
            }
            else
            {
                // Find the exact instance
                StoreType storeType = commandSet.InstanceCommands.Where(p => p.Value.Contains(command)).Select(p => p.Key).Single();

                // Build the set of headers to process
                relevantKeys = selectedKeys.Select(orderID => DataProvider.GetOrderHeader(orderID)).Where(
                    header => header.StoreID == storeType.Store.StoreID && !header.IsManual).Select(header => header.OrderID).ToList();
            }

            if (relevantKeys.Count != selectedKeys.Count())
            {
                string warningText = "Some orders will not be updated since they belong to a different store.";

                if (DialogResult.OK != MessageBox.Show(owner, warningText, "ShipWorks", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                {
                    callback(command, new MenuCommandCompleteEventArgs());
                    return;
                }
            }

            bool permissionWarning = false;

            // If it's per type, then two stores of the same type could be selected and still have relevant keys - but one of them may not have permissions.
            // We have to futher filter out by permission.
            if (commandSet.CommonCommands.Contains(command))
            {
                int countBefore = relevantKeys.Count;

                relevantKeys = relevantKeys.Where(orderID => UserSession.Security.HasPermission(PermissionType.OrdersEditStatus, DataProvider.GetOrderHeader(orderID).StoreID)).ToList();

                // See if any were filtered out due to permissions
                permissionWarning = countBefore != relevantKeys.Count;
            }

            // Kick off the processing
            command.ExecuteAsync(owner, relevantKeys, (sender, e) =>
                {
                    MenuCommandCompleteEventArgs finalArgs = e;

                    if (permissionWarning)
                    {
                        string message = "Some orders were not updated due to insufficient permission.";

                        if (e.Result == MenuCommandResult.Success)
                        {
                            finalArgs = new MenuCommandCompleteEventArgs(MenuCommandResult.Warning, message);
                        }
                        else
                        {
                            finalArgs = new MenuCommandCompleteEventArgs(e.Result, e.Message + "\n" + message);
                        }
                    }

                    callback(sender, finalArgs);
                });
        }

        /// <summary>
        /// Find the command set that holds the given command.  Throws an exception if not found.
        /// </summary>
        private OnlineUpdateCommandSet FindCommandSet(MenuCommand command)
        {
            foreach (OnlineUpdateCommandSet commandSet in storeTypeCommands.Values)
            {
                foreach (MenuCommand potential in commandSet.CommonCommands)
                {
                    if (potential == command)
                    {
                        return commandSet;
                    }
                }

                // Check the instance commands
                foreach (KeyValuePair<StoreType, List<MenuCommand>> storeCommands in commandSet.InstanceCommands)
                {
                    // Enable \ disable the commands depending on an instance of this store is in the selection
                    foreach (MenuCommand potential in storeCommands.Value)
                    {
                        if (potential == command)
                        {
                            return commandSet;
                        }
                    }
                }
            }

            throw new InvalidOperationException("The given MenuCommand does not exist in any command set.");
        }

        /// <summary>
        /// Create command sets broken up by store type
        /// </summary>
        private static Dictionary<StoreTypeCode, OnlineUpdateCommandSet> CreateCommandsByStoreType(IEnumerable<long> selected)
        {
            Dictionary<StoreTypeCode, OnlineUpdateCommandSet> storeTypeCommands = new Dictionary<StoreTypeCode, OnlineUpdateCommandSet>();

            // Get all the store keys of non-manual orders
            List<long> storeKeys = selected.Select(orderID => DataProvider.GetOrderHeader(orderID)).Where(header => !header.IsManual).Select(h => h.StoreID).Distinct().ToList();

            // Filter the storekeys by the ones that still exist
            List<StoreEntity> stores = storeKeys.Select(k => StoreManager.GetStore(k)).Where(s => s != null).ToList();

            // Instances that the user is allowed to update status for
            IEnumerable<StoreEntity> updatableStores = stores.Where(s => UserSession.Security.HasPermission(PermissionType.OrdersEditStatus, s.StoreID));

            // Go through each store type present
            foreach (StoreType storeType in updatableStores.Select(s => (StoreTypeCode) s.TypeCode).Distinct().Select(c => StoreTypeManager.GetType(c)))
            {
                OnlineUpdateCommandSet commands = new OnlineUpdateCommandSet();
                commands.StoreTypeCode = storeType.TypeCode;

                // Get all the instance of this StoreType
                List<StoreType> instances = updatableStores.Where(s => s.TypeCode == (int) storeType.TypeCode).Select(s => StoreTypeManager.GetType(s)).ToList();

                // Create the list of common commands
                commands.CommonCommands = storeType.CreateOnlineUpdateCommonCommands();

                // Map store names to their instance commands
                Dictionary<StoreType, List<MenuCommand>> instanceCommands = new Dictionary<StoreType, List<MenuCommand>>();

                // Create all the instance commands
                foreach (StoreType storeInstance in instances)
                {
                    List<MenuCommand> storeCommands = storeInstance.CreateOnlineUpdateInstanceCommands();

                    if (storeCommands.Count > 0)
                    {
                        instanceCommands[storeInstance] = storeCommands;
                    }
                }

                // Set the list of instance commands for the storetype
                commands.InstanceCommands = instanceCommands;

                // Add to the map if there are any
                if (commands.CommonCommands.Count > 0 || instanceCommands.Count > 0)
                {
                    storeTypeCommands[storeType.TypeCode] = commands;
                }
            }

            return storeTypeCommands;
        }

        /// <summary>
        /// Generate the command layout from the list of available commands provided by the store types
        /// </summary>
        private List<MenuCommand> BuildCommandLayout(Dictionary<StoreTypeCode, OnlineUpdateCommandSet> storeTypeCommands)
        {
            // This doesnt actually get displayed or returned.  Its just used as a top-level container while building the commands.
            MenuCommand root = new MenuCommand("Root");

            // Indiciates if the commands from each storetype should be put under a sub-menu of that type code.
            // First of all, there has to be more than one store type that has commands.
            // Secondly, there has to be at least one set of "Common" commands.  If they are all instance commands,
            // then they'll just all be listed under their store instance name.
            bool useStoreTypeRoot = storeTypeCommands.Count > 1 && storeTypeCommands.Values.Sum(c => c.CommonCommands.Count) > 0;

            // Add in all the commands
            foreach (KeyValuePair<StoreTypeCode, OnlineUpdateCommandSet> entry in storeTypeCommands)
            {
                OnlineUpdateCommandSet commands = entry.Value;

                // Determine the parent of the commands
                MenuCommand parent;

                if (useStoreTypeRoot)
                {
                    parent = new MenuCommand(StoreTypeManager.GetType(entry.Key).StoreTypeName);
                    root.ChildCommands.Add(parent);
                }
                else
                {
                    parent = root;
                }

                // Add the common commands
                if (commands.CommonCommands.Count > 0)
                {
                    parent.ChildCommands.AddRange(commands.CommonCommands);
                    parent.ChildCommands[parent.ChildCommands.Count - 1].BreakAfter = true;
                }

                bool instanceInSubMenu = false;

                // If there is more than one store instance, the instance commands go in there own submenu
                if (commands.InstanceCommands.Count > 1)
                {
                    instanceInSubMenu = true;
                }

                // If not divided into store type, then we have to look at the total number of instance commands acccross storetypes
                if (!useStoreTypeRoot && storeTypeCommands.Values.Count(c => c.InstanceCommands.Count > 0) > 1)
                {
                    instanceInSubMenu = true;
                }

                // Add the instance commands
                foreach (KeyValuePair<StoreType, List<MenuCommand>> instanceEntry in commands.InstanceCommands)
                {
                    MenuCommand instanceRoot;

                    if (instanceInSubMenu)
                    {
                        instanceRoot = new MenuCommand(instanceEntry.Key.Store.StoreName);
                        parent.ChildCommands.Add(instanceRoot);
                    }
                    else
                    {
                        instanceRoot = parent;
                    }

                    instanceRoot.ChildCommands.AddRange(instanceEntry.Value);
                }
            }

            return root.ChildCommands;
        }

        /// <summary>
        /// Indicates if any store in ShipWorks has any online update commands
        /// </summary>
        public static bool HasOnlineUpdateCommands()
        {
            return StoreManager.GetAllStores().Any(s => HasOnlineUpdateCommands(StoreTypeManager.GetType(s)));
        }

        /// <summary>
        /// Determines if the given store type instance has any online update commands
        /// </summary>
        public static bool HasOnlineUpdateCommands(StoreType storeType)
        {
            return storeType.CreateOnlineUpdateInstanceCommands().Concat(storeType.CreateOnlineUpdateCommonCommands()).Count() > 0;
        }
    }
}
