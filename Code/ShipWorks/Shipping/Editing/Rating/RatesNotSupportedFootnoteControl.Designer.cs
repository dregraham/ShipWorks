namespace ShipWorks.Shipping.Editing.Rating
{
    partial class RatesNotSupportedFootnoteControl
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
            this.errorMessage = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.selectCarrier = new ShipWorks.Filters.Content.Editors.ChoiceLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // errorMessage
            // 
            this.errorMessage.AutoSize = true;
            this.errorMessage.Location = new System.Drawing.Point(25, 7);
            this.errorMessage.Name = "errorMessage";
            this.errorMessage.Size = new System.Drawing.Size(185, 13);
            this.errorMessage.TabIndex = 9;
            this.errorMessage.Text = "Select a different carrier to Get Rates:";
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.information16;
            this.pictureBox.Location = new System.Drawing.Point(4, 5);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(16, 16);
            this.pictureBox.TabIndex = 11;
            this.pictureBox.TabStop = false;
            // 
            // selectCarrier
            // 
            this.selectCarrier.AutoSize = true;
            this.selectCarrier.Cursor = System.Windows.Forms.Cursors.Hand;
            this.selectCarrier.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectCarrier.ForeColor = System.Drawing.Color.Green;
            this.selectCarrier.Location = new System.Drawing.Point(216, 7);
            this.selectCarrier.Name = "selectCarrier";
            this.selectCarrier.Size = new System.Drawing.Size(82, 13);
            this.selectCarrier.TabIndex = 12;
            this.selectCarrier.Text = "Select A Carrier";
            this.selectCarrier.SelectedValueChanged += new System.EventHandler(this.OnSelectCarrierChanged);
            // 
            // RatesNotSupportedFootnoteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.selectCarrier);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.errorMessage);
            this.Name = "RatesNotSupportedFootnoteControl";
            this.Size = new System.Drawing.Size(396, 28);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label errorMessage;
        private Filters.Content.Editors.ChoiceLabel selectCarrier;
    }
}
