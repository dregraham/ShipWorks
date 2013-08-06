namespace ShipWorks.ApplicationCore.Crashes
{
    partial class CrashWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CrashWindow));
            this.labelDisclaimer = new System.Windows.Forms.Label();
            this.crashImage = new System.Windows.Forms.PictureBox();
            this.labelProblem = new System.Windows.Forms.Label();
            this.labelEmail = new System.Windows.Forms.Label();
            this.email = new System.Windows.Forms.TextBox();
            this.labelInfo = new System.Windows.Forms.Label();
            this.userComments = new System.Windows.Forms.TextBox();
            this.send = new System.Windows.Forms.Button();
            this.reportDetailsLink = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.logSize = new System.Windows.Forms.Label();
            this.labelLogInfo1 = new System.Windows.Forms.Label();
            this.includeLogFiles = new System.Windows.Forms.CheckBox();
            this.labelLogs = new System.Windows.Forms.Label();
            this.linkAddDescriptionLines = new ShipWorks.UI.Controls.LinkControl();
            ((System.ComponentModel.ISupportInitialize) (this.crashImage)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelDisclaimer
            // 
            this.labelDisclaimer.AutoSize = true;
            this.labelDisclaimer.Location = new System.Drawing.Point(79, 32);
            this.labelDisclaimer.Name = "labelDisclaimer";
            this.labelDisclaimer.Size = new System.Drawing.Size(333, 13);
            this.labelDisclaimer.TabIndex = 1;
            this.labelDisclaimer.Text = "Please help us to improve ShipWorks by submitting this error report.";
            // 
            // crashImage
            // 
            this.crashImage.Image = ((System.Drawing.Image) (resources.GetObject("crashImage.Image")));
            this.crashImage.Location = new System.Drawing.Point(9, 3);
            this.crashImage.Name = "crashImage";
            this.crashImage.Size = new System.Drawing.Size(64, 54);
            this.crashImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.crashImage.TabIndex = 1;
            this.crashImage.TabStop = false;
            // 
            // labelProblem
            // 
            this.labelProblem.AutoSize = true;
            this.labelProblem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelProblem.Location = new System.Drawing.Point(79, 13);
            this.labelProblem.Name = "labelProblem";
            this.labelProblem.Size = new System.Drawing.Size(228, 13);
            this.labelProblem.TabIndex = 0;
            this.labelProblem.Text = "ShipWorks has encountered a problem.";
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(12, 71);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(97, 13);
            this.labelEmail.TabIndex = 2;
            this.labelEmail.Text = "Your email address";
            // 
            // email
            // 
            this.email.Location = new System.Drawing.Point(29, 89);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(197, 21);
            this.email.TabIndex = 3;
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelInfo.Location = new System.Drawing.Point(12, 118);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(115, 13);
            this.labelInfo.TabIndex = 4;
            this.labelInfo.Text = "What were you doing?";
            // 
            // userComments
            // 
            this.userComments.AcceptsReturn = true;
            this.userComments.Location = new System.Drawing.Point(29, 136);
            this.userComments.Name = "userComments";
            this.userComments.Size = new System.Drawing.Size(314, 21);
            this.userComments.TabIndex = 5;
            // 
            // send
            // 
            this.send.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.send.Location = new System.Drawing.Point(346, 75);
            this.send.Name = "send";
            this.send.Size = new System.Drawing.Size(75, 23);
            this.send.TabIndex = 6;
            this.send.Text = "Submit";
            this.send.UseVisualStyleBackColor = true;
            this.send.Click += new System.EventHandler(this.OnSendReport);
            // 
            // reportDetailsLink
            // 
            this.reportDetailsLink.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.reportDetailsLink.AutoSize = true;
            this.reportDetailsLink.LinkArea = new System.Windows.Forms.LinkArea(34, 10);
            this.reportDetailsLink.Location = new System.Drawing.Point(5, 80);
            this.reportDetailsLink.Name = "reportDetailsLink";
            this.reportDetailsLink.Size = new System.Drawing.Size(223, 18);
            this.reportDetailsLink.TabIndex = 8;
            this.reportDetailsLink.TabStop = true;
            this.reportDetailsLink.Text = "To see what this report contains, click here.";
            this.reportDetailsLink.UseCompatibleTextRendering = true;
            this.reportDetailsLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnViewReport);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label1.Location = new System.Drawing.Point(110, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "(In case we have follow-up questions)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label2.Location = new System.Drawing.Point(125, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(242, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "(This really helps us figure out what went wrong)";
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.logSize);
            this.panelBottom.Controls.Add(this.labelLogInfo1);
            this.panelBottom.Controls.Add(this.includeLogFiles);
            this.panelBottom.Controls.Add(this.reportDetailsLink);
            this.panelBottom.Controls.Add(this.labelLogs);
            this.panelBottom.Controls.Add(this.send);
            this.panelBottom.Location = new System.Drawing.Point(0, 163);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(432, 108);
            this.panelBottom.TabIndex = 12;
            // 
            // logSize
            // 
            this.logSize.AutoSize = true;
            this.logSize.ForeColor = System.Drawing.SystemColors.GrayText;
            this.logSize.Location = new System.Drawing.Point(227, 49);
            this.logSize.Name = "logSize";
            this.logSize.Size = new System.Drawing.Size(100, 13);
            this.logSize.TabIndex = 16;
            this.logSize.Text = "(Calculating size...)";
            // 
            // labelLogInfo1
            // 
            this.labelLogInfo1.AutoSize = true;
            this.labelLogInfo1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelLogInfo1.Location = new System.Drawing.Point(44, 49);
            this.labelLogInfo1.Name = "labelLogInfo1";
            this.labelLogInfo1.Size = new System.Drawing.Size(186, 13);
            this.labelLogInfo1.TabIndex = 15;
            this.labelLogInfo1.Text = "Only the most recent logs will be sent";
            // 
            // includeLogFiles
            // 
            this.includeLogFiles.AutoSize = true;
            this.includeLogFiles.Checked = true;
            this.includeLogFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.includeLogFiles.Location = new System.Drawing.Point(26, 29);
            this.includeLogFiles.Name = "includeLogFiles";
            this.includeLogFiles.Size = new System.Drawing.Size(259, 17);
            this.includeLogFiles.TabIndex = 14;
            this.includeLogFiles.Text = "Include ShipWorks log files with the error report.";
            this.includeLogFiles.UseVisualStyleBackColor = true;
            // 
            // labelLogs
            // 
            this.labelLogs.AutoSize = true;
            this.labelLogs.Location = new System.Drawing.Point(12, 9);
            this.labelLogs.Name = "labelLogs";
            this.labelLogs.Size = new System.Drawing.Size(48, 13);
            this.labelLogs.TabIndex = 13;
            this.labelLogs.Text = "Log Files";
            // 
            // linkAddDescriptionLines
            // 
            this.linkAddDescriptionLines.AutoSize = true;
            this.linkAddDescriptionLines.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkAddDescriptionLines.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkAddDescriptionLines.ForeColor = System.Drawing.Color.RoyalBlue;
            this.linkAddDescriptionLines.Location = new System.Drawing.Point(347, 139);
            this.linkAddDescriptionLines.Name = "linkAddDescriptionLines";
            this.linkAddDescriptionLines.Size = new System.Drawing.Size(77, 13);
            this.linkAddDescriptionLines.TabIndex = 11;
            this.linkAddDescriptionLines.Text = "Add more lines";
            this.linkAddDescriptionLines.Click += new System.EventHandler(this.OnAddDescriptionLines);
            // 
            // CrashWindow
            // 
            this.AcceptButton = this.send;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 272);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.linkAddDescriptionLines);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.userComments);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.email);
            this.Controls.Add(this.labelEmail);
            this.Controls.Add(this.labelProblem);
            this.Controls.Add(this.crashImage);
            this.Controls.Add(this.labelDisclaimer);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CrashWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShipWorks";
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize) (this.crashImage)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelDisclaimer;
        private System.Windows.Forms.PictureBox crashImage;
        private System.Windows.Forms.Label labelProblem;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.TextBox email;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.TextBox userComments;
        private System.Windows.Forms.Button send;
        private System.Windows.Forms.LinkLabel reportDetailsLink;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private ShipWorks.UI.Controls.LinkControl linkAddDescriptionLines;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.CheckBox includeLogFiles;
        private System.Windows.Forms.Label labelLogs;
        private System.Windows.Forms.Label logSize;
        private System.Windows.Forms.Label labelLogInfo1;
    }
}