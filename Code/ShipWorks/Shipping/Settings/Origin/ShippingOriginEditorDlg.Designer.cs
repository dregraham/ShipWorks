namespace ShipWorks.Shipping.Settings.Origin
{
    partial class ShippingOriginEditorDlg
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
            this.components = new System.ComponentModel.Container();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.labelSender = new System.Windows.Forms.Label();
            this.labelOptional = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.description = new ShipWorks.UI.Controls.PromptTextBox();
            this.personControl = new ShipWorks.Data.Controls.PersonControl();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(197, 444);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 5;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(278, 444);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 6;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // labelSender
            // 
            this.labelSender.AutoSize = true;
            this.labelSender.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelSender.Location = new System.Drawing.Point(9, 9);
            this.labelSender.Name = "labelSender";
            this.labelSender.Size = new System.Drawing.Size(71, 13);
            this.labelSender.TabIndex = 0;
            this.labelSender.Text = "Description";
            // 
            // labelOptional
            // 
            this.labelOptional.AutoSize = true;
            this.labelOptional.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelOptional.Location = new System.Drawing.Point(300, 30);
            this.labelOptional.Name = "labelOptional";
            this.labelOptional.Size = new System.Drawing.Size(53, 13);
            this.labelOptional.TabIndex = 3;
            this.labelOptional.Text = "(optional)";
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(14, 30);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(64, 13);
            this.labelDescription.TabIndex = 1;
            this.labelDescription.Text = "Description:";
            // 
            // description
            // 
            this.description.Location = new System.Drawing.Point(83, 27);
            this.fieldLengthProvider.SetMaxLengthSource(this.description, ShipWorks.Data.Utility.EntityFieldLengthSource.ShipmentOriginDescription);
            this.description.Name = "description";
            this.description.PromptColor = System.Drawing.SystemColors.GrayText;
            this.description.PromptText = null;
            this.description.Size = new System.Drawing.Size(211, 21);
            this.description.TabIndex = 2;
            // 
            // personControl
            // 
            this.personControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.personControl.Location = new System.Drawing.Point(7, 51);
            this.personControl.Name = "personControl";
            this.personControl.Size = new System.Drawing.Size(358, 391);
            this.personControl.TabIndex = 4;
            this.personControl.ContentChanged += new System.EventHandler(this.OnPersonContentChanged);
            // 
            // ShippingOriginEditorDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(365, 479);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.description);
            this.Controls.Add(this.labelOptional);
            this.Controls.Add(this.labelSender);
            this.Controls.Add(this.personControl);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShippingOriginEditorDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Origin";
            this.Load += new System.EventHandler(this.OnLoad);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private ShipWorks.Data.Controls.PersonControl personControl;
        private System.Windows.Forms.Label labelSender;
        private System.Windows.Forms.Label labelOptional;
        private ShipWorks.UI.Controls.PromptTextBox description;
        private System.Windows.Forms.Label labelDescription;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
    }
}