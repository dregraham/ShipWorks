namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    partial class OdbcImportFieldMappingPage
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.newMapButton = new System.Windows.Forms.Button();
            this.loadMapButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Import Map";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(25, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(347, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Setup the database table and mappings of you columns into ShipWorks";
            // 
            // newMapButton
            // 
            this.newMapButton.Location = new System.Drawing.Point(40, 50);
            this.newMapButton.Name = "newMapButton";
            this.newMapButton.Size = new System.Drawing.Size(75, 25);
            this.newMapButton.TabIndex = 2;
            this.newMapButton.Text = "New Map...";
            this.newMapButton.UseVisualStyleBackColor = true;
            this.newMapButton.Click += new System.EventHandler(this.OnClickNewMapButton);
            // 
            // loadMapButton
            // 
            this.loadMapButton.Location = new System.Drawing.Point(121, 50);
            this.loadMapButton.Name = "loadMapButton";
            this.loadMapButton.Size = new System.Drawing.Size(75, 25);
            this.loadMapButton.TabIndex = 3;
            this.loadMapButton.Text = "Load Map...";
            this.loadMapButton.UseVisualStyleBackColor = true;
            // 
            // OdbcImportFieldMappingPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.loadMapButton);
            this.Controls.Add(this.newMapButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "OdbcImportFieldMappingPage";
            this.Size = new System.Drawing.Size(455, 342);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button newMapButton;
        private System.Windows.Forms.Button loadMapButton;
    }
}
