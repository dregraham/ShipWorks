namespace ShipWorks.Data.Administration
{
    partial class SimpleDatabaseSetupWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleDatabaseSetupWizard));
            this.wizardPageWelcome = new ShipWorks.UI.Wizard.WizardPage();
            this.startFromScratch = new ShipWorks.UI.Controls.ShieldButton();
            this.detailedSetup = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelWelcomeGlad = new System.Windows.Forms.Label();
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
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageWelcome.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCubes)).BeginInit();
            this.wizardPagePrepare.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePreparing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreparing)).BeginInit();
            this.wizardPageFinishExisting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(380, 379);
            this.next.ShowShield = true;
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(461, 379);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(299, 379);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageWelcome);
            this.mainPanel.Size = new System.Drawing.Size(548, 307);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 369);
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
            this.wizardPageWelcome.Controls.Add(this.startFromScratch);
            this.wizardPageWelcome.Controls.Add(this.detailedSetup);
            this.wizardPageWelcome.Controls.Add(this.pictureBox2);
            this.wizardPageWelcome.Controls.Add(this.kryptonBorderEdge);
            this.wizardPageWelcome.Controls.Add(this.labelWelcomeGlad);
            this.wizardPageWelcome.Controls.Add(this.labelWelcomeAdvanced);
            this.wizardPageWelcome.Controls.Add(this.pictureBoxCubes);
            this.wizardPageWelcome.Description = "Thank you for choosing ShipWorks!";
            this.wizardPageWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageWelcome.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageWelcome.Location = new System.Drawing.Point(0, 0);
            this.wizardPageWelcome.Name = "wizardPageWelcome";
            this.wizardPageWelcome.NextRequiresElevation = true;
            this.wizardPageWelcome.Size = new System.Drawing.Size(548, 307);
            this.wizardPageWelcome.TabIndex = 0;
            this.wizardPageWelcome.Title = "Welcome to ShipWorks";
            this.wizardPageWelcome.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextWelcome);
            this.wizardPageWelcome.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoWelcome);
            // 
            // startFromScratch
            // 
            this.startFromScratch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startFromScratch.Location = new System.Drawing.Point(103, 36);
            this.startFromScratch.Name = "startFromScratch";
            this.startFromScratch.Size = new System.Drawing.Size(165, 23);
            this.startFromScratch.TabIndex = 1;
            this.startFromScratch.Text = "Start from scratch >";
            this.startFromScratch.UseVisualStyleBackColor = true;
            this.startFromScratch.Click += new System.EventHandler(this.OnStartFromScratch);
            // 
            // detailedSetup
            // 
            this.detailedSetup.Location = new System.Drawing.Point(105, 107);
            this.detailedSetup.Name = "detailedSetup";
            this.detailedSetup.Size = new System.Drawing.Size(163, 23);
            this.detailedSetup.TabIndex = 3;
            this.detailedSetup.Text = "Get set up >";
            this.detailedSetup.UseVisualStyleBackColor = true;
            this.detailedSetup.Click += new System.EventHandler(this.OnOpenDetailedSetup);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ShipWorks.Properties.Resources.workplace_add;
            this.pictureBox2.Location = new System.Drawing.Point(43, 85);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(48, 48);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 51;
            this.pictureBox2.TabStop = false;
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonBorderEdge.AutoSize = false;
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(106, 73);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(342, 1);
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            // 
            // labelWelcomeGlad
            // 
            this.labelWelcomeGlad.AutoSize = true;
            this.labelWelcomeGlad.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWelcomeGlad.Location = new System.Drawing.Point(103, 17);
            this.labelWelcomeGlad.Name = "labelWelcomeGlad";
            this.labelWelcomeGlad.Size = new System.Drawing.Size(161, 13);
            this.labelWelcomeGlad.TabIndex = 0;
            this.labelWelcomeGlad.Text = "Are you new to ShipWorks?";
            // 
            // labelWelcomeAdvanced
            // 
            this.labelWelcomeAdvanced.AutoSize = true;
            this.labelWelcomeAdvanced.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWelcomeAdvanced.Location = new System.Drawing.Point(103, 89);
            this.labelWelcomeAdvanced.Name = "labelWelcomeAdvanced";
            this.labelWelcomeAdvanced.Size = new System.Drawing.Size(184, 13);
            this.labelWelcomeAdvanced.TabIndex = 2;
            this.labelWelcomeAdvanced.Text = "Do you already use ShipWorks?";
            // 
            // pictureBoxCubes
            // 
            this.pictureBoxCubes.Image = global::ShipWorks.Properties.Resources.sw_cubes_big;
            this.pictureBoxCubes.Location = new System.Drawing.Point(41, 14);
            this.pictureBoxCubes.Name = "pictureBoxCubes";
            this.pictureBoxCubes.Size = new System.Drawing.Size(50, 48);
            this.pictureBoxCubes.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
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
            this.wizardPagePrepare.Size = new System.Drawing.Size(548, 307);
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
            this.wizardPageFinishExisting.Size = new System.Drawing.Size(548, 307);
            this.wizardPageFinishExisting.TabIndex = 0;
            this.wizardPageFinishExisting.Title = "ShipWorks Setup";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(65, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(293, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "You\'re ShipWorks data is already installed on this computer.";
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
            // SimpleDatabaseSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 413);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximumSize = new System.Drawing.Size(564, 452);
            this.MinimumSize = new System.Drawing.Size(564, 452);
            this.Name = "SimpleDatabaseSetupWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageWelcome,
            this.wizardPagePrepare,
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCubes)).EndInit();
            this.wizardPagePrepare.ResumeLayout(false);
            this.wizardPagePrepare.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePreparing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreparing)).EndInit();
            this.wizardPageFinishExisting.ResumeLayout(false);
            this.wizardPageFinishExisting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Wizard.WizardPage wizardPageWelcome;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        private System.Windows.Forms.Label labelWelcomeGlad;
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
        private System.Windows.Forms.PictureBox pictureBox2;
        private UI.Controls.ShieldButton startFromScratch;
        private System.Windows.Forms.Button detailedSetup;
    }
}