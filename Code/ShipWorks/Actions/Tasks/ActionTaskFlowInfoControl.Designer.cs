namespace ShipWorks.Actions.Tasks
{
    partial class ActionTaskFlowInfoControl
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
                if (components != null)
                {
                    components.Dispose();
                }

                if (filterDisplayManager != null)
                {
                    filterDisplayManager.Dispose();
                }
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
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.panelBuffer = new System.Windows.Forms.Panel();
            this.labelFlowDefault = new System.Windows.Forms.Label();
            this.flowLink = new System.Windows.Forms.Label();
            this.labelFilter = new System.Windows.Forms.Label();
            this.filterCount = new System.Windows.Forms.Label();
            this.filterName = new System.Windows.Forms.Label();
            this.filterPicture = new System.Windows.Forms.PictureBox();
            this.panelFilter = new System.Windows.Forms.Panel();
            this.panelSuccess = new System.Windows.Forms.Panel();
            this.whenSuccess = new System.Windows.Forms.Label();
            this.labelWhenSuccess = new System.Windows.Forms.Label();
            this.panelSkipped = new System.Windows.Forms.Panel();
            this.whenSkipped = new System.Windows.Forms.Label();
            this.labelWhenSkipped = new System.Windows.Forms.Label();
            this.panelError = new System.Windows.Forms.Panel();
            this.whenError = new System.Windows.Forms.Label();
            this.labelWhenError = new System.Windows.Forms.Label();
            this.panelFilterName = new System.Windows.Forms.Panel();
            this.panelBuffer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.filterPicture)).BeginInit();
            this.panelFilter.SuspendLayout();
            this.panelSuccess.SuspendLayout();
            this.panelSkipped.SuspendLayout();
            this.panelError.SuspendLayout();
            this.panelFilterName.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.AutoSize = false;
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(0, 0);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(375, 1);
            this.kryptonBorderEdge.TabIndex = 3;
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            // 
            // panelBuffer
            // 
            this.panelBuffer.Controls.Add(this.labelFlowDefault);
            this.panelBuffer.Controls.Add(this.flowLink);
            this.panelBuffer.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBuffer.Location = new System.Drawing.Point(0, 1);
            this.panelBuffer.Name = "panelBuffer";
            this.panelBuffer.Size = new System.Drawing.Size(375, 18);
            this.panelBuffer.TabIndex = 4;
            // 
            // labelFlowDefault
            // 
            this.labelFlowDefault.AutoSize = true;
            this.labelFlowDefault.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelFlowDefault.Location = new System.Drawing.Point(50, 3);
            this.labelFlowDefault.Name = "labelFlowDefault";
            this.labelFlowDefault.Size = new System.Drawing.Size(134, 13);
            this.labelFlowDefault.TabIndex = 14;
            this.labelFlowDefault.Text = "(As different from default)";
            // 
            // flowLink
            // 
            this.flowLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.flowLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.flowLink.ForeColor = System.Drawing.Color.Blue;
            this.flowLink.Image = global::ShipWorks.Properties.Resources.branch;
            this.flowLink.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.flowLink.Location = new System.Drawing.Point(6, 3);
            this.flowLink.Name = "flowLink";
            this.flowLink.Size = new System.Drawing.Size(47, 13);
            this.flowLink.TabIndex = 13;
            this.flowLink.Text = "Flow";
            this.flowLink.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.flowLink.Click += new System.EventHandler(this.OnClickFlowLink);
            // 
            // labelFilter
            // 
            this.labelFilter.AutoSize = true;
            this.labelFilter.Location = new System.Drawing.Point(29, 3);
            this.labelFilter.Name = "labelFilter";
            this.labelFilter.Size = new System.Drawing.Size(131, 13);
            this.labelFilter.TabIndex = 12;
            this.labelFilter.Text = "Run only if the order is in:";
            // 
            // filterCount
            // 
            this.filterCount.AutoSize = true;
            this.filterCount.ForeColor = System.Drawing.Color.Blue;
            this.filterCount.Location = new System.Drawing.Point(108, 1);
            this.filterCount.Name = "filterCount";
            this.filterCount.Size = new System.Drawing.Size(33, 13);
            this.filterCount.TabIndex = 11;
            this.filterCount.Text = "(167)";
            // 
            // filterName
            // 
            this.filterName.AutoSize = true;
            this.filterName.Location = new System.Drawing.Point(17, 1);
            this.filterName.Name = "filterName";
            this.filterName.Size = new System.Drawing.Size(94, 13);
            this.filterName.TabIndex = 9;
            this.filterName.Text = "My awesome filter";
            // 
            // filterPicture
            // 
            this.filterPicture.Image = global::ShipWorks.Properties.Resources.filter;
            this.filterPicture.Location = new System.Drawing.Point(0, 0);
            this.filterPicture.Name = "filterPicture";
            this.filterPicture.Size = new System.Drawing.Size(16, 16);
            this.filterPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.filterPicture.TabIndex = 10;
            this.filterPicture.TabStop = false;
            // 
            // panelFilter
            // 
            this.panelFilter.Controls.Add(this.panelFilterName);
            this.panelFilter.Controls.Add(this.labelFilter);
            this.panelFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilter.Location = new System.Drawing.Point(0, 19);
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.Size = new System.Drawing.Size(375, 18);
            this.panelFilter.TabIndex = 5;
            // 
            // panelSuccess
            // 
            this.panelSuccess.Controls.Add(this.whenSuccess);
            this.panelSuccess.Controls.Add(this.labelWhenSuccess);
            this.panelSuccess.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSuccess.Location = new System.Drawing.Point(0, 37);
            this.panelSuccess.Name = "panelSuccess";
            this.panelSuccess.Size = new System.Drawing.Size(375, 18);
            this.panelSuccess.TabIndex = 6;
            // 
            // whenSuccess
            // 
            this.whenSuccess.AutoSize = true;
            this.whenSuccess.Location = new System.Drawing.Point(171, 3);
            this.whenSuccess.Name = "whenSuccess";
            this.whenSuccess.Size = new System.Drawing.Size(27, 13);
            this.whenSuccess.TabIndex = 29;
            this.whenSuccess.Text = "Quit";
            // 
            // labelWhenSuccess
            // 
            this.labelWhenSuccess.AutoSize = true;
            this.labelWhenSuccess.Location = new System.Drawing.Point(29, 3);
            this.labelWhenSuccess.Name = "labelWhenSuccess";
            this.labelWhenSuccess.Size = new System.Drawing.Size(146, 13);
            this.labelWhenSuccess.TabIndex = 28;
            this.labelWhenSuccess.Text = "When completed successfully:";
            // 
            // panelSkipped
            // 
            this.panelSkipped.Controls.Add(this.whenSkipped);
            this.panelSkipped.Controls.Add(this.labelWhenSkipped);
            this.panelSkipped.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSkipped.Location = new System.Drawing.Point(0, 73);
            this.panelSkipped.Name = "panelSkipped";
            this.panelSkipped.Size = new System.Drawing.Size(375, 18);
            this.panelSkipped.TabIndex = 7;
            // 
            // whenSkipped
            // 
            this.whenSkipped.AutoSize = true;
            this.whenSkipped.Location = new System.Drawing.Point(184, 3);
            this.whenSkipped.Name = "whenSkipped";
            this.whenSkipped.Size = new System.Drawing.Size(27, 13);
            this.whenSkipped.TabIndex = 30;
            this.whenSkipped.Text = "Quit";
            // 
            // labelWhenSkipped
            // 
            this.labelWhenSkipped.AutoSize = true;
            this.labelWhenSkipped.Location = new System.Drawing.Point(29, 3);
            this.labelWhenSkipped.Name = "labelWhenSkipped";
            this.labelWhenSkipped.Size = new System.Drawing.Size(158, 13);
            this.labelWhenSkipped.TabIndex = 28;
            this.labelWhenSkipped.Text = "When skipped due to condition:";
            // 
            // panelError
            // 
            this.panelError.Controls.Add(this.whenError);
            this.panelError.Controls.Add(this.labelWhenError);
            this.panelError.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelError.Location = new System.Drawing.Point(0, 55);
            this.panelError.Name = "panelError";
            this.panelError.Size = new System.Drawing.Size(375, 18);
            this.panelError.TabIndex = 8;
            // 
            // whenError
            // 
            this.whenError.AutoSize = true;
            this.whenError.Location = new System.Drawing.Point(145, 3);
            this.whenError.Name = "whenError";
            this.whenError.Size = new System.Drawing.Size(27, 13);
            this.whenError.TabIndex = 31;
            this.whenError.Text = "Quit";
            // 
            // labelWhenError
            // 
            this.labelWhenError.AutoSize = true;
            this.labelWhenError.Location = new System.Drawing.Point(29, 3);
            this.labelWhenError.Name = "labelWhenError";
            this.labelWhenError.Size = new System.Drawing.Size(119, 13);
            this.labelWhenError.TabIndex = 28;
            this.labelWhenError.Text = "When an error occurs:";
            // 
            // panelFilterName
            // 
            this.panelFilterName.Controls.Add(this.filterName);
            this.panelFilterName.Controls.Add(this.filterPicture);
            this.panelFilterName.Controls.Add(this.filterCount);
            this.panelFilterName.Location = new System.Drawing.Point(160, 2);
            this.panelFilterName.Name = "panelFilterName";
            this.panelFilterName.Size = new System.Drawing.Size(200, 16);
            this.panelFilterName.TabIndex = 9;
            // 
            // ActionTaskFlowInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelSkipped);
            this.Controls.Add(this.panelError);
            this.Controls.Add(this.panelSuccess);
            this.Controls.Add(this.panelFilter);
            this.Controls.Add(this.panelBuffer);
            this.Controls.Add(this.kryptonBorderEdge);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "ActionTaskFlowInfoControl";
            this.Size = new System.Drawing.Size(375, 224);
            this.panelBuffer.ResumeLayout(false);
            this.panelBuffer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.filterPicture)).EndInit();
            this.panelFilter.ResumeLayout(false);
            this.panelFilter.PerformLayout();
            this.panelSuccess.ResumeLayout(false);
            this.panelSuccess.PerformLayout();
            this.panelSkipped.ResumeLayout(false);
            this.panelSkipped.PerformLayout();
            this.panelError.ResumeLayout(false);
            this.panelError.PerformLayout();
            this.panelFilterName.ResumeLayout(false);
            this.panelFilterName.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        private System.Windows.Forms.Panel panelBuffer;
        private System.Windows.Forms.Label flowLink;
        private System.Windows.Forms.Label labelFilter;
        private System.Windows.Forms.Label filterCount;
        private System.Windows.Forms.Label filterName;
        private System.Windows.Forms.PictureBox filterPicture;
        private System.Windows.Forms.Panel panelFilter;
        private System.Windows.Forms.Panel panelSuccess;
        private System.Windows.Forms.Label labelWhenSuccess;
        private System.Windows.Forms.Panel panelSkipped;
        private System.Windows.Forms.Label labelWhenSkipped;
        private System.Windows.Forms.Panel panelError;
        private System.Windows.Forms.Label labelWhenError;
        private System.Windows.Forms.Label labelFlowDefault;
        private System.Windows.Forms.Label whenSuccess;
        private System.Windows.Forms.Label whenSkipped;
        private System.Windows.Forms.Label whenError;
        private System.Windows.Forms.Panel panelFilterName;
    }
}
