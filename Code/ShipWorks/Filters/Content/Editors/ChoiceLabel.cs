using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using System.Linq;
using System.Collections;
using SandContextPopup = Divelements.SandRibbon.ContextPopup;
using SandMenuItem = Divelements.SandRibbon.MenuItem;
using SandMenu = Divelements.SandRibbon.Menu;
using ShipWorks.Filters.Content.Conditions;

namespace ShipWorks.Filters.Content.Editors
{
    /// <summary>
    /// Custom label for given the user choices in the FilterDefinition editor.
    /// </summary>
    public partial class ChoiceLabel : Label
    {
        ChoiceLabelUsage usage;

        SandContextPopup contextMenu;
        Enum selectedValue;

        public event EventHandler SelectedValueChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChoiceLabel()
        {
            InitializeComponent();

            LabelUsage = ChoiceLabelUsage.Operator;
        }

        /// <summary>
        /// Initialize from the given list of enums.
        /// </summary>
        public void InitializeFromEnumList(IEnumerable list)
        {
            InitializeFromEnumList(list, null);
        }

        /// <summary>
        /// Initialize from the given list of enums
        /// </summary>
        public void InitializeFromEnumList(IEnumerable list, Func<Enum, string> textProvider)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            contextMenu = new SandContextPopup();
            contextMenu.Font = new Font(Font, FontStyle.Regular);

            SandMenu menu = new SandMenu();
            contextMenu.Items.Add(menu);

            foreach (Enum item in list)
            {
                SandMenuItem menuItem = new SandMenuItem((textProvider != null) ? textProvider(item) : EnumHelper.GetDescription(item));
                menuItem.Tag = item;
                menuItem.Activate += new EventHandler(this.OnChangeValue);

                menu.Items.Add(menuItem);
            }

            // In case the text changed, update the displayed label text
            if (selectedValue != null)
            {
                SelectedValue = selectedValue;
            }
        }

        /// <summary>
        /// Initialize the drop down functionality of the label using the specified enumeration as the item source
        /// </summary>
        public void InitializeFromEnumType(Type enumType)
        {
            if (enumType.Name == typeof(DateOperator).Name)
            {
                EnumList<DateOperator> enumList = EnumHelper.GetEnumList<DateOperator>();
                InitializeFromEnumList(enumList.Select(e => e.Value).ToArray());
            }
            else
            {
                InitializeFromEnumList(Enum.GetValues(enumType));
            }
        }

        /// <summary>
        /// Initialize the drop down functionality of the label using the specified enumeration as the item source
        /// </summary>
        public void InitializeFromEnumType(Type enumType, Func<Enum, string> textProvider)
        {
            InitializeFromEnumList(Enum.GetValues(enumType), textProvider);
        }

        /// <summary>
        /// Get / sets the selected enumeration value.
        /// </summary>
        [DefaultValue(null)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Enum SelectedValue
        {
            get
            {
                return selectedValue;
            }
            set
            {
                selectedValue = value;

                // If it's in the menu, use the value as it was loaded
                SandMenuItem menuItem = ((SandMenu) contextMenu.Items[0]).Items.OfType<SandMenuItem>().Where(i => i.Tag.Equals(value)).SingleOrDefault();
                if (menuItem != null)
                {
                    this.Text = menuItem.Text;
                }
                else
                {
                    // Fallback on it's default description
                    this.Text = EnumHelper.GetDescription(selectedValue);
                }
            }
        }

        /// <summary>
        /// User made a selection from the drop-down
        /// </summary>
        private void OnChangeValue(object sender, EventArgs e)
        {
            Enum op = (Enum) ((SandMenuItem) sender).Tag;

            SelectedValue = op;

            if (SelectedValueChanged != null)
            {
                SelectedValueChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// The usage of the label.  Dictates style.
        /// </summary>
        [DefaultValue(ChoiceLabelUsage.Operator)]
        public ChoiceLabelUsage LabelUsage
        {
            get
            { 
                return usage; 
            }
            set
            {
                usage = value;

                base.ForeColor = GetUsageColor();
            }
        }

        /// <summary>
        /// The foreground color is determined by the LabelUsage.
        /// </summary>
        [Browsable(false)]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                
            }
        }

        /// <summary>
        /// Get the color for the configured label usage
        /// </summary>
        private Color GetUsageColor()
        {
            switch (usage)
            {
                case ChoiceLabelUsage.Condition:
                    return Color.Blue;

                case ChoiceLabelUsage.Join:
                    return Color.Red;

                case ChoiceLabelUsage.Operator:
                    return Color.Green;
            }

            return Color.Orange;
        }

        /// <summary>
        /// Look for mouse down to show the context menu
        /// </summary>
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (contextMenu != null)
            {
                contextMenu.ShowStandalone(this, new Point(0, Height), false);
            }
        }
    }
}
