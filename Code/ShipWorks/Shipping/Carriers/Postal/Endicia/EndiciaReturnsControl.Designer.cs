using ShipWorks.UI.Controls;
namespace ShipWorks.Shipping.Carriers.Endicia
{
    partial class EndiciaReturnsControl
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
            this.scanBasedReturn = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // scanBasedReturn
            // 
            this.scanBasedReturn.AutoSize = true;
            this.scanBasedReturn.Location = new System.Drawing.Point(9, 3);
            this.scanBasedReturn.Name = "scanBasedReturn";
            this.scanBasedReturn.Size = new System.Drawing.Size(162, 17);
            this.scanBasedReturn.TabIndex = 14;
            this.scanBasedReturn.Text = "Scan Based Payment Return";
            this.scanBasedReturn.UseVisualStyleBackColor = true;
            // 
            // EndiciaReturnsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.scanBasedReturn);
            this.Name = "EndiciaReturnsControl";
            this.Size = new System.Drawing.Size(325, 22);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox scanBasedReturn;
    }
}
