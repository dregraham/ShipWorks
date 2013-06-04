using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Tasks;
using System.Reflection;
using System.Windows.Forms;
using ShipWorks.Stores.Platforms;
using ShipWorks.Stores;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions.Tasks.Common;
using Interapptive.Shared.Utility;

namespace ShipWorks.Actions.Tasks
{
    /// <summary>
    /// Factory to create ActionTask instances based on a unique identifier they are attributed with
    /// </summary>
    public static class ActionTaskManager
    {
        static Dictionary<string, ActionTaskDescriptor> taskDescriptors;

        class DescriptorStoreInfo
        {
            public StoreTypeCode StoreTypeCode;

            public List<ActionTaskDescriptorBinding> StoreTypeBindings = new List<ActionTaskDescriptorBinding>();
            public Dictionary<long, List<ActionTaskDescriptorBinding>> StoreInstanceBindings = new Dictionary<long, List<ActionTaskDescriptorBinding>>();
        }

        /// <summary>
        /// Static constructor
        /// </summary>
        static ActionTaskManager()
        {
            LoadDescriptors();
        }

        /// <summary>
        /// Get the descriptor that has the given identifier
        /// </summary>
        public static ActionTaskDescriptor GetDescriptor(string identifier)
        {
            ActionTaskDescriptor descriptor = null;
            if (!taskDescriptors.TryGetValue(identifier, out descriptor))
            {
                throw new NotFoundException(string.Format("Could not find descriptor for identifier '{0}'.", identifier));
            }

            return descriptor;
        }

        /// <summary>
        /// Get the descriptor that matches the given system type
        /// </summary>
        public static ActionTaskDescriptor GetDescriptor(Type type)
        {
            foreach (ActionTaskDescriptor descriptor in taskDescriptors.Values)
            {
                if (descriptor.SystemType == type)
                {
                    return descriptor;
                }
            }

            throw new NotFoundException(string.Format("Could not find descriptor for type '{0}'.", type.FullName));
        }

        /// <summary>
        /// Get the binding that goes with the given task
        /// </summary>
        public static ActionTaskDescriptorBinding GetBinding(ActionTask task)
        {
            StoreInstanceTaskBase instanceTask = task as StoreInstanceTaskBase;
            if (instanceTask != null)
            {
                return new ActionTaskDescriptorBinding(GetDescriptor(task.GetType()), instanceTask.StoreID);
            }

            StoreTypeTaskBase typeBase = task as StoreTypeTaskBase;
            if (typeBase != null)
            {
                return new ActionTaskDescriptorBinding(GetDescriptor(task.GetType()), typeBase.StoreTypeCode);
            }

            return new ActionTaskDescriptorBinding(GetDescriptor(task.GetType()));
        }

