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
            this.infotipLabelType = new ShipWorks.UI.Controls.InfoTip();
            this.SuspendLayout();
            // 
            // labelFormat
            // 
            this.labelFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.labelFormat.FormattingEnabled = true;
            this.labelFormat.Location = new System.Drawing.Point(125, 0);
            this.labelFormat.Name = "labelFormat";
            this.labelFormat.Size = new System.Drawing.Size(115, 21);
            this.labelFormat.TabIndex = 63;
            // 
            // labelFormatMessage
            // 
            this.labelFormatMessage.AutoSize = true;
            this.labelFormatMessage.Location = new System.Drawing.Point(0, 3);
            this.labelFormatMessage.Name = "labelFormatMessage";
            this.labelFormatMessage.Size = new System.Drawing.Size(123, 13);
            this.labelFormatMessage.TabIndex = 62;
            this.labelFormatMessage.Text = "Requested label format:";
            // 
            // infotipLabelType
            // 
            this.infotipLabelType.Caption = "The printer type for a shipment cannot be changed after processing.";
            this.infotipLabelType.Location = new System.Drawing.Point(246, 4);
            this.infotipLabelType.Name = "infotipLabelType";
            this.infotipLabelType.Size = new System.Drawing.Size(12, 12);
            this.infotipLabelType.TabIndex = 61;
            this.infotipLabelType.Title = "Printer Type";
            // 
            // RequestedLabelFormatProfileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelFormat);
            this.Controls.Add(this.labelFormatMessage);
            this.Controls.Add(this.infotipLabelType);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "RequestedLabelFormatProfileControl";
            this.Size = new System.Drawing.Size(265, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox labelFormat;
        private System.Windows.Forms.Label labelFormatMessage;
        private UI.Controls.InfoTip infotipLabelType;

    }
}
