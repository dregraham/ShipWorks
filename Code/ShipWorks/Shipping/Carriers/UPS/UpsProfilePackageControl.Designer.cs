namespace ShipWorks.Shipping.Carriers.UPS
{
    partial class UpsProfilePackageControl
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
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.verbalConfirmationDetails = new ShipWorks.Shipping.Carriers.UPS.UpsContactInfoControl();
            this.verbalConfirmationState = new System.Windows.Forms.CheckBox();
            this.dryIceState = new System.Windows.Forms.CheckBox();
            this.dryIceControl = new ShipWorks.Shipping.Carriers.UPS.UpsDryIceControl();
            this.additionalHandling = new System.Windows.Forms.CheckBox();
            this.labelAdditionalHandling = new System.Windows.Forms.Label();
            this.additionalHandlingState = new System.Windows.Forms.CheckBox();
            this.labelPackaging = new System.Windows.Forms.Label();
            this.packagingType = new System.Windows.Forms.ComboBox();
            this.packagingState = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.dimensionsState = new System.Windows.Forms.CheckBox();
            this.weightState = new System.Windows.Forms.CheckBox();
            this.weight = new ShipWorks.UI.Controls.WeightControl();
            this.labelWeight = new System.Windows.Forms.Label();
            this.labelDimensions = new System.Windows.Forms.Label();
            this.dimensionsControl = new ShipWorks.Shipping.Editing.DimensionsControl();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            //
            // groupBox
            //
            this.groupBox.Controls.Add(this.verbalConfirmationDetails);
            this.groupBox.Controls.Add(this.verbalConfirmationState);
            this.groupBox.Controls.Add(this.dryIceState);
            this.groupBox.Controls.Add(this.dryIceControl);
            this.groupBox.Controls.Add(this.additionalHandling);
            this.groupBox.Controls.Add(this.labelAdditionalHandling);
            this.groupBox.Controls.Add(this.additionalHandlingState);
            this.groupBox.Controls.Add(this.labelPackaging);
            this.groupBox.Controls.Add(this.packagingType);
            this.groupBox.Controls.Add(this.packagingState);
            this.groupBox.Controls.Add(this.kryptonBorderEdge);
            this.groupBox.Controls.Add(this.dimensionsState);
            this.groupBox.Controls.Add(this.weightState);
            this.groupBox.Controls.Add(this.weight);
            this.groupBox.Controls.Add(this.labelWeight);
            this.groupBox.Controls.Add(this.labelDimensions);
            this.groupBox.Controls.Add(this.dimensionsControl);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(423, 351);
            this.groupBox.TabIndex = 1;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Package {0}";
            //
            // verbalConfirmationDetails
            //
            this.verbalConfirmationDetails.ContactName = "";
            this.verbalConfirmationDetails.Location = new System.Drawing.Point(33, 259);
            this.verbalConfirmationDetails.Name = "verbalConfirmationDetails";
            this.verbalConfirmationDetails.PhoneExtension = "";
            this.verbalConfirmationDetails.PhoneNumber = "";
            this.verbalConfirmationDetails.Size = new System.Drawing.Size(374, 86);
            this.verbalConfirmationDetails.State = false;
            this.verbalConfirmationDetails.TabIndex = 11;
            //
            // verbalConfirmationState
            //
            this.verbalConfirmationState.AutoSize = true;
            this.verbalConfirmationState.Checked = true;
            this.verbalConfirmationState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.verbalConfirmationState.Location = new System.Drawing.Point(8, 259);
            this.verbalConfirmationState.Name = "verbalConfirmationState";
            this.verbalConfirmationState.Size = new System.Drawing.Size(15, 14);
            this.verbalConfirmationState.TabIndex = 10;
            this.verbalConfirmationState.Tag = "";
            this.verbalConfirmationState.UseVisualStyleBackColor = true;
            //
            // dryIceState
            //
            this.dryIceState.AutoSize = true;
            this.dryIceState.Checked = true;
            this.dryIceState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dryIceState.Location = new System.Drawing.Point(8, 175);
            this.dryIceState.Name = "dryIceState";
            this.dryIceState.Size = new System.Drawing.Size(15, 14);
            this.dryIceState.TabIndex = 8;
            this.dryIceState.Tag = "";
            this.dryIceState.UseVisualStyleBackColor = true;
            //
            // dryIceControl
            //
            this.dryIceControl.Location = new System.Drawing.Point(90, 173);
            this.dryIceControl.Name = "dryIceControl";
            this.dryIceControl.Size = new System.Drawing.Size(348, 80);
            this.dryIceControl.State = false;
            this.dryIceControl.TabIndex = 9;
            //
            // additionalHandling
            //
            this.additionalHandling.AutoSize = true;
            this.additionalHandling.Location = new System.Drawing.Point(143, 154);
            this.additionalHandling.Name = "additionalHandling";
            this.additionalHandling.Size = new System.Drawing.Size(117, 17);
            this.additionalHandling.TabIndex = 7;
            this.additionalHandling.Text = "This package requires additional handling";
            this.additionalHandling.UseVisualStyleBackColor = true;
            //
            // labelAdditionalHandling
            //
            this.labelAdditionalHandling.AutoSize = true;
            this.labelAdditionalHandling.Location = new System.Drawing.Point(35, 155);
            this.labelAdditionalHandling.Name = "labelAdditionalHandling";
            this.labelAdditionalHandling.Size = new System.Drawing.Size(102, 13);
            this.labelAdditionalHandling.TabIndex = 89;
            this.labelAdditionalHandling.Text = "Additional Handling:";
            this.labelAdditionalHandling.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // additionalHandlingState
            //
            this.additionalHandlingState.AutoSize = true;
            this.additionalHandlingState.Checked = true;
            this.additionalHandlingState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.additionalHandlingState.Location = new System.Drawing.Point(8, 155);
            this.additionalHandlingState.Name = "additionalHandlingState";
            this.additionalHandlingState.Size = new System.Drawing.Size(15, 14);
            this.additionalHandlingState.TabIndex = 6;
            this.additionalHandlingState.Tag = "";
            this.additionalHandlingState.UseVisualStyleBackColor = true;
            //
            // labelPackaging
            //
            this.labelPackaging.AutoSize = true;
            this.labelPackaging.BackColor = System.Drawing.Color.Transparent;
            this.labelPackaging.Location = new System.Drawing.Point(78, 24);
            this.labelPackaging.Name = "labelPackaging";
            this.labelPackaging.Size = new System.Drawing.Size(59, 13);
            this.labelPackaging.TabIndex = 86;
            this.labelPackaging.Text = "Packaging:";
            //
            // packagingType
            //
            this.packagingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.packagingType.FormattingEnabled = true;
            this.packagingType.Location = new System.Drawing.Point(143, 21);
            this.packagingType.Name = "packagingType";
            this.packagingType.Size = new System.Drawing.Size(145, 21);
            this.packagingType.TabIndex = 1;
            //
            // packagingState
            //
            this.packagingState.AutoSize = true;
            this.packagingState.Checked = true;
            this.packagingState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.packagingState.Location = new System.Drawing.Point(8, 24);
            this.packagingState.Name = "packagingState";
            this.packagingState.Size = new System.Drawing.Size(15, 14);
            this.packagingState.TabIndex = 0;
            this.packagingState.Tag = "";
            this.packagingState.UseVisualStyleBackColor = true;
            //
            // kryptonBorderEdge
            //
            this.kryptonBorderEdge.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge.AutoSize = false;
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(28, 22);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(1, 314);
            this.kryptonBorderEdge.TabIndex = 83;
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            //
            // dimensionsState
            //
            this.dimensionsState.AutoSize = true;
            this.dimensionsState.Checked = true;
            this.dimensionsState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dimensionsState.Location = new System.Drawing.Point(8, 79);
            this.dimensionsState.Name = "dimensionsState";
            this.dimensionsState.Size = new System.Drawing.Size(15, 14);
            this.dimensionsState.TabIndex = 4;
            this.dimensionsState.Tag = "";
            this.dimensionsState.UseVisualStyleBackColor = true;
            //
            // weightState
            //
            this.weightState.AutoSize = true;
            this.weightState.Checked = true;
            this.weightState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.weightState.Location = new System.Drawing.Point(8, 52);
            this.weightState.Name = "weightState";
            this.weightState.Size = new System.Drawing.Size(15, 14);
            this.weightState.TabIndex = 2;
            this.weightState.Tag = "";
            this.weightState.UseVisualStyleBackColor = true;
            //
            // weight
            //
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.weight.Location = new System.Drawing.Point(144, 49);
            this.weight.Name = "weight";
            this.weight.RangeMax = 300D;
            this.weight.RangeMin = 0D;
            this.weight.Size = new System.Drawing.Size(269, 21);
            this.weight.TabIndex = 3;
            this.weight.Weight = 0D;
            //
            // labelWeight
            //
            this.labelWeight.AutoSize = true;
            this.labelWeight.BackColor = System.Drawing.Color.Transparent;
            this.labelWeight.Location = new System.Drawing.Point(92, 52);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(45, 13);
            this.labelWeight.TabIndex = 75;
            this.labelWeight.Text = "Weight:";
            //
            // labelDimensions
            //
            this.labelDimensions.AutoSize = true;
            this.labelDimensions.BackColor = System.Drawing.Color.Transparent;
            this.labelDimensions.Location = new System.Drawing.Point(73, 80);
            this.labelDimensions.Name = "labelDimensions";
            this.labelDimensions.Size = new System.Drawing.Size(64, 13);
            this.labelDimensions.TabIndex = 77;
            this.labelDimensions.Text = "Dimensions:";
            //
            // dimensionsControl
            //
            this.dimensionsControl.BackColor = System.Drawing.Color.Transparent;
            this.dimensionsControl.Cleared = false;
            this.dimensionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.dimensionsControl.Location = new System.Drawing.Point(140, 74);
            this.dimensionsControl.Name = "dimensionsControl";
            this.dimensionsControl.Size = new System.Drawing.Size(210, 74);
            this.dimensionsControl.TabIndex = 5;
            //
            // UpsProfilePackageControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "UpsProfilePackageControl";
            this.Size = new System.Drawing.Size(423, 351);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        private System.Windows.Forms.CheckBox dimensionsState;
        private System.Windows.Forms.CheckBox weightState;
        private ShipWorks.UI.Controls.WeightControl weight;
        private System.Windows.Forms.Label labelWeight;
        private System.Windows.Forms.Label labelDimensions;
        private ShipWorks.Shipping.Editing.DimensionsControl dimensionsControl;
        private System.Windows.Forms.ComboBox packagingType;
        private System.Windows.Forms.CheckBox packagingState;
        private System.Windows.Forms.Label labelPackaging;
        private System.Windows.Forms.CheckBox additionalHandlingState;
        private System.Windows.Forms.CheckBox additionalHandling;
        private System.Windows.Forms.Label labelAdditionalHandling;
        private System.Windows.Forms.CheckBox verbalConfirmationState;
        private System.Windows.Forms.CheckBox dryIceState;
        private UpsDryIceControl dryIceControl;
        private UpsContactInfoControl verbalConfirmationDetails;
    }
}
