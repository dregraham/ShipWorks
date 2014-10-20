namespace ShipWorks.UI.Controls
{
    partial class ResizingRichTextBox
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
            this.questionsTextBox = new UI.Controls.ResizingRichTextBox.DisabledCursorRichTextBox();
            this.SuspendLayout();
            // 
            // questionsTextBox
            // 
            this.questionsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.questionsTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.questionsTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.questionsTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.questionsTextBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.questionsTextBox.Location = new System.Drawing.Point(-1, -2);
            this.questionsTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.questionsTextBox.Name = "questionsTextBox";
            this.questionsTextBox.ReadOnly = true;
            this.questionsTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.questionsTextBox.Size = new System.Drawing.Size(457, 44);
            this.questionsTextBox.TabIndex = 0;
            // 
            // InsureShipQuestionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.questionsTextBox);
            this.Name = "InsureShipQuestionsControl";
            this.Size = new System.Drawing.Size(457, 44);
            this.Resize += new System.EventHandler(this.OnResize);
            this.ResumeLayout(false);

        }

        #endregion

        private DisabledCursorRichTextBox questionsTextBox;
    }
}
