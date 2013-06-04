namespace ShipWorks.Actions.Tasks
{
    partial class ActionTaskBubble
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
            this.taskIndex = new System.Windows.Forms.Label();
            this.kryptonGroup = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.flowInfoControl = new ShipWorks.Actions.Tasks.ActionTaskFlowInfoControl();
            this.panelInputSource = new System.Windows.Forms.Panel();
            this.labelInput = new System.Windows.Forms.Label();
            this.inputSourceFilter = new ShipWorks.Filters.Controls.FilterComboBox();
            this.inputSourceLink = new System.Windows.Forms.Label();
            this.panelTaskSettings = new System.Windows.Forms.Panel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.buttonFlow = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.moveUp = new System.Windows.Forms.ToolStripButton();
            this.moveDown = new System.Windows.Forms.ToolStripButton();
            this.delete = new System.Windows.Forms.ToolStripButton();
            this.ordersToolbar = new System.Windows.Forms.ToolStrip();
            this.chooseMoreButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.removeShipmentButton = new System.Windows.Forms.ToolStripButton();
            this.taskTypes = new ShipWorks.UI.Controls.MenuComboBox();
            ((System.ComponentModel.ISupportInitialize) (this.kryptonGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.kryptonGroup.Panel)).BeginInit();
            this.kryptonGroup.Panel.SuspendLayout();
            this.kryptonGroup.SuspendLayout();
            this.panelInputSource.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.ordersToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // taskIndex
            // 
            this.taskIndex.AutoSize = true;
            this.taskIndex.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.taskIndex.Location = new System.Drawing.Point(3, 12);
            this.taskIndex.Name = "taskIndex";
            this.taskIndex.Size = new System.Drawing.Size(17, 13);
            this.taskIndex.TabIndex = 35;
            this.taskIndex.Text = "1.";
            // 
            // kryptonGroup
            // 
            this.kryptonGroup.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonGroup.GroupBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridHeaderColumnList;
            this.kryptonGroup.GroupBorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellList;
            this.kryptonGroup.Location = new System.Drawing.Point(25, 1);
            this.kryptonGroup.Name = "kryptonGroup";
            this.kryptonGroup.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            // 
            // kryptonGroup.Panel
            // 
            this.kryptonGroup.Panel.Controls.Add(this.flowInfoControl);
            this.kryptonGroup.Panel.Controls.Add(this.panelInputSource);
            this.kryptonGroup.Panel.Controls.Add(this.panelTaskSettings);
            this.kryptonGroup.Panel.Controls.Add(this.toolStrip);
            this.kryptonGroup.Panel.Controls.Add(this.ordersToolbar);
            this.kryptonGroup.Panel.Controls.Add(this.taskTypes);
            this.kryptonGroup.Size = new System.Drawing.Size(461, 187);
            this.kryptonGroup.StateNormal.Back.Color1 = System.Drawing.Color.White;
            this.kryptonGroup.StateNormal.Back.Color2 = System.Drawing.Color.WhiteSmoke;
            this.kryptonGroup.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders) ((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonGroup.StateNormal.Border.Rounding = 3;
            this.kryptonGroup.StateNormal.Border.Width = 1;
            this.kryptonGroup.TabIndex = 0;
            this.kryptonGroup.Resize += new System.EventHandler(this.OnResizeBubble);
            // 
            // flowInfoControl
            // 
            this.flowInfoControl.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flowInfoControl.BackColor = System.Drawing.Color.Transparent;
            this.flowInfoControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.flowInfoControl.Location = new System.Drawing.Point(26, 124);
            this.flowInfoControl.Name = "flowInfoControl";
            this.flowInfoControl.Size = new System.Drawing.Size(417, 51);
            this.flowInfoControl.TabIndex = 4;
            this.flowInfoControl.FlowClicked += new System.EventHandler(this.OnEditFlow);
            // 
            // panelInputSource
            // 
            this.panelInputSource.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelInputSource.BackColor = System.Drawing.Color.Transparent;
            this.panelInputSource.Controls.Add(this.labelInput);
            this.panelInputSource.Controls.Add(this.inputSourceFilter);
            this.panelInputSource.Controls.Add(this.inputSourceLink);
            this.panelInputSource.Location = new System.Drawing.Point(26, 34);
            this.panelInputSource.Name = "panelInputSource";
            this.panelInputSource.Size = new System.Drawing.Size(417, 29);
            this.panelInputSource.TabIndex = 2;
            // 
            // labelInput
            // 
            this.labelInput.AutoSize = true;
            this.labelInput.BackColor = System.Drawing.Color.Transparent;
            this.labelInput.Location = new System.Drawing.Point(3, 3);
            this.labelInput.Name = "labelInput";
            this.labelInput.Size = new System.Drawing.Size(78, 13);
            this.labelInput.TabIndex = 10;
            this.labelInput.Text = "Using as input:";
            // 
            // inputSourceFilter
            // 
            this.inputSourceFilter.AllowQuickFilter = true;
            this.inputSourceFilter.DropDownHeight = 300;
            this.inputSourceFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inputSourceFilter.FormattingEnabled = true;
            this.inputSourceFilter.IntegralHeight = false;
            this.inputSourceFilter.Location = new System.Drawing.Point(210, 0);
            this.inputSourceFilter.Name = "inputSourceFilter";
            this.inputSourceFilter.Size = new System.Drawing.Size(204, 21);
            this.inputSourceFilter.TabIndex = 2;
            this.inputSourceFilter.Visible = false;
            // 
            // inputSourceLink
            // 
            this.inputSourceLink.AutoSize = true;
            this.inputSourceLink.BackColor = System.Drawing.Color.Transparent;
            this.inputSourceLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.inputSourceLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.inputSourceLink.ForeColor = System.Drawing.Color.Blue;
            this.inputSourceLink.Location = new System.Drawing.Point(81, 3);
            this.inputSourceLink.Name = "inputSourceLink";
            this.inputSourceLink.Size = new System.Drawing.Size(123, 13);
            this.inputSourceLink.TabIndex = 1;
            this.inputSourceLink.Text = "The processed shipment";
            this.inputSourceLink.Click += new System.EventHandler(this.OnClickInputSourceLink);
            // 
            // panelTaskSettings
            // 
            this.panelTaskSettings.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTaskSettings.BackColor = System.Drawing.Color.Transparent;
            this.panelTaskSettings.Location = new System.Drawing.Point(26, 62);
            this.panelTaskSettings.Name = "panelTaskSettings";
            this.panelTaskSettings.Size = new System.Drawing.Size(417, 63);
            this.panelTaskSettings.TabIndex = 3;
            // 
            // toolStrip
            // 
            this.toolStrip.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonFlow,
            this.toolStripSeparator1,
            this.moveUp,
            this.moveDown,
            this.delete});
            this.toolStrip.Location = new System.Drawing.Point(318, 4);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(139, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // buttonFlow
            // 
            this.buttonFlow.Image = global::ShipWorks.Properties.Resources.branch;
            this.buttonFlow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonFlow.Name = "buttonFlow";
            this.buttonFlow.Size = new System.Drawing.Size(61, 22);
            this.buttonFlow.Text = "Flow...";
            this.buttonFlow.Click += new System.EventHandler(this.OnEditFlow);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // moveUp
            // 
            this.moveUp.AutoToolTip = false;
            this.moveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.moveUp.Image = global::ShipWorks.Properties.Resources.arrow_up_blue;
            this.moveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.moveUp.Name = "moveUp";
            this.moveUp.Size = new System.Drawing.Size(23, 22);
            this.moveUp.Click += new System.EventHandler(this.OnMoveUp);
            // 
            // moveDown
            // 
            this.moveDown.AutoToolTip = false;
            this.moveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.moveDown.Image = global::ShipWorks.Properties.Resources.arrow_down_blue;
            this.moveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.moveDown.Name = "moveDown";
            this.moveDown.Size = new System.Drawing.Size(23, 22);
            this.moveDown.Click += new System.EventHandler(this.OnMoveDown);
            // 
            // delete
            // 
            this.delete.AutoToolTip = false;
            this.delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.delete.Image = global::ShipWorks.Properties.Resources.delete16;
            this.delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(23, 22);
            this.delete.Click += new System.EventHandler(this.OnDelete);
            // 
            // ordersToolbar
            // 
            this.ordersToolbar.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ordersToolbar.BackColor = System.Drawing.Color.Transparent;
            this.ordersToolbar.Dock = System.Windows.Forms.DockStyle.None;
            this.ordersToolbar.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.ordersToolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ordersToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chooseMoreButton,
            this.toolStripButton1,
            this.removeShipmentButton});
            this.ordersToolbar.Location = new System.Drawing.Point(385, 4);
            this.ordersToolbar.Name = "ordersToolbar";
            this.ordersToolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ordersToolbar.Size = new System.Drawing.Size(72, 25);
            this.ordersToolbar.TabIndex = 7;
            this.ordersToolbar.Text = "toolStrip1";
            this.ordersToolbar.Visible = false;
            // 
            // chooseMoreButton
            // 
            this.chooseMoreButton.AutoToolTip = false;
            this.chooseMoreButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.chooseMoreButton.Enabled = false;
            this.chooseMoreButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chooseMoreButton.Name = "chooseMoreButton";
            this.chooseMoreButton.Size = new System.Drawing.Size(23, 22);
            this.chooseMoreButton.Text = "Choose More";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // removeShipmentButton
            // 
            this.removeShipmentButton.AutoToolTip = false;
            this.removeShipmentButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.removeShipmentButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeShipmentButton.Name = "removeShipmentButton";
            this.removeShipmentButton.Size = new System.Drawing.Size(23, 22);
            this.removeShipmentButton.Text = "Remove From List";
            // 
            // taskTypes
            // 
            this.taskTypes.DisplayValueProvider = null;
            this.taskTypes.FormattingEnabled = true;
            this.taskTypes.Items.AddRange(new object[] {
            "Print"});
            this.taskTypes.Location = new System.Drawing.Point(9, 7);
            this.taskTypes.Name = "taskTypes";
            this.taskTypes.SelectedMenuObject = null;
            this.taskTypes.Size = new System.Drawing.Size(291, 21);
            this.taskTypes.TabIndex = 0;
            // 
            // ActionTaskBubble
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.taskIndex);
            this.Controls.Add(this.kryptonGroup);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "ActionTaskBubble";
            this.Size = new System.Drawing.Size(497, 200);
            ((System.ComponentModel.ISupportInitialize) (this.kryptonGroup.Panel)).EndInit();
            this.kryptonGroup.Panel.ResumeLayout(false);
            this.kryptonGroup.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.kryptonGroup)).EndInit();
            this.kryptonGroup.ResumeLayout(false);
            this.panelInputSource.ResumeLayout(false);
            this.panelInputSource.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ordersToolbar.ResumeLayout(false);
            this.ordersToolbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label taskIndex;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup kryptonGroup;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton moveUp;
        private System.Windows.Forms.ToolStripButton moveDown;
        private System.Windows.Forms.ToolStripButton delete;
        private System.Windows.Forms.Label inputSourceLink;
        private System.Windows.Forms.Label labelInput;
        private System.Windows.Forms.ToolStrip ordersToolbar;
        private System.Windows.Forms.ToolStripButton chooseMoreButton;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton removeShipmentButton;
        private ShipWorks.UI.Controls.MenuComboBox taskTypes;
        private System.Windows.Forms.Panel panelTaskSettings;
        private ShipWorks.Filters.Controls.FilterComboBox inputSourceFilter;
        private System.Windows.Forms.Panel panelInputSource;
        private System.Windows.Forms.ToolStripButton buttonFlow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private ActionTaskFlowInfoControl flowInfoControl;
    }
}
