namespace ShipWorks.Filters.Management
{
    partial class AddFilterWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddFilterWizard));
            this.wizardPageNameLocation = new ShipWorks.UI.Wizard.WizardPage();
            this.labelLocation = new System.Windows.Forms.Label();
            this.filterTree = new ShipWorks.Filters.Controls.FilterTree();
            this.name = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.wizardPageCondition = new ShipWorks.UI.Wizard.WizardPage();
            this.conditionControl = new ShipWorks.Filters.Controls.FilterConditionControl();
            this.wizardPageGridColumns = new ShipWorks.UI.Wizard.WizardPage();
            this.label1 = new System.Windows.Forms.Label();
            this.gridColumns = new ShipWorks.Filters.Controls.FilterNodeColumnEditor();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageNameLocation.SuspendLayout();
            this.wizardPageCondition.SuspendLayout();
            this.wizardPageGridColumns.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            //
            // next
            //
            this.next.Location = new System.Drawing.Point(436, 455);
            //
            // cancel
            //
            this.cancel.Location = new System.Drawing.Point(517, 455);
            //
            // back
            //
            this.back.Location = new System.Drawing.Point(355, 455);
            //
            // mainPanel
            //
            this.mainPanel.Controls.Add(this.wizardPageNameLocation);
            this.mainPanel.Size = new System.Drawing.Size(604, 383);
            //
            // etchBottom
            //
            this.etchBottom.Location = new System.Drawing.Point(0, 445);
            this.etchBottom.Size = new System.Drawing.Size(608, 2);
            //
            // pictureBox
            //
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.funnel_add1;
            this.pictureBox.InitialImage = global::ShipWorks.Properties.Resources.funnel_add;
            this.pictureBox.Location = new System.Drawing.Point(544, 3);
            this.pictureBox.Size = new System.Drawing.Size(48, 48);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            //
            // topPanel
            //
            this.topPanel.Size = new System.Drawing.Size(604, 56);
            //
            // wizardPageNameLocation
            //
            this.wizardPageNameLocation.Controls.Add(this.labelLocation);
            this.wizardPageNameLocation.Controls.Add(this.filterTree);
            this.wizardPageNameLocation.Controls.Add(this.name);
            this.wizardPageNameLocation.Controls.Add(this.labelName);
            this.wizardPageNameLocation.Description = "Choose a name and location for the new filter.";
            this.wizardPageNameLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageNameLocation.Location = new System.Drawing.Point(0, 0);
            this.wizardPageNameLocation.Name = "wizardPageNameLocation";
            this.wizardPageNameLocation.Size = new System.Drawing.Size(604, 383);
            this.wizardPageNameLocation.TabIndex = 0;
            this.wizardPageNameLocation.Title = "Name and Location";
            this.wizardPageNameLocation.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextNameAndLocation);
            //
            // labelLocation
            //
            this.labelLocation.AutoSize = true;
            this.labelLocation.Location = new System.Drawing.Point(21, 58);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(200, 13);
            this.labelLocation.TabIndex = 2;
            this.labelLocation.Text = "Select the folder to put the new filter in:";
            //
            // filterTree
            //
            this.filterTree.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.filterTree.AutoRefreshCalculatingCounts = true;
            this.filterTree.FoldersOnly = true;
            this.filterTree.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.filterTree.HotTrackNode = null;
            this.filterTree.Location = new System.Drawing.Point(23, 74);
            this.filterTree.Name = "filterTree";
            this.filterTree.Size = new System.Drawing.Size(557, 296);
            this.filterTree.TabIndex = 3;
            //
            // name
            //
            this.name.Location = new System.Drawing.Point(22, 25);
            this.fieldLengthProvider.SetMaxLengthSource(this.name, ShipWorks.Data.Utility.EntityFieldLengthSource.FilterName);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(199, 21);
            this.name.TabIndex = 1;
            //
            // labelName
            //
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(19, 9);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(65, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Filter Name:";
            //
            // wizardPageCondition
            //
            this.wizardPageCondition.Controls.Add(this.conditionControl);
            this.wizardPageCondition.Description = "Configure the condition that determines the filter contents.";
            this.wizardPageCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageCondition.Location = new System.Drawing.Point(0, 0);
            this.wizardPageCondition.Name = "wizardPageCondition";
            this.wizardPageCondition.Size = new System.Drawing.Size(604, 383);
            this.wizardPageCondition.TabIndex = 0;
            this.wizardPageCondition.Title = "Filter Condition";
            this.wizardPageCondition.SteppingInto += WizardPageCondition_SteppingInto;
            this.wizardPageCondition.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextCondition);
            //
            // conditionControl
            //
            this.conditionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conditionControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.conditionControl.Location = new System.Drawing.Point(0, 0);
            this.conditionControl.Name = "conditionControl";
            this.conditionControl.Size = new System.Drawing.Size(604, 383);
            this.conditionControl.TabIndex = 0;
            //
            // wizardPageGridColumns
            //
            this.wizardPageGridColumns.Controls.Add(this.label1);
            this.wizardPageGridColumns.Controls.Add(this.gridColumns);
            this.wizardPageGridColumns.Description = "Configure the default grid columns of the filter.";
            this.wizardPageGridColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageGridColumns.Location = new System.Drawing.Point(0, 0);
            this.wizardPageGridColumns.Name = "wizardPageGridColumns";
            this.wizardPageGridColumns.Size = new System.Drawing.Size(604, 383);
            this.wizardPageGridColumns.TabIndex = 0;
            this.wizardPageGridColumns.Title = "Grid Columns";
            this.wizardPageGridColumns.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextGridColumns);
            //
            // label1
            //
            this.label1.Location = new System.Drawing.Point(26, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(454, 46);
            this.label1.TabIndex = 4;
            this.label1.Text = resources.GetString("label1.Text");
            //
            // gridColumns
            //
            this.gridColumns.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.gridColumns.EnableParentEditing = false;
            this.gridColumns.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumns.Location = new System.Drawing.Point(21, 57);
            this.gridColumns.MinimumSize = new System.Drawing.Size(344, 260);
            this.gridColumns.Name = "gridColumns";
            this.gridColumns.Size = new System.Drawing.Size(384, 323);
            this.gridColumns.TabIndex = 3;
            //
            // AddFilterWizard
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 490);
            this.LastPageCancelable = true;
            this.MinimumSize = new System.Drawing.Size(16, 456);
            this.Name = "AddFilterWizard";
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageNameLocation,
            this.wizardPageCondition,
            this.wizardPageGridColumns});
            this.Text = "Add Filter Wizard";
            this.Load += new System.EventHandler(this.OnLoad);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageNameLocation.ResumeLayout(false);
            this.wizardPageNameLocation.PerformLayout();
            this.wizardPageCondition.ResumeLayout(false);
            this.wizardPageGridColumns.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Wizard.WizardPage wizardPageGridColumns;
        private ShipWorks.UI.Wizard.WizardPage wizardPageNameLocation;
        private ShipWorks.UI.Wizard.WizardPage wizardPageCondition;
        private System.Windows.Forms.Label labelLocation;
        private ShipWorks.Filters.Controls.FilterTree filterTree;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label labelName;
        private ShipWorks.Filters.Controls.FilterNodeColumnEditor gridColumns;
        private ShipWorks.Filters.Controls.FilterConditionControl conditionControl;
        private System.Windows.Forms.Label label1;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
    }
}