﻿using System;
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
            // Loop through all the months in MonthTypeEnum.
            for (int statusIndex = 0; statusIndex < EnumHelper.GetEnumList<AddressValidationStatusType>().Count; statusIndex++)
            {
                var month = EnumHelper.GetEnumList<AddressValidationStatusType>()[statusIndex];

                // * 3 makes room for the label buttons.
                int verticlePosition = 23*(statusIndex + 3);

                // Build monthsList with new checkboxes and related enums.
                var checkboxAndMonthType = new Tuple<CheckBox, AddressValidationStatusType>(new CheckBox
                {
                    Text = month.Key,
                    Location = new Point(3, verticlePosition)
                }, month.Value);

                checkboxAndMonthType.Item1.CheckedChanged += OnStatusCheckChanged;

                statusList.Add(checkboxAndMonthType);

                // Add checkbox to panel
                statusPanel.Controls.Add(checkboxAndMonthType.Item1);
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
            statusPanel = new Panel();
            readyToGoLabel = new Label();
            needsAttentionLabel = new Label();
            notValidated = new Label();
            statusPanel.SuspendLayout();
            SuspendLayout();
            // 
            // onTheMonthsPanel
            // 
            statusPanel.BackColor = SystemColors.ControlLightLight;
            statusPanel.Controls.Add(readyToGoLabel);
            statusPanel.Controls.Add(needsAttentionLabel);
            statusPanel.Controls.Add(notValidated);
            statusPanel.Name = "statusPanel";
            statusPanel.Size = new Size(130, 306);
            statusPanel.TabIndex = 7;
            statusPanel.Visible = false;

            //
            // readyToGoLabel 
            // 
            readyToGoLabel.AutoSize = true;
            readyToGoLabel.Location = new Point(3, 3);
            readyToGoLabel.Name = "readyToGoLabel";
            readyToGoLabel.Size = new Size(120, 17);
            readyToGoLabel.TabIndex = 1;
            readyToGoLabel.Text = "Select Ready To Go";
            readyToGoLabel.Click += OnReadyToGoLabelClicked;
            readyToGoLabel.Cursor = Cursors.Hand;
            readyToGoLabel.Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            readyToGoLabel.ForeColor = Color.Blue;
            
            //
            // needsAttentionLabel 
            // 
            needsAttentionLabel.AutoSize = true;
            needsAttentionLabel.Location = new Point(3, 26);
            needsAttentionLabel.Name = "needsAttentionLabel";
            needsAttentionLabel.Size = new Size(120, 17);
            needsAttentionLabel.TabIndex = 1;
            needsAttentionLabel.Text = "Select Needs Attention";
            needsAttentionLabel.Click += OnNeedsAttentionLabelClicked;
            needsAttentionLabel.Cursor = Cursors.Hand;
            needsAttentionLabel.Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            needsAttentionLabel.ForeColor = Color.Blue;

            //
            // notValidated 
            // 
            notValidated.AutoSize = true;
            notValidated.Location = new Point(3, 49);
            notValidated.Name = "notValidated";
            notValidated.Size = new Size(120, 17);
            notValidated.TabIndex = 1;
            notValidated.Text = "Select Not Validated";
            notValidated.Click += OnNotValidatedClicked;
            notValidated.Cursor = Cursors.Hand;
            notValidated.Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            notValidated.ForeColor = Color.Blue;

            //
            // AddressValidationStatusPopup
            //
            Controls.Add(statusPanel);
            Name = "AddressValidationStatusPopup";
            Size = new Size(316, 348);
            statusPanel.ResumeLayout(false);
            statusPanel.PerformLayout();
            ResumeLayout(false);
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
            SelectStatuses(AddressValidationStatusType.NeedsAttention, AddressValidationStatusType.NotValid, AddressValidationStatusType.Error, AddressValidationStatusType.WillNotValidate);
        }

        /// <summary>
        /// Called when [ready to go label clicked].
        /// </summary>
        private void OnReadyToGoLabelClicked(object sender, EventArgs e)
        {
            SelectStatuses(AddressValidationStatusType.Valid, AddressValidationStatusType.Overridden, AddressValidationStatusType.Adjusted);
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