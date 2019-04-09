using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using Interapptive.Shared;
using Divelements.SandRibbon;
using Divelements.SandRibbon.Rendering;
using ShipWorks.Filters.Content.Conditions;
using System.Xml.XPath;
using ShipWorks.Properties;
using System.IO;
using System.Reflection;
using ShipWorks.Data;
using Interapptive.Shared.Utility;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms;
using SandContextPopup = Divelements.SandRibbon.ContextPopup;
using SandMenuItem = Divelements.SandRibbon.MenuItem;
using SandMenu = Divelements.SandRibbon.Menu;
using SandHeading = Divelements.SandRibbon.PopupHeading;

namespace ShipWorks.Filters.Content.Editors
{
    /// <summary>
    /// A combo box that allows the user to select a condition type
    /// </summary>
    public partial class ConditionChooser : System.Windows.Forms.Label
    {
        // The popup we use for the drop down portion
        SandContextPopup contextMenu;

        // The condition type currently selected
        ConditionElementDescriptor selectedType;

        // Cached hierarchy
        static XPathDocument elementHierarchy;

        /// <summary>
        /// Raised when the selected condition type changes
        /// </summary>
        public event EventHandler SelectedConditionTypeChanged;

        // Groups are what control how separators are displayed
        int groupNumber = 1;

        /// <summary>
        /// Static constructor
        /// </summary>
        static ConditionChooser()
        {
            // Open the embedded stream
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ShipWorks.Filters.Content.ConditionTree.xml"))
            {
                elementHierarchy = new XPathDocument(stream);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ConditionChooser()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get or set the currently selected condition type.
        /// </summary>
        public ConditionElementDescriptor SelectedConditionType
        {
            get
            {
                return selectedType;
            }
            set
            {
                if (selectedType != value)
                {
                    selectedType = value;
                    this.Text = string.Format("{0}", selectedType.DisplayName);

                    if (SelectedConditionTypeChanged != null)
                    {
                        SelectedConditionTypeChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Initialize the dropdown list to display choices that are compatible with the given subject
        /// </summary>
        public void Initialize(ConditionEntityTarget entityTarget)
        {
            contextMenu = new SandContextPopup();
            contextMenu.Font = new Font(Font, FontStyle.Regular);

            XPathNavigator xpath = elementHierarchy.CreateNavigator();
            xpath.MoveToFirstChild();

            XPathNodeIterator categories = xpath.Select(string.Format("ConditionTypes[@entity='{0}']/Category", EnumHelper.GetDescription(entityTarget)));
            while (categories.MoveNext())
            {
                XPathNavigator category = categories.Current;
                XPathNodeIterator items = category.SelectChildren(XPathNodeType.Element);

                // Add the child items the the menu
                SandMenu menu = new SandMenu();
                AddItems(menu, items);

                // Only add the category and menu if it contains anything
                if (menu.Items.Count > 0)
                {
                    AddHeading(category.GetAttribute("name", ""));
                    contextMenu.Items.Add(menu);
                }   
            }

            // We always add the combined result
            AddHeading("Condition Group");

            // Add the special items
            SandMenu specialMenu = new SandMenu();
            specialMenu.Items.Add(CreateMenuItem("Special.NestedConditions", "The combined result of..."));
            contextMenu.Items.Add(specialMenu);
        }

        /// <summary>
        /// Add a heading to the drop down chooser with the given name
        /// </summary>
        private void AddHeading(string text)
        {
            SandHeading heading = new SandHeading();
            heading.Text = text;

            contextMenu.Items.Add(heading);
        }

        /// <summary>
        /// Add the items found in the xpath iterator
        /// </summary>
        private void AddItems(SandMenu menu, XPathNodeIterator items)
        {
            while (items.MoveNext())
            {
                XPathNavigator item = items.Current;
                if (item.Name == "Group")
                {
                    SandMenu groupMenu = new SandMenu();
                    AddItems(groupMenu, item.SelectChildren(XPathNodeType.Element));

                    // Only add the group menu if it contains anything
                    if (groupMenu.Items.Count > 0)
                    {
                        SandMenuItem menuItem = new SandMenuItem(item.GetAttribute("name", ""));
                        menuItem.Items.Add(groupMenu);

                        menu.Items.Add(menuItem);
                    }
                }

                else if (item.Name == "Condition")
                {
                    SandMenuItem menuItem = CreateMenuItem(item.GetAttribute("identifier", ""), item.GetAttribute("displayAs", ""));
                    if (menuItem != null)
                    {
                        menu.Items.Add(menuItem);
                    }
                }

                else if (item.Name == "Separator")
                {
                    groupNumber++;
                }
            }
        }

        /// <summary>
        /// Return a MenuItem initialized and ready from the given condition element identifier;
        /// </summary>
        private SandMenuItem CreateMenuItem(string identifier, string displayAs)
        {
            ConditionElementDescriptor descriptor = ConditionElementFactory.GetDescriptor(identifier);
            if (!descriptor.IsApplicable(StoreManager.GetAllStoresReadOnly().Select(StoreTypeManager.GetTypeWithReadOnlyStore).ToList()))
            {
                return null;
            }

            string menuText = displayAs;
            if (string.IsNullOrEmpty(menuText))
            {
                menuText = descriptor.DisplayName;
            }

            SandMenuItem menuItem = new SandMenuItem(menuText);
            menuItem.Tag = descriptor;
            menuItem.GroupName = groupNumber.ToString();
            menuItem.Activate += new EventHandler(OnConditionTypeSelected);

            return menuItem;
        }

        /// <summary>
        /// A condition type has been chosen by the user
        /// </summary>
        void OnConditionTypeSelected(object sender, EventArgs e)
        {
            SelectedConditionType = (ConditionElementDescriptor) ((SandMenuItem) sender).Tag;
        }

        /// <summary>
        /// Show the context menu when clicked
        /// </summary>
        private void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (contextMenu != null)
            {
                contextMenu.ShowStandalone(this, new Point(0, Height), false);
            }
        }
    }
}
