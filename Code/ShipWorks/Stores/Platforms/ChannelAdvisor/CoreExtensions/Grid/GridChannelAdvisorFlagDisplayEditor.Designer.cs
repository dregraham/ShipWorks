namespace ShipWorks.Stores.Platforms.ChannelAdvisor.CoreExtensions.Grid
{
    partial class GridChannelAdvisorFlagDisplayEditor
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
            this.showFlag = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // showFlag
            // 
            this.showFlag.AutoSize = true;
            this.showFlag.Location = new System.Drawing.Point(6, 38);
            this.showFlag.Name = "showFlag";
            this.showFlag.Size = new System.Drawing.Size(117, 17);
            this.showFlag.TabIndex = 2;
            this.showFlag.Text = "Show flag indicator";
            this.showFlag.UseVisualStyleBackColor = true;
            // 
            // GridFlagDisplayEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.showFlag);
            this.Name = "GridFlagDisplayEditor";
            this.Size = new System.Drawing.Size(220, 69);
            this.Controls.SetChildIndex(this.showFlag, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox showFlag;
    }
}
