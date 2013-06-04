namespace ShipWorks.Stores.Platforms.ProStores.CoreExtensions.Filters
{
    partial class ProStoresAuthorizationConditionEditor
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
            this.authorizedOperator = new ShipWorks.Filters.Content.Editors.ChoiceLabel();
            this.SuspendLayout();
            // 
            // panelDateControls
            // 
            this.panelDateControls.Location = new System.Drawing.Point(200, 0);
            // 
            // authorizedOperator
            // 
            this.authorizedOperator.AutoSize = true;
            this.authorizedOperator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.authorizedOperator.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.authorizedOperator.ForeColor = System.Drawing.Color.Green;
            this.authorizedOperator.Location = new System.Drawing.Point(3, 6);
            this.authorizedOperator.Name = "authorizedOperator";
            this.authorizedOperator.Size = new System.Drawing.Size(59, 13);
            this.authorizedOperator.TabIndex = 245;
            this.authorizedOperator.Text = "Authorized";
            // 
            // ProStoresAuthorizationConditionEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.authorizedOperator);
            this.Name = "ProStoresAuthorizationConditionEditor";
            this.Size = new System.Drawing.Size(857, 27);
            this.Controls.SetChildIndex(this.panelDateControls, 0);
            this.Controls.SetChildIndex(this.authorizedOperator, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.Filters.Content.Editors.ChoiceLabel authorizedOperator;
    }
}
