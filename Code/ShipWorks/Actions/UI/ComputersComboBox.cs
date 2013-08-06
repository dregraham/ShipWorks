using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI;
using ShipWorks.UI.Controls;
using ShipWorks.Users;
using System;
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
    public class ComputersComboBox : PopupComboBox
    {
        FlowLayoutPanel checkBoxPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComputersComboBox"/> class.
        /// </summary>
        public ComputersComboBox()
        {
            InitializeComponent();

            checkBoxPanel.FlowDirection = FlowDirection.TopDown;
            PopupController = new PopupController(checkBoxPanel);

            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                BindComputers();
        }

        private void InitializeComponent()
        {
            this.checkBoxPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // checkBoxPanel
            // 
            this.checkBoxPanel.AutoScroll = true;
            this.checkBoxPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.checkBoxPanel.Location = new System.Drawing.Point(0, 0);
            this.checkBoxPanel.Name = "checkBoxPanel";
            this.checkBoxPanel.Size = new System.Drawing.Size(130, 306);
            this.checkBoxPanel.TabIndex = 0;
            this.checkBoxPanel.Visible = false;
            this.checkBoxPanel.WrapContents = false;
            // 
            // ComputersComboBox
            // 
            this.Controls.Add(this.checkBoxPanel);
            this.Name = "ComputerComboPopup";
            this.Size = new System.Drawing.Size(316, 21);
            this.ResumeLayout(false);

        }

        void BindComputers()
        {
            ComputerManager.CheckForChangesNeeded();

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
            SetSelectedItems(x => (ComputerEntity)x, computers);
        }

        /// <summary>
        /// Sets the selected computers by ID.
        /// </summary>
        public void SetSelectedComputers(IEnumerable<long> computerIDs)
        {
            SetSelectedItems(x => ((ComputerEntity)x).ComputerID, computerIDs);
        }


        IEnumerable<CheckBox> CheckBoxes
        {
            get { return checkBoxPanel.Controls.Cast<CheckBox>(); }
        }

        IEnumerable<CheckBox> SelectedCheckBoxes
        {
            get { return CheckBoxes.Where(x => x.Checked); }
        }


        IEnumerable<object> GetSelectedItems()
        {
            return SelectedCheckBoxes.Select(x => x.Tag);
        }

        void SetSelectedItems<TKey>(Func<object, TKey> keySelector, IEnumerable<TKey> keys)
        {
            var matches = CheckBoxes.GroupJoin(keys, x => keySelector(x.Tag), x => x, (x, k) => new { CheckBox = x, Matched = k.Any() });
            foreach (var item in matches)
            {
                item.CheckBox.Checked = item.Matched;
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
