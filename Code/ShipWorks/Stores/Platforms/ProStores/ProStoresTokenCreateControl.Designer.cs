﻿namespace ShipWorks.Stores.Platforms.ProStores
{
    partial class ProStoresTokenCreateControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProStoresTokenCreateControl));
            this.statusText = new System.Windows.Forms.Label();
            this.createTokenButton = new System.Windows.Forms.Button();
            this.statusPicture = new System.Windows.Forms.PictureBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize) (this.statusPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // statusText
            // 
            this.statusText.AutoSize = true;
            this.statusText.ForeColor = System.Drawing.Color.DimGray;
            this.statusText.Location = new System.Drawing.Point(152, 8);
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(243, 13);
            this.statusText.TabIndex = 16;
            this.statusText.Text = "Waiting for you to finish authorizing ShipWorks...";
            // 
            // createTokenButton
            // 
            this.createTokenButton.Location = new System.Drawing.Point(3, 3);
            this.createTokenButton.Name = "createTokenButton";
            this.createTokenButton.Size = new System.Drawing.Size(127, 23);
            this.createTokenButton.TabIndex = 14;
            this.createTokenButton.Text = "Create Login Token...";
            this.createTokenButton.UseVisualStyleBackColor = true;
            this.createTokenButton.Click += new System.EventHandler(this.OnCreateToken);
            // 
            // statusPicture
            // 
            this.statusPicture.Image = ((System.Drawing.Image) (resources.GetObject("statusPicture.Image")));
            this.statusPicture.Location = new System.Drawing.Point(135, 6);
            this.statusPicture.Name = "statusPicture";
            this.statusPicture.Size = new System.Drawing.Size(16, 16);
            this.statusPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.statusPicture.TabIndex = 15;
            this.statusPicture.TabStop = false;
            // 
            // timer
            // 
            this.timer.Interval = 5000;
            this.timer.Tick += new System.EventHandler(this.OnTimerTick);
            // 
            // EbayTokenCreateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusText);
            this.Controls.Add(this.statusPicture);
            this.Controls.Add(this.createTokenButton);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "EbayTokenCreateControl";
            this.Size = new System.Drawing.Size(411, 31);
            ((System.ComponentModel.ISupportInitialize) (this.statusPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label statusText;
        private System.Windows.Forms.PictureBox statusPicture;
        private System.Windows.Forms.Button createTokenButton;
        private System.Windows.Forms.Timer timer;
    }
}
