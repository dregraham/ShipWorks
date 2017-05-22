namespace ShipWorks.Shipping.Carriers.UPS
{
    partial class UpsDryIceControl
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
            this.labelRegulationSet = new System.Windows.Forms.Label();
            this.labelWeight = new System.Windows.Forms.Label();
            this.medicalUse = new System.Windows.Forms.CheckBox();
            this.labelDryIce = new System.Windows.Forms.Label();
            this.containsDryIce = new System.Windows.Forms.CheckBox();
            this.panelDryIceDetails = new System.Windows.Forms.Panel();
            this.regulationSet = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.weight = new ShipWorks.UI.Controls.WeightControl();
            this.panelDryIceDetails.SuspendLayout();
            this.SuspendLayout();
            //
            // labelRegulationSet
            //
            this.labelRegulationSet.AutoSize = true;
            this.labelRegulationSet.Location = new System.Drawing.Point(3, 3);
            this.labelRegulationSet.Name = "labelRegulationSet";
            this.labelRegulationSet.Size = new System.Drawing.Size(80, 13);
            this.labelRegulationSet.TabIndex = 0;
            this.labelRegulationSet.Text = "Regulation Set:";
            this.labelRegulationSet.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // labelWeight
            //
            this.labelWeight.AutoSize = true;
            this.labelWeight.BackColor = System.Drawing.Color.Transparent;
            this.labelWeight.Location = new System.Drawing.Point(39, 31);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(44, 13);
            this.labelWeight.TabIndex = 4;
            this.labelWeight.Text = "Weight:";
            //
            // medicalUse
            //
            this.medicalUse.AutoSize = true;
            this.medicalUse.Location = new System.Drawing.Point(190, 2);
            this.medicalUse.Name = "medicalUse";
            this.medicalUse.Size = new System.Drawing.Size(85, 17);
            this.medicalUse.TabIndex = 2;
            this.medicalUse.Text = "Medical Use";
            this.medicalUse.UseVisualStyleBackColor = true;
            this.medicalUse.CheckedChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            //
            // labelDryIce
            //
            this.labelDryIce.AutoSize = true;
            this.labelDryIce.Location = new System.Drawing.Point(3, 4);
            this.labelDryIce.Name = "labelDryIce";
            this.labelDryIce.Size = new System.Drawing.Size(44, 13);
            this.labelDryIce.TabIndex = 9;
            this.labelDryIce.Text = "Dry Ice:";
            this.labelDryIce.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // containsDryIce
            //
            this.containsDryIce.AutoSize = true;
            this.containsDryIce.Location = new System.Drawing.Point(53, 3);
            this.containsDryIce.Name = "containsDryIce";
            this.containsDryIce.Size = new System.Drawing.Size(146, 17);
            this.containsDryIce.TabIndex = 0;
            this.containsDryIce.Text = "Package contains dry ice";
            this.containsDryIce.UseVisualStyleBackColor = true;
            this.containsDryIce.CheckedChanged += new System.EventHandler(this.OnContainsDryIceChanged);
            //
            // panelDryIceDetails
            //
            this.panelDryIceDetails.Controls.Add(this.regulationSet);
            this.panelDryIceDetails.Controls.Add(this.labelRegulationSet);
            this.panelDryIceDetails.Controls.Add(this.labelWeight);
            this.panelDryIceDetails.Controls.Add(this.weight);
            this.panelDryIceDetails.Controls.Add(this.medicalUse);
            this.panelDryIceDetails.Location = new System.Drawing.Point(46, 26);
            this.panelDryIceDetails.Name = "panelDryIceDetails";
            this.panelDryIceDetails.Size = new System.Drawing.Size(290, 51);
            this.panelDryIceDetails.TabIndex = 11;
            //
            // regulationSet
            //
            this.regulationSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.regulationSet.FormattingEnabled = true;
            this.regulationSet.Location = new System.Drawing.Point(89, 0);
            this.regulationSet.Name = "regulationSet";
            this.regulationSet.PromptText = "(Multiple Values)";
            this.regulationSet.Size = new System.Drawing.Size(95, 21);
            this.regulationSet.TabIndex = 1;
            this.regulationSet.SelectedIndexChanged += new System.EventHandler(this.OnRegulationSetChanged);
            //
            // weight
            //
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.weight.Location = new System.Drawing.Point(89, 27);
            this.weight.Name = "weight";
            this.weight.RangeMax = 1000D;
            this.weight.RangeMin = 0D;
            this.weight.Size = new System.Drawing.Size(269, 21);
            this.weight.TabIndex = 3;
            this.weight.Weight = 0D;
            this.weight.WeightChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            //
            // UpsDryIceControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelDryIceDetails);
            this.Controls.Add(this.containsDryIce);
            this.Controls.Add(this.labelDryIce);
            this.Name = "UpsDryIceControl";
            this.Size = new System.Drawing.Size(336, 80);
            this.panelDryIceDetails.ResumeLayout(false);
            this.panelDryIceDetails.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelRegulationSet;
        private UI.Controls.WeightControl weight;
        private System.Windows.Forms.Label labelWeight;
        private System.Windows.Forms.CheckBox medicalUse;
        private ShipWorks.UI.Controls.MultiValueComboBox regulationSet;
        private System.Windows.Forms.Label labelDryIce;
        private System.Windows.Forms.CheckBox containsDryIce;
        private System.Windows.Forms.Panel panelDryIceDetails;
    }
}
