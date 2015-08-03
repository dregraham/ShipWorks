namespace ShipWorks.Stores.Management
{
    partial class ManualOrderSettingsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManualOrderSettingsControl));
            this.labelInfo1 = new System.Windows.Forms.Label();
            this.labelInfo2 = new System.Windows.Forms.Label();
            this.labelPrefix = new System.Windows.Forms.Label();
            this.labelPostfix = new System.Windows.Forms.Label();
            this.prefix = new System.Windows.Forms.TextBox();
            this.postfix = new System.Windows.Forms.TextBox();
            this.labelExample = new System.Windows.Forms.Label();
            this.example = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelInfo1
            // 
            this.labelInfo1.Location = new System.Drawing.Point(0, 0);
            this.labelInfo1.Name = "labelInfo1";
            this.labelInfo1.Size = new System.Drawing.Size(452, 41);
            this.labelInfo1.TabIndex = 0;
            this.labelInfo1.Text = resources.GetString("labelInfo1.Text");
            // 
            // labelInfo2
            // 
            this.labelInfo2.Location = new System.Drawing.Point(0, 52);
            this.labelInfo2.Name = "labelInfo2";
            this.labelInfo2.Size = new System.Drawing.Size(448, 34);
            this.labelInfo2.TabIndex = 1;
            this.labelInfo2.Text = "To keep order numbers unique, ShipWorks can add a prefix and\\or postfix to each m" +
    "anual order number:";
            // 
            // labelPrefix
            // 
            this.labelPrefix.AutoSize = true;
            this.labelPrefix.Location = new System.Drawing.Point(33, 89);
            this.labelPrefix.Name = "labelPrefix";
            this.labelPrefix.Size = new System.Drawing.Size(39, 13);
            this.labelPrefix.TabIndex = 2;
            this.labelPrefix.Text = "Prefix:";
            // 
            // labelPostfix
            // 
            this.labelPostfix.AutoSize = true;
            this.labelPostfix.Location = new System.Drawing.Point(28, 114);
            this.labelPostfix.Name = "labelPostfix";
            this.labelPostfix.Size = new System.Drawing.Size(44, 13);
            this.labelPostfix.TabIndex = 4;
            this.labelPostfix.Text = "Postfix:";
            // 
            // prefix
            // 
            this.prefix.Location = new System.Drawing.Point(78, 86);
            this.prefix.MaxLength = 15;
            this.prefix.Name = "prefix";
            this.prefix.Size = new System.Drawing.Size(95, 21);
            this.prefix.TabIndex = 5;
            this.prefix.TextChanged += new System.EventHandler(this.OnChangePrefixPostfix);
            // 
            // postfix
            // 
            this.postfix.Location = new System.Drawing.Point(78, 111);
            this.postfix.MaxLength = 15;
            this.postfix.Name = "postfix";
            this.postfix.Size = new System.Drawing.Size(95, 21);
            this.postfix.TabIndex = 6;
            this.postfix.TextChanged += new System.EventHandler(this.OnChangePrefixPostfix);
            // 
            // labelExample
            // 
            this.labelExample.AutoSize = true;
            this.labelExample.Location = new System.Drawing.Point(21, 139);
            this.labelExample.Name = "labelExample";
            this.labelExample.Size = new System.Drawing.Size(51, 13);
            this.labelExample.TabIndex = 7;
            this.labelExample.Text = "Example:";
            // 
            // example
            // 
            this.example.Location = new System.Drawing.Point(78, 136);
            this.example.Name = "example";
            this.example.ReadOnly = true;
            this.example.Size = new System.Drawing.Size(95, 21);
            this.example.TabIndex = 8;
            // 
            // ManualOrderSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.example);
            this.Controls.Add(this.labelExample);
            this.Controls.Add(this.postfix);
            this.Controls.Add(this.prefix);
            this.Controls.Add(this.labelPostfix);
            this.Controls.Add(this.labelPrefix);
            this.Controls.Add(this.labelInfo2);
            this.Controls.Add(this.labelInfo1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ManualOrderSettingsControl";
            this.Size = new System.Drawing.Size(564, 172);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Label labelInfo1;
        protected System.Windows.Forms.Label labelInfo2;
        protected System.Windows.Forms.Label labelPrefix;
        protected System.Windows.Forms.Label labelPostfix;
        protected System.Windows.Forms.TextBox prefix;
        protected System.Windows.Forms.TextBox postfix;
        protected System.Windows.Forms.Label labelExample;
        protected System.Windows.Forms.TextBox example;


    }
}
