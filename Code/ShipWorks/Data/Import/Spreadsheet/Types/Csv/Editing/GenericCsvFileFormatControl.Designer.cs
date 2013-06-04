namespace ShipWorks.Data.Import.Spreadsheet.Types.Csv.Editing
{
    partial class GenericCsvFileFormatControl
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
            this.panelQuoteEscapes = new System.Windows.Forms.Panel();
            this.labelQuoteEscapes = new System.Windows.Forms.Label();
            this.quoteEscapes = new System.Windows.Forms.ComboBox();
            this.labelDelimiter = new System.Windows.Forms.Label();
            this.delimiter = new System.Windows.Forms.ComboBox();
            this.labelTextQualifier = new System.Windows.Forms.Label();
            this.textQualifier = new System.Windows.Forms.ComboBox();
            this.fileEncoding = new System.Windows.Forms.ComboBox();
            this.labelFileEncoding = new System.Windows.Forms.Label();
            this.panelQuoteEscapes.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelQuoteEscapes
            // 
            this.panelQuoteEscapes.Controls.Add(this.labelQuoteEscapes);
            this.panelQuoteEscapes.Controls.Add(this.quoteEscapes);
            this.panelQuoteEscapes.Location = new System.Drawing.Point(219, 29);
            this.panelQuoteEscapes.Name = "panelQuoteEscapes";
            this.panelQuoteEscapes.Size = new System.Drawing.Size(236, 27);
            this.panelQuoteEscapes.TabIndex = 35;
            // 
            // labelQuoteEscapes
            // 
            this.labelQuoteEscapes.AutoSize = true;
            this.labelQuoteEscapes.Location = new System.Drawing.Point(2, 4);
            this.labelQuoteEscapes.Name = "labelQuoteEscapes";
            this.labelQuoteEscapes.Size = new System.Drawing.Size(131, 13);
            this.labelQuoteEscapes.TabIndex = 28;
            this.labelQuoteEscapes.Text = "literal quotes escaped by:";
            // 
            // quoteEscapes
            // 
            this.quoteEscapes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.quoteEscapes.FormattingEnabled = true;
            this.quoteEscapes.Items.AddRange(new object[] {
            "Double (\")",
            "Backslash (\\)"});
            this.quoteEscapes.Location = new System.Drawing.Point(134, 1);
            this.quoteEscapes.Name = "quoteEscapes";
            this.quoteEscapes.Size = new System.Drawing.Size(99, 21);
            this.quoteEscapes.TabIndex = 29;
            this.quoteEscapes.SelectedIndexChanged += new System.EventHandler(this.OnChangeEscape);
            // 
            // labelDelimiter
            // 
            this.labelDelimiter.AutoSize = true;
            this.labelDelimiter.Location = new System.Drawing.Point(20, 6);
            this.labelDelimiter.Name = "labelDelimiter";
            this.labelDelimiter.Size = new System.Drawing.Size(52, 13);
            this.labelDelimiter.TabIndex = 31;
            this.labelDelimiter.Text = "Delimiter:";
            // 
            // delimiter
            // 
            this.delimiter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.delimiter.FormattingEnabled = true;
            this.delimiter.Items.AddRange(new object[] {
            "Comma",
            "Semicolon",
            "Tab"});
            this.delimiter.Location = new System.Drawing.Point(78, 3);
            this.delimiter.Name = "delimiter";
            this.delimiter.Size = new System.Drawing.Size(135, 21);
            this.delimiter.TabIndex = 32;
            this.delimiter.SelectedIndexChanged += new System.EventHandler(this.OnChangeDelimiter);
            // 
            // labelTextQualifier
            // 
            this.labelTextQualifier.AutoSize = true;
            this.labelTextQualifier.Location = new System.Drawing.Point(3, 33);
            this.labelTextQualifier.Name = "labelTextQualifier";
            this.labelTextQualifier.Size = new System.Drawing.Size(69, 13);
            this.labelTextQualifier.TabIndex = 33;
            this.labelTextQualifier.Text = "Field quotes:";
            // 
            // textQualifier
            // 
            this.textQualifier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.textQualifier.FormattingEnabled = true;
            this.textQualifier.Items.AddRange(new object[] {
            "\"",
            "\'",
            "(None)"});
            this.textQualifier.Location = new System.Drawing.Point(78, 30);
            this.textQualifier.Name = "textQualifier";
            this.textQualifier.Size = new System.Drawing.Size(135, 21);
            this.textQualifier.TabIndex = 34;
            this.textQualifier.SelectedIndexChanged += new System.EventHandler(this.OnChangeQuotes);
            // 
            // fileEncoding
            // 
            this.fileEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fileEncoding.Location = new System.Drawing.Point(78, 57);
            this.fileEncoding.Name = "fileEncoding";
            this.fileEncoding.Size = new System.Drawing.Size(135, 21);
            this.fileEncoding.TabIndex = 37;
            this.fileEncoding.SelectedIndexChanged += new System.EventHandler(this.OnChangeEncoding);
            // 
            // labelFileEncoding
            // 
            this.labelFileEncoding.AutoSize = true;
            this.labelFileEncoding.Location = new System.Drawing.Point(18, 60);
            this.labelFileEncoding.Name = "labelFileEncoding";
            this.labelFileEncoding.Size = new System.Drawing.Size(54, 13);
            this.labelFileEncoding.TabIndex = 36;
            this.labelFileEncoding.Text = "Encoding:";
            // 
            // GenericCsvFileFormatControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.fileEncoding);
            this.Controls.Add(this.labelFileEncoding);
            this.Controls.Add(this.panelQuoteEscapes);
            this.Controls.Add(this.labelDelimiter);
            this.Controls.Add(this.delimiter);
            this.Controls.Add(this.labelTextQualifier);
            this.Controls.Add(this.textQualifier);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "GenericCsvFileFormatControl";
            this.Size = new System.Drawing.Size(465, 93);
            this.panelQuoteEscapes.ResumeLayout(false);
            this.panelQuoteEscapes.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelQuoteEscapes;
        private System.Windows.Forms.Label labelQuoteEscapes;
        private System.Windows.Forms.ComboBox quoteEscapes;
        private System.Windows.Forms.Label labelDelimiter;
        private System.Windows.Forms.ComboBox delimiter;
        private System.Windows.Forms.Label labelTextQualifier;
        private System.Windows.Forms.ComboBox textQualifier;
        private System.Windows.Forms.ComboBox fileEncoding;
        private System.Windows.Forms.Label labelFileEncoding;
    }
}
