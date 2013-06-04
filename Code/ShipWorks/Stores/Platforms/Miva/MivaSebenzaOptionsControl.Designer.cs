namespace ShipWorks.Stores.Platforms.Miva
{
    partial class MivaSebenzaOptionsControl
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
            this.sebenzaAddtionalCheckout = new System.Windows.Forms.CheckBox();
            this.infotipACD = new ShipWorks.UI.Controls.InfoTip();
            this.SuspendLayout();
            // 
            // sebenzaAddtionalCheckout
            // 
            this.sebenzaAddtionalCheckout.AutoSize = true;
            this.sebenzaAddtionalCheckout.Location = new System.Drawing.Point(3, 3);
            this.sebenzaAddtionalCheckout.Name = "sebenzaAddtionalCheckout";
            this.sebenzaAddtionalCheckout.Size = new System.Drawing.Size(381, 17);
            this.sebenzaAddtionalCheckout.TabIndex = 0;
            this.sebenzaAddtionalCheckout.Text = "Download extra order information from Sebenza Additional Checkout Data";
            this.sebenzaAddtionalCheckout.UseVisualStyleBackColor = true;
            // 
            // infotipACD
            // 
            this.infotipACD.Caption = "Each additional checkout data will be saved as a note for the order, prefixed wit" +
                "h \"ACD: \" at the beginning of the note.";
            this.infotipACD.Location = new System.Drawing.Point(381, 5);
            this.infotipACD.Name = "infotipACD";
            this.infotipACD.Size = new System.Drawing.Size(12, 12);
            this.infotipACD.TabIndex = 21;
            this.infotipACD.Title = "Additional Checkout Data";
            // 
            // MivaSebenzaOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.infotipACD);
            this.Controls.Add(this.sebenzaAddtionalCheckout);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MivaSebenzaOptionsControl";
            this.Size = new System.Drawing.Size(406, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox sebenzaAddtionalCheckout;
        private UI.Controls.InfoTip infotipACD;
    }
}
