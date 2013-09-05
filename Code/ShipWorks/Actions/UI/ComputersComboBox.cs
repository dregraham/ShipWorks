using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI;
using ShipWorks.UI.Controls;
using ShipWorks.Users;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.ComponentModel;


namespace ShipWorks.Actions.UI
{
    /// <summary>
    /// Shows a list of individually selectable current computers.
    /// </summary>
    public partial class ComputersComboBox : PopupComboBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComputersComboBox"/> class.
        /// </summary>
        public ComputersComboBox()
        {
            InitializeComponent();

            checkBoxPanel.FlowDirection = FlowDirection.TopDown;
            PopupController = new PopupController(checkBoxPanel);

            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                BindComputers();   
            }
        }

        /// <summary>
        /// Add the list of computers to the check box panel
        /// </summary>
        void BindComputers()
        {
            checkBoxPanel.Controls.Clear();
            checkBoxPanel.Controls.AddRange(
                ComputerManager.Computers
                    .OrderBy(x => x.Name)
                    .Select(x => new CheckBox { Text = x.Name, Tag = x, AutoSize = true })
                    .ToArray()
            );
        }


        /// <summary>
        /// Gets the selected computers.
        /// </summary>
        public IList<ComputerEntity> GetSelectedComputers()
        {
            return GetSelectedItems().Cast<ComputerEntity>().ToList();
        }

        /// <summary>
        /// Sets the selected computers.
        /// </summary>
        public void SetSelectedComputers(IEnumerable<ComputerEntity> computers)
        {
            SetSelectedItems(computers.Select(x => x.ComputerID));
        }

        /// <summary>
        /// Sets the selected computers by ID.
        /// </summary>
        public void SetSelectedComputers(IEnumerable<long> computerIDs)
        {
            SetSelectedItems(computerIDs);
        }

        /// <summary>
        /// Gets a list of checkboxes from the controls collection
        /// </summary>
        IEnumerable<CheckBox> CheckBoxes
        {
            get { return checkBoxPanel.Controls.Cast<CheckBox>(); }
        }

        /// <summary>
        /// Gets a list of selected checkboxes from the controls collection
        /// </summary>
        IEnumerable<CheckBox> SelectedCheckBoxes
        {
            get { return CheckBoxes.Where(x => x.Checked); }
        }

        /// <summary>
        /// Gets a list of items associated with the selected checkboxes from the controls collection
        /// </summary>
        IEnumerable<object> GetSelectedItems()
        {
            return SelectedCheckBoxes.Select(x => x.Tag);
        }

        /// <summary>
        /// Checks the checkbox for all the 
        /// </summary>
        /// <param name="computerIds"></param>
        void SetSelectedItems(IEnumerable<long> computerIds)
        {
            // Create a hash set to avoid multiple enumerations of computerId
            HashSet<long> computers = new HashSet<long>(computerIds);

            foreach (var checkBox in CheckBoxes)
            {
                long computerId = ((ComputerEntity) checkBox.Tag).ComputerID;
                checkBox.Checked = computers.Contains(computerId);
            }
        }

        /// <summary>
        /// Draws the item that the user has currently selected.
        /// </summary>
        protected override void OnDrawSelectedItem(Graphics graphics, Color foreColor, Rectangle bounds)
        {
            string text = string.Join(", ", SelectedCheckBoxes.Select(x => x.Text));
            using (var stringFormat = new StringFormat())
            {
                stringFormat.Trimming = StringTrimming.EllipsisCharacter;
                using (var brush = new SolidBrush(ForeColor))
                {
                    graphics.DrawString(text, Font, brush, bounds, stringFormat);
                }
            }
        }
    }
}
