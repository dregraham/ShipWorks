namespace ShipWorks.UI.Controls
{
    partial class WeightControl
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
            if (disposing)
            {
                scaleSubscription?.Dispose();
                components?.Dispose();
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
            this.weighToolbar = new System.Windows.Forms.ToolStrip();
            this.weighButton = new System.Windows.Forms.ToolStripButton();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.textBox = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuCut = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSep = new System.Windows.Forms.ToolStripSeparator();
            this.menuFractionalPounds = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPoundsOunces = new System.Windows.Forms.ToolStripMenuItem();
            this.liveWeight = new System.Windows.Forms.Label();
            this.weighToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            //
            // weighToolbar
            //
            this.weighToolbar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.weighToolbar.BackColor = System.Drawing.Color.Transparent;
            this.weighToolbar.CanOverflow = false;
            this.weighToolbar.Dock = System.Windows.Forms.DockStyle.None;
            this.weighToolbar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.weighToolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.weighToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.weighButton});
            this.weighToolbar.Location = new System.Drawing.Point(289, -2);
            this.weighToolbar.Name = "weighToolbar";
            this.weighToolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.weighToolbar.Size = new System.Drawing.Size(26, 25);
            this.weighToolbar.TabIndex = 1;
            //
            // weighButton
            //
            this.weighButton.AutoToolTip = false;
            this.weighButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.weighButton.Image = global::ShipWorks.Properties.Resources.weigh;
            this.weighButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.weighButton.Name = "weighButton";
            this.weighButton.Size = new System.Drawing.Size(23, 22);
            this.weighButton.Text = "Weigh";
            this.weighButton.Click += new System.EventHandler(this.OnWeigh);
            //
            // errorProvider
            //
            this.errorProvider.ContainerControl = this;
            //
            // textBox
            //
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox.ContextMenuStrip = this.contextMenu;
            this.textBox.Location = new System.Drawing.Point(0, 0);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(287, 21);
            this.textBox.TabIndex = 0;
            this.textBox.TextChanged += new System.EventHandler(this.OnTextBoxChanged);
            //
            // contextMenu
            //
            this.contextMenu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCut,
            this.menuCopy,
            this.menuPaste,
            this.menuSep,
            this.menuFractionalPounds,
            this.menuPoundsOunces});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(171, 120);
            //
            // menuCut
            //
            this.menuCut.Image = global::ShipWorks.Properties.Resources.cut;
            this.menuCut.Name = "menuCut";
            this.menuCut.Size = new System.Drawing.Size(170, 22);
            this.menuCut.Text = "Cut";
            this.menuCut.Click += new System.EventHandler(this.OnCut);
            //
            // menuCopy
            //
            this.menuCopy.Image = global::ShipWorks.Properties.Resources.copy;
            this.menuCopy.Name = "menuCopy";
            this.menuCopy.Size = new System.Drawing.Size(170, 22);
            this.menuCopy.Text = "Copy";
            this.menuCopy.Click += new System.EventHandler(this.OnCopy);
            //
            // menuPaste
            //
            this.menuPaste.Image = global::ShipWorks.Properties.Resources.paste;
            this.menuPaste.Name = "menuPaste";
            this.menuPaste.Size = new System.Drawing.Size(170, 22);
            this.menuPaste.Text = "Paste";
            this.menuPaste.Click += new System.EventHandler(this.OnPaste);
            //
            // menuSep
            //
            this.menuSep.Name = "menuSep";
            this.menuSep.Size = new System.Drawing.Size(167, 6);
            //
            // menuFractionalPounds
            //
            this.menuFractionalPounds.Name = "menuFractionalPounds";
            this.menuFractionalPounds.Size = new System.Drawing.Size(170, 22);
            this.menuFractionalPounds.Text = "Fractional Pounds";
            this.menuFractionalPounds.Click += new System.EventHandler(this.OnFractionalPounds);
            //
            // menuPoundsOunces
            //
            this.menuPoundsOunces.Name = "menuPoundsOunces";
            this.menuPoundsOunces.Size = new System.Drawing.Size(170, 22);
            this.menuPoundsOunces.Text = "Pounds && Ounces";
            this.menuPoundsOunces.Click += new System.EventHandler(this.OnPoundsOunces);
            //
            // liveWeight
            //
            this.liveWeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.liveWeight.AutoSize = true;
            this.liveWeight.ForeColor = System.Drawing.SystemColors.GrayText;
            this.liveWeight.Location = new System.Drawing.Point(315, 3);
            this.liveWeight.Name = "liveWeight";
            this.liveWeight.Size = new System.Drawing.Size(61, 13);
            this.liveWeight.TabIndex = 2;
            this.liveWeight.Text = "(12 lb 2 oz)";
            //
            // WeightControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.liveWeight);
            this.Controls.Add(this.weighToolbar);
            this.Controls.Add(this.textBox);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "WeightControl";
            this.Size = new System.Drawing.Size(410, 20);
            this.Load += new System.EventHandler(this.OnLoad);
            this.weighToolbar.ResumeLayout(false);
            this.weighToolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MultiValueTextBox textBox;
        private System.Windows.Forms.ToolStrip weighToolbar;
        private System.Windows.Forms.ToolStripButton weighButton;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuCut;
        private System.Windows.Forms.ToolStripMenuItem menuCopy;
        private System.Windows.Forms.ToolStripMenuItem menuPaste;
        private System.Windows.Forms.ToolStripSeparator menuSep;
        private System.Windows.Forms.ToolStripMenuItem menuFractionalPounds;
        private System.Windows.Forms.ToolStripMenuItem menuPoundsOunces;
        private System.Windows.Forms.Label liveWeight;

    }
}
