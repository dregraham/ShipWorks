using ShipWorks.UI.Controls;
namespace ShipWorks.Shipping.Carriers.FedEx
{
    partial class FedExReturnsControl
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
            this.labelRmaNumber = new System.Windows.Forms.Label();
            this.rmaNumber = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.SuspendLayout();
            // 
            // labelRmaNumber
            // 
            this.labelRmaNumber.AutoSize = true;
            this.labelRmaNumber.Location = new System.Drawing.Point(47, 7);
            this.labelRmaNumber.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRmaNumber.Name = "labelRmaNumber";
            this.labelRmaNumber.Size = new System.Drawing.Size(93, 17);
            this.labelRmaNumber.TabIndex = 9;
            this.labelRmaNumber.Text = "RMA Number:";
            // 
            // rmaNumber
            // 
            this.rmaNumber.Location = new System.Drawing.Point(152, 4);
            this.rmaNumber.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rmaNumber.MaxLength = 30;
            this.rmaNumber.Name = "rmaNumber";
            this.rmaNumber.Size = new System.Drawing.Size(275, 24);
            this.rmaNumber.TabIndex = 10;
            // 
            // FedExReturnsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.rmaNumber);
            this.Controls.Add(this.labelRmaNumber);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FedExReturnsControl";
            this.Size = new System.Drawing.Size(433, 40);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelRmaNumber;
        private MultiValueTextBox rmaNumber;

    }
}
