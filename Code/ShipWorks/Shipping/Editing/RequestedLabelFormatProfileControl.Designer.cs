namespace ShipWorks.Shipping.Editing
{
    partial class RequestedLabelFormatProfileControl
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
            this.labelFormat = new System.Windows.Forms.ComboBox();
            this.labelFormatMessage = new System.Windows.Forms.Label();
            this.help = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.help)).BeginInit();
            this.SuspendLayout();
            // 
            // labelFormat
            // 
            this.labelFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.labelFormat.FormattingEnabled = true;
            this.labelFormat.Location = new System.Drawing.Point(125, 0);
            this.labelFormat.Name = "labelFormat";
            this.labelFormat.Size = new System.Drawing.Size(90, 21);
            this.labelFormat.TabIndex = 63;
            // 
            // labelFormatMessage
            // 
            this.labelFormatMessage.AutoSize = true;
            this.labelFormatMessage.Location = new System.Drawing.Point(0, 3);
            this.labelFormatMessage.Name = "labelFormatMessage";
            this.labelFormatMessage.Size = new System.Drawing.Size(126, 13);
            this.labelFormatMessage.TabIndex = 62;
            this.labelFormatMessage.Text = "Requested Label format:";
            // 
            // help
            // 
            this.help.BackColor = System.Drawing.Color.Transparent;
            this.help.Cursor = System.Windows.Forms.Cursors.Hand;
            this.help.Image = global::ShipWorks.Properties.Resources.help2_16;
            this.help.Location = new System.Drawing.Point(221, 3);
            this.help.Name = "help";
            this.help.Size = new System.Drawing.Size(16, 16);
            this.help.TabIndex = 64;
            this.help.TabStop = false;
            this.help.Click += new System.EventHandler(this.OnHelpClick);
            // 
            // RequestedLabelFormatProfileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.help);
            this.Controls.Add(this.labelFormat);
            this.Controls.Add(this.labelFormatMessage);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "RequestedLabelFormatProfileControl";
            this.Size = new System.Drawing.Size(265, 21);
            ((System.ComponentModel.ISupportInitialize)(this.help)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox labelFormat;
        private System.Windows.Forms.Label labelFormatMessage;
        private System.Windows.Forms.PictureBox help;

    }
}
