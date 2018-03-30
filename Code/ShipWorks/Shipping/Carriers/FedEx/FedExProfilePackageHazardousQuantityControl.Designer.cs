using System.Windows.Forms;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    partial class FedExProfilePackageHazardousQuantityControl
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
            this.unit = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.quantity = new ShipWorks.UI.Controls.DecimalTextBox();
            this.SuspendLayout();
            // 
            // unit
            // 
            this.unit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.unit.FormattingEnabled = true;
            this.unit.Location = new System.Drawing.Point(95, 0);
            this.unit.Name = "unit";
            this.unit.PromptText = "(Multiple Values)";
            this.unit.Size = new System.Drawing.Size(76, 21);
            this.unit.TabIndex = 1;
            // 
            // quantity
            // 
            this.quantity.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.quantity.Location = new System.Drawing.Point(0, 0);
            this.quantity.Name = "quantity";
            this.quantity.Size = new System.Drawing.Size(89, 21);
            this.quantity.TabIndex = 0;
            // 
            // FedExProfilePackageHazardousQuantityControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.unit);
            this.Controls.Add(this.quantity);
            this.Name = "FedExProfilePackageHazardousQuantityControl";
            this.Size = new System.Drawing.Size(193, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Controls.MultiValueComboBox unit;
        private DecimalTextBox quantity;
    }
}
