using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.Collections;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Users;
using ShipWorks.Users.Security;

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

            public IEnumerable<IMenuCommand> CommonCommands;
            public Dictionary<StoreEntity, IEnumerable<IMenuCommand>> InstanceCommands;
        }

        #endregion

        // This is the command set, broken down by storetype, that is used to generate the final menu structure
        Dictionary<StoreTypeCode, OnlineUpdateCommandSet> storeTypeCommands = new Dictionary<StoreTypeCode, OnlineUpdateCommandSet>();

        // This is the actual list of commands that was generated in the order and structure that they are displayed in the ui
        List<IMenuCommand> commands = new List<IMenuCommand>();

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
        public List<IMenuCommand> CreateOnlineUpdateCommands(IEnumerable<long> selected, ILifetimeScope lifetimeScope)
        {
            storeTypeCommands = CreateCommandsByStoreType(selected, lifetimeScope);
            commands = BuildCommandLayout(storeTypeCommands);

            if (commands.None())
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
        public async Task ExecuteCommandAsync(IMenuCommand command, Control owner, IEnumerable<long> selectedKeys, MenuCommandCompleteEventHandler callback)
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
                StoreEntity store = commandSet.InstanceCommands.Where(p => p.Value.Contains(command)).Select(p => p.Key).Single();

                // Build the set of headers to process
                relevantKeys = selectedKeys.Select(orderID => DataProvider.GetOrderHeader(orderID)).Where(
                    header => header.StoreID == store.StoreID && !header.IsManual).Select(header => header.OrderID).ToList();
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
            // We have to further filter out by permission.
            if (commandSet.CommonCommands.Contains(command))
            {
                int countBefore = relevantKeys.Count;

                relevantKeys = relevantKeys.Where(orderID => UserSession.Security.HasPermission(PermissionType.OrdersEditStatus, DataProvider.GetOrderHeader(orderID).StoreID)).ToList();

                // See if any were filtered out due to permissions
                permissionWarning = countBefore != relevantKeys.Count;
            }

            // Kick off the processing
            await command.ExecuteAsync(owner, relevantKeys, (sender, e) =>
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
                }).ConfigureAwait(false);
        }

        /// <summary>
        /// Find the command set that holds the given command.  Throws an exception if not found.
        /// </summary>
        private OnlineUpdateCommandSet FindCommandSet(IMenuCommand command)
        {
            foreach (OnlineUpdateCommandSet commandSet in storeTypeCommands.Values)
            {
                foreach (IMenuCommand potential in commandSet.CommonCommands)
                {
                    if (potential == command)
                    {
                        return commandSet;
                    }
                }

                // Check the instance commands
                foreach (var storeCommands in commandSet.InstanceCommands)
                {
                    // Enable \ disable the commands depending on an instance of this store is in the selection
                    foreach (IMenuCommand potential in storeCommands.Value)
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
        private static Dictionary<StoreTypeCode, OnlineUpdateCommandSet> CreateCommandsByStoreType(IEnumerable<long> selected, ILifetimeScope lifetimeScope)
        {
            // Instances that the user is allowed to update status for
            return selected.Select(orderID => DataProvider.GetOrderHeader(orderID))
                .Where(header => !header.IsManual)
                .Select(h => h.StoreID)
                .Distinct()
                .Select(StoreManager.GetStore)
                .Where(s => s != null)
                .Where(s => UserSession.Security.HasPermission(PermissionType.OrdersEditStatus, s.StoreID))
                .GroupBy(x => x.StoreTypeCode)
                .Select(x => new { StoreType = x.Key, Commands = CreateOnlineCommandSet(lifetimeScope, x) })
                .Where(x => x.Commands.CommonCommands.Any() || x.Commands.InstanceCommands.Any())
                .ToDictionary(x => x.StoreType, x => x.Commands);
        }

        /// <summary>
        /// Create an online command set from a store grouping
        /// </summary>
        private static OnlineUpdateCommandSet CreateOnlineCommandSet(ILifetimeScope lifetimeScope, IGrouping<StoreTypeCode, StoreEntity> grouping)
        {
            IOnlineUpdateCommandCreator commandCreator = GetCommandCreator(lifetimeScope, grouping.Key);

            OnlineUpdateCommandSet commands = new OnlineUpdateCommandSet();
            commands.StoreTypeCode = grouping.Key;
            commands.CommonCommands = commandCreator.CreateOnlineUpdateCommonCommands();
            commands.InstanceCommands = grouping
                .Select(x => new { Store = x, Commands = commandCreator.CreateOnlineUpdateInstanceCommands(x) })
                .Where(x => x.Commands.Any())
                .ToDictionary(x => x.Store, x => x.Commands);
            return commands;
        }

        /// <summary>
        /// Get a command creator for the given key
        /// </summary>
        private static IOnlineUpdateCommandCreator GetCommandCreator(ILifetimeScope lifetimeScope, StoreTypeCode key)
        {
            return lifetimeScope.IsRegisteredWithKey<IOnlineUpdateCommandCreator>(key) ?
                lifetimeScope.ResolveKeyed<IOnlineUpdateCommandCreator>(key) :
                lifetimeScope.Resolve<IOnlineUpdateCommandCreator>(TypedParameter.From(key));
        }

        /// <summary>
        /// Generate the command layout from the list of available commands provided by the store types
        /// </summary>
        [NDependIgnoreLongMethod]
        private List<IMenuCommand> BuildCommandLayout(Dictionary<StoreTypeCode, OnlineUpdateCommandSet> storeSpecificCommands)
        {
            // This doesn't actually get displayed or returned.  Its just used as a top-level container while building the commands.
            MenuCommand root = new MenuCommand("Root");

            // Indicates if the commands from each storetype should be put under a sub-menu of that type code.
            // First of all, there has to be more than one store type that has commands.
            // Secondly, there has to be at least one set of "Common" commands.  If they are all instance commands,
            // then they'll just all be listed under their store instance name.
            bool useStoreTypeRoot = storeSpecificCommands.Count > 1 && storeSpecificCommands.Values.SelectMany(c => c.CommonCommands).Any();

            // Add in all the commands
            foreach (KeyValuePair<StoreTypeCode, OnlineUpdateCommandSet> entry in storeSpecificCommands)
            {
                OnlineUpdateCommandSet currentCommands = entry.Value;

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
                if (currentCommands.CommonCommands.Any())
                {
                    parent.ChildCommands.AddRange(currentCommands.CommonCommands);
                    parent.ChildCommands[parent.ChildCommands.Count - 1].BreakAfter = true;
                }

                bool instanceInSubMenu = false;

                // If there is more than one store instance, the instance commands go in there own submenu
                if (currentCommands.InstanceCommands.Count > 1)
                {
                    instanceInSubMenu = true;
                }

                // If not divided into store type, then we have to look at the total number of instance commands across storetypes
                if (!useStoreTypeRoot && storeSpecificCommands.Values.Count(c => c.InstanceCommands.Count > 0) > 1)
                {
                    instanceInSubMenu = true;
                }

                // Add the instance commands
                foreach (var instanceEntry in currentCommands.InstanceCommands)
                {
                    MenuCommand instanceRoot;

                    if (instanceInSubMenu)
                    {
                        instanceRoot = new MenuCommand(instanceEntry.Key.StoreName);
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
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                return StoreManager.GetAllStores().Any(s => HasOnlineUpdateCommands(lifetimeScope, s));
            }
        }

        /// <summary>
        /// Determines if the given store type instance has any online update commands
        /// </summary>
        public static bool HasOnlineUpdateCommands(ILifetimeScope lifetimeScope, StoreEntity store)
        {
            var commandCreator = GetCommandCreator(lifetimeScope, store.StoreTypeCode);
            return commandCreator.CreateOnlineUpdateCommonCommands().Any() ||
                commandCreator.CreateOnlineUpdateInstanceCommands(store).Any();
        }
    }
}
