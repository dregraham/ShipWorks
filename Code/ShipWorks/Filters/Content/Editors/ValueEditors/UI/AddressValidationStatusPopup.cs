using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.AddressValidation;
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
            Height = panelHeight + 42;
        }

        /// <summary>
        /// Binds the statuses.
        /// </summary>
        private void BindStatuses()
        {
            // Loop through all the statuses in AddressValidationStatusType.
            for (int statusIndex = 0; statusIndex < EnumHelper.GetEnumList<AddressValidationStatusType>().Count; statusIndex++)
            {
                var status = EnumHelper.GetEnumList<AddressValidationStatusType>()[statusIndex];

                // * 3 makes room for the label buttons.
                int verticlePosition = 23*(statusIndex + 3);

                // Build statusList with new checkboxes and related enums.
                var checkboxAndStatusType = new Tuple<CheckBox, AddressValidationStatusType>(new CheckBox
                {
                    Text = status.Key,
                    Location = new Point(3, verticlePosition),
                    Width = 150
                }, status.Value);

                checkboxAndStatusType.Item1.CheckedChanged += OnStatusCheckChanged;

                statusList.Add(checkboxAndStatusType);

                // Add checkbox to panel
                statusPanel.Controls.Add(checkboxAndStatusType.Item1);
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
            this.statusPanel = new System.Windows.Forms.Panel();
            this.readyToGoLabel = new System.Windows.Forms.Label();
            this.needsAttentionLabel = new System.Windows.Forms.Label();
            this.notValidated = new System.Windows.Forms.Label();
            this.statusPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusPanel
            // 
            this.statusPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.statusPanel.Controls.Add(this.readyToGoLabel);
            this.statusPanel.Controls.Add(this.needsAttentionLabel);
            this.statusPanel.Controls.Add(this.notValidated);
            this.statusPanel.Location = new System.Drawing.Point(0, 0);
            this.statusPanel.Name = "statusPanel";
            this.statusPanel.Size = new System.Drawing.Size(130, 306);
            this.statusPanel.TabIndex = 7;
            this.statusPanel.Visible = false;
            // 
            // readyToGoLabel
            // 
            this.readyToGoLabel.AutoSize = true;
            this.readyToGoLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.readyToGoLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.readyToGoLabel.ForeColor = System.Drawing.Color.Blue;
            this.readyToGoLabel.Location = new System.Drawing.Point(3, 3);
            this.readyToGoLabel.Name = "readyToGoLabel";
            this.readyToGoLabel.Size = new System.Drawing.Size(101, 13);
            this.readyToGoLabel.TabIndex = 1;
            this.readyToGoLabel.Text = "Select Ready To Go";
            this.readyToGoLabel.Click += OnReadyToGoLabelClicked;
            // 
            // needsAttentionLabel
            // 
            this.needsAttentionLabel.AutoSize = true;
            this.needsAttentionLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.needsAttentionLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.needsAttentionLabel.ForeColor = System.Drawing.Color.Blue;
            this.needsAttentionLabel.Location = new System.Drawing.Point(3, 26);
            this.needsAttentionLabel.Name = "needsAttentionLabel";
            this.needsAttentionLabel.Size = new System.Drawing.Size(117, 13);
            this.needsAttentionLabel.TabIndex = 1;
            this.needsAttentionLabel.Text = "Select Needs Attention";
            this.needsAttentionLabel.Click += OnNeedsAttentionLabelClicked;
            // 
            // notValidated
            // 
            this.notValidated.AutoSize = true;
            this.notValidated.Cursor = System.Windows.Forms.Cursors.Hand;
            this.notValidated.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notValidated.ForeColor = System.Drawing.Color.Blue;
            this.notValidated.Location = new System.Drawing.Point(3, 49);
            this.notValidated.Name = "notValidated";
            this.notValidated.Size = new System.Drawing.Size(103, 13);
            this.notValidated.TabIndex = 1;
            this.notValidated.Text = "Select Not Validated";
            this.notValidated.Click += OnNotValidatedClicked;
            // 
            // AddressValidationStatusPopup
            // 
            this.Controls.Add(this.statusPanel);
            this.Size = new System.Drawing.Size(316, 21);
            this.statusPanel.ResumeLayout(false);
            this.statusPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// Called when [not validated clicked].
        /// </summary>
        private void OnNotValidatedClicked(object sender, EventArgs e)
        {
            SelectStatuses(AddressValidationStatusType.Error, AddressValidationStatusType.NotChecked, AddressValidationStatusType.Pending, AddressValidationStatusType.WillNotValidate);
        }

        /// <summary>
        /// Called when [needs attention label clicked].
        /// </summary>
        private void OnNeedsAttentionLabelClicked(object sender, EventArgs e)
        {
            SelectStatuses(AddressValidationStatusType.HasSuggestions, AddressValidationStatusType.BadAddress, AddressValidationStatusType.Error, AddressValidationStatusType.WillNotValidate);
        }

        /// <summary>
        /// Called when [ready to go label clicked].
        /// </summary>
        private void OnReadyToGoLabelClicked(object sender, EventArgs e)
        {
            SelectStatuses(AddressValidationStatusType.Valid, AddressValidationStatusType.SuggestionIgnored, AddressValidationStatusType.Fixed);
        }

        /// <summary>
        /// Selects the statuses specified and deselects the other statuses.
        /// </summary>
        /// <param name="addressValidationStatusTypes">The address validation status types to select.</param>
        public void SelectStatuses(params AddressValidationStatusType[] addressValidationStatusTypes)
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
            string text = string.Join(", ", GetSelectedStatuses().Select(m => EnumHelper.GetDescription(m)));
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