        /// <summary>
        /// Create a menu that can be used to select task types
        /// </summary>
        public static ContextMenuStrip CreateTasksMenu()
        {
            List<ActionTaskDescriptorBinding> commonBindings = new List<ActionTaskDescriptorBinding>();
            Dictionary<StoreTypeCode, DescriptorStoreInfo> storeBindingsMap = new Dictionary<StoreTypeCode, DescriptorStoreInfo>();

            // A menu item for each descriptor that has no store-specific issues
            foreach (ActionTaskDescriptor descriptor in taskDescriptors.Values.OrderBy(d => d.BaseName))
            {
                // We need to create a dummy instance of the task to figure out how to bind it
                ActionTask taskDummy = descriptor.CreateInstance();

                StoreInstanceTaskBase instanceTask = taskDummy as StoreInstanceTaskBase;
                if (instanceTask != null)
                {
                    // Go through each store to see which ones this instance task supports
                    foreach (StoreEntity store in StoreManager.GetAllStores())
                    {
                        if (instanceTask.SupportsStore(store))
                        {
                            ActionTaskDescriptorBinding binding = new ActionTaskDescriptorBinding(descriptor, store.StoreID);

                            // Get the bucket to put it in
                            DescriptorStoreInfo infoBucket;
                            if (!storeBindingsMap.TryGetValue((StoreTypeCode) store.TypeCode, out infoBucket))
                            {
                                infoBucket = new DescriptorStoreInfo();
                                infoBucket.StoreTypeCode = (StoreTypeCode) store.TypeCode;
                                storeBindingsMap[infoBucket.StoreTypeCode] = infoBucket;
                            }

                            // Get the bucket for this particular store
                            List<ActionTaskDescriptorBinding> storeBindings;
                            if (!infoBucket.StoreInstanceBindings.TryGetValue(store.StoreID, out storeBindings))
                            {
                                storeBindings = new List<ActionTaskDescriptorBinding>();
                                infoBucket.StoreInstanceBindings[store.StoreID] = storeBindings;
                            }

                            // Add this binding
                            storeBindings.Add(binding);
                        }
                    }
                }
                else
                {
                    StoreTypeTaskBase typeTask = taskDummy as StoreTypeTaskBase;
                    if (typeTask != null)
                    {
                        // Go through each store type to see what the task supports
                        foreach (StoreType storeType in StoreManager.GetUniqueStoreTypes())
                        {
                            if (typeTask.SupportsType(storeType))
                            {
                                ActionTaskDescriptorBinding binding = new ActionTaskDescriptorBinding(descriptor, storeType.TypeCode);

                                // Get the bucket to put it in
                                DescriptorStoreInfo infoBucket;
                                if (!storeBindingsMap.TryGetValue(storeType.TypeCode, out infoBucket))
                                {
                                    infoBucket = new DescriptorStoreInfo();
                                    infoBucket.StoreTypeCode = storeType.TypeCode;
                                    storeBindingsMap[infoBucket.StoreTypeCode] = infoBucket;
                                }

                                infoBucket.StoreTypeBindings.Add(binding);
                            }
                        }
                    }
                    else
                    {
                        ActionTaskDescriptorBinding binding = new ActionTaskDescriptorBinding(descriptor);
                        commonBindings.Add(binding);
                    }
                }
            }

            return CrateTasksMenu(commonBindings, storeBindingsMap);
        }

        /// <summary>
        /// Create the menu based on the given bindings data
        /// </summary>
        private static ContextMenuStrip CrateTasksMenu(List<ActionTaskDescriptorBinding> commonBindings, Dictionary<StoreTypeCode, DescriptorStoreInfo> storeBindingsMap)
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            // Add all the common bindings first
            foreach (ActionTaskDescriptorBinding binding in commonBindings)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem(binding.BaseName);
                menuItem.Tag = binding;

                contextMenu.Items.Add(menuItem);
            }

            bool addedRootInstanceSep = false;

            // Indiciates if the commands from each storetype should be put under a sub-menu of that type code.
            // First of all, there has to be more than one store type
            // Secondly, there has to be at least one set of "Common" commands.  If they are all instance commands,
            // then they'll just all be listed under their store instance name.
            bool useStoreTypeRoot = StoreManager.GetUniqueStoreTypes().Count > 1 && storeBindingsMap.Values.Sum(c => c.StoreTypeBindings.Count) > 0;

            // If there are going to be common commands such that we use the storetype root, then we'll need a sep
            if (useStoreTypeRoot)
            {
                contextMenu.Items.Add(new ToolStripSeparator());
            }

