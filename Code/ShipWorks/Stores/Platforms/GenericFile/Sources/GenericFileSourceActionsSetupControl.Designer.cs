namespace ShipWorks.Stores.Platforms.GenericFile.Sources
{
    partial class GenericFileSourceActionsSetupControl
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
            this.comboSuccessAction = new System.Windows.Forms.ComboBox();
            this.panelError = new System.Windows.Forms.Panel();
            this.comboErrorAction = new System.Windows.Forms.ComboBox();
            this.panelErrorFolder = new System.Windows.Forms.Panel();
            this.labelErrorFolder = new System.Windows.Forms.Label();
            this.errorFolderBrowse = new System.Windows.Forms.Button();
            this.labelError = new System.Windows.Forms.Label();
            this.labelSuccess = new System.Windows.Forms.Label();
            this.panelSuccessFolder = new System.Windows.Forms.Panel();
            this.labelSuccessFolder = new System.Windows.Forms.Label();
            this.successFolderBrowse = new System.Windows.Forms.Button();
            this.errorFolder = new ShipWorks.UI.Controls.PathTextBox();
            this.successFolder = new ShipWorks.UI.Controls.PathTextBox();
            this.panelError.SuspendLayout();
            this.panelErrorFolder.SuspendLayout();
            this.panelSuccessFolder.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboSuccessAction
            // 
            this.comboSuccessAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSuccessAction.FormattingEnabled = true;
            this.comboSuccessAction.Location = new System.Drawing.Point(25, 24);
            this.comboSuccessAction.Name = "comboSuccessAction";
            this.comboSuccessAction.Size = new System.Drawing.Size(236, 21);
            this.comboSuccessAction.TabIndex = 97;
            this.comboSuccessAction.SelectedIndexChanged += new System.EventHandler(this.OnChangeSuccessAction);
            // 
            // panelError
            // 
            this.panelError.Controls.Add(this.comboErrorAction);
            this.panelError.Controls.Add(this.panelErrorFolder);
            this.panelError.Controls.Add(this.labelError);
            this.panelError.Location = new System.Drawing.Point(3, 82);
            this.panelError.Name = "panelError";
            this.panelError.Size = new System.Drawing.Size(478, 81);
            this.panelError.TabIndex = 99;
            // 
            // comboErrorAction
            // 
            this.comboErrorAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboErrorAction.FormattingEnabled = true;
            this.comboErrorAction.Location = new System.Drawing.Point(22, 23);
            this.comboErrorAction.Name = "comboErrorAction";
            this.comboErrorAction.Size = new System.Drawing.Size(236, 21);
            this.comboErrorAction.TabIndex = 95;
            this.comboErrorAction.SelectedIndexChanged += new System.EventHandler(this.OnChangeErrorAction);
            // 
            // panelErrorFolder
            // 
            this.panelErrorFolder.Controls.Add(this.labelErrorFolder);
            this.panelErrorFolder.Controls.Add(this.errorFolder);
            this.panelErrorFolder.Controls.Add(this.errorFolderBrowse);
            this.panelErrorFolder.Location = new System.Drawing.Point(3, 47);
            this.panelErrorFolder.Name = "panelErrorFolder";
            this.panelErrorFolder.Size = new System.Drawing.Size(472, 32);
            this.panelErrorFolder.TabIndex = 96;
            // 
            // labelErrorFolder
            // 
            this.labelErrorFolder.AutoSize = true;
            this.labelErrorFolder.Location = new System.Drawing.Point(20, 6);
            this.labelErrorFolder.Name = "labelErrorFolder";
            this.labelErrorFolder.Size = new System.Drawing.Size(41, 13);
            this.labelErrorFolder.TabIndex = 96;
            this.labelErrorFolder.Text = "Folder:";
            // 
            // errorFolderBrowse
            // 
            this.errorFolderBrowse.Location = new System.Drawing.Point(397, 1);
            this.errorFolderBrowse.Name = "errorFolderBrowse";
            this.errorFolderBrowse.Size = new System.Drawing.Size(75, 23);
            this.errorFolderBrowse.TabIndex = 94;
            this.errorFolderBrowse.Text = "Browse...";
            this.errorFolderBrowse.UseVisualStyleBackColor = true;
            this.errorFolderBrowse.Click += new System.EventHandler(this.OnBrowseErrorFolder);
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelError.Location = new System.Drawing.Point(0, 4);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(198, 13);
            this.labelError.TabIndex = 93;
            this.labelError.Text = "If an error occurs while importing:";
            // 
            // labelSuccess
            // 
            this.labelSuccess.AutoSize = true;
            this.labelSuccess.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelSuccess.Location = new System.Drawing.Point(3, 5);
            this.labelSuccess.Name = "labelSuccess";
            this.labelSuccess.Size = new System.Drawing.Size(151, 13);
            this.labelSuccess.TabIndex = 96;
            this.labelSuccess.Text = "After a successful import:";
            // 
            // panelSuccessFolder
            // 
            this.panelSuccessFolder.Controls.Add(this.labelSuccessFolder);
            this.panelSuccessFolder.Controls.Add(this.successFolder);
            this.panelSuccessFolder.Controls.Add(this.successFolderBrowse);
            this.panelSuccessFolder.Location = new System.Drawing.Point(6, 48);
            this.panelSuccessFolder.Name = "panelSuccessFolder";
            this.panelSuccessFolder.Size = new System.Drawing.Size(472, 32);
            this.panelSuccessFolder.TabIndex = 98;
            // 
            // labelSuccessFolder
            // 
            this.labelSuccessFolder.AutoSize = true;
            this.labelSuccessFolder.Location = new System.Drawing.Point(20, 6);
            this.labelSuccessFolder.Name = "labelSuccessFolder";
            this.labelSuccessFolder.Size = new System.Drawing.Size(41, 13);
            this.labelSuccessFolder.TabIndex = 96;
            this.labelSuccessFolder.Text = "Folder:";
            // 
            // successFolderBrowse
            // 
            this.successFolderBrowse.Location = new System.Drawing.Point(397, 1);
            this.successFolderBrowse.Name = "successFolderBrowse";
            this.successFolderBrowse.Size = new System.Drawing.Size(75, 23);
            this.successFolderBrowse.TabIndex = 94;
            this.successFolderBrowse.Text = "Browse...";
            this.successFolderBrowse.UseVisualStyleBackColor = true;
            this.successFolderBrowse.Click += new System.EventHandler(this.OnBrowseSuccessFolder);
            // 
            // errorFolder
            // 
            this.errorFolder.Location = new System.Drawing.Point(63, 3);
            this.errorFolder.Name = "errorFolder";
            this.errorFolder.ReadOnly = true;
            this.errorFolder.Size = new System.Drawing.Size(328, 21);
            this.errorFolder.TabIndex = 95;
            // 
            // successFolder
            // 
            this.successFolder.Location = new System.Drawing.Point(63, 3);
            this.successFolder.Name = "successFolder";
            this.successFolder.ReadOnly = true;
            this.successFolder.Size = new System.Drawing.Size(328, 21);
            this.successFolder.TabIndex = 95;
            // 
            // GenericFileSourceActionsSetupControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboSuccessAction);
            this.Controls.Add(this.panelError);
            this.Controls.Add(this.labelSuccess);
            this.Controls.Add(this.panelSuccessFolder);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "GenericFileSourceActionsSetupControl";
            this.Size = new System.Drawing.Size(490, 170);
            this.panelError.ResumeLayout(false);
            this.panelError.PerformLayout();
            this.panelErrorFolder.ResumeLayout(false);
            this.panelErrorFolder.PerformLayout();
            this.panelSuccessFolder.ResumeLayout(false);
            this.panelSuccessFolder.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboSuccessAction;
        private System.Windows.Forms.Panel panelError;
        private System.Windows.Forms.ComboBox comboErrorAction;
        private System.Windows.Forms.Panel panelErrorFolder;
        private System.Windows.Forms.Label labelErrorFolder;
        private UI.Controls.PathTextBox errorFolder;
        private System.Windows.Forms.Button errorFolderBrowse;
        private System.Windows.Forms.Label labelError;
        private System.Windows.Forms.Label labelSuccess;
        private System.Windows.Forms.Panel panelSuccessFolder;
        private System.Windows.Forms.Label labelSuccessFolder;
        private UI.Controls.PathTextBox successFolder;
        private System.Windows.Forms.Button successFolderBrowse;
    }
}
