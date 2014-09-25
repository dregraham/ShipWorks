using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    partial class UpsOltOptionsControl
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
            this.labelLabels = new System.Windows.Forms.Label();
            this.requestedLabelFormat = new ShipWorks.Shipping.Editing.RequestedLabelFormatOptionControl();
            this.reqestedLabelFormat = new ShipWorks.Shipping.Editing.RequestedLabelFormatOptionControl();
            this.SuspendLayout();
            // 
            // labelLabels
            // 
            this.labelLabels.AutoSize = true;
            this.labelLabels.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLabels.Location = new System.Drawing.Point(0, 1);
            this.labelLabels.Name = "labelLabels";
            this.labelLabels.Size = new System.Drawing.Size(43, 13);
            this.labelLabels.TabIndex = 39;
            this.labelLabels.Text = "Labels";
            // 
            // requestedLabelFormat
            // 
            this.requestedLabelFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.requestedLabelFormat.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.requestedLabelFormat.Location = new System.Drawing.Point(16, 21);
            this.requestedLabelFormat.Name = "requestedLabelFormat";
            this.requestedLabelFormat.Size = new System.Drawing.Size(340, 25);
            this.requestedLabelFormat.TabIndex = 58;
            // 
            // reqestedLabelFormat
            // 
            this.reqestedLabelFormat.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.reqestedLabelFormat.Location = new System.Drawing.Point(0, 0);
            this.reqestedLabelFormat.Name = "reqestedLabelFormat";
            this.reqestedLabelFormat.Size = new System.Drawing.Size(115, 21);
            this.reqestedLabelFormat.TabIndex = 57;
            // 
            // UpsOltOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelLabels);
            this.Controls.Add(this.requestedLabelFormat);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UpsOltOptionsControl";
            this.Size = new System.Drawing.Size(363, 50);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelLabels;
        private RequestedLabelFormatOptionControl reqestedLabelFormat;
        private RequestedLabelFormatOptionControl requestedLabelFormat;
    }
}
