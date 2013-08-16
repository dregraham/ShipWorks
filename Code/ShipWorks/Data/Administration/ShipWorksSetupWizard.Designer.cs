﻿namespace ShipWorks.Data.Administration
{
    partial class ShipWorksSetupWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShipWorksSetupWizard));
            this.wizardPageWelcome = new ShipWorks.UI.Wizard.WizardPage();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelWelcomeNext = new System.Windows.Forms.Label();
            this.labelWelcomeGlad = new System.Windows.Forms.Label();
            this.linkDetailedSetup = new ShipWorks.UI.Controls.LinkControl();
            this.labelWelcomeAdvanced = new System.Windows.Forms.Label();
            this.pictureBoxCubes = new System.Windows.Forms.PictureBox();
            this.wizardPagePrepare = new ShipWorks.UI.Wizard.WizardPage();
            this.progressPreparing = new System.Windows.Forms.ProgressBar();
            this.picturePreparing = new System.Windows.Forms.PictureBox();
            this.labelPeparingToRun = new System.Windows.Forms.Label();
            this.pictureBoxPreparing = new System.Windows.Forms.PictureBox();
            this.labelPreparing = new System.Windows.Forms.Label();
            this.wizardPageFinishExisting = new ShipWorks.UI.Wizard.WizardPage();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.localDbProgressTimer = new System.Windows.Forms.Timer(this.components);
            this.wizardPageUser = new ShipWorks.UI.Wizard.WizardPage();
            this.helpUserEmail = new ShipWorks.UI.Controls.InfoTip();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.swEmail = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.swPasswordAgain = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.swPassword = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.swUsername = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageWelcome.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCubes)).BeginInit();
            this.wizardPagePrepare.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePreparing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreparing)).BeginInit();
            this.wizardPageFinishExisting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.wizardPageUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(380, 343);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(461, 343);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(299, 343);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageUser);
            this.mainPanel.Size = new System.Drawing.Size(548, 271);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 333);
            this.etchBottom.Size = new System.Drawing.Size(552, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.sw_cubes_big;
            this.pictureBox.Location = new System.Drawing.Point(486, 3);
            this.pictureBox.Size = new System.Drawing.Size(54, 50);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(548, 56);
            // 
            // wizardPageWelcome
            // 
            this.wizardPageWelcome.Controls.Add(this.kryptonBorderEdge);
            this.wizardPageWelcome.Controls.Add(this.labelWelcomeNext);
            this.wizardPageWelcome.Controls.Add(this.labelWelcomeGlad);
            this.wizardPageWelcome.Controls.Add(this.linkDetailedSetup);
            this.wizardPageWelcome.Controls.Add(this.labelWelcomeAdvanced);
            this.wizardPageWelcome.Controls.Add(this.pictureBoxCubes);
            this.wizardPageWelcome.Description = "Thank you for choosing ShipWorks!";
            this.wizardPageWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageWelcome.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageWelcome.Location = new System.Drawing.Point(0, 0);
            this.wizardPageWelcome.Name = "wizardPageWelcome";
            this.wizardPageWelcome.NextRequiresElevation = true;
            this.wizardPageWelcome.Size = new System.Drawing.Size(548, 271);
            this.wizardPageWelcome.TabIndex = 0;
            this.wizardPageWelcome.Title = "Welcome to ShipWorks";
            this.wizardPageWelcome.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextWelcome);
            this.wizardPageWelcome.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoWelcome);
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonBorderEdge.AutoSize = false;
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(88, 49);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(342, 1);
            this.kryptonBorderEdge.TabIndex = 50;
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            // 
            // labelWelcomeNext
            // 
            this.labelWelcomeNext.AutoSize = true;
            this.labelWelcomeNext.Location = new System.Drawing.Point(85, 30);
            this.labelWelcomeNext.Name = "labelWelcomeNext";
            this.labelWelcomeNext.Size = new System.Drawing.Size(104, 13);
            this.labelWelcomeNext.TabIndex = 49;
            this.labelWelcomeNext.Text = "Click \'Next\' to begin.";
            // 
            // labelWelcomeGlad
            // 
            this.labelWelcomeGlad.AutoSize = true;
            this.labelWelcomeGlad.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWelcomeGlad.Location = new System.Drawing.Point(84, 13);
            this.labelWelcomeGlad.Name = "labelWelcomeGlad";
            this.labelWelcomeGlad.Size = new System.Drawing.Size(140, 13);
            this.labelWelcomeGlad.TabIndex = 46;
            this.labelWelcomeGlad.Text = "Welcome to ShipWorks!";
            // 
            // linkDetailedSetup
            // 
            this.linkDetailedSetup.AutoSize = true;
            this.linkDetailedSetup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkDetailedSetup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkDetailedSetup.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.linkDetailedSetup.Location = new System.Drawing.Point(86, 77);
            this.linkDetailedSetup.Name = "linkDetailedSetup";
            this.linkDetailedSetup.Size = new System.Drawing.Size(141, 13);
            this.linkDetailedSetup.TabIndex = 45;
            this.linkDetailedSetup.Text = "Switch to the detailed setup";
            this.linkDetailedSetup.Click += new System.EventHandler(this.OnLinkDetailedSetup);
            // 
            // labelWelcomeAdvanced
            // 
            this.labelWelcomeAdvanced.AutoSize = true;
            this.labelWelcomeAdvanced.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelWelcomeAdvanced.Location = new System.Drawing.Point(86, 60);
            this.labelWelcomeAdvanced.Name = "labelWelcomeAdvanced";
            this.labelWelcomeAdvanced.Size = new System.Drawing.Size(227, 13);
            this.labelWelcomeAdvanced.TabIndex = 44;
            this.labelWelcomeAdvanced.Text = "Is ShipWorks already installed on another PC?";
            // 
            // pictureBoxCubes
            // 
            this.pictureBoxCubes.Image = global::ShipWorks.Properties.Resources.sw_cubes_big;
            this.pictureBoxCubes.Location = new System.Drawing.Point(24, 8);
            this.pictureBoxCubes.Name = "pictureBoxCubes";
            this.pictureBoxCubes.Size = new System.Drawing.Size(54, 50);
            this.pictureBoxCubes.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxCubes.TabIndex = 43;
            this.pictureBoxCubes.TabStop = false;
            // 
            // wizardPagePrepare
            // 
            this.wizardPagePrepare.Controls.Add(this.progressPreparing);
            this.wizardPagePrepare.Controls.Add(this.picturePreparing);
            this.wizardPagePrepare.Controls.Add(this.labelPeparingToRun);
            this.wizardPagePrepare.Controls.Add(this.pictureBoxPreparing);
            this.wizardPagePrepare.Controls.Add(this.labelPreparing);
            this.wizardPagePrepare.Description = "ShipWorks needs to prepare your system.";
            this.wizardPagePrepare.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPagePrepare.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPagePrepare.Location = new System.Drawing.Point(0, 0);
            this.wizardPagePrepare.Name = "wizardPagePrepare";
            this.wizardPagePrepare.Size = new System.Drawing.Size(548, 271);
            this.wizardPagePrepare.TabIndex = 0;
            this.wizardPagePrepare.Title = "ShipWorks Setup";
            this.wizardPagePrepare.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextPrepareAutomaticDatabase);
            this.wizardPagePrepare.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoPrepareAutomaticDatabase);
            this.wizardPagePrepare.Cancelling += new System.ComponentModel.CancelEventHandler(this.OnCancelPrepareAutomaticDatabase);
            // 
            // progressPreparing
            // 
            this.progressPreparing.Location = new System.Drawing.Point(86, 44);
            this.progressPreparing.Name = "progressPreparing";
            this.progressPreparing.Size = new System.Drawing.Size(379, 23);
            this.progressPreparing.TabIndex = 47;
            // 
            // picturePreparing
            // 
            this.picturePreparing.Image = ((System.Drawing.Image)(resources.GetObject("picturePreparing.Image")));
            this.picturePreparing.Location = new System.Drawing.Point(64, 47);
            this.picturePreparing.Name = "picturePreparing";
            this.picturePreparing.Size = new System.Drawing.Size(16, 16);
            this.picturePreparing.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picturePreparing.TabIndex = 46;
            this.picturePreparing.TabStop = false;
            // 
            // labelPeparingToRun
            // 
            this.labelPeparingToRun.AutoSize = true;
            this.labelPeparingToRun.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelPeparingToRun.Location = new System.Drawing.Point(62, 26);
            this.labelPeparingToRun.Name = "labelPeparingToRun";
            this.labelPeparingToRun.Size = new System.Drawing.Size(415, 13);
            this.labelPeparingToRun.TabIndex = 45;
            this.labelPeparingToRun.Text = "ShipWorks is preparing to run on your computer.  This may take just a few minutes" +
    "...";
            // 
            // pictureBoxPreparing
            // 
            this.pictureBoxPreparing.Image = global::ShipWorks.Properties.Resources.gears_preferences;
            this.pictureBoxPreparing.Location = new System.Drawing.Point(25, 8);
            this.pictureBoxPreparing.Name = "pictureBoxPreparing";
            this.pictureBoxPreparing.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxPreparing.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxPreparing.TabIndex = 44;
            this.pictureBoxPreparing.TabStop = false;
            // 
            // labelPreparing
            // 
            this.labelPreparing.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPreparing.Location = new System.Drawing.Point(61, 10);
            this.labelPreparing.Name = "labelPreparing";
            this.labelPreparing.Size = new System.Drawing.Size(404, 16);
            this.labelPreparing.TabIndex = 43;
            this.labelPreparing.Text = "Preparing to Run";
            // 
            // wizardPageFinishExisting
            // 
            this.wizardPageFinishExisting.Controls.Add(this.label2);
            this.wizardPageFinishExisting.Controls.Add(this.label1);
            this.wizardPageFinishExisting.Controls.Add(this.pictureBox1);
            this.wizardPageFinishExisting.Description = "ShipWorks is ready to start.";
            this.wizardPageFinishExisting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageFinishExisting.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageFinishExisting.Location = new System.Drawing.Point(0, 0);
            this.wizardPageFinishExisting.Name = "wizardPageFinishExisting";
            this.wizardPageFinishExisting.Size = new System.Drawing.Size(526, 278);
            this.wizardPageFinishExisting.TabIndex = 0;
            this.wizardPageFinishExisting.Title = "ShipWorks Setup";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(65, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(437, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "We found your ShipWorks data already installed on this computer.  You\'re all read" +
    "y to go.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(63, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Welcome back to ShipWorks!";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.check32;
            this.pictureBox1.Location = new System.Drawing.Point(25, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // localDbProgressTimer
            // 
            this.localDbProgressTimer.Tick += new System.EventHandler(this.OnLocalDbProgressTimer);
            // 
            // wizardPageUser
            // 
            this.wizardPageUser.Controls.Add(this.helpUserEmail);
            this.wizardPageUser.Controls.Add(this.pictureBox6);
            this.wizardPageUser.Controls.Add(this.swEmail);
            this.wizardPageUser.Controls.Add(this.label7);
            this.wizardPageUser.Controls.Add(this.swPasswordAgain);
            this.wizardPageUser.Controls.Add(this.label6);
            this.wizardPageUser.Controls.Add(this.swPassword);
            this.wizardPageUser.Controls.Add(this.label8);
            this.wizardPageUser.Controls.Add(this.swUsername);
            this.wizardPageUser.Controls.Add(this.label9);
            this.wizardPageUser.Controls.Add(this.label10);
            this.wizardPageUser.Description = "Create a user account to log on to ShipWorks.";
            this.wizardPageUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageUser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageUser.Location = new System.Drawing.Point(0, 0);
            this.wizardPageUser.Name = "wizardPageUser";
            this.wizardPageUser.Size = new System.Drawing.Size(548, 271);
            this.wizardPageUser.TabIndex = 0;
            this.wizardPageUser.Title = "ShipWorks Account";
            this.wizardPageUser.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextCreateUsername);
            // 
            // helpUserEmail
            // 
            this.helpUserEmail.Caption = "Your email address will be used to send you a new password if its forgotten.";
            this.helpUserEmail.Location = new System.Drawing.Point(401, 64);
            this.helpUserEmail.Name = "helpUserEmail";
            this.helpUserEmail.Size = new System.Drawing.Size(12, 12);
            this.helpUserEmail.TabIndex = 195;
            this.helpUserEmail.Title = "Email Address";
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = global::ShipWorks.Properties.Resources.dude31;
            this.pictureBox6.Location = new System.Drawing.Point(24, 9);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(32, 32);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox6.TabIndex = 194;
            this.pictureBox6.TabStop = false;
            // 
            // swEmail
            // 
            this.swEmail.Location = new System.Drawing.Point(150, 60);
            this.swEmail.Name = "swEmail";
            this.swEmail.Size = new System.Drawing.Size(243, 21);
            this.swEmail.TabIndex = 186;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(68, 63);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 193;
            this.label7.Text = "Email address:";
            // 
            // swPasswordAgain
            // 
            this.swPasswordAgain.Location = new System.Drawing.Point(150, 114);
            this.swPasswordAgain.Name = "swPasswordAgain";
            this.swPasswordAgain.Size = new System.Drawing.Size(243, 21);
            this.swPasswordAgain.TabIndex = 188;
            this.swPasswordAgain.UseSystemPasswordChar = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(49, 117);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 192;
            this.label6.Text = "Retype password:";
            // 
            // swPassword
            // 
            this.swPassword.Location = new System.Drawing.Point(150, 87);
            this.swPassword.Name = "swPassword";
            this.swPassword.Size = new System.Drawing.Size(243, 21);
            this.swPassword.TabIndex = 187;
            this.swPassword.UseSystemPasswordChar = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(87, 90);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 191;
            this.label8.Text = "Password:";
            // 
            // swUsername
            // 
            this.swUsername.Location = new System.Drawing.Point(150, 33);
            this.swUsername.Name = "swUsername";
            this.swUsername.Size = new System.Drawing.Size(243, 21);
            this.swUsername.TabIndex = 185;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(85, 36);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 13);
            this.label9.TabIndex = 190;
            this.label9.Text = "Username:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(65, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(172, 13);
            this.label10.TabIndex = 189;
            this.label10.Text = "Create your ShipWorks username:";
            // 
            // ShipWorksSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 378);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimumSize = new System.Drawing.Size(554, 406);
            this.Name = "ShipWorksSetupWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageWelcome,
            this.wizardPagePrepare,
            this.wizardPageUser,
            this.wizardPageFinishExisting});
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ShipWorks Setup";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnClosed);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageWelcome.ResumeLayout(false);
            this.wizardPageWelcome.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCubes)).EndInit();
            this.wizardPagePrepare.ResumeLayout(false);
            this.wizardPagePrepare.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePreparing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreparing)).EndInit();
            this.wizardPageFinishExisting.ResumeLayout(false);
            this.wizardPageFinishExisting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.wizardPageUser.ResumeLayout(false);
            this.wizardPageUser.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Wizard.WizardPage wizardPageWelcome;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        private System.Windows.Forms.Label labelWelcomeNext;
        private System.Windows.Forms.Label labelWelcomeGlad;
        private UI.Controls.LinkControl linkDetailedSetup;
        private System.Windows.Forms.Label labelWelcomeAdvanced;
        private System.Windows.Forms.PictureBox pictureBoxCubes;
        private UI.Wizard.WizardPage wizardPagePrepare;
        private System.Windows.Forms.ProgressBar progressPreparing;
        private System.Windows.Forms.PictureBox picturePreparing;
        private System.Windows.Forms.Label labelPeparingToRun;
        private System.Windows.Forms.PictureBox pictureBoxPreparing;
        private System.Windows.Forms.Label labelPreparing;
        private UI.Wizard.WizardPage wizardPageFinishExisting;
        private System.Windows.Forms.Timer localDbProgressTimer;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private UI.Wizard.WizardPage wizardPageUser;
        private UI.Controls.InfoTip helpUserEmail;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.TextBox swEmail;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox swPasswordAgain;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox swPassword;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox swUsername;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
    }
}