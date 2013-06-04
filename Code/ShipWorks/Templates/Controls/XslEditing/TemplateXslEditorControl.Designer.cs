namespace ShipWorks.Templates.Controls.XslEditing
{
    partial class TemplateXslEditorControl
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
            ActiproSoftware.SyntaxEditor.Document document1 = new ActiproSoftware.SyntaxEditor.Document();
            ActiproSoftware.SyntaxEditor.VisualStudio2005SyntaxEditorRenderer visualStudio2005SyntaxEditorRenderer1 = new ActiproSoftware.SyntaxEditor.VisualStudio2005SyntaxEditorRenderer();
            this.syntaxEditor = new ActiproSoftware.SyntaxEditor.SyntaxEditor();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemCut = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.buttonCut = new System.Windows.Forms.ToolStripButton();
            this.buttonCopy = new System.Windows.Forms.ToolStripButton();
            this.buttonPaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonUndo = new System.Windows.Forms.ToolStripButton();
            this.buttonRedo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonSearch = new System.Windows.Forms.ToolStripButton();
            this.findReplaceControl = new ShipWorks.Templates.Controls.XslEditing.FindReplaceControl();
            this.contextMenu.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // syntaxEditor
            // 
            this.syntaxEditor.AllowDrop = true;
            this.syntaxEditor.ContextMenuStrip = this.contextMenu;
            this.syntaxEditor.CutCopyBlankLineWhenNoSelection = false;
            this.syntaxEditor.DefaultContextMenuEnabled = false;
            this.syntaxEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEditor.Document = document1;
            this.syntaxEditor.IndentationGuidesVisible = false;
            this.syntaxEditor.IndicatorMarginVisible = false;
            this.syntaxEditor.LineNumberMarginVisible = true;
            this.syntaxEditor.Location = new System.Drawing.Point(0, 124);
            this.syntaxEditor.Name = "syntaxEditor";
            visualStudio2005SyntaxEditorRenderer1.ResetAllPropertiesOnSystemColorChange = false;
            this.syntaxEditor.Renderer = visualStudio2005SyntaxEditorRenderer1;
            this.syntaxEditor.Size = new System.Drawing.Size(486, 357);
            this.syntaxEditor.SplitType = ActiproSoftware.SyntaxEditor.SyntaxEditorSplitType.None;
            this.syntaxEditor.TabIndex = 0;
            this.syntaxEditor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnEditorKeyDown);
            this.syntaxEditor.DocumentUndoRedoStateChanged += new ActiproSoftware.SyntaxEditor.UndoRedoStateChangedEventHandler(this.OnUndoRedoStateChanged);
            // 
            // contextMenu
            // 
            this.contextMenu.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemCut,
            this.menuItemCopy,
            this.menuItemPaste,
            this.toolStripSeparator3,
            this.menuItemUndo,
            this.menuItemRedo,
            this.toolStripSeparator4,
            this.menuItemSelectAll});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(121, 148);
            this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.OnOpeningContextMenu);
            // 
            // menuItemCut
            // 
            this.menuItemCut.Image = global::ShipWorks.Properties.Resources.cut;
            this.menuItemCut.Name = "menuItemCut";
            this.menuItemCut.Size = new System.Drawing.Size(120, 22);
            this.menuItemCut.Text = "Cut";
            this.menuItemCut.Click += new System.EventHandler(this.OnCut);
            // 
            // menuItemCopy
            // 
            this.menuItemCopy.Image = global::ShipWorks.Properties.Resources.copy;
            this.menuItemCopy.Name = "menuItemCopy";
            this.menuItemCopy.Size = new System.Drawing.Size(120, 22);
            this.menuItemCopy.Text = "Copy";
            this.menuItemCopy.Click += new System.EventHandler(this.OnCopy);
            // 
            // menuItemPaste
            // 
            this.menuItemPaste.Image = global::ShipWorks.Properties.Resources.paste;
            this.menuItemPaste.Name = "menuItemPaste";
            this.menuItemPaste.Size = new System.Drawing.Size(120, 22);
            this.menuItemPaste.Text = "Paste";
            this.menuItemPaste.Click += new System.EventHandler(this.OnPaste);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(117, 6);
            // 
            // menuItemUndo
            // 
            this.menuItemUndo.Image = global::ShipWorks.Properties.Resources.undo;
            this.menuItemUndo.Name = "menuItemUndo";
            this.menuItemUndo.Size = new System.Drawing.Size(120, 22);
            this.menuItemUndo.Text = "Undo";
            this.menuItemUndo.Click += new System.EventHandler(this.OnUndo);
            // 
            // menuItemRedo
            // 
            this.menuItemRedo.Image = global::ShipWorks.Properties.Resources.redo;
            this.menuItemRedo.Name = "menuItemRedo";
            this.menuItemRedo.Size = new System.Drawing.Size(120, 22);
            this.menuItemRedo.Text = "Redo";
            this.menuItemRedo.Click += new System.EventHandler(this.OnRedo);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(117, 6);
            // 
            // menuItemSelectAll
            // 
            this.menuItemSelectAll.Name = "menuItemSelectAll";
            this.menuItemSelectAll.Size = new System.Drawing.Size(120, 22);
            this.menuItemSelectAll.Text = "Select All";
            this.menuItemSelectAll.Click += new System.EventHandler(this.OnSelectAll);
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonCut,
            this.buttonCopy,
            this.buttonPaste,
            this.toolStripSeparator1,
            this.buttonUndo,
            this.buttonRedo,
            this.toolStripSeparator2,
            this.buttonSearch});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(486, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip";
            // 
            // buttonCut
            // 
            this.buttonCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonCut.Image = global::ShipWorks.Properties.Resources.cut;
            this.buttonCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonCut.Name = "buttonCut";
            this.buttonCut.Size = new System.Drawing.Size(23, 22);
            this.buttonCut.Text = "Cut";
            this.buttonCut.Click += new System.EventHandler(this.OnCut);
            // 
            // buttonCopy
            // 
            this.buttonCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonCopy.Image = global::ShipWorks.Properties.Resources.copy;
            this.buttonCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(23, 22);
            this.buttonCopy.Text = "Copy";
            this.buttonCopy.Click += new System.EventHandler(this.OnCopy);
            // 
            // buttonPaste
            // 
            this.buttonPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonPaste.Image = global::ShipWorks.Properties.Resources.paste;
            this.buttonPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonPaste.Name = "buttonPaste";
            this.buttonPaste.Size = new System.Drawing.Size(23, 22);
            this.buttonPaste.Text = "Paste";
            this.buttonPaste.Click += new System.EventHandler(this.OnPaste);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // buttonUndo
            // 
            this.buttonUndo.Image = global::ShipWorks.Properties.Resources.undo;
            this.buttonUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonUndo.Name = "buttonUndo";
            this.buttonUndo.Size = new System.Drawing.Size(52, 22);
            this.buttonUndo.Text = "Undo";
            this.buttonUndo.ToolTipText = "Undo (Ctrl + Z)";
            this.buttonUndo.Click += new System.EventHandler(this.OnUndo);
            // 
            // buttonRedo
            // 
            this.buttonRedo.Image = global::ShipWorks.Properties.Resources.redo;
            this.buttonRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonRedo.Name = "buttonRedo";
            this.buttonRedo.Size = new System.Drawing.Size(55, 22);
            this.buttonRedo.Text = "Redo ";
            this.buttonRedo.ToolTipText = "Redo (Ctrl + Y)";
            this.buttonRedo.Click += new System.EventHandler(this.OnRedo);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Image = global::ShipWorks.Properties.Resources.find;
            this.buttonSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(109, 22);
            this.buttonSearch.Text = "Find and Replace";
            this.buttonSearch.ToolTipText = "Search (Ctrl + F)";
            this.buttonSearch.Click += new System.EventHandler(this.OnSearch);
            // 
            // findReplaceControl
            // 
            this.findReplaceControl.CloseButtonText = "Hide";
            this.findReplaceControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.findReplaceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.findReplaceControl.Location = new System.Drawing.Point(0, 25);
            this.findReplaceControl.Name = "findReplaceControl";
            this.findReplaceControl.Size = new System.Drawing.Size(486, 99);
            this.findReplaceControl.TabIndex = 2;
            this.findReplaceControl.Visible = false;
            this.findReplaceControl.CloseClicked += new System.EventHandler(this.OnCloseFindReplace);
            // 
            // TemplateXslEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.syntaxEditor);
            this.Controls.Add(this.findReplaceControl);
            this.Controls.Add(this.toolStrip);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "TemplateXslEditorControl";
            this.Size = new System.Drawing.Size(486, 481);
            this.Load += new System.EventHandler(this.OnLoad);
            this.contextMenu.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ActiproSoftware.SyntaxEditor.SyntaxEditor syntaxEditor;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton buttonCut;
        private System.Windows.Forms.ToolStripButton buttonCopy;
        private System.Windows.Forms.ToolStripButton buttonPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton buttonRedo;
        private System.Windows.Forms.ToolStripButton buttonUndo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton buttonSearch;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuItemCut;
        private System.Windows.Forms.ToolStripMenuItem menuItemCopy;
        private System.Windows.Forms.ToolStripMenuItem menuItemPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuItemUndo;
        private System.Windows.Forms.ToolStripMenuItem menuItemRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuItemSelectAll;
        private FindReplaceControl findReplaceControl;

    }
}
