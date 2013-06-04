namespace ShipWorks.Email.Outlook
{
    partial class EmailOutboxControl
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
            this.send = new ShipWorks.UI.Controls.DropDownButton();
            this.contextMenuSend = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemSendSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSendAll = new System.Windows.Forms.ToolStripMenuItem();
            this.edit = new System.Windows.Forms.Button();
            this.panelMessageControls.SuspendLayout();
            this.contextMenuSend.SuspendLayout();
            this.SuspendLayout();
            // 
            // delete
            // 
            this.delete.Location = new System.Drawing.Point(5, 75);
            this.delete.TabIndex = 2;
            // 
            // entityGrid
            // 
            this.entityGrid.TabIndex = 0;
            this.entityGrid.RowActivated += new Divelements.SandGrid.GridRowEventHandler(this.OnRowActivated);
            // 
            // panelMessageControls
            // 
            this.panelMessageControls.Controls.Add(this.send);
            this.panelMessageControls.Controls.Add(this.edit);
            this.panelMessageControls.Size = new System.Drawing.Size(124, 111);
            this.panelMessageControls.Controls.SetChildIndex(this.delete, 0);
            this.panelMessageControls.Controls.SetChildIndex(this.edit, 0);
            this.panelMessageControls.Controls.SetChildIndex(this.send, 0);
            // 
            // send
            // 
            this.send.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.send.AutoSize = true;
            this.send.ContextMenuStrip = this.contextMenuSend;
            this.send.Image = global::ShipWorks.Properties.Resources.mail2;
            this.send.Location = new System.Drawing.Point(5, 46);
            this.send.Name = "send";
            this.send.Size = new System.Drawing.Size(114, 23);
            this.send.SplitContextMenu = this.contextMenuSend;
            this.send.TabIndex = 1;
            this.send.Text = "Send";
            this.send.UseVisualStyleBackColor = true;
            this.send.Click += new System.EventHandler(this.OnSendSelected);
            // 
            // contextMenuSend
            // 
            this.contextMenuSend.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.contextMenuSend.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSendSelected,
            this.menuItemSendAll});
            this.contextMenuSend.Name = "contextMenuSend";
            this.contextMenuSend.Size = new System.Drawing.Size(147, 48);
            // 
            // menuItemSendSelected
            // 
            this.menuItemSendSelected.Name = "menuItemSendSelected";
            this.menuItemSendSelected.Size = new System.Drawing.Size(146, 22);
            this.menuItemSendSelected.Text = "Send Selected";
            this.menuItemSendSelected.Click += new System.EventHandler(this.OnSendSelected);
            // 
            // menuItemSendAll
            // 
            this.menuItemSendAll.Name = "menuItemSendAll";
            this.menuItemSendAll.Size = new System.Drawing.Size(146, 22);
            this.menuItemSendAll.Text = "Send All";
            this.menuItemSendAll.Click += new System.EventHandler(this.OnSendAll);
            // 
            // edit
            // 
            this.edit.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.edit.Image = global::ShipWorks.Properties.Resources.edit16;
            this.edit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.edit.Location = new System.Drawing.Point(5, 17);
            this.edit.Name = "edit";
            this.edit.Size = new System.Drawing.Size(115, 23);
            this.edit.TabIndex = 0;
            this.edit.Text = "Edit";
            this.edit.UseVisualStyleBackColor = true;
            this.edit.Click += new System.EventHandler(this.OnEdit);
            // 
            // EmailOutboxControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "EmailOutboxControl";
            this.panelMessageControls.ResumeLayout(false);
            this.panelMessageControls.PerformLayout();
            this.contextMenuSend.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Controls.DropDownButton send;
        private System.Windows.Forms.Button edit;
        private System.Windows.Forms.ContextMenuStrip contextMenuSend;
        private System.Windows.Forms.ToolStripMenuItem menuItemSendSelected;
        private System.Windows.Forms.ToolStripMenuItem menuItemSendAll;

    }
}
