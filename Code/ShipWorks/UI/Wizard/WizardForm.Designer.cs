namespace ShipWorks.UI.Wizard
{
    partial class WizardForm
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
            this.next = new ShipWorks.UI.Controls.ShieldButton();
            this.cancel = new System.Windows.Forms.Button();
            this.topPanel = new System.Windows.Forms.Panel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.description = new System.Windows.Forms.Label();
            this.etchTop = new System.Windows.Forms.Label();
            this.title = new System.Windows.Forms.Label();
            this.back = new System.Windows.Forms.Button();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.etchBottom = new System.Windows.Forms.Label();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).BeginInit();
            this.SuspendLayout();
            //
            // next
            //
            this.next.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.next.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.next.Location = new System.Drawing.Point(358, 350);
            this.next.Name = "next";
            this.next.ShowShield = false;
            this.next.Size = new System.Drawing.Size(75, 23);
            this.next.TabIndex = 0;
            this.next.Text = "Next >";
            this.next.UseVisualStyleBackColor = true;
            this.next.Click += new System.EventHandler(this.OnNext);
            //
            // cancel
            //
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(439, 350);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.OnCancel);
            //
            // topPanel
            //
            this.topPanel.BackColor = System.Drawing.Color.White;
            this.topPanel.Controls.Add(this.pictureBox);
            this.topPanel.Controls.Add(this.description);
            this.topPanel.Controls.Add(this.etchTop);
            this.topPanel.Controls.Add(this.title);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(526, 56);
            this.topPanel.TabIndex = 2;
            //
            // pictureBox
            //
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox.Location = new System.Drawing.Point(473, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(50, 50);
            this.pictureBox.TabIndex = 8;
            this.pictureBox.TabStop = false;
            //
            // description
            //
            this.description.AutoSize = true;
            this.description.Location = new System.Drawing.Point(40, 32);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(173, 13);
            this.description.TabIndex = 7;
            this.description.Text = "This is the description of the page.";
            //
            // etchTop
            //
            this.etchTop.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.etchTop.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.etchTop.Location = new System.Drawing.Point(0, 54);
            this.etchTop.Margin = new System.Windows.Forms.Padding(0);
            this.etchTop.Name = "etchTop";
            this.etchTop.Size = new System.Drawing.Size(526, 2);
            this.etchTop.TabIndex = 6;
            //
            // title
            //
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.title.Location = new System.Drawing.Point(20, 12);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(61, 13);
            this.title.TabIndex = 0;
            this.title.Text = "Page title";
            //
            // back
            //
            this.back.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.back.Location = new System.Drawing.Point(277, 350);
            this.back.Name = "back";
            this.back.Size = new System.Drawing.Size(75, 23);
            this.back.TabIndex = 3;
            this.back.Text = "< Back";
            this.back.UseVisualStyleBackColor = true;
            this.back.Click += new System.EventHandler(this.OnBack);
            //
            // mainPanel
            //
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPanel.Location = new System.Drawing.Point(0, 59);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(526, 278);
            this.mainPanel.TabIndex = 4;
            //
            // etchBottom
            //
            this.etchBottom.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.etchBottom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.etchBottom.Location = new System.Drawing.Point(0, 340);
            this.etchBottom.Margin = new System.Windows.Forms.Padding(0);
            this.etchBottom.Name = "etchBottom";
            this.etchBottom.Size = new System.Drawing.Size(530, 2);
            this.etchBottom.TabIndex = 5;
            //
            // WizardForm
            //
            this.AcceptButton = this.next;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(526, 385);
            this.Controls.Add(this.etchBottom);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.back);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.next);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WizardForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Wizard Title";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Label etchTop;
        protected ShipWorks.UI.Controls.ShieldButton next;
        protected System.Windows.Forms.Button cancel;
        protected System.Windows.Forms.Button back;
        protected System.Windows.Forms.Panel mainPanel;
        protected System.Windows.Forms.Label etchBottom;
        private System.Windows.Forms.Label description;
        protected System.Windows.Forms.PictureBox pictureBox;
        protected System.Windows.Forms.Panel topPanel;
    }
}