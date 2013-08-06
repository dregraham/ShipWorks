namespace ShipWorks.ApplicationCore.MessageBoxes
{
    partial class WelcomeDlg
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
            this.etchBottom = new System.Windows.Forms.Label();
            this.topPanel = new System.Windows.Forms.Panel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.description = new System.Windows.Forms.Label();
            this.etchTop = new System.Windows.Forms.Label();
            this.title = new System.Windows.Forms.Label();
            this.cancel = new System.Windows.Forms.Button();
            this.next = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.labelHelp = new System.Windows.Forms.Label();
            this.linkControlSupportForum = new ShipWorks.UI.Controls.LinkControl();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // etchBottom
            // 
            this.etchBottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.etchBottom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.etchBottom.Location = new System.Drawing.Point(0, 333);
            this.etchBottom.Margin = new System.Windows.Forms.Padding(0);
            this.etchBottom.Name = "etchBottom";
            this.etchBottom.Size = new System.Drawing.Size(550, 2);
            this.etchBottom.TabIndex = 11;
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.Color.White;
            this.topPanel.Controls.Add(this.pictureBox);
            this.topPanel.Controls.Add(this.description);
            this.topPanel.Controls.Add(this.etchTop);
            this.topPanel.Controls.Add(this.title);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(546, 56);
            this.topPanel.TabIndex = 8;
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.sw_cubes_big;
            this.pictureBox.Location = new System.Drawing.Point(489, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(54, 50);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 8;
            this.pictureBox.TabStop = false;
            // 
            // description
            // 
            this.description.AutoSize = true;
            this.description.Location = new System.Drawing.Point(40, 32);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(172, 13);
            this.description.TabIndex = 7;
            this.description.Text = "Thank you for choosing ShipWorks";
            // 
            // etchTop
            // 
            this.etchTop.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.etchTop.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.etchTop.Location = new System.Drawing.Point(0, 54);
            this.etchTop.Margin = new System.Windows.Forms.Padding(0);
            this.etchTop.Name = "etchTop";
            this.etchTop.Size = new System.Drawing.Size(546, 2);
            this.etchTop.TabIndex = 6;
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.Location = new System.Drawing.Point(20, 12);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(137, 13);
            this.title.TabIndex = 0;
            this.title.Text = "Welcome to ShipWorks";
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(459, 343);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // next
            // 
            this.next.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.next.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.next.Location = new System.Drawing.Point(380, 343);
            this.next.Name = "next";
            this.next.Size = new System.Drawing.Size(75, 23);
            this.next.TabIndex = 0;
            this.next.Text = "Next >";
            this.next.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(20, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(404, 30);
            this.label2.TabIndex = 12;
            this.label2.Text = "Welcome to ShipWorks!";
            // 
            // labelHelp
            // 
            this.labelHelp.Location = new System.Drawing.Point(20, 98);
            this.labelHelp.Name = "labelHelp";
            this.labelHelp.Size = new System.Drawing.Size(488, 44);
            this.labelHelp.TabIndex = 15;
            this.labelHelp.Text = "If you have any questions during installation, we are happy to help.  Please call" +
    " us at 1-800-95-APPTIVE or visit our support forum at ";
            // 
            // linkControlSupportForum
            // 
            this.linkControlSupportForum.AutoSize = true;
            this.linkControlSupportForum.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkControlSupportForum.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkControlSupportForum.ForeColor = System.Drawing.Color.Blue;
            this.linkControlSupportForum.Location = new System.Drawing.Point(252, 111);
            this.linkControlSupportForum.Name = "linkControlSupportForum";
            this.linkControlSupportForum.Size = new System.Drawing.Size(182, 13);
            this.linkControlSupportForum.TabIndex = 16;
            this.linkControlSupportForum.Text = "http://www.shipworks.com/support.";
            this.linkControlSupportForum.Click += new System.EventHandler(this.OnLinkSupportForum);
            // 
            // WelcomeDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 376);
            this.Controls.Add(this.linkControlSupportForum);
            this.Controls.Add(this.labelHelp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.etchBottom);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.next);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WelcomeDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ShipWorks Setup";
            this.Load += new System.EventHandler(this.OnLoad);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label description;
        private System.Windows.Forms.Label etchTop;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button next;
        private System.Windows.Forms.Label etchBottom;
        private System.Windows.Forms.Label labelHelp;
        private UI.Controls.LinkControl linkControlSupportForum;
    }
}