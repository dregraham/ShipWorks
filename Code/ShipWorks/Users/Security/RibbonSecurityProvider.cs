using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Divelements.SandRibbon;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Filters;
using RibbonShortcut = Divelements.SandRibbon.Shortcut;

namespace ShipWorks.Users.Security
{
    /// <summary>
    /// Provides security to the ribbon through the ExtenderProvider model
    /// </summary>
    [ProvideProperty("Permission", typeof(WidgetBase))]
    public partial class RibbonSecurityProvider : Component, IExtenderProvider
    {
        private ILog log = LogManager.GetLogger(typeof(RibbonSecurityProvider));
        private Dictionary<WidgetBase, PermissionType> permissionMap = new Dictionary<WidgetBase, PermissionType>();
        private Dictionary<WidgetBase, Func<bool>> additionalConditions = new Dictionary<WidgetBase, Func<bool>>();
        private Ribbon ribbon;
        private MainGridControl gridControl;

        /// <summary>
        /// Constructor
        /// </summary>
        public RibbonSecurityProvider()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RibbonSecurityProvider(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// Indicates if this extender applies to the object passed in
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool CanExtend(object extendee)
        {
            return extendee is WidgetBase;
        }

        /// <summary>
        /// Necessary piece of the extender provider plumbing
        /// </summary>
        [Category("ShipWorks")]
        [DefaultValue(PermissionType.AlwaysGrant)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public PermissionType GetPermission(WidgetBase widget)
        {
            PermissionType permission;
            if (!permissionMap.TryGetValue(widget, out permission))
            {
                return PermissionType.AlwaysGrant;
            }

            return permission;
        }

        /// <summary>
        /// Necessary piece of the extender provider plumbing
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetPermission(WidgetBase widget, PermissionType permission)
        {
            if (permission == PermissionType.AlwaysGrant)
            {
                permissionMap.Remove(widget);
            }
            else
            {
                permissionMap[widget] = permission;
            }
        }

        /// <summary>
        /// The Ribbon on the MainForm, set in the designer
        /// </summary>
        [DefaultValue(null)]
        public Ribbon Ribbon
        {
            get { return ribbon; }
            set { ribbon = value; }
        }

        /// <summary>
        /// The MainGridControl from the main form, set in the designer.  Needed for access to the current selection.
        /// </summary>
        public MainGridControl MainGridControl
        {
            get { return gridControl; }
            set { gridControl = value; }
        }

        /// <summary>
        /// Add an additional condition that a widget has to pass to be visible
        /// </summary>
        public void AddAdditionalCondition(WidgetBase widget, Func<bool> condition)
        {
            if (!permissionMap.ContainsKey(widget))
            {
                throw new InvalidOperationException("Cannot add additional condition for widget that isn't managed in the first place.");
            }

            additionalConditions.Add(widget, condition);
        }

        /// <summary>
        /// Update security information that can change when store's change
        /// </summary>
        public void UpdateSecurityUI()
        {
            foreach (KeyValuePair<WidgetBase, PermissionType> pair in permissionMap)
            {
                WidgetBase widget = pair.Key;
                PermissionType permission = pair.Value;

                bool permitted = CheckAdditionalCondition(widget);

                // No need to do further checks if the first condition wasn't ok
                if (permitted)
                {
                    PermissionScope scope = PermissionHelper.GetScope(permission);

                    // If scope is Entity dependeant, do a translation.  The SecurityContext does do this translation too...
                    // except we can't take advantage of it, since we may not have a selection for it to determine with.  But
                    // we can always determine since we have the current FilterTarget.
                    if (scope == PermissionScope.IndirectEntityType)
                    {
                        permission = PermissionHelper.GetIndirectEntityActualPermission(permission, FilterHelper.GetEntityType(gridControl.ActiveFilterTarget));
                        scope = PermissionHelper.GetScope(permission);
                    }

                    switch (scope)
                    {
                        case PermissionScope.Global:
                            {
                                permitted = UserSession.Security.HasPermission(permission);
                                break;
                            }

                        case PermissionScope.Store:
                            {
                                // See how many stores the user is permitted to do this for
                                StorePermissionCoverage coverage = UserSession.Security.GetStorePermissionCoverage(permission);

                                if (coverage == StorePermissionCoverage.All)
                                {
                                    permitted = true;
                                }

                                if (coverage == StorePermissionCoverage.None)
                                {
                                    permitted = false;
                                }

                                if (coverage == StorePermissionCoverage.Some)
                                {
                                    // The only way selection matters is if it's orders... if its not orders, then it can't be locked down per store
                                    if (gridControl.Selection.Count > 0 && gridControl.ActiveFilterTarget == FilterTarget.Orders)
                                    {
                                        // Permitted if you can do it for any of the selected stores
                                        permitted = gridControl.SelectedStoreKeys.Any(k => UserSession.Security.HasPermission(permission, k));
                                    }
                                    else
                                    {
                                        // Order's arent selected - still show it, but the UI will probably have it disabled
                                        permitted = true;
                                    }
                                }

                                break;
                            }

                        default:
                            throw new InvalidOperationException("Unhandled PermissionScope");

                    }
                }

                widget.Visible = permitted;
            }

            // Some tabs\chunks etc may not be needed anymore, or may now be needed
            AdjustRibbonVisibility();
        }

        /// <summary>
        /// If there is an additional condition for visibility of the widget see what the result is
        /// </summary>
        private bool CheckAdditionalCondition(WidgetBase widget)
        {
            Func<bool> condition;
            if (additionalConditions.TryGetValue(widget, out condition))
            {
                return condition();
            }

            return true;
        }

        /// <summary>
        /// Adjust the ribbon for its current state of secured visibility.  So like if two buttons that are in a StripLayout arent visible anymore,
        /// the StripLayout get's hidden.
        /// </summary>
        private void AdjustRibbonVisibility()
        {
            foreach (RibbonTab tab in ribbon.Tabs)
            {
                AdjustRibbonContainerVisibility(tab.Chunks);
            }

            Dictionary<WidgetBase, bool> visibleWidgets = permissionMap.ToDictionary(p => p.Key, p => p.Key.Visible);

            // Go through and hide the QAT shortcuts. ToolBar can be not present.
            if (ribbon.ToolBar != null)
            {
                foreach (WidgetBase item in ribbon.ToolBar.Items)
                {
                    RibbonShortcut shortcut = item as RibbonShortcut;
                    if (shortcut != null)
                    {
                        shortcut.Visible = !visibleWidgets.ContainsKey(shortcut.Target) || visibleWidgets[shortcut.Target];
                    }
                }
            }

            // Hide any tabs that have no more visible chunks
            foreach (RibbonTab tab in ribbon.Tabs)
            {
                if (tab.Chunks.Count > 0 && !tab.Chunks.Cast<RibbonChunk>().Any(c => c.Visible))
                {
                    // We used to hide Tabs that were empty - but this code was causing Tabs to be improperly hidden forever
                    // since for some reason SandRibbon reports Tab.Chunks as empty when the Ribbon is minimized, and the tab is popped open.
                    // Further, there should no longer actually ever be a case where a tab would be completely empty.  Even users with no rights
                    // still have some 'stuff' on each tab.  This exception is to catch the case for when\if that ever changes.  If it does,
                    // then this will have to be updated to handle proper add/remove of tabs as necessary.  Review the code prior to 10/10/2011 to see how
                    // it used to (buggily) work, and update from there.
                    log.DebugFormat("A tab ({0}) has no visible chunks.", tab.Name);
                }
            }
        }

        /// <summary>
        /// Apply visibility recursively to all widgets based on the visibility of the given widgets
        /// </summary>
        private void AdjustRibbonContainerVisibility(WidgetCollection widgets)
        {
            foreach (WidgetBase widget in widgets)
            {
                // If its a parent, we have to look at the children
                ParentWidgetBase widgetContainer = widget as ParentWidgetBase;
                if (widgetContainer != null)
                {
                    AdjustRibbonContainerVisibility(widgetContainer.Items);

                    // Container will only be visible if all children are
                    widget.Visible = widgetContainer.Items.Cast<WidgetBase>().Any(w => w.Visible);
                }
            }
        }
    }
}
