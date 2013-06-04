namespace ShipWorks.Templates.Controls.XslEditing
{
    partial class FindReplaceControl
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
            this.findAll = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.replaceText = new System.Windows.Forms.TextBox();
            this.findText = new System.Windows.Forms.TextBox();
            this.close = new System.Windows.Forms.Button();
            this.replaceAll = new System.Windows.Forms.Button();
            this.replace = new System.Windows.Forms.Button();
            this.searchUpCheckBox = new System.Windows.Forms.CheckBox();
            this.matchWholeWordCheckBox = new System.Windows.Forms.CheckBox();
            this.matchCaseCheckBox = new System.Windows.Forms.CheckBox();
            this.searchTypeCheckBox = new System.Windows.Forms.CheckBox();
            this.find = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // findAll
            // 
            this.findAll.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.findAll.Enabled = false;
            this.findAll.Location = new System.Drawing.Point(366, 1);
            this.findAll.Name = "findAll";
            this.findAll.Size = new System.Drawing.Size(75, 23);
            this.findAll.TabIndex = 5;
            this.findAll.Text = "Find All";
            this.findAll.Click += new System.EventHandler(this.OnFindAll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Replace with:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Find what:";
            // 
            // replaceText
            // 
            this.replaceText.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.replaceText.Location = new System.Drawing.Point(85, 28);
            this.replaceText.Name = "replaceText";
            this.replaceText.Size = new System.Drawing.Size(194, 21);
            this.replaceText.TabIndex = 3;
            this.replaceText.Leave += new System.EventHandler(this.OnLeaveReplace);
            this.replaceText.Enter += new System.EventHandler(this.OnEnterReplace);
            // 
            // findText
            // 
            this.findText.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.findText.Location = new System.Drawing.Point(85, 3);
            this.findText.Name = "findText";
            this.findText.Size = new System.Drawing.Size(194, 21);
            this.findText.TabIndex = 1;
            this.findText.TextChanged += new System.EventHandler(this.OnFindTextChanged);
            this.findText.Leave += new System.EventHandler(this.OnLeaveFind);
            this.findText.Enter += new System.EventHandler(this.OnEnterFind);
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.close.Location = new System.Drawing.Point(366, 68);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 12;
            this.close.Text = "Close";
            this.close.Click += new System.EventHandler(this.OnClose);
            // 
            // replaceAll
            // 
            this.replaceAll.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.replaceAll.Enabled = false;
            this.replaceAll.Location = new System.Drawing.Point(366, 27);
            this.replaceAll.Name = "replaceAll";
            this.replaceAll.Size = new System.Drawing.Size(75, 23);
            this.replaceAll.TabIndex = 7;
            this.replaceAll.Text = "Replace All";
            this.replaceAll.Click += new System.EventHandler(this.OnReplaceAll);
            // 
            // replace
            // 
            this.replace.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.replace.Enabled = false;
            this.replace.Location = new System.Drawing.Point(285, 27);
            this.replace.Name = "replace";
            this.replace.Size = new System.Drawing.Size(75, 23);
            this.replace.TabIndex = 6;
            this.replace.Text = "Replace";
            this.replace.Click += new System.EventHandler(this.OnReplace);
            // 
            // searchUpCheckBox
            // 
            this.searchUpCheckBox.AutoSize = true;
            this.searchUpCheckBox.Location = new System.Drawing.Point(127, 56);
            this.searchUpCheckBox.Name = "searchUpCheckBox";
            this.searchUpCheckBox.Size = new System.Drawing.Size(74, 17);
            this.searchUpCheckBox.TabIndex = 9;
            this.searchUpCheckBox.Text = "Search up";
            // 
            // matchWholeWordCheckBox
            // 
            this.matchWholeWordCheckBox.AutoSize = true;
            this.matchWholeWordCheckBox.Location = new System.Drawing.Point(10, 76);
            this.matchWholeWordCheckBox.Name = "matchWholeWordCheckBox";
            this.matchWholeWordCheckBox.Size = new System.Drawing.Size(113, 17);
            this.matchWholeWordCheckBox.TabIndex = 10;
            this.matchWholeWordCheckBox.Text = "Match whole word";
            // 
            // matchCaseCheckBox
            // 
            this.matchCaseCheckBox.AutoSize = true;
            this.matchCaseCheckBox.Location = new System.Drawing.Point(10, 56);
            this.matchCaseCheckBox.Name = "matchCaseCheckBox";
            this.matchCaseCheckBox.Size = new System.Drawing.Size(80, 17);
            this.matchCaseCheckBox.TabIndex = 8;
            this.matchCaseCheckBox.Text = "Match case";
            // 
            // searchTypeCheckBox
            // 
            this.searchTypeCheckBox.AutoSize = true;
            this.searchTypeCheckBox.Location = new System.Drawing.Point(127, 76);
            this.searchTypeCheckBox.Name = "searchTypeCheckBox";
            this.searchTypeCheckBox.Size = new System.Drawing.Size(141, 17);
            this.searchTypeCheckBox.TabIndex = 11;
            this.searchTypeCheckBox.Text = "Use regular expressions";
            // 
            // find
            // 
            this.find.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.find.Enabled = false;
            this.find.Location = new System.Drawing.Point(285, 1);
            this.find.Name = "find";
            this.find.Size = new System.Drawing.Size(75, 23);
            this.find.TabIndex = 4;
            this.find.Text = "Find Next";
            this.find.Click += new System.EventHandler(this.OnFind);
            // 
            // FindReplaceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.findAll);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.replaceText);
            this.Controls.Add(this.findText);
            this.Controls.Add(this.close);
            this.Controls.Add(this.replaceAll);
            this.Controls.Add(this.replace);
            this.Controls.Add(this.searchUpCheckBox);
            this.Controls.Add(this.matchWholeWordCheckBox);
            this.Controls.Add(this.matchCaseCheckBox);
            this.Controls.Add(this.searchTypeCheckBox);
            this.Controls.Add(this.find);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "FindReplaceControl";
            this.Size = new System.Drawing.Size(444, 99);
            this.Enter += new System.EventHandler(this.OnFocusEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button findAll;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox replaceText;
        private System.Windows.Forms.TextBox findText;
        private System.Windows.Forms.Button close;
        private System.Windows.Forms.Button replaceAll;
        private System.Windows.Forms.Button replace;
        private System.Windows.Forms.CheckBox searchUpCheckBox;
        private System.Windows.Forms.CheckBox matchWholeWordCheckBox;
        private System.Windows.Forms.CheckBox matchCaseCheckBox;
        private System.Windows.Forms.CheckBox searchTypeCheckBox;
        private System.Windows.Forms.Button find;
    }
}
