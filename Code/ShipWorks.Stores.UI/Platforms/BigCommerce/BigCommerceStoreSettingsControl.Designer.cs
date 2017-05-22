using System.Drawing;

namespace ShipWorks.Stores.UI.Platforms.BigCommerce
{
    partial class BigCommerceStoreSettingsControl
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
            this.labelWeightUnitOfMeasure = new System.Windows.Forms.Label();
            this.weightUnitOfMeasure = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // labelWeightUnitOfMeasure
            // 
            this.labelWeightUnitOfMeasure.AutoSize = true;
            this.labelWeightUnitOfMeasure.Location = new System.Drawing.Point(17, 22);
            this.labelWeightUnitOfMeasure.Name = "labelWeightUnitOfMeasure";
            this.labelWeightUnitOfMeasure.Size = new System.Drawing.Size(202, 13);
            this.labelWeightUnitOfMeasure.TabIndex = 20;
            this.labelWeightUnitOfMeasure.Text = "Weights are entered in BigCommerce as:";
            // 
            // weightUnitOfMeasure
            // 
            this.weightUnitOfMeasure.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.weightUnitOfMeasure.FormattingEnabled = true;
            this.weightUnitOfMeasure.Location = new System.Drawing.Point(221, 20);
            this.weightUnitOfMeasure.Name = "weightUnitOfMeasure";
            this.weightUnitOfMeasure.Size = new System.Drawing.Size(106, 21);
            this.weightUnitOfMeasure.TabIndex = 21;
            // 
            // BigCommerceStoreSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.weightUnitOfMeasure);
            this.Controls.Add(this.labelWeightUnitOfMeasure);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "BigCommerceStoreSettingsControl";
            this.Size = new System.Drawing.Size(489, 58);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelWeightUnitOfMeasure;
        private System.Windows.Forms.ComboBox weightUnitOfMeasure;
    }
}
