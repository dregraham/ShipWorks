namespace ShipWorks.Actions
{
    partial class ActionEditorDlg
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
            this.imageUser = new System.Windows.Forms.PictureBox();
            this.labelName = new System.Windows.Forms.Label();
            this.optionControl = new ShipWorks.UI.Controls.OptionControl();
            this.optionPageAction = new ShipWorks.UI.Controls.OptionPage();
            this.addTask = new ShipWorks.UI.Controls.DropDownButton();
            this.panelTrigger = new System.Windows.Forms.Panel();
            this.triggerCombo = new System.Windows.Forms.ComboBox();
            this.panelTasks = new System.Windows.Forms.Panel();
            this.labelTasksHeader = new System.Windows.Forms.Label();
            this.labelTriggerHeader = new System.Windows.Forms.Label();
            this.optionPageSettings = new ShipWorks.UI.Controls.OptionPage();
            this.computersPanel = new System.Windows.Forms.Panel();
            this.runOnSpecificComputersList = new ShipWorks.Actions.UI.ComputersComboBox();
            this.runOnSpecificComputers = new System.Windows.Forms.RadioButton();
            this.runOnAnyComputer = new System.Windows.Forms.RadioButton();
            this.runOnOtherComputers = new System.Windows.Forms.CheckBox();
            this.panelStores = new System.Windows.Forms.Panel();
            this.checkBoxSample1 = new System.Windows.Forms.CheckBox();
            this.checkBoxSample2 = new System.Windows.Forms.CheckBox();
            this.storeLimited = new System.Windows.Forms.CheckBox();
            this.enabled = new System.Windows.Forms.CheckBox();
            this.name = new System.Windows.Forms.TextBox();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.imageUser)).BeginInit();
            this.optionControl.SuspendLayout();
            this.optionPageAction.SuspendLayout();
            this.optionPageSettings.SuspendLayout();
            this.computersPanel.SuspendLayout();
            this.panelStores.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(496, 492);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 3;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(577, 492);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 4;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // imageUser
            // 
            this.imageUser.Image = global::ShipWorks.Properties.Resources.gear_run32;
            this.imageUser.Location = new System.Drawing.Point(12, 10);
            this.imageUser.Name = "imageUser";
            this.imageUser.Size = new System.Drawing.Size(32, 32);
            this.imageUser.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imageUser.TabIndex = 8;
            this.imageUser.TabStop = false;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(51, 9);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(34, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Name";
            // 
            // optionControl
            // 
            this.optionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.optionControl.Controls.Add(this.optionPageAction);
            this.optionControl.Controls.Add(this.optionPageSettings);
            this.optionControl.Location = new System.Drawing.Point(12, 55);
            this.optionControl.MenuListWidth = 100;
            this.optionControl.Name = "optionControl";
            this.optionControl.SelectedIndex = 0;
            this.optionControl.Size = new System.Drawing.Size(640, 431);
            this.optionControl.TabIndex = 2;
            // 
            // optionPageAction
            // 
            this.optionPageAction.BackColor = System.Drawing.Color.White;
            this.optionPageAction.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.optionPageAction.Controls.Add(this.addTask);
            this.optionPageAction.Controls.Add(this.panelTrigger);
            this.optionPageAction.Controls.Add(this.triggerCombo);
            this.optionPageAction.Controls.Add(this.panelTasks);
            this.optionPageAction.Controls.Add(this.labelTasksHeader);
            this.optionPageAction.Controls.Add(this.labelTriggerHeader);
            this.optionPageAction.Location = new System.Drawing.Point(103, 0);
            this.optionPageAction.Name = "optionPageAction";
            this.optionPageAction.Padding = new System.Windows.Forms.Padding(3);
            this.optionPageAction.Size = new System.Drawing.Size(537, 431);
            this.optionPageAction.TabIndex = 1;
            this.optionPageAction.Text = "Action";
            // 
            // addTask
            // 
            this.addTask.AutoSize = true;
            this.addTask.Image = global::ShipWorks.Properties.Resources.add16;
            this.addTask.Location = new System.Drawing.Point(8, 403);
            this.addTask.Name = "addTask";
            this.addTask.Size = new System.Drawing.Size(97, 23);
            this.addTask.SplitButton = false;
            this.addTask.TabIndex = 5;
            this.addTask.Text = "Add Task";
            this.addTask.UseVisualStyleBackColor = true;
            // 
            // panelTrigger
            // 
            this.panelTrigger.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTrigger.Location = new System.Drawing.Point(35, 52);
            this.panelTrigger.Name = "panelTrigger";
            this.panelTrigger.Size = new System.Drawing.Size(496, 65);
            this.panelTrigger.TabIndex = 2;
            // 
            // triggerCombo
            // 
            this.triggerCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.triggerCombo.FormattingEnabled = true;
            this.triggerCombo.Items.AddRange(new object[] {
            "A shipment is processed"});
            this.triggerCombo.Location = new System.Drawing.Point(21, 25);
            this.triggerCombo.Name = "triggerCombo";
            this.triggerCombo.Size = new System.Drawing.Size(217, 21);
            this.triggerCombo.TabIndex = 1;
            // 
            // panelTasks
            // 
            this.panelTasks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTasks.AutoScroll = true;
            this.panelTasks.Location = new System.Drawing.Point(8, 134);
            this.panelTasks.Name = "panelTasks";
            this.panelTasks.Size = new System.Drawing.Size(523, 269);
            this.panelTasks.TabIndex = 4;
            // 
            // labelTasksHeader
            // 
            this.labelTasksHeader.AutoSize = true;
            this.labelTasksHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTasksHeader.Location = new System.Drawing.Point(4, 120);
            this.labelTasksHeader.Name = "labelTasksHeader";
            this.labelTasksHeader.Size = new System.Drawing.Size(101, 13);
            this.labelTasksHeader.TabIndex = 3;
            this.labelTasksHeader.Text = "Run these tasks:";
            // 
            // labelTriggerHeader
            // 
            this.labelTriggerHeader.AutoSize = true;
            this.labelTriggerHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTriggerHeader.Location = new System.Drawing.Point(5, 7);
            this.labelTriggerHeader.Name = "labelTriggerHeader";
            this.labelTriggerHeader.Size = new System.Drawing.Size(157, 13);
            this.labelTriggerHeader.TabIndex = 0;
            this.labelTriggerHeader.Text = "When the following occurs:";
            // 
            // optionPageSettings
            // 
            this.optionPageSettings.BackColor = System.Drawing.Color.White;
            this.optionPageSettings.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.optionPageSettings.Controls.Add(this.computersPanel);
            this.optionPageSettings.Controls.Add(this.runOnOtherComputers);
            this.optionPageSettings.Controls.Add(this.panelStores);
            this.optionPageSettings.Controls.Add(this.storeLimited);
            this.optionPageSettings.Controls.Add(this.enabled);
            this.optionPageSettings.Location = new System.Drawing.Point(103, 0);
            this.optionPageSettings.Name = "optionPageSettings";
            this.optionPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.optionPageSettings.Size = new System.Drawing.Size(537, 431);
            this.optionPageSettings.TabIndex = 4;
            this.optionPageSettings.Text = "Settings";
            // 
            // computersPanel
            // 
            this.computersPanel.Controls.Add(this.runOnSpecificComputersList);
            this.computersPanel.Controls.Add(this.runOnSpecificComputers);
            this.computersPanel.Controls.Add(this.runOnAnyComputer);
            this.computersPanel.Enabled = false;
            this.computersPanel.Location = new System.Drawing.Point(24, 48);
            this.computersPanel.Name = "computersPanel";
            this.computersPanel.Size = new System.Drawing.Size(488, 45);
            this.computersPanel.TabIndex = 4;
            // 
            // runOnSpecificComputersList
            // 
            this.runOnSpecificComputersList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.runOnSpecificComputersList.Enabled = false;
            this.runOnSpecificComputersList.FormattingEnabled = true;
            this.runOnSpecificComputersList.IntegralHeight = false;
            this.runOnSpecificComputersList.Location = new System.Drawing.Point(143, 23);
            this.runOnSpecificComputersList.Name = "runOnSpecificComputersList";
            this.runOnSpecificComputersList.Size = new System.Drawing.Size(250, 21);
            this.runOnSpecificComputersList.TabIndex = 7;
            // 
            // runOnSpecificComputers
            // 
            this.runOnSpecificComputers.AutoSize = true;
            this.runOnSpecificComputers.Location = new System.Drawing.Point(3, 24);
            this.runOnSpecificComputers.Name = "runOnSpecificComputers";
            this.runOnSpecificComputers.Size = new System.Drawing.Size(134, 17);
            this.runOnSpecificComputers.TabIndex = 6;
            this.runOnSpecificComputers.Text = "On specific computers:";
            this.runOnSpecificComputers.UseVisualStyleBackColor = true;
            this.runOnSpecificComputers.CheckedChanged += new System.EventHandler(this.OnRunOnSpecificComputersChecked);
            // 
            // runOnAnyComputer
            // 
            this.runOnAnyComputer.AutoSize = true;
            this.runOnAnyComputer.Checked = true;
            this.runOnAnyComputer.Location = new System.Drawing.Point(3, 2);
            this.runOnAnyComputer.Name = "runOnAnyComputer";
            this.runOnAnyComputer.Size = new System.Drawing.Size(108, 17);
            this.runOnAnyComputer.TabIndex = 5;
            this.runOnAnyComputer.TabStop = true;
            this.runOnAnyComputer.Text = "On any computer";
            this.runOnAnyComputer.UseVisualStyleBackColor = true;
            // 
            // runOnOtherComputers
            // 
            this.runOnOtherComputers.AutoSize = true;
            this.runOnOtherComputers.Location = new System.Drawing.Point(6, 29);
            this.runOnOtherComputers.Name = "runOnOtherComputers";
            this.runOnOtherComputers.Size = new System.Drawing.Size(391, 17);
            this.runOnOtherComputers.TabIndex = 4;
            this.runOnOtherComputers.Text = "Allow tasks to run on computers other than the one that triggers the action:";
            this.runOnOtherComputers.UseVisualStyleBackColor = true;
            this.runOnOtherComputers.CheckedChanged += new System.EventHandler(this.OnRunOnOtherComputersChecked);
            // 
            // panelStores
            // 
            this.panelStores.Controls.Add(this.checkBoxSample1);
            this.panelStores.Controls.Add(this.checkBoxSample2);
            this.panelStores.Location = new System.Drawing.Point(24, 123);
            this.panelStores.Name = "panelStores";
            this.panelStores.Size = new System.Drawing.Size(488, 45);
            this.panelStores.TabIndex = 3;
            // 
            // checkBoxSample1
            // 
            this.checkBoxSample1.AutoSize = true;
            this.checkBoxSample1.Location = new System.Drawing.Point(3, 3);
            this.checkBoxSample1.Name = "checkBoxSample1";
            this.checkBoxSample1.Size = new System.Drawing.Size(82, 17);
            this.checkBoxSample1.TabIndex = 2;
            this.checkBoxSample1.Text = "ElectroGear";
            this.checkBoxSample1.UseVisualStyleBackColor = true;
            // 
            // checkBoxSample2
            // 
            this.checkBoxSample2.AutoSize = true;
            this.checkBoxSample2.Location = new System.Drawing.Point(3, 23);
            this.checkBoxSample2.Name = "checkBoxSample2";
            this.checkBoxSample2.Size = new System.Drawing.Size(138, 17);
            this.checkBoxSample2.TabIndex = 1;
            this.checkBoxSample2.Text = "Liznobber Incorporated";
            this.checkBoxSample2.UseVisualStyleBackColor = true;
            // 
            // storeLimited
            // 
            this.storeLimited.AutoSize = true;
            this.storeLimited.Location = new System.Drawing.Point(6, 103);
            this.storeLimited.Name = "storeLimited";
            this.storeLimited.Size = new System.Drawing.Size(202, 17);
            this.storeLimited.TabIndex = 2;
            this.storeLimited.Text = "Only run the action for these stores:";
            this.storeLimited.UseVisualStyleBackColor = true;
            this.storeLimited.CheckedChanged += new System.EventHandler(this.OnChangeLimitStores);
            // 
            // enabled
            // 
            this.enabled.AutoSize = true;
            this.enabled.Location = new System.Drawing.Point(6, 6);
            this.enabled.Name = "enabled";
            this.enabled.Size = new System.Drawing.Size(64, 17);
            this.enabled.TabIndex = 0;
            this.enabled.Text = "Enabled";
            this.enabled.UseVisualStyleBackColor = true;
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(52, 24);
            this.fieldLengthProvider.SetMaxLengthSource(this.name, ShipWorks.Data.Utility.EntityFieldLengthSource.ActionName);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(273, 21);
            this.name.TabIndex = 1;
            // 
            // ActionEditorDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(664, 527);
            this.Controls.Add(this.optionControl);
            this.Controls.Add(this.name);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.imageUser);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(680, 446);
            this.Name = "ActionEditorDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Action Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.Resize += new System.EventHandler(this.OnResize);
            ((System.ComponentModel.ISupportInitialize)(this.imageUser)).EndInit();
            this.optionControl.ResumeLayout(false);
            this.optionPageAction.ResumeLayout(false);
            this.optionPageAction.PerformLayout();
            this.optionPageSettings.ResumeLayout(false);
            this.optionPageSettings.PerformLayout();
            this.computersPanel.ResumeLayout(false);
            this.computersPanel.PerformLayout();
            this.panelStores.ResumeLayout(false);
            this.panelStores.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.PictureBox imageUser;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label labelName;
        private ShipWorks.UI.Controls.OptionControl optionControl;
        private ShipWorks.UI.Controls.OptionPage optionPageAction;
        private ShipWorks.UI.Controls.OptionPage optionPageSettings;
        private System.Windows.Forms.CheckBox enabled;
        private System.Windows.Forms.Panel panelStores;
        private System.Windows.Forms.CheckBox checkBoxSample1;
        private System.Windows.Forms.CheckBox checkBoxSample2;
        private System.Windows.Forms.CheckBox storeLimited;
        private System.Windows.Forms.ComboBox triggerCombo;
        private System.Windows.Forms.CheckBox runOnOtherComputers;
        private System.Windows.Forms.Label labelTriggerHeader;
        private System.Windows.Forms.Panel panelTasks;
        private System.Windows.Forms.Label labelTasksHeader;
        private System.Windows.Forms.Panel panelTrigger;
        private ShipWorks.UI.Controls.DropDownButton addTask;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.Panel computersPanel;
        private System.Windows.Forms.RadioButton runOnAnyComputer;
        private System.Windows.Forms.RadioButton runOnSpecificComputers;
        private UI.ComputersComboBox runOnSpecificComputersList;
    }
}