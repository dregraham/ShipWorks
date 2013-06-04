namespace ShipWorks.Stores.Platforms.Infopia
{
    partial class InfopiaAccountSettingsControl
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
            this.components = new System.ComponentModel.Container();
            this.tokenTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.linkHelp = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // tokenTextBox
            // 
            this.tokenTextBox.Location = new System.Drawing.Point(15, 29);
            this.fieldLengthProvider.SetMaxLengthSource(this.tokenTextBox, ShipWorks.Data.Utility.EntityFieldLengthSource.InfopiaApiToken);
            this.tokenTextBox.Name = "tokenTextBox";
            this.tokenTextBox.Size = new System.Drawing.Size(443, 21);
            this.tokenTextBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Infopia user token:";
            // 
            // linkHelp
            // 
            this.linkHelp.AutoSize = true;
            this.linkHelp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkHelp.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkHelp.ForeColor = System.Drawing.Color.Blue;
            this.linkHelp.Location = new System.Drawing.Point(119, 11);
            this.linkHelp.Name = "linkHelp";
            this.linkHelp.Size = new System.Drawing.Size(107, 13);
            this.linkHelp.TabIndex = 4;
            this.linkHelp.Text = "Where do I find this?";
            // 
            // InfopiaAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkHelp);
            this.Controls.Add(this.tokenTextBox);
            this.Controls.Add(this.label1);
            this.Name = "InfopiaAccountSettingsControl";
            this.Size = new System.Drawing.Size(501, 206);
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tokenTextBox;
        private System.Windows.Forms.Label label1;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private ShipWorks.ApplicationCore.Interaction.HelpLink linkHelp;
    }
}
