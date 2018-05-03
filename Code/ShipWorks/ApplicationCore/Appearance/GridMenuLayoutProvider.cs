using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using log4net;
using ShipWorks.Users.Security;
using ShipWorks.Stores.Platforms;
using ShipWorks.Stores;
using ShipWorks.Users;
using ShipWorks.Filters;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model;

namespace ShipWorks.ApplicationCore.Appearance
{
    /// <summary>
    /// Component that provides menu layout functionality.
    /// </summary>
    [ProvideProperty("LayoutGuid", typeof(ToolStripItem))]
    [ProvideProperty("Permission", typeof(ToolStripItem))]
    public sealed partial class GridMenuLayoutProvider : Component, IExtenderProvider
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(GridMenuLayoutProvider));

        // The context menus
        ContextMenuStrip orderMenu;
        ContextMenuStrip customerMenu;

        // All context items in there original order for each store
        List<ToolStripItem> originalOrderItems;
        List<ToolStripItem> originalCustomerItems;

        // All the items mapped to their original visibility.
        Dictionary<ToolStripItem, bool> originalVisibility = new Dictionary<ToolStripItem, bool>();
        
        // All the items mapped to the visibility defined by the user
        Dictionary<ToolStripItem, bool> userVisibility = new Dictionary<ToolStripItem, bool>();

        // The guids for each item
        Dictionary<ToolStripItem, Guid> itemGuids = new Dictionary<ToolStripItem, Guid>();
        Dictionary<ToolStripItem, PermissionType> permissions = new Dictionary<ToolStripItem, PermissionType>();

        // Known guids
        static Guid updateOnlineGuid = new Guid("16ff9a2f-0a14-4efc-acac-f872844d18e7");
        static Guid ordersCustomActionsGuid = new Guid("3263d695-fa30-4738-81b5-1dc3bb18d82c");
        static Guid customersCustomActionsGuid = new Guid("3263d695-fa30-4738-81b5-1dc3bb18d82d");
        static Guid customersCustomActionsSeparatorGuid = new Guid("c145fc99-b215-47a8-b496-25d0d7a2dc27");

        // The main grid control, so we know what is selected
        MainGridControl gridControl;

        /// <summary>
        /// Constructor
        /// </summary>
        public GridMenuLayoutProvider()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GridMenuLayoutProvider(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        [DefaultValue(null)]
        [Category("Behavior")]
        [Description("The context menu for the order grid.")]
        public ContextMenuStrip OrderGridMenu
        {
            get { return orderMenu; }
            set { orderMenu = value; }
        }

        [DefaultValue(null)]
        [Category("Behavior")]
        [Description("The context menu for the customer grid.")]
        public ContextMenuStrip CustomerGridMenu
        {
            get { return customerMenu; }
            set { customerMenu = value; }
        }

        /// <summary>
        /// One-time initialization
        /// </summary>
        public void Initialize(MainGridControl gridControl)
        {
            this.gridControl = gridControl;

            originalOrderItems = orderMenu.Items.Cast<ToolStripItem>().ToList();
            originalCustomerItems = customerMenu.Items.Cast<ToolStripItem>().ToList();

            // Grab the visibility of the items
            foreach (ToolStripItem item in 
                originalOrderItems.Concat(
                originalCustomerItems))
            {
                originalVisibility[item] = item.Available;
                userVisibility[item] = item.Available;
            }

            orderMenu.Opening += new CancelEventHandler(OnContextMenuOpening);
            customerMenu.Opening += new CancelEventHandler(OnContextMenuOpening);
        }

        /// <summary>
        /// Update item availability based on the current store set
        /// </summary>
        public void UpdateStoreDependentUI()
        {
            // Apply security-based availability
            UpdateStoreDependentUI(orderMenu, originalOrderItems);
            UpdateStoreDependentUI(customerMenu, originalCustomerItems);

            ToolStripItem onlineUpdateItem = originalOrderItems.Single(i => GetLayoutGuid(i) == updateOnlineGuid);

            // Apply availability to the OnlineUpdateCommand
            ApplyExistance(
                onlineUpdateItem,
                orderMenu, 
                originalOrderItems,
                (OnlineUpdateCommandProvider.HasOnlineUpdateCommands() && CheckPermission(onlineUpdateItem, false)));

            // Make sure what we did above doesn't overwrite what actions need it to be
            UpdateUserInitiatedActionDependentUI();
        }

        /// <summary>
        /// Update UI that depends on custom user actions
        /// </summary>
        public void UpdateUserInitiatedActionDependentUI()
        {
            ToolStripItem orderActionsItem = originalOrderItems.Single(i => GetLayoutGuid(i) == ordersCustomActionsGuid);
            ToolStripItem customerActionsItem = originalCustomerItems.Single(i => GetLayoutGuid(i) == customersCustomActionsGuid);
            ToolStripItem customerActionsSepItem = originalCustomerItems.Single(i => GetLayoutGuid(i) == customersCustomActionsSeparatorGuid);

            // Apply existance to each of them
            ApplyExistance(orderActionsItem, orderMenu, originalOrderItems, ((ToolStripMenuItem) orderActionsItem).HasDropDownItems);
            ApplyExistance(customerActionsItem, customerMenu, originalCustomerItems, ((ToolStripMenuItem) customerActionsItem).HasDropDownItems);
            ApplyExistance(customerActionsSepItem, customerMenu, originalCustomerItems, ((ToolStripMenuItem) customerActionsItem).HasDropDownItems);
        }

        /// <summary>
        /// Update the store dependent UI for the given menu and its original untampered set of items
        /// </summary>
        private void UpdateStoreDependentUI(ContextMenuStrip menu, List<ToolStripItem> allItems)
        {
            // Go through every item and apply its availability
            foreach (ToolStripItem item in allItems)
            {
                bool available = CheckPermission(item, false);

                ApplyExistance(item, menu, allItems, available);
            }
        }

        /// <summary>
        /// Ensure the given item is available in the menu's items or not
        /// </summary>
        private void ApplyExistance(ToolStripItem item, ContextMenuStrip menu, List<ToolStripItem> allItems, bool available)
        {
            bool wasAvailable = menu.Items.Contains(item);

            if (!available && wasAvailable)
            {
                menu.Items.Remove(item);
            }

            if (available && !wasAvailable)
            {
                AddMissingStandardItems(menu, new List<ToolStripItem> { item }, allItems);
            }
        }

        /// <summary>
        /// One of the menu's is opening
        /// </summary>
        void OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = (ContextMenuStrip) sender;

            // Go through each item and apply its availability based on user selection and security
            foreach (ToolStripItem item in menu.Items)
            {
                item.Available = userVisibility[item] && CheckPermission(item, true);
            }

            List<ToolStripItem> availableItems = menu.Items.OfType<ToolStripItem>().Where(i => i.Available).ToList();

            // If the first item is a sep, that's stupid
            if (availableItems[0] is ToolStripSeparator)
            {
                availableItems[0].Available = false;
                availableItems.RemoveAt(0);
            }

            int lastIndex = availableItems.Count - 1;

            // If the last item is a sep, that's stupid
            if (availableItems[lastIndex] is ToolStripSeparator)
            {
                availableItems[lastIndex].Available = false;
                availableItems.RemoveAt(lastIndex);
            }

            bool previousSep = false;

            // Check for double-separators
            foreach (ToolStripItem item in availableItems)
            {
                if (item is ToolStripSeparator)
                {
                    if (previousSep)
                    {
                        item.Available = false;
                    }

                    previousSep = true;
                }
                else
                {
                    previousSep = false;
                }
            }
        }

        /// <summary>
        /// See if the currently logged in user has permission to view the given item
        /// </summary>
        private bool CheckPermission(ToolStripItem item, bool selectedStoreOnly)
        {
            PermissionType permissionType = GetPermission(item);
            PermissionScope scope = PermissionHelper.GetScope(permissionType);

            switch (scope)
            {
                case PermissionScope.Global:
                    {
                        return UserSession.Security.HasPermission(permissionType);
                    }

                case PermissionScope.Store:
                    {
                        StorePermissionCoverage coverage = UserSession.Security.GetStorePermissionCoverage(permissionType);

                        // Shortcut before we iterate each one
                        if (coverage == StorePermissionCoverage.All)
                        {
                            return true;
                        }
                        else if (coverage == StorePermissionCoverage.None)
                        {
                            return false;
                        }

                        List<long> storeKeys = selectedStoreOnly ? gridControl.SelectedStoreKeys : StoreManager.GetAllStores().Select(s => s.StoreID).ToList();

                        return storeKeys.Any(k => UserSession.Security.HasPermission(permissionType, k));
                    }

                default:
                    throw new InvalidOperationException("Unhandled PermissionScope");
            }
        }

        /// <summary>
        /// Necessary piece of the extender provider plumbing
        /// </summary>
        [Category("ShipWorks")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Guid GetLayoutGuid(ToolStripItem item)
        {
            Guid guid;

            if (!itemGuids.TryGetValue(item, out guid))
            {
                guid = Guid.NewGuid();
                itemGuids[item] = guid;
            }

            return guid;
        }

        /// <summary>
        /// Necessary piece of the extender provider plumbing
        /// </summary>
        [Category("ShipWorks")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetLayoutGuid(ToolStripItem item, Guid value)
        {
            itemGuids[item] = value;
        }

        /// <summary>
        /// Necessary piece of the extender provider plumbing
        /// </summary>
        [Category("ShipWorks")]
        [DefaultValue(PermissionType.AlwaysGrant)]
        public PermissionType GetPermission(ToolStripItem item)
        {
            PermissionType permission;

            if (!permissions.TryGetValue(item, out permission))
            {
                permission = PermissionType.AlwaysGrant;
                permissions[item] = permission;
            }

            return permission;
        }

        /// <summary>
        /// Necessary piece of the extender provider plumbing
        /// </summary>
        [Category("ShipWorks")]
        public void SetPermission(ToolStripItem item, PermissionType value)
        {
            permissions[item] = value;
        }

        /// <summary>
        /// Indicates if the ToolStripItem should be displayed
        /// </summary>
        public bool GetItemVisibility(ToolStripItem item)
        {
            return userVisibility[item];
        }

        /// <summary>
        /// Sets if the ToolStripItem should be displayed
        /// </summary>
        public void SetItemVisibility(ToolStripItem item, bool visible)
        {
            userVisibility[item] = visible;
        }

        /// <summary>
        /// Load the default context menu settings
        /// </summary>
        public void LoadDefault()
        {
            LoadLayout(null);
        }

        /// <summary>
        /// Load the settings of the context menu based on the given layout.
        /// </summary>
        public void LoadLayout(string xmlLayout)
        {
            // Reset to original
            ResetLayout(orderMenu, originalOrderItems);
            ResetLayout(customerMenu, originalCustomerItems);

            // Nothing to do if empty
            if (!string.IsNullOrEmpty(xmlLayout))
            {
                // Load the XDocument
                XDocument xDocument = XDocument.Parse(xmlLayout);

                XElement ordersMenuLayout = xDocument.Descendants("MenuLayout").Where(e => (string) e.Attribute("type") == "Orders").Single();
                XElement customerMenuLayout = xDocument.Descendants("MenuLayout").Where(e => (string) e.Attribute("type") == "Customers").Single();

                // Load the layouts
                LoadMenuLayoutElement(ordersMenuLayout, orderMenu, originalOrderItems);
                LoadMenuLayoutElement(customerMenuLayout, customerMenu, originalCustomerItems);
            }

            // We have to make sure the store-level security is still up-to-date.  All of the above code makes sure every MenuItem has a place in the
            // menu... we need to call this to have the secured items removed.
            UpdateStoreDependentUI();
        }

        /// <summary>
        /// Load the meny layout from the given element
        /// </summary>
        private void LoadMenuLayoutElement(XElement layoutElement, ContextMenuStrip menu, List<ToolStripItem> originalItems)
        {
            // Clear the current menu items
            menu.Items.Clear();

            // Keep track of what we have not yet used
            List<ToolStripItem> unusedItems = originalItems.ToList();

            // Go through each element, in order, to build the menu.
            foreach (var layoutInfo in layoutElement.Elements().Select(e => 
                new 
                { 
                    ID = (Guid) e.Attribute("id"), 
                    Visible = (bool) e.Attribute("visible") 
                } ))
            {
                // Find the item with this guid (or null if it does not exist)
                ToolStripItem item = originalItems.SingleOrDefault(i => GetLayoutGuid(i) == layoutInfo.ID);
                if (item != null)
                {
                    SetItemVisibility(item, layoutInfo.Visible);

                    menu.Items.Add(item);
                    unusedItems.Remove(item);
                }
                else
                {
                    log.InfoFormat("MenuItem {0} not found, and dropped from layout.", layoutInfo.ID);
                }
            }

            // Now we have to account for unused items, which are those items which may have been newly added to ShipWorks
            AddMissingStandardItems(menu, unusedItems, originalItems);
        }

        /// <summary>
        /// Add the given unused items, that are not store-specific, to the menu.
        /// </summary>
        private void AddMissingStandardItems(ContextMenuStrip menu, List<ToolStripItem> unusedItems, List<ToolStripItem> originalItems)
        {
            foreach (ToolStripItem unusedItem in unusedItems)
            {
                int menuPosition;

                // Items at the top stay at top
                if (originalItems.IndexOf(unusedItem) == 0 || menu.Items.Count == 0)
                {
                    menuPosition = 0;
                }
                else
                {
                    // The item that the unused item will be added before or after
                    ToolStripItem anchorItem = FindAnchorItem(unusedItem, originalItems, menu);

                    // Find the position of the anchor item in the menu
                    menuPosition = menu.Items.IndexOf(anchorItem);

                    // If our anchor is above us, we want to insert below it
                    if (originalItems.IndexOf(anchorItem) < originalItems.IndexOf(unusedItem))
                    {
                        menuPosition++;
                    }
                }

                menu.Items.Insert(menuPosition, unusedItem);
            }
        }

        /// <summary>
        /// Find the item, that exists in the menu, that can be used as an anchor point for inserting a missing menu item.
        /// </summary>
        private ToolStripItem FindAnchorItem(ToolStripItem unusedItem, List<ToolStripItem> originalItems, ContextMenuStrip menu)
        {
            ToolStripItem anchorItem = FindAnchorItem(unusedItem, originalItems, menu, -1);

            if (anchorItem == null)
            {
                anchorItem = FindAnchorItem(unusedItem, originalItems, menu, +1);
            }

            if (anchorItem == null)
            {
                throw new InvalidOperationException("An anchor item could not be found.  This should be impossible.");
            }

            return anchorItem;
        }

        /// <summary>
        /// Find the item, that exists in the menu, that can be used as an anchor point for inserting a missing menu item, by searching the
        /// original items in the given direction for an item that exists in the menu.
        /// </summary>
        private ToolStripItem FindAnchorItem(ToolStripItem item, List<ToolStripItem> originalItems, ContextMenuStrip menu, int direction)
        {
            int nextIndex = originalItems.IndexOf(item) + direction;

            // We passed the bounds
            if (nextIndex < 0 || nextIndex >= originalItems.Count)
            {
                return null;
            }

            ToolStripItem anchorItem = originalItems[nextIndex];

            // Found one that is in the menu
            if (menu.Items.Contains(anchorItem))
            {
                return anchorItem;
            }
            // Keep searching
            else
            {
                return FindAnchorItem(anchorItem, originalItems, menu, direction);
            }
        }

        /// <summary>
        /// Gets the layout as an XML string
        /// </summary>
        public string SerializeLayout()
        {
            XDocument xDocument = new XDocument(
                new XElement("ShipWorks",
                    GetMenuLayoutElement(orderMenu, "Orders"),
                    GetMenuLayoutElement(customerMenu, "Customers")
                )
            );

            return xDocument.ToString();
        }

        /// <summary>
        /// Get the XElement reprsenting the given menu items
        /// </summary>
        private XElement GetMenuLayoutElement(ContextMenuStrip menu, string type)
        {
            return new XElement("MenuLayout",
                new XAttribute("type", type),
                menu.Items.Cast<ToolStripItem>()
                    .Select(i => new XElement("MenuItem",
                                    new XAttribute("id", GetLayoutGuid(i)),
                                    new XAttribute("visible", GetItemVisibility(i))
                                 )
                            )
                   );
        }

        /// <summary>
        /// Reset the layout for the given menu with the specified items
        /// </summary>
        private void ResetLayout(ContextMenuStrip contextMenu, List<ToolStripItem> items)
        {
            contextMenu.Items.Clear();
            contextMenu.Items.AddRange(items.ToArray());

            // Apply original visibility
            foreach (ToolStripItem item in contextMenu.Items)
            {
                SetItemVisibility(item, originalVisibility[item]);
            }
        }

        /// <summary>
        /// Indicates if this extender applies to the object passed in
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool CanExtend(object extendee)
        {
            ToolStripItem item = extendee as ToolStripItem;
            if (item == null)
            {
                return false;
            }

            if (customerMenu != null && customerMenu.Items.Contains(item))
            {
                return true;
            }

            if (orderMenu != null && orderMenu.Items.Contains(item))
            {
                return true;
            }

            return false;
        }
    }
}
