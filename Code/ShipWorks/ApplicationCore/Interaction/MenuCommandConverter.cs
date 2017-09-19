using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SandMenu = Divelements.SandRibbon.Menu;
using SandMenuItem = Divelements.SandRibbon.MenuItem;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// Converts MenuCommand objects to usable UI elements
    /// </summary>
    public static class MenuCommandConverter
    {
        /// <summary>
        /// Create a list of ToolStripItem objects from the given MenuCommand list
        /// </summary>
        public static ToolStripItem[] ToToolStripItems(IEnumerable<IMenuCommand> commands, EventHandler actionHandler)
        {
            List<ToolStripItem> items = new List<ToolStripItem>();

            // Go through each command
            foreach (IMenuCommand command in commands)
            {
                // Add separator if we need it
                if (command.BreakBefore)
                {
                    if (items.Count > 0 && !(items[items.Count - 1] is ToolStripSeparator))
                    {
                        items.Add(new ToolStripSeparator());
                    }
                }

                ToolStripMenuItem item = new ToolStripMenuItem(command.Text);
                item.Tag = command;
                item.Enabled = command.Enabled;

                if (command.ChildCommands.Count == 0)
                {
                    item.Click += actionHandler;
                }

                // Listen for the enabled state to change
                command.EnabledChanged += (sender, e) => item.Enabled = ((IMenuCommand) sender).Enabled;

                items.Add(item);

                // Add child items
                item.DropDownItems.AddRange(ToToolStripItems(command.ChildCommands, actionHandler));

                // Add a separator if we need it
                if (command.BreakAfter)
                {
                    items.Add(new ToolStripSeparator());
                }
            }

            // Don't let it end in a separator
            if (items.Count > 0 && items[items.Count - 1] is ToolStripSeparator)
            {
                items.RemoveAt(items.Count - 1);
            }

            return items.ToArray();
        }

        /// <summary>
        /// Create a SandRibbon menu from the given MenuCommand list
        /// </summary>
        public static SandMenu ToRibbonMenu(IEnumerable<IMenuCommand> commands, EventHandler actionHandler)
        {
            SandMenu menu = new SandMenu();

            // Separators for the ribbon are done using groups
            int group = 0;

            // Add each command
            foreach (IMenuCommand command in commands)
            {
                // To make a break, we start a new group
                if (command.BreakBefore)
                {
                    group++;
                }

                SandMenuItem item = new SandMenuItem(command.Text);
                item.GroupName = string.Format("Group{0}", group);
                item.Tag = command;
                item.Activate += actionHandler;
                item.Enabled = command.Enabled;

                // Listen for the enabled state to change
                command.EnabledChanged += (sender, e) => item.Enabled = ((IMenuCommand) sender).Enabled;

                menu.Items.Add(item);

                // Add children
                if (command.ChildCommands.Count > 0)
                {
                    item.Items.Add(ToRibbonMenu(command.ChildCommands, actionHandler));
                }

                // To make a break, we start a new group
                if (command.BreakAfter)
                {
                    group++;
                }
            }

            return menu;
        }

        /// <summary>
        /// Extract a MenuCommand object from the given UI sender
        /// </summary>
        public static IMenuCommand ExtractMenuCommand(object sender)
        {
            // Extract the command from the sender
            ToolStripItem toolStripItem = sender as ToolStripItem;
            if (toolStripItem != null)
            {
                return (IMenuCommand) toolStripItem.Tag;
            }
            else
            {
                return (IMenuCommand) ((SandMenuItem) sender).Tag;
            }
        }
    }
}
