namespace ShipWorks.ApplicationCore
{
    partial class MainGridControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainGridControl));
            this.gridPanel = new System.Windows.Forms.Panel();
            this.gridColumnOrderNumber = new Divelements.SandGrid.GridColumn();
            this.gridColumnOrderDate = new Divelements.SandGrid.Specialized.GridDateTimeColumn();
            this.gridColumnBillFirstName = new Divelements.SandGrid.GridColumn();
            this.gridColumnBillLastName = new Divelements.SandGrid.GridColumn();
            this.gridColumnBillCompany = new Divelements.SandGrid.GridColumn();
            this.gridColumnBillCity = new Divelements.SandGrid.GridColumn();
            this.gridColumnBillState = new Divelements.SandGrid.GridColumn();
            this.gridColumnBillPostalCode = new Divelements.SandGrid.GridColumn();
            this.gridColumnBillCountry = new Divelements.SandGrid.GridColumn();
            this.gridColumnBillPhone = new Divelements.SandGrid.GridColumn();
            this.gridColumnBillEmail = new Divelements.SandGrid.GridColumn();
            this.gridColumnShipLastName = new Divelements.SandGrid.GridColumn();
            this.headerHost = new System.Windows.Forms.Integration.ElementHost();
            this.pictureSearchHourglass = new System.Windows.Forms.PictureBox();
            this.kryptonBorderEdge1 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.kryptonBorderEdge2 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.kryptonBorderEdge3 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.borderAdvanced = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.filterEditor = new ShipWorks.Filters.Controls.FilterDefinitionEditor();
            ((System.ComponentModel.ISupportInitialize) (this.pictureSearchHourglass)).BeginInit();
            this.SuspendLayout();
            //
            // gridPanel
            //
            this.gridPanel.BackColor = System.Drawing.SystemColors.Control;
            this.gridPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPanel.Location = new System.Drawing.Point(1, 62);
            this.gridPanel.Name = "gridPanel";
            this.gridPanel.Padding = new System.Windows.Forms.Padding(1, 0, 1, 1);
            this.gridPanel.Size = new System.Drawing.Size(731, 468);
            this.gridPanel.TabIndex = 0;
            //
            // gridColumnOrderNumber
            //
            this.gridColumnOrderNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            //
            // gridColumnOrderDate
            //
            this.gridColumnOrderDate.DataFormatString = "{0:d}";
            this.gridColumnOrderDate.EditorType = typeof(Divelements.SandGrid.GridDateTimeEditor);
            this.gridColumnOrderDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            //
            // gridColumnBillFirstName
            //
            this.gridColumnBillFirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            //
            // gridColumnBillLastName
            //
            this.gridColumnBillLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            //
            // gridColumnBillCompany
            //
            this.gridColumnBillCompany.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            //
            // gridColumnBillCity
            //
            this.gridColumnBillCity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            //
            // gridColumnBillState
            //
            this.gridColumnBillState.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            //
            // gridColumnBillPostalCode
            //
            this.gridColumnBillPostalCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            //
            // gridColumnBillCountry
            //
            this.gridColumnBillCountry.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            //
            // gridColumnBillPhone
            //
            this.gridColumnBillPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            //
            // gridColumnBillEmail
            //
            this.gridColumnBillEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            //
            // gridColumnShipLastName
            //
            this.gridColumnShipLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            
            //
            // HeaderHost
            //
            this.headerHost.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerHost.Location = new System.Drawing.Point(0, 0);
            this.headerHost.Name = "headerHost";
            this.headerHost.Size = new System.Drawing.Size(733, 30);
            this.headerHost.TabIndex = 1;
            //
            // pictureSearchHourglass
            //
            this.pictureSearchHourglass.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureSearchHourglass.BackColor = System.Drawing.Color.Transparent;
            this.pictureSearchHourglass.Image = ((System.Drawing.Image) (resources.GetObject("pictureSearchHourglass.Image")));
            this.pictureSearchHourglass.Location = new System.Drawing.Point(449, 5);
            this.pictureSearchHourglass.Name = "pictureSearchHourglass";
            this.pictureSearchHourglass.Size = new System.Drawing.Size(16, 16);
            this.pictureSearchHourglass.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureSearchHourglass.TabIndex = 4;
            this.pictureSearchHourglass.TabStop = false;
            //
            // kryptonBorderEdge1
            //
            this.kryptonBorderEdge1.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.HeaderPrimary;
            this.kryptonBorderEdge1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonBorderEdge1.Location = new System.Drawing.Point(0, 530);
            this.kryptonBorderEdge1.Name = "kryptonBorderEdge1";
            this.kryptonBorderEdge1.Size = new System.Drawing.Size(733, 1);
            this.kryptonBorderEdge1.TabIndex = 5;
            this.kryptonBorderEdge1.Text = "kryptonBorderEdge1";
            //
            // kryptonBorderEdge2
            //
            this.kryptonBorderEdge2.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.HeaderPrimary;
            this.kryptonBorderEdge2.Dock = System.Windows.Forms.DockStyle.Right;
            this.kryptonBorderEdge2.Location = new System.Drawing.Point(732, 30);
            this.kryptonBorderEdge2.Name = "kryptonBorderEdge2";
            this.kryptonBorderEdge2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.kryptonBorderEdge2.Size = new System.Drawing.Size(1, 500);
            this.kryptonBorderEdge2.TabIndex = 6;
            this.kryptonBorderEdge2.Text = "kryptonBorderEdge2";
            //
            // kryptonBorderEdge3
            //
            this.kryptonBorderEdge3.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.HeaderPrimary;
            this.kryptonBorderEdge3.Dock = System.Windows.Forms.DockStyle.Left;
            this.kryptonBorderEdge3.Location = new System.Drawing.Point(0, 30);
            this.kryptonBorderEdge3.Name = "kryptonBorderEdge3";
            this.kryptonBorderEdge3.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.kryptonBorderEdge3.Size = new System.Drawing.Size(1, 500);
            this.kryptonBorderEdge3.TabIndex = 7;
            this.kryptonBorderEdge3.Text = "kryptonBorderEdge3";
            //
            // borderAdvanced
            //
            this.borderAdvanced.Dock = System.Windows.Forms.DockStyle.Top;
            this.borderAdvanced.Location = new System.Drawing.Point(1, 62);
            this.borderAdvanced.Name = "borderAdvanced";
            this.borderAdvanced.Size = new System.Drawing.Size(731, 1);
            this.borderAdvanced.TabIndex = 0;
            this.borderAdvanced.Text = "kryptonBorderEdge4";
            //
            // filterEditor
            //
            this.filterEditor.BackColor = System.Drawing.Color.White;
            this.filterEditor.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterEditor.Location = new System.Drawing.Point(1, 30);
            this.filterEditor.Name = "filterEditor";
            this.filterEditor.Size = new System.Drawing.Size(731, 32);
            this.filterEditor.TabIndex = 2;
            this.filterEditor.RequiredHeightChanged += new System.EventHandler(this.OnAdvancedSearchRequiredHeightChanged);
            //
            // MainGridControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.borderAdvanced);
            this.Controls.Add(this.gridPanel);
            this.Controls.Add(this.filterEditor);
            this.Controls.Add(this.kryptonBorderEdge3);
            this.Controls.Add(this.kryptonBorderEdge2);
            this.Controls.Add(this.kryptonBorderEdge1);
            this.Controls.Add(this.headerHost);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "MainGridControl";
            this.Size = new System.Drawing.Size(733, 531);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize) (this.pictureSearchHourglass)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel gridPanel;
        private Divelements.SandGrid.GridColumn gridColumnOrderNumber;
        private Divelements.SandGrid.Specialized.GridDateTimeColumn gridColumnOrderDate;
        private Divelements.SandGrid.GridColumn gridColumnBillFirstName;
        private Divelements.SandGrid.GridColumn gridColumnBillLastName;
        private Divelements.SandGrid.GridColumn gridColumnBillCompany;
        private Divelements.SandGrid.GridColumn gridColumnBillCity;
        private Divelements.SandGrid.GridColumn gridColumnBillState;
        private Divelements.SandGrid.GridColumn gridColumnBillPostalCode;
        private Divelements.SandGrid.GridColumn gridColumnBillCountry;
        private Divelements.SandGrid.GridColumn gridColumnBillPhone;
        private Divelements.SandGrid.GridColumn gridColumnBillEmail;
        private Divelements.SandGrid.GridColumn gridColumnShipLastName;
        private System.Windows.Forms.Integration.ElementHost headerHost;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge1;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge2;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge3;
        private ShipWorks.Filters.Controls.FilterDefinitionEditor filterEditor;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge borderAdvanced;
        private System.Windows.Forms.PictureBox pictureSearchHourglass;
    }
}
