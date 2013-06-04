namespace ShipWorks.Templates.Printing
{
    partial class PrinterConfigurationErrorDlg
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrinterConfigurationErrorDlg));
            this.close = new System.Windows.Forms.Button();
            this.helpLocalPort = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.printerName = new System.Windows.Forms.Label();
            this.labelPrinter = new System.Windows.Forms.Label();
            this.labelError = new System.Windows.Forms.Label();
            this.pictureError = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize) (this.pictureError)).BeginInit();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(403, 257);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 0;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // helpLocalPort
            // 
            this.helpLocalPort.AutoSize = true;
            this.helpLocalPort.BackColor = System.Drawing.Color.Transparent;
            this.helpLocalPort.Cursor = System.Windows.Forms.Cursors.Hand;
            this.helpLocalPort.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.helpLocalPort.ForeColor = System.Drawing.Color.Blue;
            this.helpLocalPort.Location = new System.Drawing.Point(187, 223);
            this.helpLocalPort.Name = "helpLocalPort";
            this.helpLocalPort.Size = new System.Drawing.Size(66, 18);
            this.helpLocalPort.TabIndex = 20;
            this.helpLocalPort.TabStop = true;
            this.helpLocalPort.Text = " found here.";
            this.helpLocalPort.UseCompatibleTextRendering = true;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(27, 176);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(462, 30);
            this.label5.TabIndex = 19;
            this.label5.Text = "Check the properties of the printer on the computer it is on and grant the approp" +
                "riate permissions.";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label4.Location = new System.Drawing.Point(11, 156);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 18;
            this.label4.Text = "Resolution";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label3.Location = new System.Drawing.Point(11, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 17;
            this.label3.Text = "Details";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(27, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(450, 38);
            this.label2.TabIndex = 16;
            this.label2.Text = "This is usually caused by printing to a networked printer that has not granted th" +
                "e \"Manage Printers\" permission to your Windows user account.";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(27, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(468, 20);
            this.label1.TabIndex = 15;
            this.label1.Text = "ShipWorks was unable to configure the printer for the settings specified in the t" +
                "emplate. ";
            // 
            // printerName
            // 
            this.printerName.Location = new System.Drawing.Point(121, 34);
            this.printerName.Name = "printerName";
            this.printerName.Size = new System.Drawing.Size(258, 23);
            this.printerName.TabIndex = 14;
            this.printerName.Text = "\\\\Computer\\Printer";
            // 
            // labelPrinter
            // 
            this.labelPrinter.Location = new System.Drawing.Point(71, 34);
            this.labelPrinter.Name = "labelPrinter";
            this.labelPrinter.Size = new System.Drawing.Size(48, 23);
            this.labelPrinter.TabIndex = 13;
            this.labelPrinter.Text = "Printer: ";
            // 
            // labelError
            // 
            this.labelError.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelError.Location = new System.Drawing.Point(69, 12);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(262, 23);
            this.labelError.TabIndex = 12;
            this.labelError.Text = "There was an error configuring the printer.";
            // 
            // pictureError
            // 
            this.pictureError.Image = ((System.Drawing.Image) (resources.GetObject("pictureError.Image")));
            this.pictureError.Location = new System.Drawing.Point(9, 10);
            this.pictureError.Name = "pictureError";
            this.pictureError.Size = new System.Drawing.Size(48, 48);
            this.pictureError.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureError.TabIndex = 11;
            this.pictureError.TabStop = false;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(27, 210);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(450, 35);
            this.label6.TabIndex = 21;
            this.label6.Text = "Alternatively, many users have success setting up the printer to use a Local Port" +
                ".  Instructions for doing this can be";
            // 
            // PrinterConfigurationErrorDlg
            // 
            this.AcceptButton = this.close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(490, 292);
            this.Controls.Add(this.helpLocalPort);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.printerName);
            this.Controls.Add(this.labelPrinter);
            this.Controls.Add(this.labelError);
            this.Controls.Add(this.pictureError);
            this.Controls.Add(this.close);
            this.Controls.Add(this.label6);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrinterConfigurationErrorDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Printer Configuration Error";
            ((System.ComponentModel.ISupportInitialize) (this.pictureError)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button close;
        private ShipWorks.ApplicationCore.Interaction.HelpLink helpLocalPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label printerName;
        private System.Windows.Forms.Label labelPrinter;
        private System.Windows.Forms.Label labelError;
        private System.Windows.Forms.PictureBox pictureError;
        private System.Windows.Forms.Label label6;
    }
}