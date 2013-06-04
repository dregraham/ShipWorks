using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public static ToolStripItem[] ToToolStripItems(List<MenuCommand> commands, EventHandler actionHandler)
        {
            List<ToolStripItem> items = new List<ToolStripItem>();

            // Go through each command
            foreach (MenuCommand command in commands)
            {
                // Add seperator if we need it
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
                command.EnabledChanged += delegate(object sender, EventArgs e) { item.Enabled = ((MenuCommand) sender).Enabled; };

                items.Add(item);

                // Add child items
                item.DropDownItems.AddRange(ToToolStripItems(command.ChildCommands, actionHandler));

                // Add a seperator if we need it
                if (command.BreakAfter)
                {
                    items.Add(new ToolStripSeparator());
                }
            }

            // Don't let it end in a sep
            if (items.Count > 0 && items[items.Count - 1] is ToolStripSeparator)
            {
                items.RemoveAt(items.Count - 1);
            }

            return items.ToArray();
        }

        /// <summary>
        /// Create a SandRibbon menu from the given MenuCommand list
        /// </summary>
        public static SandMenu ToRibbonMenu(List<MenuCommand> commands, EventHandler actionHandler)
        {
            SandMenu menu = new SandMenu();

            // Seperators for the ribbon are done using groups
            int group = 0;

            // Add each command
            foreach (MenuCommand command in commands)
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
                command.EnabledChanged += delegate(object sender, EventArgs e) { item.Enabled = ((MenuCommand) sender).Enabled; };

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
        public static MenuCommand ExtractMenuCommand(object sender)
        {
            // Extract the command from the sender
            ToolStripItem toolStripItem = sender as ToolStripItem;
            if (toolStripItem != null)
            {
                return (MenuCommand) toolStripItem.Tag;
            }
            else
            {
                return (MenuCommand) ((SandMenuItem) sender).Tag;
            }
        }
    }
}
