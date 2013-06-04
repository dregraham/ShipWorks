using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace ShipWorks.UI.Controls.Design
{
    /// <summary>
    /// Implements a custom type editor for selecting an enum that is marked as having flags
    /// </summary>
    public class EnumFlagsTypeEditor : UITypeEditor
    {
        IWindowsFormsEditorService editService = null;
        CheckedListBox listBox;

        bool handleLostfocus = false;

        /// <summary>
        /// Internal class used for storing custom data in listviewitems
        /// </summary>
        class FlagsListItem
        {
            string text;
            int value;

            /// <summary>
            /// Constructor
            /// </summary>
            public FlagsListItem(string text, int value)
            {
                this.text = text;
                this.value = value;
            }

            /// <summary>
            /// Text to display for this enum
            /// </summary>
            public string Text
            {
                get { return text; }
            }

            /// <summary>
            /// Gets the int value for this item
            /// </summary>
            public int Value
            {
                get { return value; }
            }

            /// <summary>
            /// Gets the name of this item
            /// </summary>
            public override string ToString()
            {
                return text;
            }
        }

        /// <summary>
        /// Overrides the method used to provide basic behaviour for selecting editor.
        /// Shows our custom control for editing the value.
        /// </summary>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                editService = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));

                if (editService != null)
                {
                    // Create a CheckedListBox and populate it with all the enum values
                    listBox = new CheckedListBox();
                    listBox.Height = 300;
                    listBox.BorderStyle = BorderStyle.FixedSingle;
                    listBox.MouseDown += new MouseEventHandler(this.OnMouseDown);

                    listBox.CheckOnClick = true;
                    listBox.ItemCheck += new ItemCheckEventHandler(OnItemCheck);

                    // Get the int value of the current enum value (the one being edited)
                    int currentValue = (int) Convert.ChangeType(value, typeof(int));

                    // Add each enum possibility as a checkbox
                    foreach (string enumName in Enum.GetNames(context.PropertyDescriptor.PropertyType))
                    {
                        int enumValue = (int) Convert.ChangeType(Enum.Parse(context.PropertyDescriptor.PropertyType, enumName), typeof(int));

                        // Creates an item that stores the name, the int value and the tooltip
                        FlagsListItem item = new FlagsListItem(enumName, enumValue);

                        // Add the item
                        listBox.Items.Add(item);
                    }

                    // Update the check state for hte list box
                    UpdateCheckState(listBox, currentValue);

                    // This methods returns only when the dropdowncontrol is closed
                    editService.DropDownControl(listBox);

                    // Get the sum of all checked flags
                    int result = GetCheckListValue(listBox);

                    // return the right enum value corresponding to the result
                    return Enum.ToObject(context.PropertyDescriptor.PropertyType, result);
                }
            }

            return value;
        }
        
        /// <summary>
        /// A list item check state has changed
        /// </summary>
        void OnItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox listBox = (CheckedListBox) sender;
            int value = GetCheckListValue(listBox);

            FlagsListItem listItem = (FlagsListItem) listBox.Items[e.Index];

            // If its checked, add in the new value
            if (e.NewValue == CheckState.Checked)
            {
                if (listItem.Value == 0)
                {
                    value = 0;
                }
                else
                {
                    value |= listItem.Value;
                }
            }
            // Otherwise strip out all the bits of the unchecked box
            else
            {
                value &= ~listItem.Value;
            }

            UpdateCheckState(listBox, value);
        }

        /// <summary>
        /// Get the value currently selected in the checklist box
        /// </summary>
        private int GetCheckListValue(CheckedListBox listBox)
        {
            int result = 0;
            foreach (FlagsListItem listItem in listBox.CheckedItems)
            {
                result |= listItem.Value;
            }

            return result;
        }

        /// <summary>
        /// Update the state of the CheckListBox
        /// </summary>
        private void UpdateCheckState(CheckedListBox listBox, int currentValue)
        {
            listBox.ItemCheck -= this.OnItemCheck;

            foreach (FlagsListItem listItem in listBox.Items.Cast<FlagsListItem>().ToList())
            {
                // Get the checkstate from the value being edited
                bool checkedItem = listItem.Value == 0 ? currentValue == 0 : (currentValue & listItem.Value) == listItem.Value;

                listBox.SetItemChecked(listBox.Items.IndexOf(listItem), checkedItem);
            }

            listBox.ItemCheck += this.OnItemCheck;
        }

        /// <summary>
        /// Shows a dropdown icon in the property editor
        /// </summary>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        /// <summary>
        /// When got the focus, handle the lost focus event.
        /// </summary>
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (!handleLostfocus && listBox.ClientRectangle.Contains(listBox.PointToClient(new Point(e.X, e.Y))))
            {
                listBox.LostFocus += new EventHandler(this.OnLostFocus);
                handleLostfocus = true;
            }
        }

        /// <summary>
        /// Close the dropdowncontrol when the user has selected a value
        /// </summary>
        private void OnLostFocus(object sender, EventArgs e)
        {
            if (editService != null)
            {
                editService.CloseDropDown();
            }
        }
    }
}
