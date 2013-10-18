namespace ShipWorks.Common.IO.Hardware.Printers
{
    partial class PrinterTypeControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrinterTypeControl));
            this.labelPaperType = new System.Windows.Forms.Label();
            this.radioThermal = new System.Windows.Forms.RadioButton();
            this.radioPaper = new System.Windows.Forms.RadioButton();
            this.labelThermalLanguage = new System.Windows.Forms.Label();
            this.thermalLanguage = new System.Windows.Forms.ComboBox();
            this.panelThermal = new System.Windows.Forms.Panel();
            this.linkThermalHelp = new ShipWorks.UI.Controls.LinkControl();
            this.picturePaper = new System.Windows.Forms.PictureBox();
            this.pictureThermal = new System.Windows.Forms.PictureBox();
            this.panelThermal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePaper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureThermal)).BeginInit();
            this.SuspendLayout();
            // 
            // labelPaperType
            // 
            this.labelPaperType.AutoSize = true;
            this.labelPaperType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPaperType.Location = new System.Drawing.Point(10, 6);
            this.labelPaperType.Name = "labelPaperType";
            this.labelPaperType.Size = new System.Drawing.Size(241, 13);
            this.labelPaperType.TabIndex = 51;
            this.labelPaperType.Text = "What type of paper does this printer use?";
            // 
            // radioThermal
            // 
            this.radioThermal.AutoSize = true;
            this.radioThermal.Location = new System.Drawing.Point(89, 39);
            this.radioThermal.Name = "radioThermal";
            this.radioThermal.Size = new System.Drawing.Size(110, 17);
            this.radioThermal.TabIndex = 54;
            this.radioThermal.TabStop = true;
            this.radioThermal.Text = "Thermal label rolls";
            this.radioThermal.UseVisualStyleBackColor = true;
            this.radioThermal.CheckedChanged += new System.EventHandler(this.OnPaperTypeChanged);
            // 
            // radioPaper
            // 
            this.radioPaper.AutoSize = true;
            this.radioPaper.Location = new System.Drawing.Point(90, 120);
            this.radioPaper.Name = "radioPaper";
            this.radioPaper.Size = new System.Drawing.Size(162, 17);
            this.radioPaper.TabIndex = 55;
            this.radioPaper.TabStop = true;
            this.radioPaper.Text = "Normal paper or label sheets";
            this.radioPaper.UseVisualStyleBackColor = true;
            this.radioPaper.CheckedChanged += new System.EventHandler(this.OnPaperTypeChanged);
            // 
            // labelThermalLanguage
            // 
            this.labelThermalLanguage.AutoSize = true;
            this.labelThermalLanguage.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelThermalLanguage.Location = new System.Drawing.Point(10, 4);
            this.labelThermalLanguage.Name = "labelThermalLanguage";
            this.labelThermalLanguage.Size = new System.Drawing.Size(90, 13);
            this.labelThermalLanguage.TabIndex = 56;
            this.labelThermalLanguage.Text = "Printer language:";
            // 
            // thermalLanguage
            // 
            this.thermalLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.thermalLanguage.FormattingEnabled = true;
            this.thermalLanguage.Location = new System.Drawing.Point(106, 1);
            this.thermalLanguage.Name = "thermalLanguage";
            this.thermalLanguage.Size = new System.Drawing.Size(110, 21);
            this.thermalLanguage.TabIndex = 57;
            // 
            // panelThermal
            // 
            this.panelThermal.Controls.Add(this.linkThermalHelp);
            this.panelThermal.Controls.Add(this.labelThermalLanguage);
            this.panelThermal.Controls.Add(this.thermalLanguage);
            this.panelThermal.Enabled = false;
            this.panelThermal.Location = new System.Drawing.Point(97, 58);
            this.panelThermal.Name = "panelThermal";
            this.panelThermal.Size = new System.Drawing.Size(348, 27);
            this.panelThermal.TabIndex = 58;
            // 
            // linkThermalHelp
            // 
            this.linkThermalHelp.AutoSize = true;
            this.linkThermalHelp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkThermalHelp.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkThermalHelp.ForeColor = System.Drawing.Color.Blue;
            this.linkThermalHelp.Location = new System.Drawing.Point(222, 4);
            this.linkThermalHelp.Name = "linkThermalHelp";
            this.linkThermalHelp.Size = new System.Drawing.Size(94, 13);
            this.linkThermalHelp.TabIndex = 58;
            this.linkThermalHelp.Text = "Help me choose...";
            this.linkThermalHelp.Click += new System.EventHandler(this.OnHelpMeChooseThermalLanguage);
            // 
            // picturePaper
            // 
            this.picturePaper.Image = ((System.Drawing.Image)(resources.GetObject("picturePaper.Image")));
            this.picturePaper.Location = new System.Drawing.Point(30, 99);
            this.picturePaper.Name = "picturePaper";
            this.picturePaper.Size = new System.Drawing.Size(53, 62);
            this.picturePaper.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picturePaper.TabIndex = 53;
            this.picturePaper.TabStop = false;
            this.picturePaper.Click += new System.EventHandler(this.OnClickPaperTypeImage);
            // 
            // pictureThermal
            // 
            this.pictureThermal.Image = global::ShipWorks.Properties.Resources.thermal_label_roll;
            this.pictureThermal.Location = new System.Drawing.Point(30, 26);
            this.pictureThermal.Name = "pictureThermal";
            this.pictureThermal.Size = new System.Drawing.Size(60, 65);
            this.pictureThermal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureThermal.TabIndex = 52;
            this.pictureThermal.TabStop = false;
            this.pictureThermal.Click += new System.EventHandler(this.OnClickPaperTypeImage);
            // 
            // PrinterTypeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radioThermal);
            this.Controls.Add(this.picturePaper);
            this.Controls.Add(this.labelPaperType);
            this.Controls.Add(this.pictureThermal);
            this.Controls.Add(this.panelThermal);
            this.Controls.Add(this.radioPaper);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PrinterTypeControl";
            this.Size = new System.Drawing.Size(448, 170);
            this.Load += new System.EventHandler(this.OnLoad);
            this.panelThermal.ResumeLayout(false);
            this.panelThermal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePaper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureThermal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelPaperType;
        private System.Windows.Forms.PictureBox pictureThermal;
        private System.Windows.Forms.PictureBox picturePaper;
        private System.Windows.Forms.RadioButton radioThermal;
        private System.Windows.Forms.RadioButton radioPaper;
        private System.Windows.Forms.Label labelThermalLanguage;
        private System.Windows.Forms.ComboBox thermalLanguage;
        private System.Windows.Forms.Panel panelThermal;
        private UI.Controls.LinkControl linkThermalHelp;
    }
}
