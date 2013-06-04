namespace ShipWorks.Templates.Emailing
{
    partial class EmailSettingsResolveDlg
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
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.labelInfo = new System.Windows.Forms.Label();
            this.useMostRecentOrder = new System.Windows.Forms.RadioButton();
            this.useMostRecentOrderAlways = new System.Windows.Forms.CheckBox();
            this.useSpecificStore = new System.Windows.Forms.RadioButton();
            this.storeCombo = new System.Windows.Forms.ComboBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.labelStoreSpecific = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(331, 156);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 0;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(412, 156);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.OnCancel);
            // 
            // labelInfo
            // 
            this.labelInfo.Location = new System.Drawing.Point(56, 12);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(431, 32);
            this.labelInfo.TabIndex = 2;
            this.labelInfo.Text = "The selection used for the report has orders from more than one store.  The email" +
                " settings for the stores are different, and you must choose which settings to us" +
                "e.";
            // 
            // useMostRecentOrder
            // 
            this.useMostRecentOrder.AutoSize = true;
            this.useMostRecentOrder.Location = new System.Drawing.Point(59, 49);
            this.useMostRecentOrder.Name = "useMostRecentOrder";
            this.useMostRecentOrder.Size = new System.Drawing.Size(274, 17);
            this.useMostRecentOrder.TabIndex = 3;
            this.useMostRecentOrder.TabStop = true;
            this.useMostRecentOrder.Text = "Use the settings from the most recent order placed.";
            this.useMostRecentOrder.UseVisualStyleBackColor = true;
            this.useMostRecentOrder.CheckedChanged += new System.EventHandler(this.OnChoiceChanged);
            // 
            // useMostRecentOrderAlways
            // 
            this.useMostRecentOrderAlways.AutoSize = true;
            this.useMostRecentOrderAlways.Location = new System.Drawing.Point(78, 71);
            this.useMostRecentOrderAlways.Name = "useMostRecentOrderAlways";
            this.useMostRecentOrderAlways.Size = new System.Drawing.Size(333, 17);
            this.useMostRecentOrderAlways.TabIndex = 4;
            this.useMostRecentOrderAlways.Text = "Use this answer for the rest of the emails I\'m sending right now.";
            this.useMostRecentOrderAlways.UseVisualStyleBackColor = true;
            // 
            // useSpecificStore
            // 
            this.useSpecificStore.AutoSize = true;
            this.useSpecificStore.Location = new System.Drawing.Point(59, 97);
            this.useSpecificStore.Name = "useSpecificStore";
            this.useSpecificStore.Size = new System.Drawing.Size(180, 17);
            this.useSpecificStore.TabIndex = 5;
            this.useSpecificStore.TabStop = true;
            this.useSpecificStore.Text = "Use the settings from this store:";
            this.useSpecificStore.UseVisualStyleBackColor = true;
            this.useSpecificStore.CheckedChanged += new System.EventHandler(this.OnChoiceChanged);
            // 
            // storeCombo
            // 
            this.storeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.storeCombo.Enabled = false;
            this.storeCombo.FormattingEnabled = true;
            this.storeCombo.Location = new System.Drawing.Point(78, 118);
            this.storeCombo.Name = "storeCombo";
            this.storeCombo.Size = new System.Drawing.Size(255, 21);
            this.storeCombo.TabIndex = 6;
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.mail_preferences;
            this.pictureBox.Location = new System.Drawing.Point(12, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(32, 32);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 7;
            this.pictureBox.TabStop = false;
            // 
            // labelStoreSpecific
            // 
            this.labelStoreSpecific.AutoSize = true;
            this.labelStoreSpecific.Location = new System.Drawing.Point(56, 144);
            this.labelStoreSpecific.Name = "labelStoreSpecific";
            this.labelStoreSpecific.Size = new System.Drawing.Size(162, 13);
            this.labelStoreSpecific.TabIndex = 8;
            this.labelStoreSpecific.Text = "Use the settings from this store:";
            // 
            // EmailSettingsResolveDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(499, 191);
            this.Controls.Add(this.labelStoreSpecific);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.storeCombo);
            this.Controls.Add(this.useSpecificStore);
            this.Controls.Add(this.useMostRecentOrderAlways);
            this.Controls.Add(this.useMostRecentOrder);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EmailSettingsResolveDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Email Settings";
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.RadioButton useMostRecentOrder;
        private System.Windows.Forms.CheckBox useMostRecentOrderAlways;
        private System.Windows.Forms.RadioButton useSpecificStore;
        private System.Windows.Forms.ComboBox storeCombo;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label labelStoreSpecific;
    }
}