using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.AddressValidation;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.UI;
using ShipWorks.UI.Controls;

namespace ShipWorks.Filters.Content.Editors.ValueEditors.UI
{
    internal class AddressValidationStatusPopup : PopupComboBox
    {
        private readonly List<Tuple<CheckBox, AddressValidationStatusType>> statusList;

        private bool ignoreChanged;
        private Panel statusPanel;
        private Label readyToGoLabel;
        private Label needsAttentionLabel;
        private Label notValidated;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressValidationStatusPopup"/> class.
        /// </summary>
        public AddressValidationStatusPopup()
        {
            statusList = new List<Tuple<CheckBox, AddressValidationStatusType>>();

            InitializeComponent();

            PopupController = new PopupController(statusPanel);

            BindStatuses();

            int panelHeight = statusList.Last().Item1.Bottom + 5;
            statusPanel.Height = panelHeight;
            DropDownMinimumHeight = panelHeight - 12;
            DropDownHeight = panelHeight - 12;
            Height = panelHeight + 20;
        }

        /// <summary>
        /// Binds the statuses.
        /// </summary>
        private void BindStatuses()
        {
            EnumList<AddressValidationStatusType> statuses = EnumHelper.GetEnumList<AddressValidationStatusType>();

            // Loop through all the statuses in AddressValidationStatusType.
            for (int statusIndex = 0; statusIndex < statuses.Count; statusIndex++)
            {
                EnumEntry<AddressValidationStatusType> status = statuses[statusIndex];

                // * 2 makes room for the label buttons.
                int verticlePosition = 23*(statusIndex + 2);

                // Build statusList with new checkboxes and related enums.
                var checkboxAndStatusType = new Tuple<CheckBox, AddressValidationStatusType>(new CheckBox
                {
                    Text = status.Value == AddressValidationStatusType.Pending ? "Pending" : status.Key,
                    Location = new Point(3, verticlePosition),
                    AutoSize = true
                }, status.Value);

                checkboxAndStatusType.Item1.CheckedChanged += OnStatusCheckChanged;

                statusList.Add(checkboxAndStatusType);

                // Add checkbox to panel
                statusPanel.Controls.Add(checkboxAndStatusType.Item1);
                if (statusPanel.Width < checkboxAndStatusType.Item1.Width)
                {
                    statusPanel.Width = checkboxAndStatusType.Item1.Width;
                }
            }
        }

        /// <summary>
        /// Called when [status check changed].
        /// </summary>
        private void OnStatusCheckChanged(object sender, EventArgs e)
        {
            if (!ignoreChanged)
            {
                if (StatusChanged != null)
                {
                    StatusChanged();
                }

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the status changed.
        /// </summary>
        public Action StatusChanged { private get; set; }

        /// <summary>
        /// Initialize Component
        /// </summary>
        private void InitializeComponent()
        {
            this.statusPanel = new Panel();
            this.readyToGoLabel = new Label();
            this.needsAttentionLabel = new Label();
            this.notValidated = new Label();
            this.statusPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusPanel
            // 
            this.statusPanel.BackColor = SystemColors.ControlLightLight;
            this.statusPanel.Controls.Add(this.readyToGoLabel);
            this.statusPanel.Controls.Add(this.needsAttentionLabel);
            this.statusPanel.Controls.Add(this.notValidated);
            this.statusPanel.Location = new Point(0, 0);
            this.statusPanel.Name = "statusPanel";
            this.statusPanel.Size = new Size(130, 306);
            this.statusPanel.TabIndex = 7;
            this.statusPanel.Visible = false;
            // 
            // readyToGoLabel
            // 
            this.readyToGoLabel.AutoSize = true;
            this.readyToGoLabel.Cursor = Cursors.Hand;
            this.readyToGoLabel.Font = new Font("Tahoma", 8.25F, FontStyle.Underline, GraphicsUnit.Point, ((byte)(0)));
            this.readyToGoLabel.ForeColor = Color.Blue;
            this.readyToGoLabel.Location = new Point(3, 3);
            this.readyToGoLabel.Name = "readyToGoLabel";
            this.readyToGoLabel.Size = new Size(101, 13);
            this.readyToGoLabel.TabIndex = 1;
            this.readyToGoLabel.Text = "Select Ready To Go";
            this.readyToGoLabel.Click += OnReadyToGoLabelClicked;
            // 
            // notValidated
            // 
            this.notValidated.AutoSize = true;
            this.notValidated.Cursor = Cursors.Hand;
            this.notValidated.Font = new Font("Tahoma", 8.25F, FontStyle.Underline, GraphicsUnit.Point, ((byte)(0)));
            this.notValidated.ForeColor = Color.Blue;
            this.notValidated.Location = new Point(3, 26);
            this.notValidated.Name = "notValidated";
            this.notValidated.Size = new Size(103, 13);
            this.notValidated.TabIndex = 1;
            this.notValidated.Text = "Select Not Validated";
            this.notValidated.Click += OnNotValidatedClicked;
            // 
            // AddressValidationStatusPopup
            // 
            this.Controls.Add(this.statusPanel);
            this.Size = new Size(316, 21);
            this.statusPanel.ResumeLayout(false);
            this.statusPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// Called when [not validated clicked].
        /// </summary>
        private void OnNotValidatedClicked(object sender, EventArgs e)
        {
            SelectStatuses(AddressSelector.NotValidated);
        }

        /// <summary>
        /// Called when [ready to go label clicked].
        /// </summary>
        private void OnReadyToGoLabelClicked(object sender, EventArgs e)
        {
            SelectStatuses(AddressSelector.ReadyToShip);
        }

        /// <summary>
        /// Selects the statuses specified and deselects the other statuses.
        /// </summary>
        /// <param name="addressValidationStatusTypes">The address validation status types to select.</param>
        public void SelectStatuses(List<AddressValidationStatusType> addressValidationStatusTypes)
        {
            ignoreChanged = true;

            foreach (var statusAndCheckbox in statusList)
            {
                statusAndCheckbox.Item1.Checked = addressValidationStatusTypes.Contains(statusAndCheckbox.Item2);
            }

            if (StatusChanged != null)
            {
                StatusChanged();
            }

            ignoreChanged = false;
        }

        /// <summary>
        /// Gets the selected statuses.
        /// </summary>
        public List<AddressValidationStatusType> GetSelectedStatuses()
        {
            return statusList
                .Where(s => s.Item1.Checked)
                .Select(s => s.Item2)
                .ToList();
        }

        /// <summary>
        /// Draw the item that the user has currently selected
        /// </summary>
        protected override void OnDrawSelectedItem(Graphics graphics, Color foreColor, Rectangle bounds)
        {
            // Pending's description is blank
            string text = string.Join(", ", GetSelectedStatuses().Select(m => m == AddressValidationStatusType.Pending ? "Pending" : EnumHelper.GetDescription(m)));

            if (statusList.All(s => s.Item1.Checked))
            {
                text = "All Statuses";
            }

            using (StringFormat stringFormat = new StringFormat())
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