            // Add in all the commands
            foreach (KeyValuePair<StoreTypeCode, DescriptorStoreInfo> entry in storeBindingsMap)
            {
                DescriptorStoreInfo storeInfo = entry.Value;

                // Determine the parent of the commands
                ToolStripItemCollection parentItems;

                if (useStoreTypeRoot)
                {
                    ToolStripMenuItem parent = new ToolStripMenuItem(StoreTypeManager.GetType(entry.Key).StoreTypeName);
                    contextMenu.Items.Add(parent);

                    parentItems = parent.DropDownItems;
                }
                else
                {
                    parentItems = contextMenu.Items;
                }

                // Add the common commands
                foreach (ActionTaskDescriptorBinding binding in storeInfo.StoreTypeBindings.OrderBy(b => b.BaseName))
                {
                    ToolStripMenuItem menuItem = new ToolStripMenuItem(binding.BaseName);
                    menuItem.Tag = binding;

                    parentItems.Add(menuItem);
                }

                // If there is more than one store in
                bool instanceInSubMenu = false;

                // If all instance stuff is going at the root, and there is more than one store, then instance commands need to be in an instance menu
                if (!useStoreTypeRoot && storeInfo.StoreInstanceBindings.Count > 0 && StoreManager.GetAllStores().Count > 1)
                {
                    instanceInSubMenu = true;
                }

                // If we are using the store type root, then if there is more than one instance of this particular type, we need the submenu
                if (useStoreTypeRoot && storeInfo.StoreInstanceBindings.Count > 1)
                {
                    instanceInSubMenu = true;
                }

                if (useStoreTypeRoot && instanceInSubMenu)
                {
                    parentItems.Add(new ToolStripSeparator());
                }

                if (!useStoreTypeRoot && instanceInSubMenu && !addedRootInstanceSep)
                {
                    addedRootInstanceSep = true;
                    parentItems.Add(new ToolStripSeparator());
                }

                // Add the instance commands
                foreach (KeyValuePair<long, List<ActionTaskDescriptorBinding>> instanceEntry in storeInfo.StoreInstanceBindings)
                {
                    ToolStripItemCollection instanceItems;

                    if (instanceInSubMenu)
                    {
                        ToolStripMenuItem instanceRoot = new ToolStripMenuItem(StoreManager.GetStore(instanceEntry.Key).StoreName);
                        parentItems.Add(instanceRoot);

                        instanceItems = instanceRoot.DropDownItems;
                    }
                    else
                    {
                        instanceItems = parentItems;
                    }

                    // Add the bindings
                    foreach (ActionTaskDescriptorBinding binding in instanceEntry.Value.OrderBy(b => b.BaseName))
                    {
                        ToolStripMenuItem menuItem = new ToolStripMenuItem(binding.BaseName);
                        menuItem.Tag = binding;

                        instanceItems.Add(menuItem);
                    }
                }
            }

            SortMenuItems(contextMenu.Items);

            return contextMenu;
        }

        /// <summary>
        /// Sort the items, separating the sort groups based on separators
        /// </summary>
        private static void SortMenuItems(ToolStripItemCollection items)
        {
            List<List<ToolStripItem>> itemGroups = new List<List<ToolStripItem>>();
            itemGroups.Add(new List<ToolStripItem>());

            foreach (ToolStripItem item in items)
            {
                ToolStripSeparator sep = item as ToolStripSeparator;
                if (sep != null)
                {
                    itemGroups.Add(new List<ToolStripItem>());
                }
                else
                {
                    itemGroups[itemGroups.Count - 1].Add(item);
                }
            }

            items.Clear();

            foreach (List<ToolStripItem> group in itemGroups)
            {
                if (items.Count > 0)
                {
                    items.Add(new ToolStripSeparator());
                }

                items.AddRange(group.OrderBy(i => i.Text).ToArray());
            }

            foreach (ToolStripMenuItem item in items.OfType<ToolStripMenuItem>())
            {
                SortMenuItems(item.DropDownItems);
            }
        }

        /// <summary>
        /// Load all the condition element descriptors present in the assembly
        /// </summary>
        private static void LoadDescriptors()
        {
            taskDescriptors = new Dictionary<string, ActionTaskDescriptor>();

            // Look for the ConditionAttribute on each type in the assembly
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (Attribute.IsDefined(type, typeof(ActionTaskAttribute)))
                {
                    ActionTaskDescriptor descriptor = new ActionTaskDescriptor(type);

                    // Each key must be unique
                    if (taskDescriptors.ContainsKey(descriptor.Identifier))
                    {
                        throw new InvalidOperationException(string.Format("Multiple action tasks with the same identifier were found. ({0})", descriptor.Identifier));
                    }

                    // Cache the descriptor
                    taskDescriptors[descriptor.Identifier] = descriptor;
                }
            }
        }
    }
}
