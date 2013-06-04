namespace ShipWorks.Templates.Printing
{
    partial class PrintResultViewerDlg
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
            this.close = new System.Windows.Forms.Button();
            this.labelPrintedFor = new System.Windows.Forms.Label();
            this.entityImage = new System.Windows.Forms.PictureBox();
            this.entityText = new System.Windows.Forms.Label();
            this.labelTemplate = new System.Windows.Forms.Label();
            this.templateName = new System.Windows.Forms.Label();
            this.templateImage = new System.Windows.Forms.PictureBox();
            this.printDate = new System.Windows.Forms.Label();
            this.labelPrinter = new System.Windows.Forms.Label();
            this.printerName = new System.Windows.Forms.Label();
            this.printImage = new System.Windows.Forms.PictureBox();
            this.computerName = new System.Windows.Forms.Label();
            this.reprint = new System.Windows.Forms.Button();
            this.themeBorderPanel = new ShipWorks.UI.Controls.ThemeBorderPanel();
            this.htmlControl = new ShipWorks.UI.Controls.Html.HtmlControl();
            ((System.ComponentModel.ISupportInitialize) (this.entityImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.templateImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.printImage)).BeginInit();
            this.themeBorderPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.close.Location = new System.Drawing.Point(462, 439);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 0;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // labelPrintedFor
            // 
            this.labelPrintedFor.AutoSize = true;
            this.labelPrintedFor.Location = new System.Drawing.Point(32, 9);
            this.labelPrintedFor.Name = "labelPrintedFor";
            this.labelPrintedFor.Size = new System.Drawing.Size(62, 13);
            this.labelPrintedFor.TabIndex = 2;
            this.labelPrintedFor.Text = "Printed for:";
            // 
            // entityImage
            // 
            this.entityImage.Image = global::ShipWorks.Properties.Resources.order16;
            this.entityImage.Location = new System.Drawing.Point(96, 8);
            this.entityImage.Name = "entityImage";
            this.entityImage.Size = new System.Drawing.Size(16, 16);
            this.entityImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.entityImage.TabIndex = 3;
            this.entityImage.TabStop = false;
            // 
            // entityText
            // 
            this.entityText.AutoSize = true;
            this.entityText.Location = new System.Drawing.Point(113, 9);
            this.entityText.Name = "entityText";
            this.entityText.Size = new System.Drawing.Size(62, 13);
            this.entityText.TabIndex = 4;
            this.entityText.Text = "Order 1028";
            // 
            // labelTemplate
            // 
            this.labelTemplate.AutoSize = true;
            this.labelTemplate.Location = new System.Drawing.Point(13, 30);
            this.labelTemplate.Name = "labelTemplate";
            this.labelTemplate.Size = new System.Drawing.Size(82, 13);
            this.labelTemplate.TabIndex = 5;
            this.labelTemplate.Text = "Using template:";
            // 
            // templateName
            // 
            this.templateName.AutoSize = true;
            this.templateName.Location = new System.Drawing.Point(113, 30);
            this.templateName.Name = "templateName";
            this.templateName.Size = new System.Drawing.Size(89, 13);
            this.templateName.TabIndex = 6;
            this.templateName.Text = "Standard Invoice";
            // 
            // templateImage
            // 
            this.templateImage.Image = global::ShipWorks.Properties.Resources.template_standard_doc16;
            this.templateImage.Location = new System.Drawing.Point(96, 29);
            this.templateImage.Name = "templateImage";
            this.templateImage.Size = new System.Drawing.Size(16, 16);
            this.templateImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.templateImage.TabIndex = 7;
            this.templateImage.TabStop = false;
            // 
            // printDate
            // 
            this.printDate.AutoSize = true;
            this.printDate.ForeColor = System.Drawing.Color.DimGray;
            this.printDate.Location = new System.Drawing.Point(173, 9);
            this.printDate.Name = "printDate";
            this.printDate.Size = new System.Drawing.Size(150, 13);
            this.printDate.TabIndex = 9;
            this.printDate.Text = "on January 8, 2008 10:14 AM";
            // 
            // labelPrinter
            // 
            this.labelPrinter.AutoSize = true;
            this.labelPrinter.Location = new System.Drawing.Point(37, 52);
            this.labelPrinter.Name = "labelPrinter";
            this.labelPrinter.Size = new System.Drawing.Size(58, 13);
            this.labelPrinter.TabIndex = 10;
            this.labelPrinter.Text = "To printer:";
            // 
            // printerName
            // 
            this.printerName.AutoSize = true;
            this.printerName.Location = new System.Drawing.Point(114, 52);
            this.printerName.Name = "printerName";
            this.printerName.Size = new System.Drawing.Size(77, 13);
            this.printerName.TabIndex = 11;
            this.printerName.Text = "HP DonkJet 69";
            // 
            // printImage
            // 
            this.printImage.Image = global::ShipWorks.Properties.Resources.printer1;
            this.printImage.Location = new System.Drawing.Point(96, 50);
            this.printImage.Name = "printImage";
            this.printImage.Size = new System.Drawing.Size(16, 16);
            this.printImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.printImage.TabIndex = 12;
            this.printImage.TabStop = false;
            // 
            // computerName
            // 
            this.computerName.AutoSize = true;
            this.computerName.ForeColor = System.Drawing.Color.DimGray;
            this.computerName.Location = new System.Drawing.Point(189, 52);
            this.computerName.Name = "computerName";
            this.computerName.Size = new System.Drawing.Size(122, 13);
            this.computerName.TabIndex = 13;
            this.computerName.Text = "on computer \\\\BRIANPC";
            // 
            // reprint
            // 
            this.reprint.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.reprint.Image = global::ShipWorks.Properties.Resources.printer1;
            this.reprint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.reprint.Location = new System.Drawing.Point(376, 439);
            this.reprint.Name = "reprint";
            this.reprint.Size = new System.Drawing.Size(80, 23);
            this.reprint.TabIndex = 14;
            this.reprint.Text = "Reprint...";
            this.reprint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.reprint.UseVisualStyleBackColor = true;
            this.reprint.Click += new System.EventHandler(this.OnReprint);
            // 
            // themeBorderPanel
            // 
            this.themeBorderPanel.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.themeBorderPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.themeBorderPanel.Controls.Add(this.htmlControl);
            this.themeBorderPanel.Location = new System.Drawing.Point(12, 72);
            this.themeBorderPanel.Name = "themeBorderPanel";
            this.themeBorderPanel.Size = new System.Drawing.Size(525, 361);
            this.themeBorderPanel.TabIndex = 1;
            // 
            // htmlControl
            // 
            this.htmlControl.AllowActiveContent = false;
            this.htmlControl.AllowContextMenu = false;
            this.htmlControl.AllowNavigation = false;
            this.htmlControl.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.htmlControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htmlControl.Html = "";
            this.htmlControl.Location = new System.Drawing.Point(0, 0);
            this.htmlControl.Name = "htmlControl";
            this.htmlControl.OpenLinksInNewWindow = true;
            this.htmlControl.SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.htmlControl.SelectionBackColor = System.Drawing.Color.Empty;
            this.htmlControl.SelectionBold = false;
            this.htmlControl.SelectionBullets = false;
            this.htmlControl.SelectionFont = null;
            this.htmlControl.SelectionFontName = "Times New Roman";
            this.htmlControl.SelectionFontSize = 2;
            this.htmlControl.SelectionForeColor = System.Drawing.Color.Empty;
            this.htmlControl.SelectionItalic = false;
            this.htmlControl.SelectionNumbering = false;
            this.htmlControl.SelectionUnderline = false;
            this.htmlControl.ShowBorderGuides = true;
            this.htmlControl.ShowGlyphs = false;
            this.htmlControl.Size = new System.Drawing.Size(521, 357);
            this.htmlControl.TabIndex = 234;
            // 
            // PrintResultViewerDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(549, 474);
            this.Controls.Add(this.reprint);
            this.Controls.Add(this.computerName);
            this.Controls.Add(this.printImage);
            this.Controls.Add(this.printerName);
            this.Controls.Add(this.labelPrinter);
            this.Controls.Add(this.printDate);
            this.Controls.Add(this.templateImage);
            this.Controls.Add(this.templateName);
            this.Controls.Add(this.labelTemplate);
            this.Controls.Add(this.entityText);
            this.Controls.Add(this.entityImage);
            this.Controls.Add(this.labelPrintedFor);
            this.Controls.Add(this.themeBorderPanel);
            this.Controls.Add(this.close);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(467, 421);
            this.Name = "PrintResultViewerDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Print Viewer";
            this.Load += new System.EventHandler(this.OnLoad);
            this.Shown += new System.EventHandler(this.OnShown);
            ((System.ComponentModel.ISupportInitialize) (this.entityImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.templateImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.printImage)).EndInit();
            this.themeBorderPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button close;
        private ShipWorks.UI.Controls.ThemeBorderPanel themeBorderPanel;
        private ShipWorks.UI.Controls.Html.HtmlControl htmlControl;
        private System.Windows.Forms.Label labelPrintedFor;
        private System.Windows.Forms.PictureBox entityImage;
        private System.Windows.Forms.Label entityText;
        private System.Windows.Forms.Label labelTemplate;
        private System.Windows.Forms.Label templateName;
        private System.Windows.Forms.PictureBox templateImage;
        private System.Windows.Forms.Label printDate;
        private System.Windows.Forms.Label labelPrinter;
        private System.Windows.Forms.Label printerName;
        private System.Windows.Forms.PictureBox printImage;
        private System.Windows.Forms.Label computerName;
        private System.Windows.Forms.Button reprint;
    }
}