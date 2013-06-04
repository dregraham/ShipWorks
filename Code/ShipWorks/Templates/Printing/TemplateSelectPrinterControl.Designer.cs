namespace ShipWorks.Templates.Printing
{
    partial class TemplateSelectPrinterControl
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
            this.labelMissing = new System.Windows.Forms.Label();
            this.printerName = new System.Windows.Forms.Label();
            this.printerIcon = new System.Windows.Forms.PictureBox();
            this.labelPrinter = new System.Windows.Forms.Label();
            this.labelTemplate = new System.Windows.Forms.Label();
            this.templateName = new System.Windows.Forms.Label();
            this.templateIcon = new System.Windows.Forms.PictureBox();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.linkChoose = new ShipWorks.UI.Controls.LinkControl();
            this.linkUseDefault = new ShipWorks.UI.Controls.LinkControl();
            ((System.ComponentModel.ISupportInitialize) (this.printerIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.templateIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // labelMissing
            // 
            this.labelMissing.AutoSize = true;
            this.labelMissing.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelMissing.Location = new System.Drawing.Point(187, 34);
            this.labelMissing.Name = "labelMissing";
            this.labelMissing.Size = new System.Drawing.Size(49, 13);
            this.labelMissing.TabIndex = 27;
            this.labelMissing.Text = "(Missing)";
            // 
            // printerName
            // 
            this.printerName.AutoSize = true;
            this.printerName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.printerName.Location = new System.Drawing.Point(85, 34);
            this.printerName.Name = "printerName";
            this.printerName.Size = new System.Drawing.Size(102, 13);
            this.printerName.TabIndex = 26;
            this.printerName.Text = "HP DeskJet 1450";
            // 
            // printerIcon
            // 
            this.printerIcon.BackColor = System.Drawing.Color.Transparent;
            this.printerIcon.Image = global::ShipWorks.Properties.Resources.printer2;
            this.printerIcon.Location = new System.Drawing.Point(67, 33);
            this.printerIcon.Name = "printerIcon";
            this.printerIcon.Size = new System.Drawing.Size(16, 16);
            this.printerIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.printerIcon.TabIndex = 25;
            this.printerIcon.TabStop = false;
            // 
            // labelPrinter
            // 
            this.labelPrinter.AutoSize = true;
            this.labelPrinter.Location = new System.Drawing.Point(19, 34);
            this.labelPrinter.Name = "labelPrinter";
            this.labelPrinter.Size = new System.Drawing.Size(43, 13);
            this.labelPrinter.TabIndex = 24;
            this.labelPrinter.Text = "Printer:";
            // 
            // labelTemplate
            // 
            this.labelTemplate.AutoSize = true;
            this.labelTemplate.Location = new System.Drawing.Point(7, 9);
            this.labelTemplate.Name = "labelTemplate";
            this.labelTemplate.Size = new System.Drawing.Size(55, 13);
            this.labelTemplate.TabIndex = 23;
            this.labelTemplate.Text = "Template:";
            // 
            // templateName
            // 
            this.templateName.AutoSize = true;
            this.templateName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.templateName.Location = new System.Drawing.Point(85, 9);
            this.templateName.Name = "templateName";
            this.templateName.Size = new System.Drawing.Size(113, 13);
            this.templateName.TabIndex = 22;
            this.templateName.Text = "Invoices\\Standard";
            // 
            // templateIcon
            // 
            this.templateIcon.BackColor = System.Drawing.Color.Transparent;
            this.templateIcon.Image = global::ShipWorks.Properties.Resources.template_general_16;
            this.templateIcon.Location = new System.Drawing.Point(67, 8);
            this.templateIcon.Name = "templateIcon";
            this.templateIcon.Size = new System.Drawing.Size(16, 16);
            this.templateIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.templateIcon.TabIndex = 21;
            this.templateIcon.TabStop = false;
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.AutoSize = false;
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(0, 0);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(330, 1);
            this.kryptonBorderEdge.TabIndex = 31;
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            this.kryptonBorderEdge.Visible = false;
            // 
            // linkChoose
            // 
            this.linkChoose.AutoSize = true;
            this.linkChoose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkChoose.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkChoose.ForeColor = System.Drawing.Color.Blue;
            this.linkChoose.Location = new System.Drawing.Point(147, 55);
            this.linkChoose.Name = "linkChoose";
            this.linkChoose.Size = new System.Drawing.Size(55, 13);
            this.linkChoose.TabIndex = 29;
            this.linkChoose.Text = "Choose...";
            this.linkChoose.Click += new System.EventHandler(this.OnChoosePrinter);
            // 
            // linkUseDefault
            // 
            this.linkUseDefault.AutoSize = true;
            this.linkUseDefault.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkUseDefault.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkUseDefault.ForeColor = System.Drawing.Color.Blue;
            this.linkUseDefault.Location = new System.Drawing.Point(84, 55);
            this.linkUseDefault.Name = "linkUseDefault";
            this.linkUseDefault.Size = new System.Drawing.Size(63, 13);
            this.linkUseDefault.TabIndex = 28;
            this.linkUseDefault.Text = "Use Default";
            this.linkUseDefault.Click += new System.EventHandler(this.OnUseDefault);
            // 
            // TemplateSelectPrinterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonBorderEdge);
            this.Controls.Add(this.linkChoose);
            this.Controls.Add(this.linkUseDefault);
            this.Controls.Add(this.labelMissing);
            this.Controls.Add(this.printerName);
            this.Controls.Add(this.printerIcon);
            this.Controls.Add(this.labelPrinter);
            this.Controls.Add(this.labelTemplate);
            this.Controls.Add(this.templateName);
            this.Controls.Add(this.templateIcon);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "TemplateSelectPrinterControl";
            this.Size = new System.Drawing.Size(330, 83);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize) (this.printerIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.templateIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Controls.LinkControl linkChoose;
        private UI.Controls.LinkControl linkUseDefault;
        private System.Windows.Forms.Label labelMissing;
        private System.Windows.Forms.Label printerName;
        private System.Windows.Forms.PictureBox printerIcon;
        private System.Windows.Forms.Label labelPrinter;
        private System.Windows.Forms.Label labelTemplate;
        private System.Windows.Forms.Label templateName;
        private System.Windows.Forms.PictureBox templateIcon;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
    }
}
