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
            this.weight = new ShipWorks.UI.Controls.WeightControl();
            this.labelWeight = new System.Windows.Forms.Label();
            this.medicalUse = new System.Windows.Forms.CheckBox();
            this.regulationSet = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // labelRegulationSet
            // 
            this.labelRegulationSet.AutoSize = true;
            this.labelRegulationSet.Location = new System.Drawing.Point(3, 5);
            this.labelRegulationSet.Name = "labelRegulationSet";
            this.labelRegulationSet.Size = new System.Drawing.Size(80, 13);
            this.labelRegulationSet.TabIndex = 0;
            this.labelRegulationSet.Text = "Regulation Set:";
            this.labelRegulationSet.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // weight
            // 
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weight.Location = new System.Drawing.Point(89, 29);
            this.weight.Name = "weight";
            this.weight.RangeMax = 1000D;
            this.weight.RangeMin = 0D;
            this.weight.Size = new System.Drawing.Size(218, 21);
            this.weight.TabIndex = 5;
            this.weight.Weight = 0D;
            // 
            // labelWeight
            // 
            this.labelWeight.AutoSize = true;
            this.labelWeight.BackColor = System.Drawing.Color.Transparent;
            this.labelWeight.Location = new System.Drawing.Point(39, 32);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(44, 13);
            this.labelWeight.TabIndex = 4;
            this.labelWeight.Text = "Weight:";
            // 
            // medicalUse
            // 
            this.medicalUse.AutoSize = true;
            this.medicalUse.Location = new System.Drawing.Point(227, 4);
            this.medicalUse.Name = "medicalUse";
            this.medicalUse.Size = new System.Drawing.Size(85, 17);
            this.medicalUse.TabIndex = 7;
            this.medicalUse.Text = "Medical Use";
            this.medicalUse.UseVisualStyleBackColor = true;
            // 
            // regulationSet
            // 
            this.regulationSet.FormattingEnabled = true;
            this.regulationSet.Location = new System.Drawing.Point(89, 2);
            this.regulationSet.Name = "regulationSet";
            this.regulationSet.Size = new System.Drawing.Size(121, 21);
            this.regulationSet.TabIndex = 8;
            this.regulationSet.SelectedIndexChanged += new System.EventHandler(this.OnRegulationSetChanged);
            // 
            // UpsDryIceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.regulationSet);
            this.Controls.Add(this.medicalUse);
            this.Controls.Add(this.weight);
            this.Controls.Add(this.labelWeight);
            this.Controls.Add(this.labelRegulationSet);
            this.Name = "UpsDryIceControl";
            this.Size = new System.Drawing.Size(318, 56);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelRegulationSet;
        private UI.Controls.WeightControl weight;
        private System.Windows.Forms.Label labelWeight;
        private System.Windows.Forms.CheckBox medicalUse;
        private System.Windows.Forms.ComboBox regulationSet;
    }
}
