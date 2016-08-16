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
using SandContextPopup = Divelements.SandRibbon.ContextPopup;
using SandMenuItem = Divelements.SandRibbon.MenuItem;
using SandMenu = Divelements.SandRibbon.Menu;
using SandHeading = Divelements.SandRibbon.PopupHeading;
using System.Drawing;
using Interapptive.Shared;

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
        [NDependIgnoreLongMethod]
        public static SandContextPopup CreateTasksMenu()
        {
            List<ActionTaskDescriptorBinding> commonBindings = new List<ActionTaskDescriptorBinding>();
            Dictionary<StoreTypeCode, DescriptorStoreInfo> storeBindingsMap = new Dictionary<StoreTypeCode, DescriptorStoreInfo>();

            // A menu item for each descriptor that has no store-specific issues
            foreach (ActionTaskDescriptor descriptor in taskDescriptors.Values.Where(atd => !atd.Hidden).OrderBy(d => d.BaseName))
            {
                // We need to create a dummy instance of the task to figure out how to bind it
                ActionTask taskDummy = descriptor.CreateInstance();

                StoreInstanceTaskBase instanceTask = taskDummy as StoreInstanceTaskBase;
                if (instanceTask != null)
                {
                    // Go through each store to see which ones this instance task supports
                    foreach (StoreEntity store in StoreManager.GetEnabledStores())
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
                        foreach (StoreType storeType in StoreManager.GetUniqueStoreTypes(true))
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
        private static SandContextPopup CrateTasksMenu(List<ActionTaskDescriptorBinding> commonBindings, Dictionary<StoreTypeCode, DescriptorStoreInfo> storeBindingsMap)
        {
            SandContextPopup contextMenu = new SandContextPopup();
            contextMenu.Font = new System.Drawing.Font("Tahoma", 8.25f);

            // Add all the common bindings first, ordered by the numeric value of the category
            foreach (var categoryGroup in commonBindings.GroupBy(b => b.Category).OrderBy(g => (int) g.Key))
            {
                // Category name goes as the heading
                SandHeading heading = new SandHeading() { Text = EnumHelper.GetDescription(categoryGroup.Key) };
                contextMenu.Items.Add(heading);

                SandMenu menu = new SandMenu();
                contextMenu.Items.Add(menu);

                // Add all the items in the category, sorted by name
                foreach (ActionTaskDescriptorBinding binding in categoryGroup.OrderBy(b => b.FullName))
                {
                    SandMenuItem menuItem = new SandMenuItem(binding.BaseName);
                    menuItem.Tag = binding;

                    menu.Items.Add(menuItem);
                }

                // Figure out where to insert Update Online
                if ((int) ActionTaskCategory.UpdateOnline == (int) categoryGroup.Key + 1)
                {
                    AddUpdateOnlineTasksMenu(contextMenu, storeBindingsMap);
                }
            }

            return contextMenu;
        }

        /// <summary>
        /// Add the 'Update Online' section of the tasks menu
        /// </summary>
        [NDependIgnoreLongMethod]
        private static void AddUpdateOnlineTasksMenu(SandContextPopup contextMenu, Dictionary<StoreTypeCode, DescriptorStoreInfo> storeBindingsMap)
        {
            // Indiciates if the commands from each storetype should be put under a sub-menu of that type code.
            // First of all, there has to be more than one store type
            // Secondly, there has to be at least one set of "Common" commands.  If they are all instance commands,
            // then they'll just all be listed under their store instance name.
            bool useStoreTypeRoot = StoreManager.GetUniqueStoreTypes(true).Count > 1 && storeBindingsMap.Values.Sum(c => c.StoreTypeBindings.Count) > 0;

            // Menu that will contain all our stores
            SandMenu storeMenu = new SandMenu();

            // Add in all the commands
            foreach (KeyValuePair<StoreTypeCode, DescriptorStoreInfo> entry in storeBindingsMap)
            {
                DescriptorStoreInfo storeInfo = entry.Value;

                // Determine the parent of the commands
                SandMenu parentMenu;

                if (useStoreTypeRoot)
                {
                    SandMenuItem typeRootItem = new SandMenuItem(StoreTypeManager.GetType(entry.Key).StoreTypeName);
                    storeMenu.Items.Add(typeRootItem);

                    parentMenu = new SandMenu();
                    typeRootItem.Items.Add(parentMenu);
                }
                else
                {
                    parentMenu = storeMenu;
                }

                // Add the common commands
                foreach (ActionTaskDescriptorBinding binding in storeInfo.StoreTypeBindings.OrderBy(b => b.BaseName))
                {
                    SandMenuItem menuItem = new SandMenuItem(binding.BaseName);
                    menuItem.Tag = binding;

                    parentMenu.Items.Add(menuItem);
                }

                // If there is more than one store in
                bool instanceInSubMenu = false;

                // If all instance stuff is going at the root, and there is more than one store, then instance commands need to be in an instance menu
                if (!useStoreTypeRoot && storeInfo.StoreInstanceBindings.Count > 0 && StoreManager.GetEnabledStores().Count > 1)
                {
                    instanceInSubMenu = true;
                }

                // If we are using the store type root, then if there is more than one instance of this particular type, we need the submenu
                if (useStoreTypeRoot && storeInfo.StoreInstanceBindings.Count > 1)
                {
                    instanceInSubMenu = true;
                }

                // Add the instance commands
                foreach (KeyValuePair<long, List<ActionTaskDescriptorBinding>> instanceEntry in storeInfo.StoreInstanceBindings)
                {
                    SandMenu instanceParent;

                    if (instanceInSubMenu)
                    {
                        SandMenuItem instanceRootItem = new SandMenuItem(StoreManager.GetStore(instanceEntry.Key).StoreName);
                        parentMenu.Items.Add(instanceRootItem);

                        // If there are common commands, and instance commands are in a submenu, add a separator
                        if (useStoreTypeRoot)
                        {
                            instanceRootItem.GroupName = "Group";
                        }

                        instanceParent = new SandMenu();
                        instanceRootItem.Items.Add(instanceParent);
                    }
                    else
                    {
                        instanceParent = parentMenu;
                    }

                    // Add the bindings
                    foreach (ActionTaskDescriptorBinding binding in instanceEntry.Value.OrderBy(b => b.BaseName))
                    {
                        SandMenuItem menuItem = new SandMenuItem(binding.BaseName);
                        menuItem.Tag = binding;

                        instanceParent.Items.Add(menuItem);
                    }
                }
            }

            // If the store menu actually has something, add it in
            if (storeMenu.Items.Count > 0)
            {
                SandHeading heading = new SandHeading() { Text = EnumHelper.GetDescription(ActionTaskCategory.UpdateOnline) };
                contextMenu.Items.Add(heading);

                contextMenu.Items.Add(storeMenu);
            }
        }

        /// <summary>
        /// Load all the condition element descriptors present in the assembly
        /// </summary>
        private static void LoadDescriptors()
        {
            taskDescriptors = new Dictionary<string, ActionTaskDescriptor>();

            IEnumerable<Type> allTypes = Assembly.Load("ShipWorks.Stores").GetTypes().Union(Assembly.GetExecutingAssembly().GetTypes());
            
            // Look for the ConditionAttribute on each type in the assembly
            foreach (Type type in allTypes)
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
