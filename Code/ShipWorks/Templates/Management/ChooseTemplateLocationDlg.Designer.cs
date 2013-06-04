namespace ShipWorks.Templates.Management
{
    partial class ChooseTemplateLocationDlg
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
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.labelInstructions = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelNodeName = new System.Windows.Forms.Label();
            this.nodeIcon = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.templateTree = new ShipWorks.Templates.Controls.TemplateTreeControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.nodeIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(205, 383);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 0;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(286, 383);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // labelInstructions
            // 
            this.labelInstructions.AutoSize = true;
            this.labelInstructions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelInstructions.Location = new System.Drawing.Point(12, 9);
            this.labelInstructions.Name = "labelInstructions";
            this.labelInstructions.Size = new System.Drawing.Size(79, 13);
            this.labelInstructions.TabIndex = 3;
            this.labelInstructions.Text = "{0} this {1}:";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (240)))), ((int) (((byte) (240)))), ((int) (((byte) (240)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.labelNodeName);
            this.panel1.Controls.Add(this.nodeIcon);
            this.panel1.Location = new System.Drawing.Point(15, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(349, 23);
            this.panel1.TabIndex = 5;
            // 
            // labelNodeName
            // 
            this.labelNodeName.AutoSize = true;
            this.labelNodeName.Location = new System.Drawing.Point(22, 2);
            this.labelNodeName.Name = "labelNodeName";
            this.labelNodeName.Size = new System.Drawing.Size(61, 13);
            this.labelNodeName.TabIndex = 8;
            this.labelNodeName.Text = "Node name";
            // 
            // nodeIcon
            // 
            this.nodeIcon.BackColor = System.Drawing.Color.Transparent;
            this.nodeIcon.Image = global::ShipWorks.Properties.Resources.template_label16;
            this.nodeIcon.Location = new System.Drawing.Point(3, 1);
            this.nodeIcon.Name = "nodeIcon";
            this.nodeIcon.Size = new System.Drawing.Size(16, 16);
            this.nodeIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.nodeIcon.TabIndex = 7;
            this.nodeIcon.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label1.Location = new System.Drawing.Point(12, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Into this folder:";
            // 
            // templateTree
            // 
            this.templateTree.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.templateTree.FoldersOnly = true;
            this.templateTree.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.templateTree.HotTrackNode = null;
            this.templateTree.Location = new System.Drawing.Point(15, 74);
            this.templateTree.Name = "templateTree";
            this.templateTree.Size = new System.Drawing.Size(346, 303);
            this.templateTree.TabIndex = 4;
            // 
            // ChooseTemplateLocationDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(373, 418);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.templateTree);
            this.Controls.Add(this.labelInstructions);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChooseTemplateLocationDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Location";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.nodeIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label labelInstructions;
        private ShipWorks.Templates.Controls.TemplateTreeControl templateTree;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelNodeName;
        private System.Windows.Forms.PictureBox nodeIcon;
    }
}