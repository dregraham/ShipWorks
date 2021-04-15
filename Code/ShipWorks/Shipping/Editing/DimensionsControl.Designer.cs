using static ShipWorks.UI.Controls.WeightControl;

namespace ShipWorks.Shipping.Editing
{
    partial class DimensionsControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.addToWeight = new System.Windows.Forms.CheckBox();
            this.labelInches = new System.Windows.Forms.Label();
            this.weight = new ShipWorks.UI.Controls.WeightControl();
            this.linkManageProfiles = new ShipWorks.UI.Controls.LinkControl();
            this.profiles = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.height = new ShipWorks.UI.Controls.PromptTextBox();
            this.width = new ShipWorks.UI.Controls.PromptTextBox();
            this.length = new ShipWorks.UI.Controls.PromptTextBox();
            this.SuspendLayout();
            // 
            // addToWeight
            // 
            this.addToWeight.AutoSize = true;
            this.addToWeight.BackColor = System.Drawing.Color.Transparent;
            this.addToWeight.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.addToWeight.ForeColor = System.Drawing.Color.DimGray;
            this.addToWeight.Location = new System.Drawing.Point(0, 53);
            this.addToWeight.Name = "addToWeight";
            this.addToWeight.Size = new System.Drawing.Size(93, 17);
            this.addToWeight.TabIndex = 6;
            this.addToWeight.Text = "Add to weight";
            this.addToWeight.UseVisualStyleBackColor = false;
            this.addToWeight.CheckedChanged += new System.EventHandler(this.OnDimensionsChanged);
            // 
            // labelInches
            // 
            this.labelInches.AutoSize = true;
            this.labelInches.BackColor = System.Drawing.Color.Transparent;
            this.labelInches.Location = new System.Drawing.Point(123, 30);
            this.labelInches.Name = "labelInches";
            this.labelInches.Size = new System.Drawing.Size(19, 13);
            this.labelInches.TabIndex = 5;
            this.labelInches.Text = "in.";
            // 
            // weight
            // 
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.weight.Location = new System.Drawing.Point(96, 50);
            this.weight.Name = "weight";
            this.weight.RangeMax = 3000;
            this.weight.RangeMin = 0;
            this.weight.ShowWeighButton = false;
            this.weight.Size = new System.Drawing.Size(87, 24);
            this.weight.TabIndex = 7;
            this.weight.Weight = 0;
            this.weight.WeightChanged += new System.EventHandler<WeightChangedEventArgs>(this.OnDimensionsChanged);
            // 
            // linkManageProfiles
            // 
            this.linkManageProfiles.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkManageProfiles.AutoSize = true;
            this.linkManageProfiles.BackColor = System.Drawing.Color.Transparent;
            this.linkManageProfiles.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkManageProfiles.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkManageProfiles.ForeColor = System.Drawing.Color.Blue;
            this.linkManageProfiles.Location = new System.Drawing.Point(150, 6);
            this.linkManageProfiles.Name = "linkManageProfiles";
            this.linkManageProfiles.Size = new System.Drawing.Size(57, 13);
            this.linkManageProfiles.TabIndex = 1;
            this.linkManageProfiles.Text = "Manage...";
            this.linkManageProfiles.Click += new System.EventHandler(this.OnManageProfiles);
            // 
            // profiles
            // 
            this.profiles.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.profiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.profiles.FormattingEnabled = true;
            this.profiles.Items.AddRange(new object[] {
            "Enter dimensions"});
            this.profiles.Location = new System.Drawing.Point(3, 3);
            this.profiles.Name = "profiles";
            this.profiles.Size = new System.Drawing.Size(144, 21);
            this.profiles.TabIndex = 0;
            // 
            // height
            // 
            this.height.Location = new System.Drawing.Point(85, 27);
            this.height.Name = "height";
            this.height.PromptColor = System.Drawing.SystemColors.GrayText;
            this.height.PromptText = "H";
            this.height.Size = new System.Drawing.Size(35, 21);
            this.height.TabIndex = 4;
            this.height.TextChanged += new System.EventHandler(this.OnDimensionsChanged);
            // 
            // width
            // 
            this.width.Location = new System.Drawing.Point(44, 27);
            this.width.Name = "width";
            this.width.PromptColor = System.Drawing.SystemColors.GrayText;
            this.width.PromptText = "W";
            this.width.Size = new System.Drawing.Size(35, 21);
            this.width.TabIndex = 3;
            this.width.TextChanged += new System.EventHandler(this.OnDimensionsChanged);
            // 
            // length
            // 
            this.length.Location = new System.Drawing.Point(3, 27);
            this.length.Name = "length";
            this.length.PromptColor = System.Drawing.SystemColors.GrayText;
            this.length.PromptText = "L";
            this.length.Size = new System.Drawing.Size(35, 21);
            this.length.TabIndex = 2;
            this.length.TextChanged += new System.EventHandler(this.OnDimensionsChanged);
            // 
            // DimensionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.weight);
            this.Controls.Add(this.addToWeight);
            this.Controls.Add(this.linkManageProfiles);
            this.Controls.Add(this.profiles);
            this.Controls.Add(this.labelInches);
            this.Controls.Add(this.height);
            this.Controls.Add(this.width);
            this.Controls.Add(this.length);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "DimensionsControl";
            this.Size = new System.Drawing.Size(210, 77);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox addToWeight;
        private ShipWorks.UI.Controls.LinkControl linkManageProfiles;
        private ShipWorks.UI.Controls.MultiValueComboBox profiles;
        private System.Windows.Forms.Label labelInches;
        private ShipWorks.UI.Controls.PromptTextBox height;
        private ShipWorks.UI.Controls.PromptTextBox width;
        private ShipWorks.UI.Controls.PromptTextBox length;
        private ShipWorks.UI.Controls.WeightControl weight;
    }
}
