using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using Interapptive.Shared;
using System.Drawing;
using Interapptive.Shared.Win32;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// A ComboBox that displays a Menu instead of the standard drop-down stuff
    /// </summary>
    public class MenuComboBox : ComboBox
    {
        ContextMenuStrip contextMenu = null;

        object selectedMenuObject = null;

        Func<object, string> displayValueProvider = null;

        /// <summary>
        /// Raised when the selected item chagnes
        /// </summary>
        [Category("Behavior")]
        public event EventHandler SelectedMenuObjectChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public MenuComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Overridden to provide new default value
        /// </summary>
        [DefaultValue(ComboBoxStyle.DropDownList)]
        public new ComboBoxStyle DropDownStyle
        {
            get { return base.DropDownStyle; }
            set { base.DropDownStyle = value; }
        }

        /// <summary>
        /// Overridden to turn-off the design-time
        /// </summary>
        [Browsable(false)]
        public override ContextMenuStrip ContextMenuStrip
        {
            get { return contextMenu; }
            set { contextMenu = value; }
        }

        [DefaultValue(null)]
        public ContextMenuStrip DropDownMenu
        {
            get 
            { 
                return contextMenu; 
            }
            set 
            {
                if (contextMenu == value)
                {
                    return;
                }

                if (contextMenu != null)
                {
                    UnhookEvents(contextMenu.Items);
                }

                contextMenu = value;

                if (contextMenu != null)
                {
                    HookEvents(contextMenu.Items);
                }

                UpdateDisplayText();
            }
        }

        /// <summary>
        /// Can optionally be set to provide custom display text for given object values.  Returning null 
        /// from the function causes the default to be used.  The default is to use the MenuItem.Text property
        /// for a menu item with a matching tag as the object.
        /// </summary>
        [Browsable(false)]
        public Func<object, string> DisplayValueProvider
        {
            get 
            { 
                return displayValueProvider; 
            }
            set
            {
                displayValueProvider = value;

                UpdateDisplayText();
            }
        }

        /// <summary>
        /// Hook the events of all the items in the collection recursively
        /// </summary>
        private void HookEvents(ToolStripItemCollection items)
        {
            foreach (ToolStripMenuItem item in items.OfType<ToolStripMenuItem>())
            {
                if (item.DropDownItems.Count == 0)
                {
                    item.Click += new EventHandler(OnMenuItemClicked);
                }
                else
                {
                    HookEvents(item.DropDownItems);
                }
            }
        }

        /// <summary>
        /// Unhook the events of all the items in the collection recursively
        /// </summary>
        private void UnhookEvents(ToolStripItemCollection items)
        {
            foreach (ToolStripMenuItem item in items.OfType<ToolStripMenuItem>())
            {
                item.Click -= new EventHandler(OnMenuItemClicked);

                UnhookEvents(item.DropDownItems);
            }
        }

        /// <summary>
        /// A menu item has been selected
        /// </summary>
        void OnMenuItemClicked(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;

            SelectedMenuObject = menuItem.Tag;
        }

        /// <summary>
        /// The Tag propertly value of the selected MenuItem.
        /// </summary>
        [Browsable(false)]
        public object SelectedMenuObject
        {
            get
            {
                return selectedMenuObject;
            }
            set
            {
                if (selectedMenuObject == null && value == null)
                {
                    return;
                }

                if (selectedMenuObject != null && value != null && selectedMenuObject.Equals(value))
                {
                    return;
                }

                selectedMenuObject = value;

                UpdateDisplayText();

                // Raise the change event
                if (SelectedMenuObjectChanged != null)
                {
                    SelectedMenuObjectChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Update the display text based on the current selection
        /// </summary>
        private void UpdateDisplayText()
        {
            // Hack to get the text to display
            Items.Clear();

            if (selectedMenuObject != null)
            {
                Items.Add(GetTagText(selectedMenuObject));
                SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Return the text to use for the given tag value
        /// </summary>
        private string GetTagText(object tag)
        {
            if (displayValueProvider != null)
            {
                string text = displayValueProvider(tag);
                if (!string.IsNullOrEmpty(text))
                {
                    return text;
                }
            }

            if (contextMenu != null)
            {
                ToolStripMenuItem foundItem = FindMenuItemTag(contextMenu.Items, tag);

                if (foundItem != null)
                {
                    return foundItem.Text;
                }
            }

            return tag.ToString();
        }

        /// <summary>
        /// Find the menu item whose tag represents the given value
        /// </summary>
        private ToolStripMenuItem FindMenuItemTag(ToolStripItemCollection items, object value)
        {
            if (value == null)
            {
                return null;
            }

            foreach (ToolStripMenuItem item in items.OfType<ToolStripMenuItem>())
            {
                if (item.Tag != null && item.Tag.Equals(value))
                {
                    return item;
                }

                ToolStripMenuItem childFound = FindMenuItemTag(item.DropDownItems, value);
                if (childFound != null)
                {
                    return childFound;
                }
            }

            return null;
        }

        /// <summary>
        /// Intercept the mouse down
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            // On mouse down, we show our color chooser
            if (m.Msg == NativeMethods.WM_LBUTTONDOWN || m.Msg == NativeMethods.WM_LBUTTONDBLCLK)
            {
                if (contextMenu == null)
                {
                    throw new InvalidOperationException("The DropDownMenu property has not been initialized.");
                }

                if (GetStyle(ControlStyles.Selectable))
                {
                    Focus();
                }

                Point location = Parent.PointToScreen(new Point(Left, Bottom));
                contextMenu.Show(location);
            }

            else
            {
                base.WndProc(ref m);
            }
        }
    }
}
