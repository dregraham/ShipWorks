﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Filters;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// Provides design-time declarative UI state management for menu and buttons
    /// </summary>
    [ProvideProperty("EnabledWhen", typeof(Component))]
    public partial class SelectionDependentEnabler : Component, IExtenderProvider
    {
        Dictionary<Component, SelectionDependentType> componentMap = new Dictionary<Component, SelectionDependentType>();

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
            SelectionDependentType state;
            if (!componentMap.TryGetValue(component, out state))
            {
                return SelectionDependentType.Ignore;
            }

            return state;
        }

        /// <summary>
        /// Necessary piece of the extender provider plumbing
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetEnabledWhen(Component component, SelectionDependentType state)
        {
            if (!CanExtend(component))
            {
                throw new ArgumentException("The object type is not supported by the ExtenderProvider.", "component");
            }

            if (state == SelectionDependentType.Ignore)
            {
                componentMap.Remove(component);
            }
            else
            {
                componentMap[component] = state;
            }
        }

        /// <summary>
        /// Update the command state based on the selection state of the given grid control
        /// </summary>
        public void UpdateCommandState(int selectionCount, FilterTarget target)
        {
            bool oneOrder = target == FilterTarget.Orders && selectionCount == 1;
            bool oneOrMoreOrders = target == FilterTarget.Orders && selectionCount > 0;

            bool oneCustomer = target == FilterTarget.Customers && selectionCount == 1;
            bool oneOrMoreCustomers = target == FilterTarget.Customers && selectionCount > 0;

            // Go through each registered component
            foreach (KeyValuePair<Component, SelectionDependentType> entry in componentMap)
            {
                // Skip any that are being updated manually
                if (entry.Value == SelectionDependentType.Ignore)
                {
                    continue;
                }

                Component component = entry.Key;

                switch (entry.Value)
                {
                    case SelectionDependentType.OneOrder:
                        EnableComponent(component, oneOrder);
                        break;

                    case SelectionDependentType.OneOrMoreOrders:
                        EnableComponent(component, oneOrMoreOrders);
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
