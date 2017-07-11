using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using ShipWorks.Data.Grid;
using ShipWorks.Filters;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// Provides design-time declarative UI state management for menu and buttons
    /// </summary>
    [ProvideProperty("EnabledWhen", typeof(Component))]
    public partial class SelectionDependentEnabler : Component, IExtenderProvider
    {
        Dictionary<Component, SelectionDependentEntry> componentMap = new Dictionary<Component, SelectionDependentEntry>();

        /// <summary>
        /// Constructor
        /// </summary>
        public SelectionDependentEnabler()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SelectionDependentEnabler(IContainer container)
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
            ToolStripItem item = extendee as ToolStripItem;
            if (item != null)
            {
                return true;
            }

            Divelements.SandRibbon.Button button = extendee as Divelements.SandRibbon.Button;
            if (button != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Necessary piece of the extender provider plumbing
        /// </summary>
        [Category("ShipWorks")]
        [DefaultValue(SelectionDependentType.Ignore)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SelectionDependentType GetEnabledWhen(Component component)
        {
            SelectionDependentEntry selectionDependentEntry;
            if (!componentMap.TryGetValue(component, out selectionDependentEntry))
            {
                return SelectionDependentType.Ignore;
            }

            return selectionDependentEntry.SelectionDependentType;
        }

        /// <summary>
        /// Necessary piece of the extender provider plumbing
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetEnabledWhen(Component component, SelectionDependentType state, Func<IEnumerable<long>, bool> applies = null)
        {
            if (!CanExtend(component))
            {
                throw new ArgumentException("The object type is not supported by the ExtenderProvider.", "component");
            }

            if (state == SelectionDependentType.Ignore)
            {
                componentMap.Remove(component);

                // Now that we are ignoring it, make sure it's enabled
                EnableComponent(component, true);
            }
            else
            {
                componentMap[component] = new SelectionDependentEntry()
                {
                    SelectionDependentType = state,
                    Applies = applies
                };
            }
        }

        /// <summary>
        /// Update the command state based on the selection state of the given grid control
        /// </summary>
        public void UpdateCommandState(IGridSelection selection, FilterTarget target)
        {
            bool oneOrder = target == FilterTarget.Orders && selection.Count == 1;
            bool oneOrMoreOrders = target == FilterTarget.Orders && selection.Count > 0;
            bool twoOrMoreOrders = target == FilterTarget.Orders && selection.Count > 1;

            bool oneCustomer = target == FilterTarget.Customers && selection.Count == 1;
            bool oneOrMoreCustomers = target == FilterTarget.Customers && selection.Count > 0;
            bool twoOrMoreCustomers = target == FilterTarget.Customers && selection.Count > 1;

            // Go through each registered component
            foreach (KeyValuePair<Component, SelectionDependentEntry> entry in componentMap)
            {
                Component component = entry.Key;
                SelectionDependentType selectionDependentType = entry.Value.SelectionDependentType;

                // Skip any that are being updated manually
                if (entry.Value.SelectionDependentType == SelectionDependentType.Ignore)
                {
                    continue;
                }

                switch (selectionDependentType)
                {
                    case SelectionDependentType.OneOrder:
                        EnableComponent(component, oneOrder);
                        break;

                    case SelectionDependentType.OneOrMoreOrders:
                        EnableComponent(component, oneOrMoreOrders);
                        break;

                    case SelectionDependentType.TwoOrMoreOrders:
                        EnableComponent(component, twoOrMoreOrders || twoOrMoreCustomers);
                        break;

                    case SelectionDependentType.OneCustomer:
                        EnableComponent(component, oneCustomer);
                        break;

                    case SelectionDependentType.OneOrMoreCustomers:
                        EnableComponent(component, oneOrMoreCustomers);
                        break;

                    case SelectionDependentType.OneRow:
                        EnableComponent(component, oneOrder || oneCustomer);
                        break;

                    case SelectionDependentType.OneOrMoreRows:
                        EnableComponent(component, oneOrMoreOrders || oneOrMoreCustomers);
                        break;

                    case SelectionDependentType.AppliesFunction:
                        if (entry.Value.Applies != null)
                        {
                            EnableComponent(component, entry.Value.Applies(selection.Keys));
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Enable or disable the given component
        /// </summary>
        private void EnableComponent(Component component, bool enable)
        {
            ToolStripItem item = component as ToolStripItem;
            if (item != null)
            {
                item.Enabled = enable;
            }

            Divelements.SandRibbon.Button button = component as Divelements.SandRibbon.Button;
            if (button != null)
            {
                button.Enabled = enable;
            }
        }
    }

}
